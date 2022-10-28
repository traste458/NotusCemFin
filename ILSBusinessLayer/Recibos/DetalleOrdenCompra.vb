Imports LMDataAccessLayer

Namespace Recibos
    Public Class DetalleOrdenCompra

#Region "variables"
        Private _idDetalle As Long
        Private _idOrden As Long
        Private _idFabricante As Integer
        Private _idProducto As Long
        Private _idTipoUnidad As Integer
        Private _cantidad As Integer
        Private _valorUnitario As Long
        Private _idUsuario As Long
        Private _fechaRegistro As Date
        Private _observacion As String
        Private _producto As String
        Private _fabricante As String
        Private _idTipoDetalle As Short
        Private _eliminarPreinstrucciones As Boolean
        Private _tipoDetalleOrdenCompra As TipoDetalleOrdenCompra
        Private _listaPreInstrucciones As OMS.PreinstruccionClienteColeccion
        Private _dtErrores As DataTable
        Private _mensajeInfo As String
#End Region

#Region "propiedades"

        Public Property ListaErrores() As DataTable
            Get
                If _dtErrores Is Nothing Then _dtErrores = New DataTable
                Return _dtErrores
            End Get
            Set(ByVal value As DataTable)
                _dtErrores = value
            End Set
        End Property
       
        Public ReadOnly Property PreInstrucciones() As OMS.PreinstruccionClienteColeccion
            Get
                Return _listaPreInstrucciones
            End Get
        End Property

        Public Property Fabricante() As String
            Get
                Return _fabricante
            End Get
            Set(ByVal value As String)
                _fabricante = value
            End Set
        End Property

        Public Property IdDetalle() As Long
            Get
                Return _idDetalle
            End Get
            Set(ByVal value As Long)
                _idDetalle = value
            End Set
        End Property

        Public Property IdOrden() As Long
            Get
                Return _idOrden
            End Get
            Set(ByVal value As Long)
                _idOrden = value
            End Set
        End Property

        Public Property IdFabricante() As Integer
            Get
                Return _idFabricante
            End Get
            Set(ByVal value As Integer)
                _idFabricante = value
            End Set
        End Property

        Public Property IdProducto() As Long
            Get
                Return _idProducto
            End Get
            Set(ByVal value As Long)
                _idProducto = value
            End Set
        End Property

        Public Property IdTipoUnidad() As Integer
            Get
                Return _idTipoUnidad
            End Get
            Set(ByVal value As Integer)
                _idTipoUnidad = value
            End Set
        End Property

        Public Property Cantidad() As Integer
            Get
                Return _cantidad
            End Get
            Set(ByVal value As Integer)
                _cantidad = value
            End Set
        End Property

        Public Property ValorUnitario() As Long
            Get
                Return _valorUnitario
            End Get
            Set(ByVal value As Long)
                _valorUnitario = value
            End Set
        End Property

        Public Property IdUsuario() As Long
            Get
                Return _idUsuario
            End Get
            Set(ByVal value As Long)
                _idUsuario = value
            End Set
        End Property

        Public Property FechaRegistro() As Date
            Get
                Return _fechaRegistro
            End Get
            Set(ByVal value As Date)
                _fechaRegistro = value
            End Set
        End Property

        Public Property Observacion() As String
            Get
                Return _observacion
            End Get
            Set(ByVal value As String)
                _observacion = value
            End Set
        End Property

        Public ReadOnly Property Producto() As String
            Get
                Return _producto
            End Get

        End Property

        Public Property EliminarPreinstrucciones() As Boolean
            Get
                Return _eliminarPreinstrucciones
            End Get
            Set(ByVal value As Boolean)
                _eliminarPreinstrucciones = value
            End Set
        End Property

        Public Property IdTipoDetalleOrdenCompra() As Short
            Get
                Return _idTipoDetalle
            End Get
            Set(ByVal value As Short)
                _idTipoDetalle = value
            End Set
        End Property

        Public ReadOnly Property TipoDetalleOrdenCompra() As TipoDetalleOrdenCompra
            Get
                If _idTipoDetalle > 0 Then
                    Return New TipoDetalleOrdenCompra(_idTipoDetalle)
                Else
                    Return New TipoDetalleOrdenCompra()
                End If
            End Get
        End Property

        Public ReadOnly Property MensajeInfo() As String
            Get
                Return _mensajeInfo
            End Get
        End Property

