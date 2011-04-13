USE [DB_LTR]
GO
/****** Object:  Table [dbo].[T_RESULT]    Script Date: 04/14/2011 02:29:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_RESULT](
	[RESULT_ID] [nvarchar](50) NOT NULL,
	[RESULT_DATE] [datetime] NULL,
	[RESULT_STATUS] [nvarchar](50) NULL,
	[RESULT_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [varchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_T_RESULT] PRIMARY KEY CLUSTERED 
(
	[RESULT_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[M_GAME]    Script Date: 04/14/2011 02:29:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[M_GAME](
	[GAME_ID] [nvarchar](50) NOT NULL,
	[GAME_NAME] [nvarchar](50) NULL,
	[GAME_PRIZE] [numeric](18, 5) NULL,
	[GAME_STATUS] [nvarchar](50) NULL,
	[GAME_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [varchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_GAME] PRIMARY KEY CLUSTERED 
(
	[GAME_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[M_AGENT]    Script Date: 04/14/2011 02:29:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[M_AGENT](
	[AGENT_ID] [nvarchar](50) NOT NULL,
	[AGENT_NAME] [nvarchar](50) NULL,
	[AGENT_STATUS] [nvarchar](50) NULL,
	[AGENT_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [varchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_AGENT] PRIMARY KEY CLUSTERED 
(
	[AGENT_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_SALES_DET]    Script Date: 04/14/2011 02:29:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_SALES_DET](
	[SALES_DET_ID] [nvarchar](50) NOT NULL,
	[SALES_ID] [nvarchar](50) NULL,
	[GAME_ID] [nvarchar](50) NULL,
	[SALES_DET_NUMBER] [nvarchar](50) NULL,
	[SALES_DET_VALUE] [numeric](18, 5) NULL,
	[SALES_DET_COMM] [numeric](18, 5) NULL,
	[SALES_DET_STATUS] [nvarchar](50) NULL,
	[SALES_DET_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [varchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_T_SALES_DET] PRIMARY KEY CLUSTERED 
(
	[SALES_DET_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_RESULT_DET]    Script Date: 04/14/2011 02:29:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_RESULT_DET](
	[RESULT_DET_ID] [nvarchar](50) NOT NULL,
	[RESULT_ID] [nvarchar](50) NULL,
	[GAME_ID] [nvarchar](50) NULL,
	[RESULT_DET_ORDER_NO] [int] NULL,
	[RESULT_DET_NUMBER] [nvarchar](50) NULL,
	[RESULT_DET_STATUS] [nvarchar](50) NULL,
	[RESULT_DET_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [varchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_T_RESULT_DET] PRIMARY KEY CLUSTERED 
(
	[RESULT_DET_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[M_AGENT_COMM]    Script Date: 04/14/2011 02:29:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[M_AGENT_COMM](
	[COMM_ID] [nvarchar](50) NOT NULL,
	[AGENT_ID] [nvarchar](50) NULL,
	[GAME_ID] [nvarchar](50) NULL,
	[COMM_VALUE] [numeric](18, 5) NULL,
	[COMM_STATUS] [nvarchar](50) NULL,
	[COMM_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [varchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_AGENT_COMM] PRIMARY KEY CLUSTERED 
(
	[COMM_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_SALES]    Script Date: 04/14/2011 02:29:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_SALES](
	[SALES_ID] [nvarchar](50) NOT NULL,
	[SALES_NO] [nvarchar](50) NULL,
	[AGENT_ID] [nvarchar](50) NULL,
	[SALES_DATE] [datetime] NULL,
	[SALES_TOTAL] [numeric](18, 5) NULL,
	[SALES_MUST_PAID] [numeric](18, 5) NULL,
	[SALES_STATUS] [nvarchar](50) NULL,
	[SALES_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [varchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_T_SALES] PRIMARY KEY CLUSTERED 
(
	[SALES_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  ForeignKey [FK_M_AGENT_COMM_M_AGENT]    Script Date: 04/14/2011 02:29:25 ******/
