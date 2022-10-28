Imports LMDataAccessLayer

Public Class Colores
#Region "Atributos"
    Private _idUsuario As Integer
    Private _mensaje As String
    Private _resultado As Integer
    Private _color As String
    Private _idColor As Integer
    Private _idEstado As Integer
#End Region

#Region "Propiedades"

    Public Property Resultado() As Integer
        Get
            Return _resultado
        End Get
        Set(value As Integer)
            _resultado = value
        End Set
    End Property

    Public Property IdUsuario() As Integer
        Get
            Return _idUsuario
        End Get
        Set(value As Integer)
            _idUsuario = value
        End Set
    End Property

    Public Property Mensaje() As String
        Get
            Return _mensaje
        End Get
        Set(value As String)
            _mensaje = value
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

    Public Property IdColor() As Integer
        Get
            Return _idColor
        End Get
        Set(value As Integer)
            _idColor = value
        End Set
    End Property

    Public Property IdEstado() As Integer
        Get
            Return _idEstado
        End Get
        Set(value As Integer)
            _idEstado = value
        End Set
    End Property

#End Region

#Region "Constructores"
    Public Sub New()
        MyBase.New()
    End Sub
#End Region

#Region "Metodos Publicos"

    Public Function ObtenerColores() As DataTable
        Dim dtResultado As New DataTable
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Clear()
                If _idColor > 0 Then .SqlParametros.Add("@idColor", SqlDbType.Int).Value = _idColor
                If _color <> Nothing Then .SqlParametros.Add("@Color", SqlDbType.VarChar).Value = _color
                If _idEstado >= 0 Then .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = _idEstado
                dtResultado = .ejecutarDataTable("ObtenerColores", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtResultado
    End Function

    Public Sub RegistrarColor()
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    .iniciarTransaccion()
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@Color", SqlDbType.VarChar).Value = _color
                    .SqlParametros.Add("@idUsuario", SqlDbType.BigInt).Value = _idUsuario
                    .SqlParametros.Add("@idColorCreado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .ejecutarNonQuery("RegistrarColores", CommandType.StoredProcedure)
                    _resultado = .SqlParametros("@idColorCreado").Value

                    If .SqlParametros("@idColorCreado").Value > 0 Then
                        .confirmarTransaccion()
                        _resultado = .SqlParametros("@idColorCreado").Value
                        _mensaje = "Registro Exitoso"
                    ElseIf .SqlParametros("@idColorCreado").Value < 0 Then
                        .abortarTransaccion()
                        _resultado = .SqlParametros("@idColorCreado").Value
                        _mensaje = "Ya se encuentra registrado en el en el sistema el color "
                    ElseIf .SqlParametros("@idColorCreado").Value = 0 Then
                        .abortarTransaccion()
                        _resultado = .SqlParametros("@idColorCreado").Value
                        _mensaje = "Error al registrar el color."
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message)
            End Try
        End Using
    End Sub

    Public Sub RegistrarCambioColor()
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idColor", SqlDbType.Int).Value = _idColor
                    .SqlParametros.Add("@color", SqlDbType.VarChar).Value = _color
                    .SqlParametros.Add("@idEstado", SqlDbType.VarChar).Value = _idEstado
                    .SqlParametros.Add("@idRespuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .iniciarTransaccion()
                    .ejecutarNonQuery("RegistrarCambioColor", CommandType.StoredProcedure)
                    _resultado = .SqlParametros("@idRespuesta").Value
                    If .SqlParametros("@idRespuesta").Value = 0 Then
                        .confirmarTransaccion()
                        _resultado = .SqlParametros("@idRespuesta").Value
                    Else
                        .abortarTransaccion()
                        _resultado = .SqlParametros("@idRespuesta").Value
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message)
            End Try
        End Using
    End Sub

#End Region

End Class
