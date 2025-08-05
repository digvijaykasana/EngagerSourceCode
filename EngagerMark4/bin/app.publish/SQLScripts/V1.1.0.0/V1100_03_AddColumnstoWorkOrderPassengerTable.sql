ALTER TABLE  [SOP].[Tb_WorkOrderPassenger]
ADD IsSigned int not null default 0;

ALTER TABLE [SOP].[Tb_WorkOrderPassenger]
ADD ServiceJobId bigint null;