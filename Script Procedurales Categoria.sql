
--Procedimiento para categorias
Create Proc SP_RegistrarCategoria(
@Descripcion varchar(50),
@Estado bit,
@Resultado int output ,
@Mensaje varchar (500) output
)
as
begin
	Set @Resultado=0
	if Not exists (Select *From CATEGORIA Where Descripcion = @Descripcion)
		begin 
			insert into Categoria (Descripcion , Estado) values (@Descripcion , @Estado)
			set @Resultado = SCOPE_IDENTITY()
		end
		else 
			set @Mensaje = 'Ya existe esa categoria'
end



Create Proc SP_ModificarCategoria(
@IdCategoria int ,
@Descripcion varchar(50),
@Resultado int output ,
@Estado bit,
@Mensaje varchar (500) output
)
as
begin
	Set @Resultado=1
	if Not exists (Select *From CATEGORIA Where Descripcion = @Descripcion and IdCategoria != @IdCategoria)
		update CATEGORIA set 
		Descripcion = @Descripcion,
		Estado = @Estado
		where IdCategoria = @IdCategoria
		else 
		begin 
			set @Resultado=0
			set @Mensaje = 'Ya existe esa categoria'
		end 
end

Create Proc SP_EliminarCategoria(
@IdCategoria int ,
@Resultado int output ,
@Mensaje varchar (500) output)
as
begin 
	set @Resultado=1
	if not exists ( Select * From CATEGORIA C Inner Join PRODUCTO p on p.IdCategoria = c.IdCategoria
	where c.IdCategoria =@IdCategoria)
	begin
		delete top(1) from CATEGORIA Where IdCategoria = @IdCategoria
	end
	else
		begin
			set @Resultado=0
			set @Mensaje = 'La categoria tiene un producto no se puede eliminar'
		end
end





--Creacion Procedurales de producto

Create Proc SP_RegistrarProducto(
@Codigo varchar(50),
@Nombre varchar (50) ,
@Descripcion varchar(50),
@IdCategoria int ,
@Estado bit,
@Resultado int output ,
@Mensaje varchar (500) output
)
as
begin
	Set @Resultado=0
	if Not exists (Select *From PRODUCTO Where Codigo = @Codigo)
		begin 
			 INSERT INTO PRODUCTO (Codigo, Nombre, Descripcion, IdCategoria, Estado)
			 VALUES (@Codigo, @Nombre, @Descripcion, @IdCategoria, @Estado)
			set @Resultado = SCOPE_IDENTITY()
		end
		else 
			set @Mensaje = 'Revisar el codigo , el usado ya es utilizado para otro producto'
end


alter Proc SP_ModificarProducto(
@IdProducto int ,
@Codigo varchar(50),
@Nombre varchar (50) ,
@Descripcion varchar(50),
@IdCategoria int ,
@Estado bit,
@Resultado int output ,
@Mensaje varchar (500) output
)
as
begin
	Set @Resultado=1
	if Not exists (Select *From PRODUCTO Where Codigo = @Codigo and IdProducto != @IdProducto)
		update PRODUCTO  
		 SET Codigo = @Codigo,
        Nombre = @Nombre,
        Descripcion = @Descripcion,
        IdCategoria = @IdCategoria,
        Estado = @Estado
		where IdProducto = @IdProducto
		else 
		begin 
			set @Resultado=0
			set @Mensaje = 'El codigo con el que intenaste nombrar al producto ya esta utilizado usa otro'
		end 
end


Create Proc SP_EliminarProducto(
@IdProducto int ,
@Resultado int output ,
@Mensaje varchar (500) output)
as
begin 
	set @Resultado= 0
	set @Mensaje=''
	declare @VerificacionCorrecta bit=1
	If exists (Select * from DETALLE_COMPRA dc 
	Inner Join Producto p On p.IdProducto = dc.IdProducto
	Where p.IdProducto = @IdProducto)
	Begin 
		set  @VerificacionCorrecta = 0
		set @Resultado= 0
		set @Mensaje= @Mensaje + 'El producto esta relacionado con una compra no se lo puede eliminar'
	End
	If exists (Select * from DETALLE_VENTA dv 
	Inner Join Producto p On p.IdProducto = dv.IdProducto
	Where p.IdProducto = @IdProducto)
	Begin 
		set  @VerificacionCorrecta = 0
		set @Resultado= 0
		set @Mensaje= @Mensaje + 'El producto esta relacionado con una venta no se lo puede eliminar'
	End
	if(@VerificacionCorrecta=1)
		begin 
			delete from PRODUCTO where IdProducto=@IdProducto
			set @Resultado=1
		end
