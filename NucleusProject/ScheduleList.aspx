<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ScheduleList.aspx.cs" Inherits="NucleusProject.ScheduleList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="GV_Schedule" runat="server" AutoGenerateColumns="false" CssClass="table table-hover">
        <Columns>
            <asp:TemplateField>
                <HeaderTemplate>
                    Status
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("Status") %>'></asp:Label>
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
