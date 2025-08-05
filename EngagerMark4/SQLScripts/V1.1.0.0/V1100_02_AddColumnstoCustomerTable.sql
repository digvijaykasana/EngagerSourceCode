ALTER TABLE [Customer].[Tb_Customer]
ADD LetterheadId bigint null;

ALTER TABLE  [Customer].[Tb_Customer]
ADD TransferVoucherFormat int not null default 0;

ALTER TABLE  [Customer].[Tb_Customer]
ADD TFRequireAllPassSignatures bit not null default 0;
