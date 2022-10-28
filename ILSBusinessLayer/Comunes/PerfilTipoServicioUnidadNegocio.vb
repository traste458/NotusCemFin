Imports System.Text
Imports LMDataAccessLayer
Imports System.String

''' <summary>
''' Author: Beltrán, Diego
''' Create date: 12/08/2014
''' Description: Clase diseñada para el manejo y administración de los datos almacenados en la tabla PerfilTipoServicioUnidadNegocio
''' </summary>
''' <remarks></remarks>
Public Class PerfilTipoServicioUnidadNegocio

#Region "Atributos"

    Private _idTipoServicioNegocio As Integer
    Private _idPerfil As Integer
    Private _perfil As String

    Private _listIdPerfil As List(Of Integer)

    Private _registrado As Boolean

#End Region

#Region "Propiedades"

    ''' <summary>
    ''' Define o establece el identificador idTipoServicioNegocio de la tabla PerfilTipoServicioUnidadNegocio, relacionado a la tabla TipoServicioUnidadNegocio
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdTipoServicioNegocio As Integer
        Get
            Return _idTipoServicioNegocio
        End Get
        Set(value As Integer)
            _idTipoServicioNegocio = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el identificador del perfil asociado al idTipoServicioNegocio
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdPerfil As Integer
        Get
            Return _idPerfil
        End Get
        Set(value As Integer)
            _idPerfil = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el nombre del perfil asociado al idTipoServicioNegocio
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Perfil As String
        Get
            Return _perfil
        End Get
        Set(value As String)
            _perfil = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece la lista de identificadores de perfiles asociados a un tipo de servicio
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ListIdPerfil As List(Of Integer)
        Get
            If _listIdPerfil Is Nothing Then _listIdPerfil = New List(Of Integer)
            Return _listIdPerfil
        End Get
        Set(value As List(Of Integer))
            _listIdPerfil = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary>
    ''' Constructor que sobrecarga la clase con los datos del idTipoServicioNegocio proporcionado
    ''' </summary>
    ''' <param name="idTipoServicioNegocio">de tipo <see langword="Integer"/> que contiene la información correspondiente al identificador de la tabla TipoServicioUnidadNegocio. 
    ''' </param>
    ''' <remarks>
    ''' Su forma de instanciamiento se debe realizar de la siguiente manera:
    ''' Dim miClase As New  Comunes.PerfilTipoServicioUnidadNegocio(idTipoServicioNegocio:= idTipoServicioNegocio)
    ''' </remarks>
    Public Sub New(ByVal idTipoServicioNegocio As Integer)
        MyBase.New()
        _idTipoServicioNegocio = idTipoServicioNegocio
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    ''' <summary>
    ''' Función que realiza la inicialización de la carga de los atributos de la clase, según los parametros establecidos
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Add("@idTipoServicioNegocio", SqlDbType.VarChar, 30).Value = CStr(_idTipoServicioNegocio)
                .ejecutarReader("ObtenerInfoPerfilTipoServicioUnidadNegocio", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    If .Reader.Read Then
                        CargarResultadoConsulta(.Reader)
                        _registrado = True
                    End If
                    .Reader.Close()
                End If

            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

#Region "Métodos Públicos"

    ''' <summary>
    ''' Función que permite realizar el registro en la tabla
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@idTipoServicioNegocio", SqlDbType.Int).Value = _idTipoServicioNegocio
                    If _listIdPerfil IsNot Nothing AndAlso _listIdPerfil.Count > 0 Then _
                        .Add("@listIdPerfil", SqlDbType.VarChar).Value = Join(",", _listIdPerfil.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .IniciarTransaccion()
                .EjecutarNonQuery("RegistrarPerfilTipoServicioUnidadNegocio", CommandType.StoredProcedure)

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
                    resultado.EstablecerMensajeYValor(400, "No se logró establecer la respuesta del servidor, por favor intentelo nuevamente. ")
                End If
            End With
        Catch ex As Exception
            If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            resultado.EstablecerMensajeYValor(500, "Se presentó un error al realizar el registro: " & ex.Message)
        End Try
        Return resultado
    End Function

#End Region

#Region "Métodos Protegidos"

    ''' <summary>
    ''' Método encargado de sobrecargar los atributos de la clase 
    ''' </summary>
    ''' <param name="reader"> de tipo <see langword="Data.Common.DbDataReader"/> que contiene un objeto de tipo reader, 
    ''' para realizar la lectura y asignación de valores a los atributos de la clase</param>
    ''' <remarks></remarks>
    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idTipoServicioNegocio"), _idTipoServicioNegocio)
                Integer.TryParse(reader("idPerfil"), _idPerfil)
                If Not String.IsNullOrEmpty(reader("perfil")) Then _perfil = reader("perfil").ToString
            End If
        End If
    End Sub

#End Region

End Class
