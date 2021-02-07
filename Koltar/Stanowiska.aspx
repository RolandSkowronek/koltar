<%@ Page Title="" Language="C#" UICulture="pl" Culture="pl-PL" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Stanowiska.aspx.cs" Inherits="stat2018.Stanowiska" %>

<%@ Register Assembly="DevExpress.Web.v17.1, Version=17.1.17.0,  Culture=neutral,  PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.Bootstrap.v17.1, Version=17.1.17.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Bootstrap" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        #menu {
            position: relative;
        }

            #menu.scrolling {
                position: fixed;
                top: 0;
                left: 0;
                right: 0;
            }

        </style>

    <script src="Scripts/jquery.print.js"></script>

    <script src="Scripts/rls.js"></script>
    <script>
        function printIt()
            {
                window.print();
            }
    </script>

                <div class="noprint">
        <div id="menu" style="background-color: #f7f7f7; z-index: 9999">
            <div class="manu_back" style="height: 40px; margin: 0 auto 0 auto; position: relative; width: 1050px; left: 0px;">
                <table>
                    <tr>
                        <td style="width: auto; padding-left: 5px;">
                            <asp:Label ID="Label4" runat="server" Text="Proszę wybrać zakres:"></asp:Label>
                        </td>
                        <td style="width: auto; padding-left: 5px;">

                            <dx:ASPxDateEdit ID="Date1" runat="server" Theme="Moderno">
                            </dx:ASPxDateEdit>
                        </td>
                        <td style="width: auto; padding-left: 5px;">

                            <dx:ASPxDateEdit ID="Date2" runat="server" Theme="Moderno">
                            </dx:ASPxDateEdit>
                        </td>
                        <td style="width: auto; padding-left: 5px;">
                            <asp:LinkButton ID="LinkButton54" runat="server" class="ax_box" OnClick="LinkButton54_Click">  Odśwież</asp:LinkButton>
                        </td>
                        <td style="width: auto; padding-left: 5px;" class="az">
                            <button id="Button1" class="ax_box " onclick="printIt()">Drukuj</button>
                        </td>
                        <td style="width: auto; padding-left: 5px;">&nbsp;</td>
                        <td style="width: auto; padding-left: 5px;">
                            <asp:LinkButton ID="LinkButton57" runat="server" CssClass="ax_box" OnClick="Button3_Click">Zapisz do Excel</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    <div style="width: 1150px; margin: 0 auto 0 auto; position: relative; top: 60px;">

        <div id="Div2" style="z-index: 10;">
            <div style="margin-left: auto; margin-right: auto; text-align: center; width: auto;">
                <asp:Label ID="Label3" runat="server" Text="Stanowiska" Style="font-weight: 700"></asp:Label>
                <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" DataSourceID="StanowiskaDataSet" EnableTheming="True" KeyFieldName="nazwa" OnCellEditorInitialize="ASPxGridView1_CellEditorInitialize" OnRowUpdating="ASPxGridView1_RowUpdating" Theme="Office2003Blue">
                    <SettingsEditing Mode="PopupEditForm">
                    </SettingsEditing>
                    <Settings ShowFooter="True" />
                    <SettingsResizing ColumnResizeMode="Control" />
                    <Columns>
                        <dx:GridViewCommandColumn ShowDeleteButton="True" ShowEditButton="True" ShowNewButtonInHeader="True" VisibleIndex="0">
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn FieldName="nazwa" ReadOnly="True" VisibleIndex="1">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                </dx:ASPxGridView>
                <br />
            </div>

            <asp:SqlDataSource ID="StanowiskaDataSet" runat="server" ConnectionString="<%$ ConnectionStrings:TarnokolConnectionString %>" InsertCommand="INSERT INTO Stanowiska(nazwa) VALUES (@nazwa)" SelectCommand="SELECT nazwa FROM Stanowiska" UpdateCommand="UPDATE Stanowiska SET nazwa = @nazwa WHERE (ident = @ident)">
                <InsertParameters>
                    <asp:Parameter Name="nazwa" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="nazwa" />
                    <asp:Parameter Name="ident" />
                </UpdateParameters>
            </asp:SqlDataSource>

            <br />
        </div>

        <br />
    </div>
</asp:Content>