Imports System.Text
Imports LMDataAccessLayer
Imports System.String

Namespace MensajeriaEspecializada

    ''' <summary>
    ''' Author: Beltrán, Diego
    ''' Create date: 03/09/2014
    ''' Description: Clase diseñada para administrar los registros de la tabla RelacionUsuarioCadenaWEB
    ''' </summary>
    ''' <remarks></remarks>
    Public Class RelacionUsuarioCadenaWEB

#Region "Atributos"

        Private _idUsuario As Integer
        Private _usuario As String
        Private _idClienteCem As String

        Private _listIdUsuario As List(Of Integer)

        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        ''' <summary>
        ''' Define o establece el identificador del usuario registrado o consultado
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
        ''' Define o establece el nombre del usuario registrado o consultado
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
        ''' Define o establece el identificador del cliente CEM al cual pertenece el usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdClienteCem As String
            Get
                Return _idClienteCem
            End Get
            Set(value As String)
                _idClienteCem = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece la lista de identificadores de usuarios relacionados al cliente CEM
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

#End Region

#Region "Construtores"

        Public Sub New()
            MyBase.New()
        End Sub

        ''' <summary>
        ''' Constructor que sobrecarga la clase con los datos del idUsuario proporcionado
        ''' </summary>
        ''' <param name="idUsuario"> de tipo <see langword="Integer"/> que contiene la información correspondiente al identificador del Usuario. </param>
        ''' <remarks>
        ''' Su forma de instanciamiento se debe realizar de la siguiente manera:
        ''' Dim miClase As New  MensajeriaEspecializada.RelacionUsuarioCadenaWEB(idUsuario:= idUsuario)
        ''' </remarks>
        Public Sub New(ByVal idUsuario As Integer)
            MyBase.New()
            _idUsuario = idUsuario
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        ''' <summary>
        ''' función que realiza la inicialización de la carga de los atributos de la clase, según los parametros establecidos
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@listaIdUsuario", SqlDbType.VarChar, 30).Value = CStr(_idUsuario)
                    .ejecutarReader("ObtenerInfoRelacionUsuarioCadenaWEB", CommandType.StoredProcedure)

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
                        .Add("@idClienteCem", SqlDbType.Int).Value = _idClienteCem
                        If _listIdUsuario IsNot Nothing AndAlso _listIdUsuario.Count > 0 Then _
                            .Add("@listIdUsuario", SqlDbType.VarChar).Value = Join(",", _listIdUsuario.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                        .Add("@mensaje", SqlDbType.VarChar, 3000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .IniciarTransaccion()
                    .EjecutarNonQuery("CrearRelacionUsuarioCadenaWEB", CommandType.StoredProcedure)

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
                resultado.EstablecerMensajeYValor(500, "Ocurrio un error al realizar el registro: " & ex.Message)
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
                    Integer.TryParse(reader("idUsuario"), _idUsuario)
                    If Not IsDBNull(reader("usuario")) Then _usuario = CStr(reader("usuario"))
                    Integer.TryParse(reader("idClienteCem"), _idClienteCem)
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace