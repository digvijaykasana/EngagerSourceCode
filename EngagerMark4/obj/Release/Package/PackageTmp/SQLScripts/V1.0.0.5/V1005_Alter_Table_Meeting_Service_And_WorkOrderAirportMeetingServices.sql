
alter table [Dummy].[Tb_MeetingService]
Add Description nvarchar(max) null;

alter table [Dummy].[Tb_MeetingService]
Add AdditionalPersonCharge decimal(18,2) not null default(0);

alter table [Dummy].[Tb_MeetingService]
Add LastMinuteCharge decimal(18,2) not null default(0);

alter table [Dummy].[Tb_MeetingService]
Add OvernightCharge decimal(18,2) not null default(0);

alter table [Dummy].[Tb_MeetingService]
Add MajorAmendmentCharge decimal(18,2) not null default(0);

alter table [Dummy].[Tb_MeetingService]
Add IsHighCharge bit not null default(0);

alter table [Dummy].[Tb_MeetingServiceDetails]
Add MinPax int not null default(0);

alter table [Dummy].[Tb_MeetingServiceDetails]
Add MaxPax int not null default(0);

alter table [Dummy].[Tb_MeetingService]
Add IsHighCharge bit not null default(0);

alter table [dbo].[WorkOrderAirportMeetingServices]
Add IsLastMinuteCharge bit not null default(0);

alter table [dbo].[WorkOrderAirportMeetingServices]
Add LastMinuteCharge decimal(18,2) not null default(0);

alter table [dbo].[WorkOrderAirportMeetingServices]
Add OvernightCharge decimal(18,2) not null default(0);

alter table [dbo].[WorkOrderAirportMeetingServices]
Add IsMajorAmendment bit not null default(0);

alter table [dbo].[WorkOrderAirportMeetingServices]
Add MajorAmendmentCharge decimal(18,2) not null default(0);

alter table [dbo].[WorkOrderAirportMeetingServices]
Add AdditionalPersonCharge decimal(18,2) not null default(0);

alter table [dbo].[WorkOrderAirportMeetingServices]
Add TotalPaxCharge decimal(18,2) not null default(0);

alter table [dbo].[WorkOrderAirportMeetingServices]
Add PerPaxChargeLabel nvarchar(max) null;

alter table [dbo].[WorkOrderAirportMeetingServices]
Add PerPaxCharge decimal(18,2) not null default(0);

alter table [dbo].[WorkOrderAirportMeetingServices]
Add MeetingServiceDetailId bigint not null default(0);

alter table [dbo].[WorkOrderAirportMeetingServices]
Add MaxPaxRange int not null default(0);

alter table [dbo].[WorkOrderAirportMeetingServices]
Add MeetingServicePassengerInCharge nvarchar(max) null;