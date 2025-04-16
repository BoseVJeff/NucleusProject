<%@ Page Title="Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="NucleusProject.Report" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="d-flex mb-3">
        <!-- Preview Button -->
        <asp:Button runat="server" ID="PreviewButton" Text="Show Report Preview" CssClass="btn btn-primary d-print-none me-3" OnClick="Unnamed_Click" />

        <!-- Print Button -->
        <asp:Button runat="server" ID="PrintButton" Text="Print Report" CssClass="btn btn-primary d-print-none me-3" OnClientClick="printPreview();" />
    </div>

    <div class="row mt-3">
        <asp:Repeater ID="CourseRepeater" runat="server">
            <ItemTemplate>
                <div class="col-md-12 mb-4" >
                    <div class="card shadow-lg">
                        <div class="card-header bg-gradient">
                            <b style="font-size:20px;"><%# Eval("CourseTitle") %> <br /></b>
                                <span><%# Eval("CourseCode") %></span>
                        </div>
                        <div class="card-body bg-light">
                            <table class="table table-bordered table-sm text-center">
                                <thead>
                                    <tr>
                                        <th style="width:25rem;">Exam</th>
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
                                                <td style="text-align:justify;"><%# Eval("ExamName") %></td>
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

    <div id="ResultPreview" runat="server" visible="false">
        <h2 class="text-center text-uppercase mt-5">Semester Grade Report / Transcript</h2>
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
                        <td>23000068</td>
                        <td>Vineet Maurya</td>
                        <td>Autumn (July to Dec 2024)</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="text-uppercase mb-3">
            <b>Regular Semester Courses
            </b>
        </div>
        <div class="mb-3">
            <table class="table table-bordered table-sm">
                <thead class="text-center">
                    <tr>
                        <th>Course Code</th>
                        <th>Course Title</th>
                        <th>Credits</th>
                        <th>Grade</th>
                        <th>Grade Points</th>
                    </tr>
                </thead>
                <tbody class="text-start">
                    <tr>
                        <td>MAT002</td>
                        <td>Fuzzy Logic in Artificial Intelligence</td>
                        <td>2</td>
                        <td>A</td>
                        <td>18</td>
                    </tr>
                    <tr>
                        <td>PRO002</td>
                        <td>Scientific Writing and Softwares</td>
                        <td>3</td>
                        <td>A+</td>
                        <td>30</td>
                    </tr>
                    <tr>
                        <td>CA210</td>
                        <td>Software Engineering</td>
                        <td>3</td>
                        <td>A</td>
                        <td>27</td>
                    </tr>
                    <tr>
                        <td>CMP304</td>
                        <td>Introduction to Web Designing and PHP</td>
                        <td>5</td>
                        <td>A+</td>
                        <td>50</td>
                    </tr>
                    <tr>
                        <td>CMP305</td>
                        <td>Fundamentals of Cloud Computing</td>
                        <td>5</td>
                        <td>A+</td>
                        <td>50</td>
                    </tr>
                    <tr>
                        <td>CMP308</td>
                        <td>Operating Systems</td>
                        <td>3</td>
                        <td>A</td>
                        <td>27</td>
                    </tr>
                </tbody>
            </table>
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
                        <td>21</td>
                        <td>21</td>
                        <td>202</td>
                        <td>9.62</td>
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

    <script type="text/javascript">
        function printPreview() {

            //Trigger browser's print functionality
            window.print();
        }
    </script>
</asp:Content>
