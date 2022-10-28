Namespace LogisticaInversa
    Public Class OrdenRecoleccionAccesorio

#Region "Variables"
        Private _idAccesorio As Integer
        Private _idOrden As Integer
        Private _articulo As String
        Private _cantidadPedida As Integer
        Private _cantidadRecogida As Integer
        Private _cantidadEntregada As Integer
        Private _listaAccesorios As DataTable
#End Region

#Region "Propiedades"

        Public Property ListaAccesorios() As DataTable
            Get
                Return _listaAccesorios
            End Get
            Set(ByVal value As DataTable)
                _listaAccesorios = value
            End Set
        End Property

        Public Property CantidadEntregada() As Integer
            Get
                Return _cantidadEntregada
            End Get
            Set(ByVal value As Integer)
                _cantidadEntregada = value
            End Set
        End Property

        Public Property CantidadRecogida() As Integer
            Get
                Return _cantidadRecogida
            End Get
            Set(ByVal value As Integer)
                _cantidadRecogida = value
            End Set
        End Property

        Public Property CantidadPedida() As Integer
            Get
                Return _cantidadPedida
            End Get
            Set(ByVal value As Integer)
                _cantidadPedida = value
            End Set
        End Property

        Public Property Articulo() As String
            Get
                Return _articulo
            End Get
            Set(ByVal value As String)
                _articulo = value
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

        Public Property IdAccesorio() As Integer
            Get
                Return _idAccesorio
            End Get
            Set(ByVal value As Integer)
                _idAccesorio = value
            End Set
        End Property

#End Region

#Region "Metodos"

        Friend Sub Actualizar(ByVal db As LMDataAccessLayer.LMDataAccess)
            Dim dview As New DataView(_listaAccesorios)
            'elimino accesorios
            dview.RowStateFilter = DataViewRowState.Deleted
            db.SqlParametros.Clear()
            db.SqlParametros.Add("@idAccesorio", SqlDbType.Int)
            For Each fila As DataRowView In dview
                If fila("idAccesorio").ToString() <> "" Then
                    _idAccesorio = fila("idAccesorio")
                    db.SqlParametros("@idAccesorio").Value = _idAccesorio
                    db.ejecutarNonQuery("EliminarOrdenRecoleccionAccesorio", CommandType.StoredProcedure)
                End If
            Next
            _listaAccesorios.DefaultView.RowStateFilter = DataViewRowState.Added
            If _listaAccesorios.DefaultView.Count > 0 Then
                Me.Registrar(db)
            End If
        End Sub

        Public Shared Function ObtenerEstructuraDeDatos() As DataTable
            Dim dt As New DataTable("OrdenRecoleccionAccesorio")
            dt.Columns.Add(New DataColumn("IdAccesorio", GetType(Integer)))
            dt.Columns.Add(New DataColumn("idOrden", GetType(Integer)))
            dt.Columns.Add(New DataColumn("articulo", GetType(String)))
            dt.Columns.Add(New DataColumn("cantidadPedida", GetType(Integer)))
            dt.Columns.Add(New DataColumn("cantidadRecogida", GetType(Integer)))
            dt.Columns.Add(New DataColumn("cantidadEntregada", GetType(Integer)))
            dt.Columns.Add(New DataColumn("tipoArticulo", GetType(Integer)))
            dt.Columns.Add(New DataColumn("valorArticulo", GetType(Integer)))
            dt.Columns.Add(New DataColumn("tipoArticuloDescripcion", GetType(String)))
            Return dt
        End Function

        Friend Sub Registrar(ByVal db As LMDataAccessLayer.LMDataAccess)
            If _listaAccesorios IsNot Nothing Then
                db.SqlParametros.Clear()
                db.inicilizarBulkCopy()
                _listaAccesorios.Columns.Remove(_listaAccesorios.Columns("idOrden"))
                _listaAccesorios.Columns.Add(New DataColumn("idOrden", GetType(Integer), _idOrden.ToString()))
                db.BulkCopy.DestinationTableName = "dbo.OrdenRecoleccionaccesorio"
                db.BulkCopy.ColumnMappings.Add("idAccesorio", "idAccesorio")
                db.BulkCopy.ColumnMappings.Add("idOrden", "idOrden")
                db.BulkCopy.ColumnMappings.Add("articulo", "articulo")
                db.BulkCopy.ColumnMappings.Add("cantidadPedida", "cantidadPedida")
                db.BulkCopy.ColumnMappings.Add("tipoArticulo", "tipoArticulo")
                db.BulkCopy.ColumnMappings.Add("valorArticulo", "valorArticulo")
                db.BulkCopy.WriteToServer(_listaAccesorios, DataRowState.Added)
            End If
        End Sub

        Public Sub CargarListaAccesorios(ByVal idOrden As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idOrden", idOrden, SqlDbType.Int)
            _listaAccesorios = db.ejecutarDataTable("ObtenerOrdenRecoleccionAccesorio", CommandType.StoredProcedure)
        End Sub

        Public Sub ConfirmarAccesorios()
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.SqlParametros.Add("@cantidadRecogida", SqlDbType.Int)
            db.SqlParametros.Add("@cantidadEntregada", SqlDbType.Int)
            db.SqlParametros.Add("@idAccesorio", SqlDbType.Int)
            Try
                db.iniciarTransaccion()
                For Each fila As DataRow In _listaAccesorios.Rows
                    db.SqlParametros("@idAccesorio").Value = fila("idAccesorio")
                    db.SqlParametros("@cantidadRecogida").Value = fila("cantidadRecogida")
                    db.SqlParametros("@cantidadEntregada").Value = fila("cantidadEntregada")
                    db.ejecutarNonQuery("ConfirmarRecoleccionAccesorios", CommandType.StoredProcedure)
                Next
                db.confirmarTransaccion()
            Catch ex As Exception
                db.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                db.Dispose()
            End Try

        End Sub

        Public Shared Function ObtenerLog(ByVal idHistorial As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idHistorial", idHistorial, SqlDbType.Int)
            Dim dt As DataTable = db.ejecutarDataTable("ObtenerLogOrdenRecoleccionAccesorio", CommandType.StoredProcedure)

            Return dt
        End Function

#End Region
    End Class
End Namespace

