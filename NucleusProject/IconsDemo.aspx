<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IconsDemo.aspx.cs" Inherits="NucleusProject.IconsDemo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
      <i class="bi bi-circle-fill text-success"></i>
      Student Present
    </div>
    <div>
      <i class="bi bi-circle-fill text-danger"></i>
      Student Absent
    </div>
    <hr />
    <div>
      <i class="bi bi-check-circle-fill text-success"></i>
      Class Completed
    </div>
    <div>
      <i class="bi bi-x-circle-fill text-danger"></i>
      Class Cancelled
    </div>
    <div>
      <i class="bi bi-clock-fill text-warning"></i>
      Class Ongoing
    </div>
    <div>
      <i class="bi bi-question-circle-fill text-secondary"></i>
      Class Status Unknown
    </div>
    <div>
      <i class="bi bi-calendar-event-fill text-secondary"></i>
      Class Scheduled
    </div>
</asp:Content>
