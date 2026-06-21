-- ==================================================================
-- MariaDB: creacion de base de datos TiendaDB con 4 entidades
-- MISMA estructura que SQL Server, con tipos compatibles.
-- Se ejecuta automaticamente al iniciar el contenedor (initdb.d),
-- o manualmente:
--   docker exec -i demo-mariadb mariadb -uroot -p"Passw0rd!2024" < 01_init_mariadb.sql
-- ==================================================================

CREATE DATABASE IF NOT EXISTS TiendaDB
    CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

USE TiendaDB;

-- Eliminar tablas si existen (respetando dependencias)
DROP TABLE IF EXISTS DetallePedidos;
DROP TABLE IF EXISTS Pedidos;
DROP TABLE IF EXISTS Productos;
DROP TABLE IF EXISTS Clientes;

-- ------------------------------------------------------------------
-- Entidad 1: Clientes
-- ------------------------------------------------------------------
CREATE TABLE Clientes (
    ClienteId     INT AUTO_INCREMENT PRIMARY KEY,
    Nombre        VARCHAR(100) NOT NULL,
    Email         VARCHAR(150) NOT NULL,
    Telefono      VARCHAR(20),
    Ciudad        VARCHAR(80),
    FechaRegistro DATE NOT NULL
);

-- ------------------------------------------------------------------
-- Entidad 2: Productos
-- ------------------------------------------------------------------
CREATE TABLE Productos (
    ProductoId  INT AUTO_INCREMENT PRIMARY KEY,
    Nombre      VARCHAR(120) NOT NULL,
    Descripcion VARCHAR(255),
    Precio      DECIMAL(10,2) NOT NULL,
    Stock       INT NOT NULL,
    Categoria   VARCHAR(60)
);

-- ------------------------------------------------------------------
-- Entidad 3: Pedidos
-- ------------------------------------------------------------------
CREATE TABLE Pedidos (
    PedidoId    INT AUTO_INCREMENT PRIMARY KEY,
    ClienteId   INT NOT NULL,
    FechaPedido DATETIME NOT NULL,
    Total       DECIMAL(10,2) NOT NULL,
    Estado      VARCHAR(30) NOT NULL,
    CONSTRAINT FK_Pedidos_Clientes FOREIGN KEY (ClienteId)
        REFERENCES Clientes(ClienteId)
);

-- ------------------------------------------------------------------
-- Entidad 4: DetallePedidos
-- ------------------------------------------------------------------
CREATE TABLE DetallePedidos (
    DetalleId      INT AUTO_INCREMENT PRIMARY KEY,
    PedidoId       INT NOT NULL,
    ProductoId     INT NOT NULL,
    Cantidad       INT NOT NULL,
    PrecioUnitario DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_Detalle_Pedidos FOREIGN KEY (PedidoId)
        REFERENCES Pedidos(PedidoId),
    CONSTRAINT FK_Detalle_Productos FOREIGN KEY (ProductoId)
        REFERENCES Productos(ProductoId)
);

-- ==================================================================
-- DATOS (10 registros por tabla) -- DIFERENTES a los de SQL Server
-- ==================================================================

INSERT INTO Clientes (Nombre, Email, Telefono, Ciudad, FechaRegistro) VALUES
('Gabriela Reyes',  'gabriela.reyes@correo.com',  '666-2001', 'Madrid',     '2023-09-10'),
('Andres Morales',  'andres.morales@correo.com',  '666-2002', 'Barcelona',  '2023-10-02'),
('Lucia Fernandez', 'lucia.fernandez@correo.com', '666-2003', 'Valencia',   '2023-10-21'),
('Diego Romero',    'diego.romero@correo.com',    '666-2004', 'Sevilla',    '2023-11-14'),
('Carmen Ortiz',    'carmen.ortiz@correo.com',    '666-2005', 'Bilbao',     '2023-12-01'),
('Pablo Navarro',   'pablo.navarro@correo.com',   '666-2006', 'Malaga',     '2023-12-19'),
('Isabel Ruiz',     'isabel.ruiz@correo.com',     '666-2007', 'Zaragoza',   '2024-01-08'),
('Miguel Santos',   'miguel.santos@correo.com',   '666-2008', 'Murcia',     '2024-01-27'),
('Laura Jimenez',   'laura.jimenez@correo.com',   '666-2009', 'Granada',    '2024-02-15'),
('Sergio Castro',   'sergio.castro@correo.com',   '666-2010', 'Alicante',   '2024-03-04');

INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, Categoria) VALUES
('Tablet Air 11',     'Tablet 11 pulgadas 128GB',         8999.00, 25, 'Computo'),
('Cargador Rapido',   'Cargador USB-C 65W',                 599.00, 90, 'Accesorios'),
('Soporte Laptop',    'Soporte ajustable de aluminio',      499.00, 55, 'Accesorios'),
('Proyector HD',      'Proyector portatil 1080p',          5499.00, 10, 'Video'),
('Bocina Bluetooth',  'Bocina portatil resistente al agua',1199.00, 40, 'Audio'),
('Microfono USB',     'Microfono de condensador',          1599.00, 28, 'Audio'),
('Memoria USB 256GB', 'Memoria flash USB 3.2',              449.00, 100,'Almacenamiento'),
('Lampara LED',       'Lampara de escritorio regulable',    349.00, 65, 'Oficina'),
('Escritorio Ajustable','Escritorio sentado-de pie',        9999.00, 8, 'Mobiliario'),
('Router WiFi 6',     'Router de doble banda',             2199.00, 22, 'Redes');

INSERT INTO Pedidos (ClienteId, FechaPedido, Total, Estado) VALUES
(1, '2024-03-10 09:00:00',  9598.00, 'Entregado'),
(2, '2024-03-12 11:30:00',   599.00, 'Enviado'),
(3, '2024-03-15 10:15:00',  5499.00, 'Pendiente'),
(4, '2024-03-18 14:45:00',  2798.00, 'Entregado'),
(5, '2024-03-20 16:00:00',  1199.00, 'Enviado'),
(6, '2024-03-22 08:20:00',  9999.00, 'Pendiente'),
(7, '2024-03-25 13:10:00',   898.00, 'Entregado'),
(8, '2024-03-27 15:35:00',  2199.00, 'Cancelado'),
(9, '2024-03-29 12:50:00',  1599.00, 'Enviado'),
(10,'2024-04-01 17:05:00',   449.00, 'Entregado');

INSERT INTO DetallePedidos (PedidoId, ProductoId, Cantidad, PrecioUnitario) VALUES
(1, 1, 1, 8999.00),
(1, 2, 1,  599.00),
(2, 2, 1,  599.00),
(3, 4, 1, 5499.00),
(4, 5, 1, 1199.00),
(4, 6, 1, 1599.00),
(5, 5, 1, 1199.00),
(6, 9, 1, 9999.00),
(7, 3, 1,  499.00),
(8, 10,1, 2199.00);

SELECT 'MariaDB: TiendaDB creada con 4 tablas y 10 registros cada una.' AS Resultado;
