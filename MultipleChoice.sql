USE [master]
GO
/****** Object:  Database [MultipleChoice]    Script Date: 22/06/2023 2:15:30 AM ******/
CREATE DATABASE [MultipleChoice]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MultipleChoice', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\MultipleChoice.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'MultipleChoice_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\MultipleChoice_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [MultipleChoice] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MultipleChoice].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MultipleChoice] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MultipleChoice] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MultipleChoice] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MultipleChoice] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MultipleChoice] SET ARITHABORT OFF 
GO
ALTER DATABASE [MultipleChoice] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MultipleChoice] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MultipleChoice] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MultipleChoice] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MultipleChoice] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MultipleChoice] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MultipleChoice] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MultipleChoice] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MultipleChoice] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MultipleChoice] SET  DISABLE_BROKER 
GO
ALTER DATABASE [MultipleChoice] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MultipleChoice] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MultipleChoice] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MultipleChoice] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MultipleChoice] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MultipleChoice] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MultipleChoice] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MultipleChoice] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [MultipleChoice] SET  MULTI_USER 
GO
ALTER DATABASE [MultipleChoice] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MultipleChoice] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MultipleChoice] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MultipleChoice] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [MultipleChoice] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [MultipleChoice] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [MultipleChoice] SET QUERY_STORE = ON
GO
ALTER DATABASE [MultipleChoice] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [MultipleChoice]
GO
/****** Object:  Table [dbo].[Admins]    Script Date: 22/06/2023 2:15:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Admins](
	[Username] [varchar](20) NOT NULL,
	[Password] [varchar](250) NULL,
 CONSTRAINT [PK_Admins] PRIMARY KEY CLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Answers]    Script Date: 22/06/2023 2:15:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Answers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AnswerLabel] [nchar](1) NULL,
	[IsDelete] [tinyint] NULL,
 CONSTRAINT [PK_Answers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Chapters]    Script Date: 22/06/2023 2:15:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Chapters](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ChapterName] [nvarchar](50) NULL,
	[Meta] [varchar](50) NULL,
	[IdSubject] [int] NULL,
	[IsDelete] [tinyint] NULL,
 CONSTRAINT [PK_Chapters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Classes]    Script Date: 22/06/2023 2:15:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Classes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClassName] [nvarchar](50) NULL,
	[Meta] [varchar](50) NULL,
	[IdGrade] [int] NULL,
	[IsDelete] [tinyint] NULL,
 CONSTRAINT [PK_Grades] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExamDurations]    Script Date: 22/06/2023 2:15:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExamDurations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DurationName] [nvarchar](50) NULL,
	[DurationTime] [int] NULL,
 CONSTRAINT [PK_ExamDurations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Exams]    Script Date: 22/06/2023 2:15:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Exams](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ExamDate] [datetime] NULL,
	[IdDuration] [int] NULL,
	[IdSubject] [int] NULL,
	[Author] [int] NULL,
	[IsApprove] [tinyint] NULL,
	[IsDelete] [tinyint] NULL,
 CONSTRAINT [PK_Exams] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Exams_Questions]    Script Date: 22/06/2023 2:15:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Exams_Questions](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdExam] [int] NULL,
	[IdQuestion] [bigint] NULL,
	[IsDelete] [tinyint] NULL,
 CONSTRAINT [PK_Exams_Questions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Grades]    Script Date: 22/06/2023 2:15:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Grades](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GradeName] [nvarchar](50) NULL,
	[Meta] [varchar](50) NULL,
	[IsDelete] [tinyint] NULL,
 CONSTRAINT [PK_Grades_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Learnings]    Script Date: 22/06/2023 2:15:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Learnings](
	[IdStudent] [varchar](50) NOT NULL,
	[IdClass] [int] NOT NULL,
	[Year] [int] NOT NULL,
 CONSTRAINT [PK_Learnings_1] PRIMARY KEY CLUSTERED 
(
	[IdStudent] ASC,
	[IdClass] ASC,
	[Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lessons]    Script Date: 22/06/2023 2:15:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lessons](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LessonName] [nvarchar](150) NULL,
	[Meta] [varchar](150) NULL,
	[IdChapter] [int] NULL,
	[IsDelete] [tinyint] NULL,
 CONSTRAINT [PK_Lessons] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Levels]    Script Date: 22/06/2023 2:15:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Levels](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LevelName] [nvarchar](20) NULL,
	[IsDelete] [tinyint] NULL,
 CONSTRAINT [PK_Levels] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Questions]    Script Date: 22/06/2023 2:15:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Questions](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[QuestionContent] [nvarchar](250) NULL,
	[Answer_A] [nvarchar](250) NULL,
	[Answer_B] [nvarchar](250) NULL,
	[Answer_C] [nvarchar](250) NULL,
	[Answer_D] [nvarchar](250) NULL,
	[IdAnswer] [int] NULL,
	[Note] [nvarchar](250) NULL,
	[IsApprove] [tinyint] NULL,
	[Approver] [int] NULL,
	[IdLevel] [int] NULL,
	[IdLesson] [int] NULL,
	[Author] [int] NULL,
	[CreationDate] [datetime] NULL,
	[Editor] [int] NULL,
	[EditingDate] [datetime] NULL,
	[Eraser] [int] NULL,
	[DeletionDate] [datetime] NULL,
	[IsDelete] [tinyint] NULL,
 CONSTRAINT [PK_Questions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Results]    Script Date: 22/06/2023 2:15:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Results](
	[IdStudent] [varchar](50) NOT NULL,
	[IdExam_Question] [bigint] NOT NULL,
	[Answer] [varchar](1) NULL,
	[IsDelete] [tinyint] NULL,
 CONSTRAINT [PK_Results_1] PRIMARY KEY CLUSTERED 
(
	[IdStudent] ASC,
	[IdExam_Question] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Settings]    Script Date: 22/06/2023 2:15:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Settings](
	[Keyword] [varchar](20) NOT NULL,
	[Value] [nvarchar](250) NULL,
	[Description] [ntext] NULL,
 CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED 
(
	[Keyword] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Students]    Script Date: 22/06/2023 2:15:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Students](
	[IdStudent] [varchar](50) NOT NULL,
	[Password] [varchar](250) NULL,
	[StudentName] [nvarchar](30) NULL,
	[Birthday] [date] NULL,
	[Email] [varchar](150) NULL,
	[Phone] [varchar](10) NULL,
	[Address] [nvarchar](150) NULL,
	[IsDelete] [tinyint] NULL,
 CONSTRAINT [PK_Members] PRIMARY KEY CLUSTERED 
(
	[IdStudent] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subjects]    Script Date: 22/06/2023 2:15:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subjects](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SubjectName] [nvarchar](50) NULL,
	[IdGrade] [int] NULL,
	[Meta] [varchar](50) NULL,
	[IdLeader] [int] NULL,
	[IsDelete] [tinyint] NULL,
 CONSTRAINT [PK_Subjects] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teachers]    Script Date: 22/06/2023 2:15:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teachers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdTeacher] [varchar](20) NULL,
	[Password] [varchar](150) NULL,
	[TeacherName] [nvarchar](30) NULL,
	[Phone] [varchar](30) NULL,
	[Email] [varchar](50) NULL,
	[Address] [nvarchar](150) NULL,
	[IdSubject] [int] NULL,
	[IsDelete] [tinyint] NULL,
 CONSTRAINT [PK_Teachers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teachings]    Script Date: 22/06/2023 2:15:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teachings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdTeacher] [int] NOT NULL,
	[IdSubject] [int] NOT NULL,
	[IdClass] [int] NULL,
	[StartingDate] [date] NULL,
	[EndingDate] [date] NULL,
	[IsDelete] [tinyint] NULL,
 CONSTRAINT [PK_Teachings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Answers] ADD  CONSTRAINT [DF_Answers_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Chapters] ADD  CONSTRAINT [DF_Chapters_IsDelete_1]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Classes] ADD  CONSTRAINT [DF_Classes_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Exams] ADD  CONSTRAINT [DF_Exams_IsApprove]  DEFAULT ((0)) FOR [IsApprove]
GO
ALTER TABLE [dbo].[Exams] ADD  CONSTRAINT [DF_Exams_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Exams_Questions] ADD  CONSTRAINT [DF_Exams_Questions_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Grades] ADD  CONSTRAINT [DF_Grades_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Lessons] ADD  CONSTRAINT [DF_Lessons_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Levels] ADD  CONSTRAINT [DF_Levels_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Questions] ADD  CONSTRAINT [DF_Questions_Approved]  DEFAULT ((0)) FOR [IsApprove]
GO
ALTER TABLE [dbo].[Questions] ADD  CONSTRAINT [DF_Questions_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Results] ADD  CONSTRAINT [DF_Results_IsDelete_1]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Students] ADD  CONSTRAINT [DF_Students_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Subjects] ADD  CONSTRAINT [DF_Subjects_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Teachers] ADD  CONSTRAINT [DF_Teachers_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Teachings] ADD  CONSTRAINT [DF_Teachings_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Chapters]  WITH CHECK ADD  CONSTRAINT [FK_Chapters_Subjects] FOREIGN KEY([IdSubject])
REFERENCES [dbo].[Subjects] ([Id])
GO
ALTER TABLE [dbo].[Chapters] CHECK CONSTRAINT [FK_Chapters_Subjects]
GO
ALTER TABLE [dbo].[Classes]  WITH CHECK ADD  CONSTRAINT [FK_Classes_Grades] FOREIGN KEY([IdGrade])
REFERENCES [dbo].[Grades] ([Id])
GO
ALTER TABLE [dbo].[Classes] CHECK CONSTRAINT [FK_Classes_Grades]
GO
ALTER TABLE [dbo].[Exams]  WITH CHECK ADD  CONSTRAINT [FK_Exams_ExamDurations] FOREIGN KEY([IdDuration])
REFERENCES [dbo].[ExamDurations] ([Id])
GO
ALTER TABLE [dbo].[Exams] CHECK CONSTRAINT [FK_Exams_ExamDurations]
GO
ALTER TABLE [dbo].[Exams]  WITH CHECK ADD  CONSTRAINT [FK_Exams_Subjects] FOREIGN KEY([IdSubject])
REFERENCES [dbo].[Subjects] ([Id])
GO
ALTER TABLE [dbo].[Exams] CHECK CONSTRAINT [FK_Exams_Subjects]
GO
ALTER TABLE [dbo].[Exams]  WITH CHECK ADD  CONSTRAINT [FK_Exams_Teachers] FOREIGN KEY([Author])
REFERENCES [dbo].[Teachers] ([Id])
GO
ALTER TABLE [dbo].[Exams] CHECK CONSTRAINT [FK_Exams_Teachers]
GO
ALTER TABLE [dbo].[Exams_Questions]  WITH CHECK ADD  CONSTRAINT [FK_Exams_Questions_Exams] FOREIGN KEY([IdExam])
REFERENCES [dbo].[Exams] ([Id])
GO
ALTER TABLE [dbo].[Exams_Questions] CHECK CONSTRAINT [FK_Exams_Questions_Exams]
GO
ALTER TABLE [dbo].[Exams_Questions]  WITH CHECK ADD  CONSTRAINT [FK_Exams_Questions_Questions] FOREIGN KEY([IdQuestion])
REFERENCES [dbo].[Questions] ([Id])
GO
ALTER TABLE [dbo].[Exams_Questions] CHECK CONSTRAINT [FK_Exams_Questions_Questions]
GO
ALTER TABLE [dbo].[Learnings]  WITH CHECK ADD  CONSTRAINT [FK_Learnings_Classes] FOREIGN KEY([IdClass])
REFERENCES [dbo].[Classes] ([Id])
GO
ALTER TABLE [dbo].[Learnings] CHECK CONSTRAINT [FK_Learnings_Classes]
GO
ALTER TABLE [dbo].[Learnings]  WITH CHECK ADD  CONSTRAINT [FK_Learnings_Students] FOREIGN KEY([IdStudent])
REFERENCES [dbo].[Students] ([IdStudent])
GO
ALTER TABLE [dbo].[Learnings] CHECK CONSTRAINT [FK_Learnings_Students]
GO
ALTER TABLE [dbo].[Lessons]  WITH CHECK ADD  CONSTRAINT [FK_Lessons_Chapters] FOREIGN KEY([IdChapter])
REFERENCES [dbo].[Chapters] ([Id])
GO
ALTER TABLE [dbo].[Lessons] CHECK CONSTRAINT [FK_Lessons_Chapters]
GO
ALTER TABLE [dbo].[Questions]  WITH CHECK ADD  CONSTRAINT [FK_Questions_Answers] FOREIGN KEY([IdAnswer])
REFERENCES [dbo].[Answers] ([Id])
GO
ALTER TABLE [dbo].[Questions] CHECK CONSTRAINT [FK_Questions_Answers]
GO
ALTER TABLE [dbo].[Questions]  WITH CHECK ADD  CONSTRAINT [FK_Questions_Lessons] FOREIGN KEY([IdLesson])
REFERENCES [dbo].[Lessons] ([Id])
GO
ALTER TABLE [dbo].[Questions] CHECK CONSTRAINT [FK_Questions_Lessons]
GO
ALTER TABLE [dbo].[Questions]  WITH CHECK ADD  CONSTRAINT [FK_Questions_Levels1] FOREIGN KEY([IdLevel])
REFERENCES [dbo].[Levels] ([Id])
GO
ALTER TABLE [dbo].[Questions] CHECK CONSTRAINT [FK_Questions_Levels1]
GO
ALTER TABLE [dbo].[Results]  WITH CHECK ADD  CONSTRAINT [FK_Results_Exams_Questions] FOREIGN KEY([IdExam_Question])
REFERENCES [dbo].[Exams_Questions] ([Id])
GO
ALTER TABLE [dbo].[Results] CHECK CONSTRAINT [FK_Results_Exams_Questions]
GO
ALTER TABLE [dbo].[Results]  WITH CHECK ADD  CONSTRAINT [FK_Results_Students] FOREIGN KEY([IdStudent])
REFERENCES [dbo].[Students] ([IdStudent])
GO
ALTER TABLE [dbo].[Results] CHECK CONSTRAINT [FK_Results_Students]
GO
ALTER TABLE [dbo].[Subjects]  WITH CHECK ADD  CONSTRAINT [FK_Subjects_Grades] FOREIGN KEY([IdGrade])
REFERENCES [dbo].[Grades] ([Id])
GO
ALTER TABLE [dbo].[Subjects] CHECK CONSTRAINT [FK_Subjects_Grades]
GO
ALTER TABLE [dbo].[Subjects]  WITH CHECK ADD  CONSTRAINT [FK_Subjects_Teachers] FOREIGN KEY([IdLeader])
REFERENCES [dbo].[Teachers] ([Id])
GO
ALTER TABLE [dbo].[Subjects] CHECK CONSTRAINT [FK_Subjects_Teachers]
GO
ALTER TABLE [dbo].[Teachers]  WITH CHECK ADD  CONSTRAINT [FK_Teachers_Subjects] FOREIGN KEY([IdSubject])
REFERENCES [dbo].[Subjects] ([Id])
GO
ALTER TABLE [dbo].[Teachers] CHECK CONSTRAINT [FK_Teachers_Subjects]
GO
ALTER TABLE [dbo].[Teachings]  WITH CHECK ADD  CONSTRAINT [FK_Teachings_Classes] FOREIGN KEY([IdClass])
REFERENCES [dbo].[Classes] ([Id])
GO
ALTER TABLE [dbo].[Teachings] CHECK CONSTRAINT [FK_Teachings_Classes]
GO
ALTER TABLE [dbo].[Teachings]  WITH CHECK ADD  CONSTRAINT [FK_Teachings_Subjects] FOREIGN KEY([IdSubject])
REFERENCES [dbo].[Subjects] ([Id])
GO
ALTER TABLE [dbo].[Teachings] CHECK CONSTRAINT [FK_Teachings_Subjects]
GO
ALTER TABLE [dbo].[Teachings]  WITH CHECK ADD  CONSTRAINT [FK_Teachings_Teachers] FOREIGN KEY([IdTeacher])
REFERENCES [dbo].[Teachers] ([Id])
GO
ALTER TABLE [dbo].[Teachings] CHECK CONSTRAINT [FK_Teachings_Teachers]
GO
USE [master]
GO
ALTER DATABASE [MultipleChoice] SET  READ_WRITE 
GO
