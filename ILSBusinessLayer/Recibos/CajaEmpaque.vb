Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Enumerados
Imports LMDataAccessLayer

Namespace Recibos

    Public Class CajaEmpaque

#Region "Campos"

        Private _idCaja As Long
        Private _idOrdenRecepcion As Long
        Private _idProducto As Integer
        Private _producto As String
        Private _idTipoProducto As Integer
        Private _material As String
        Private _referenciaMaterial As String
        Private _idRegion As Short
        Private _region As String
        Private _cantidad As Integer
        Private _idTipoUnidad As Short
        Private _unidadEmpaque As String
        Private _codigoUnidadEmpaque As String
        Private _idDetallePallet As Long
        Private _idPallet As Long
        Private _idCreador As Integer
        Private _creador As String
        Private _fechaRegistro As Date
        Private _idTipoDetalleProducto As Short
        Private _cantidadProducida As Integer
        Private _remision As String
        Private _fechaRecepcion As Date
        Private _registrado As Boolean
        Private _idEstado As Integer
        Private _productoPrincipal As String

#End Region

#Region "Propiedades"

        Public Property IdCaja() As Long
            Get
                Return _idCaja
            End Get
            Set(ByVal value As Long)
                _idCaja = value
            End Set
        End Property

        Public Property IdOrdenRecepcion() As Long
            Get
                Return _idOrdenRecepcion
            End Get
            Set(ByVal value As Long)
                _idOrdenRecepcion = value
            End Set
        End Property

        Public Property IdProducto() As Integer
            Get
                Return _idProducto
            End Get
            Set(ByVal value As Integer)
                _idProducto = value
            End Set
        End Property

        Public Property IdTipoProducto() As Integer
            Get
                Return _idTipoProducto
            End Get
            Set(ByVal value As Integer)
                _idTipoProducto = value
            End Set
        End Property

        Public Property Material() As String
            Get
                Return _material
            End Get
            Set(ByVal value As String)
                _material = value
            End Set
        End Property

        Public Property DescripcionMaterial() As String
            Get
                Return _referenciaMaterial
            End Get
            Protected Friend Set(ByVal value As String)
                _referenciaMaterial = value
            End Set
        End Property

        Public ReadOnly Property Producto() As String
            Get
                Return _producto
            End Get
        End Property

        Public Property IdRegion() As Short
            Get
                Return _idRegion
            End Get
            Set(ByVal value As Short)
                _idRegion = value
            End Set
        End Property

        Public ReadOnly Property Region() As String
            Get
                Return _region
            End Get
        End Property

        Public Property Cantidad() As Integer
            Get
                Return _cantidad
            End Get
            Set(ByVal value As Integer)
                _cantidad = value
            End Set
        End Property

        Public Property IdTipoUnidad() As Short
            Get
                Return _idTipoUnidad
            End Get
            Set(ByVal value As Short)
                _idTipoUnidad = value
            End Set
        End Property

        Public ReadOnly Property UnidadEmpaque() As String
            Get
                Return _unidadEmpaque
            End Get
        End Property

        Public Property CodigoUnidadEmpaque() As String
            Get
                Return _codigoUnidadEmpaque
            End Get
            Protected Friend Set(ByVal value As String)
                _codigoUnidadEmpaque = value
            End Set
        End Property

        Public Property IdDetallePallet() As Long
            Get
                Return _idDetallePallet
            End Get
            Set(ByVal value As Long)
                _idDetallePallet = value
            End Set
        End Property

        Public ReadOnly Property IdPallet() As Long
            Get
                Return _idPallet
            End Get
        End Property

        Public Property IdCreador() As Integer
            Get
                Return _idCreador
            End Get
            Set(ByVal value As Integer)
                _idCreador = value
            End Set
        End Property

        Public ReadOnly Property Creador() As String
            Get
                Return _creador
            End Get
        End Property

        Public Property FechaRegistro() As Date
            Get
                Return _fechaRegistro
            End Get
            Set(ByVal value As Date)
                _fechaRegistro = value
            End Set
        End Property

        Public Property IdTipoDetalleProducto() As Short
            Get
                Return _idTipoDetalleProducto
            End Get
            Set(ByVal value As Short)
                _idTipoDetalleProducto = value
            End Set
        End Property

        Public Property CantidadProducida() As Integer
            Get
                Return _cantidadProducida
            End Get
            Set(ByVal value As Integer)
                _cantidadProducida = value
            End Set
        End Property

        Public Property Remision() As String
            Get
                Return _remision
            End Get
            Protected Friend Set(ByVal value As String)
                _remision = value
            End Set
        End Property

        Public Property FechaRecepcion() As Date
            Get
                Return _fechaRecepcion
            End Get
            Protected Friend Set(ByVal value As Date)
                _fechaRecepcion = value
            End Set
        End Property

        Public Property Registrado()
            Get
                Return _registrado
            End Get
            Protected Friend Set(ByVal value)
                _registrado = value
            End Set
        End Property

        Public Property IdEstado() As Integer
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Integer)
                _idEstado = value
            End Set
        End Property

        Public Property ProductoPrincipal() As String
            Get
                Return _productoPrincipal
            End Get
            Set(value As String)
                _productoPrincipal = value
            End Set
        End Property
