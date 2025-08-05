select * from SOP.Tb_SalesOrder where PickupPoint ='Airport - 760245'

select * from [Inventory].[Tb_Price] where DropPoint = 'Airport - '

update [SOP].[Tb_SalesOrder]
set PickUpPoint = 'Airport - 760245'
where PickUpPoint = 'Airport 760245'

update [SOP].[Tb_SalesOrder]
set DropPoint = 'Airport - 760245'
where DropPoint = 'Airport 760245'

update [Inventory].[Tb_Price]
set PickUpPoint = 'Airport'
where PickUpPoint = 'Airport - '

update [Inventory].[Tb_Price]
set DropPoint = 'Airport'
where DropPoint = 'Airport - '

