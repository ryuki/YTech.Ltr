USE [DB_LTR]
GO

/****** Object:  Table [dbo].[M_GAME_PRIZE]    Script Date: 04/17/2011 12:38:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[M_GAME_PRIZE](
	[PRIZE_ID] [nvarchar](50) NOT NULL,
	[GAME_ID] [nvarchar](50) NULL,
	[PRIZE_NO_START] [int] NULL,
	[PRIZE_NO_END] [int] NULL,
	[PRIZE_VALUE] [nchar](10) NULL,
	[PRIZE_STATUS] [nvarchar](50) NULL,
	[PRIZE_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [varchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_GAME_PRIZE] PRIMARY KEY CLUSTERED 
(
	[PRIZE_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[M_GAME_PRIZE]  WITH CHECK ADD  CONSTRAINT [FK_M_GAME_PRIZE_M_GAME] FOREIGN KEY([GAME_ID])
REFERENCES [dbo].[M_GAME] ([GAME_ID])
GO

ALTER TABLE [dbo].[M_GAME_PRIZE] CHECK CONSTRAINT [FK_M_GAME_PRIZE_M_GAME]
GO

