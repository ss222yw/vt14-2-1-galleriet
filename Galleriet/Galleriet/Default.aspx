<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Galleriet.Default" ViewStateMode="Disabled" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Galleriet</title>
    <link href="~/Content/Color.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <h1>Galleriet</h1>
        <div class="clear"></div>
        <asp:Panel ID="ClosePanel" runat="server" Visible="false">
            <asp:Label ID="Label1" runat="server"></asp:Label>
            <asp:ImageButton ID="closeImg" runat="server" ImageUrl="Content/delete1.png" CausesValidation="false" OnClick="closeImg_Click" />
        </asp:Panel>
        <div class="clear"></div>
        <asp:Image ID="BiggImage" runat="server" Visible="false" />
        <asp:Panel ID="Panel1" runat="server">
            <asp:Repeater ID="Repeater1" runat="server" ItemType="Galleriet.Model.ThumImgUrl" SelectMethod="Repeater1_GetData" OnItemCommand="Repeater1_ItemCommand">

                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#Item.FileUrl %>'>
                        <asp:Image ID="ImageOne" runat="server" ImageUrl='<%#Item.ImgUrl %>' />
                    </asp:HyperLink>
                </ItemTemplate>
            </asp:Repeater>
        </asp:Panel>

        <asp:Panel ID="Panel2" runat="server" GroupingText="ladda upp bild.">
            <div>
                <asp:FileUpload ID="TheFileUpload" runat="server" />
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="<img src=Content/wrong.png /> Fel inträffade! Korrigera felet och försök igen." />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Det måste finna ett filnamn." ControlToValidate="TheFileUpload" Display="None"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="<img src=Content/wrong.png /> Giltiga filändelser är gif, jpg och png." ControlToValidate="TheFileUpload" ValidationExpression="^.*\.(gif|GIF|jpg|JPG|png|PNG)$" Display="None"></asp:RegularExpressionValidator>
            </div>


            <div>
                <asp:Button ID="Button1" runat="server" Text="Ladda upp" OnClick="Button1_Click" />
            </div>
        </asp:Panel>
    </form>

</body>
</html>