end


--Procedurales para clientes
Create Proc SP_RegistrarCliente(
@Documento varchar(50),
@NombreCompleto varchar(50),
@Correo varchar(50),
@Telefono varchar (50),
@DireccionDomicilio varchar(100),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output 
)
as
begin 
	Set @Resultado =0
	Declare @IdPersona Int
	if Not exists(Select * from CLIENTE Where Documento = @Documento)
	begin 
		insert into Cliente(Documento,NombreCompleto,Correo,Telefono,DireccionDomicilio,Estado) Values 
		(@Documento,@NombreCompleto,@Correo,@Telefono,@DireccionDomicilio,@Estado)
		set @Resultado =  SCOPE_IDENTITY()
	end
	else
		set @Mensaje = 'El numero de documentacion ya existe ,intentar con otro'
end

Create Proc SP_ModificarCliente(
@IdCliente int ,
@Documento varchar(50),
@NombreCompleto varchar(50),
@Correo varchar(50),
@Telefono varchar (50),
@DireccionDomicilio varchar(100),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output 
)
as
begin 
	Set @Resultado =1
	Declare @IdPersona Int
	if Not exists(Select * from CLIENTE Where Documento = @Documento and IdCliente = @IdCliente)
	begin 
		Update CLIENTE set
		Documento = @Documento,
		NombreCompleto  = @NombreCompleto,
		Correo = @Correo,
		Telefono = @Telefono,
		DireccionDomicilio  =@DireccionDomicilio,
		Estado =@Estado
		Where IdCliente = @IdCliente
		end
	else
		begin 
		set @Resultado = 0
		set @Mensaje = 'El numero de documentacion ya existe ,intentar con otro'
end
end

--Procedurales para proveedores
Select * From PROVEEDOR

Create Proc SP_RegistrarProveedores(
@Documento varchar(50),
@RazonSocial varchar(50),
@Correo varchar(50),
@Telefono varchar (50),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output 
)
as
begin 
	Set @Resultado =0
	Declare @IdPersona Int
	if Not exists(Select * from CLIENTE Where Documento = @Documento)
	begin 
		insert into PROVEEDOR(Documento,RazonSocial,Correo,Telefono,Estado) Values 
		(@Documento,@RazonSocial,@Correo,@Telefono,@Estado)
		set @Resultado =  SCOPE_IDENTITY()
	end
	else
		set @Mensaje = 'El numero de documentacion ya fue registrado con otro proveedor,intentar con uno nuevo'
end


Create Proc SP_ModificarProveedores( 
@IdProveedor int ,
@Documento varchar(50),
@RazonSocial varchar(50),
@Correo varchar(50),
@Telefono varchar (50),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output 
)
as
begin 
	Set @Resultado =1
	Declare @IdPersona Int
	if Not exists(Select * from PROVEEDOR Where Documento = @Documento and IdProveedor = @IdProveedor)
	begin 
		Update PROVEEDOR set
		Documento = @Documento,
		RazonSocial  = @RazonSocial,
		Correo = @Correo,
		Telefono = @Telefono,
		Estado =@Estado
		Where IdProveedor = @IdProveedor
		end
	else
		begin 
		set @Resultado = 0
		set @Mensaje = 'El numero de documentacion ya fue registrado con otro proveedor,intentar con uno nuevo'
end
end


Create Proc SP_EliminarProveedores(
@IdProveedor int ,
@Resultado int output ,
@Mensaje varchar (500) output)
as
begin 
	set @Resultado= 1
		if Not exists (
		Select * From PROVEEDOR p 
		inner join COMPRA c on p.IdProveedor = c.IdProveedor
		where p.IdProveedor = c.IdProveedor
		)
		begin 
			delete top(1) from PROVEEDOR where IdProveedor = @IdProveedor 
		end
		else 
		begin 
			set @Resultado= 0
			set @Mensaje= @Mensaje + 'El proveedor esta relacionado con una compra no se lo puede eliminar'
		end
	end

	
	Select*From PROVEEDOR
	Select IdProveedor ,Documento,RazonSocial,Correo,Telefono,Estado From PROVEEDOR