Namespace LogisticaInversa
    Public Class OrdenRecoleccionSerial

#Region "Variables"
        Private _idSerial As Integer
        Private _idDetalle As Integer
        Private _serial As String
        Private _tipoSerial As Integer
        Private _cajaVacia As Boolean
        Private _listaSeriales As DataTable
        Private _dtErroresValidacion As DataTable
#End Region

#Region "Propiedades"

        Public Property IdSerial() As Integer
            Get
                Return _idSerial
            End Get
            Set(ByVal value As Integer)
                _idSerial = value
            End Set
        End Property

        Public Property IdDetalle() As Integer
            Get
                Return _idDetalle
            End Get
            Set(ByVal value As Integer)
                _idDetalle = value
            End Set
        End Property

        Public Property Serial() As Integer
            Get
                Return _serial
            End Get
            Set(ByVal value As Integer)
                _serial = value
            End Set
        End Property

        Public Property TipoSerial() As Integer
            Get
                Return _tipoSerial
            End Get
            Set(ByVal value As Integer)
                _tipoSerial = value
            End Set
        End Property

        Public Property CajaVacia() As Integer
            Get
                Return _cajaVacia
            End Get
            Set(ByVal value As Integer)
                _cajaVacia = value
            End Set
        End Property

        Public Property ListaSeriales() As DataTable
            Get
                Return _listaSeriales
            End Get
            Set(ByVal value As DataTable)
                _listaSeriales = value
            End Set
        End Property

        Public Property ErroresValidacion() As DataTable
            Get
                Return _dtErroresValidacion
            End Get
            Set(ByVal value As DataTable)
                _dtErroresValidacion = value
            End Set
        End Property

#End Region

#Region "Metodos"


        Public Sub Actualizar(ByVal db As LMDataAccessLayer.LMDataAccess)
            Dim dview As New DataView(_listaSeriales)
            dview.RowFilter = "material='" & db.SqlParametros("@material").Value & "'"
            'elimino seriales de la referencia establecida
            dview.RowStateFilter = DataViewRowState.Deleted
            db.SqlParametros.Clear()
            For Each fila As DataRowView In dview
                If fila("idSerial").ToString() <> "" Then
                    _idSerial = fila("idSerial")
                    Me.Eliminar(db)
                End If
            Next
            dview.RowStateFilter = DataViewRowState.Added
            db.SqlParametros.Clear()
            db.SqlParametros.Add("@idDetalle", SqlDbType.Int)
            db.agregarParametroSQL("@cajaVacia", SqlDbType.Bit)
            db.agregarParametroSQL("@serial", _serial)
            For Each fila As DataRowView In dview
                db.SqlParametros("@idDetalle").Value = _idDetalle
                db.SqlParametros("@cajaVacia").Value = fila("cajaVacia")
                db.SqlParametros("@serial").Value = fila("serial")
                db.ejecutarNonQuery("RegistrarOrdenRecoleccionSerial", CommandType.StoredProcedure)
            Next
        End Sub

        Public Sub Eliminar(ByVal db As LMDataAccessLayer.LMDataAccess)
            db.SqlParametros.Clear()
            With db
                .agregarParametroSQL("@idSerial", _idSerial, SqlDbType.Int)

                .ejecutarNonQuery("EliminarOrdenRecoleccionSerial", CommandType.StoredProcedure)
            End With
        End Sub

        Public Sub Registrar(ByVal db As LMDataAccessLayer.LMDataAccess)
            If _listaSeriales IsNot Nothing Then
                Dim material As String = db.SqlParametros("@material").Value
                db.SqlParametros.Clear()
                _listaSeriales.Columns.Remove(_listaSeriales.Columns("idDetalle"))
                _listaSeriales.Columns.Add(New DataColumn("idDetalle", GetType(Integer), _idDetalle.ToString()))

                Dim filas As DataRow() = _listaSeriales.Select("material='" & material & "'")

                db.inicilizarBulkCopy()
                db.BulkCopy.DestinationTableName = "dbo.OrdenRecoleccionSerial"
                db.BulkCopy.ColumnMappings.Add("idDetalle", "idDetalle")
                db.BulkCopy.ColumnMappings.Add("serial", "serial")
                db.BulkCopy.ColumnMappings.Add("cajaVacia", "cajaVacia")
                db.BulkCopy.WriteToServer(filas)
            End If
        End Sub

        Public Function ValidarSerialesRecoleccion(ByVal idUsuario As Integer, Optional ByVal idRecoleccion As Integer = 0) As Boolean
            _dtErroresValidacion = New DataTable
            If _listaSeriales IsNot Nothing Then
                Dim db As New LMDataAccessLayer.LMDataAccess
                Try
                    db.agregarParametroSQL("@idUsuario", idUsuario, SqlDbType.Int)
                    db.ejecutarNonQuery("LimpiarValidarSerialesRecoleccion", CommandType.StoredProcedure)
                    db.inicilizarBulkCopy()
                    db.BulkCopy.ColumnMappings.Add("serial", "serial")
                    db.BulkCopy.ColumnMappings.Add("material", "material")
                    If _listaSeriales.Columns.Contains("lineaArchivo") Then db.BulkCopy.ColumnMappings.Add("lineaArchivo", "lineaArchivo")
                    If _listaSeriales.Columns.Contains("idUsuario") Then _listaSeriales.Columns.Remove("idUsuario")
                    _listaSeriales.Columns.Add(New DataColumn("idUsuario", GetType(Integer), idUsuario.ToString()))
                    db.BulkCopy.ColumnMappings.Add("idUsuario", "idUsuario")
                    db.BulkCopy.DestinationTableName = "dbo.ValidacionSerialRecoleccion"
                    db.BulkCopy.WriteToServer(_listaSeriales, DataRowState.Added)
                    If idRecoleccion > 0 Then db.agregarParametroSQL("@idRecoleccion", idRecoleccion, SqlDbType.Int)
                    _dtErroresValidacion = db.ejecutarDataTable("ValidarSerialesRecoleccion", CommandType.StoredProcedure)
                Finally
                    db.Dispose()
                End Try
            End If
            Return (_dtErroresValidacion.Rows.Count = 0)
        End Function

        Public Shared Function ObtenerEstructuraDeDatos() As DataTable
            Dim dt As New DataTable("OrdenRecoleccionSerial")
            dt.Columns.Add(New DataColumn("IdSerial", GetType(Integer)))
            dt.Columns.Add(New DataColumn("IdDetalle", GetType(Integer)))
            dt.Columns.Add(New DataColumn("Serial", GetType(String)))
            dt.Columns.Add(New DataColumn("CajaVacia", GetType(Boolean)))
            dt.Columns.Add(New DataColumn("lineaArchivo", GetType(Integer)))
            dt.Columns.Add(New DataColumn("idUsuario", GetType(Boolean)))
            Return dt
        End Function

        Public Sub CargarListaSeriales(ByVal idOrden As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idOrden", idOrden, SqlDbType.Int)
            _listaSeriales = db.ejecutarDataTable("ObtenerOrdenRecoleccionSerial", CommandType.StoredProcedure)
        End Sub

        Public Shared Function ObtenerLog(ByVal idHistorial As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idHistorial", idHistorial, SqlDbType.Int)
            Dim dt As DataTable = db.ejecutarDataTable("ObtenerlogOrdenRecoleccionSerial", CommandType.StoredProcedure)
            Return dt
        End Function

#End Region

    End Class
End Namespace


