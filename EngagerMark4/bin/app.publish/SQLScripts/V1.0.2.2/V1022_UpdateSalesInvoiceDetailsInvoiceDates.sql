UPDATE [SOP].[Tb_SalesInvoice]
SET InvoiceDate = (SELECT InvoiceDate FROM [SOP].[Tb_SalesInvoiceSummary] si WHERE [SOP].[Tb_SalesInvoice].SalesInvoiceSummaryId = si.Id)