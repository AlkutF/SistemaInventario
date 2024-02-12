
Create Proc SP_RegistrarUsuario(
@Documento varchar(50),
@NombreCompleto varchar(100),
@Correo varchar(100),
@Clave varchar(100),
@IdRol int,
@Estado bit,
@IdUsuarioResultado int output,
@Mensaje varchar(500) output
)
as
begin
	set @IdUsuarioResultado = 0
	set @Mensaje=''
	if not exists (Select * from USUARIO where Documento = @Documento)
	begin
		insert into USUARIO(Documento,NombreCompleto,Correo,Clave,IdRol,Estado) values 
		(@Documento,@NombreCompleto,@Correo,@Clave,@IdRol,@Estado)

		set @IdUsuarioResultado = SCOPE_IDENTITY()
	end
	else set @Mensaje='No se puede repetir el documento de identificacion, intente con otro'
end



create Proc SP_EditarUsuario(
@IdUsuario int ,
@Documento varchar(50),
@NombreCompleto varchar(100),
@Correo varchar(100),
@Clave varchar(100),
@IdRol int,
@Estado bit,
@Respuesta bit output,
@Mensaje varchar(500) output
)
as
begin
	set @Respuesta = 0
	set @Mensaje=''
	if not exists (Select * from USUARIO where Documento = @Documento and IdUsuario != @IdUsuario)
	begin
		 update USUARIO set 
		 Documento = @Documento,
		 NombreCompleto = @NombreCompleto,
		 Correo = @Correo,
		 Clave = @Clave,
		 IdRol = @IdRol,
		 Estado = @Estado
		 where IdUsuario = @IdUsuario

		set @Respuesta =1
	end
	else set @Mensaje='Existe un error en los documentos del usuario, revisar que sea unico'
end


create Proc SP_EliminarUsuario(
@IdUsuario int ,
@Respuesta bit output,
@Mensaje varchar(500) output
)
as
begin
	set @Respuesta = 0
	set @Mensaje=''
	declare @pasoreglas bit = 1

	if exists(Select * From COMPRA C 
	Inner Join Usuario U on U.IdUsuario = C.IdUsuario
	Where U.IdUsuario = @IdUsuario
	)
	Begin
		set @pasoreglas = 0
		set @Respuesta = 0
		set @Mensaje=@Mensaje+'No se puede eliminar a un usuario que se haya relacionado con una compra \n'
	End

	if exists(Select * From VENTA V 
	Inner Join Usuario U on U.IdUsuario = V.IdUsuario
	Where U.IdUsuario = @IdUsuario
	)
	Begin
		set @pasoreglas = 0
		set @Respuesta = 0
		set @Mensaje=@Mensaje+'No se puede eliminar a un usuario que se haya relacionado con una venta \n'
	End

	if(@pasoreglas = 1)
	begin
		delete from usuario where IdUsuario = @IdUsuario
		set @Respuesta = 1
	end

	return @Mensaje
end

--Creacion de compra 

Create type [dbo].[EDetalle_Compra] As Table (
[IdProducto] int Null ,
[Precio_Compra] decimal(18,2) Null ,
[Precio_Venta] decimal (18,2) Null ,
[Cantidad] int Null ,
[Monto_Total] decimal (18,2) Null
)

Go



Create Procedure SP_Registrar_Compra(
@IdUsuario int,
@IdProveedor int ,
@TipoDocumento varchar(100),
@NumeroDocumento varchar(100),
@MontoTotal decimal (10,2),
@DetalleCompra [EDetalle_Compra] READONLY ,
@Resultado bit output,
@Mensaje varchar(500) output
)
as 
begin
	begin try 
		declare @idcompra int = 0
		set @Resultado = 1
		set @Mensaje = ''

		begin transaction registro 

		insert into COMPRA(IdUsuario , IdProveedor , TipoDocumento , NumeroDocumento, MontoTotal)
		values(@IdUsuario , @IdProveedor,@TipoDocumento,@NumeroDocumento,@MontoTotal)

		set @idcompra = SCOPE_IDENTITY()

		insert into DETALLE_COMPRA(IdCompra , IdProducto, PrecioCompra,PrecioVenta,Cantidad,MontoTotal)
		Select @idcompra,IdProducto,Precio_Compra,Precio_Venta,Cantidad,Monto_Total from @DetalleCompra

		update p set p.Stock = p.Stock + dc.Cantidad,
					 p.PrecioCompra = dc.Precio_Compra,
					 p.PrecioVenta = dc.Precio_Venta
		From Producto p
		INNER Join @DetalleCompra dc on dc.IdProducto = p.IdProducto
		commit transaction registro

	end try
	begin catch 
		set @Resultado = 0
		set @Mensaje = ERROR_MESSAGE()
		rollback transaction registro
	end catch 
