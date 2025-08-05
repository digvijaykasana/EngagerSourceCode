/****** Object:  Trigger [SOP].[trg_InvoiceSummary_Status_Update]    Script Date: 22/8/2022 12:57:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  Trigger [svc].[trg_workOrderNo_Update]    Script Date: 8/15/2022 3:05:03 PM ******/
CREATE TRIGGER [SOP].[trg_InvoiceSummary_Status_Update]  ON [SOP].[Tb_SalesOrder]
AFTER UPDATE
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @CurrentInvoiceSummaryId bigint;
	DECLARE @AllWorkOrderCount int;
	DECLARE @BilledWorkOrderCount int;

	SELECT @CurrentInvoiceSummaryId =  si.Id FROM [SOP].[Tb_SalesInvoiceSummary] si
	INNER JOIN [SOP].[Tb_SalesInvoiceSummaryDetails] siD ON si.Id = siD.SalesInvoiceSummaryId
	INNER JOIN inserted i ON siD.WorkOrderId = i.Id

	IF(@CurrentInvoiceSummaryId = 0)
	BEGIN

	UPDATE [SOP].[Tb_SalesOrder]
	SET [ShortText9] = 'NO INVOICE FOUND' 
	FROM [SOP].[Tb_SalesOrder] so
	INNER JOIN inserted i on so.Id = i.Id;

	END
	ELSE
	BEGIN	

	--UPDATE [SOP].[Tb_SalesOrder]
	--SET [ShortText5] = 'INVOICE FOUND - ' + CAST(@CurrentInvoiceSummaryId AS nvarchar(MAX)),
	--[Id2] = @CurrentInvoiceSummaryId
	--FROM [SOP].[Tb_SalesOrder] so
	--INNER JOIN inserted i on so.Id = i.Id;
	
	SELECT @AllWorkOrderCount = COUNT(so.Id) FROM [SOP].[Tb_SalesOrder] so
	INNER JOIN [SOP].[Tb_SalesInvoiceSummaryDetails] siD ON so.Id = siD.WorkOrderId
	WHERE siD.SalesInvoiceSummaryId = @CurrentInvoiceSummaryId;
	
	SELECT @BilledWorkOrderCount = COUNT(so.Id) FROM [SOP].[Tb_SalesOrder] so
	INNER JOIN [SOP].[Tb_SalesInvoiceSummaryDetails] siD ON so.Id = siD.WorkOrderId
	WHERE siD.SalesInvoiceSummaryId = @CurrentInvoiceSummaryId
	AND so.Status = 100;

	--UPDATE [SOP].[Tb_SalesInvoiceSummary]
	--SET [ShortText5] = 'ALL COUNT - ' + CAST(@AllWorkOrderCount AS nvarchar(MAX))  + '; BILLED INVOICE COUNT - ' + CAST(@BilledWorkOrderCount AS nvarchar(MAX)) 
	--WHERE Id = @CurrentInvoiceSummaryId;
	
	IF(@AllWorkOrderCount = @BilledWorkOrderCount)
	BEGIN
	UPDATE [SOP].[Tb_SalesInvoiceSummary]
	SET [Id3] = 1 
	WHERE [Id] = @CurrentInvoiceSummaryId
	
	END
	ELSE
	BEGIN

	UPDATE [SOP].[Tb_SalesInvoiceSummary]
	SET [Id3] = 0 
	WHERE [Id] = @CurrentInvoiceSummaryId

	END

	END	
END
