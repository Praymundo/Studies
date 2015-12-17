<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="EstudosAspNet.Upload" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:FileUpload ID="compUpload" runat="server" AllowMultiple="true"/>
    <br /><br />
    <asp:Button ID="btnEnviar" runat="server" Text="Enviar" 
    onclick="btnEnviar_Click" />
</asp:Content>
