IF OBJECT_ID('Productos', 'U') IS NOT NULL
    DROP TABLE Productos;
GO

CREATE TABLE Productos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(100),
    Precio DECIMAL(10, 2),
    Stock INT
);
GO

IF OBJECT_ID('ObtenerProductosPorPrecio', 'P') IS NOT NULL
    DROP PROCEDURE ObtenerProductosPorPrecio;
GO

CREATE PROCEDURE ObtenerProductosPorPrecio
    @Precio DECIMAL(10, 2)
AS
BEGIN
    SELECT
		Id,
		Nombre,
		Precio,
		Stock
	FROM Productos WHERE Precio = @Precio;
END;
GO

IF OBJECT_ID('ObtenerProductoPorId', 'P') IS NOT NULL
    DROP PROCEDURE ObtenerProductoPorId;
GO

CREATE PROCEDURE ObtenerProductoPorId
    @Id INT
AS
BEGIN
    SELECT
		Id,
		Nombre,
		Precio,
		Stock
	FROM Productos WHERE Id = @Id;
END;
GO

IF OBJECT_ID('ObtenerProductos', 'P') IS NOT NULL
    DROP PROCEDURE ObtenerProductos;
GO

CREATE PROCEDURE ObtenerProductos
AS
BEGIN
    SELECT
		Id,
		Nombre,
		Precio,
		Stock
	FROM Productos;
END;
GO

IF OBJECT_ID('EliminarProductoPorId', 'P') IS NOT NULL
    DROP PROCEDURE EliminarProductoPorId;
GO

CREATE PROCEDURE EliminarProductoPorId
    @Id INT
AS
BEGIN
    DELETE FROM Productos WHERE Id = @Id;
END;
GO

IF OBJECT_ID('ActualizarProducto', 'P') IS NOT NULL
    DROP PROCEDURE ActualizarProducto;
GO

CREATE PROCEDURE ActualizarProducto
    @Id INT,
    @Nombre VARCHAR(100),
    @Precio DECIMAL(10, 2),
    @Stock INT
AS
BEGIN
    UPDATE Productos
    SET Nombre = @Nombre,
        Precio = @Precio,
        Stock = @Stock
    WHERE Id = @Id;
END;
GO

IF OBJECT_ID('InsertarProducto', 'P') IS NOT NULL
    DROP PROCEDURE InsertarProducto;
GO

CREATE PROCEDURE InsertarProducto
    @Nombre VARCHAR(100),
    @Precio DECIMAL(10, 2),
    @Stock INT
AS
BEGIN
    INSERT INTO Productos (Nombre, Precio, Stock)
    VALUES (@Nombre, @Precio, @Stock);
END;
GO

-- Ejemplos de uso de los procedimientos almacenados
EXEC InsertarProducto 'Producto1', 10.99, 100;
EXEC InsertarProducto 'Producto2', 15.99, 150;
--EXEC ObtenerProductos;
--EXEC ActualizarProducto 1, 'NuevoNombre', 15.99, 200;
--EXEC EliminarProductoPorId 2;
--EXEC ObtenerProductos;
--EXEC ObtenerProductoPorId 1;
--EXEC ObtenerProductosPorPrecio 15.99;
