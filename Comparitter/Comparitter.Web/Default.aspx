<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Comparitter.Web.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Comparitter</h1>
            <section>
                <h2>Compare two words</h2>
                <asp:TextBox ID="Word1Tb" runat="server" />
                <asp:TextBox ID="Word2Tb" runat="server" />
                <asp:Button ID="CopmareBtn" runat="server" Text="Compare" OnClick="CopmareBtn_Click" />
            </section>
            <section>
                <h2>Compare Results</h2>
                <asp:Label ID="ResultsLbl" runat="server" />
            </section>
        </div>
    </form>
</body>
</html>
