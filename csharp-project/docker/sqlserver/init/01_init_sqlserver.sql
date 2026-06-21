-- ==================================================================
-- SQL Server: creacion de base de datos TiendaDB con 4 entidades
-- Ejecutar dentro del contenedor con sqlcmd, o desde SSMS / Azure Data Studio.
--   docker exec -i demo-sqlserver /opt/mssql-tools18/bin/sqlcmd \
--     -S localhost -U sa -P "Passw0rd!2024" -C -i /tmp/01_init_sqlserver.sql
-- ==================================================================

IF DB_ID('TiendaDB') IS NULL
    CREATE DATABASE TiendaDB;
GO

USE TiendaDB;
GO

-- ------------------------------------------------------------------
-- Eliminar tablas si existen (respetando dependencias)
-- ------------------------------------------------------------------
IF OBJECT_ID('dbo.DetallePedidos', 'U') IS NOT NULL DROP TABLE dbo.DetallePedidos;
IF OBJECT_ID('dbo.Pedidos', 'U')        IS NOT NULL DROP TABLE dbo.Pedidos;
IF OBJECT_ID('dbo.Productos', 'U')      IS NOT NULL DROP TABLE dbo.Productos;
IF OBJECT_ID('dbo.Clientes', 'U')       IS NOT NULL DROP TABLE dbo.Clientes;
GO

-- ------------------------------------------------------------------
-- Entidad 1: Clientes
-- ------------------------------------------------------------------
CREATE TABLE dbo.Clientes (
    ClienteId     INT IDENTITY(1,1) PRIMARY KEY,
    Nombre        NVARCHAR(100) NOT NULL,
    Email         NVARCHAR(150) NOT NULL,
    Telefono      NVARCHAR(20),
    Ciudad        NVARCHAR(80),
    FechaRegistro DATE NOT NULL
);
GO

-- ------------------------------------------------------------------
-- Entidad 2: Productos
-- ------------------------------------------------------------------
CREATE TABLE dbo.Productos (
    ProductoId  INT IDENTITY(1,1) PRIMARY KEY,
    Nombre      NVARCHAR(120) NOT NULL,
    Descripcion NVARCHAR(255),
    Precio      DECIMAL(10,2) NOT NULL,
    Stock       INT NOT NULL,
    Categoria   NVARCHAR(60)
);
GO

-- ------------------------------------------------------------------
-- Entidad 3: Pedidos
-- ------------------------------------------------------------------
CREATE TABLE dbo.Pedidos (
    PedidoId    INT IDENTITY(1,1) PRIMARY KEY,
    ClienteId   INT NOT NULL,
    FechaPedido DATETIME NOT NULL,
    Total       DECIMAL(10,2) NOT NULL,
    Estado      NVARCHAR(30) NOT NULL,
    CONSTRAINT FK_Pedidos_Clientes FOREIGN KEY (ClienteId)
        REFERENCES dbo.Clientes(ClienteId)
);
GO

-- ------------------------------------------------------------------
-- Entidad 4: DetallePedidos
-- ------------------------------------------------------------------
CREATE TABLE dbo.DetallePedidos (
    DetalleId      INT IDENTITY(1,1) PRIMARY KEY,
    PedidoId       INT NOT NULL,
    ProductoId     INT NOT NULL,
    Cantidad       INT NOT NULL,
    PrecioUnitario DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_Detalle_Pedidos FOREIGN KEY (PedidoId)
        REFERENCES dbo.Pedidos(PedidoId),
    CONSTRAINT FK_Detalle_Productos FOREIGN KEY (ProductoId)
        REFERENCES dbo.Productos(ProductoId)
);
GO

-- ==================================================================
-- DATOS (10 registros por tabla) -- DIFERENTES a los de MariaDB
-- ==================================================================

