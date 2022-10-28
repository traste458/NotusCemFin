Imports LMDataAccessLayer
Imports System.String
Imports ILSBusinessLayer.MensajeriaEspecializada.HerramientasMensajeria
Imports System.Web

Public Class CallCenter

#Region "Atributos"

    Private _idCallCenter As Integer
    Private _nombre As String
    Private _nombreContacto As String
    Private _telefonoContacto As String
    Private _activo As Nullable(Of Boolean)

    Private _listaIdTiposServicios As List(Of Integer)
#End Region

#Region "Propiedades"

    Public Property IdCallCenter As Integer
        Get
            Return _idCallCenter
        End Get
        Set(value As Integer)
            _idCallCenter = value
        End Set
    End Property

    Public Property NombreCallCenter As String
        Get
            Return _nombre
        End Get
        Set(value As String)
            _nombre = value
        End Set
    End Property

    Public Property NombreContacto As String
        Get
            Return _nombreContacto
        End Get
        Set(value As String)
            _nombreContacto = value
        End Set
    End Property

    Public Property TelefonoContacto As String
        Get
            Return _telefonoContacto
        End Get
        Set(value As String)
            _telefonoContacto = value
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

    Public Property ListaIdTiposServicios As List(Of Integer)
        Get
            Return _listaIdTiposServicios
        End Get
        Set(value As List(Of Integer))
            _listaIdTiposServicios = value
        End Set
    End Property
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idCallCenter As Integer)
        MyBase.New()
        _idCallCenter = idCallCenter
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Public Sub CargarDatos()
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    Dim idUsuarioConsulta As Integer = 0

                    If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.Session IsNot Nothing Then
                        If Not EsNuloOVacio(HttpContext.Current.Session("usxp001")) Then Integer.TryParse(HttpContext.Current.Session("usxp001").ToString, idUsuarioConsulta)
                    End If
                    If _idCallCenter > 0 Then .SqlParametros.Add("@idCallCenter", SqlDbType.Int).Value = _idCallCenter
                    If Not String.IsNullOrEmpty(_nombre) Then .SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = _nombre
                    If Not String.IsNullOrEmpty(_nombreContacto) Then .SqlParametros.Add("@nombreContacto", SqlDbType.VarChar).Value = _nombreContacto
                    If Not String.IsNullOrEmpty(_telefonoContacto) Then .SqlParametros.Add("@telefonoContacto", SqlDbType.VarChar).Value = _telefonoContacto
                    If _activo IsNot Nothing Then .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo
                    If idUsuarioConsulta > 0 Then .SqlParametros.Add("@idUsuarioConsulta", SqlDbType.Int).Value = idUsuarioConsulta

                    .ejecutarReader("ObtenerCallCenters", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idCallCenter").ToString, _idCallCenter)
                            _nombre = .Reader("nombreCallCenter").ToString
                            _nombreContacto = .Reader("nombreContacto")
                            _telefonoContacto = .Reader("telefonoContacto")
                            _activo = .Reader("activo")
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
                        If Not String.IsNullOrEmpty(_nombreContacto) Then .SqlParametros.Add("@nombreContacto", SqlDbType.VarChar).Value = _nombreContacto
                        If Not String.IsNullOrEmpty(_telefonoContacto) Then .SqlParametros.Add("@telefonoContacto", SqlDbType.VarChar).Value = _telefonoContacto
                        .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo
                        .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        If _listaIdTiposServicios IsNot Nothing AndAlso _listaIdTiposServicios.Count > 0 Then _
                            .SqlParametros.Add("@listaTiposServicios", SqlDbType.VarChar).Value = Join(",", _listaIdTiposServicios.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())

                        .IniciarTransaccion()
                        .EjecutarNonQuery("RegistrarCallCenter", CommandType.StoredProcedure)
                        Dim respuesta As Integer = .SqlParametros("@respuesta").Value
                        If respuesta = 0 Then
                            .ConfirmarTransaccion()
                        Else
                            Select Case respuesta
                                Case 1 : resultado.EstablecerMensajeYValor(respuesta, "El nombre del Call Center ya se encuentra registrado")
                            End Select
                            .AbortarTransaccion()
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
                If _idCallCenter > 0 Then
                    With dbManager
                        .SqlParametros.Add("@idCallCenter", SqlDbType.Int).Value = _idCallCenter
                        If Not String.IsNullOrEmpty(_nombre) Then .SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = _nombre
                        .SqlParametros.Add("@nombreContacto", SqlDbType.VarChar).Value = _nombreContacto
                        .SqlParametros.Add("@telefonoContacto", SqlDbType.VarChar).Value = _telefonoContacto
                        .SqlParametros.Add("@activo", SqlDbType.VarChar).Value = _activo
                        .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        If _listaIdTiposServicios IsNot Nothing AndAlso _listaIdTiposServicios.Count > 0 Then _
                            .SqlParametros.Add("@listaTiposServicios", SqlDbType.VarChar).Value = Join(",", _listaIdTiposServicios.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())

                        .IniciarTransaccion()
                        .EjecutarNonQuery("ActualizarCallCenter", CommandType.StoredProcedure)
                        Dim respuesta As Integer = .SqlParametros("@respuesta").Value
                        If respuesta = 0 Then
                            .ConfirmarTransaccion()
                        Else
                            .AbortarTransaccion()
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
