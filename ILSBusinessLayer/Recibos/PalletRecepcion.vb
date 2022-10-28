Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras

Namespace Recibos
    Public Class PalletRecepcion

#Region "variables"
        Private _idPallet As Long
        Private _idOrdenRecepcion As Long
        Private _peso As Decimal
        Private _fechaCreacion As Date
        Private _idCreador As Integer
        Private _observacion As String
        Private _remision As String
        Private _creador As String        
        Private _detalle As DataTable
        Private _novedades As DataTable
        Private _idPosicion As Integer
        Private _idFacturaGuia As Long
        Private _idTipoDetalleProducto As Short
        Private _idAcomodador As Integer
        Private _color As String
        Private _productoPrincipal As String
#End Region

#Region "propiedades"
        Public ReadOnly Property IdPallet() As Long
            Get
                Return _idPallet
            End Get
        End Property

        Public Property IdOrdenRecepcion() As Long
            Get
                Return _idOrdenRecepcion
            End Get
            Set(ByVal value As Long)
                _idOrdenRecepcion = value
            End Set
        End Property

        Public Property Peso() As Decimal
            Get
                Return _peso
            End Get
            Set(ByVal value As Decimal)
                _peso = value
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

        Public Property IdCreador() As Long
            Get
                Return _idCreador
            End Get
            Set(ByVal value As Long)
                _idCreador = value
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

        Public ReadOnly Property Remision()
            Get
                Return _remision
            End Get
        End Property

        Public ReadOnly Property Creador()
            Get
                Return _creador
            End Get
        End Property

        Public ReadOnly Property Detalle() As DataTable
            Get
                If _detalle Is Nothing Then ObtenerInfoDetalle()
                Return _detalle
            End Get
        End Property

        Public ReadOnly Property Novedades() As DataTable
            Get
                If _novedades Is Nothing Then _novedades = GenerarEstructuraNovedades()
                Return _novedades
            End Get
        End Property

        Public Property IdPosicion() As Integer
            Get
                Return _idPosicion
            End Get
            Set(ByVal value As Integer)
                _idPosicion = value
            End Set
        End Property

        Public Property IdFacturaGuia() As Long
            Get
                Return _idFacturaGuia
            End Get
            Set(ByVal value As Long)
                _idFacturaGuia = value
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

        Public Property IdAcomodador() As Integer
            Get
                Return _idAcomodador
            End Get
            Set(ByVal value As Integer)
                _idAcomodador = value
            End Set
        End Property

        Public Property Color() As String
            Get
                Return _color
            End Get
            Set(value As String)
                _color = value
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

#Region "constructores"
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(ByVal idPallet As Long)
            Me.New()
            Me.CargarDatos(idPallet)
            _idPallet = idPallet
        End Sub
#End Region

