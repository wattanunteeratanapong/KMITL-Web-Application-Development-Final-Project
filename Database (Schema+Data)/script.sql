USE [master]
GO
/****** Object:  Database [TogetaDB]    Script Date: 3/13/2025 9:52:15 AM ******/
CREATE DATABASE [TogetaDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TogetaDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\TogetaDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'TogetaDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\TogetaDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [TogetaDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TogetaDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TogetaDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TogetaDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TogetaDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TogetaDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TogetaDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [TogetaDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [TogetaDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TogetaDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TogetaDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TogetaDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TogetaDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TogetaDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TogetaDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TogetaDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TogetaDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [TogetaDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TogetaDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TogetaDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TogetaDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TogetaDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TogetaDB] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [TogetaDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TogetaDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [TogetaDB] SET  MULTI_USER 
GO
ALTER DATABASE [TogetaDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TogetaDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TogetaDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TogetaDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [TogetaDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [TogetaDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [TogetaDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [TogetaDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [TogetaDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 3/13/2025 9:52:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 3/13/2025 9:52:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EventId] [int] NOT NULL,
	[SenderId] [int] NOT NULL,
	[ReceiverId] [int] NOT NULL,
	[Content] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[Creditscore] [int] NOT NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Events]    Script Date: 3/13/2025 9:52:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Events](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[StartDateTime] [datetime2](7) NOT NULL,
	[location] [nvarchar](max) NOT NULL,
	[Participants] [int] NOT NULL,
	[Tag] [nvarchar](max) NOT NULL,
	[Detail] [nvarchar](max) NOT NULL,
	[Imagepath] [nvarchar](max) NOT NULL,
	[CreatorId] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ClosedDate] [datetime2](7) NULL,
	[EndDateTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventUsers]    Script Date: 3/13/2025 9:52:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventUsers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[EventId] [int] NOT NULL,
 CONSTRAINT [PK_EventUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notifications]    Script Date: 3/13/2025 9:52:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EventId] [int] NULL,
	[UserId] [int] NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[IsRead] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 3/13/2025 9:52:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[Username] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[BirthDate] [datetime2](7) NOT NULL,
	[Gender] [nvarchar](max) NOT NULL,
	[RequestId] [nvarchar](max) NULL,
	[ShowRequestId] [bit] NOT NULL,
	[Bio] [nvarchar](max) NOT NULL,
	[CoverImagePath] [nvarchar](max) NOT NULL,
	[CreditScore] [real] NOT NULL,
	[Interests] [nvarchar](max) NOT NULL,
	[ProfileImagePath] [nvarchar](max) NOT NULL,
	[CommentCount] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250208062955_addTogetaDB', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250216093453_addEventTable', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250220093127_addProfileDB', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250220112814_FixEventModel', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250220115031_update', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250224122407_InitialCreate', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250226121803_AdjustUsersTable', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250226122029_AddCreatedAtToEvents', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250226180715_AddClosedDate', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250227125532_AddEventUsersRelation', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250301072559_AddEventUsersNav', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250301103045_AddEventTimeRange', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250302134940_MigrationName', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250302135503_beginning', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250303152209_MakeEventIdNullable', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250309203804_MakeEventIdNullableInNotifications', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250310084800_AddCommentCountToUser', N'9.0.2')
GO
SET IDENTITY_INSERT [dbo].[Comments] ON 

INSERT [dbo].[Comments] ([Id], [EventId], [SenderId], [ReceiverId], [Content], [CreatedAt], [Creditscore]) VALUES (44, 1046, 23, 21, N'That was FUNNN!', CAST(N'2025-03-11T22:09:49.0373459' AS DateTime2), 5)
INSERT [dbo].[Comments] ([Id], [EventId], [SenderId], [ReceiverId], [Content], [CreatedAt], [Creditscore]) VALUES (45, 1046, 23, 19, N'That was FUNNN!', CAST(N'2025-03-11T22:09:51.8592511' AS DateTime2), 5)
INSERT [dbo].[Comments] ([Id], [EventId], [SenderId], [ReceiverId], [Content], [CreatedAt], [Creditscore]) VALUES (46, 1046, 23, 20, N'That was FUNNN!', CAST(N'2025-03-11T22:09:54.1607404' AS DateTime2), 5)
INSERT [dbo].[Comments] ([Id], [EventId], [SenderId], [ReceiverId], [Content], [CreatedAt], [Creditscore]) VALUES (47, 1046, 19, 21, N'That movie is Fire!', CAST(N'2025-03-11T22:10:46.9203363' AS DateTime2), 5)
INSERT [dbo].[Comments] ([Id], [EventId], [SenderId], [ReceiverId], [Content], [CreatedAt], [Creditscore]) VALUES (48, 1046, 19, 20, N'That movie is Fire!', CAST(N'2025-03-11T22:10:50.7106324' AS DateTime2), 5)
INSERT [dbo].[Comments] ([Id], [EventId], [SenderId], [ReceiverId], [Content], [CreatedAt], [Creditscore]) VALUES (49, 1046, 19, 23, N'That movie is Fire!', CAST(N'2025-03-11T22:10:54.5417526' AS DateTime2), 5)
INSERT [dbo].[Comments] ([Id], [EventId], [SenderId], [ReceiverId], [Content], [CreatedAt], [Creditscore]) VALUES (50, 1046, 20, 19, N'Yeah that movie is FUN MAN!', CAST(N'2025-03-11T22:12:45.7829077' AS DateTime2), 3)
INSERT [dbo].[Comments] ([Id], [EventId], [SenderId], [ReceiverId], [Content], [CreatedAt], [Creditscore]) VALUES (51, 1046, 20, 23, N'Yeah that movie is FUN MAN!', CAST(N'2025-03-11T22:12:48.6125326' AS DateTime2), 4)
INSERT [dbo].[Comments] ([Id], [EventId], [SenderId], [ReceiverId], [Content], [CreatedAt], [Creditscore]) VALUES (52, 1046, 20, 21, N'Yeah that movie is FUN MAN!', CAST(N'2025-03-11T22:12:52.3967224' AS DateTime2), 2)
INSERT [dbo].[Comments] ([Id], [EventId], [SenderId], [ReceiverId], [Content], [CreatedAt], [Creditscore]) VALUES (53, 1046, 21, 19, N'Yeah it fun!', CAST(N'2025-03-11T22:14:20.4838661' AS DateTime2), 5)
INSERT [dbo].[Comments] ([Id], [EventId], [SenderId], [ReceiverId], [Content], [CreatedAt], [Creditscore]) VALUES (54, 1046, 21, 20, N'Yeah it fun!', CAST(N'2025-03-11T22:14:23.5050955' AS DateTime2), 3)
INSERT [dbo].[Comments] ([Id], [EventId], [SenderId], [ReceiverId], [Content], [CreatedAt], [Creditscore]) VALUES (55, 1046, 21, 23, N'Yeah it fun!', CAST(N'2025-03-11T22:14:27.7691843' AS DateTime2), 1)
SET IDENTITY_INSERT [dbo].[Comments] OFF
GO
SET IDENTITY_INSERT [dbo].[Events] ON 

INSERT [dbo].[Events] ([Id], [Name], [StartDateTime], [location], [Participants], [Tag], [Detail], [Imagepath], [CreatorId], [CreatedAt], [ClosedDate], [EndDateTime]) VALUES (1040, N'Hollywood Party', CAST(N'2025-03-23T21:42:00.0000000' AS DateTime2), N'Hollywood', 6, N'🎬 Movies & TV Shows', N'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Itaque ipsa deserunt hic velit quos ducimus voluptatem eos officiis!', N'/uploads/bojack.png', 19, CAST(N'2025-03-11T21:44:51.2465683' AS DateTime2), CAST(N'2025-03-22T21:42:00.0000000' AS DateTime2), CAST(N'2025-03-29T21:42:00.0000000' AS DateTime2))
INSERT [dbo].[Events] ([Id], [Name], [StartDateTime], [location], [Participants], [Tag], [Detail], [Imagepath], [CreatorId], [CreatedAt], [ClosedDate], [EndDateTime]) VALUES (1043, N'Chilling', CAST(N'2025-03-23T21:49:00.0000000' AS DateTime2), N'Hollywood', 7, N'🎲 Board Games', N'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Itaque ipsa deserunt hic velit quos ducimus voluptatem eos officiis!', N'/uploads/bojack-horseman.webp', 19, CAST(N'2025-03-11T21:50:08.0834895' AS DateTime2), CAST(N'2025-03-22T21:49:00.0000000' AS DateTime2), CAST(N'2025-03-29T21:49:00.0000000' AS DateTime2))
INSERT [dbo].[Events] ([Id], [Name], [StartDateTime], [location], [Participants], [Tag], [Detail], [Imagepath], [CreatorId], [CreatedAt], [ClosedDate], [EndDateTime]) VALUES (1044, N'Hollywood Sightseeing', CAST(N'2025-03-23T21:52:00.0000000' AS DateTime2), N'Hollywood', 6, N'✈️ Travel & Adventure', N'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Itaque ipsa deserunt hic velit quos ducimus voluptatem eos officiis!', N'/uploads/o-bojack-facebook.png', 23, CAST(N'2025-03-11T21:53:47.9538954' AS DateTime2), CAST(N'2025-03-22T21:52:00.0000000' AS DateTime2), CAST(N'2025-03-29T21:52:00.0000000' AS DateTime2))
INSERT [dbo].[Events] ([Id], [Name], [StartDateTime], [location], [Participants], [Tag], [Detail], [Imagepath], [CreatorId], [CreatedAt], [ClosedDate], [EndDateTime]) VALUES (1045, N'Vietnam Trip', CAST(N'2025-03-26T22:02:00.0000000' AS DateTime2), N'Vietnam', 3, N'✈️ Travel & Adventure', N'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Itaque ipsa deserunt hic velit quos ducimus voluptatem eos officiis!', N'/uploads/Diane-as-Tourist.png', 23, CAST(N'2025-03-11T22:03:03.3799820' AS DateTime2), CAST(N'2025-03-25T22:02:00.0000000' AS DateTime2), CAST(N'2025-03-31T22:02:00.0000000' AS DateTime2))
INSERT [dbo].[Events] ([Id], [Name], [StartDateTime], [location], [Participants], [Tag], [Detail], [Imagepath], [CreatorId], [CreatedAt], [ClosedDate], [EndDateTime]) VALUES (1046, N'Movie', CAST(N'2025-03-27T22:05:00.0000000' AS DateTime2), N'Hollywood', 4, N'🎬 Movies & TV Shows', N'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Itaque ipsa deserunt hic velit quos ducimus voluptatem eos officiis!', N'/uploads/OpEd-BoJackHorseman_Season6_Episode10_00_23_01_01 (1).png', 21, CAST(N'2025-03-11T22:07:09.7558548' AS DateTime2), CAST(N'2025-03-11T22:08:37.5815633' AS DateTime2), CAST(N'2025-03-29T22:06:00.0000000' AS DateTime2))
INSERT [dbo].[Events] ([Id], [Name], [StartDateTime], [location], [Participants], [Tag], [Detail], [Imagepath], [CreatorId], [CreatedAt], [ClosedDate], [EndDateTime]) VALUES (1047, N'Oscar Nominations', CAST(N'2025-03-25T09:36:00.0000000' AS DateTime2), N'Hollywood', 100, N'🎬 Movies & TV Shows', N'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.', N'/uploads/272d91377ec54301f73325a2573090faa4-21-bojack-10.2x.h473.w710.png', 22, CAST(N'2025-03-13T09:37:16.6988420' AS DateTime2), CAST(N'2025-03-24T09:37:00.0000000' AS DateTime2), CAST(N'2025-03-26T09:37:00.0000000' AS DateTime2))
INSERT [dbo].[Events] ([Id], [Name], [StartDateTime], [location], [Participants], [Tag], [Detail], [Imagepath], [CreatorId], [CreatedAt], [ClosedDate], [EndDateTime]) VALUES (1048, N'Movie Casting', CAST(N'2025-03-26T09:38:00.0000000' AS DateTime2), N'Hollywood', 20, N'🎬 Movies & TV Shows', N'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna.', N'/uploads/bojack-horseman-influences.png', 21, CAST(N'2025-03-13T09:39:11.5548929' AS DateTime2), CAST(N'2025-03-25T09:38:00.0000000' AS DateTime2), CAST(N'2025-03-27T09:38:00.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Events] OFF
GO
SET IDENTITY_INSERT [dbo].[EventUsers] ON 

INSERT [dbo].[EventUsers] ([Id], [UserId], [EventId]) VALUES (1099, 19, 1040)
INSERT [dbo].[EventUsers] ([Id], [UserId], [EventId]) VALUES (1102, 19, 1043)
INSERT [dbo].[EventUsers] ([Id], [UserId], [EventId]) VALUES (1103, 23, 1044)
INSERT [dbo].[EventUsers] ([Id], [UserId], [EventId]) VALUES (1104, 19, 1044)
INSERT [dbo].[EventUsers] ([Id], [UserId], [EventId]) VALUES (1105, 20, 1044)
INSERT [dbo].[EventUsers] ([Id], [UserId], [EventId]) VALUES (1106, 21, 1044)
INSERT [dbo].[EventUsers] ([Id], [UserId], [EventId]) VALUES (1107, 22, 1044)
INSERT [dbo].[EventUsers] ([Id], [UserId], [EventId]) VALUES (1108, 20, 1040)
INSERT [dbo].[EventUsers] ([Id], [UserId], [EventId]) VALUES (1109, 21, 1040)
INSERT [dbo].[EventUsers] ([Id], [UserId], [EventId]) VALUES (1110, 22, 1040)
INSERT [dbo].[EventUsers] ([Id], [UserId], [EventId]) VALUES (1111, 23, 1040)
INSERT [dbo].[EventUsers] ([Id], [UserId], [EventId]) VALUES (1113, 23, 1043)
INSERT [dbo].[EventUsers] ([Id], [UserId], [EventId]) VALUES (1114, 23, 1045)
INSERT [dbo].[EventUsers] ([Id], [UserId], [EventId]) VALUES (1115, 21, 1046)
INSERT [dbo].[EventUsers] ([Id], [UserId], [EventId]) VALUES (1116, 19, 1046)
INSERT [dbo].[EventUsers] ([Id], [UserId], [EventId]) VALUES (1117, 20, 1046)
INSERT [dbo].[EventUsers] ([Id], [UserId], [EventId]) VALUES (1118, 23, 1046)
INSERT [dbo].[EventUsers] ([Id], [UserId], [EventId]) VALUES (1119, 22, 1047)
INSERT [dbo].[EventUsers] ([Id], [UserId], [EventId]) VALUES (1120, 21, 1048)
SET IDENTITY_INSERT [dbo].[EventUsers] OFF
GO
SET IDENTITY_INSERT [dbo].[Notifications] ON 

INSERT [dbo].[Notifications] ([Id], [EventId], [UserId], [Message], [IsRead], [CreatedAt]) VALUES (1086, 1044, 23, N'111 has joined your event: Hollywood Sightseeing', 0, CAST(N'2025-03-11T21:55:08.0752484' AS DateTime2))
INSERT [dbo].[Notifications] ([Id], [EventId], [UserId], [Message], [IsRead], [CreatedAt]) VALUES (1087, 1044, 23, N'222 has joined your event: Hollywood Sightseeing', 0, CAST(N'2025-03-11T21:55:17.5453192' AS DateTime2))
INSERT [dbo].[Notifications] ([Id], [EventId], [UserId], [Message], [IsRead], [CreatedAt]) VALUES (1088, 1044, 23, N'333 has joined your event: Hollywood Sightseeing', 0, CAST(N'2025-03-11T21:55:27.2250219' AS DateTime2))
INSERT [dbo].[Notifications] ([Id], [EventId], [UserId], [Message], [IsRead], [CreatedAt]) VALUES (1089, 1044, 23, N'444 has joined your event: Hollywood Sightseeing', 0, CAST(N'2025-03-11T21:55:40.3458959' AS DateTime2))
INSERT [dbo].[Notifications] ([Id], [EventId], [UserId], [Message], [IsRead], [CreatedAt]) VALUES (1090, 1040, 19, N'222 has joined your event: Hollywood Party', 0, CAST(N'2025-03-11T21:56:08.0836291' AS DateTime2))
INSERT [dbo].[Notifications] ([Id], [EventId], [UserId], [Message], [IsRead], [CreatedAt]) VALUES (1091, 1040, 19, N'333 has joined your event: Hollywood Party', 0, CAST(N'2025-03-11T21:56:15.6170200' AS DateTime2))
INSERT [dbo].[Notifications] ([Id], [EventId], [UserId], [Message], [IsRead], [CreatedAt]) VALUES (1092, 1040, 19, N'444 has joined your event: Hollywood Party', 0, CAST(N'2025-03-11T21:56:26.5268979' AS DateTime2))
INSERT [dbo].[Notifications] ([Id], [EventId], [UserId], [Message], [IsRead], [CreatedAt]) VALUES (1093, 1040, 19, N'555 has joined your event: Hollywood Party', 0, CAST(N'2025-03-11T21:56:34.9048088' AS DateTime2))
INSERT [dbo].[Notifications] ([Id], [EventId], [UserId], [Message], [IsRead], [CreatedAt]) VALUES (1094, NULL, 19, N'222 has joined your event: Running', 0, CAST(N'2025-03-11T21:57:48.0014534' AS DateTime2))
INSERT [dbo].[Notifications] ([Id], [EventId], [UserId], [Message], [IsRead], [CreatedAt]) VALUES (1095, 1043, 19, N'555 has joined your event: Chilling', 0, CAST(N'2025-03-11T21:58:12.1253255' AS DateTime2))
INSERT [dbo].[Notifications] ([Id], [EventId], [UserId], [Message], [IsRead], [CreatedAt]) VALUES (1096, 1046, 21, N'111 has joined your event: Movie', 0, CAST(N'2025-03-11T22:07:43.9681595' AS DateTime2))
INSERT [dbo].[Notifications] ([Id], [EventId], [UserId], [Message], [IsRead], [CreatedAt]) VALUES (1097, 1046, 21, N'222 has joined your event: Movie', 0, CAST(N'2025-03-11T22:07:57.8305336' AS DateTime2))
INSERT [dbo].[Notifications] ([Id], [EventId], [UserId], [Message], [IsRead], [CreatedAt]) VALUES (1098, 1046, 21, N'555 has joined your event: Movie', 0, CAST(N'2025-03-11T22:08:37.5843790' AS DateTime2))
INSERT [dbo].[Notifications] ([Id], [EventId], [UserId], [Message], [IsRead], [CreatedAt]) VALUES (1099, NULL, 20, N'Event ''Running'' has been cancelled by the host.', 0, CAST(N'2025-03-12T16:32:37.9430448' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Notifications] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Username], [Password], [Email], [BirthDate], [Gender], [RequestId], [ShowRequestId], [Bio], [CoverImagePath], [CreditScore], [Interests], [ProfileImagePath], [CommentCount]) VALUES (19, N'Bojack', N'Horseman', N'111', N'111', N'66011464@kmitl.ac.th', CAST(N'2025-03-01T00:00:00.0000000' AS DateTime2), N'Male', NULL, 0, N'This is Bojack..... Horseman Obviously.', N'/uploads/bojack - Copy.png', 4.33333349, N'🚗 Cars & Motorcycles, 💰 Finance & Investing, 🎬 Movies & TV Shows', N'/uploads/1_VcVML7BKIeUfZD_45DvkNg - Copy.png', 3)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Username], [Password], [Email], [BirthDate], [Gender], [RequestId], [ShowRequestId], [Bio], [CoverImagePath], [CreditScore], [Interests], [ProfileImagePath], [CommentCount]) VALUES (20, N'Todd', N'Chavez', N'222', N'222', N'66011464@kmitl.ac.th', CAST(N'2025-03-01T00:00:00.0000000' AS DateTime2), N'Male', NULL, 0, N'Hi, this is Todd..... by the way.', N'/uploads/asking-your-opinion-on-a-bojack-horseman-character-every-v0-lr1rmpgs87x91.webp', 4.33333349, N'📖 Anime & Manga, 🎲 Board Games, 🎮 Video Games', N'/uploads/d268eaf1cb956c02eba2edf728663887ce-19-bojack-todd.rsocial.w1200.webp', 3)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Username], [Password], [Email], [BirthDate], [Gender], [RequestId], [ShowRequestId], [Bio], [CoverImagePath], [CreditScore], [Interests], [ProfileImagePath], [CommentCount]) VALUES (21, N'Princess', N'Carolyn', N'333', N'333', N'66011464@kmitl.ac.th', CAST(N'2025-03-01T00:00:00.0000000' AS DateTime2), N'Female', NULL, 0, N'Hi am Princess Carolyn...... a famous manager/agent.', N'/uploads/AMBITIOUS (1).png', 4, N'💄 Fashion & Beauty, 🍽️ Food & Cooking, 💰 Finance & Investing', N'/uploads/images.jpg', 3)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Username], [Password], [Email], [BirthDate], [Gender], [RequestId], [ShowRequestId], [Bio], [CoverImagePath], [CreditScore], [Interests], [ProfileImagePath], [CommentCount]) VALUES (22, N'Mr', N'Peanutbutter', N'444', N'444', N'66011464@kmitl.ac.th', CAST(N'2025-03-01T00:00:00.0000000' AS DateTime2), N'Male', NULL, 0, N'This is Mr.Peanutbutter nice to meet you guys.', N'/uploads/cx9xr5hyzyg91.jpg', 0, N'🎬 Movies & TV Shows, 🎶 Music, 🐶 Pets & Animals', N'/uploads/Mr.-Peanutbutter-Costume_600x.webp', 0)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Username], [Password], [Email], [BirthDate], [Gender], [RequestId], [ShowRequestId], [Bio], [CoverImagePath], [CreditScore], [Interests], [ProfileImagePath], [CommentCount]) VALUES (23, N'Diane', N'Nguyen', N'555', N'555', N'66011464@kmitl.ac.th', CAST(N'2025-03-01T00:00:00.0000000' AS DateTime2), N'Female', NULL, 0, N'This is Diane...... Nguyen by the way.', N'/uploads/1_kkXjMMWpRHkV7CIfCGXugA.jpg', 3.33333325, N'📖 Anime & Manga, 🎲 Board Games, 📚 Books & Literature', N'/uploads/99659205445efa5dc5c1176568c368677755b3a3_hq.jpg', 3)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
/****** Object:  Index [IX_Comments_EventId]    Script Date: 3/13/2025 9:52:15 AM ******/
CREATE NONCLUSTERED INDEX [IX_Comments_EventId] ON [dbo].[Comments]
(
	[EventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_EventUsers_EventId]    Script Date: 3/13/2025 9:52:15 AM ******/
CREATE NONCLUSTERED INDEX [IX_EventUsers_EventId] ON [dbo].[EventUsers]
(
	[EventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_EventUsers_UserId]    Script Date: 3/13/2025 9:52:15 AM ******/
CREATE NONCLUSTERED INDEX [IX_EventUsers_UserId] ON [dbo].[EventUsers]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Notifications_EventId]    Script Date: 3/13/2025 9:52:15 AM ******/
CREATE NONCLUSTERED INDEX [IX_Notifications_EventId] ON [dbo].[Notifications]
(
	[EventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Notifications_UserId]    Script Date: 3/13/2025 9:52:15 AM ******/
CREATE NONCLUSTERED INDEX [IX_Notifications_UserId] ON [dbo].[Notifications]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Events] ADD  DEFAULT ((0)) FOR [CreatorId]
GO
ALTER TABLE [dbo].[Events] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Events] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [EndDateTime]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (N'') FOR [Bio]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (N'') FOR [CoverImagePath]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (N'') FOR [Interests]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (N'') FOR [ProfileImagePath]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [CommentCount]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Events_EventId] FOREIGN KEY([EventId])
REFERENCES [dbo].[Events] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Events_EventId]
GO
ALTER TABLE [dbo].[EventUsers]  WITH CHECK ADD  CONSTRAINT [FK_EventUsers_Events_EventId] FOREIGN KEY([EventId])
REFERENCES [dbo].[Events] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EventUsers] CHECK CONSTRAINT [FK_EventUsers_Events_EventId]
GO
ALTER TABLE [dbo].[EventUsers]  WITH CHECK ADD  CONSTRAINT [FK_EventUsers_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EventUsers] CHECK CONSTRAINT [FK_EventUsers_Users_UserId]
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_Events_EventId] FOREIGN KEY([EventId])
REFERENCES [dbo].[Events] ([Id])
GO
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_Events_EventId]
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_Users_UserId]
GO
USE [master]
GO
ALTER DATABASE [TogetaDB] SET  READ_WRITE 
GO
