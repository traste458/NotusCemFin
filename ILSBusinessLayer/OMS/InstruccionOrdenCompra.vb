Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras

Namespace OMS

    Public Class InstruccionOrdenCompra
#Region "Variables"
        Private _idInstruccion As Integer
        Private _idDetalleOrdenCompra As Integer
        Private _idSubproducto As String
        Private _subProducto As Productos.Producto
        Private _idTipoDistribucion As Integer
        Private _porcentaje As Decimal
        Private _idEstado As Integer
        Private _error As String
#End Region

#Region "Propiedades"

        Public Property IdEstado() As Integer
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Integer)
                _idEstado = value
            End Set
        End Property

        Public Property Porcentaje() As Decimal
            Get
                Return _porcentaje
            End Get
            Set(ByVal value As Decimal)
                _porcentaje = value
            End Set
        End Property

        Public Property IdTipoDistribucion() As Integer
            Get
                Return _idTipoDistribucion
            End Get
            Set(ByVal value As Integer)
                _idTipoDistribucion = value
            End Set
        End Property

        Public Property IdSubproducto() As Integer
            Get
                Return _idSubproducto
            End Get
            Set(ByVal value As Integer)
                _idSubproducto = value
            End Set
        End Property

        Public ReadOnly Property Subproducto() As Productos.Producto
            Get
                If Me._subProducto Is Nothing Then
                    If Me._idSubproducto > 0 Then
                        Return New Productos.Producto(CInt(Me._idSubproducto))
                    End If
                    Return New Productos.Producto()
                Else
                    Return Me._subProducto
                End If                
            End Get
        End Property

        Public Property IdDetalleOrdenCompra() As Integer
            Get
                Return _idDetalleOrdenCompra
            End Get
            Set(ByVal value As Integer)
                _idDetalleOrdenCompra = value
            End Set
        End Property

        Public Property IdInstruccion() As Integer
            Get
                Return _idInstruccion
            End Get
            Set(ByVal value As Integer)
                _idInstruccion = value
            End Set
        End Property

        Public ReadOnly Property InfoError() As String
            Get
                Return _error
            End Get
        End Property

#End Region

#Region "Constructoes"
        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idInstruccion As Integer)
            CargarDatos(idInstruccion)
        End Sub
#End Region

#Region "Metodos Privados"
        Private Sub CargarDatos(ByVal idInstruccion As Integer)
            Dim db As New LMDataAccess
            Try
                db.SqlParametros.Add("@idInstruccion", SqlDbType.Int).Value = idInstruccion
                db.ejecutarReader("ObtenerInfoInstruccionOrdenCompra", CommandType.StoredProcedure)
                If db.Reader.Read Then
                    _idInstruccion = idInstruccion
                    _idDetalleOrdenCompra = CLng(db.Reader("idDetalleOrdenCompra"))
                    _idSubproducto = CInt(db.Reader("idSubproducto"))
                    _idTipoDistribucion = CInt(db.Reader("idTipoDistribucion"))
                    _porcentaje = CDec(db.Reader("porcentaje"))
                    _idEstado = CInt(db.Reader("idEstado"))                    
                End If
                Me._error = String.Empty
            Catch ex As Exception
                Me._error = "Error al cargar datos " & ex.Message
            Finally
                db.Dispose()
            End Try
        End Sub
#End Region


#Region "Metodos Publicos"

        Public Function Crear()
            Dim db As New LMDataAccess
            Dim retorno As Boolean
            If _idDetalleOrdenCompra > 0 And _idSubproducto > 0 And _idTipoDistribucion > 0 And _porcentaje > 0 Then
                With db
                    With .SqlParametros
                        .Add("@idDetalleOrdenCompra", SqlDbType.Int).Value = _idDetalleOrdenCompra
                        .Add("@idSubproducto", SqlDbType.Int).Value = _idSubproducto
                        .Add("@idTipoDistribucion", SqlDbType.Int).Value = _idTipoDistribucion
                        .Add("@porcentaje", SqlDbType.Int).Value = _porcentaje
                        .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                    End With

                    Try
                        Dim result As Integer
                        .iniciarTransaccion()
                        .ejecutarNonQuery("CrearInfoInstruccioOrdenCompra", CommandType.StoredProcedure)
                        result = .SqlParametros("@result").Value
                        If result = 0 Then
                            _idInstruccion = CInt(.SqlParametros("@identity").Value)
                            CargarDatos(_idInstruccion)
                            .confirmarTransaccion()
                            retorno = True
                        End If
                    Catch ex As Exception
                        Me._error = "Error al crear la instruccion de orden de compra. " & ex.Message
                    Finally
                        db.Dispose()
                    End Try
                End With
            Else
                Me._error = "Los datos para crear la instruccion de orden de compra no esta completos"
            End If
            Return retorno
        End Function

        Public Function Actualizar() As Boolean
            Dim retorno As Boolean = False
            If _idInstruccion > 0 Then
                Dim db As New LMDataAccess
                Try
                    With db
                        With .SqlParametros
                            .Add("@idInstruccion", SqlDbType.Int).Value = _idInstruccion
                            .Add("@idDetalleOrdenCompra", SqlDbType.Int).Value = _idDetalleOrdenCompra
                            .Add("@idSubproducto", SqlDbType.Int).Value = _idSubproducto
                            .Add("@idTipoDistribucion", SqlDbType.Int).Value = _idTipoDistribucion
                            .Add("@porcentaje", SqlDbType.Int).Value = _porcentaje
                            .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                            .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                        End With
                        Dim result As Integer
                        .iniciarTransaccion()
                        .ejecutarNonQuery("ActualizarInfoInstruccionOrdenCompra", CommandType.StoredProcedure)
                        result = .SqlParametros("@result").Value
                        If result = 0 Then
                            CargarDatos(_idTipoDistribucion)
                            .confirmarTransaccion()
                            retorno = True
                        End If
                    End With
                Catch ex As Exception
                    Throw New Exception(ex.Message)
                Finally
                    If Not db.Reader.IsClosed Then db.Reader.Close()
                    db.Dispose()
                End Try
            End If
            Return retorno
        End Function

