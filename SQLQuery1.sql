--crear una base de datos SECCION-PDM:
create database LOGIN_PDM
GO

--USAR ESA BASE DE DATOS:
USE LOGIN_PDM
GO

-- Creación de tabla de roles
CREATE TABLE roles (
    id_rol INT PRIMARY KEY,
    nombre_rol VARCHAR(50) NOT NULL
);

select * from roles
-- Creación de tabla de usuarios
CREATE TABLE usuarios (
    id_usuario INT PRIMARY KEY,
    nombre_usuario VARCHAR(50) NOT NULL,
    password VARCHAR(50) NOT NULL,
    id_rol INT NOT NULL,
    FOREIGN KEY (id_rol) REFERENCES roles(id_rol)
);

select * from usuarios

-- Insertando registros en tabla de roles
INSERT INTO roles (id_rol, nombre_rol) VALUES (1, 'administrador');
INSERT INTO roles (id_rol, nombre_rol) VALUES (2, 'supervisor');
INSERT INTO roles (id_rol, nombre_rol) VALUES (3, 'vendedor');

-- Insertando registros en tabla de usuarios
INSERT INTO usuarios (id_usuario, nombre_usuario, password, id_rol) VALUES (1, 'Jose', '232425', 1);
INSERT INTO usuarios (id_usuario, nombre_usuario, password, id_rol) VALUES (2, 'Jhoan', '202020', 2);
INSERT INTO usuarios (id_usuario, nombre_usuario, password, id_rol) VALUES (3, 'Carla', '123456', 3);
INSERT INTO usuarios (id_usuario, nombre_usuario, password, id_rol) VALUES (4, 'Cristal', 'rojas1', 1);
INSERT INTO usuarios (id_usuario, nombre_usuario, password, id_rol) VALUES (5, 'Mary ', 'rojas', 2);

-- Consultas básicas

-- Seleccionar todos los registros de la tabla de roles
SELECT * FROM roles;

-- Seleccionar todos los registros de la tabla de usuarios
SELECT * FROM usuarios;

-- Seleccionar un registro específico de la tabla de roles
SELECT * FROM roles WHERE id_rol = 2;

-- Seleccionar un registro específico de la tabla de usuarios
SELECT * FROM usuarios WHERE id_usuario = 4;

-- Consultas avanzadas

-- Seleccionar todos los usuarios y sus roles
SELECT usuarios.*, roles.nombre_rol
FROM usuarios
INNER JOIN roles ON usuarios.id_rol = roles.id_rol;

-- Seleccionar los usuarios que tengan un rol específico
SELECT * FROM usuarios WHERE id_rol = 3;

-- Contar la cantidad de usuarios por cada rol
SELECT roles.nombre_rol, COUNT(usuarios.id_usuario) AS cantidad_usuarios
FROM usuarios
INNER JOIN roles ON usuarios.id_rol = roles.id_rol
GROUP BY roles.nombre_rol;

-- Actualizar un registro en la tabla de roles
UPDATE roles SET nombre_rol = 'administrador principal' WHERE id_rol = 1;

-- Eliminar un registro en la tabla de usuarios
DELETE FROM usuarios WHERE id_usuario = 8;

SELECT * FROM usuarios;
SELECT * FROM roles;

-- Creación de la tabla usuario

CREATE TABLE usuario (
    id_usuario INT NOT NULL,
    nombre_user VARCHAR(20) NOT NULL,
    telefono VARCHAR(30) NOT NULL,
    email VARCHAR(30) NOT NULL
);

SELECT * FROM usuario
-- Insertar datos en la tabla usuario

INSERT INTO usuario VALUES (1, 'Jose', '829-5130365', 'josemiguel.r.rd.com');

-- Creación de la tabla Articulos

CREATE TABLE Articulos (
    IDArticulo INT PRIMARY KEY,
    Nombre NVARCHAR(100),
    Precio DECIMAL(10, 2)
);

--Insertar datos en la tabla Articulos

INSERT INTO Articulos (IDArticulo, Nombre, Precio)
VALUES
    (1, 'Laptop Legion', 850.50),
    (2, 'Mouse Gamer', 30.25),
    (3, 'Monitor Gamer 32', 250.75),
    (4, 'Bocinas', 12.90),
    (5, 'UPS de 1200 kw', 120.00);

-- Creación de la tabla Ventas

CREATE TABLE Ventas (
    IDVenta INT IDENTITY(1, 1) PRIMARY KEY,
    IDArticulo INT,
    Cantidad INT,
    Precio DECIMAL(18, 2)
);


-- Consulta de la tabla Ventas

SELECT * FROM Ventas;

-- Insertar 10 artículos tecnológicos en la tabla Articulos

INSERT INTO Articulos (IDArticulo, Nombre, Precio)
VALUES
    (6, 'Smartphone', 500.00),
    (7, 'Laptop Legion', 1000.00),
    (8, 'Tablet', 300.00),
    (9, 'Smartwatch', 150.00),
    (10, 'Gafas de Realidad Virtual', 200.00),
    (11, 'Auriculares Inalámbricos', 100.00),
    (12, 'Altavoz Inteligente', 80.00),
    (13, 'Cámara de Acción', 250.00),
    (14, 'Monitor Curvo', 300.00),
    (15, 'Consola de Videojuegos', 400.00);

-- Consulta de la tabla Articulos

SELECT * FROM Articulos;


