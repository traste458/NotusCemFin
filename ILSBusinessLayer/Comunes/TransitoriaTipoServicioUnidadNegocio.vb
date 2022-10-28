Imports System.Text
Imports LMDataAccessLayer
Imports System.String

''' <summary>
''' Author: Beltrán, Diego
''' Create date: 14/08/2014
''' Description: Clase diseñada para el manejo y administración de los datos almacenados en la tabla TransitoriaTipoServicioUnidadNegocio
''' </summary>
''' <remarks></remarks>
Public Class TransitoriaTipoServicioUnidadNegocio

#Region "Atributos"

    Private _idRegistro As Integer
    Private _idTipoServicio As Integer
    Private _tipoServicio As String
    Private _idUsuario As Integer

    Private _listPerfil As List(Of Integer)

    Private _registrado As Boolean

#End Region

#Region "Propiedades"

    ''' <summary>
    ''' Define o establece el identificador de la tabla TransitoriaTipoServicioUnidadNegocio
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdRegistro As Integer
        Get
            Return _idRegistro
        End Get
        Set(value As Integer)
            _idRegistro = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el identificador del tipo de servicio
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdTipoServicio As Integer
        Get
            Return _idTipoServicio
        End Get
        Set(value As Integer)
            _idTipoServicio = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el nombre del tipo de servicio
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TipoServicio As String
        Get
            Return _tipoServicio
        End Get
        Set(value As String)
            _tipoServicio = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el identificador del usuario que esta realizando el registro
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
    ''' Define o establece la lista de identificadores de perfiles asociados al tipo de servicio
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ListIdPerfil As List(Of Integer)
        Get
            If _listPerfil Is Nothing Then _listPerfil = New List(Of Integer)
            Return _listPerfil
        End Get
        Set(value As List(Of Integer))
            _listPerfil = value
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
    ''' <param name="idRegistro">de tipo <see langword="Integer"/> que contiene la información correspondiente al identificador de la tabla. </param>
    ''' <remarks>
    ''' Su forma de instanciamiento se debe realizar de la siguiente manera:
    ''' Dim miClase As New  Comunes.TransitoriaTipoServicioUnidadNegocio(idRegistro:= idRegistro)
    ''' </remarks>
    Public Sub New(ByVal idRegistro As Integer)
        MyBase.New()
        _idRegistro = idRegistro
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
                .SqlParametros.Add("@listIdRegistro", SqlDbType.VarChar, 30).Value = CStr(_idRegistro)
                .ejecutarReader("ObtenerInfoTransitoriaTipoServicioUnidadNegocio", CommandType.StoredProcedure)

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
    ''' Función que permite registrar un nuevo elemento a la tabla
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Crear() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .Add("@idTipoServicio", SqlDbType.Int).Value = _idTipoServicio
                    If _listPerfil IsNot Nothing AndAlso _listPerfil.Count > 0 Then _
                        .Add("@listPerfil", SqlDbType.VarChar).Value = Join(",", _listPerfil.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                    .Add("@mensaje", SqlDbType.VarChar, 3000).Direction = ParameterDirection.Output
                    .Add("@idRegistro", SqlDbType.Int).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .IniciarTransaccion()
                .EjecutarNonQuery("CrearTransitoriaTipoServicioUnidadNegocio", CommandType.StoredProcedure)

                If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                    _idRegistro = .SqlParametros("@idRegistro").Value
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
            resultado.EstablecerMensajeYValor(500, "Ocurrio un error al realizar el registro: " & ex.Message)
        End Try
        Return resultado
    End Function

    ''' <summary>
    ''' Función que permite eliminar un registro de la tabla
    ''' </summary>
    ''' <param name="idUsuario"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Eliminar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@idRegistro", SqlDbType.Int).Value = _idRegistro
                    .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .IniciarTransaccion()
                .EjecutarNonQuery("EliminarTransitoriaTipoServicioUnidadNegocio", CommandType.StoredProcedure)

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
                    resultado.EstablecerMensajeYValor(400, "No se logró obtener respuesta del servidor, por favor intentelo nuevamente. ")
                End If

            End With
        Catch ex As Exception
            If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            resultado.EstablecerMensajeYValor(500, "Se generó un error al eliminar el registro: " & ex.Message)
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
                Integer.TryParse(reader("idRegistro"), _idRegistro)
                Integer.TryParse(reader("idTipoServicio"), _idTipoServicio)
                If Not String.IsNullOrEmpty(reader("tipoServicio")) Then _tipoServicio = reader("tipoServicio").ToString
                Integer.TryParse(reader("idUsuario"), _idUsuario)
            End If
        End If
    End Sub

#End Region

End Class
