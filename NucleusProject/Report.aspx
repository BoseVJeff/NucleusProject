<%@ Page Title="Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="NucleusProject.Report" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Button runat="server" Text="Download Report" CssClass="btn btn-primary d-print-none" OnClick="Unnamed_Click" />
    <h2 class="text-center text-uppercase">Semester Grade Report / Transcript</h2>
    <hr />
    <div class="text-uppercase mb-3">
        <b>
            Programme - Bachelor of Computer Applications
        </b>
    </div>
    <div class="mb-3">
        <table class="table table-bordered text-center">
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
        <b>
            Regular Semester Courses
        </b>
    </div>
    <div class="mb-3">
        <table class="table table-bordered">
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
            <b>
                Semester Grade Point Average (SGPA)
            </b>
        </div>
        <div class="mb-3">
            <table class="table table-bordered">
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
            <b>
                Cumulative Grade Point Average (CGPA)
            </b>
        </div>
        <div class="mb-3">
            <table class="table table-bordered">
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
        <div class="mb-3">
            <table class="table table-borderless">
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
</asp:Content>
