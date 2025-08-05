update [Account].[Tb_GeneralLedger]
set Id1 = 1
where Name = 'Trip Charges'

update [Account].[Tb_GeneralLedger]
set Id1 = 2
where Name = 'Waiting time / Disposal'

update [Account].[Tb_GeneralLedger]
set Id1 = 3
where Name = 'Meeting Services'

update [Account].[Tb_GeneralLedger]
set Id1 = 4
where Name = 'Additional Stop'

update [Account].[Tb_GeneralLedger]
set Id1 = 5
where Name = 'Irregular Hour Charges'

update [Account].[Tb_GeneralLedger]
set Id1 = 6
where Name = 'Parking Charges'

update [Account].[Tb_GeneralLedger]
set Id1 = 7
where Name = 'ERP Charges'

update [Account].[Tb_GeneralLedger]
set Id1 = 8
where Name = 'Toll Fees'

update [Account].[Tb_GeneralLedger]
set Id1 = 9
where Name = 'Visa Fee'

update [Account].[Tb_GeneralLedger]
set Id1 = 10
where Name = 'PSA Pass Fee'

update [Account].[Tb_GeneralLedger]
set Id1 = 11
where Name = 'Jurong Port Pass Fee'

update [Account].[Tb_GeneralLedger]
set Id1 = 12
where Name = 'Certificate Fee'

update [Account].[Tb_GeneralLedger]
set Id1 = 13
where Name = 'Ferry Ticket'

update [Account].[Tb_GeneralLedger]
set Id1 = 14
where Name = 'Hotel Accomodation / Meals'

update [Account].[Tb_GeneralLedger]
set Id1 = 15
where Name = 'Accomodation / Meal'

update [Account].[Tb_GeneralLedger]
set Id1 = 16
where Name = 'Handphone Usage / SIM Card'

update [Account].[Tb_GeneralLedger]
set Id1 = 17
where Name = 'Pass / Licence'

update [Account].[Tb_GeneralLedger]
set Id1 = 18
where Name = 'Porterage Fee'

update [Account].[Tb_GeneralLedger]
set Id1 = 19
where Name = 'CN Invoice (Discount for Customers)'

update [Account].[Tb_GeneralLedger]
set Id1 = 20
where Name = 'Others'


UPDATE [Inventory].[Tb_Price]
SET  [Inventory].[Tb_Price].Id1 = gl.Id1
FROM  
     [Inventory].[Tb_Price] p
	 INNER JOIN [Account].[Tb_GeneralLedger] gl
	 ON p.GLCodeId = gl.Id


