UPDATE SOP.Tb_SalesOrder SET ShortText3='0' WHERE ShortText3 is null;
 
UPDATE SOP.Tb_SalesOrder SET SalesInvoiceSummaryId = CAST(ShortText3 AS bigint);

UPDATE SOP.Tb_SalesInvoice SET ShortText2='0' WHERE ShortText2 is null;

UPDATE SOP.Tb_SalesInvoice SET SalesInvoiceSummaryId = CAST(ShortText2 AS bigint);