ALTER TABLE [dbo].[M_AGENT_COMM]  WITH CHECK ADD  CONSTRAINT [FK_M_AGENT_COMM_M_AGENT] FOREIGN KEY([AGENT_ID])
REFERENCES [dbo].[M_AGENT] ([AGENT_ID])
GO
ALTER TABLE [dbo].[M_AGENT_COMM] CHECK CONSTRAINT [FK_M_AGENT_COMM_M_AGENT]
GO
/****** Object:  ForeignKey [FK_M_AGENT_COMM_M_GAME]    Script Date: 04/14/2011 02:29:25 ******/
ALTER TABLE [dbo].[M_AGENT_COMM]  WITH CHECK ADD  CONSTRAINT [FK_M_AGENT_COMM_M_GAME] FOREIGN KEY([GAME_ID])
REFERENCES [dbo].[M_GAME] ([GAME_ID])
GO
ALTER TABLE [dbo].[M_AGENT_COMM] CHECK CONSTRAINT [FK_M_AGENT_COMM_M_GAME]
GO
/****** Object:  ForeignKey [FK_T_RESULT_DET_M_GAME]    Script Date: 04/14/2011 02:29:25 ******/
ALTER TABLE [dbo].[T_RESULT_DET]  WITH CHECK ADD  CONSTRAINT [FK_T_RESULT_DET_M_GAME] FOREIGN KEY([GAME_ID])
REFERENCES [dbo].[M_GAME] ([GAME_ID])
GO
ALTER TABLE [dbo].[T_RESULT_DET] CHECK CONSTRAINT [FK_T_RESULT_DET_M_GAME]
GO
/****** Object:  ForeignKey [FK_T_RESULT_DET_T_RESULT]    Script Date: 04/14/2011 02:29:25 ******/
ALTER TABLE [dbo].[T_RESULT_DET]  WITH CHECK ADD  CONSTRAINT [FK_T_RESULT_DET_T_RESULT] FOREIGN KEY([RESULT_ID])
REFERENCES [dbo].[T_RESULT] ([RESULT_ID])
GO
ALTER TABLE [dbo].[T_RESULT_DET] CHECK CONSTRAINT [FK_T_RESULT_DET_T_RESULT]
GO
/****** Object:  ForeignKey [FK_T_SALES_M_AGENT]    Script Date: 04/14/2011 02:29:25 ******/
ALTER TABLE [dbo].[T_SALES]  WITH CHECK ADD  CONSTRAINT [FK_T_SALES_M_AGENT] FOREIGN KEY([AGENT_ID])
REFERENCES [dbo].[M_AGENT] ([AGENT_ID])
GO
ALTER TABLE [dbo].[T_SALES] CHECK CONSTRAINT [FK_T_SALES_M_AGENT]
GO
/****** Object:  ForeignKey [FK_T_SALES_DET_M_GAME]    Script Date: 04/14/2011 02:29:25 ******/
ALTER TABLE [dbo].[T_SALES_DET]  WITH CHECK ADD  CONSTRAINT [FK_T_SALES_DET_M_GAME] FOREIGN KEY([GAME_ID])
REFERENCES [dbo].[M_GAME] ([GAME_ID])
GO
ALTER TABLE [dbo].[T_SALES_DET] CHECK CONSTRAINT [FK_T_SALES_DET_M_GAME]
GO
/****** Object:  ForeignKey [FK_T_SALES_DET_T_SALES]    Script Date: 04/14/2011 02:29:25 ******/
ALTER TABLE [dbo].[T_SALES_DET]  WITH CHECK ADD  CONSTRAINT [FK_T_SALES_DET_T_SALES] FOREIGN KEY([SALES_ID])
REFERENCES [dbo].[T_SALES] ([SALES_ID])
GO
ALTER TABLE [dbo].[T_SALES_DET] CHECK CONSTRAINT [FK_T_SALES_DET_T_SALES]
GO