#Region "metodos privados"

        Private Sub CargarDatos(ByVal idPallet As Long)
            Dim db As New LMDataAccess
            db.SqlParametros.Add("@idPallet", SqlDbType.BigInt).Value = idPallet
            Try
                db.ejecutarReader("ObtenerPalletRecepcion", CommandType.StoredProcedure)
                If db.Reader.Read Then
                    _idOrdenRecepcion = db.Reader("idOrdenRecepcion")
                    _peso = db.Reader("peso")
                    _fechaCreacion = db.Reader("fechaCreacion")
                    _idCreador = db.Reader("idCreador")
                    _observacion = db.Reader("observacion").ToString()
                    _remision = db.Reader("remision").ToString
                    _creador = db.Reader("creador").ToString
                    _idTipoDetalleProducto = CShort(db.Reader("idTipoDetalleProducto"))
                End If
            Catch ex As Exception
                Throw New Exception("Imposible obtener los datos de pallet de recepcion. " & ex.Message)
            Finally
                If Not db.Reader.IsClosed Then db.Reader.Close()
                db.Dispose()
            End Try
        End Sub

        Private Sub RegistrarDetalle(ByVal dtDetalle As DataTable, ByVal dbManager As LMDataAccess)
            If Not dtDetalle.Columns.Contains("idPallet") Then
                Dim dcIdPallet As New DataColumn("idPallet", GetType(Long))
                dcIdPallet.DefaultValue = _idPallet
                dtDetalle.Columns.Add(dcIdPallet)
            Else
                dtDetalle.Columns("idPallet").DefaultValue = _idPallet
            End If
            With dbManager
                .inicilizarBulkCopy(SqlClient.SqlBulkCopyOptions.FireTriggers)
                With .BulkCopy
                    .DestinationTableName = "DetallePallet"
                    .ColumnMappings.Add("idPallet", "idPallet")
                    .ColumnMappings.Add("idProducto", "idProducto")
                    .ColumnMappings.Add("cantidad", "cantidad")
                    .ColumnMappings.Add("cantidad", "cantidadRecibida")
                    .ColumnMappings.Add("idTipoUnidad", "idTipoUnidad")
                    .ColumnMappings.Add("idRegion", "idRegion")
                    .ColumnMappings.Add("color", "color")
                    .ColumnMappings.Add("productoPrincipal", "productoPrincipal")
                    .WriteToServer(dtDetalle)
                End With
            End With
        End Sub

        Private Sub ObtenerInfoDetalle()
            Dim dbManarger As New LMDataAccess
            Try
                With dbManarger
                    .SqlParametros.Add("@idPallet", SqlDbType.Int).Value = _idPallet
                    _detalle = .ejecutarDataTable("ObtenerInfoDetallePallet", CommandType.StoredProcedure)
                End With
                Dim pkColumn(0) As DataColumn
                pkColumn(0) = _detalle.Columns("idProducto")
                _detalle.PrimaryKey = pkColumn
            Finally
                If dbManarger IsNot Nothing Then dbManarger.Dispose()
            End Try
        End Sub

        Private Function GenerarEstructuraDetalle() As DataTable
            Dim dtAux As New DataTable
            With dtAux.Columns
                .Add("idDetallePallet", GetType(Integer))
                .Add("idProducto", GetType(Integer))
                .Add("nombreProducto", GetType(String))
                .Add("cantidad", GetType(Integer))
                .Add("cantidadRecibida", GetType(Integer))
                .Add("idTipoUnidad", GetType(Short))
                .Add("unidadEmpaque", GetType(String))
                .Add("idRegion", GetType(Integer))
                .Add("region", GetType(String))
                .Add("idOrdenBodega", GetType(Integer))
                .Add("color", GetType(String))
                .Add("productoPrincipal", GetType(String))
            End With
            Return dtAux
        End Function

        Private Function GenerarEstructuraNovedades() As DataTable
            Dim dtAux As New DataTable
            With dtAux.Columns
                .Add("idNovedad", GetType(Integer))
                .Add("novedad", GetType(String))
            End With
            Dim pkColumn(0) As DataColumn
            pkColumn(0) = dtAux.Columns("idNovedad")
            dtAux.PrimaryKey = pkColumn
            Return dtAux
        End Function

        Private Sub GenerarOrdenBodegaPorDetalle(ByVal dbManager As LMDataAccess)
            With dbManager
                .SqlParametros.Clear()
                .SqlParametros.Add("@idPallet", SqlDbType.BigInt).Value = _idPallet
                .SqlParametros.Add("@idCreador", SqlDbType.Int).Value = _idCreador
                .ejecutarNonQuery("CrearOrdenBodegaPorDetallePallet", CommandType.StoredProcedure)
            End With
        End Sub

        Private Sub RegistrarNovedades(ByVal dbManager As LMDataAccess)
            Try
                If _novedades.Columns.Contains("idPallet") Then _novedades.Columns.Remove("idPallet")
                Dim dcIdPallet As New DataColumn("idPallet", GetType(Long))
                dcIdPallet.DefaultValue = _idPallet
                _novedades.Columns.Add(dcIdPallet)
                With dbManager
                    .inicilizarBulkCopy()
                    With .BulkCopy
                        .DestinationTableName = "PalletNovedad"
                        .ColumnMappings.Add("idPallet", "idPallet")
                        .ColumnMappings.Add("idNovedad", "idNovedad")
                        .WriteToServer(_novedades)
                    End With
                End With
            Finally
                If _novedades.Columns.Contains("idPallet") Then _novedades.Columns.Remove("idPallet")
            End Try
        End Sub

        Private Sub ConfirmarCajas(ByVal dtCajas As DataTable, ByVal dbManager As LMDataAccess, Optional ByVal ConRegion As Boolean = True)
            Dim arrCajas As ArrayList = GenerarArrayDeCajas(dtCajas)
            With dbManager

                If .SqlParametros.Count > 0 Then .SqlParametros.Clear()
                .SqlParametros.Add("@idPallet", SqlDbType.BigInt).Value = _idPallet
                .SqlParametros.Add("@listaCajas", SqlDbType.VarChar, 8000).Value = Join(arrCajas.ToArray, ",")
                If ConRegion Then
                    .ejecutarNonQuery("ConfirmarRecepcionCajaEmpaque", CommandType.StoredProcedure)
                Else
                    .ejecutarNonQuery("ConfirmarRecepcionCajaEmpaqueSinRegion", CommandType.StoredProcedure)
                End If

            End With
        End Sub

        Private Function GenerarArrayDeCajas(ByVal dtCajas As DataTable) As ArrayList
            Dim arrCajas As New ArrayList
            For Each drCaja As DataRow In dtCajas.Rows
                arrCajas.Add(drCaja("idCaja").ToString)
            Next
            Return arrCajas
        End Function

        Private Sub GenerarDetalleAPartirDeCajas(ByVal dtCajas As DataTable, Optional ByVal conRegion As Boolean = True)
            Dim dtAux As New DataTable
            If conRegion Then
                Dim arrCampos As New ArrayList(("idProducto,idRegion").Split(","))
                dtAux = GetDistinctsFromDataTable(dtCajas, arrCampos)
            Else
                dtAux = dtCajas
            End If
            Dim idProducto As Integer
            Dim idRegion As Integer
            Dim cantidad As Integer
            Dim filtro As String
            Dim productoPrincipal As String
            Dim productoAgregado As New ArrayList
            For Each drAux As DataRow In dtAux.Rows
                Integer.TryParse(drAux("idProducto").ToString, idProducto)
                Integer.TryParse(drAux("idRegion").ToString, idRegion)
                If conRegion Then
                    filtro = "idProducto = " & idProducto.ToString & " AND idRegion = " & idRegion.ToString
                    Integer.TryParse(dtCajas.Compute("SUM(cantidad)", filtro).ToString, cantidad)
                    productoPrincipal = drAux.Item("productoPrincipal")
                Else
                    filtro = "idProducto = " & idProducto.ToString
                    Integer.TryParse(dtCajas.Compute("SUM(cantidad)", filtro).ToString, cantidad)
                    productoPrincipal = drAux.Item("productoPrincipal")
                End If
                If Not productoAgregado.Contains(idProducto) Then
                    Me.AdicionarDetalle(idProducto, cantidad, "", productoPrincipal, 0, idRegion)
                End If
                productoAgregado.Add(idProducto)
            Next

        End Sub

