ALTER TABLE [SOP].[Tb_SalesInvoiceSummary]
ADD CustomerId bigint not null default 0;

ALTER TABLE [SOP].[Tb_SalesInvoiceSummary]
ADD VesselId bigint not null default 0;