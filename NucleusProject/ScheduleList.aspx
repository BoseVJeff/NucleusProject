<%@ Page Title="Schedule" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ScheduleList.aspx.cs" Inherits="NucleusProject.ScheduleList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script runat="server">
        // Generate icon for attendance
        protected string getAttendanceClass()
        {
            object att = Eval("Attendance");
            object startStr = Eval("Start");
            object endStr = Eval("End");
            //string attStr;
            if(att is System.DBNull)
            {
                return "<i class=\"bi bi-circle-fill text-warning\"></i>";
            } else
            {
                string attStr = (string)att;
                //return attStr;
                StringBuilder sb = new StringBuilder();
                sb.Append("<i class=\"bi bi-circle-fill text-");
                // TODO: Make this comparision on the basis of values from E_Attendance
                if(attStr=="Present")
                {
                    sb.Append("success");
                } else
                {
                    if (!(startStr is System.DBNull) && !(endStr is System.DBNull))
                    {
                        // Start and end dates ate available for use
                        DateTimeOffset currentTimestamp = DateTimeOffset.UtcNow;
                        DateTimeOffset start = DateTimeOffset.FromUnixTimeSeconds((int)startStr);
                        DateTimeOffset end = DateTimeOffset.FromUnixTimeSeconds((int)endStr);

                        if (start <= currentTimestamp && currentTimestamp <= end)
                        {
                            // Mark class as ongoing
                            sb.Append("info");
                        }
                        else
                        {
                            // Mark for highlight
                            sb.Append("danger");
                        }
                    }
                    else
                    {
                        // Highlighting danger here as it the student is supposed to case about these scenarios
                        sb.Append("danger");
                    }
                }
                sb.Append("\"></i>");
                return sb.ToString();
            }
        }
    </script>
    <script type="text/javascript">
        function toogleFilterCard() {
            var filterCard = document.getElementById("filterCard");
            if (filterCard.style.display === "none" || filterCard.style.display === "") {
                filterCard.style.display = "block";
            }
            else {
                filterCard.style.display = "none";
            }
        }
        function getTimeZone() {
            let el = document.getElementById("<%= tzData.ClientID %>").value = (new Date(Date.now())).getTimezoneOffset();
        }
    </script>
    <div class="btn-group mb-1">
        <asp:Button runat="server" Text="Month" CssClass="btn btn-primary" OnClick="Unnamed_Click" ID="MonthBtn" />
        <asp:Button runat="server" Text="Week" CssClass="btn btn-primary" OnClick="Unnamed_Click1" ID="WeekBtn" />
        <asp:Button runat="server" Text="Day" CssClass="btn btn-primary" OnClick="Unnamed_Click2" ID="DayBtn" />
        
    </div>

    <div class="btn-group mb-1">
        <button type="button" class="btn btn-secondary" onclick="toogleFilterCard()">
            <i runat="server" class="bi bi-funnel" id="filtericon"></i>
        </button>
    </div>

    <div class="text-center mb-4 align-text-top">
         
        <button runat="server" onserverClick="PreviousBtn_Click" class="btn btn-secondary" id="PreviousBtn" disabled="disabled">
            <i class="bi bi-arrow-left"></i>
        </button>
        <asp:Label runat="server" CssClass="font-weight-bold" ID="DateRangeLabel" Text="Current Range"></asp:Label>
        <button runat="server" onserverClick="NextBtn_Click" id="NextBtn" class="btn btn-secondary" disabled="disabled">
            <i class="bi bi-arrow-right"></i>
        </button>
        
    </div>

    <div id="filterCard" class="card" style="display: none">
        <div class="card-body">
            <div class="d-flex">
                <div class="form-group col-md-4">
                    <label for="fromDate">From</label>
                    <asp:TextBox ID="fromDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
                <div class="form-group col-md-4">
                    <label for="toDate">To</label>
                    <asp:TextBox ID="toDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
                <div class="form-group col-md-4 align-self-end">
                    <asp:Button runat="server" Text="Submit" CssClass="btn btn-primary" OnClientClick="getTimeZone()" OnClick="FilterBtn_Click" ID="Button1" />
                </div>
                <asp:HiddenField runat="server" Value="0" ID="tzData" />
            </div>
        </div>
    </div><br />
    
   <!-- <asp:Label runat="server" CssClass="text-center font-weight-bold" Text="No schedule available"></asp:Label>
    <div class="text-center d-none">
        <b>No schedule available!</b>
    </div>-->

    <asp:Label ID="NoDataLabel" runat="server" Text="No Data Available" Visible="false"></asp:Label>
    
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
                    <asp:Label runat="server" Text='<%# DateTimeOffset.FromUnixTimeSeconds((int)Eval("Start")).ToOffset(new TimeSpan(5, 30, 0)).DayOfWeek.ToString() %>'></asp:Label>
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
