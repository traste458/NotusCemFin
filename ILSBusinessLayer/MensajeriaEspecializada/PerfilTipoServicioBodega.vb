Imports System.Text
Imports LMDataAccessLayer
Imports System.String

''' <summary>
''' Author: Beltrán, Diego
''' Create date: 02/02/2015
''' Description: Clase diseñada para el manejo y administración de los perfiles asociados a una bodega en dependencia de su unidad de negocio
''' </summary>
''' <remarks></remarks>
Public Class PerfilTipoServicioBodega

#Region "Atributos"

    Private _idUnidadNegocio As Integer
    Private _unidadNegocio As String
    Private _idBodega As Integer
    Private _bodega As String
    Private _idPerfil As Integer
    Private _perfil As String

    Private _registrado As Boolean

#End Region

#Region "Propiedades"

    ''' <summary>
    ''' Define o establece el identificador de la unidad de negocio asociado a la bodega
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdUnidadNegocio As Integer
        Get
            Return _idUnidadNegocio
        End Get
        Set(value As Integer)
            _idUnidadNegocio = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el nombre Unidad Negocio asociado a la bodega
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UnidadNegocio As String
        Get
            Return _unidadNegocio
        End Get
        Set(value As String)
            _unidadNegocio = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el identificador de la bodega
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

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary>
    ''' Constructor que sobrecarga la clase con los datos del idBodega proporcionado
    ''' </summary>
    ''' <param name="idBodega">de tipo <see langword="Integer"/> que contiene la información correspondiente al identificador de la Bodega. 
    ''' </param>
    ''' <remarks>
    ''' Su forma de instanciamiento se debe realizar de la siguiente manera:
    ''' Dim miClase As New  MensajeriaEspecializada.PerfilTipoServicioBodega(idBodega:= idBodega)
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
                .SqlParametros.Add("@idBodega", SqlDbType.VarChar, 30).Value = _idBodega
                .ejecutarReader("ObtenerInfoPerfilBodega", CommandType.StoredProcedure)

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
                Integer.TryParse(reader("idUnidadNegocio"), _idUnidadNegocio)
                If Not String.IsNullOrEmpty(reader("unidadNegocio")) Then _unidadNegocio = reader("unidadNegocio").ToString
                Integer.TryParse(reader("idBodega"), _idBodega)
                If Not String.IsNullOrEmpty(reader("bodega")) Then _bodega = reader("bodega").ToString
                Integer.TryParse(reader("idPerfil"), _idPerfil)
                If Not String.IsNullOrEmpty(reader("perfil")) Then _perfil = reader("perfil").ToString
            End If
        End If
    End Sub

#End Region

End Class