#End Region

#Region "constructores"
        Public Sub New()
            MyBase.New()
            _listaPreInstrucciones = New OMS.PreinstruccionClienteColeccion
            _mensajeInfo = String.Empty
        End Sub
        Public Sub New(ByVal idDetalle As Long)
            Me.New()
            _idDetalle = idDetalle
            Me.CargarDatos()
            _mensajeInfo = String.Empty
        End Sub

        Public Sub New(ByVal idOrdenCompra As Long, ByVal idProducto As Integer)
            Me.New()
            Me.CargarDatos(idOrdenCompra, idProducto)
            _mensajeInfo = String.Empty
        End Sub
#End Region

#Region "metodos Privados"

        Private Sub CargarDatos()
            Dim db As New LMDataAccess
            db.SqlParametros.Add("@idDetalle", SqlDbType.BigInt).Value = _idDetalle
            Try
                db.ejecutarReader("ObtenerInfoDetalleOrdenCompra", CommandType.StoredProcedure)
                If db.Reader.Read Then
                    _idDetalle = db.Reader("idDetalle")
                    _idOrden = db.Reader("idOrden")
                    _idFabricante = db.Reader("idFabricante")
                    _idProducto = db.Reader("idProducto")
                    _idTipoUnidad = db.Reader("idTipoUnidad")
                    _cantidad = db.Reader("cantidad")
                    _valorUnitario = db.Reader("valorUnitario")
                    _fechaRegistro = db.Reader("fechaRegistro")
                    _observacion = db.Reader("observacion").ToString
                    _fabricante = db.Reader("fabricante")
                    _producto = db.Reader("producto")
                    '_fechaRegistro = db.Reader("fechaRegistro")
                End If
            Catch ex As Exception
            Finally
                If Not db.Reader.IsClosed Then db.Reader.Close()
                db.Dispose()
            End Try
        End Sub

        Private Sub CargarDatos(ByVal idOrdenCompra As Long, ByVal idProducto As Integer)
            Dim db As New LMDataAccess
            db.SqlParametros.Add("@idOrden", SqlDbType.BigInt).Value = idOrdenCompra
            db.SqlParametros.Add("@idProducto", SqlDbType.Int).Value = idProducto
            Try
                db.ejecutarReader("ObtenerInfoDetalleOrdenCompra", CommandType.StoredProcedure)
                If db.Reader.Read Then
                    _idDetalle = db.Reader("idDetalle")
                    _idOrden = db.Reader("idOrden")
                    _idFabricante = db.Reader("idFabricante")
                    _idProducto = db.Reader("idProducto")
                    _idTipoUnidad = db.Reader("idTipoUnidad")
                    _cantidad = db.Reader("cantidad")
                    _valorUnitario = db.Reader("valorUnitario")
                    _fechaRegistro = db.Reader("fechaRegistro")
                    _observacion = db.Reader("observacion").ToString
                    _fabricante = db.Reader("fabricante")
                    _producto = db.Reader("producto")
                    '_fechaRegistro = db.Reader("fechaRegistro")
                End If
            Catch ex As Exception
            Finally
                If Not db.Reader.IsClosed Then db.Reader.Close()
                db.Dispose()
            End Try
        End Sub
#End Region

