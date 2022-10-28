Imports System.Text
Imports LMDataAccessLayer
Imports System.String

''' <summary>
''' Author: Beltrán, Diego
''' Create date: 02/02/2014
''' Description: Clase diseñada para el manejo y administración de los datos almacenados en la tabla UsuarioBodega
''' </summary>
''' <remarks></remarks>
Public Class UsuarioBodega

#Region "Atributos"

    Private _idBodega As Integer
    Private _bodega As String
    Private _idUsuario As Integer
    Private _usuario As String
    Private _idPerfil As Integer

    Private _listIdUsuario As List(Of Integer)

    Private _registrado As Boolean

#End Region

#Region "Propiedades"

    ''' <summary>
    ''' Define o establece el identificador idBodega de la tabla UsuarioBodega
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdBodega As Integer
        Get
            Return _idBodega
        End Get
        Set(value As Integer)
            _idBodega = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el nombre de la bodega
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Bodega As String
        Get
            Return _bodega
        End Get
        Set(value As String)
            _bodega = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el identificador del usuario asociado a la bodega
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdUsuario As Integer
        Get
            Return _idUsuario
        End Get
        Set(value As Integer)
            _idUsuario = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el nombre del Usuario asociado al idTipoServicioNegocio
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Usuario As String
        Get
            Return _usuario
        End Get
        Set(value As String)
            _usuario = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece la lista de identificadores de usuarios asociados a una bodega
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ListIdUsuario As List(Of Integer)
        Get
            If _listIdUsuario Is Nothing Then _listIdUsuario = New List(Of Integer)
            Return _listIdUsuario
        End Get
        Set(value As List(Of Integer))
            _listIdUsuario = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el identificador del perfil del usuario asociado a la bodega
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

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary>
    ''' Constructor que sobrecarga la clase con los datos del idBodega proporcionado
    ''' </summary>
    ''' <param name="idBodega">de tipo <see langword="Integer"/> que contiene la información correspondiente al identificador de la tabla UsuarioBodega. 
    ''' </param>
    ''' <remarks>
    ''' Su forma de instanciamiento se debe realizar de la siguiente manera:
    ''' Dim miClase As New  MensajeriaEspecializada.UsuarioBodega(idBodega:= idBodega)
    ''' </remarks>
    Public Sub New(ByVal idBodega As Integer)
        MyBase.New()
        _idBodega = idBodega
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
                .SqlParametros.Add("@listIdBodega", SqlDbType.VarChar, 30).Value = CStr(_idBodega)
                .ejecutarReader("ObtenerInfoUsuarioBodega", CommandType.StoredProcedure)

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
                    .Add("@idBodega", SqlDbType.Int).Value = _idBodega
                    .Add("@idPerfil", SqlDbType.Int).Value = _idPerfil
                    If _listIdUsuario IsNot Nothing AndAlso _listIdUsuario.Count > 0 Then _
                        .Add("@listIdUsuario", SqlDbType.VarChar).Value = Join(",", _listIdUsuario.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .IniciarTransaccion()
                .EjecutarNonQuery("RegistrarUsuarioBodega", CommandType.StoredProcedure)

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
                Integer.TryParse(reader("idBodega"), _idBodega)
                If Not String.IsNullOrEmpty(reader("bodega")) Then _bodega = reader("bodega").ToString
                Integer.TryParse(reader("idtercero"), _idUsuario)
                If Not String.IsNullOrEmpty(reader("tercero")) Then _usuario = reader("tercero").ToString
            End If
        End If
    End Sub

#End Region

End Class