INSERT INTO dbo.Clientes (Nombre, Email, Telefono, Ciudad, FechaRegistro) VALUES
('Ana Torres',        'ana.torres@correo.com',      '555-1001', 'Monterrey',     '2024-01-15'),
('Luis Ramirez',      'luis.ramirez@correo.com',    '555-1002', 'Guadalajara',   '2024-02-03'),
('Maria Gomez',       'maria.gomez@correo.com',     '555-1003', 'CDMX',          '2024-02-20'),
('Carlos Mendoza',    'carlos.mendoza@correo.com',  '555-1004', 'Puebla',        '2024-03-11'),
('Sofia Herrera',     'sofia.herrera@correo.com',   '555-1005', 'Queretaro',     '2024-03-28'),
('Jorge Vargas',      'jorge.vargas@correo.com',    '555-1006', 'Tijuana',       '2024-04-09'),
('Elena Castillo',    'elena.castillo@correo.com',  '555-1007', 'Merida',        '2024-04-22'),
('Roberto Diaz',      'roberto.diaz@correo.com',    '555-1008', 'Leon',          '2024-05-05'),
('Patricia Nunez',    'patricia.nunez@correo.com',  '555-1009', 'Cancun',        '2024-05-19'),
('Fernando Silva',    'fernando.silva@correo.com',  '555-1010', 'Toluca',        '2024-06-01');
GO

INSERT INTO dbo.Productos (Nombre, Descripcion, Precio, Stock, Categoria) VALUES
('Laptop Pro 14',     'Portatil 14 pulgadas 16GB RAM',    24999.00, 12, 'Computo'),
('Mouse Inalambrico', 'Mouse ergonomico bluetooth',         349.50, 80, 'Accesorios'),
('Teclado Mecanico',  'Switches rojos retroiluminado',     1299.00, 45, 'Accesorios'),
('Monitor 27 4K',     'Monitor UHD 27 pulgadas',           7899.00, 20, 'Computo'),
('Audifonos ANC',     'Cancelacion activa de ruido',       2499.00, 33, 'Audio'),
('Webcam Full HD',    'Camara 1080p con microfono',         899.00, 60, 'Accesorios'),
('Disco SSD 1TB',     'Unidad de estado solido NVMe',      1799.00, 50, 'Almacenamiento'),
('Hub USB-C',         'Hub 7 en 1 con HDMI',                699.00, 70, 'Accesorios'),
('Silla Ergonomica',  'Silla de oficina con soporte lumbar',4599.00, 15, 'Mobiliario'),
('Impresora Laser',   'Impresora monocromatica wifi',      3299.00, 18, 'Oficina');
GO

INSERT INTO dbo.Pedidos (ClienteId, FechaPedido, Total, Estado) VALUES
(1, '2024-06-02 10:15:00', 25348.50, 'Entregado'),
(2, '2024-06-03 14:30:00',  1648.00, 'Enviado'),
(3, '2024-06-05 09:45:00',  7899.00, 'Pendiente'),
(4, '2024-06-07 16:20:00',  3398.00, 'Entregado'),
(5, '2024-06-08 11:05:00',  2499.00, 'Cancelado'),
(6, '2024-06-10 13:50:00',  5298.00, 'Enviado'),
(7, '2024-06-11 08:30:00',   699.00, 'Entregado'),
(8, '2024-06-12 17:10:00',  4599.00, 'Pendiente'),
(9, '2024-06-14 12:40:00',  3299.00, 'Enviado'),
(10,'2024-06-15 15:25:00',  1799.00, 'Entregado');
GO

INSERT INTO dbo.DetallePedidos (PedidoId, ProductoId, Cantidad, PrecioUnitario) VALUES
(1, 1, 1, 24999.00),
(1, 2, 1,   349.50),
(2, 3, 1,  1299.00),
(2, 6, 1,   899.00),
(3, 4, 1,  7899.00),
(4, 5, 1,  2499.00),
(4, 8, 1,   699.00),
(5, 5, 1,  2499.00),
(6, 4, 1,  7899.00),
(7, 8, 1,   699.00);
GO

PRINT 'SQL Server: TiendaDB creada con 4 tablas y 10 registros cada una.';
GO
