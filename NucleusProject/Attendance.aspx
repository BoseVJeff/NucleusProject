<%@ Page Title="Attendance Summary" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Attendance.aspx.cs" Inherits="NucleusProject.Attendance" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
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
    <div class="input-group mb-3">
        <div class="input-group-prepend">
            <span class="input-group-text" style="border-bottom-right-radius: 0px;border-top-right-radius: 0px;">Semester</span>
        </div>
        <asp:DropDownList runat="server" CssClass="form-select" ID="SemesterSelect" AutoPostBack="true">
            <asp:ListItem Text="Select Semester" Value="" />
            <asp:ListItem Text="Semester 1" Value="1" />
            <asp:ListItem Text="Semester 2" Value="2" />
            <asp:ListItem Text="Semester 3" Value="3" />
            <asp:ListItem Text="Semester 4" Value="4" />
            <asp:ListItem Text="Semester 5" Value="5" />
        </asp:DropDownList>
    </div>
    <div class="d-flex flex-row flex-wrap justify-content-start" style="gap:1rem">
        <asp:Repeater runat="server" ID="AttendanceRepeater">
            <ItemTemplate>
                <div class="card bg-light shadow-sm" style="width: 26rem;">
                    <div class="card-body position-relative">
                        <h5 class="card-title">
                            <asp:HyperLink runat="server" CssClass="text-decoration-none" Text='<%# Eval("Course") %>' href='<%# "/ScheduleList?course="+System.Net.WebUtility.UrlEncode((string)Eval("Course")) %>'></asp:HyperLink>
                            
                            <br />
                            <asp:Label runat="server" ID="CourseCode" Text='<%# Eval("Code") %>' CssClass="h6"></asp:Label>
                        </h5>
                        <p class="card-text">
                            <asp:Label runat="server" Text='<%# Eval("School") %>'></asp:Label>
                        </p>
                        <div class="container">
                            <div class="row justify-content-end">
                                <div class="col-7 align-items-center">
                                    <ul class="list-group list-group-flush">
                                        <li class="list-group-item bg-light">
                                            Present:
                                            <asp:Label runat="server" Text='<%# Eval("Present") %>'></asp:Label>
                                            of <asp:Label runat="server" Text='<%# Eval("Total") %>'></asp:Label>
                                            <!--<asp:Label runat="server" Text='<%# getCurrentRatio() %>'></asp:Label>-->
                                        </li>
                                        <li class="list-group-item bg-light">
                                            Total Sessions:
                                            <asp:Label runat="server" Text='<%# Eval("All") %>'></asp:Label>
                                        </li>
                                        <li class="list-group-item bg-light">
                                            <!--Attendance Scale:
                                            <asp:Label runat="server" Text='<%# getMaxRatio() %>'></asp:Label>-->
                                        </li>
                                    </ul>
                                </div>
                                <div class="col-5 d-flex flex-nowrap justify-content-end">
                                    <asp:Chart ID="AttendanceChart" runat="server" Width="140" Height="140">
                                        
                                        <Series>
                                            <asp:Series Name="Series1" ChartType="Doughnut" IsValueShownAsLabel="false" Label=" "></asp:Series>
                                        </Series>
                                        
                                        <ChartAreas>
                                            <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
                                        </ChartAreas>
                                    </asp:Chart>
                                </div>
                            </div>
                            <div class="row bg-light">
                                Attendance Scale 
                                <div class="position-relative">
                                    <div class="progress p-0">
                                        <!-- All classes set here WILL be overridden. Change `baseProgressClass` in the CodeBehind instead. -->
                                        <div id="barone" runat="server" title="If you skip all classes"></div>
                                        <div id="bartwo" runat="server"></div>
                                        <div id="barthree" runat="server" title="current attendance"></div>
                                        <div id="barfour" runat="server"></div>
                                        <div id="barfive" runat="server" title="If you attend all classes"></div>
                                    </div>
                                    <div style="left:0;right:0;margin-inline: calc(var(--bs-gutter-x) * 0.5); pointer-events: none;" class="progress p-0 position-absolute top-0 left-0 right-0 bg-transparent">
                                        <div style="width: 49.5%" class="progress-bar bg-transparent"></div>
                                        <div style="width:1%" class="progress-bar bg-dark opacity-50 rounded"></div>
                                        <div style="width:9%" class="progress-bar bg-transparent"></div>
                                        <div style="width:1%" class="progress-bar bg-dark opacity-50 rounded"></div>
                                        <div style="width:9%" class="progress-bar bg-transparent"></div>
                                        <div style="width:1%" class="progress-bar bg-dark opacity-50 rounded"></div>
                                        <div style="width:9%" class="progress-bar bg-transparent"></div>
                                        <div style="width:1%" class="progress-bar bg-dark opacity-50 rounded"></div>
                                        <div style="width:19.5%" class="progress-bar bg-transparent"></div>
                                        <!--<div style="width:1%" class="progress-bar bg-dark opacity-50 rounded"></div>
                                        <div style="width:9.5%" class="progress-bar bg-transparent"></div>-->
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <div class="container-fluid border rounded border-2 bg-light" >
            <div class="legend-item d-flex align-items-center">
                <span class="legend-color bg-success rounded" style="width: 2rem; height: 1rem; display: inline-block; margin-right: 5px;"></span>
                <span>Estimated Attendance if no further sessions are attended</span>
            </div>
            <div class="legend-item d-flex align-items-center">
                <span class="legend-color bg-dark rounded" style="width: 2rem; height: 1rem; display: inline-block; margin-right: 5px;"></span>
                <span>Current Attendance</span>
            </div>
            <div class="legend-item d-flex align-items-center">
                <span class="legend-color bg-danger rounded" style="width: 2rem; height: 1rem; display: inline-block; margin-right: 5px;"></span>
                <span>Estimated Attendance if all further sessioins are attended</span>
            </div>
        </div>
    </div>
</asp:Content>
