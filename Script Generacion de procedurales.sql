
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