DELETE FROM [SOP].[Tb_SalesInvoiceSummaryDetails]
WHERE Id NOT IN
(
SELECT siDetails.Id FROM [SOP].[Tb_SalesInvoiceSummaryDetails] siDetails
INNER JOIN [SOP].[Tb_SalesOrder] so ON siDetails.WorkOrderId = so.Id
WHERE siDetails.SalesInvoiceSummaryId = so.SalesInvoiceSummaryId
) 