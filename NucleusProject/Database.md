Mst_Student:
```sql
CREATE TABLE [dbo].[Mst_Student] (
    [Id]     INT          IDENTITY (1, 1) NOT NULL,
    [Enr_no] NCHAR (10)   NOT NULL,
    [Name]   VARCHAR (50) NOT NULL,
    [Ad_Year] INT NOT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
```
Data:

|Enr_no  |Name         |Ad_Year|
|--------|-------------|-------|
|23000041|Mohit Kumhar |2023   |
|23000056|Arpan Parekh |2023   |
|23000068|Vineet Maurya|2023   |

Mst_Course
```sql
CREATE TABLE [dbo].[Mst_Course]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Code] VARCHAR(6) NOT NULL, 
    [Name] VARCHAR(50) NOT NULL, 
    [Semester] INT NOT NULL 
)
```
Data:

|Code  |Name                               |Semester|
|------|-----------------------------------|--------|
|CMP405|.NET Technologies                  |2       |
|CMP402|Software Testing Basics            |2       |
|CMP403|Object Oriented Analysis and Design|2       |
|MGT403|Entrepreneurship                   |2       |
|CMP404|Introduction to Data Science       |2       |

Mst_Programme
```sql
CREATE TABLE [dbo].[Mst_Programme]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL, 
    [Short_Name] VARCHAR(50) NULL
)
```
Data:

|Name                                      |Short Name|
|------------------------------------------|----------|
|Bachelor of Computer Application          |BCA       |
|B.Tech in Computer Science and Engineering|B.Tech CSE|
|B.Sc. in Data Science                     |B.Sc. DS  |

Mst_Class
```sql
CREATE TABLE [dbo].[Mst_Class]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(8) NOT NULL
)
```
Data:

|Class |
|------|
|A-202A|
|A-202B|
|A-203A|
|A-203B|
|A-602 |
|A-701 |

Mst_School
```sql
CREATE TABLE [dbo].[Mst_School]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL, 
    [Short_Name] VARCHAR(10) NULL
)
```

Data:

|Name                                |Short Name|
|------------------------------------|----------|
|School of Engineering and Technology|SET       |
|School of Science                   |SoS       |
|School of Business and Law          |SBL       |

Mst_Year
```sql
CREATE TABLE [dbo].[Mst_Year]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL
)
```

Mst_Semester
```sql
CREATE TABLE [dbo].[Mst_Semester]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL, 
    [Year] INT NOT NULL,
	CONSTRAINT FK_Semester_Year FOREIGN KEY (Year) REFERENCES Mst_Year(Id),
)
```
Data:

|Name      |Year|
|----------|----|
|2024-25 I |1   |
|2024-25 II|1   |
|2025-26 I |2   |
|2025-26 II|2   |

Mst_Faculty
```sql
CREATE TABLE [dbo].[Mst_Faculty]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL, 
    [Phone] VARCHAR(50) NOT NULL, 
    [Email] VARCHAR(50) NOT NULL
)
```
Data:

|Name                      |Phone     |Email                    |
|--------------------------|----------|-------------------------|
|Chirag Darji              |0123456789|chirag.darji@nuv.ac.in   |
|Rupali Shinde             |0123456789|rupali.shinde@nuv.ac.in  |
|Vedant Parmar             |0123456789|vedant.j.parmar@nuv.ac.in|
|Humayd Sheikh             |0123456789|humayd.shaikh@nuv.ac.in  |
|Megha Patel               |0123456789|megha.patel@nuv.ac.in    |
|Parth Kinjal Shah         |0123456789|parth.shah@nuv.ac.in     |
|Vivek Bhatt               |0123456789|vivek.bhatt@nuv.ac.in    |
|Swapnila Nigam            |0123456789|swapnila.nigam@nuv.ac.in |
|Sheetal Shende            |0123456789|sheetals@nuv.ac.in       |
|Umme Salma Mahebub Pirzada|0123456789|salmap@nuv.ac.in         |
|Aymaan Garasia            |0123456789|aymaan.garasia@nuv.ac.in |
|Shardav Bhatt             |0123456789|shardavb@nuv.ac.in       |

E_Days
```sql
CREATE TABLE [dbo].[E_Days]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL
)
```
Data:

|Name     |
|---------|
|Sunday   |
|Monday   |
|Tuesday  |
|Wednesday|
|Thursday |
|Friday   |
|Saturday |

E_Attendance
```sql
CREATE TABLE [dbo].[E_Attendance]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL
)
```
Data:

|Name   |
|-------|
|Present|
|Absent |

E_Class_Status
```sql
CREATE TABLE [dbo].[E_Class_Status]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL
)
```
Data:

|Name     |
|---------|
|Scheduled|
|Ongoing  |
|Completed|
|Cancelled|

E_Grade
```sql
CREATE TABLE [dbo].[E_Grade]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL, 
    [Short_Name] VARCHAR(50) NULL
)
```
Data:

|Name      |Short Name|
|----------|----------|
|A         |          |
|B         |          |
|C         |          |
|D         |          |
|E         |          |
|F         |          |
|Incomplete|I         |

Trn_Schedule
```sql
CREATE TABLE [dbo].[Trn_Schedule]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Course] INT NOT NULL, 
    [Student] INT NOT NULL, 
    [Class] INT NOT NULL, 
    [Day] INT NOT NULL, 
    [Status] INT NOT NULL, 
    [Start] INT NOT NULL, 
    [End] INT NOT NULL
)
```
Data:
