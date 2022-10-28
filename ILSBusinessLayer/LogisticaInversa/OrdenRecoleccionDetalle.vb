Namespace LogisticaInversa
    Public Class OrdenRecoleccionDetalle

#Region "Variables"
        Private _idDetalle As Integer
        Private _idOrden As Integer
        Private _material As String
        Private _referencia As String
        Private _idProducto As Integer
        Private _cantidad As Integer
        Private _valorMaterial As Decimal
        Private _seriales As OrdenRecoleccionSerial
        Private _listaReferencias As DataTable
#End Region

#Region "Propiedades"

        Public Property IdDetalle() As Integer
            Get
                Return _idDetalle
            End Get
            Set(ByVal value As Integer)
                _idDetalle = value
            End Set
        End Property

        Public Property IdOrden() As Integer
            Get
                Return _idOrden
            End Get
            Set(ByVal value As Integer)
                _idOrden = value
            End Set
        End Property

        Public Property Referencia() As String
            Get
                Return _referencia
            End Get
            Set(ByVal value As String)
                _referencia = value
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

        Public Property IdProducto() As Integer
            Get
                Return _idProducto
            End Get
            Set(ByVal value As Integer)
                _idProducto = value
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

        Public Property Seriales() As OrdenRecoleccionSerial
            Get
                Return _seriales
            End Get
            Set(ByVal value As OrdenRecoleccionSerial)
                _seriales = value
            End Set
        End Property

        Public Property ListaReferencias() As DataTable
            Get
                Return _listaReferencias
            End Get
            Set(ByVal value As DataTable)
                _listaReferencias = value
            End Set
        End Property

        Public Property valorMaterial() As Decimal
            Get
                Return _valorMaterial
            End Get
            Set(ByVal value As Decimal)
                _valorMaterial = value
            End Set
        End Property

#End Region

#Region "Metodos"
        Public Sub Eliminar(ByVal db As LMDataAccessLayer.LMDataAccess)
            db.SqlParametros.Clear()
            With db
                .agregarParametroSQL("@idDetalle", _idDetalle, SqlDbType.Int)
                .ejecutarNonQuery("EliminarOrdenRecoleccionDetalle", CommandType.StoredProcedure)
            End With
        End Sub

        Public Sub Actualizar(ByVal db As LMDataAccessLayer.LMDataAccess, ByVal idUsuario As Integer)

            Dim dvRef As New DataView(_listaReferencias)
            'inserto los nuevos productos
            dvRef.RowStateFilter = DataViewRowState.Added
            Dim esValido As Boolean = _seriales.ValidarSerialesRecoleccion(idUsuario, IdOrden)
            If esValido Then
                For Each fila As DataRowView In dvRef
                    _material = fila("material")
                    _idProducto = fila("idProducto")
                    _cantidad = fila("cantidad")
                    _valorMaterial = fila("valorMaterial")
                    With db
                        db.SqlParametros.Clear()
                        .agregarParametroSQL("@material", _material)
                        .agregarParametroSQL("@idProducto", _idProducto, SqlDbType.Int)
                        .agregarParametroSQL("@cantidad", _cantidad, SqlDbType.Int)
                        .agregarParametroSQL("@idOrden", _idOrden, SqlDbType.Int)
                        .agregarParametroSQL("@valorMaterial", _valorMaterial, SqlDbType.Decimal)
                        db.SqlParametros.Add("@idDetalle", SqlDbType.Int).Direction = ParameterDirection.Output
                        .ejecutarNonQuery("CrearOrdenRecoleccionDetalle", CommandType.StoredProcedure)
                        _idDetalle = db.SqlParametros("@idDetalle").Value
                        _seriales.IdDetalle = _idDetalle
                        _seriales.Actualizar(db)
                    End With

                Next
                'eliminación de productos 

                dvRef.RowStateFilter = DataViewRowState.Deleted
                For Each Auxfila As DataRowView In dvRef
                    If Auxfila("idDetalle").ToString() <> "" Then
                        _idDetalle = Auxfila("idDetalle")
                        Me.Eliminar(db)
                    End If
                Next
                'Actualizo los productos editados
                db.SqlParametros.Clear()
                dvRef.RowStateFilter = DataViewRowState.ModifiedCurrent Or DataViewRowState.ModifiedCurrent
                For Each Auxfila As DataRowView In dvRef
                    If Auxfila("idDetalle").ToString() <> "" Then
                        _material = Auxfila("material")
                        _idDetalle = Auxfila("idDetalle")
                        _cantidad = Auxfila("cantidad")
                        _valorMaterial = Auxfila("valorMaterial")
                        db.agregarParametroSQL("@idDetalle", _idDetalle, SqlDbType.Int)
                        db.agregarParametroSQL("@cantidad", _cantidad, SqlDbType.Int)
                        db.agregarParametroSQL("@valorMaterial", _valorMaterial, SqlDbType.Decimal)
                        db.ejecutarNonQuery("ActualizarOrdenRecoleccionDetalle", CommandType.StoredProcedure)
                        'actualizo los seriales para este material
                        db.agregarParametroSQL("@material", _material)
                        _seriales.IdDetalle = _idDetalle
                        _seriales.Actualizar(db)
                    End If
                Next
            Else
                Throw New Exception("1", New Exception("uno o varios seriales no cumplen con las validaciónes para crear la recolección"))
            End If
        End Sub

        Friend Sub Registrar(ByVal db As LMDataAccessLayer.LMDataAccess, ByVal idUsuario As Integer)
            db.SqlParametros.Clear()
            Dim dvRef As New DataView(_listaReferencias)
            dvRef.RowStateFilter = DataViewRowState.CurrentRows
            Dim esValido As Boolean = _seriales.ValidarSerialesRecoleccion(idUsuario)
            If esValido Then
                For Each fila As DataRowView In dvRef
                    _material = fila("material")
                    _idProducto = fila("idProducto")
                    _cantidad = fila("cantidad")
                    _valorMaterial = fila("valorMaterial")
                    With db
                        .SqlParametros.Clear()
                        .agregarParametroSQL("@material", _material)
                        .agregarParametroSQL("@idProducto", _idProducto, SqlDbType.Int)
                        .agregarParametroSQL("@cantidad", _cantidad, SqlDbType.Int)
                        .agregarParametroSQL("@idOrden", _idOrden, SqlDbType.Int)
                        .agregarParametroSQL("@valorMaterial", _valorMaterial, SqlDbType.Decimal)
                        db.SqlParametros.Add("@idDetalle", SqlDbType.Int).Direction = ParameterDirection.Output
                        .ejecutarNonQuery("CrearOrdenRecoleccionDetalle", CommandType.StoredProcedure)
                        _idDetalle = db.SqlParametros("@idDetalle").Value
                        _seriales.IdDetalle = _idDetalle
                        _seriales.Registrar(db)
                    End With
                Next
            Else
                Throw New Exception("1", New Exception("uno o varios seriales no cumplen con las validaciónes para crear la recolección"))
            End If
        End Sub

        Public Shared Function ObtenerEstructuraDeDatos() As DataTable
            Dim dt As New DataTable("OrdenRecoleccionDetalle")
            dt.Columns.Add(New DataColumn("idDetalle", GetType(Integer)))
            dt.Columns.Add(New DataColumn("idOrden", GetType(Integer)))
            dt.Columns.Add(New DataColumn("material", GetType(String)))
            dt.Columns.Add(New DataColumn("idProducto", GetType(Integer)))
            dt.Columns.Add(New DataColumn("cantidad", GetType(Integer)))
            dt.Columns.Add(New DataColumn("valorMaterial", GetType(Decimal)))
            dt.Columns.Add(New DataColumn("referencia", GetType(String)))
            Return dt
        End Function

        Public Sub CargarListaReferencias(ByVal idOrden As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idOrden", idOrden, SqlDbType.Int)
            _listaReferencias = db.ejecutarDataTable("ObtenerOrdenRecoleccionDetalle", CommandType.StoredProcedure)
        End Sub

        Private Sub CargarDatos()
            Dim db As New LMDataAccessLayer.LMDataAccess
            If _idDetalle > 0 Then db.agregarParametroSQL("@idDetalle", _idDetalle, SqlDbType.Int)
            If _idOrden > 0 And _material <> "" Then
                db.agregarParametroSQL("@idOrden", _idOrden, SqlDbType.Int)
                db.agregarParametroSQL("@material", _material, SqlDbType.Int)
            End If
        End Sub

        Public Shared Function ObtenerLog(ByVal idHistorial As Integer) As DataTable

            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idHistorial", idHistorial, SqlDbType.Int)
            Dim dt As DataTable = db.ejecutarDataTable("ObtenerlogOrdenRecoleccionDetalle", CommandType.StoredProcedure)
            Return dt
        End Function
#End Region

        Public Sub New()
            _seriales = New OrdenRecoleccionSerial
        End Sub

        Public Sub New(ByVal idDetalle As Integer)
            Me.New()
            _idDetalle = idDetalle
            Me.CargarDatos()
        End Sub

        Public Sub New(ByVal material As String, ByVal idOrden As Integer)
            Me.New()
            _material = material
            _idOrden = idOrden
            Me.CargarDatos()
        End Sub

    End Class
End Namespace


