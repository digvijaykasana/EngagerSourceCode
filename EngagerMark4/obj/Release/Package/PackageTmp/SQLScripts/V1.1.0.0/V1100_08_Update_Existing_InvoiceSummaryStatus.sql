DROP TABLE #TempAllWorkOrders;
DROP TABLE #TempBilledWorkOrders;

SELECT si.Id AS SalesInvoiceSummaryId, COUNT(so.Id) AS WorkOrderCount
INTO #TempAllWorkOrders
FROM [SOP].[Tb_SalesInvoiceSummary] si
INNER JOIN [SOP].[Tb_SalesInvoiceSummaryDetails] siDs ON si.Id = siDs.SalesInvoiceSummaryId
INNER JOIN [SOP].[Tb_SalesOrder] so ON siDs.WorkOrderId = so.Id
GROUP BY si.Id

SELECT si.Id AS SalesInvoiceSummaryId, COUNT(so.Id)  AS BilledWorkOrderCount  
INTO #TempBilledWorkOrders
FROM [SOP].[Tb_SalesInvoiceSummary] si
INNER JOIN [SOP].[Tb_SalesInvoiceSummaryDetails] siDs ON si.Id = siDs.SalesInvoiceSummaryId
INNER JOIN [SOP].[Tb_SalesOrder] so ON siDs.WorkOrderId = so.Id
WHERE so.Status = 100
GROUP BY si.Id

UPDATE [SOP].[Tb_SalesInvoiceSummary]
SET [Id3] = 1
WHERE [Id] IN 
(SELECT allWO.SalesInvoiceSummaryId FROM #TempAllWorkOrders allWO
INNER JOIN #TempBilledWorkOrders billedWO ON allWO.SalesInvoiceSummaryId = billedWO.SalesInvoiceSummaryId
AND allWO.WorkOrderCount = billedWO.BilledWorkOrderCount)