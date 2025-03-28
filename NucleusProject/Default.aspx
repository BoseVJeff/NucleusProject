<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NucleusProject._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <div class="card" style="width: 18rem;">
          <div class="card-body">
            <h5 class="card-title">Login</h5>
            <div class="card-text pb-1">
                <div>
                    <label for="username" class="form-label">Username</label>
                    <!-- Attrs not known are passed as is. From: https://stackoverflow.com/a/15824040 -->
                    <asp:TextBox runat="server" AutoCompleteType="DisplayName" ID="Username" placeholder="User ID" CssClass="form-control"></asp:TextBox>
                    <!--<input type="text" id="username" placeholder="User ID" class="form-control" />-->
                </div>
                <div>
                    <label for="password" class="form-label">Password</label>
                    <asp:TextBox runat="server" AutoCompleteType="None" ID="Password" type="password" placeholder="Password" CssClass="form-control"></asp:TextBox>
                    <!--<input type="text" id="password" placeholder="Password" class="form-control" />-->
                </div>
            </div>
              <asp:Button CssClass="btn btn-primary" Text="Login" runat="server" OnClick="Unnamed_Click" />
          </div>
        </div>
    </main>

</asp:Content>
