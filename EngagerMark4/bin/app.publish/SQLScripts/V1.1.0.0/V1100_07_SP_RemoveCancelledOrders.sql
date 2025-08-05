/****** Object:  StoredProcedure [dbo].[SP_RemoveCancelledOrders]    Script Date: 22/8/2022 9:57:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_RemoveCancelledOrders]
(@RemovalPeriodMins INT)
AS
BEGIN
SET NOCOUNT ON

SELECT [Id] AS WorkOrderId,
[RefereneceNo] AS ReferenceNo
INTO #TempOrderTable
FROM [SOP].[Tb_SalesOrder] 
WHERE [Status] = -1 AND
[InvoiceId] IS NULL AND
[CreditNoteId] IS NULL AND
(DATEDIFF(second, [Modified], DATEADD(hour,8,GETUTCDATE())) / 60) > @RemovalPeriodMins

-- DELETE Order-related SalesInvoiceSummaryDetails 
DELETE SISD FROM [SOP].[Tb_SalesInvoiceSummaryDetails] SISD 
INNER JOIN #TempOrderTable T On SISD.WorkOrderId = T.WorkOrderId;

--DELETE Order-related CreditNote 
DELETE CN FROM [SOP].[Tb_CreditNote] CN 
INNER JOIN #TempOrderTable T On CN.[OrderId] = T.WorkOrderId;

-- DELETE Order-related SalesInvoice
DELETE SI FROM [SOP].[Tb_SalesInvoice] SI 
INNER JOIN #TempOrderTable T On SI.[WorkOrderId] = T.WorkOrderId;

-- DELETE Order-related WorkOrderAirportMeetingServices
DELETE WOAMS FROM [dbo].[WorkOrderAirportMeetingServices] WOAMS 
INNER JOIN #TempOrderTable T On WOAMS.[WorkOrderId] = T.WorkOrderId;

-- DELETE Order-related WorkOrderHistory
DELETE WOH FROM [SOP].[Tb_WorkOrderHistory] WOH 
INNER JOIN #TempOrderTable T On WOH.[WorkOrderId] = T.WorkOrderId;

-- DELETE Order-related WorkOrderPassenger
DELETE WOP FROM [SOP].[Tb_WorkOrderPassenger] WOP 
INNER JOIN #TempOrderTable T On WOP.[WorkOrderId] = T.WorkOrderId;

-- DELETE Order-related WorkOrderLocation
DELETE WOL FROM [SOP].[Tb_WorkOrderLocation] WOL 
INNER JOIN #TempOrderTable T On WOL.[WorkOrderId] = T.WorkOrderId;
 
 -- DELETE Order-related ServiceJob
DELETE SJ FROM [SOP].[Tb_ServiceJob] SJ 
INNER JOIN #TempOrderTable T On SJ.[WorkOrderId] = T.WorkOrderId;

 -- DELETE Work Order
DELETE SO FROM [SOP].[Tb_SalesOrder] SO 
INNER JOIN #TempOrderTable T On SO.[Id] = T.WorkOrderId;

IF(SELECT COUNT(WorkOrderId) FROM #TempOrderTable) > 0
	BEGIN

	DECLARE @val VARCHAR(max);
	DECLARE @currentDateTime DATETIME;
	SET @currentDateTime = DATEADD(HOUR,8,GETUTCDATE());
	SELECT @val = COALESCE(@val + '| ' + ReferenceNo, ReferenceNo) FROM #TempOrderTable SELECT @val;

	IF(@val IS NOT NULL) AND (LEN(@val) > 0)
		BEGIN
		INSERT INTO [Core].[Tb_Audit] (Description, StartProcessingTime, EndProcessingTime, Created, CreatedByName, ParentCompanyId, State, Type)
		VALUES (@val, @currentDateTime, @currentDateTime, @currentDateTime, 'SP_RemoveCancelledOrders', 1, 0, 2);
		END

	END

END
--SELECT DATEDIFF(hour, [Modified], GETDATE()), [Modified] FROM [SOP].[Tb_SalesOrder] WHERE [Status] = -1


EXEC SP_RemoveCancelledOrders 2114930

--DROP PROCEDURE SP_RemoveCancelledOrdersAfter48Hrs

--SELECT DATEADD(hour,8,GETUTCDATE()) as UTCNow, 
--DATEDIFF(month, [Modified], DATEADD(hour,8,GETUTCDATE())) as MonthVal,
--DATEDIFF(day, [Modified], DATEADD(hour,8,GETUTCDATE())) as DayVal, 
--DATEDIFF(second, [Modified], DATEADD(hour,8,GETUTCDATE())) / 3600 as HourVal, 
--DATEDIFF(minute, [Modified], DATEADD(hour,8,GETUTCDATE())) as MinVal,
--[Modified],
--[Id],
--[InvoiceId],
--[CreditNoteId],
--[SalesInvoiceSummaryId],
--[Status]
--FROM [SOP].[Tb_SalesOrder] 
--WHERE [Status] = -1
--ORDER BY [MinVal] DESC

--SELECT [Id] AS WorkOrderId,
--[RefereneceNo] AS ReferenceNo
--FROM [SOP].[Tb_SalesOrder] 
--WHERE [Status] = -1 AND
--[InvoiceId] IS NULL AND
--[CreditNoteId] IS NULL AND
--(DATEDIFF(second, [Modified], DATEADD(hour,8,GETUTCDATE())))/ 60 > 2065273

--SELECT * FROM [Core].[Tb_Audit] WHERE [Created] > '2022-08-22 00:01:00.000'