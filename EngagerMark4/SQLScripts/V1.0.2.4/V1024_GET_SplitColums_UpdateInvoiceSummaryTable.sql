CREATE TABLE #TempTable(
 ID int,
 YearMonthNo int,
 SerialNo int)


INSERT INTO #TempTable (ID, YearMonthNo, SerialNo)
SELECT
    Id, CAST( CONCAT(dbo.GetColumnValue(ReferenceNo, '/', 1),dbo.GetColumnValue(ReferenceNo,'/',2)) AS INT)
        ,CAST(dbo.GetColumnValue(ReferenceNo, '/', 3) AS INT)
FROM [SOP].[Tb_SalesInvoiceSummary]

SELECT * FROM #TempTable

declare @idColumn int
declare @YearMonthNo int
declare @SerialNo int

select @idColumn = min( ID ) from #TempTable

while @idColumn is not null
begin

SET @YearMonthNo = (SELECT YearMonthNo FROM #TempTable WHERE ID = @idColumn)
SET @SerialNo = (SELECT SerialNo FROM #TempTable WHERE ID = @idColumn)

UPDATE [SOP].[Tb_SalesInvoiceSummary]
SET Id1 = @YearMonthNo, Id2 = @SerialNo
WHERE Id = @idColumn

select @idColumn = min( ID ) from #TempTable where ID > @idColumn
end

SELECT Id, ReferenceNo, Id1, Id2 FROM [SOP].[Tb_SalesInvoiceSummary] WHERE Id1 >= 202002 AND Id2 >= 100 AND Id1 <= 202002 AND Id2 <=400

SELECT * FROM [SOP].[Tb_SalesOrder]