Imports LMDataAccessLayer
Imports System.String

Public Class Site

#Region "Atributos"

    Private _idSite As Integer
    Private _idCallCenter As Integer
    Private _nombre As String
    Private _activo As Nullable(Of Boolean)
    Private _nombreCallCenter As String

    Private _listBodegas As List(Of Integer)
    Private _listUsuarios As List(Of Integer)

#End Region

#Region "Propiedades"

    Public Property IdSite As Integer
        Get
            Return _idSite
        End Get
        Set(value As Integer)
            _idSite = value
        End Set
    End Property

    Public Property IdCallCenter As Integer
        Get
            Return _idCallCenter
        End Get
        Set(value As Integer)
            _idCallCenter = value
        End Set
    End Property

    Public Property Nombre As String
        Get
            Return _nombre
        End Get
        Set(value As String)
            _nombre = value
        End Set
    End Property

    Public Property Activo As Boolean
        Get
            Return _activo
        End Get
        Set(value As Boolean)
            _activo = value
        End Set
    End Property

    Public Property NombreCallCenter As String
        Get
            Return _nombreCallCenter
        End Get
        Set(value As String)
            _nombreCallCenter = value
        End Set
    End Property

    Public Property ListaBodegas As List(Of Integer)
        Get
            Return _listBodegas
        End Get
        Set(value As List(Of Integer))
            _listBodegas = value
        End Set
    End Property

    Public Property ListaUsuarios As List(Of Integer)
        Get
            Return _listUsuarios
        End Get
        Set(value As List(Of Integer))
            _listUsuarios = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idSite As Integer)
        MyBase.New()
        _idSite = idSite
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idSite > 0 Then .SqlParametros.Add("@idSite", SqlDbType.Int).Value = _idSite
                    If _idCallCenter > 0 Then .SqlParametros.Add("@idCallCenter", SqlDbType.Int).Value = _idCallCenter
                    If Not String.IsNullOrEmpty(_nombre) Then .SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = _nombre
                    If _activo IsNot Nothing Then .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo

                    .ejecutarReader("ObtenerSites", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idSite").ToString, _idSite)
                            Integer.TryParse(.Reader("idCallCenter").ToString, _idCallCenter)
                            _nombre = .Reader("nombre").ToString
                            _activo = .Reader("activo")
                            _nombreCallCenter = .Reader("nombreCallCenter")
                        End If
                        .Reader.Close()
                    End If
                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Using
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Using dbManager As New LMDataAccess
            Try
                If Not String.IsNullOrEmpty(_nombre) Then
                    With dbManager
                        .SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = _nombre
                        .SqlParametros.Add("@idCallCenter", SqlDbType.Int).Value = _idCallCenter
                        .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo

                        If _listBodegas IsNot Nothing AndAlso _listBodegas.Count > 0 Then _
                            .SqlParametros.Add("@listaBodegas", SqlDbType.VarChar).Value = Join(",", _listBodegas.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                        If _listUsuarios IsNot Nothing AndAlso _listUsuarios.Count > 0 Then _
                            .SqlParametros.Add("@listaUsuarios", SqlDbType.VarChar).Value = Join(",", _listUsuarios.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                        .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                        .iniciarTransaccion()
                        .ejecutarNonQuery("RegistrarSite", CommandType.StoredProcedure)

                        Dim respuesta As Integer = .SqlParametros("@respuesta").Value
                        If respuesta = 0 Then
                            .confirmarTransaccion()
                        Else
                            Select Case respuesta
                                Case 1 : resultado.EstablecerMensajeYValor(respuesta, "El nombre del site ya se encuentra registrado")
                            End Select
                            .abortarTransaccion()
                        End If
                    End With
                Else
                    resultado.EstablecerMensajeYValor(100, "No se proporcionaron los datos suficientes para realizar el registro.")
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return resultado
    End Function

    Public Function Actualizar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Using dbManager As New LMDataAccess
            Try
                If _idSite > 0 Then
                    With dbManager
                        .SqlParametros.Add("@idSite", SqlDbType.Int).Value = _idSite
                        .SqlParametros.Add("@idCallCenter", SqlDbType.Int).Value = _idCallCenter
                        .SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = _nombre
                        .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo

                        If _listBodegas IsNot Nothing AndAlso _listBodegas.Count > 0 Then _
                            .SqlParametros.Add("@listBodegas", SqlDbType.VarChar).Value = Join(",", _listBodegas.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                        If _listUsuarios IsNot Nothing AndAlso _listUsuarios.Count > 0 Then _
                            .SqlParametros.Add("@listUsuarios", SqlDbType.VarChar).Value = Join(",", _listUsuarios.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                        .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                        .iniciarTransaccion()
                        .ejecutarNonQuery("ActualizarSite", CommandType.StoredProcedure)

                        Dim respuesta As Integer = .SqlParametros("@respuesta").Value
                        If respuesta = 0 Then
                            .confirmarTransaccion()
                        Else
                            .abortarTransaccion()
                        End If
                    End With
                Else
                    resultado.EstablecerMensajeYValor(100, "No se proporcionaron los datos suficientes para actualizar el registro.")
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return resultado
    End Function

#End Region

End Class