end


--Creacion de venta


Use Sistema_Inventario_Test


Create type [dbo].[EDetalle_Venta] As Table (
[IdProducto] int Null ,
[Precio_Venta] decimal (18,2) Null ,
[Cantidad] int Null ,
[SubTotal] decimal (18,2) Null
)

create procedure USP_Registrar_Venta(
@IdUsuario int ,
@TipoDocumento varchar(100),
@NumeroDocumento varchar(100),
@DocumentoCliente varchar(100),
@NombreCliente varchar(100),
@MontoPago decimal (18,2),
@MontoCambio decimal (18,2),
@MontoTotal decimal (18,2),
@DetalleVenta [EDetalle_Venta] READONLY ,
@Resultado  bit output,
@Mensaje varchar(500) output
)
as
begin 
	begin try
	declare @Idventa int =0
	set @Resultado = 1
	set @Mensaje =''
		begin transaction registro  
			insert into VENTA (IdUsuario ,TipoDocumento,NumeroDocumento,DocumentoCliente,NombreCliente,MontoPago,MontoCambio,MontoTotal)
			values(@IdUsuario,@TipoDocumento,@NumeroDocumento,@DocumentoCliente,@NombreCliente,@MontoPago,@MontoCambio,@MontoTotal)
			set @Idventa = SCOPE_IDENTITY()
			Insert into DETALLE_VENTA(IdVenta,IdProducto,PrecioVenta,Cantidad,Subtotal)
			select @Idventa , IdProducto,Precio_Venta,Cantidad,Subtotal from @DetalleVenta
			commit transaction registro
	end try
		begin catch 
		set @Resultado = 0
		set @Mensaje = ERROR_MESSAGE()
		rollback transaction registro
	end catch 
end


Select * from Venta where NumeroDocumento ='00005'
Select * from DETALLE_VENTA where IdVenta=5



Select c.IdCompra , u.NombreCompleto , pr.Documento , pr.RazonSocial,
c.TipoDocumento ,c.NumeroDocumento , c.MontoTotal,CONVERT(char(10),c.FechaRegistro,103)[FechaRegistro]
from COMPRA c 
inner join USUARIO u on c.IdUsuario = u.IdUsuario 
inner join PROVEEDOR pr on pr.IdProveedor = c.IdProveedor
where c.NumeroDocumento = @NumeroDocumento




Select p.Nombre , dv.PrecioVenta,dv.Cantidad,dv.Subtotal from DETALLE_VENTA dv 
inner join PRODUCTO p on dv.IdProducto = p.IdProducto
where dv.IdVenta = @Idventa

Delete  From VENTA
Delete From DETALLE_VENTA 


Select p.Nombre , dc.PrecioCompra , dc.Cantidad , dc.MontoTotal from DETALLE_COMPRA dc
inner join PRODUCTO p on p.IdProducto = dc.IdProducto
where dc.IdCompra = 1




Select v.IdVenta , u.NombreCompleto ,
v.DocumentoCliente , v.NombreCliente , v.TipoDocumento,
v.NumeroDocumento , v.MontoPago ,v.MontoCambio, v.MontoTotal , CONVERT(char(10),v.FechaRegistro,103)[FechaRegistro]
from Venta v
Inner Join Usuario u on v.IdUsuario =u.IdUsuario
where v.NumeroDocumento= '00005'