
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


--Prueba para evaluar si funciona el proceso , no requiere ejecutarse
declare @Respuesta bit 
declare @mensaje varchar(500)

exec SP_EditarUsuario 3,'1','PruebaProcedural2','test@gmail.com','1234',2,1,@Respuesta output , @mensaje output
select @Respuesta
select @mensaje

select * from USUARIO