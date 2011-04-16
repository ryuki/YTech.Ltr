/****** Object:  Table [dbo].[M_GAME]    Script Date: 04/16/2011 19:16:14 ******/
INSERT [dbo].[M_GAME] ([GAME_ID], [GAME_NAME], [GAME_PRIZE], [GAME_STATUS], [GAME_DESC], [DATA_STATUS], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE]) VALUES (N'D2', N'D2', CAST(70.00000 AS Numeric(18, 5)), NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[M_GAME] ([GAME_ID], [GAME_NAME], [GAME_PRIZE], [GAME_STATUS], [GAME_DESC], [DATA_STATUS], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE]) VALUES (N'D3', N'D3', CAST(210.00000 AS Numeric(18, 5)), NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[M_GAME] ([GAME_ID], [GAME_NAME], [GAME_PRIZE], [GAME_STATUS], [GAME_DESC], [DATA_STATUS], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE]) VALUES (N'CB', N'COLOK BEBAS', CAST(1.50000 AS Numeric(18, 5)), NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[M_GAME] ([GAME_ID], [GAME_NAME], [GAME_PRIZE], [GAME_STATUS], [GAME_DESC], [DATA_STATUS], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE]) VALUES (N'CJ', N'COLOK JITU', CAST(1.50000 AS Numeric(18, 5)), NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[M_GAME] ([GAME_ID], [GAME_NAME], [GAME_PRIZE], [GAME_STATUS], [GAME_DESC], [DATA_STATUS], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE]) VALUES (N'D4', N'D4', CAST(1500.00000 AS Numeric(18, 5)), NULL, NULL, NULL, NULL, NULL, NULL, NULL)


USE [DB_LTR]
GO

/****** Object:  StoredProcedure [dbo].[SP_CALCULATE_PRIZE]    Script Date: 04/16/2011 22:20:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_CALCULATE_PRIZE]
	-- Add the parameters for the stored procedure here
	@SalesDetStatus nvarchar(50), 
	@SalesDate DateTime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	update det
set SALES_DET_STATUS = @SalesDetStatus
	,SALES_DET_PRIZE = g.GAME_PRIZE
from dbo.T_SALES_DET det, dbo.T_SALES s, dbo.T_RESULT res, dbo.T_RESULT_DET res_det, dbo.M_GAME g
where s.SALES_DATE = res.RESULT_DATE
	and det.SALES_ID = s.SALES_ID
	and res.RESULT_ID = res_det.RESULT_ID
	and det.GAME_ID = res_det.GAME_ID
	and g.GAME_ID = res_det.GAME_ID
	and s.SALES_DATE = @SalesDate

END

GO