#Region "metodos Publicos"

        Public Function Crear() As Boolean
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim retorno As Boolean
            With db
                With .SqlParametros
                    .Add("@idOrden", SqlDbType.BigInt).Value = _idOrden
                    .Add("@idFabricante", SqlDbType.Int).Value = _idFabricante
                    .Add("@idProducto", SqlDbType.Int).Value = _idProducto
                    .Add("@idTipoUnidad", SqlDbType.SmallInt).Value = _idTipoUnidad
                    .Add("@cantidad", SqlDbType.Int).Value = _cantidad
                    .Add("@valorUnitario", SqlDbType.BigInt).Value = _valorUnitario
                    .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .Add("@observacion", SqlDbType.VarChar).Value = _observacion
                    .Add("@idTipoDetalle", SqlDbType.SmallInt).Value = _idTipoDetalle
                    .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                End With

                Try
                    Dim result As Integer = 0
                    .iniciarTransaccion()
                    .ejecutarNonQuery("CrearInfoDetalleOrdenCompra", CommandType.StoredProcedure)
                    result = .SqlParametros("@result").Value
                    If result = 0 Then
                        _idDetalle = CLng(.SqlParametros("@identity").Value)
                        .confirmarTransaccion()
                        retorno = True
                    End If

                Catch ex As Exception
                    If .estadoTransaccional Then .abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    .cerrarConexion()
                    .Dispose()
                End Try
            End With
            Return retorno
        End Function

        Public Sub Actualizar()
            If _idDetalle <> 0 Then
                Dim db As New LMDataAccess

                Try
                    db.iniciarTransaccion()
                    With db.SqlParametros
                        .Add("@idDetalle", SqlDbType.BigInt).Value = _idDetalle
                        .Add("@idOrden", SqlDbType.BigInt).Value = _idOrden
                        .Add("@idFabricante", SqlDbType.Int).Value = _idFabricante
                        .Add("@idProducto", SqlDbType.Int).Value = _idProducto
                        .Add("@idTipoUnidad", SqlDbType.Int).Value = _idTipoUnidad
                        .Add("@cantidad", SqlDbType.Int).Value = _cantidad
                        .Add("@valorUnitario", SqlDbType.BigInt).Value = _valorUnitario
                        .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                        .Add("@observacion", SqlDbType.VarChar).Value = _observacion.ToString
                        .Add("@idTipoDetalle", SqlDbType.SmallInt).Value = _idTipoDetalle
                    End With
                    db.ejecutarNonQuery("ActualizarInfoDetalleOrdenCompra", CommandType.StoredProcedure)
                    db.confirmarTransaccion()
                Catch ex As Exception
                    If db.estadoTransaccional Then db.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    db.cerrarConexion()
                End Try
            Else
                Throw New DuplicateNameException("El detalle de la Orden aún no ha sido registrada en la Base de Datos.")
            End If
        End Sub

        Public Sub InstruccionarTodo(ByVal listaPrealertas As List(Of DetalleOrdenCompra))
            Dim db As New LMDataAccess
            _dtErrores = New DataTable
            _dtErrores.Columns.Add(New DataColumn("idDetalleOrdenCompra", GetType(Integer)))
            _dtErrores.Columns.Add(New DataColumn("mensajeError", GetType(String)))
            Try
                db.iniciarTransaccion()
                For Each prealerta As DetalleOrdenCompra In listaPrealertas
                    If prealerta.EliminarPreinstrucciones Then
                        prealerta.PreInstrucciones.Procesar(db, _dtErrores)
                    ElseIf prealerta.PreInstrucciones.TotalInstruccionado <= prealerta.Cantidad Then
                        prealerta.PreInstrucciones.Procesar(db, _dtErrores)
                    Else
                        RegistrarError(_dtErrores, prealerta.IdOrden, "La sumatoria de la cantidad instruccionada es mayor a la cantidad de la prealerta")
                    End If
                Next
                If _dtErrores.Rows.Count = 0 Then
                    db.confirmarTransaccion()
                Else
                    db.abortarTransaccion()
                End If

            Catch ex As Exception
                If db.estadoTransaccional Then db.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                db.Dispose()
            End Try

        End Sub

        Friend Shared Sub RegistrarError(ByVal dtErrores As DataTable, ByVal idDetalleOrdenCompra As Integer, ByVal mensaje As String)
            Dim dr As DataRow = dtErrores.NewRow()
            dr("idDetalleOrdenCompra") = idDetalleOrdenCompra
            dr("mensajeError") = mensaje
            dtErrores.Rows.Add(dr)
        End Sub

