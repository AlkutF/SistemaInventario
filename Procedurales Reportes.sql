


Create Proc SP_ReporteCompra(
@fechaInicio varchar(10),
@Fechafin varchar(10),
@IdProveedor int 
)

as 
	begin
	Set DATEFORMAT dmy;
	Select 
		CONVERT (char(10),c.FechaRegistro,103)[FechaRegistro],c.TipoDocumento,c.NumeroDocumento,c.MontoTotal,
		u.NombreCompleto[UsuarioRegistro], pr.Documento[DocumentoProveedor] , pr.RazonSocial,
		p.Codigo[CodigoProducto] ,p.Nombre[NombreProducto], ca.Descripcion[Categoria],dc.PrecioCompra,dc.PrecioVenta,dc.Cantidad,dc.MontoTotal[SubTotal]
		From COMPRA c
		inner join USUARIO u on u.IdUsuario = c.IdUsuario
		inner join PROVEEDOR pr on pr.IdProveedor = c.IdProveedor
		inner join DETALLE_COMPRA dc on dc.IdCompra = c.IdCompra
		inner join PRODUCTO p on p.IdProducto = dc.IdProducto
		inner join CATEGORIA ca on ca.IdCategoria = p.IdCategoria
		Where CONVERT (Date ,c.FechaRegistro) between @fechaInicio and @Fechafin
		and pr.IdProveedor = iif(@IdProveedor =0 , pr.IdProveedor ,@IdProveedor) 
	end


	Create Proc SP_ReporteVenta(
@fechaInicio varchar(10),
@Fechafin varchar(10)
)

as 
	begin
	Set DATEFORMAT dmy;
	Select 
		CONVERT (char(10),v.FechaRegistro,103)[FechaRegistro],v.TipoDocumento,v.NumeroDocumento,v.MontoTotal,
		u.NombreCompleto[UsuarioRegistro], 
		v.DocumentoCliente , v.NombreCliente,
		p.Codigo[CodigoProducto] ,p.Nombre[NombreProducto], ca.Descripcion[Categoria],dv.PrecioVenta,dv.Cantidad,dv.Subtotal
		From Venta v
		inner join USUARIO u on u.IdUsuario = v.IdUsuario
		inner join DETALLE_VENTA dv on dv.IdVenta = v.IdVenta
		inner join PRODUCTO p on p.IdProducto = dv.IdProducto
		inner join CATEGORIA ca on ca.IdCategoria = p.IdCategoria
		Where CONVERT (Date ,v.FechaRegistro) between @fechaInicio and @Fechafin
	end




	exec SP_ReporteVenta'04/10/2023' , '11/02/2024' 


exec SP_ReporteCompra  '04/10/2023' , '11/02/2024' ,0




		Select 
		CONVERT (char(10),c.FechaRegistro,103)[FechaRegistro],c.TipoDocumento,c.NumeroDocumento,c.MontoTotal,
		u.NombreCompleto[UsuarioRegistro], pr.Documento[DocumentoProveedor] , pr.RazonSocial,
		p.Codigo[CodigoProducto] ,p.Nombre[NombreProducto], ca.Descripcion[Categoria],dc.PrecioCompra,dc.PrecioVenta,dc.Cantidad,dc.MontoTotal[SubTotal]
		From COMPRA c
		inner join USUARIO u on u.IdUsuario = c.IdUsuario
		inner join PROVEEDOR pr on pr.IdProveedor = c.IdProveedor
		inner join DETALLE_COMPRA dc on dc.IdCompra = c.IdCompra
		inner join PRODUCTO p on p.IdProducto = dc.IdProducto
		inner join CATEGORIA ca on ca.IdCategoria = p.IdCategoria
		Where CONVERT (Date ,c.FechaRegistro) between '04/10/2023' and '11/02/2024'
		and pr.IdProveedor = iif(3 =0 , pr.IdProveedor ,3) 