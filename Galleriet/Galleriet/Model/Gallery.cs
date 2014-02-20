using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Galleriet.Model
{
    public class Gallery
    {
        //privat Fältet statiskt ”readonly” ApprovedExenstions fält,av typen Regex,används för att med hjälp av ett reguljärt uttryck undersöka om en fil har en tillåten filändelse
        private static readonly Regex ApprovedExtensions;


        // privat statisk sträng som innehåller den fysiska sökvägen till katalogen där uppladdade filer sparas.
        private static string PhysicalUploadImagePath;

        //privat statiskt ”readonly” fält,av typen Regex och används för att med hjälp av ett reguljärt uttryck se till att filnamn innehåller godkända tecken.
        private static readonly Regex SantizePath;

        //statik konsruktor, initiera de statiska ”readonly” fälten.
        static Gallery()
        {
            ApprovedExtensions = new Regex("(.gif|.GIF|.jpg|.JPG|.png|.PNG)", RegexOptions.IgnoreCase);

            PhysicalUploadImagePath = Path.Combine(
                AppDomain.CurrentDomain.GetData("APPBASE").ToString(),
                "Pics"
                );

            
            var invalidChars = new string(Path.GetInvalidFileNameChars());
            SantizePath = new Regex(string.Format("[{0}]", Regex.Escape(invalidChars)));
        }

        // Metoden GetImagesNames returnerar en referens av typen IEnumerable<string> till ett List-objekt innehållande bildernas filnamn sorterade i bokstavsordning.
        public IEnumerable<Galleriet.Model.ThumImgUrl> GetImageNames()
        {
            var di = new DirectoryInfo(Path.Combine(PhysicalUploadImagePath , "ThumbNails"));
            return (from fi in di.GetFiles()
                    select new ThumImgUrl
                    {
                        Name = fi.Name,
                        FileUrl = Path.Combine("?Name=",fi.Name),
                        ImgUrl = Path.Combine("Pics/ThumbNails/",fi.Name)
                    }).AsEnumerable();
        }

        //statisk metod som returnerar true om en bild med angivet namn finns katalogen för uppladdade bilder; annars false.
        public static bool ImageExists(string name)
        {
            if (File.Exists(Path.Combine(PhysicalUploadImagePath, name)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Metoden IsValidImage returnerar true om den uppladdade filens innehåll verkligen är av typen gif, jpeg eller png.
        public static bool IsValidImage(Image image)
        {
            if (image.RawFormat.Guid == ImageFormat.Gif.Guid || image.RawFormat.Guid == ImageFormat.Jpeg.Guid || image.RawFormat.Guid == ImageFormat.Png.Guid)
            {
                return true;
            }
            return false;
        }


        //Metoden SaveImage verifierar att filen är av rätt MIME-typ annars kastas ett undantag, sparar bilden samt skapar och sparar en tumnagelbild.
        public static string SaveImage(Stream stream, string fileName)
        {
            var image = System.Drawing.Image.FromStream(stream);

            fileName = SantizePath.Replace(fileName, string.Empty);

            if (!IsValidImage(image))
            {
                throw new ArgumentException("Fel! Filen har fel MIME-typ");
            }


            // Skapar räknare .
            int counter = 1;

            // Om bilden som vill användaren spara har samma namn som en bild som sparat i filen redan, kör koden inuti if-satsen.
            if (ImageExists(fileName))
            {

                
                // Hämtar namn från filen.
                string directory = Path.GetDirectoryName(fileName);
                // Hämtar bara namn på filen eller vilden i detta fall.
                string nameOnly = Path.GetFileNameWithoutExtension(fileName);
                // Hämtar bara formatet av bilden bara om det är png eller ....
                string ex = Path.GetExtension(fileName);

                // Medan bilden är exists, kör koden inuti while loopen.
                while (ImageExists(fileName))
                {
                    fileName = string.Format("{0}{1}{2}", nameOnly, counter, ex);
                    counter++;
                }
                
            }

            // Bestämmer storleken på tumnail bilder.
            var thumbnail = image.GetThumbnailImage(60, 45, null, System.IntPtr.Zero);

            try
            {
                // Sparar bilder.
                image.Save(Path.Combine(PhysicalUploadImagePath, fileName));
                // Sparar tumnbbilder.
                thumbnail.Save(PhysicalUploadImagePath + "/ThumbNails/" + fileName);


            }
            catch
            {
                throw new ArgumentException("Det gick inte att spara bilden!");
            }
           
            // Retunerar filename.
            return fileName;
        }

    }
}