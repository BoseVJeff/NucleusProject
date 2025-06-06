﻿<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NucleusProject._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <div class="card shadow" style="width: 18rem;" id="login-card">
          <div class="card-body">
            <h5 class="card-title">Student Login</h5>
            <div class="card-text pb-1 form-group">
                <asp:Label runat="server" ID="Incorrect" Visible="false" CssClass="text-danger" Text="Enrollment no / password incorrect"></asp:Label>
                <div>
                    <label for="<%= Username.ClientID %>" class="form-label">Enrollment No</label>
                    <!-- Attrs not known are passed as is. From: https://stackoverflow.com/a/15824040 -->
                    <asp:TextBox runat="server" AutoCompleteType="DisplayName" ID="Username" placeholder="User ID" CssClass="form-control w-100"></asp:TextBox>
                </div>
                <div>
                    <label for="<%= Password.ClientID %>" class="form-label">Password</label>
                    <asp:TextBox runat="server" AutoCompleteType="None" ID="Password" type="password" placeholder="Password" CssClass="form-control"></asp:TextBox>
                </div>
                <div>
                    <asp:CheckBox runat="server" ID="remember" CssClass="custom-control custom-checkbox" Text="Remember Me" />
                </div>
            </div>
              <asp:Button CssClass="btn btn-primary" Text="Login" runat="server" OnClick="Unnamed_Click" />
          </div>
        </div>
    </main>

</asp:Content>
