/****** Object:  Table [SOP].[Tb_CreditNoteSummary]    Script Date: 7/4/2019 5:13:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [SOP].[Tb_CreditNoteSummary](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ReferenceNo] [nvarchar](50) NOT NULL,
	[CreditNoteDate] [datetime] NULL,
	[CustomerId] [bigint] NOT NULL,
	[ShortText1] [nvarchar](255) NULL,
	[ShortText2] [nvarchar](255) NULL,
	[ShortText3] [nvarchar](255) NULL,
	[ShortText4] [nvarchar](255) NULL,
	[ShortText5] [nvarchar](255) NULL,
	[ShortText6] [nvarchar](255) NULL,
	[ShortText7] [nvarchar](255) NULL,
	[ShortText8] [nvarchar](255) NULL,
	[ShortText9] [nvarchar](255) NULL,
	[ShortText10] [nvarchar](255) NULL,
	[LongText1] [nvarchar](max) NULL,
	[LongText2] [nvarchar](max) NULL,
	[LongText3] [nvarchar](max) NULL,
	[LongText4] [nvarchar](max) NULL,
	[LongText5] [nvarchar](max) NULL,
	[LongText6] [nvarchar](max) NULL,
	[LongText7] [nvarchar](max) NULL,
	[LongText8] [nvarchar](max) NULL,
	[LongText9] [nvarchar](max) NULL,
	[LongText10] [nvarchar](max) NULL,
	[Num1] [decimal](18, 2) NOT NULL,
	[Num2] [decimal](18, 2) NOT NULL,
	[Num3] [decimal](18, 2) NOT NULL,
	[Num4] [decimal](18, 2) NOT NULL,
	[Num5] [decimal](18, 2) NOT NULL,
	[Num6] [decimal](18, 2) NOT NULL,
	[Num7] [decimal](18, 2) NOT NULL,
	[Num8] [decimal](18, 2) NOT NULL,
	[Num9] [decimal](18, 2) NOT NULL,
	[Num10] [decimal](18, 2) NOT NULL,
	[YesNo1] [bit] NOT NULL,
	[YesNo2] [bit] NOT NULL,
	[YesNo3] [bit] NOT NULL,
	[YesNo4] [bit] NOT NULL,
	[YesNo5] [bit] NOT NULL,
	[YesNo6] [bit] NOT NULL,
	[YesNo7] [bit] NOT NULL,
	[YesNo8] [bit] NOT NULL,
	[YesNo9] [bit] NOT NULL,
	[YesNo10] [bit] NOT NULL,
	[Id1] [int] NOT NULL,
	[Id2] [int] NOT NULL,
	[Id3] [int] NOT NULL,
	[Id4] [int] NOT NULL,
	[Id5] [int] NOT NULL,
	[Id6] [int] NOT NULL,
	[Id7] [int] NOT NULL,
	[Id8] [int] NOT NULL,
	[Id9] [int] NOT NULL,
	[Id10] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedByName] [nvarchar](255) NULL,
	[ModifiedByName] [nvarchar](255) NULL,
	[Modified] [datetime] NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ParentCompanyId] [bigint] NOT NULL,
	[State] [int] NOT NULL,
	[FromDate] [datetime] NULL,
	[ToDate] [datetime] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_SOP.Tb_CreditNoteSummary] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [SOP].[Tb_CreditNoteSummary] ADD  DEFAULT (NULL) FOR [FromDate]
GO

ALTER TABLE [SOP].[Tb_CreditNoteSummary] ADD  DEFAULT (NULL) FOR [ToDate]
GO

ALTER TABLE [SOP].[Tb_CreditNoteSummary] ADD  DEFAULT ((1)) FOR [Status]
GO

ALTER TABLE [SOP].[Tb_CreditNoteSummary]  WITH CHECK ADD  CONSTRAINT [FK_SOP.Tb_CreditNoteSummary_Customer.Tb_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [Customer].[Tb_Customer] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [SOP].[Tb_CreditNoteSummary] CHECK CONSTRAINT [FK_SOP.Tb_CreditNoteSummary_Customer.Tb_Customer_CustomerId]
GO


