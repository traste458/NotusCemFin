Imports ILSBusinessLayer.Estructuras
Imports LMDataAccessLayer
Imports System.Web.UI.WebControls.GridView

Namespace Recibos
    
    Public Class OrdenCompra

#Region "variables"
        Private _idOrden As Long
        Private _numeroOrden As String
        Private _idTipoProducto As Integer
        Private _idProveedor As Integer
        Private _idMoneda As Integer
        Private _idIncoterm As Integer
        Private _idEstado As Integer
        Private _idCreador As Long
        Private _fechaCreacion As Date
        Private _observacion As String
        Private _tipoProducto As Productos.TipoProducto
        Private _proveedor As String
        Private _moneda As String
        Private _incoterm As String
        Private _detalle As DataTable
        Private _distribucionRegional As DataTable
        Private _estado As String
        Private _arrayDetallesEliminados As ArrayList
        Private _productoRecibido As Boolean
        Private _fechaPrevista As Date
        Public Enum EstadoOrden
            Cancelada = 15
            Abierta = 16
            Parcial = 17
            Finalizada = 18
        End Enum
        Private _mensajeInfo As String

#End Region

#Region "propiedades"
        Public ReadOnly Property Proveedor() As String
            Get
                Return _proveedor
            End Get
        End Property

        Public ReadOnly Property IdOrden() As Long
            Get
                Return _idOrden
            End Get
        End Property

        Public Property NumeroOrden() As String
            Get
                Return _numeroOrden
            End Get
            Set(ByVal value As String)
                _numeroOrden = value
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

        Public Property IdProveedor() As Integer
            Get
                Return _idProveedor
            End Get
            Set(ByVal value As Integer)
                _idProveedor = value
            End Set
        End Property

        Public Property IdMoneda() As Integer
            Get
                Return _idMoneda
            End Get
            Set(ByVal value As Integer)
                _idMoneda = value
            End Set
        End Property

        Public Property IdIncoterm() As Integer
            Get
                Return _idIncoterm
            End Get
            Set(ByVal value As Integer)
                _idIncoterm = value
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

        Public Property IdCreador() As Long
            Get
                Return _idCreador
            End Get
            Set(ByVal value As Long)
                _idCreador = value
            End Set
        End Property

        Public Property FechaCreacion() As Date
            Get
                Return _fechaCreacion
            End Get
            Set(ByVal value As Date)
                _fechaCreacion = value
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

        Public Property TipoProducto() As Productos.TipoProducto
            Get
                If _tipoProducto Is Nothing Then _tipoProducto = New Productos.TipoProducto(_idTipoProducto)
                Return _tipoProducto
            End Get
            Set(ByVal value As Productos.TipoProducto)
                _tipoProducto = value
            End Set
        End Property

        Public ReadOnly Property Moneda() As String
            Get
                Return _moneda
            End Get
        End Property

        Public ReadOnly Property Incoterm() As String
            Get
                Return _incoterm
            End Get
        End Property

        Public ReadOnly Property Detalle() As DataTable
            Get
                If _detalle Is Nothing Then CargarDetalle()
                Return _detalle
            End Get
        End Property

        Public ReadOnly Property DistribucionRegional() As DataTable
            Get
                If _distribucionRegional Is Nothing Then CargarDistribucionRegional()
                Return _distribucionRegional
            End Get
        End Property

        Public ReadOnly Property Estado() As String
            Get
                Return _estado
            End Get
        End Property

        Public Property ProductoRecibido() As Boolean
            Get
                Return _productoRecibido
            End Get
            Set(ByVal value As Boolean)
                _productoRecibido = value
            End Set
        End Property

        Public Property FechaPrevista() As Date
            Get
                Return _fechaPrevista
            End Get
            Set(ByVal value As Date)
                _fechaPrevista = value
            End Set
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
        End Sub
        Public Sub New(ByVal idOrden As Long)
            Me.New()
            Me.CargarDatos(idOrden)
            _idOrden = idOrden
        End Sub
#End Region

