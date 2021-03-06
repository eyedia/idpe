USE [master]
GO
/****** Object:  Database [idpe]    Script Date: 1/13/2018 3:29:00 PM ******/
CREATE DATABASE [idpe] ON  PRIMARY 
( NAME = N'idpe', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\idpe.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'idpe_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\idpe_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [idpe] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [idpe].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [idpe] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [idpe] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [idpe] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [idpe] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [idpe] SET ARITHABORT OFF 
GO
ALTER DATABASE [idpe] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [idpe] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [idpe] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [idpe] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [idpe] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [idpe] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [idpe] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [idpe] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [idpe] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [idpe] SET  DISABLE_BROKER 
GO
ALTER DATABASE [idpe] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [idpe] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [idpe] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [idpe] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [idpe] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [idpe] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [idpe] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [idpe] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [idpe] SET  MULTI_USER 
GO
ALTER DATABASE [idpe] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [idpe] SET DB_CHAINING OFF 
GO
USE [idpe]
GO
/****** Object:  Table [dbo].[AVPair]    Script Date: 1/13/2018 3:29:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AVPair](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Key] [varchar](255) NULL,
	[Value] [varchar](1000) NULL,
	[CreatedBy] [varchar](255) NULL,
	[CreatedTS] [datetime] NULL,
	[ModifiedBy] [varchar](255) NULL,
	[ModifiedTS] [datetime] NULL,
	[Source] [varchar](255) NULL,
 CONSTRAINT [PK_AVPair] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Key] UNIQUE NONCLUSTERED 
(
	[Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CodeSet]    Script Date: 1/13/2018 3:29:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CodeSet](
	[CodeSetId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](50) NOT NULL,
	[EnumCode] [int] NOT NULL,
	[Value] [varchar](255) NOT NULL,
	[ReferenceKey] [varchar](50) NULL,
	[Description] [varchar](255) NULL,
	[Position] [int] NULL,
	[CreatedTS] [datetime] NULL,
	[CreatedBy] [varchar](255) NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [varchar](255) NULL,
	[Source] [varchar](255) NULL,
 CONSTRAINT [PK_IdpeCodeSet] PRIMARY KEY CLUSTERED 
(
	[CodeSetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EmailTracker]    Script Date: 1/13/2018 3:29:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmailTracker](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DataSourceId] [int] NOT NULL,
	[Subject] [varchar](4000) NOT NULL,
	[Body] [varchar](4000) NOT NULL,
	[ReferenceId] [int] NULL,
	[CreatedTS] [datetime] NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [varchar](255) NULL,
	[Source] [varchar](255) NOT NULL,
 CONSTRAINT [PK_EmailTracker] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Group]    Script Date: 1/13/2018 3:29:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Group](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](1000) NULL,
	[CreatedTS] [datetime] NULL,
	[CreatedBy] [varchar](255) NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [varchar](255) NULL,
	[Source] [varchar](255) NULL,
 CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GroupUser]    Script Date: 1/13/2018 3:29:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GroupUser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GroupId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[CreatedTS] [datetime] NULL,
	[CreatedBy] [varchar](255) NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [varchar](255) NULL,
	[Source] [varchar](255) NULL,
 CONSTRAINT [PK_GroupUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IdpeAttribute]    Script Date: 1/13/2018 3:29:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IdpeAttribute](
	[AttributeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Type] [varchar](50) NOT NULL,
	[Minimum] [varchar](255) NULL,
	[Maximum] [varchar](255) NULL,
	[Formula] [varchar](2000) NULL,
	[IsAcceptable] [bit] NULL,
	[Position] [int] NULL,
	[AttributePrintValueType] [int] NULL,
	[AttributePrintValueCustom] [varchar](4000) NULL,
	[CreatedTS] [datetime] NOT NULL CONSTRAINT [DF_IdpeAttribute_CreatedTS]  DEFAULT (getdate()),
	[CreatedBy] [varchar](255) NOT NULL CONSTRAINT [DF_IdpeAttribute_CreatedBy]  DEFAULT (suser_sname()),
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [varchar](255) NULL,
	[Source] [varchar](255) NOT NULL CONSTRAINT [DF_IdpeAttribute_Source]  DEFAULT ('Management Studio'),
 CONSTRAINT [PK_IdpeAttribute] PRIMARY KEY CLUSTERED 
(
	[AttributeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IdpeAttributeDataSource]    Script Date: 1/13/2018 3:29:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IdpeAttributeDataSource](
	[AttributeDataSourceId] [int] IDENTITY(1,1) NOT NULL,
	[DataSourceId] [int] NOT NULL,
	[AttributeId] [int] NOT NULL,
	[Position] [int] NULL,
	[IsAcceptable] [bit] NOT NULL CONSTRAINT [DF_IdpeAttributeDataSource_IsAcceptable]  DEFAULT ((0)),
	[AttributePrintValueType] [int] NULL,
	[AttributePrintValueCustom] [varchar](4000) NULL,
	[CreatedTS] [datetime] NOT NULL CONSTRAINT [DF_IdpeAttributeDataSource_CreatedTS]  DEFAULT (getdate()),
	[CreatedBy] [varchar](255) NOT NULL CONSTRAINT [DF_IdpeAttributeDataSource_CreatedBy]  DEFAULT (suser_sname()),
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [varchar](255) NULL,
	[Source] [varchar](255) NOT NULL CONSTRAINT [DF_IdpeAttributeDataSource_Source]  DEFAULT ('Management Studio'),
 CONSTRAINT [PK_IdpeAttributeDataSource] PRIMARY KEY CLUSTERED 
(
	[AttributeDataSourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IdpeDataSource]    Script Date: 1/13/2018 3:29:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IdpeDataSource](
	[Id] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](500) NOT NULL,
	[IsSystem] [bit] NULL,
	[DataFeederType] [int] NULL,
	[DataFormatType] [int] NULL,
	[Delimiter] [varchar](2) NULL,
	[SystemDataSourceId] [int] NULL,
	[DataContainerValidatorType] [varchar](255) NULL,
	[OutputType] [int] NULL,
	[OutputWriterTypeFullName] [varchar](255) NULL,
	[PlugInsType] [varchar](255) NULL,
	[ProcessingBy] [varchar](255) NULL,
	[PusherType] [int] NULL,
	[PusherTypeFullName] [varchar](500) NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_IdpeDataSource_IsActive]  DEFAULT ((1)),
	[CreatedTS] [datetime] NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [varchar](255) NULL,
	[Source] [varchar](255) NULL,
 CONSTRAINT [PK_IdpeDataSource] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IdpeKey]    Script Date: 1/13/2018 3:29:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IdpeKey](
	[KeyId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](500) NOT NULL,
	[Value] [varchar](4000) NULL,
	[ValueBinary] [varbinary](max) NULL,
	[Type] [int] NOT NULL,
	[IsDeployable] [bit] NULL,
	[DataSourceId] [int] NULL,
	[NextKeyId] [int] NULL,
	[CreatedTS] [datetime] NULL,
	[CreatedBy] [varchar](255) NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [varchar](255) NULL,
	[Source] [varchar](255) NULL,
 CONSTRAINT [PK_IdpeKey] PRIMARY KEY CLUSTERED 
(
	[KeyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IdpeKeyDataSource]    Script Date: 1/13/2018 3:29:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IdpeKeyDataSource](
	[KeyDataSourceId] [int] IDENTITY(1,1) NOT NULL,
	[KeyId] [int] NOT NULL,
	[DataSourceId] [int] NOT NULL,
	[IsDeployable] [bit] NULL,
	[CreatedTS] [datetime] NULL,
	[CreatedBy] [varchar](255) NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [varchar](255) NULL,
	[Source] [varchar](255) NULL,
 CONSTRAINT [PK_IdpeKeyDataSource] PRIMARY KEY CLUSTERED 
(
	[KeyDataSourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IdpeLog]    Script Date: 1/13/2018 3:29:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IdpeLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [varchar](500) NOT NULL,
	[SubFileName] [varchar](260) NULL,
	[DataSourceId] [int] NOT NULL,
	[DataSourceName] [varchar](255) NULL,
	[TotalRecords] [int] NOT NULL,
	[TotalValidRecords] [int] NOT NULL,
	[Started] [datetime] NOT NULL,
	[Finished] [datetime] NOT NULL,
	[Environment] [varchar](500) NOT NULL,
	[CreatedTS] [datetime] NULL,
	[CreatedBy] [varchar](255) NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [varchar](255) NULL,
	[Source] [varchar](255) NULL,
 CONSTRAINT [PK_IdpeLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IdpePersistentVariable]    Script Date: 1/13/2018 3:29:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IdpePersistentVariable](
	[DataSourceId] [int] NOT NULL,
	[Name] [varchar](500) NOT NULL,
	[Value] [varchar](max) NULL,
	[CreatedTS] [datetime] NOT NULL,
	[CreatedBy] [varchar](255) NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [varchar](255) NULL,
	[Source] [varchar](255) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IdpeRule]    Script Date: 1/13/2018 3:29:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IdpeRule](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Description] [varchar](500) NULL,
	[Xaml] [varchar](max) NOT NULL,
	[Priority] [int] NULL,
	[RuleSetType] [int] NULL,
	[CreatedTS] [datetime] NOT NULL CONSTRAINT [DF_IdpeRule_UpdatedOn]  DEFAULT (getdate()),
	[CreatedBy] [varchar](255) NOT NULL CONSTRAINT [DF_IdpeRule_UpdatedBy]  DEFAULT (suser_sname()),
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [varchar](255) NULL,
	[Source] [varchar](255) NULL,
 CONSTRAINT [PK_IdpeRule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IdpeRuleDataSource]    Script Date: 1/13/2018 3:29:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IdpeRuleDataSource](
	[RuleDataSourceId] [int] IDENTITY(1,1) NOT NULL,
	[RuleId] [int] NOT NULL,
	[DataSourceId] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[RuleSetType] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedTS] [datetime] NULL,
	[CreatedBy] [varchar](255) NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [varchar](255) NULL,
	[Source] [varchar](255) NULL,
 CONSTRAINT [PK_IdpeRuleDataSource] PRIMARY KEY CLUSTERED 
(
	[RuleDataSourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IdpeVersion]    Script Date: 1/13/2018 3:29:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IdpeVersion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Version] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[ReferenceId] [int] NOT NULL,
	[Data] [varbinary](max) NOT NULL,
	[CreatedTS] [datetime] NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [varchar](255) NULL,
	[Source] [varchar](255) NULL,
 CONSTRAINT [PK_IdpeVersion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_IdpeVersion] UNIQUE NONCLUSTERED 
(
	[Version] ASC,
	[Type] ASC,
	[ReferenceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [Ix_IdpeVersions] UNIQUE NONCLUSTERED 
(
	[Version] ASC,
	[Type] ASC,
	[ReferenceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User]    Script Date: 1/13/2018 3:29:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [varchar](255) NOT NULL,
	[UserName] [varchar](30) NOT NULL,
	[Password] [varchar](1000) NOT NULL,
	[EmailId] [varchar](50) NULL,
	[Preferences] [varchar](4000) NULL,
	[GroupId] [int] NULL,
	[IsDebugUser] [bit] NULL,
	[IsSystemUser] [bit] NULL,
	[CreatedTS] [datetime] NULL,
	[CreatedBy] [varchar](255) NULL,
	[ModifiedTS] [datetime] NULL,
	[ModifiedBy] [varchar](255) NULL,
	[Source] [varchar](255) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [Ix_CodeSet_EnumCode]    Script Date: 1/13/2018 3:29:00 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [Ix_CodeSet_EnumCode] ON [dbo].[CodeSet]
(
	[Code] ASC,
	[EnumCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [Ix_Groups]    Script Date: 1/13/2018 3:29:00 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [Ix_Groups] ON [dbo].[Group]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [Ix_IdpAttributes]    Script Date: 1/13/2018 3:29:00 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [Ix_IdpAttributes] ON [dbo].[IdpeAttribute]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [Ix_Idpe_AttributeId_DataSourceId]    Script Date: 1/13/2018 3:29:00 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [Ix_Idpe_AttributeId_DataSourceId] ON [dbo].[IdpeAttributeDataSource]
(
	[DataSourceId] ASC,
	[AttributeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [Ix_IdpeDataSources]    Script Date: 1/13/2018 3:29:00 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [Ix_IdpeDataSources] ON [dbo].[IdpeDataSource]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_IdpeRules]    Script Date: 1/13/2018 3:29:00 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_IdpeRules] ON [dbo].[IdpeRule]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [Ix_Users]    Script Date: 1/13/2018 3:29:00 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [Ix_Users] ON [dbo].[User]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AVPair] ADD  CONSTRAINT [DF_AVPair_CreatedBy]  DEFAULT (suser_sname()) FOR [CreatedBy]
GO
ALTER TABLE [dbo].[AVPair] ADD  CONSTRAINT [DF_AVPair_CreatedOn]  DEFAULT (getdate()) FOR [CreatedTS]
GO
ALTER TABLE [dbo].[AVPair] ADD  CONSTRAINT [DF_AVPair_Source]  DEFAULT ('Management Studio') FOR [Source]
GO
ALTER TABLE [dbo].[GroupUser]  WITH CHECK ADD  CONSTRAINT [FK_GroupUser_Group] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Group] ([Id])
GO
ALTER TABLE [dbo].[GroupUser] CHECK CONSTRAINT [FK_GroupUser_Group]
GO
ALTER TABLE [dbo].[GroupUser]  WITH CHECK ADD  CONSTRAINT [FK_GroupUser_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[GroupUser] CHECK CONSTRAINT [FK_GroupUser_User]
GO
ALTER TABLE [dbo].[IdpeAttributeDataSource]  WITH CHECK ADD  CONSTRAINT [FK_IdpeAttributeDataSource_IdpeAttribute] FOREIGN KEY([AttributeId])
REFERENCES [dbo].[IdpeAttribute] ([AttributeId])
GO
ALTER TABLE [dbo].[IdpeAttributeDataSource] CHECK CONSTRAINT [FK_IdpeAttributeDataSource_IdpeAttribute]
GO
ALTER TABLE [dbo].[IdpeAttributeDataSource]  WITH CHECK ADD  CONSTRAINT [FK_IdpeAttributeDataSource_IdpeDataSource] FOREIGN KEY([DataSourceId])
REFERENCES [dbo].[IdpeDataSource] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[IdpeAttributeDataSource] CHECK CONSTRAINT [FK_IdpeAttributeDataSource_IdpeDataSource]
GO
ALTER TABLE [dbo].[IdpeDataSource]  WITH CHECK ADD  CONSTRAINT [FK_IdpeDataSource_IdpeDataSource] FOREIGN KEY([SystemDataSourceId])
REFERENCES [dbo].[IdpeDataSource] ([Id])
GO
ALTER TABLE [dbo].[IdpeDataSource] CHECK CONSTRAINT [FK_IdpeDataSource_IdpeDataSource]
GO
ALTER TABLE [dbo].[IdpeKey]  WITH NOCHECK ADD  CONSTRAINT [FK_IdpeKey_IdpeKey] FOREIGN KEY([NextKeyId])
REFERENCES [dbo].[IdpeKey] ([KeyId])
GO
ALTER TABLE [dbo].[IdpeKey] CHECK CONSTRAINT [FK_IdpeKey_IdpeKey]
GO
ALTER TABLE [dbo].[IdpeKeyDataSource]  WITH CHECK ADD  CONSTRAINT [FK_IdpeKeyDataSource_IdpeDataSource] FOREIGN KEY([DataSourceId])
REFERENCES [dbo].[IdpeDataSource] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[IdpeKeyDataSource] CHECK CONSTRAINT [FK_IdpeKeyDataSource_IdpeDataSource]
GO
ALTER TABLE [dbo].[IdpeKeyDataSource]  WITH CHECK ADD  CONSTRAINT [FK_IdpeKeyDataSource_IdpeKey] FOREIGN KEY([KeyId])
REFERENCES [dbo].[IdpeKey] ([KeyId])
GO
ALTER TABLE [dbo].[IdpeKeyDataSource] CHECK CONSTRAINT [FK_IdpeKeyDataSource_IdpeKey]
GO
ALTER TABLE [dbo].[IdpeLog]  WITH CHECK ADD  CONSTRAINT [FK_IdpeLog_IdpeDataSource] FOREIGN KEY([DataSourceId])
REFERENCES [dbo].[IdpeDataSource] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[IdpeLog] CHECK CONSTRAINT [FK_IdpeLog_IdpeDataSource]
GO
ALTER TABLE [dbo].[IdpePersistentVariable]  WITH CHECK ADD  CONSTRAINT [FK_IdpePersistentVariable_IdpeDataSource] FOREIGN KEY([DataSourceId])
REFERENCES [dbo].[IdpeDataSource] ([Id])
GO
ALTER TABLE [dbo].[IdpePersistentVariable] CHECK CONSTRAINT [FK_IdpePersistentVariable_IdpeDataSource]
GO
ALTER TABLE [dbo].[IdpeRuleDataSource]  WITH CHECK ADD  CONSTRAINT [FK_IdpeRuleDataSource_IdpeDataSource] FOREIGN KEY([DataSourceId])
REFERENCES [dbo].[IdpeDataSource] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[IdpeRuleDataSource] CHECK CONSTRAINT [FK_IdpeRuleDataSource_IdpeDataSource]
GO
ALTER TABLE [dbo].[IdpeRuleDataSource]  WITH CHECK ADD  CONSTRAINT [FK_IdpeRuleDataSource_IdpeRule] FOREIGN KEY([RuleId])
REFERENCES [dbo].[IdpeRule] ([Id])
GO
ALTER TABLE [dbo].[IdpeRuleDataSource] CHECK CONSTRAINT [FK_IdpeRuleDataSource_IdpeRule]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Group] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Group] ([Id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Group]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'is required with Poolers are configured to process files' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdpeDataSource', @level2type=N'COLUMN',@level2name=N'ProcessingBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PusherType can inherit SimplusRuleEngine.Services.Pusher class and override FileProcessed(PoolersEventArgs e) to implement any push mechanism' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdpeDataSource', @level2type=N'COLUMN',@level2name=N'PusherTypeFullName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This column will not have any value in the database, it was used to use the field at object level' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdpeKey', @level2type=N'COLUMN',@level2name=N'DataSourceId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This has been placed only to be used within .Net. It will never be populated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdpeLog', @level2type=N'COLUMN',@level2name=N'DataSourceName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dummy column (to be used in c#)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdpeRule', @level2type=N'COLUMN',@level2name=N'Priority'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dummy column (to be used in c#)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdpeRule', @level2type=N'COLUMN',@level2name=N'RuleSetType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Standard primary identity key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdpeVersion', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Current version number...for example:

1    data.zip
2    data.zip' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdpeVersion', @level2type=N'COLUMN',@level2name=N'Version'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Stored object type (1 = Attribute, 2 = DataSource, 3 = Rule, 4 = Key)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdpeVersion', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary key of the stored object, if data source is stored, this will be IdpeDataSource.Id, if rule is stored, this will be IdpeRule.Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdpeVersion', @level2type=N'COLUMN',@level2name=N'ReferenceId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The zipped object' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdpeVersion', @level2type=N'COLUMN',@level2name=N'Data'
GO
USE [master]
GO
ALTER DATABASE [idpe] SET  READ_WRITE 
GO
