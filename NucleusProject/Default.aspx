<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NucleusProject._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <div class="card" style="width: 18rem;">
          <div class="card-body">
            <h5 class="card-title">Login</h5>
            <div class="card-text pb-1">
                <div>
                    <label for="username" class="form-label">Username</label>
                    <input type="text" id="username" placeholder="User ID" class="form-control" />
                </div>
                <div>
                    <label for="password" class="form-label">Password</label>
                    <input type="text" id="password" placeholder="Password" class="form-control" />
                </div>
            </div>
            <a runat="server" href="~/Attendance" class="btn btn-primary">Login</a>
          </div>
        </div>
    </main>

</asp:Content>