#End Region

#Region "métodos compartidos"
        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New Estructuras.FiltroDetalleOrdenCompra
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As Estructuras.FiltroDetalleOrdenCompra) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdDetalle > 0 Then db.SqlParametros.Add("@idDetalle", SqlDbType.Int).Value = .IdDetalle
                If .IdOrden > 0 Then db.SqlParametros.Add("@idOrden", SqlDbType.Int).Value = .IdOrden
                If .IdFabricante > 0 Then db.SqlParametros.Add("@idFabricante", SqlDbType.Int).Value = .IdFabricante
                If .IdProducto > 0 Then db.SqlParametros.Add("@idProducto", SqlDbType.Int).Value = .IdProducto
                If .IdTipoDetalle > 0 Then db.SqlParametros.Add("@idTipoDetalle", SqlDbType.SmallInt).Value = .IdTipoDetalle
                dtDatos = db.ejecutarDataTable("ObtenerInfoDetalleOrdenCompra", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos
        End Function

        Public Shared Sub EliminarDetalle(ByVal idDetalle As Long)
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim dtProductosOrdenRecepcion As New DataTable
            If idDetalle > 0 Then
                With dbManager
                    .SqlParametros.Add("@idDetalle", SqlDbType.BigInt).Value = idDetalle
                    .ejecutarNonQuery("EliminarDetalleDeOrdenCompra", CommandType.StoredProcedure)
                End With
            End If
        End Sub

        Public Shared Function ObtenerPoolPrealertasPendientes(Optional ByVal idOrdenCompra As Long = 0, Optional ByVal idDetalleOrdenCompra As Long = 0) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtAux As DataTable
            Try
                With dbManager
                    .TiempoEsperaComando = 600
                    If idOrdenCompra > 0 Then .SqlParametros.Add("@idOrdenCompra", SqlDbType.BigInt).Value = idOrdenCompra
                    If idDetalleOrdenCompra > 0 Then .SqlParametros.Add("@idDetalleOrdenCompra", SqlDbType.BigInt).Value = idDetalleOrdenCompra
                    dtAux = .ejecutarDataTable("ObtenerPoolInstruccionOrdenCompra", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtAux
        End Function

        Public Shared Function ObtenerPoolInstruccionesOC() As DataTable
            Dim db As New LMDataAccess
            db.TiempoEsperaComando = 600
            Dim dt As DataTable = db.ejecutarDataTable("ObtenerPoolInstruccionOrdenCompra", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Shared Function ObtenerPoolInstruccionesOC(ByVal FiltroPreinstruccion As Estructuras.FiltroPreinstruccion) As DataTable
            Dim db As New LMDataAccess
            With FiltroPreinstruccion
                If .IdOrdenCompra > 0 Then db.SqlParametros.Add("@idOrdenCompra", SqlDbType.Int).Value = .IdOrdenCompra
                If .NumeroOrdenCompra <> String.Empty Then db.SqlParametros.Add("@numeroOrdenCompra", SqlDbType.VarChar).Value = .NumeroOrdenCompra
                If .Factura <> String.Empty Then db.SqlParametros.Add("@factura", SqlDbType.VarChar).Value = .Factura
                If .IdProducto > 0 Then db.SqlParametros.Add("@idProducto", SqlDbType.Int).Value = .IdProducto
                If .IdFabricante > 0 Then db.SqlParametros.Add("@idFabricante", SqlDbType.Int).Value = .IdFabricante
                If .FechaInicial <> Date.MinValue Then db.SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = .FechaInicial
                If .FechaFinal <> Date.MinValue Then db.SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = .FechaFinal
                If .IdEstado > 0 Then db.SqlParametros.Add("@idEstado", SqlDbType.Int).Value = .IdEstado
            End With
            db.TiempoEsperaComando = 600
            Dim dt As DataTable = db.ejecutarDataTable("ObtenerPoolInstruccionOrdenCompraFiltro", CommandType.StoredProcedure)
            Return dt
        End Function

        ''' <summary>
        ''' Verifica si la orden de la instancia actual cumple las condiciones para adicionar una factura
        ''' si no es asi en la propiedad MensajeInfo quedan cargadas la razones.
        ''' </summary>
        ''' <returns>Retorna TRUE si se puede adicionar o FALSE de lo contrario</returns>
        ''' <remarks></remarks>
        Public Function PosibleAdicionarFactura() As Boolean
            Dim retorno As Short
            Dim db As New LMDataAccess
            Try
                Me._mensajeInfo = String.Empty
                With db
                    With .SqlParametros
                        .Add("@idOperacion", SqlDbType.Int).Value = 1
                        .Add("@idDetalle", SqlDbType.BigInt).Value = Me.IdDetalle
                        .Add("@mensaje", SqlDbType.VarChar, 8000).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue
                    End With
                    .ejecutarNonQuery("EsPosibleOperacionInfoFactura", CommandType.StoredProcedure)
                    Short.TryParse(.SqlParametros("@result").Value.ToString, retorno)
                    If retorno = 0 Then
                        Me._mensajeInfo = .SqlParametros("@mensaje").Value.ToString
                    End If
                End With
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                db.Dispose()
            End Try
            Return CBool(retorno)
        End Function

        ''' <summary>
        ''' Verifica si es posible eliminar la factura indicada
        ''' si no es asi en la propiedad MensajeInfo quedan cargadas la razones.
        ''' </summary>
        ''' <returns>Retorna TRUE si se puede eliminar o FALSE de lo contrario</returns>
        ''' <remarks></remarks>
        Public Function PosibleEliminarFactura(ByVal idFactura As Long) As Boolean
            Dim retorno As Short
            Dim db As New LMDataAccess
            Try
                Me._mensajeInfo = String.Empty
                With db
                    With .SqlParametros
                        .Add("@idOperacion", SqlDbType.Int).Value = 2
                        .Add("@idFactura", SqlDbType.BigInt).Value = idFactura
                        .Add("@mensaje", SqlDbType.VarChar, 8000).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue
                    End With
                    .ejecutarNonQuery("EsPosibleOperacionInfoFactura", CommandType.StoredProcedure)
                    Short.TryParse(.SqlParametros("@result").Value.ToString, retorno)
                    If retorno = 0 Then
                        Me._mensajeInfo = .SqlParametros("@mensaje").Value.ToString
                    End If
                End With
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                db.Dispose()
            End Try
            Return CBool(retorno)
        End Function

        ''' <summary>
        ''' Verifica si es posible editar toda la información asocida a la factura
        ''' si no es asi en la propiedad MensajeInfo quedan cargadas la razones.
        ''' </summary>
        ''' <returns>Retorna TRUE si se puede editar toda la factura o FALSE de lo contrario</returns>
        ''' <remarks></remarks>
        Public Function PosibleEditarTodaFactura(ByVal idFactura As Long) As Boolean
            Dim retorno As Short
            Dim db As New LMDataAccess
            Try
                Me._mensajeInfo = String.Empty
                With db
                    With .SqlParametros
                        .Add("@idOperacion", SqlDbType.Int).Value = 3
                        .Add("@idFactura", SqlDbType.BigInt).Value = idFactura
                        .Add("@mensaje", SqlDbType.VarChar, 8000).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue
                    End With
                    .ejecutarNonQuery("EsPosibleOperacionInfoFactura", CommandType.StoredProcedure)
                    Short.TryParse(.SqlParametros("@result").Value.ToString, retorno)
                    If retorno = 0 Then
                        Me._mensajeInfo = .SqlParametros("@mensaje").Value.ToString
                    End If
                End With
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                db.Dispose()
            End Try
            Return CBool(retorno)
        End Function

#End Region

    End Class
End Namespace

