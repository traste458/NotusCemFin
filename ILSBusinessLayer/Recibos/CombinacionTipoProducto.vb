Imports LMDataAccessLayer
Namespace Recibos
    Public Class CombinacionTipoProducto

#Region "variables"

        Private _idTipoProductoPrimario As Short
        Private _idTipoProductoSecundario As Short
        Private _fechaCreacion As Date
        Private _idCreador As Long
        Private _descripcion As String
        Private _error As String


#End Region

#Region "propiedades"

        Public ReadOnly Property IdTipoProductoPrimario() As Short
            Get
                Return _idTipoProductoPrimario
            End Get
        End Property

        Public ReadOnly Property IdTipoProductoSecundario() As Short
            Get
                Return _idTipoProductoSecundario
            End Get
        End Property

        Public ReadOnly Property FechaCreacion() As DateTime
            Get
                Return _fechaCreacion
            End Get
        End Property

        Public Property IdCreador() As Long
            Get
                Return _idCreador
            End Get
            Set(ByVal value As Long)
                _idCreador = value
            End Set
        End Property

        Public Property Descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
        End Property

        Public ReadOnly Property InfoError() As String
            Get
                Return _error
            End Get
        End Property

#End Region

#Region "contructores"

        Public Sub New()
            MyBase.New()
            _error = String.Empty
        End Sub

        Public Sub New(ByVal idTipoPrimario As Short, ByVal idTipoSecundario As Short)
            Me.New()
            Me.CargarDatos(idTipoPrimario, idTipoSecundario)
        End Sub


#End Region

#Region "metodos privados"

        Private Sub CargarDatos(ByVal idTipoPrimario As Short, ByVal idTipoSecundario As Short)
            Dim db As New LMDataAccess
            Try
                db.SqlParametros.Add("@idTipoPrimario", SqlDbType.SmallInt).Value = idTipoPrimario
                db.SqlParametros.Add("@idTipoSecundario", SqlDbType.SmallInt).Value = idTipoSecundario
                db.ejecutarReader("ObtenerInfoCombinacionTipoProducto", CommandType.StoredProcedure)
                If db.Reader.Read Then
                    _idTipoProductoPrimario = idTipoPrimario
                    _idTipoProductoSecundario = idTipoSecundario
                    _fechaCreacion = db.Reader("fechaCrecion").ToString()
                    _idCreador = db.Reader("idCreador").ToString()
                    _descripcion = db.Reader("descripcion").ToString()
                End If
            Catch ex As Exception
                _error = "Error al cargar los datos. " & ex.Message
            Finally
                If Not db.Reader.IsClosed Then db.Reader.Close()
                db.Dispose()
            End Try
        End Sub

#End Region

