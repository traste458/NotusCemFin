Imports LMDataAccessLayer
Imports System.String

Public Class UnidadNegocio

#Region "Atributos"

    Private _idUnidadNegocio As Short
    Private _idClienteExterno As Integer
    Private _nombre As String
    Private _activo As Nullable(Of Boolean)
    Private _codigo As String
    Private _idTipoUnidadNegocio As Integer
    Private _tipoUnidadNegocio As String

#End Region

#Region "Propiedades"

    Public Property IdUnidadNegocio As Short
        Get
            Return _idUnidadNegocio
        End Get
        Set(value As Short)
            _idUnidadNegocio = value
        End Set
    End Property

    Public Property IdClienteExterno As Integer
        Get
            Return _idClienteExterno
        End Get
        Set(value As Integer)
            _idClienteExterno = value
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

    Public Property Codigo As String
        Get
            Return _codigo
        End Get
        Set(value As String)
            _codigo = value
        End Set
    End Property

    Public Property IdTipoUnidadNegocio As Integer
        Get
            Return _idTipoUnidadNegocio
        End Get
        Set(value As Integer)
            _idTipoUnidadNegocio = value
        End Set
    End Property

    Public Property TipoUnidadNegocio As String
        Get
            Return _tipoUnidadNegocio
        End Get
        Set(value As String)
            _tipoUnidadNegocio = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idUnidadNegocio As Short)
        MyBase.New()
        _idUnidadNegocio = idUnidadNegocio
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idUnidadNegocio > 0 Then .SqlParametros.Add("@idUnidadNegocio", SqlDbType.SmallInt).Value = _idUnidadNegocio
                    If _idClienteExterno > 0 Then .SqlParametros.Add("@idClienteExterno", SqlDbType.Int).Value = _idClienteExterno
                    If _activo IsNot Nothing Then .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo

                    .ejecutarReader("ObtenerHorariosVentas", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idUnidadNegocio").ToString, _idUnidadNegocio)
                            Integer.TryParse(.Reader("idClienteExterno").ToString, _idClienteExterno)
                            _nombre = .Reader("nombre")
                            _activo = .Reader("activo")
                            _codigo = .Reader("codigo")
                            If Not IsDBNull(.Reader("idTipoUnidadNegocio")) Then Integer.TryParse(.Reader("idTipoUnidadNegocio"), _idTipoUnidadNegocio)
                            If Not IsDBNull(.Reader("tipoUnidadNegocio")) Then _tipoUnidadNegocio = .Reader("tipoUnidadNegocio")
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

    Public Function Registrar(ByVal idUsuario As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@idClienteExterno", SqlDbType.Int).Value = _idClienteExterno
                    .Add("@nombre", SqlDbType.VarChar, 2000).Value = _nombre
                    If Not String.IsNullOrEmpty(_codigo) Then .Add("@codigo", SqlDbType.VarChar, 2000).Value = _codigo
                    .Add("@idTipoUnidadNegocio", SqlDbType.Int).Value = _idTipoUnidadNegocio
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .IniciarTransaccion()
                .EjecutarNonQuery("RegistrarUnidadNegocio", CommandType.StoredProcedure)

                If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                    If resultado.Valor = 0 Then
                        .ConfirmarTransaccion()
                    Else
                        .AbortarTransaccion()
                    End If
                Else
                    .AbortarTransaccion()
                    resultado.EstablecerMensajeYValor(400, "No se logró establecer la respuesta del servidor.")
                End If
            End With
        Catch ex As Exception
            If dbManager.EstadoTransaccional() Then dbManager.AbortarTransaccion()
            resultado.EstablecerMensajeYValor(500, "Se generó un error al generar el registro:  " & ex.Message)
        End Try
        Return resultado
    End Function

#End Region

End Class