#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            _idCaja = identificador
            CargarInformacion()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            Dim dbManager As New LMDataAccess

            Try
                With dbManager
                    .SqlParametros.Add("@idCaja", SqlDbType.BigInt).Value = _idCaja
                    .ejecutarReader("ObtenerInfoCajaEmpaque", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing AndAlso .Reader.Read Then
                        Long.TryParse(.Reader("idCaja").ToString, _idCaja)
                        Long.TryParse(.Reader("idOrdenRecepcion").ToString, _idOrdenRecepcion)
                        Integer.TryParse(.Reader("idProducto").ToString, _idProducto)
                        Integer.TryParse(.Reader("idTipoProducto").ToString, _idTipoProducto)
                        _producto = .Reader("producto").ToString()
                        _material = .Reader("material").ToString()
                        _referenciaMaterial = .Reader("referenciaMaterial").ToString()
                        Short.TryParse(.Reader("idRegion").ToString, _idRegion)
                        _region = .Reader("region").ToString
                        Integer.TryParse(.Reader("cantidad").ToString, _cantidad)
                        Short.TryParse(.Reader("idTipoUnidad").ToString, _idTipoUnidad)
                        _unidadEmpaque = .Reader("unidadEmpaque").ToString
                        _codigoUnidadEmpaque = .Reader("codigoUnidadEmpaque").ToString
                        Long.TryParse(.Reader("idDetallePallet").ToString, _idDetallePallet)
                        Long.TryParse(.Reader("idPallet").ToString, _idPallet)
                        Integer.TryParse(.Reader("idCreador").ToString, _idCreador)
                        _creador = .Reader("creador").ToString
                        Date.TryParse(.Reader("fechaRegistro").ToString, _fechaRegistro)
                        Short.TryParse(.Reader("idTipoDetalleProducto").ToString, _idTipoDetalleProducto)
                        Integer.TryParse(.Reader("cantidadProducida").ToString, _cantidadProducida)
                        _remision = .Reader("remision").ToString
                        Date.TryParse(.Reader("fechaRecepcion").ToString, _fechaRecepcion)
                        Integer.TryParse(.Reader("idEstado").ToString, _idEstado)
                        _registrado = True
                    End If
                    If .Reader IsNot Nothing Then .Reader.Close()
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As Short
            Dim resultado As Short
            If _idOrdenRecepcion > 0 And _cantidad > 0 And _idCreador > 0 Then
                Dim dbManager As New LMDataAccess

                Try
                    With dbManager
                        .SqlParametros.Add("@idOrdenRecepcion", SqlDbType.BigInt).Value = _idOrdenRecepcion
                        .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = _idProducto
                        If _material <> String.Empty Then .SqlParametros.Add("@material", SqlDbType.VarChar).Value = _material
                        If _idRegion > 0 Then .SqlParametros.Add("@idRegion", SqlDbType.SmallInt).Value = _idRegion
                        .SqlParametros.Add("@cantidad", SqlDbType.Int).Value = _cantidad
                        .SqlParametros.Add("@idTipoUnidad", SqlDbType.SmallInt).IsNullable = True
                        .SqlParametros("@idTipoUnidad").Value = IIf(_idTipoUnidad <> 0, _idTipoUnidad, DBNull.Value)
                        .SqlParametros.Add("@idDetallePallet", SqlDbType.BigInt).IsNullable = True
                        .SqlParametros("@idDetallePallet").Value = IIf(_idDetallePallet <> 0, _idDetallePallet, DBNull.Value)
                        .SqlParametros.Add("@idCreador", SqlDbType.Int).Value = _idCreador
                        .SqlParametros.Add("@idTipoDetalleProducto", SqlDbType.SmallInt).Value = _idTipoDetalleProducto
						.SqlParametros.Add("@productoPrincipal", SqlDbType.VarChar).Value = _productoPrincipal
                        .SqlParametros.Add("@idCaja", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("RegistrarCajaEmpaque", CommandType.StoredProcedure)
                        resultado = CShort(.SqlParametros("@returnValue").Value)
                        If resultado = 0 Then _idCaja = CLng(.SqlParametros("@idCaja").Value)
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 2
            End If
            Return resultado
        End Function

        Public Function Actualizar() As Short
            Dim resultado As Short
            'If _idTipoProducto <> 0 And _descripcion.Trim.Length > 0 And _idTipoUnidad <> 0 Then
            '    Dim dbManager As New LMDataAccess

            '    Try
            '        With dbManager
            '            .SqlParametros.Add("@idTipoProducto", SqlDbType.SmallInt).Value = _idTipoProducto
            '            .SqlParametros.Add("@descripcion", SqlDbType.VarChar, 50).Value = _descripcion
            '            .SqlParametros.Add("@estado", SqlDbType.Bit).Value = _estado
            '            .SqlParametros.Add("@instruccionable", SqlDbType.Bit).Value = _instruccionable
            '            .SqlParametros.Add("@idTipoUnidad", SqlDbType.SmallInt).IsNullable = True
            '            .SqlParametros.Add("@aplicaTecnologia", SqlDbType.Bit).Value = _aplicaTecnologia
            '            .SqlParametros("@idTipoUnidad").Value = IIf(_idTipoUnidad <> 0, _idTipoUnidad, DBNull.Value)
            '            .SqlParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
            '            .ejecutarNonQuery("ActualizarTipoProducto", CommandType.StoredProcedure)
            '            resultado = CShort(.SqlParametros("@returnValue").Value)
            '        End With
            '    Finally
            '        If dbManager IsNot Nothing Then dbManager.Dispose()
            '    End Try
            'Else
            '    resultado = 3
            'End If
            Return resultado
        End Function

        Public Function Anular() As Short
            Dim resultado As Short
            If _idCaja Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idCaja", SqlDbType.BigInt).Value = _idCaja
                        .SqlParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("AnularCajaEmpaque", CommandType.StoredProcedure)
                        resultado = CShort(.SqlParametros("@returnValue").Value)
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 3
            End If
            Return resultado
        End Function

#End Region

#Region "Métodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroCajaEmpaque
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroCajaEmpaque) As DataTable
            Dim dtDatos As New DataTable
            Dim dbManager As New LMDataAccess
            With filtro
                If .IdCaja > 0 Then dbManager.SqlParametros.Add("@idCaja", SqlDbType.BigInt).Value = .IdCaja
                If .IdOrdenRecepcion > 0 Then dbManager.SqlParametros.Add("@idOrdenRecepcion", SqlDbType.BigInt).Value = .IdOrdenRecepcion
                If .IdDetallePallet > 0 Then dbManager.SqlParametros.Add("@idDetallePallet", SqlDbType.BigInt).Value = .IdDetallePallet
                If .IdPallet > 0 Then dbManager.SqlParametros.Add("@idPallet", SqlDbType.BigInt).Value = .IdPallet
                If .IdProducto > 0 Then dbManager.SqlParametros.Add("@idProducto", SqlDbType.Int).Value = .IdProducto
                If .Material <> String.Empty Then dbManager.SqlParametros.Add("@material", SqlDbType.VarChar).Value = .Material
                If .IdRegion > 0 Then dbManager.SqlParametros.Add("@idRegion", SqlDbType.SmallInt).Value = .IdRegion
                If .IdEstado > 0 Then dbManager.SqlParametros.Add("@idEstado", SqlDbType.SmallInt).Value = .IdEstado
                If .IdTipoDetalleProducto > 0 Then dbManager.SqlParametros.Add("@idTipoDetalleProducto", SqlDbType.SmallInt).Value = .IdTipoDetalleProducto
                dtDatos = dbManager.ejecutarDataTable("ObtenerInfoCajaEmpaque", CommandType.StoredProcedure)
            End With
            Return dtDatos
        End Function

        Public Shared Function ObtenerPorId(ByVal identificador As Short) As DataTable
            Dim dtDatos As DataTable
            Dim filtro As New FiltroCajaEmpaque
            filtro.IdCaja = identificador
            dtDatos = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Sub LlenarListado(ByVal filtro As FiltroCajaEmpaque, ByVal dtCajas As DataTable)
            Dim dbManager As New LMDataAccess
            With filtro
                With dbManager.SqlParametros
                    If filtro.IdCaja > 0 Then .Add("@idCaja", SqlDbType.BigInt).Value = filtro.IdCaja
                    If filtro.IdOrdenRecepcion > 0 Then .Add("@idOrdenRecepcion", SqlDbType.BigInt).Value = filtro.IdOrdenRecepcion
                    If filtro.IdDetallePallet > 0 Then .Add("@idDetallePallet", SqlDbType.BigInt).Value = filtro.IdDetallePallet
                    If filtro.IdPallet > 0 Then .Add("@idPallet", SqlDbType.BigInt).Value = filtro.IdPallet
                    If filtro.IdProducto > 0 Then .Add("@idProducto", SqlDbType.Int).Value = filtro.IdProducto
                    If filtro.Material <> String.Empty Then .Add("@material", SqlDbType.VarChar).Value = filtro.Material
                    If filtro.IdRegion > 0 Then .Add("@idRegion", SqlDbType.SmallInt).Value = filtro.IdRegion
                    If filtro.IdEstado > 0 Then .Add("@idEstado", SqlDbType.SmallInt).Value = filtro.IdEstado
                    If filtro.IdTipoDetalleProducto > 0 Then .Add("@idTipoDetalleProducto", SqlDbType.SmallInt).Value = filtro.IdTipoDetalleProducto
                End With
                dbManager.llenarDataTable(dtCajas, "ObtenerInfoCajaEmpaque", CommandType.StoredProcedure)
            End With
        End Sub

        ''' <summary>
        ''' Obtiene las cajas que estan pendientes por la asignación de pallet de recepción
        ''' </summary>
        ''' <param name="idOrdenCompra"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ObtenerCajasTemporales(Optional ByVal idOrdenCompra As Integer = 0) As DataTable
            Dim db As New LMDataAccess
            Dim dt As New DataTable
            Try
                If idOrdenCompra > 0 Then
                    db.SqlParametros.Add("@idOrdenCompra", SqlDbType.Int).Value = idOrdenCompra
                End If
                db.SqlParametros.Add("@idEstado", SqlDbType.Int).Value = 39
                dt = db.ejecutarDataTable("ObtenerInfoCajaEmpaque", CommandType.StoredProcedure)
            Catch ex As Exception
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
            Return dt
        End Function

        Public Shared Function ObtenerCantidadCargadaTemporal(ByVal idOrdenCompra As Integer) As Integer
            Dim retorno As Integer
            Dim dtCajasTemporales As DataTable            
            Try
                dtCajasTemporales = CajaEmpaque.ObtenerCajasTemporales(idOrdenCompra)                
                Integer.TryParse(dtCajasTemporales.Compute("SUM(cantidad)", "").ToString, retorno)
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
            Return retorno
        End Function

        Public Overloads Shared Sub LlenarListado(ByVal dtCajas As DataTable)
            Dim filtro As New FiltroCajaEmpaque
            LlenarListado(filtro, dtCajas)
        End Sub

        Public Shared Sub AdicionarSerialesAOtbCreadaEnRecepcion(ByVal idCaja As Long)
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idCajaEmpaque", SqlDbType.BigInt).Value = idCaja
                    .ejecutarNonQuery("AdicionarAOtbSerialesDeCaja", CommandType.StoredProcedure)
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.Dispose()
                Throw New Exception("Error al tratar de ingresar seriales de la Caja a la OTB generada por cantidades durante la recepción. " & ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

#End Region

    End Class

End Namespace
