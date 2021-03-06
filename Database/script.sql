USE [MaxiCorp]
GO
/****** Object:  StoredProcedure [dbo].[sp_generateCode]    Script Date: 03/10/2021 9:29:14 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_generateCode] (@type varchar(50), @id int)
as
begin
declare @prefix varchar(2)
declare @value int
declare @suffixes varchar(10)
declare @result varchar(20) = '000000' 
declare @suffix_temp varchar(10) = concat('00', month(getdate()))
declare @year varchar(4) = year(getdate())
set @year = SUBSTRING(@year,1,2)
select @prefix = isnull(Prefix,''), @suffixes = isnull(Suffixes,''),@value = isnull([Value],0) from AutoId where Id= @id and Name = @type
set @suffix_temp = SUBSTRING(@suffix_temp,len(@suffix_temp) - 1,2)

if(@prefix = @year)
begin
	if(@suffixes = @suffix_temp)
		set @value+=1;
	else
		begin
			set @suffixes = @suffix_temp;
			set @value = @id
		end
end
else
begin
	set @prefix = @year;
	set @suffixes = @suffix_temp;
	set @value = @id;
end
set @result = concat(@result,@value)
set @result = SUBSTRING(@result, len(@result) - 5, 6);
set @result = @prefix + @suffixes + @result

	update AutoId set Prefix=@prefix,Suffixes=@suffixes,[Value]=@value, Name = @type where ID=@id
select @result
end

GO
/****** Object:  StoredProcedure [dbo].[spGetEmployeeById]    Script Date: 03/10/2021 9:29:14 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetEmployeeById] 
(@id int)
AS
BEGIN
Select count(*) over() as TotalRecord, e.Id, e.FullName,e.code,e.Title,e.Email, e.Phone,e.LevelId ,e.CreatedAt, e.CreatedBy, e.UpdatedAt,
 e.UpdatedBy, L.Name as LevelName, T.Name as TeamName, T.Id as TeamId, e2.FullName as LeaderName, e2.Id as LeaderId from Employee e
inner join Team T on e.TeamId = T.Id left join Employee e2 on T.LeaderId = e2.Id left join Level L on e.LevelId = L.Id
where isnull(e.IsDeleted,0) = 0 and e.Id = @id
END
 
  
  

GO
/****** Object:  StoredProcedure [dbo].[spGetEmployees]    Script Date: 03/10/2021 9:29:14 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetEmployees] 
(@freeText nvarchar(50) ='',
 @page int = 1,
  @limit int= 20,
  @fromDate DATETIME,
@toDate DATETIME)
AS
BEGIN
declare @offset int = 0;
set @offset = (@page-1)*@limit;
declare @where  nvarchar(1000) = '';
declare @mainClause nvarchar(max);
declare @params nvarchar(300);
if @freetext = '' begin set @freetext = null end;
set @mainClause = 'Select count(*) over() as TotalRecord, e.Id, e.FullName,e.code,e.Title,e.Email,e.Phone,e.LevelId ,e.CreatedAt, e.CreatedBy, e.UpdatedAt,
 e.UpdatedBy, L.Name as LevelName, T.Name as TeamName, T.Id as TeamId, e2.FullName as LeaderName, e2.Id as LeaderId from Employee e
left join Team T on e.TeamId = T.Id left join Employee e2 on T.LeaderId = e2.Id left join Level L on e.LevelId = L.Id
where isnull(e.IsDeleted,0) = 0 and e.CreatedAt between @fromDate and @toDate ';
if(@freetext  is not null)
	begin
	set @where +='and (e.FullName like  N''%' + @freetext +'%'' or e.Code like  N''%' + @freetext +'%'' or e.Title like  N''%' + @freetext +'%'')' ;
end;
set @where += ' order by e.CreatedAt desc offset @offset ROWS FETCH NEXT @limit ROWS ONLY'
set @mainClause = @mainClause +  @where
set @params =N' @offset int, @limit int, @fromDate datetime, @toDate datetime';
EXECUTE sp_executesql @mainClause, @params, @offset = @offset, @limit = @limit, @fromDate = @fromDate, @toDate = @toDate
print @mainClause;
END
 
  
  

GO
/****** Object:  StoredProcedure [dbo].[spGetTeamById]    Script Date: 03/10/2021 9:29:14 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[spGetTeamById] 
(@id int)
AS
BEGIN
select T.Id, T.Name, T.CreatedAt, T.CreatedBy, T.UpdatedAt, T.UpdatedBy, e.FullName as LeaderName, e.Id as LeaderId, T2.Name as ParentName, T2.Id as ParentTeamId from Team T
inner join Employee e on T.Leaderid = e.Id left join Team T2 on T.ParentTeamId = T2.Id
where isnull(T.IsDeleted,0) = 0 and T.id = @id
END
 
  
  

GO
/****** Object:  StoredProcedure [dbo].[spGetTeamMembers]    Script Date: 03/10/2021 9:29:14 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetTeamMembers] 
(@teamId int,
 @page int = 1,
  @limit int= 20)
AS
BEGIN
declare @offset int = 0;
set @offset = (@page-1)*@limit;
declare @where  nvarchar(1000) = '';
declare @mainClause nvarchar(max);
declare @params nvarchar(300);
set @mainClause = 'Select count(*) over() as TotalRecord, e.Id, e.FullName,e.code,e.Title,e.Email, e.Phone,e.LevelId ,e.CreatedAt, e.CreatedBy, e.UpdatedAt,
 e.UpdatedBy, L.Name as LevelName, T.Name as TeamName, T.Id as TeamId, e2.FullName as LeaderName, e2.Id as LeaderId from Employee e
inner join Team T on e.TeamId = T.Id left join Employee e2 on T.LeaderId = e2.Id left join Level L on e.LevelId = L.Id
where isnull(e.IsDeleted,0) = 0 and e.TeamId = @teamId';

set @where += ' order by e.CreatedAt desc offset @offset ROWS FETCH NEXT @limit ROWS ONLY'
set @mainClause = @mainClause +  @where
set @params =N' @offset int, @limit int,@teamId int';
EXECUTE sp_executesql @mainClause, @params, @offset = @offset, @limit = @limit, @teamID = @teamId
print @mainClause;
END
 
  
  

GO
/****** Object:  StoredProcedure [dbo].[spGetTeams]    Script Date: 03/10/2021 9:29:14 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetTeams] 
(@freeText nvarchar(50) ='',
 @page int = 1,
  @limit int= 20)
AS
BEGIN
declare @offset int = 0;
set @offset = (@page-1)*@limit;
declare @where  nvarchar(1000) = '';
declare @mainClause nvarchar(max);
declare @params nvarchar(300);
if @freetext = '' begin set @freetext = null end;
set @mainClause = 'Select count(*) over() as TotalRecord, T.Id, T.Name, T.CreatedAt, T.CreatedBy, T.UpdatedAt, T.UpdatedBy, e.FullName as LeaderName, e.Id as LeaderId, T2.Name as ParentName, T2.Id as ParentTeamId from Team T
inner join Employee e on T.Leaderid = e.Id left join Team T2 on T.ParentTeamId = T2.Id
where isnull(T.IsDeleted,0) = 0 ';
if(@freetext  is not null)
	begin
	set @where +='and (T.Name like  N''%' + @freetext +'%'' or e.FullName like  N''%' + @freetext +'%'')' ;
end;
set @where += ' order by CreatedAt desc offset @offset ROWS FETCH NEXT @limit ROWS ONLY'
set @mainClause = @mainClause +  @where
set @params =N' @offset int, @limit int';
EXECUTE sp_executesql @mainClause, @params, @offset = @offset, @limit = @limit
print @mainClause;
END
 
  
  

GO
/****** Object:  Table [dbo].[AutoId]    Script Date: 03/10/2021 9:29:14 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AutoId](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Prefix] [varchar](10) NULL,
	[Suffixes] [varchar](10) NULL,
	[Value] [int] NULL,
 CONSTRAINT [PK_AutoId] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 03/10/2021 9:29:14 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Employee](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](50) NULL,
	[FullName] [nvarchar](250) NULL,
	[Title] [nvarchar](250) NULL,
	[Birthday] [datetime] NULL,
	[LevelId] [int] NOT NULL,
	[Phone] [varchar](20) NULL,
	[TeamId] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedAt] [datetime] NULL,
	[IsDeleted] [bit] NULL,
	[Email] [varchar](200) NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Level]    Script Date: 03/10/2021 9:29:14 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Level](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Level] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Team]    Script Date: 03/10/2021 9:29:14 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Team](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[ParentTeamId] [int] NOT NULL,
	[LeaderId] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedAt] [datetime] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Team] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Employee] ON 

INSERT [dbo].[Employee] ([Id], [Code], [FullName], [Title], [Birthday], [LevelId], [Phone], [TeamId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted], [Email]) VALUES (1, N'001', N'Quang Nghị', N'Qc', NULL, 1, NULL, 25, 1, CAST(0x0000AD9700000000 AS DateTime), NULL, CAST(0x0000ADB600DB7613 AS DateTime), NULL, N'quangnghi@gmail.com')
INSERT [dbo].[Employee] ([Id], [Code], [FullName], [Title], [Birthday], [LevelId], [Phone], [TeamId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted], [Email]) VALUES (3, N'002', N'Quang Phát', N'Dev', NULL, 2, N'039999993', 1, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, CAST(0x0000ADB600DB427C AS DateTime), NULL, N'quangphat@gmail.com')
INSERT [dbo].[Employee] ([Id], [Code], [FullName], [Title], [Birthday], [LevelId], [Phone], [TeamId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted], [Email]) VALUES (4, N'2010000004', N'Hữu  Tân', N'dev', NULL, 0, NULL, 1, NULL, CAST(0x0000ADB600DF5CF6 AS DateTime), NULL, NULL, NULL, NULL)
INSERT [dbo].[Employee] ([Id], [Code], [FullName], [Title], [Birthday], [LevelId], [Phone], [TeamId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted], [Email]) VALUES (5, N'2010000005', N'Sơn', N'Manager', NULL, 0, NULL, 1, NULL, CAST(0x0000ADB600DFBB7C AS DateTime), NULL, NULL, NULL, NULL)
INSERT [dbo].[Employee] ([Id], [Code], [FullName], [Title], [Birthday], [LevelId], [Phone], [TeamId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted], [Email]) VALUES (6, N'2010000006', N'Kiệt', N'Manager', NULL, 0, N'3333', 25, NULL, CAST(0x0000ADB600DFEDA5 AS DateTime), NULL, CAST(0x0000ADB600E6ABB0 AS DateTime), NULL, N'kiet@gmail.com')
INSERT [dbo].[Employee] ([Id], [Code], [FullName], [Title], [Birthday], [LevelId], [Phone], [TeamId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted], [Email]) VALUES (7, N'2010000007', N'edwed', NULL, NULL, 0, NULL, 2, NULL, CAST(0x0000ADB600E6D186 AS DateTime), NULL, CAST(0x0000ADB600EA6AA9 AS DateTime), 1, NULL)
SET IDENTITY_INSERT [dbo].[Employee] OFF
SET IDENTITY_INSERT [dbo].[Level] ON 

INSERT [dbo].[Level] ([Id], [Name]) VALUES (1, N'1.1')
INSERT [dbo].[Level] ([Id], [Name]) VALUES (2, N'1.2')
SET IDENTITY_INSERT [dbo].[Level] OFF
SET IDENTITY_INSERT [dbo].[Team] ON 

INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (1, N'.Net Team', 7, 5, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, CAST(0x0000ADB600DFC489 AS DateTime), 0)
INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (2, N'Developemnt', 1, 1, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, CAST(0x0000ADB600EBB73B AS DateTime), 1)
INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (3, N'QC', 2, 6, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, CAST(0x0000ADB600EBF61C AS DateTime), 0)
INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (4, N'Developemnt', 0, 1, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, CAST(0x0000ADB600ED391C AS DateTime), 1)
INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (5, N'Developemnt', 0, 1, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, CAST(0x0000ADB600ED4214 AS DateTime), 1)
INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (6, N'Developemnt', 0, 1, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (7, N'Developemnt', 0, 1, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (8, N'Developemnt', 0, 1, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (9, N'Developemnt', 0, 1, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (10, N'Developemnt', 0, 1, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (11, N'Developemnt', 0, 1, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (12, N'Developemnt', 0, 1, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (13, N'Developemnt', 0, 1, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (14, N'Developemnt', 0, 1, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (15, N'Developemnt', 0, 1, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (16, N'Developemnt', 0, 1, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (17, N'Developemnt', 0, 1, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (18, N'Developemnt', 0, 1, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (19, N'Developemnt', 0, 1, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (20, N'Developemnt', 0, 1, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (21, N'Developemnt', 0, 1, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (22, N'Developemnt', 0, 1, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (23, N'Developemnt', 0, 1, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (24, N'Developemnt', 0, 1, 1, CAST(0x0000ADB500000000 AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Team] ([Id], [Name], [ParentTeamId], [LeaderId], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (25, N'QC', 1, 6, NULL, CAST(0x0000ADB600CCDF8E AS DateTime), NULL, CAST(0x0000ADB600EB93D1 AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[Team] OFF
