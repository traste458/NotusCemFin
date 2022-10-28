Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras

Namespace OMS
    Public Class TipoDistribucion

#Region "Variables"
        Private _idTipoDistribucion As Integer
        Private _descripcion As String
        Private _ordenOrdinal As Integer
        Private _error As String
#End Region

#Region "Propiedades"
        Public Property IdTipoDistribucion() As Integer
            Get
                Return _idTipoDistribucion
            End Get
            Set(ByVal value As Integer)
                _idTipoDistribucion = value
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

        Public Property OrdenOrdinal() As Integer
            Get
                Return _ordenOrdinal
            End Get
            Set(ByVal value As Integer)
                _ordenOrdinal = value
            End Set
        End Property

        Public ReadOnly Property InfoError() As String
            Get
                Return _error
            End Get
        End Property
#End Region

#Region "Constructores"
        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idTipoDistribucion As Integer)
            CargarDatos(idTipoDistribucion)
        End Sub

#End Region

#Region "Metodos Privados"

        Private Sub CargarDatos(ByVal idTipo As Integer)
            Dim db As New LMDataAccess
            Try
                db.SqlParametros.Add("@idTipoDistribucion", SqlDbType.Int).Value = idTipo                
                db.ejecutarReader("ObtenerTipoDistribucion", CommandType.StoredProcedure)
                If db.Reader.Read Then
                    _idTipoDistribucion = idTipo
                    _descripcion = db.Reader("descripcion").ToString()
                    _ordenOrdinal = CInt(db.Reader("ordenOrdinal"))
                End If
            Catch ex As Exception
                Me._error = "Error al cargar datos " & ex.Message
            Finally
                db.Dispose()
            End Try
        End Sub

#End Region

#Region "Metodos Publicos"

        Public Function Crear() As Boolean
            Dim db As New LMDataAccess
            Dim retorno As Boolean
            If _descripcion <> String.Empty Then
                With db
                    With .SqlParametros
                        .Add("@descripcion", SqlDbType.VarChar).Value = _descripcion
                        .Add("@ordenOrdinal", SqlDbType.Int).Value = _ordenOrdinal
                        .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                    End With

                    Try
                        Dim result As Integer
                        .iniciarTransaccion()
                        .ejecutarNonQuery("CrearInfoTipoDistribucion", CommandType.StoredProcedure)
                        result = .SqlParametros("@result").Value
                        If result = 0 Then
                            _idTipoDistribucion = CInt(.SqlParametros("@identity").Value)
                            CargarDatos(_idTipoDistribucion)
                            .confirmarTransaccion()
                            retorno = True
                        End If
                    Catch ex As Exception
                        Me._error = "Error al crear el tipo distribución indicado. " & ex.Message
                    Finally
                        db.Dispose()
                    End Try
                End With
            End If
            Return retorno
        End Function

        Public Function Actualizar() As Boolean
            Dim retorno As Boolean = False
            If _idTipoDistribucion > 0 Then
                Dim db As New LMDataAccess
                Try
                    With db
                        With .SqlParametros
                            .Add("@idTipoDistribucion", SqlDbType.SmallInt).Value = _idTipoDistribucion
                            .Add("@descripcion", SqlDbType.VarChar).Value = _descripcion
                            .Add("@ordenOrdinal", SqlDbType.Int).Value = _ordenOrdinal
                            .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                        End With
                        Dim result As Integer
                        .iniciarTransaccion()
                        .ejecutarNonQuery("ActualizarInfoTipoDistribucion", CommandType.StoredProcedure)
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


#Region "metodos Compartidos"
        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroTipoDistribucion
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroTipoDistribucion) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdTipoDistribucion > 0 Then db.SqlParametros.Add("@idTipoDistribucion", SqlDbType.SmallInt).Value = .IdTipoDistribucion
                If .Descripcion <> String.Empty Then db.SqlParametros.Add("@descripcion", SqlDbType.VarChar).Value = .Descripcion.ToString
                If .OrdenOrdinal > 0 Then db.SqlParametros.Add("@ordenOrdinal", SqlDbType.Int).Value = .OrdenOrdinal
                dtDatos = db.ejecutarDataTable("ObtenerTipoDistribucion", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos

        End Function

#End Region

    End Class
End Namespace
