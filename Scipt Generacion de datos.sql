--Tablas de insert de usuario 
Select * from USUARIO

Insert into USUARIO(Documento, NombreCompleto , Correo , Clave ,IdRol , Estado)
values 
--Tablas de insert de roles
Select * from ROL
Insert into ROL (Descripcion) Values ('ADMINISTRADOR')
Insert into ROL (Descripcion) Values ('EMPLEADO')



-- Tablas de insert de permisos
Select * from PERMISO

Select p.IdRol , NombreMenu from PERMISO p 
inner join  ROL r on r.IdRol = p.IdRol 
inner join Usuario u on u.IdRol = r.IdRol
where u.IdRol=  2



Insert into PERMISO (IdRol ,NombreMenu) values 
(1,'MenuUsuario'),
(1,'MenuMantenedor'),
(1,'MenuVentas'),
(1,'MenuCompras'),
(1,'MenuClientes'),
(1,'MenuProveedores'),
(1,'MenuReportes'),
(1,'MenuAcercaDe')

Insert into PERMISO (IdRol ,NombreMenu) values 
(2,'MenuVentas'),
(2,'MenuCompras'),
(2,'MenuClientes'),
(2,'MenuProveedores'),
(2,'MenuAcercaDe')

--Ingreso de datos tabla negocio 
Use Sistema_Inventario_Test

Select * From Negocio

Insert into Negocio (IdNegocio , Nombre , Ruc ,Direccion) values (1,'NombreNegocio','Numero Ruc','La ecuatoriana')