#End Region

#Region "metodos publicos"

        Public Function Crear(Optional ByVal nuevaCantidadFacturaGuia As Integer = 0) As Boolean
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim retorno As Boolean
            With db
                With .SqlParametros
                    .Add("@idOrdenRecepcion", SqlDbType.Int).Value = _idOrdenRecepcion
                    .Add("@peso", SqlDbType.Decimal).Value = _peso
                    .Add("@idCreador", SqlDbType.Int).Value = _idCreador
                    .Add("@observacion", SqlDbType.VarChar).IsNullable = True
                    .Item("@observacion").Value = IIf(_observacion.Trim.Length > 0, _observacion.Trim, DBNull.Value)
                    .Add("@idFacturaGuia", SqlDbType.VarChar).IsNullable = True
                    .Item("@idFacturaGuia").Value = IIf(_idFacturaGuia > 0, _idFacturaGuia, DBNull.Value)
                    If _idTipoDetalleProducto > 0 Then .Add("@idTipoDetalleProducto", SqlDbType.SmallInt).Value = _idTipoDetalleProducto
                    .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                End With

                Try
                    Dim result As Integer = 0
                    .iniciarTransaccion()
                    .ejecutarNonQuery("CrearPalletRecepcion", CommandType.StoredProcedure)
                    result = CInt(.SqlParametros("@result").Value)
                    If result = 0 Then
                        _idPallet = CLng(.SqlParametros("@identity").Value)
                        If _detalle Is Nothing Then _detalle = GenerarEstructuraDetalle()
                        Dim dtAux As DataTable = _detalle.Copy
                        If nuevaCantidadFacturaGuia > 0 Then
                            Dim ordenRecepcionObj As New OrdenRecepcion(_idOrdenRecepcion)
                            Dim facturaGuiaOjb As New FacturaGuia(ordenRecepcionObj.IdFacturaGuia)
                            facturaGuiaOjb.Cantidad = nuevaCantidadFacturaGuia
                            facturaGuiaOjb.Actualizar(db)
                        End If
                        RegistrarDetalle(dtAux, db)
                        'Generar OTB's
                        GenerarOrdenBodegaPorDetalle(db)
                        If _novedades IsNot Nothing AndAlso _novedades.Rows.Count > 0 Then RegistrarNovedades(db)
                        .confirmarTransaccion()
                        retorno = True
                    Else
                        Throw New Exception("Imposible registrar la información de la Orden en la Base de Datos.")
                    End If
                Catch ex As Exception
                    If db IsNot Nothing AndAlso .estadoTransaccional Then .abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then .Dispose()
                End Try
            End With
            Return retorno
        End Function

        Public Function CrearConCajas(ByVal dtCajas As DataTable) As Boolean
            Dim retorno As Boolean = False
            If dtCajas.Rows.Count > 0 Then
                Dim dbManager As New LMDataAccessLayer.LMDataAccess
                With dbManager
                    With .SqlParametros
                        .Add("@idOrdenRecepcion", SqlDbType.Int).Value = _idOrdenRecepcion
                        .Add("@peso", SqlDbType.Decimal).Value = _peso
                        .Add("@idCreador", SqlDbType.Int).Value = _idCreador
                        .Add("@observacion", SqlDbType.VarChar).IsNullable = True
                        .Item("@observacion").Value = IIf(_observacion.Trim.Length > 0, _observacion.Trim, DBNull.Value)
                        .Add("@idFacturaGuia", SqlDbType.VarChar).IsNullable = True
                        .Item("@idFacturaGuia").Value = IIf(_idFacturaGuia > 0, _idFacturaGuia, DBNull.Value)
                        If _idTipoDetalleProducto > 0 Then .Add("@idTipoDetalleProducto", SqlDbType.SmallInt).Value = _idTipoDetalleProducto
                        .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                    End With

                    Try
                        Dim result As Integer = 0
                        .iniciarTransaccion()
                        .ejecutarNonQuery("CrearPalletRecepcion", CommandType.StoredProcedure)
                        result = CInt(.SqlParametros("@result").Value)
                        If result = 0 Then
                            _idPallet = CLng(.SqlParametros("@identity").Value)
                            If _detalle Is Nothing Then _detalle = GenerarEstructuraDetalle()
                            GenerarDetalleAPartirDeCajas(dtCajas)
                            Dim dtAux As DataTable = _detalle.Copy
                            RegistrarDetalle(dtAux, dbManager)
                            'Generar OTB's
                            GenerarOrdenBodegaPorDetalle(dbManager)
                            If _novedades IsNot Nothing AndAlso _novedades.Rows.Count > 0 Then RegistrarNovedades(dbManager)
                            If dtCajas.Rows.Count > 0 Then ConfirmarCajas(dtCajas, dbManager)
                            .confirmarTransaccion()
                            retorno = True
                        Else
                            Throw New Exception("Imposible registrar la información de la Orden en la Base de Datos.")
                        End If
                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso .estadoTransaccional Then .abortarTransaccion()
                        Throw New Exception(ex.Message, ex)
                    Finally
                        If dbManager IsNot Nothing Then .Dispose()
                    End Try
                End With
            Else
                Throw New Exception("No se ha proporcionado la información de las cajas. Por favor verifique.")
            End If
            Return retorno
        End Function

        Public Function CrearConCajasSinRegion(ByVal dtCajas As DataTable, Optional ByVal flag As Integer = 0) As Boolean
            Dim retorno As Boolean = False
            If dtCajas.Rows.Count > 0 Then
                Dim dbManager As New LMDataAccessLayer.LMDataAccess
                With dbManager
                    With .SqlParametros
                        .Add("@idOrdenRecepcion", SqlDbType.Int).Value = _idOrdenRecepcion
                        .Add("@peso", SqlDbType.Decimal).Value = _peso
                        .Add("@idCreador", SqlDbType.Int).Value = _idCreador
                        .Add("@observacion", SqlDbType.VarChar).IsNullable = True
                        .Item("@observacion").Value = IIf(_observacion.Trim.Length > 0, _observacion.Trim, DBNull.Value)
                        .Add("@idFacturaGuia", SqlDbType.VarChar).IsNullable = True
                        .Item("@idFacturaGuia").Value = IIf(_idFacturaGuia > 0, _idFacturaGuia, DBNull.Value)
                        If _idTipoDetalleProducto > 0 Then .Add("@idTipoDetalleProducto", SqlDbType.SmallInt).Value = _idTipoDetalleProducto
                        If flag > 0 Then .Add("@flag", SqlDbType.Int).Value = flag
                        .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                    End With

                    Try
                        Dim result As Integer = 0
                        .iniciarTransaccion()
                        .ejecutarNonQuery("CrearPalletRecepcion", CommandType.StoredProcedure)
                        result = CInt(.SqlParametros("@result").Value)
                        If result = 0 Then
                            _idPallet = CLng(.SqlParametros("@identity").Value)
                            If _detalle Is Nothing Then _detalle = GenerarEstructuraDetalle()
                            GenerarDetalleAPartirDeCajas(dtCajas, False)
                            Dim dtAux As DataTable = _detalle.Copy
                            RegistrarDetalle(dtAux, dbManager)
                            'Generar OTB's
                            GenerarOrdenBodegaPorDetalle(dbManager)
                            If _novedades IsNot Nothing AndAlso _novedades.Rows.Count > 0 Then RegistrarNovedades(dbManager)
                            If dtCajas.Rows.Count > 0 Then ConfirmarCajas(dtCajas, dbManager, False)
                            .confirmarTransaccion()
                            retorno = True
                        Else
                            Throw New Exception("Imposible registrar la información de la Orden en la Base de Datos.")
                        End If
                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso .estadoTransaccional Then .abortarTransaccion()
                        Throw New Exception(ex.Message, ex)
                    Finally
                        If dbManager IsNot Nothing Then .Dispose()
                    End Try
                End With
            Else
                Throw New Exception("No se ha proporcionado la información de las cajas. Por favor verifique.")
            End If
            Return retorno
        End Function

        Public Shared Function Eiliminar(ByVal idPallet As Long) As Boolean
            Try
                If idPallet > 0 Then
                    Dim resultado As Integer
                    Dim db As New LMDataAccessLayer.LMDataAccess
                    With db.SqlParametros
                        .Add("@idPalletRecepcion", SqlDbType.BigInt).Value = idPallet
                        .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                    End With
                    db.ejecutarNonQuery("EliminarPalletRecepcion", CommandType.StoredProcedure)
                    resultado = CInt(db.SqlParametros("@result").Value)
                    If resultado = 0 Then
                        Return True
                    Else
                        Return False
                    End If
                End If
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Function

        Public Sub Actualizar()
            If IdPallet <> 0 Then
                Dim db As New LMDataAccessLayer.LMDataAccess

                Try
                    db.iniciarTransaccion()
                    With db.SqlParametros
                        .Add("@idPallet", SqlDbType.BigInt).Value = _idPallet
                        .Add("@idOrdenRecepcion", SqlDbType.Int).Value = _idOrdenRecepcion
                        .Add("@peso", SqlDbType.Int).Value = _peso
                        .Add("@idTipoDetalleProducto", SqlDbType.SmallInt).Value = IIf(_idTipoDetalleProducto > 0, _idTipoDetalleProducto, 1)
                        .Add("@idCreador", SqlDbType.Int).Value = _idCreador
                        .Add("@observacion", SqlDbType.VarChar).Value = _observacion
                    End With
                    db.ejecutarNonQuery("ActualizarPalletRecepcion", CommandType.StoredProcedure)
                    db.confirmarTransaccion()
                Catch ex As Exception
                    If db.estadoTransaccional Then db.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    db.cerrarConexion()
                End Try
            Else
                Throw New DuplicateNameException("La Orden de Compra aún no ha sido registrada en la Base de Datos.")
            End If
        End Sub

        Public Sub AdicionarDetalle(ByVal idProducto As Integer, ByVal cantidad As Integer, ByVal color As String, ByVal productoPrincipal As String, _
                                    Optional ByVal idTipoUnidad As Short = 0, Optional ByVal idRegion As Integer = 0)
            If _detalle Is Nothing Then _detalle = GenerarEstructuraDetalle()
            Dim drDetalle As DataRow = _detalle.NewRow
            drDetalle("idProducto") = idProducto
            drDetalle("cantidad") = cantidad
            If idRegion > 0 Then drDetalle("idRegion") = idRegion
            If idTipoUnidad > 0 Then drDetalle("idTipoUnidad") = idTipoUnidad
            drDetalle("color") = color
            drDetalle("productoPrincipal") = productoPrincipal
            _detalle.Rows.Add(drDetalle)
        End Sub

        Public Sub RemoverDetalle(ByVal idProducto As Integer)
            Dim drDetalle As DataRow = _detalle.Rows.Find(idProducto)
            If drDetalle IsNot Nothing Then drDetalle.Delete()
        End Sub

        Public Sub AdicionarNovedad(ByVal idNovedad As Integer)
            If _novedades Is Nothing Then _novedades = GenerarEstructuraNovedades()
            Dim drNovedad As DataRow = _novedades.NewRow
            drNovedad("idNovedad") = idNovedad
            _novedades.Rows.Add(drNovedad)
        End Sub

        Public Function Acomodar() As Short
            Dim resultado As Short
            If _idPallet > 0 And _idPosicion > 0 And _idAcomodador > 0 Then
                Dim dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Add("@idPallet", SqlDbType.BigInt).Value = _idPallet
                    .SqlParametros.Add("@idPosicion", SqlDbType.Int).Value = _idPosicion
                    .SqlParametros.Add("@idAcomodador", SqlDbType.Int).Value = _idAcomodador
                    .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .ejecutarNonQuery("AsignarPalletAPosicionDeBodega", CommandType.StoredProcedure)
                    resultado = CShort(.SqlParametros("@returnValue").Value.ToString)
                End With
            Else
                resultado = 10
            End If
            Return resultado
        End Function

        Public Function Acomodar(ByVal posicion As String) As Short
            Dim resultado As Short
            Dim laPosicion As New WMS.PosicionBodega(posicion)
            If _idPallet > 0 And _idPosicion > 0 And _idAcomodador > 0 Then
                If laPosicion.IdPosicion > 0 Then
                    Me._idPosicion = laPosicion.IdPosicion
                    resultado = Me.Acomodar()
                Else
                    Throw New Exception("La posición especificada no existe.")
                End If
            Else
                resultado = 10
            End If
            Return resultado
        End Function

        Public Function ValidarTipoPallet(ByVal idPallet As Long) As Integer
            Dim tipoPallet As Integer
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idPallet", SqlDbType.BigInt).Value = idPallet
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .ejecutarNonQuery("ValidarTipoPallet", CommandType.StoredProcedure)
                    If Integer.TryParse(.SqlParametros("@resultado").Value.ToString, tipoPallet) Then
                        tipoPallet = .SqlParametros("@resultado").Value
                    Else
                        tipoPallet = 0
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return tipoPallet
        End Function