#Region "metodos publicos"

        Public Function Crear() As Boolean
            Dim db As New LMDataAccess
            Dim retorno As Boolean = False
            If _idTipoProductoPrimario > 0 AndAlso _idTipoProductoSecundario > 0 Then
                With db
                    With .SqlParametros
                        .Add("@idTipoPrimario", SqlDbType.SmallInt).Value = _idTipoProductoPrimario
                        .Add("@idTipoSecundario", SqlDbType.SmallInt).Value = _idTipoProductoSecundario
                        .Add("@idCreador", SqlDbType.BigInt).Value = _idCreador
                        .Add("@descripcion", SqlDbType.VarChar).Value = _descripcion                        
                        .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                    End With

                    Try
                        Dim result As Integer
                        .iniciarTransaccion()
                        .ejecutarNonQuery("CrearInfoCombinacionTipoProducto", CommandType.StoredProcedure)
                        result = .SqlParametros("@result").Value
                        If result = 0 Then                            
                            CargarDatos(_idTipoProductoPrimario, _idTipoProductoSecundario)
                            .confirmarTransaccion()
                            retorno = True
                        End If
                    Catch ex As Exception
                        Throw New Exception(ex.Message)
                    Finally                        
                        db.Dispose()
                    End Try
                End With
            Else
                Me._error = "Los tipos de datos no estan establecidos."
            End If
            Return retorno
        End Function

        Public Function Crear(ByVal idTipoPrimario As Integer, ByVal listaProductoSecundario As ArrayList) As Boolean
            Dim db As New LMDataAccess
            Dim retorno As Boolean = False
            _idTipoProductoPrimario = idTipoPrimario
            If _idTipoProductoPrimario > 0 Then
                With db
                    Try
                        .iniciarTransaccion()
                        With .SqlParametros
                            For i As Integer = 0 To listaProductoSecundario.Count - 1
                                .Clear()
                                .Add("@idTipoPrimario", SqlDbType.SmallInt).Value = _idTipoProductoPrimario
                                .Add("@idTipoSecundario", SqlDbType.SmallInt).Value = listaProductoSecundario(i)
                                .Add("@idCreador", SqlDbType.BigInt).Value = _idCreador
                                .Add("@descripcion", SqlDbType.VarChar).Value = _descripcion
                                .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                                db.ejecutarNonQuery("CrearInfoCombinacionTipoProducto", CommandType.StoredProcedure)
                            Next
                        End With
                        .confirmarTransaccion()
                        retorno = True
                    Catch ex As Exception
                        db.abortarTransaccion()
                        Throw New Exception(ex.Message)
                    Finally
                        db.Dispose()
                    End Try
                End With
            Else
                Me._error = "Los tipos de datos no estan establecidos."
            End If
            Return retorno
        End Function

        Public Function Actualizar() As Boolean
            Dim retorno As Boolean = False
            If _idTipoProductoPrimario > 0 AndAlso _idTipoProductoSecundario > 0 Then
                Dim db As New LMDataAccess
                Try
                    With db
                        With .SqlParametros
                            .Add("@idTipoPrimario", SqlDbType.SmallInt).Value = _idTipoProductoPrimario
                            .Add("@idTipoSecundario", SqlDbType.SmallInt).Value = _idTipoProductoSecundario
                            .Add("@descripcion", SqlDbType.VarChar).Value = _descripcion
                            .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                        End With
                        Dim result As Integer
                        .iniciarTransaccion()
                        .ejecutarNonQuery("ActualizarInfoCombinacionTipoProducto", CommandType.StoredProcedure)
                        result = .SqlParametros("@result").Value
                        If result = 0 Then
                            CargarDatos(_idTipoProductoPrimario, _idTipoProductoSecundario)
                            .confirmarTransaccion()
                            retorno = True
                        End If
                    End With
                Catch ex As Exception
                    Throw New Exception(ex.Message)
                Finally                    
                    db.Dispose()
                End Try
            End If
            Return retorno
        End Function

        Public Function Eliminar() As Boolean
            Dim retorno As Boolean = False
            If _idTipoProductoPrimario > 0 AndAlso _idTipoProductoSecundario > 0 Then
                Dim db As New LMDataAccess
                Try
                    With db
                        With .SqlParametros
                            .Add("@idTipoPrimario", SqlDbType.SmallInt).Value = _idTipoProductoPrimario
                            .Add("@idTipoSecundario", SqlDbType.SmallInt).Value = _idTipoProductoSecundario
                            .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                        End With
                        Dim result As Integer
                        .iniciarTransaccion()
                        .ejecutarNonQuery("EliminarInfoCombinacionTipoProducto", CommandType.StoredProcedure)
                        result = .SqlParametros("@result").Value
                        If result = 0 Then
                            .confirmarTransaccion()
                            retorno = True
                        End If
                    End With
                Catch ex As Exception
                    Throw New Exception(ex.Message)
                Finally                    
                    db.Dispose()
                End Try
            Else
                _error = "No se han especificado los tipos de producto."
            End If
            Return retorno
        End Function

#End Region

#Region "metodos compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New Estructuras.FiltroCombinacionTipoProducto
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As Estructuras.FiltroCombinacionTipoProducto) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdTipoPrimario > 0 Then db.SqlParametros.Add("@idTipoPrimario", SqlDbType.SmallInt).Value = .IdTipoPrimario
                If .IdTipoSecundario > 0 Then db.SqlParametros.Add("@idTipoSecundario", SqlDbType.SmallInt).Value = .IdTipoSecundario
                If .IdCreador > 0 Then db.SqlParametros.Add("@idCreador", SqlDbType.SmallInt).Value = .IdCreador
                If .Observacion <> String.Empty Then db.SqlParametros.Add("@descripcion", SqlDbType.VarChar).Value = .Observacion
                If .FechaInicial > Date.MinValue Then db.SqlParametros.Add("@fechaInicial", SqlDbType.Date).Value = .FechaInicial
                If .FechaFinal > Date.MinValue Then db.SqlParametros.Add("@fechaFinal", SqlDbType.Date).Value = .FechaFinal                
                dtDatos = db.ejecutarDataTable("ObtenerInfoCombinacionTipoProducto", CommandType.StoredProcedure)
            End With
            Return dtDatos
        End Function

#End Region

    End Class
End Namespace

