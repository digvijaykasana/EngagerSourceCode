ALTER TABLE SOP.Tb_SalesOrder
ADD SalesInvoiceSummaryId bigint default 0;

ALTER TABLE SOP.Tb_SalesInvoice
ADD SalesInvoiceSummaryId bigint default 0;
