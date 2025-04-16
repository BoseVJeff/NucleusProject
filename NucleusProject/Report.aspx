<%@ Page Title="Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="NucleusProject.Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="d-flex mb-3">
        <!-- Preview Button -->
        <asp:Button runat="server" ID="PreviewButton" Text="Show Report Preview" CssClass="btn btn-primary d-print-none me-3" OnClick="Unnamed_Click" />

        <!-- Print Button -->
        <button class="btn btn-primary d-print-none me-3" onclick="window.print()">Print Report</button>


    </div>

    <div class="row mt-3">
        <asp:Repeater ID="CourseRepeater" runat="server">
            <ItemTemplate>
                <div class="col-md-12 mb-4">
                    <div class="card shadow-lg">
                        <div class="card-header bg-gradient">
                            <b style="font-size: 20px;"><%# Eval("CourseTitle") %>
                                <br />
                            </b>
                            <span><%# Eval("CourseCode") %></span>
                        </div>
                        <div class="card-body bg-light">
                            <table class="table table-bordered table-sm text-center">
                                <thead>
                                    <tr>
                                        <th style="width: 25rem;">Exam</th>
                                        <th>Obtained Marks</th>
                                        <th>Total Marks</th>
                                        <th>Percentage</th>
                                        <th>Published Date</th>
                                        <th>Published By</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="ExamRepeater" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td style="text-align: justify;"><%# Eval("ExamName") %></td>
                                                <td><%# Eval("ObtainedMarks") %></td>
                                                <td><%# Eval("TotalMarks") %></td>
                                                <td><%# Eval("Percentage") %>%</td>
                                                <td><%# Eval("PublishedDate") %></td>
                                                <td><%# Eval("PublishedBy") %></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <!--<button class="btn btn-primary d-print-none" onclick="window.print()">Print Report</button>-->
    <div id="ResultPreview" runat="server" visible="false">
        <h2 class="text-center text-uppercase">Semester Grade Report / Transcript</h2>
        <hr />
        <div class="text-uppercase mb-3">
            <b>Programme - Bachelor of Computer Applications
            </b>
        </div>
        <div class="mb-3">
            <table class="table table-bordered text-center table-sm">
                <thead>
                    <tr>
                        <th>Student ID</th>
                        <th>Name</th>
                        <th>Semester</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="StudentEnr"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="StudentName"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="StudentSemester"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="text-uppercase mb-3">
            <b>Regular Semester Courses
            </b>
        </div>
        <div class="mb-3">
            <asp:GridView runat="server" ID="GV_Result" AutoGenerateColumns="false" CssClass="table table-bordered table-sm">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label runat="server" Text="Course Code" CssClass="d-block text-center"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Code") %>' CssClass="d-block text-center"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Course Title
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label runat="server" Text="Credits" CssClass="d-block text-center"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Credits") %>' CssClass="d-block text-center"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label runat="server" Text="Grade" CssClass="d-block text-center"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Grade") %>' CssClass="d-block text-center"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label runat="server" Text="Grade Points" CssClass="d-block text-center"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Points") %>' CssClass="d-block text-center"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="text-uppercase mb-3">
            <b>Semester Grade Point Average (SGPA)
            </b>
        </div>
        <div class="mb-3">
            <table class="table table-bordered table-sm">
                <thead class="text-center">
                    <tr>
                        <th>Credits Registered</th>
                        <th>Credits Earned</th>
                        <th>Grade Points Earned</th>
                        <th>SGPA</th>
                    </tr>
                </thead>
                <tbody class="text-center">
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="RegCredits" Text="Registered Credits"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="Credits" Text="Credits"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="Points" Text="Grade Points"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="Sgpa" Text="SGPA"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="text-uppercase mb-3">
            <b>Cumulative Grade Point Average (CGPA)
            </b>
        </div>
        <div class="mb-3">
            <table class="table table-bordered table-sm">
                <thead class="text-center">
                    <tr>
                        <th>Credits Registered</th>
                        <th>Credits Earned</th>
                        <th>Grade Points Earned</th>
                        <th>SGPA</th>
                    </tr>
                </thead>
                <tbody class="text-center">
                    <tr>
                        <td>67</td>
                        <td>67</td>
                        <td>634</td>
                        <td>9.46</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="mb-3" id="signatures">
            <table class="table table-borderless table-sm">
                <tbody class="text-center">
                    <tr>
                        <th class="py-5"></th>
                        <th class="py-5"></th>
                    </tr>
                    <tr>
                        <td class="text-start text-uppercase">Date: 30/03/2025</td>
                        <td class="text-end text-uppercase">Asst. Registrar (Examination)</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="text-uppercase mb-3">
            <b>Grading System
            </b>
        </div>
        <div class="mb-3">
            <asp:GridView ID="GradeExplanation" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-sm text-center">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Letter Grade
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Corresponding Points
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Points") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Explanation
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Explanation")??"-" %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="mb-3">
            <table class="table table-borderless table-sm">
                <tbody>
                    <tr>
                        <th scope="row">Credits
                        </th>
                        <td>Indicates the Load
                        </td>
                    </tr>
                    <tr>
                        <th scope="row">Grade Points
                        </th>
                        <td>Product of Credits and points of a letter grade
                        </td>
                    </tr>
                    <tr>
                        <th scope="row">SGPA
                        </th>
                        <td>Weighted average of the grade points obtained in the courses registered in a semester
                        </td>
                    </tr>
                    <tr>
                        <th scope="row">CGPA
                        </th>
                        <td>Weighted average of the grade points obtained in all the courses registered after entering the program
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
