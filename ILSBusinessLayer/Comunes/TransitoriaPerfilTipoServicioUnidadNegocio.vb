Imports System.Text
Imports LMDataAccessLayer
Imports System.String

''' <summary>
''' Author: Beltrán, Diego
''' Create date: 15/08/2014
''' Description: Clase diseñada para el manejo y administración de los datos almacenados en la tabla TransitoriaPerfilTipoServicioUnidadNegocio
''' </summary>
''' <remarks></remarks>
Public Class TransitoriaPerfilTipoServicioUnidadNegocio

#Region "Atributos"

    Private _idRegistro As Integer
    Private _idPerfil As Integer
    Private _perfil As String

    Private _registrado As Boolean

#End Region

#Region "Propiedades"

    ''' <summary>
    ''' Define o establece el identificador de la tabla TransitoriaPerfilTipoServicioUnidadNegocio
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
    ''' Define o establece el identificador del perfil asociado a la tabla
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

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary>
    ''' Constructor que sobrecarga la clase con los datos del idRegistro proporcionado
    ''' </summary>
    ''' <param name="idRegistro">de tipo <see langword="Integer"/> que contiene la información correspondiente al identificador de la tabla TipoServicioUnidadNegocio. 
    ''' </param>
    ''' <remarks>
    ''' Su forma de instanciamiento se debe realizar de la siguiente manera:
    ''' Dim miClase As New  Comunes.TransitoriaPerfilTipoServicioUnidadNegocio(idRegistro:= idRegistro)
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
                .SqlParametros.Add("@idRegistro", SqlDbType.VarChar, 30).Value = CStr(_idRegistro)
                .ejecutarReader("ObtenerInfoTransitoriaPerfilTipoServicioUnidadNegocio", CommandType.StoredProcedure)

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
                Integer.TryParse(reader("idPerfil"), _idPerfil)
                If Not String.IsNullOrEmpty(reader("perfil")) Then _perfil = reader("perfil").ToString
            End If
        End If
    End Sub

#End Region

End Class
