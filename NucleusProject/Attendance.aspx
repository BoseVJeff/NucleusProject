<%@ Page Title="Attendance Summary" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Attendance.aspx.cs" Inherits="NucleusProject.Attendance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script runat="server">
        protected string getCurrentRatio()
        {
            // Classes student was present in
            object present = Eval("Present");
            // Classes student was marked present/absent in
            object total = Eval("Total");
            if(present is System.DBNull || total is System.DBNull)
            {
                // Some data is unavailable. Data is unavailable.
                return "";
            } else
            {
                // Both fields are present. Proceed with calculation.
                int pre = (int)present;
                int tot = (int)total;

                if(tot==0)
                {
                    // No classes have been marked present/absent. Data cannot be calculated.
                    return "";
                }

                int ratio = (int) ((pre * 100) / tot);

                // Round to two decimal places
                return "("+ratio.ToString()+"%)";
            }
        }
        protected string getMaxRatio()
        {
            // Classes student was present in
            object present = Eval("Present");
            // Classes student was marked present/absent in
            object total = Eval("Total");
            // All classes that are scheduled to be conducted for the semester
            object allObj = Eval("All");
            if(present is System.DBNull || total is System.DBNull || allObj is System.DBNull)
            {
                // Some data is unavailable. Data is unavailable.
                return "";
            } else
            {
                // All fields are present. Proceed with calculation.
                int pre = (int)present;
                int tot = (int)total;
                int allInt = (int)allObj;

                if(tot==0||allInt==0)
                {
                    // No classes have been marked present/absent or scheduled. Data cannot be calculated.
                    return "";
                }

                int minRatio = (int) ((pre * 100) / allInt);

                int remainingClasses = allInt - tot;
                int maxPresent = remainingClasses + pre;
                int maxRatio = (int)((maxPresent * 100) / allInt);

                return minRatio.ToString()+"% - "+maxRatio.ToString()+"%";
            }
        }
    </script>
    <div class="d-flex flex-row flex-wrap justify-content-between" style="gap:1rem">
    <asp:Repeater runat="server" ID="AttendanceRepeater">
        <ItemTemplate>
            <div class="card bg-light shadow-sm" style="width: 18rem;">
            <div class="card-body">
    <h5 class="card-title">
        <asp:Label runat="server" Text='<%# Eval("Course") %>'></asp:Label>
    </h5>
    <p class="card-text">
        Course Code
    </p>
    <ul class="list-group list-group-flush">
        <li class="list-group-item bg-light">
            Present:
            <asp:Label runat="server" Text='<%# Eval("Present") %>'></asp:Label>
            of <asp:Label runat="server" Text='<%# Eval("Total") %>'></asp:Label>
            <asp:Label runat="server" Text='<%# getCurrentRatio() %>'></asp:Label>
        </li>
        <!--<li class="list-group-item bg-light">
            Classes marked in:
            <asp:Label runat="server" Text='<%# Eval("Total") %>'></asp:Label>
        </li>-->
        <li class="list-group-item bg-light">
            Classes In Semester:
            <asp:Label runat="server" Text='<%# Eval("All") %>'></asp:Label>
        </li>
        <li class="list-group-item bg-light">
            Attendance Scale:
            <asp:Label runat="server" Text='<%# getMaxRatio() %>'></asp:Label>
        </li>
      </ul>
  </div>
                </div>
        </ItemTemplate>
    </asp:Repeater>
        </div>
</asp:Content>