#End Region

#Region "métodos compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroPalletRecepcion
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroPalletRecepcion) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdPallet > 0 Then db.SqlParametros.Add("@idPallet", SqlDbType.BigInt).Value = .IdPallet
                If .IdOrdenRecepcion > 0 Then db.SqlParametros.Add("@idOrdenRecepcion", SqlDbType.Int).Value = .IdOrdenRecepcion
                If .IdCreador > 0 Then db.SqlParametros.Add("@idCreador", SqlDbType.Int).Value = .IdCreador
                If .IdFacturaGuia > 0 Then db.SqlParametros.Add("@idFacturaGuia", SqlDbType.Int).Value = .IdFacturaGuia
                If .IdTipoDetalleProducto > 0 Then db.SqlParametros.Add("@idTipoDetalleProducto", SqlDbType.SmallInt).Value = .IdTipoDetalleProducto
                If .IdEstado > 0 Then db.SqlParametros.Add("@idEstado", SqlDbType.Int).Value = .IdEstado
                dtDatos = db.ejecutarDataTable("ObtenerPalletRecepcion", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos

        End Function

        Public Overloads Shared Sub LlenarListado(ByVal filtro As FiltroPalletRecepcion, ByVal dtPallet As DataTable)
            Dim dbManager As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdPallet > 0 Then dbManager.SqlParametros.Add("@idPallet", SqlDbType.BigInt).Value = .IdPallet
                If .IdOrdenRecepcion > 0 Then dbManager.SqlParametros.Add("@idOrdenRecepcion", SqlDbType.Int).Value = .IdOrdenRecepcion
                If .IdCreador > 0 Then dbManager.SqlParametros.Add("@idCreador", SqlDbType.Int).Value = .IdCreador
                If .IdFacturaGuia > 0 Then dbManager.SqlParametros.Add("@idFacturaGuia", SqlDbType.Int).Value = .IdFacturaGuia
                If .IdTipoDetalleProducto > 0 Then dbManager.SqlParametros.Add("@idTipoDetalleProducto", SqlDbType.SmallInt).Value = .IdTipoDetalleProducto
                If .IdEstado > 0 Then dbManager.SqlParametros.Add("@idEstado", SqlDbType.Int).Value = .IdEstado
                dbManager.llenarDataTable(dtPallet, "ObtenerPalletRecepcion", CommandType.StoredProcedure)
            End With
        End Sub

        Public Overloads Shared Sub LlenarListado(ByVal dtPallet As DataTable)
            Dim filtro As New FiltroPalletRecepcion
            LlenarListado(filtro, dtPallet)
        End Sub

        Public Shared Function ObtenerInfoDetalle(ByVal idOrdenRecepcion As Long, Optional ByVal tipoDetalleProducto As Integer = 0) As DataTable
            Dim dt As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If tipoDetalleProducto > 0 Then .SqlParametros.Add("@tipoDetalleProducto", SqlDbType.Int).Value = tipoDetalleProducto
                    .SqlParametros.Add("@mostrarIdPallet", SqlDbType.Bit).Value = 1
                    .SqlParametros.Add("@idOrdenRecepcion", SqlDbType.Int).Value = idOrdenRecepcion
                    dt = .ejecutarDataTable("ObtenerInfoDetallePallet", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dt
        End Function

        Public Shared Function ObtenerInfoDetallePorMaterial(ByVal idOrdenRecepcion As Long, Optional ByVal serialesCargados As Boolean = False) As DataTable
            Dim dt As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idOrdenRecepcion", SqlDbType.Int).Value = idOrdenRecepcion
                    If serialesCargados Then .SqlParametros.Add("@serialesCargados", SqlDbType.SmallInt).Value = 1
                    dt = .ejecutarDataTable("ObtenerInfoDetallePalletPorMaterial", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dt
        End Function

        Public Shared Function ObtenerDetallePorPallet(ByVal idPallet As Long, Optional ByVal tipoDetalleProducto As Integer = 0) As DataTable
            Dim dtDatos As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If tipoDetalleProducto > 0 Then .SqlParametros.Add("@tipoDetalleProducto", SqlDbType.Int).Value = tipoDetalleProducto
                    .SqlParametros.Add("@idPallet", SqlDbType.Int).Value = idPallet
                    dtDatos = .ejecutarDataTable("ObtenerInfoDetallePallet", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Shared Function ObtenerDetallePalletGeneral(ByVal idOrdenRecepcion As Long) As DataTable
            Dim dt As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idOrdenRecepcion", SqlDbType.Int).Value = idOrdenRecepcion
                    dt = .ejecutarDataTable("ObtenerDetallePallets", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dt
        End Function

        Public Shared Function ObtenerNovedadesPallet(ByVal idOrdenRecepcion As Long) As DataTable
            Dim dt As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idOrdenRecepcion", SqlDbType.Int).Value = idOrdenRecepcion
                    dt = .ejecutarDataTable("ObtenerNovedadesPallets", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dt
        End Function

        Public Shared Function ObtenerPesoPalletGeneral(ByVal idOrdenRecepcion As Long) As String
            Dim _peso As String
            Dim dt As DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idOrdenRecepcion", SqlDbType.Int).Value = idOrdenRecepcion
                    dt = .ejecutarDataTable("ObtenerPesoPallets", CommandType.StoredProcedure)
                    If dt.Rows.Count > 0 Then
                        _peso = dt.Rows(0).Item("peso")
                    Else
                        _peso = "0"
                    End If

                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return _peso
        End Function

#End Region

    End Class
End Namespace