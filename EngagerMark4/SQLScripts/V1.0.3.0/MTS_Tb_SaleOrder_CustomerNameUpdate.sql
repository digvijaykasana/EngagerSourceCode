UPDATE [SOP].[Tb_SalesOrder] 
   SET [SOP].[Tb_SalesOrder].ShortText4 = CUS.Name 
   FROM [SOP].[Tb_SalesOrder] SO INNER JOIN  [Customer].[Tb_Customer] CUS ON SO.CustomerId = CUS.Id


   SELECT TOP 100 SO.Created, SO.ShortText4, SO.CustomerId, CUS.Name, * FROM [SOP].[Tb_SalesOrder] SO 
   INNER JOIN [Customer].[Tb_Customer] CUS ON CUS.Id = SO.CustomerId 
   ORDER BY SO.Created DESC