#End Region


#Region "Metodos Compartidos"
        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroInstruccionOrdenCompra
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroInstruccionOrdenCompra) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdInstruccion > 0 Then db.SqlParametros.Add("@idInstruccion", SqlDbType.Int).Value = .IdInstruccion
                If .IdDetalleOrdenCompra > 0 Then db.SqlParametros.Add("@idDetalleOrdenCompra", SqlDbType.Int).Value = .IdDetalleOrdenCompra
                If .IdSubproducto > 0 Then db.SqlParametros.Add("@idSubproducto", SqlDbType.Int).Value = .IdSubproducto
                If .IdTipoDistribucion > 0 Then db.SqlParametros.Add("@idTipoDistribucion", SqlDbType.Int).Value = .IdTipoDistribucion
                If .Porcentaje > 0 Then db.SqlParametros.Add("@porcentaje", SqlDbType.Decimal).Value = .Porcentaje
                If .IdEstado > 0 Then db.SqlParametros.Add("@idEstado", SqlDbType.Int).Value = .IdEstado
                dtDatos = db.ejecutarDataTable("ObtenerInfoInstruccionOrdenCompra", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos
        End Function

        Public Shared Function GenerarEstructuraDeTablaDatos() As DataTable
            Dim dtAux As New DataTable
            With dtAux
                .Columns.Add("idDetalleOrdenCompra", GetType(Integer))
                .Columns.Add("idSubproducto", GetType(Integer))
                .Columns.Add("idTipoDistribucion", GetType(Integer))
                .Columns.Add("porcentaje", GetType(Decimal))
                .Columns.Add("idEstado", GetType(Integer))
            End With
            Dim pkColumn(0) As DataColumn
            pkColumn(0) = dtAux.Columns("idSubproducto")
            dtAux.PrimaryKey = pkColumn
            Return dtAux
        End Function

        Public Shared Sub AdicionarMaterial(ByVal dtDatos As DataTable, ByVal idDetalleOrdenCompra As Integer, ByVal idSubproducto As Integer, _
        ByVal idTipoDistribucion As Integer, ByVal porcentaje As Decimal, ByVal idEstado As Integer)
            Dim drAux As DataRow
            drAux = dtDatos.Rows.Find(idSubproducto)
            If drAux Is Nothing Then
                drAux = dtDatos.NewRow
                drAux("idDetalleOrdenCompra") = idDetalleOrdenCompra
                drAux("idSubproducto") = idSubproducto                
                drAux("idTipoDistribucion") = idTipoDistribucion
                drAux("porcentaje") = porcentaje
                drAux("idEstado") = idEstado
                dtDatos.Rows.Add(drAux)
            End If
        End Sub

        Public Shared Sub RegistrarInstruccion(ByVal dtDatos As DataTable)
            Dim dbManager As New LMDataAccess            
            Try
                With dbManager
                    .inicilizarBulkCopy()
                    With .BulkCopy
                        .DestinationTableName = "InstruccionOrdenCompra"
                        .ColumnMappings.Add("idDetalleOrdenCompra", "idDetalleOrdenCompra")
                        .ColumnMappings.Add("idSubProducto", "idSubProducto")
                        .ColumnMappings.Add("idTipoDistribucion", "idTipoDistribucion")
                        .ColumnMappings.Add("porcentaje", "porcentaje")
                        .ColumnMappings.Add("idEstado", "idEstado")
                        .WriteToServer(dtDatos)
                    End With
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

#End Region


    End Class

End Namespace
