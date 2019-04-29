<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Comparitter.Web.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <h1>Comparitter</h1>
                <section>
                    <h2>Compare two words</h2>
                    <asp:TextBox ID="Word1Tb" runat="server" />
                    <asp:TextBox ID="Word2Tb" runat="server" />
                    <asp:Button ID="CompareBtn" runat="server" Text="Compare" OnClick="CompareBtn_Click" />
                    <asp:Label ID="ValidationLbl" runat="server" ForeColor="Red" Visible="false" EnableViewState="false" />
                </section>
                <section id="CompareSection" runat="server" visible="false">
                    <h2>Compare Results</h2>
                    <p>
                        <strong><asp:Label ID="PopularityLbl" runat="server" /></strong>
                    </p>
                    <p>
                        <asp:Label ID="SearchElapsedTimeLbl" runat="server" />
                    </p>
                    <div style="width: 50%">
                        <div style="float: left;">
                            <table border="1" cellspacing="0" cellpadding="5" style="border-collapse: collapse;">
                                <tr>
                                    <td>Word 1
                                    </td>
                                    <td>
                                        <asp:Label ID="Word1Lbl" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Number of Appearances
                                    </td>
                                    <td>
                                        <asp:Label ID="Word1AppearancesLbl" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Date of oldest tweet</td>
                                    <td>
                                        <asp:Label ID="Word1OldestTweetDateLbl" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td>Date of newest tweet</td>
                                    <td>
                                        <asp:Label ID="Word1NewestTweetDateLbl" runat="server" /></td>
                                </tr>
                            </table>
                        </div>
                        <div style="float: right;">
                            <table border="1" cellspacing="0" cellpadding="5" style="border-collapse: collapse;">
                                <tr>
                                    <td>Word 2
                                    </td>
                                    <td>
                                        <asp:Label ID="Word2Lbl" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Number of Appearances
                                    </td>
                                    <td>
                                        <asp:Label ID="Word2AppearancesLbl" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Date of oldest tweet</td>
                                    <td>
                                        <asp:Label ID="Word2OldestTweetDateLbl" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td>Date of newest tweet</td>
                                    <td>
                                        <asp:Label ID="Word2NewestTweetDateLbl" runat="server" /></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </section>
                <br style="clear: both;"/>
            </div>
            <hr style="margin-top: 20px;" />
            <div>
                <section>
                    <h2>History of Searches</h2>
                    <asp:GridView ID="CompareHistoryGv" runat="server" AutoGenerateColumns="false" EmptyDataText="No history yet. Do a comparison." ItemType="Comparitter.Compare.WordCompareResult" CellPadding="5">
                        <Columns>
                            <asp:TemplateField HeaderText="Comparison Date">
                                <ItemTemplate>
                                    <asp:Label ID="ComparisonDateLbl" runat="server" Text='<%# Item.CompareDateTime %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Elapsed Seconds">
                                <ItemTemplate>
                                    <asp:Label ID="SearchElapsedSecondsLbl" runat="server" Text='<%# Item.SearchElapsedSeconds %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Words are Equally Popular">
                                <ItemTemplate>
                                    <asp:Label ID="WordsAreEquallyPopular" runat="server" Text='<%# Item.WordsAreEquallyPopular %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Word 1">
                                <ItemTemplate>
                                    <asp:Label ID="Word1Lbl" runat="server" Text='<%# (Item.WordsAreEquallyPopular) ? Item.EquallyPopularResults[0].Word : Item.MostPopularWordSearchResult.Word %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Word 1 Appearances">
                                <ItemTemplate>
                                    <asp:Label ID="Word1AppearancesLbl" runat="server" Text='<%# (Item.WordsAreEquallyPopular) ? Item.EquallyPopularResults[0].AppearanceCount : Item.MostPopularWordSearchResult.AppearanceCount %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Word 2">
                                <ItemTemplate>
                                    <asp:Label ID="Word2Lbl" runat="server" Text='<%# (Item.WordsAreEquallyPopular) ? Item.EquallyPopularResults[1].Word : Item.LeastPopularWordSearchResult.Word %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Word 2 Appearances">
                                <ItemTemplate>
                                    <asp:Label ID="Word2AppearancesLbl" runat="server" Text='<%# (Item.WordsAreEquallyPopular) ? Item.EquallyPopularResults[1].AppearanceCount : Item.LeastPopularWordSearchResult.AppearanceCount %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </section>
            </div>
        </div>
    </form>
</body>
</html>
