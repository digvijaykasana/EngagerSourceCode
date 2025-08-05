update [Inventory].[Tb_Price]
set PickUpPoint = LEFT(PickUpPoint, len(PickUpPoint)-6)
where PickUpPoint like '%[0-9][0-9][0-9][0-9][0-9][0-9]'

update [Inventory].[Tb_Price]
set DropPoint = LEFT(DropPoint, len(DropPoint)-6)
where DropPoint like '%[0-9][0-9][0-9][0-9][0-9][0-9]'