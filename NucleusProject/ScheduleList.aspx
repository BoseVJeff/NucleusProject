<%@ Page Title="Schedule" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ScheduleList.aspx.cs" Inherits="NucleusProject.ScheduleList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script runat="server">
        // Generate icon for attendance
        protected string getAttendanceClass()
        {
            object att = Eval("Attendance");
            //string attStr;
            if(att is System.DBNull)
            {
                return "";
            } else
            {
                string attStr = (string)att;
                StringBuilder sb = new StringBuilder();
                sb.Append("<i class=\"bi bi-circle-fill text-");
                // TODO: Make this comparision on the basis of values from E_Attendance
                if(attStr=="Present")
                {
                    sb.Append("success");
                } else
                {
                    // Highlighting danger here as it the student is supposed to case about these scenarios
                    sb.Append("danger");
                }
                sb.Append("\"></i>");
                //sb.Append(attStr);
                return sb.ToString();
            }
            //return attStr;
        }
        protected string getClassStatusClass()
        {
            object status = Eval("Status");
            if(status is System.DBNull)
            {
                return "N/A";
            } else
            {
                string statusStr = (string)status;
                switch (statusStr)
                {
                    case "Scheduled":
                        return "<i class=\"bi bi-calendar-event-fill text-secondary\"></i>";
                    case "Ongoing":
                        return "<i class=\"bi bi-clock-fill text-warning\"></i>";
                    case "Completed":
                        return "<i class=\"bi bi-check-circle-fill text-success\"></i>";
                    case "Cancelled":
                        return "<i class=\"bi bi-x-circle-fill text-danger\"></i>";
                    default:
                        // Unknown Status
                        return "<i class=\"bi bi-question-circle-fill text-secondary\"></i>";
                }
            }
        }
    </script>
    <div class="btn-group mb-1">
        <asp:Button runat="server" Text="Month" CssClass="btn btn-primary" OnClick="Unnamed_Click" ID="MonthBtn" />
        <asp:Button runat="server" Text="Week" CssClass="btn btn-primary" OnClick="Unnamed_Click1" ID="WeekBtn" />
        <asp:Button runat="server" Text="Day" CssClass="btn btn-primary" OnClick="Unnamed_Click2" ID="DayBtn" />
    </div>
    <asp:Label runat="server" CssClass="text-center font-weight-bold" Text="No schedule available"></asp:Label>
    <div class="text-center d-none">
        <b>No schedule available!</b>
    </div>
    <asp:GridView ID="GV_Schedule" runat="server" AutoGenerateColumns="false" CssClass="table table-hover ">
        <Columns>
            <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                <HeaderTemplate>
                    <span class="text-center">Att</span>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# getAttendanceClass() %>' CssClass="text-center"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                <HeaderTemplate>
                    Status
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# getClassStatusClass() %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    Course
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("Course") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    Faculty
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Panel ID="HoverPanel" runat="server" CssClass="card">
                        <div class="card-body">
                            <div class="card-title">
                                <asp:Label runat="server" Text='<%# Eval("Faculty") %>'></asp:Label>
                            </div>
                            <div class="card-text">
                                Phone: <asp:HyperLink runat="server" NavigateUrl='<%#"tel:"+(string)Eval("Phone") %>'><asp:Label runat="server" Text='<%# Eval("Phone") %>'></asp:Label></asp:HyperLink>
                                <br />
                                Email: <asp:HyperLink runat="server" NavigateUrl='<%#"mailto:"+(string)Eval("Email") %>'><asp:Label runat="server" Text='<%# Eval("Email") %>'></asp:Label></asp:HyperLink>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Label runat="server" Text="<i class='bi bi-info-circle'></i>" ID="HoverIcon"></asp:Label>
                    <asp:Label runat="server" Text='<%# Eval("Faculty") %>'></asp:Label>
                    <ajaxToolkit:HoverMenuExtender ID="hme" runat="server" TargetControlID="HoverIcon" PopupControlID="HoverPanel" PopupPosition="Right" PopDelay="50" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    Class
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("Class") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    Day
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("Day") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    Date
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# DateTimeOffset.FromUnixTimeSeconds((int)Eval("Start")).ToOffset(new TimeSpan(5, 30, 0)).Date.ToShortDateString() %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    Start
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# DateTimeOffset.FromUnixTimeSeconds((int)Eval("Start")).ToOffset(new TimeSpan(5,30,0)).TimeOfDay %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    End
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# DateTimeOffset.FromUnixTimeSeconds((int)Eval("End")).ToOffset(new TimeSpan(5,30,0)).TimeOfDay %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
