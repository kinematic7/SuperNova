USE [Spiral]
GO

/****** Object:  Table [dbo].[Logins]    Script Date: 6/4/2020 2:09:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Logins](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LoginId] [nvarchar](200) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Token] [nvarchar](max) NULL,
	[LastModified] [datetime] NULL,
 CONSTRAINT [PK_dbo.Logins] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

