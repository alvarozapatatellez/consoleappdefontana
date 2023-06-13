IF OBJECT_ID('tempdb..#DetalleVenta') IS NOT NULL
BEGIN
    DROP TABLE #DetalleVenta
END

SELECT v.ID_Local AS IdLocal, l.Nombre AS NombreLocal, l.Direccion, v.ID_Venta AS IdVenta, v.Fecha, v.Total, 
vd.ID_VentaDetalle AS IdVentaDetalle, vd.Precio_Unitario AS PrecioUnitario, vd.Cantidad, vd.TotalLinea, 
p.ID_Producto AS IdProducto, p.Codigo, p.Nombre AS ProductoNombre, m.ID_Marca AS IdMarca, m.Nombre AS MarcarNombre, p.Modelo, p.Costo_Unitario AS CostoUnitario
INTO #DetalleVenta FROM Prueba.dbo.Venta AS v
INNER JOIN Prueba.dbo.[Local] AS l ON v.ID_Local = l.ID_Local
INNER JOIN Prueba.dbo.VentaDetalle AS vd ON v.ID_Venta = vd.ID_Venta
INNER JOIN Prueba.dbo.Producto AS p ON vd.ID_Producto = p.ID_Producto
INNER JOIN Prueba.dbo.Marca AS m ON p.ID_Marca = m.ID_Marca
WHERE v.Fecha >= DATEADD(DD, -30, GETDATE()) AND v.Fecha <= CONVERT(DATETIME, CONVERT(VARCHAR(10), GETDATE(), 120) + ' 23:59:59', 120)

--DETALLE DE VENTA DE LOS ULTIMOS 30 DIAS
SELECT * FROM #DetalleVenta 

--El total de ventas de los últimos 30 días (monto total y cantidad total de ventas)
SELECT SUM(v.Total) AS 'Monto Total', Count(v.IdVenta) AS 'Cantidad Ventas' 
FROM (SELECT DISTINCT IdLocal, NombreLocal, IdVenta, Fecha, Total FROM #DetalleVenta) AS v 

--Indicar cuál es el producto con mayor monto total de ventas
SELECT TOP 1 IdProducto, Codigo, ProductoNombre, SUM(TotalLinea) AS Total FROM #DetalleVenta
GROUP BY IdProducto, Codigo, ProductoNombre
ORDER BY Total DESC 

--Indicar el local con mayor monto de ventas"
SELECT TOP 1 IdLocal, NombreLocal, Sum(TotalLinea) AS Total FROM #DetalleVenta
GROUP BY IdLocal, NombreLocal
ORDER BY Total DESC 

--¿Cuál es la marca con mayor margen de ganancias?
SELECT TOP 1 IdMarca, MarcarNombre, SUM(CostoUnitario) AS Costo, SUM(PrecioUnitario) AS Precio, 
ROUND((1-(SUM(CAST(CostoUnitario AS decimal(12,2)))/SUM(CAST(PrecioUnitario AS decimal(12,2)))))*100, 2) AS MargenVenta
FROM #DetalleVenta GROUP BY IdMarca, MarcarNombre
ORDER BY MargenVenta DESC

--¿Cómo obtendrías cuál es el producto que más se vende en cada local?
SELECT v.IdLocal, v.NombreLocal, v.IdProducto, v.ProductoNombre, v.CantidadVentas
FROM (
	SELECT IdLocal, NombreLocal, IdProducto, ProductoNombre, SUM(Cantidad) AS CantidadVentas,
		ROW_NUMBER() OVER (PARTITION BY IdLocal ORDER BY SUM(Cantidad) DESC) AS RowNum
	FROM #DetalleVenta
	GROUP BY IdLocal, NombreLocal, IdProducto, ProductoNombre
) AS v WHERE V.RowNum = 1
ORDER BY IdLocal