#Region "metodos privados"

        Private Sub CargarDatos(ByVal idOrden As Long)
            Dim db As New LMDataAccess
            db.SqlParametros.Add("@idOrden", SqlDbType.BigInt).Value = idOrden
            Try
                db.ejecutarReader("ObtenerOrdenCompra", CommandType.StoredProcedure)
                If db.Reader.Read Then
                    _idOrden = idOrden
                    _numeroOrden = db.Reader("numeroOrden")
                    _idTipoProducto = db.Reader("idTipoProducto")
                    _idProveedor = db.Reader("idProveedor")
                    _idMoneda = db.Reader("idMoneda")
                    _idIncoterm = db.Reader("idIncoterm")
                    _idEstado = db.Reader("idEstado")
                    _idCreador = db.Reader("idCreador")
                    _fechaCreacion = db.Reader("fechaCreacion")
                    _observacion = db.Reader("observacion").ToString
                    _proveedor = db.Reader("proveedor").ToString
                    _moneda = db.Reader("moneda").ToString
                    _incoterm = db.Reader("incoterm").ToString
                    _estado = db.Reader("estado").ToString
                    _productoRecibido = CBool(db.Reader("productoRecibido"))
                    Date.TryParse(db.Reader("fechaPrevista").ToString(), _fechaPrevista)
                    If IdTipoProducto = 3 Or IdTipoProducto = 5 Then
                        CargarDistribucionRegional()
                    End If
                End If
            Catch ex As Exception
            Finally
                If Not db.Reader.IsClosed Then db.Reader.Close()
                db.Dispose()
            End Try
        End Sub

        Private Sub CargarDetalle()
            If _detalle Is Nothing Then _detalle = GenerarEstructuraTablaDetalle()
            If _idOrden > 0 Then
                Dim dbManager As New LMDataAccess
                Dim drDetalle As DataRow

                With dbManager
                    .SqlParametros.Add("@idOrden", SqlDbType.Int).Value = _idOrden
                    .ejecutarReader("ObtenerInfoDetalleOrdenCompra", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        While .Reader.Read
                            drDetalle = _detalle.NewRow
                            drDetalle("idDetalleOrden") = .Reader("idDetalle").ToString
                            drDetalle("fabricante") = .Reader("fabricante").ToString
                            drDetalle("idFabricante") = .Reader("idFabricante").ToString
                            drDetalle("producto") = .Reader("producto").ToString
                            drDetalle("idProducto") = .Reader("idProducto").ToString
                            drDetalle("tipoUnidad") = .Reader("tipoUnidad").ToString
                            drDetalle("idTipoUnidad") = .Reader("idTipoUnidad").ToString
                            drDetalle("cantidad") = .Reader("cantidad").ToString
                            drDetalle("valorUnitario") = .Reader("valorUnitario").ToString
                            drDetalle("observacion") = .Reader("observacion").ToString
                            drDetalle("idTipoDetalle") = .Reader("idTipoDetalle").ToString
                            _detalle.Rows.Add(drDetalle)
                            drDetalle.AcceptChanges()
                        End While
                        .Reader.Close()
                    End If
                End With
            End If
        End Sub

        Private Sub CargarDistribucionRegional()
            If _distribucionRegional Is Nothing Then _distribucionRegional = GenerarEstructuraTablaDistribucionRegional()
            If _idOrden > 0 Then
                Dim dbManager As New LMDataAccess
                Dim drDistribucion As DataRow
                Try
                    With dbManager
                        .SqlParametros.Add("@idOrden", SqlDbType.Int).Value = _idOrden
                        .ejecutarReader("ObtenerDistribucionRegionalOrdenCompra", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            While .Reader.Read
                                drDistribucion = _distribucionRegional.NewRow
                                drDistribucion("idDistribucion") = .Reader("idDistribucion").ToString
                                drDistribucion("idRegion") = .Reader("idRegion").ToString
                                drDistribucion("region") = .Reader("region").ToString
                                drDistribucion("cantidad") = .Reader("cantidad").ToString
                                drDistribucion("idUsuario") = .Reader("idUsuario").ToString
                                drDistribucion("fechaRegistro") = .Reader("fechaRegistro").ToString
                                _distribucionRegional.Rows.Add(drDistribucion)
                            End While
                            .Reader.Close()
                        End If
                    End With
                Catch ex As Exception
                End Try
            End If
        End Sub

        Private Function GenerarEstructuraTablaDetalle() As DataTable
            Dim dtDetalle As New DataTable
            With dtDetalle.Columns
                .Add("idDetalleOrden", GetType(Integer))
                .Add("fabricante", GetType(String))
                .Add("idFabricante", GetType(String))
                .Add("producto", GetType(String))
                .Add("idProducto", GetType(String))
                .Add("tipoUnidad", GetType(String))
                .Add("idTipoUnidad", GetType(Short))
                .Add("cantidad", GetType(Integer))
                .Add("valorUnitario", GetType(Decimal))
                .Add("observacion", GetType(String))
                .Add("idTipoDetalle", GetType(Short))
            End With
            Dim pkKeys() As DataColumn = {dtDetalle.Columns("idProducto")}
            dtDetalle.PrimaryKey = pkKeys
            Return dtDetalle
        End Function

        Private Function GenerarEstructuraTablaDistribucionRegional() As DataTable
            Dim dtDetalle As New DataTable
            With dtDetalle.Columns
                .Add("idDistribucion", GetType(Integer))
                .Add("idRegion", GetType(Integer))
                .Add("region", GetType(String))
                .Add("cantidad", GetType(Integer))
                .Add("idUsuario", GetType(Integer))
                .Add("fechaRegistro", GetType(Date))
            End With
            Dim pkKeys() As DataColumn = {dtDetalle.Columns("idRegion")}
            dtDetalle.PrimaryKey = pkKeys
            Return dtDetalle
        End Function

        Private Sub EliminarColumnasAdicionadasADetalle()
            If _detalle IsNot Nothing Then
                If _detalle.Columns.Contains("idOrden") Then _detalle.Columns.Remove("idOrden")
                If _detalle.Columns.Contains("idUsuario") Then _detalle.Columns.Remove("idUsuario")
            End If
        End Sub

        Private Sub EliminarColumnasAdicionadasADistribucion()
            If _distribucionRegional IsNot Nothing Then
                If _distribucionRegional.Columns.Contains("idOrdenCompra") Then _distribucionRegional.Columns.Remove("idOrdenCompra")
                If _distribucionRegional.Columns.Contains("idUsuario") Then _distribucionRegional.Columns.Remove("idUsuario")
            End If
        End Sub

        Private Function ObtenerDetalleOrdenAdicionado() As DataTable
            Dim dtAux As New DataTable
            If Not _detalle Is Nothing Then
                dtAux = _detalle.Clone
                For Each drAux As DataRow In _detalle.Rows
                    If drAux.RowState = DataRowState.Added Then dtAux.ImportRow(drAux)
                Next                
            End If
            Return dtAux
        End Function

        Private Function ObtenerDistribucionRegionalAdicionada() As DataTable
            Dim dtAux As DataTable = _distribucionRegional.Clone
            For Each drAux As DataRow In _distribucionRegional.Rows
                If drAux.RowState = DataRowState.Added Then dtAux.ImportRow(drAux)
            Next
            Return dtAux
        End Function

        Private Function ObtenerDetalleOrdenModificado() As DataTable
            Dim dtAux As New DataTable
            If Not _detalle Is Nothing Then
                dtAux = _detalle.Clone
                For Each drAux As DataRow In _detalle.Rows
                    If drAux.RowState = DataRowState.Modified Then dtAux.ImportRow(drAux)
                Next
            End If
            Return dtAux
        End Function

        Private Function ObtenerDistribucionRegionalModificada() As DataTable
            Dim dtAux As New DataTable
            If Not _distribucionRegional Is Nothing Then
                dtAux = _distribucionRegional.Clone
                For Each drAux As DataRow In _distribucionRegional.Rows
                    If drAux.RowState = DataRowState.Modified Then dtAux.ImportRow(drAux)
                Next
            End If
            Return dtAux
        End Function

        Private Function ObtenerDetalleOrdenEliminado() As DataView
            Dim dvAux As New DataView(_detalle)
            dvAux.RowStateFilter = DataViewRowState.Deleted
            Return dvAux
        End Function

        Private Function ObtenerDistribucionRegionalEliminada() As DataTable
            Dim dtAux As New DataTable
            If Not _distribucionRegional Is Nothing Then
                dtAux = _distribucionRegional.Clone
                For Each drAux As DataRow In _distribucionRegional.Rows
                    If drAux.RowState = DataRowState.Deleted Then dtAux.ImportRow(drAux)
                Next
                dtAux.RejectChanges()
            End If
            Return dtAux
        End Function

        Private Sub RegistrarDetalleOrden(ByVal dtDetalle As DataTable, ByVal dbManager As LMDataAccess)
            Dim dcAux As DataColumn = Nothing
            If dtDetalle.Columns.Contains("idOrden") Then dtDetalle.Columns.Remove("idOrden")
            dcAux = New DataColumn("idOrden", GetType(Integer))
            dcAux.DefaultValue = _idOrden
            dtDetalle.Columns.Add(dcAux)

            If dtDetalle.Columns.Contains("idUsuario") Then dtDetalle.Columns.Remove("idUsuario")
            dcAux = New DataColumn("idUsuario", GetType(Integer))
            dcAux.DefaultValue = _idCreador
            dtDetalle.Columns.Add(dcAux)

            With dbManager
                '***Se registra el Detalle de la Orden***'
                .inicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "DetalleOrdenCompra"
                    .ColumnMappings.Add("idOrden", "idOrden")
                    .ColumnMappings.Add("idFabricante", "idFabricante")
                    .ColumnMappings.Add("idProducto", "idProducto")
                    .ColumnMappings.Add("idTipoUnidad", "idTipoUnidad")
                    .ColumnMappings.Add("cantidad", "cantidad")
                    .ColumnMappings.Add("valorUnitario", "valorUnitario")
                    .ColumnMappings.Add("idUsuario", "idUsuario")
                    .ColumnMappings.Add("observacion", "observacion")
                    .ColumnMappings.Add("idTipoDetalle", "idTipoDetalle")
                    .WriteToServer(dtDetalle)
                End With
            End With
        End Sub

        Private Sub RegistrarDistribucionRegional(ByVal dtDistribucionRegional As DataTable, ByVal dbManager As LMDataAccess)
            Dim dcAux As DataColumn = Nothing
            If dtDistribucionRegional.Columns.Contains("idOrdenCompra") Then dtDistribucionRegional.Columns.Remove("idOrdenCompra")
            dcAux = New DataColumn("idOrdenCompra", GetType(Integer))
            dcAux.DefaultValue = _idOrden
            dtDistribucionRegional.Columns.Add(dcAux)

            If dtDistribucionRegional.Columns.Contains("idUsuario") Then dtDistribucionRegional.Columns.Remove("idUsuario")
            dcAux = New DataColumn("idUsuario", GetType(Integer))
            dcAux.DefaultValue = _idCreador
            dtDistribucionRegional.Columns.Add(dcAux)

            With dbManager
                '***Se registra la distribución regional de la orden***'
                .inicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "DistribucionRegionalOrdenCompra"
                    .ColumnMappings.Add("idOrdenCompra", "idOrdenCompra")
                    .ColumnMappings.Add("idRegion", "idRegion")
                    .ColumnMappings.Add("cantidad", "cantidad")
                    .ColumnMappings.Add("idUsuario", "idUsuario")
                    .WriteToServer(dtDistribucionRegional)
                End With
            End With
        End Sub

        Private Sub ModificarDetalleOrdenCompra(ByVal dtDetalle As DataTable, ByVal dbManager As LMDataAccess)
            For Each drAux As DataRow In dtDetalle.Rows
                With dbManager
                    .SqlParametros.Clear()
                    With .SqlParametros
                        .Add("@idDetalle", SqlDbType.BigInt).Value = CLng(drAux("idDetalleOrden"))
                        .Add("@idOrden", SqlDbType.BigInt).Value = _idOrden
                        .Add("@idFabricante", SqlDbType.Int).Value = CInt(drAux("idFabricante"))
                        .Add("@idProducto", SqlDbType.Int).Value = CInt(drAux("idProducto"))
                        .Add("@idTipoUnidad", SqlDbType.Int).Value = CInt(drAux("idTipoUnidad"))
                        .Add("@cantidad", SqlDbType.Int).Value = CInt(drAux("cantidad"))
                        .Add("@valorUnitario", SqlDbType.Decimal).Value = drAux("valorUnitario")
                        .Add("@idUsuario", SqlDbType.Int).Value = _idCreador
                        .Add("@observacion", SqlDbType.VarChar).Value = drAux("observacion").ToString
                        .Add("@idTipoDetalle", SqlDbType.SmallInt).Value = IIf(drAux("idTipoDetalle") > 0, drAux("idTipoDetalle"), TipoDetalleOrdenCompra.TipoDetalle.Principal)
                    End With
                    .ejecutarNonQuery("ActualizarInfoDetalleOrdenCompra", CommandType.StoredProcedure)
                End With
            Next
        End Sub

        Private Sub ModificarDistribucionRegional(ByVal dtDistribucionRegional As DataTable, ByVal dbManager As LMDataAccess)
            For Each drAux As DataRow In dtDistribucionRegional.Rows
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idOrdenCompra", SqlDbType.BigInt).Value = _idOrden
                    .SqlParametros.Add("@idRegion", SqlDbType.Int).Value = CInt(drAux("idRegion"))
                    .SqlParametros.Add("@cantidad", SqlDbType.Int).Value = CInt(drAux("cantidad"))
                    .ejecutarNonQuery("ActualizarDistribucionRegionalOrdenCompra", CommandType.StoredProcedure)
                End With
            Next
        End Sub

        Private Sub EliminarDetalleOrdenCompra(ByVal arrayDetalle As ArrayList, ByVal dbManager As LMDataAccess)
            With dbManager
                .SqlParametros.Clear()
                .SqlParametros.Add("@listaDetalle", SqlDbType.VarChar).Value = Join(arrayDetalle.ToArray, ",")
                .ejecutarNonQuery("EliminarDetalleDeOrdenCompra", CommandType.StoredProcedure)
            End With
        End Sub

        Private Sub EliminarDetalleOrdenCompra(ByVal dvDetalle As DataView, ByVal dbManager As LMDataAccess)
            Dim arrayEliminar As New ArrayList()
            For Each fila As DataRowView In dvDetalle
                arrayEliminar.Add(fila("idDetalleOrden"))
            Next

            With dbManager
                .SqlParametros.Clear()
                .SqlParametros.Add("@listaDetalle", SqlDbType.VarChar).Value = Join(arrayEliminar.ToArray, ",")
                .ejecutarNonQuery("EliminarDetalleDeOrdenCompra", CommandType.StoredProcedure)
            End With
        End Sub

        Private Sub EliminarDistribucionRegional(ByVal dtDistribucionRegional As DataTable, ByVal dbManager As LMDataAccess)
            Dim arrAux As New ArrayList
            For Each drAux As DataRow In dtDistribucionRegional.Rows
                arrAux.Add(drAux("idRegion"))
            Next
            With dbManager
                .SqlParametros.Clear()
                .SqlParametros.Add("@idOrdenCompra", SqlDbType.BigInt).Value = _idOrden
                .SqlParametros.Add("@listaRegiones", SqlDbType.VarChar).Value = Join(arrAux.ToArray, ",")
                .ejecutarNonQuery("EliminarDistribucionRegionDeOrdenCompra", CommandType.StoredProcedure)
            End With
        End Sub

        Private Sub ActualizarOrdenesRecepcion(ByVal ordenesRecepcion As ArrayList, ByVal dbManager As LMDataAccess)
            If _idOrden > 0 Then
                For Each idOrdenRecepcion As Integer In ordenesRecepcion
                    Dim ordenRecepcionObj As New OrdenRecepcion(CLng(idOrdenRecepcion))                    
                    ordenRecepcionObj.IdOrdenCompra = CLng(_idOrden)
                    ordenRecepcionObj.Actualizar(dbManager)                    
                Next
            End If
        End Sub

#End Region

#Region "metodos publicos"

        Public Function Crear(Optional ByVal ordenesRecepcion As ArrayList = Nothing) As Boolean
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim retorno As Boolean
            With dbManager
                With .SqlParametros
                    .Add("@numeroOrden", SqlDbType.VarChar).Value = _numeroOrden
                    .Add("@idTipoProducto", SqlDbType.Int).Value = _idTipoProducto
                    .Add("@idProveedor", SqlDbType.Int).Value = _idProveedor
                    .Add("@idMoneda", SqlDbType.Int).Value = _idMoneda
                    If _idIncoterm Then .Add("@idIncoterm", SqlDbType.Int).Value = _idIncoterm
                    .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                    .Add("@idCreador", SqlDbType.BigInt).Value = _idCreador
                    .Add("@observacion", SqlDbType.VarChar, 250).Value = _observacion
                    .Add("@productoRecibido", SqlDbType.Bit).Value = _productoRecibido
                    If (_fechaPrevista > Date.MinValue) Then .Add("@fechaPrevista", SqlDbType.SmallDateTime).Value = _fechaPrevista
                    .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                Try
                    Dim result As Short = 0
                    .iniciarTransaccion()
                    .ejecutarNonQuery("CrearOrdenCompra", CommandType.StoredProcedure)
                    Short.TryParse(.SqlParametros("@result").Value.ToString, result)
                    If result = 0 Then
                        _idOrden = CLng(.SqlParametros("@identity").Value)
                        Using dtAux As DataTable = _detalle.Copy
                            RegistrarDetalleOrden(dtAux, dbManager)
                        End Using
                        '***Se verifica si se debe registrar la Distribución Regional de la Orden de Compra***'
                        If _distribucionRegional IsNot Nothing AndAlso _distribucionRegional.Rows.Count > 0 Then
                            Using dtAux As DataTable = _distribucionRegional.Copy
                                RegistrarDistribucionRegional(dtAux, dbManager)
                            End Using
                        End If

                        If ordenesRecepcion IsNot Nothing AndAlso ordenesRecepcion.Count > 0 Then
                            ActualizarOrdenesRecepcion(ordenesRecepcion, dbManager)
                        End If

                        .confirmarTransaccion()
                        _detalle.Dispose()
                        _detalle = Nothing
                        retorno = True
                    Else
                        Throw New Exception("Imposible registrar la información de la Orden en la Base de Datos.")
                    End If
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso .estadoTransaccional Then .abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                    EliminarColumnasAdicionadasADetalle()
                    EliminarColumnasAdicionadasADistribucion()
                End Try
            End With
            Return retorno
        End Function

        Public Function Actualizar() As Short
            Dim resultado As Short = 0
            If IdOrden <> 0 Then
                Dim db As New LMDataAccessLayer.LMDataAccess

                Try
                    With db
                        With .SqlParametros
                            .Add("@idOrden", SqlDbType.BigInt).Value = _idOrden
                            .Add("@numeroOrden", SqlDbType.VarChar).Value = _numeroOrden
                            .Add("@idTipoProducto", SqlDbType.BigInt).Value = _idTipoProducto
                            .Add("@idProveedor", SqlDbType.BigInt).Value = _idProveedor
                            .Add("@idMoneda", SqlDbType.BigInt).Value = _idMoneda
                            If _idIncoterm Then .Add("@idIncoterm", SqlDbType.BigInt).Value = _idIncoterm
                            .Add("@idEstado", SqlDbType.BigInt).Value = _idEstado
                            .Add("@idCreador", SqlDbType.BigInt).Value = _idCreador                            
                            .Add("@observacion", SqlDbType.VarChar).Value = _observacion
                            .Add("@productoRecibido", SqlDbType.Bit).Value = _productoRecibido
                            If _fechaPrevista <> Date.MinValue Then .Add("@fechaPrevista", SqlDbType.SmallDateTime).Value = _fechaPrevista
                            .Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        End With
                        .iniciarTransaccion()
                        .ejecutarNonQuery("ActualizarOrdenCompra", CommandType.StoredProcedure)
                        Short.TryParse(.SqlParametros("@returnValue").Value.ToString, resultado)
                        If resultado = 0 Then
                            'Se verifica si se adicionaron detalles a la orden
                            Using dtAux As DataTable = ObtenerDetalleOrdenAdicionado()
                                If dtAux.Rows.Count > 0 Then RegistrarDetalleOrden(dtAux, db)
                            End Using

                            'Se verifica si se modificaron detalles a la orden
                            Using dtAux As DataTable = ObtenerDetalleOrdenModificado()
                                If dtAux.Rows.Count > 0 Then ModificarDetalleOrdenCompra(dtAux, db)
                            End Using

                            'Se verifica si se eliminaron detalles a la orden
                            Using dvAux As DataView = ObtenerDetalleOrdenEliminado()
                                If dvAux.Count > 0 Then EliminarDetalleOrdenCompra(dvAux, db)
                            End Using

                            'Se verifica si se eliminaron detalles a la orden                           
                            If Not Me._arrayDetallesEliminados Is Nothing AndAlso Me._arrayDetallesEliminados.Count > 0 Then
                                EliminarDetalleOrdenCompra(Me._arrayDetallesEliminados, db)
                                _arrayDetallesEliminados.Clear()
                            End If


                            'Se verifica si se adicionaron nuevos distribuciones a la orden y de ser así se registran en la BD
                            'Using dtAux As DataTable = ObtenerDistribucionRegionalAdicionada()
                            '    If dtAux.Rows.Count > 0 Then RegistrarDistribucionRegional(dtAux, db)
                            'End Using

                            'Se verifica si se modificaron distribuiciones de la orden si es asi se modifican
                            If _idTipoProducto = 3 Or _idTipoProducto = 5 Then
                                Using dtAux As DataTable = ObtenerDistribucionRegionalModificada()
                                    If dtAux.Rows.Count > 0 Then ModificarDistribucionRegional(dtAux, db)
                                End Using
                            End If

                            'Se verifica si se eliminaron detalles de la orden si es asi se eliminan
                            'Using dtAux As DataTable = ObtenerDistribucionRegionalEliminada()
                            '    If dtAux.Rows.Count > 0 Then EliminarDistribucionRegional(dtAux, db)
                            'End Using
                        End If
                        If resultado = 0 Then .confirmarTransaccion()
                    End With
                Catch ex As Exception
                    If db.estadoTransaccional Then db.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    db.cerrarConexion()
                End Try
            Else
                Throw New DuplicateNameException("La Orden de Compra aún no ha sido registrada en la Base de Datos.")
            End If
            Return resultado
        End Function

        Public Sub AdicionarDetalle(ByVal idFabricante As Integer, ByVal fabricante As String, ByVal idProducto As Integer, ByVal producto As String, _
                ByVal idTipoUnidad As Short, ByVal unidadEmpaque As String, ByVal cantidad As Integer, ByVal valorUnitario As Decimal, ByVal observacion As String, Optional ByVal idTipoDetalle As Integer = TipoDetalleOrdenCompra.TipoDetalle.Principal)

            If _detalle Is Nothing Then _detalle = GenerarEstructuraTablaDetalle()
            Dim drDetalle As DataRow = _detalle.NewRow
            drDetalle("fabricante") = fabricante
            drDetalle("idFabricante") = idFabricante
            drDetalle("producto") = producto
            drDetalle("idProducto") = idProducto
            drDetalle("tipoUnidad") = unidadEmpaque
            drDetalle("idTipoUnidad") = idTipoUnidad
            drDetalle("cantidad") = cantidad
            drDetalle("valorUnitario") = valorUnitario
            drDetalle("observacion") = observacion
            drDetalle("idTipoDetalle") = idTipoDetalle
            _detalle.Rows.Add(drDetalle)
        End Sub

        Public Sub ModificarDetalle(ByVal idFabricante As Integer, ByVal fabricante As String, ByVal idProducto As Integer, ByVal producto As String, _
                ByVal idTipoUnidad As Short, ByVal unidadEmpaque As String, ByVal cantidad As Integer, ByVal valorUnitario As Decimal, ByVal observacion As String)

            If _detalle Is Nothing Then CargarDetalle()
            If _detalle.Rows.Find(idProducto) IsNot Nothing Then
                _detalle.Rows.Find(idProducto).Item("idFabricante") = idFabricante            
                _detalle.Rows.Find(idProducto).Item("idTipoUnidad") = idTipoUnidad
                _detalle.Rows.Find(idProducto).Item("cantidad") = cantidad
                _detalle.Rows.Find(idProducto).Item("valorUnitario") = valorUnitario
                _detalle.Rows.Find(idProducto).Item("observacion") = observacion.ToString
            End If
           
        End Sub


        Public Sub AdicionarDetalle(ByVal dtInfoDetalle As DataTable)
            If _detalle Is Nothing Then _detalle = GenerarEstructuraTablaDetalle()
            For Each drOrigenDetalle As DataRow In dtInfoDetalle.Rows
                _detalle.ImportRow(drOrigenDetalle)
            Next
        End Sub

        Public Function BorrarDetallePorId(ByVal idDetalle As Integer) As Boolean
            Dim resultado As Boolean = False
            If _detalle IsNot Nothing Then
                Dim drAux() As DataRow = _detalle.Select("idDetalle=" & idDetalle.ToString)
                For index As Integer = 0 To drAux.GetUpperBound(0)
                    drAux(index).Delete()
                Next
            End If
            Return resultado
        End Function

        Public Function BorrarDetallePorProducto(ByVal idProducto As Integer) As Boolean
            Dim resultado As Boolean = False
            If _detalle IsNot Nothing Then
                Dim drAux As DataRow = _detalle.Select("idProducto=" & idProducto)(0)
                If drAux IsNot Nothing Then drAux.Delete()
            End If
            Return resultado
        End Function

        Public Sub AjustarInfoDetalle(ByVal dtDetalle As DataTable)
            Dim drDetalle As DataRow
            Dim idProducto As Integer
            If _detalle Is Nothing Then CargarDetalle()

            'Se adicionan los nuevos registros de Detalle
            For Each drAux As DataRow In dtDetalle.Rows
                Integer.TryParse(drAux("idProducto").ToString, idProducto)
                drDetalle = _detalle.Select("idProducto=" & idProducto)(0)
                If drDetalle Is Nothing Then _detalle.ImportRow(drAux)
            Next

            'Se elimina los detalles desasignados 
            For Each drAux As DataRow In _detalle.Rows
                Integer.TryParse(drAux("idProducto").ToString, idProducto)
                drDetalle = _detalle.Select("idProducto=" & idProducto)(0)
                If drDetalle Is Nothing Then Me.BorrarDetallePorProducto(idProducto)
            Next
        End Sub

        Public Sub AjustarADetalle(ByVal dtDetalle As DataTable)
            Dim idProducto As Integer
            If _detalle Is Nothing Then CargarDetalle()

            'Se adicionan los nuevos registros de Detalle
            For Each drAux As DataRow In dtDetalle.Rows
                Integer.TryParse(drAux("idProducto").ToString, idProducto)
                If _detalle.Rows.Find(idProducto) Is Nothing Then _detalle.ImportRow(drAux)
            Next

            'Se elimina los detalles desasignados 
            Me._arrayDetallesEliminados = New ArrayList
            For Each drAux As DataRow In _detalle.Rows
                Integer.TryParse(drAux("idProducto").ToString, idProducto)
                If dtDetalle.Select("idProducto=" & idProducto).Count < 1 Then Me._arrayDetallesEliminados.Add(drAux("idDetalleOrden"))
            Next

            'Se modifican los modificados
            For Each drAux As DataRow In dtDetalle.Rows
                Integer.TryParse(drAux("idProducto").ToString, idProducto)
                If _detalle.Rows.Find(idProducto) IsNot Nothing AndAlso drAux.RowState = DataRowState.Modified Then
                    _detalle.Rows.Find(idProducto).Item("idFabricante") = drAux.Item("idFabricante").ToString
                    _detalle.Rows.Find(idProducto).Item("idProducto") = drAux.Item("idProducto").ToString
                    _detalle.Rows.Find(idProducto).Item("idTipoUnidad") = drAux.Item("idTipoUnidad").ToString
                    _detalle.Rows.Find(idProducto).Item("cantidad") = drAux.Item("cantidad").ToString
                    _detalle.Rows.Find(idProducto).Item("valorUnitario") = drAux.Item("valorUnitario").ToString
                    _detalle.Rows.Find(idProducto).Item("observacion") = drAux.Item("observacion").ToString
                End If
            Next

        End Sub

        Public Sub ActualizarDetalleOrdenCompra(ByVal dtDetalle As DataTable)
            _detalle = dtDetalle            
        End Sub

        Public Sub AdicionarDistribucionRegional(ByVal idRegion As Integer, ByVal cantidad As Integer)
            If _distribucionRegional Is Nothing Then _distribucionRegional = GenerarEstructuraTablaDistribucionRegional()
            Dim drDistribucion As DataRow = _distribucionRegional.NewRow
            drDistribucion("idRegion") = idRegion
            drDistribucion("cantidad") = cantidad
            _distribucionRegional.Rows.Add(drDistribucion)
        End Sub

        Public Sub ModificarDistribucionRegional(ByVal idRegion As Integer, ByVal cantidad As Integer)
            If _distribucionRegional Is Nothing Then _distribucionRegional = GenerarEstructuraTablaDistribucionRegional()
            If _distribucionRegional.Rows.Find(idRegion) IsNot Nothing Then
                _distribucionRegional.Rows.Find(idRegion).Item("cantidad") = cantidad
            End If            
        End Sub

        Public Sub AdicionarDistribucionRegional(ByVal dtInfoDistribucion As DataTable)
            If _distribucionRegional Is Nothing Then _distribucionRegional = GenerarEstructuraTablaDistribucionRegional()
            Dim drDistribucion As DataRow
            For Each drOrigenDistribucion As DataRow In dtInfoDistribucion.Rows
                drDistribucion = _distribucionRegional.NewRow
                drDistribucion("idRegion") = drOrigenDistribucion("idRegion")
                drDistribucion("cantidad") = drOrigenDistribucion("cantidad")
                _distribucionRegional.Rows.Add(drDistribucion)
            Next
        End Sub

        Public Function BorrarDistribucionPorId(ByVal idDistribucion As Integer) As Boolean
            Dim resultado As Boolean = False
            If _distribucionRegional IsNot Nothing Then
                Dim drAux() As DataRow = _distribucionRegional.Select("idDistrubucion=" & idDistribucion.ToString)
                For index As Integer = 0 To drAux.GetUpperBound(0)
                    drAux(index).Delete()
                Next
            End If
            Return resultado
        End Function

        Public Function BorrarDistribucionPorRegion(ByVal idRegion As Integer) As Boolean
            Dim resultado As Boolean = False
            If _distribucionRegional IsNot Nothing Then
                Dim drAux As DataRow = _distribucionRegional.Rows.Find(idRegion)
                If drAux IsNot Nothing Then drAux.Delete()
            End If
            Return resultado
        End Function

        Public Sub AjustarInfoDistribucionRegional(ByVal dtDistribucion As DataTable)
            Dim drDistribucion As DataRow
            Dim idRegion As Integer
            If _distribucionRegional Is Nothing Then CargarDistribucionRegional()

            'Se adicionan los nuevos registros de Distribucion Regional
            For Each drAux As DataRow In dtDistribucion.Rows
                Integer.TryParse(drAux("idRegion").ToString, idRegion)
                drDistribucion = _distribucionRegional.Rows.Find(idRegion)
                If drDistribucion Is Nothing Then Me.AdicionarDistribucionRegional(idRegion, CInt(drAux("cantidad")))
            Next

            'Se elimina las regiones desasignadas
            For Each drAux As DataRow In _detalle.Rows
                Integer.TryParse(drAux("idRegion").ToString, idRegion)
                drDistribucion = _distribucionRegional.Rows.Find(idRegion)
                If drDistribucion Is Nothing Then Me.BorrarDistribucionPorRegion(idRegion)
            Next
        End Sub

        Public Sub ModificarDistribucionRegional(ByVal dtDistribucion As DataTable)
            Dim idRegion As Integer
            Dim cantidad As Integer
            If Me._distribucionRegional IsNot Nothing Then
                For Each drAux As DataRow In dtDistribucion.Rows
                    Integer.TryParse(drAux("idRegion").ToString, idRegion)
                    Integer.TryParse(drAux("cantidad").ToString, cantidad)
                    If _distribucionRegional.Rows.Find(idRegion) IsNot Nothing Then
                        If _distribucionRegional.Rows.Find(idRegion)("cantidad") <> cantidad Then
                            _distribucionRegional.Rows.Find(idRegion).AcceptChanges()
                            _distribucionRegional.Rows.Find(idRegion)("cantidad") = cantidad
                        End If
                    End If
                Next
            End If
        End Sub

        Public Sub AsociarOrdenesRecepcion(ByVal ordenesRecepcion As ArrayList)
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            If _idOrden > 0 Then
                For Each idOrdenRecepcion As Integer In ordenesRecepcion
                    Dim ordenRecepcionObj As New OrdenRecepcion(CLng(idOrdenRecepcion))
                    ordenRecepcionObj.IdOrdenCompra = CLng(_idOrden)
                    ordenRecepcionObj.Actualizar(dbManager)
                Next
            End If
        End Sub

        Public Sub DesAsociarOrdenesRecepcion(ByVal idOrdenRecepcion As Long)
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim dtProductosOrdenRecepcion As New DataTable
            If _idOrden > 0 Then                
                Dim ordenRecepcionObj As New OrdenRecepcion(CLng(idOrdenRecepcion))
                dtProductosOrdenRecepcion = OrdenRecepcion.ObtenerListadoProducto(idOrdenRecepcion)
                For Each fila As DataRow In dtProductosOrdenRecepcion.Rows
                    With dbManager
                        .SqlParametros.Clear()
                        .SqlParametros.Add("@idOrdenCompra", SqlDbType.BigInt).Value = _idOrden
                        .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = CInt(fila("idProducto"))
                        .ejecutarNonQuery("EliminarDetalleDeOrdenCompra", CommandType.StoredProcedure)
                    End With                    
                Next
                ordenRecepcionObj.IdOrdenCompra = 0
                ordenRecepcionObj.Actualizar(dbManager)                
            End If
        End Sub

        ''' <summary>
        ''' Verifica si la orden de la instancia actual cumple las condiciones para ser anulada
        ''' si no es asi en la propiedad MensajeInfo quedan cargadas la razones.
        ''' </summary>
        ''' <returns>Retorna TRUE si se puede anular o FALSE de lo contrario</returns>
        ''' <remarks></remarks>
        Public Function PosibleAnular() As Boolean
            Dim retorno As Short
            Dim db As New LMDataAccess
            Try
                Me._mensajeInfo = String.Empty
                With db
                    With .SqlParametros
                        .Add("@idOperacion", SqlDbType.Int).Value = 1
                        .Add("@idOrden", SqlDbType.BigInt).Value = Me.IdOrden
                        .Add("@mensaje", SqlDbType.VarChar, 8000).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue
                    End With
                    .ejecutarNonQuery("EsPosibleOperacionOrdenCompra", CommandType.StoredProcedure)
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
        ''' Verifica si la orden de la instancia actual cumple las condiciones para adicionar un detalle
        ''' si no es asi en la propiedad MensajeInfo quedan cargadas la razones.
        ''' </summary>
        ''' <returns>Retorna TRUE si se puede adicionar o FALSE de lo contrario</returns>
        ''' <remarks></remarks>
        Public Function PosibleAdicionarDetalle() As Boolean
            Dim retorno As Short
            Dim db As New LMDataAccess
            Try
                Me._mensajeInfo = String.Empty
                With db
                    With .SqlParametros
                        .Add("@idOperacion", SqlDbType.Int).Value = 2
                        .Add("@idOrden", SqlDbType.BigInt).Value = Me.IdOrden
                        .Add("@mensaje", SqlDbType.VarChar, 8000).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue
                    End With
                    .ejecutarNonQuery("EsPosibleOperacionOrdenCompra", CommandType.StoredProcedure)
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
        ''' Verifica si el detalle de orden de compra cumple las condiciones para poder eliminarse
        ''' si no es asi en la propiedad MensajeInfo quedan cargadas la razones.
        ''' </summary>
        ''' <returns>Retorna TRUE si se puede eliminar o FALSE de lo contrario</returns>
        ''' <remarks></remarks>
        Public Function PosibleEliminarDetalle(ByVal idDetalle) As Boolean
            Dim retorno As Short
            Dim db As New LMDataAccess
            Try
                Me._mensajeInfo = String.Empty
                With db
                    With .SqlParametros
                        .Add("@idOperacion", SqlDbType.Int).Value = 3
                        .Add("@idDetalle", SqlDbType.BigInt).Value = idDetalle
                        .Add("@mensaje", SqlDbType.VarChar, 8000).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue
                    End With
                    .ejecutarNonQuery("EsPosibleOperacionOrdenCompra", CommandType.StoredProcedure)
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
        ''' Verifica si el detalle de orden de compra cumple las condiciones para editarse en su totalidad
        ''' si no es asi en la propiedad MensajeInfo quedan cargadas la razones.
        ''' </summary>
        ''' <returns>Retorna TRUE si se puede editar en su totalidad o FALSE de lo contrario</returns>
        ''' <remarks></remarks>
        Public Function PosibleEditarTodoDetalle(ByVal idDetalle) As Boolean
            Dim retorno As Short
            Dim db As New LMDataAccess
            Try
                Me._mensajeInfo = String.Empty
                With db
                    With .SqlParametros
                        .Add("@idOperacion", SqlDbType.Int).Value = 4
                        .Add("@idDetalle", SqlDbType.BigInt).Value = idDetalle
                        .Add("@mensaje", SqlDbType.VarChar, 8000).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue
                    End With
                    .ejecutarNonQuery("EsPosibleOperacionOrdenCompra", CommandType.StoredProcedure)
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

#Region "métodos compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroOrdenCompra
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroOrdenCompra) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdOrden > 0 Then db.SqlParametros.Add("@idOrden", SqlDbType.BigInt).Value = .IdOrden
                If .NumeroOrden <> "" Then db.SqlParametros.Add("@numeroOrden", SqlDbType.VarChar).Value = .NumeroOrden.ToString
                If .IdTipoProducto > 0 Then db.SqlParametros.Add("@idTipoProducto", SqlDbType.Int).Value = .IdTipoProducto
                If .IdProveedor > 0 Then db.SqlParametros.Add("@idProveedor", SqlDbType.Int).Value = .IdProveedor
                If .IdMoneda > 0 Then db.SqlParametros.Add("@idMoneda", SqlDbType.Int).Value = .IdMoneda
                If .IdIncoterm > 0 Then db.SqlParametros.Add("@idIncoterm", SqlDbType.Int).Value = .IdIncoterm
                If .IdEstado > 0 Then db.SqlParametros.Add("@idEstado", SqlDbType.Int).Value = .IdEstado
                If .IdCreador > 0 Then db.SqlParametros.Add("@idCreador", SqlDbType.BigInt).Value = .IdCreador
                'If .FechaCreacion.ToString <> "" Then db.SqlParametros.Add("@fechaCreacion", SqlDbType.DateTime).Value = CDate(.FechaCreacion.ToString)
                If .Observacion <> "" Then db.SqlParametros.Add("@observacion", SqlDbType.VarChar).Value = .Observacion
                If .FechaInicial <> Date.MinValue Then db.SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = .FechaInicial
                If .FechaFinal <> Date.MinValue Then db.SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = .FechaFinal
                If .ListaEstado IsNot Nothing AndAlso .ListaEstado.Count Then db.SqlParametros.Add("@listaEstado", SqlDbType.VarChar).Value = Join(.ListaEstado.ToArray, ",")
                If .IdNumeroOrden <> "" Then db.SqlParametros.Add("@idNumeroOrden", SqlDbType.VarChar).Value = .IdNumeroOrden.ToString()
                If .ProductoRecibido > 0 Then db.SqlParametros.Add("@productoRecibido", SqlDbType.Bit).Value = IIf(.ProductoRecibido = 1, 1, 0)
                If .FechaPrevista > Date.MinValue Then db.SqlParametros.Add("@fechaPrevista", SqlDbType.SmallDateTime).Value = .FechaPrevista
                If .CantidadPendiente > 0 Then db.SqlParametros.Add("@cantidadPendiente", SqlDbType.Bit).Value = IIf(.CantidadPendiente = 1, 1, 0)
                dtDatos = db.ejecutarDataTable("ObtenerListadoOrdenCompra", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos

        End Function

        Public Overloads Shared Function ExisteNumeroOrden(ByVal numeroOrden As String) As Boolean
            Dim retorno As Boolean = False
            If numeroOrden <> String.Empty Then
                Dim filtro As New FiltroOrdenCompra
                Dim dt As New DataTable
                filtro.NumeroOrden = numeroOrden
                dt = ObtenerListado(filtro)
                If dt.Rows.Count > 0 Then
                    retorno = True
                End If
            End If
            Return retorno
        End Function

        Public Shared Function ObtenerDetalle(ByVal idOrden As Integer)
            Dim dtDetalle As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idOrden", SqlDbType.Int).Value = idOrden
                    dtDetalle = .ejecutarDataTable("ObtenerInfoDetalleOrdenCompra", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtDetalle
        End Function

        Public Shared Function ObtenerMateriales(ByVal idOrden As Integer) As DataTable
            Dim dtMateriales As New DataTable
            Dim db As New LMDataAccess
            Try
                With db
                    .SqlParametros.Add("@idOrden", SqlDbType.Int).Value = idOrden
                    dtMateriales = .ejecutarDataTable("ObtenerMaterialesOrdenCompra", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
            Return dtMateriales
        End Function

        Public Shared Function ObtenerDistribucionRegional(ByVal idOrden As Integer)
            Dim dtDetalle As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idOrden", SqlDbType.Int).Value = idOrden
                    dtDetalle = .ejecutarDataTable("ObtenerDistribucionRegionalOrdenCompra", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtDetalle
        End Function

        Public Shared Function ObtenerDetalleRecepcion(ByVal idOrden As Integer)
            Dim dtDetalle As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idOrdenCompra", SqlDbType.Int).Value = idOrden
                    dtDetalle = .ejecutarDataTable("ObtenerDetalleRecepcionOrdenCompra", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtDetalle
        End Function

        Public Shared Function ObtenerFacturasNoCerradas(ByVal idOrden As Integer)
            Dim dtDetalle As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idOrdenCompra", SqlDbType.Int).Value = idOrden
                    dtDetalle = .ejecutarDataTable("ObtenerInfoFacturaDeOrdenCompra", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtDetalle
        End Function

#End Region

    End Class
End Namespace

