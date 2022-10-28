Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Enumerados

Public Class TipoAlistamiento
#Region "Atributos"
    Private _idTipoAlistamiento As Integer
    Private _nombre As String
    Private _codigo As String
    Private _estado As Boolean
#End Region

#Region "Propiedades"
    Public ReadOnly Property IdTipoAlistamiento() As Integer
        Get
            Return _idTipoAlistamiento
        End Get
    End Property

    Public Property Nombre() As String
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
        End Set
    End Property

    Public Property Codigo() As String
        Get
            Return _codigo
        End Get
        Set(ByVal value As String)
            _codigo = value
        End Set
    End Property

    Public Property Estado() As Boolean
        Get
            Return _estado
        End Get
        Set(ByVal value As Boolean)
            _estado = value
        End Set
    End Property

#End Region


#Region "Constructores"
    Public Sub New()
        _idTipoAlistamiento = 0
        _nombre = ""
        _estado = 0
    End Sub

    Public Sub New(ByVal idTipoAlistamiento As Integer)
        Me.New()
        Me.SeleccionarPorID(idTipoAlistamiento)
    End Sub
#End Region

#Region "Metodos Amigos"

    Public Overloads Function ListadoTipoAlistamiento() As DataTable
        Dim resultado As New DataTable
        Dim adminBD As New LMDataAccessLayer.LMDataAccess
        Try
            resultado = adminBD.ejecutarDataTable("ObtenerTipoAlistamiento", CommandType.StoredProcedure)
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar listado de tipos de alistamiento: " & ex.Message)
        Finally
            adminBD.Dispose()
        End Try

        Return resultado
    End Function

    Public Overloads Function ListadoTipoAlistamiento(ByVal filtro As FiltroTipoAlistamiento) As DataTable
        Dim dt As New DataTable
        Dim dm As New LMDataAccessLayer.LMDataAccess
        Try
            With dm
                If filtro.activo <> EstadoBinario.NoEstablecido Then .agregarParametroSQL("@estado", filtro.activo, SqlDbType.Bit)
                If filtro.idTipoAlistamiento <> 0 Then .agregarParametroSQL("@idTipoMovimientoTransporte", filtro.idTipoAlistamiento, SqlDbType.Int)
                If filtro.codigo IsNot Nothing AndAlso filtro.codigo.Trim.Length <> 0 Then .agregarParametroSQL("@codigo", filtro.codigo, SqlDbType.VarChar, 4)
                dt = .ejecutarDataTable("ObtenerTipoAlistamiento", CommandType.StoredProcedure)
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If dm IsNot Nothing Then dm.Dispose()
        End Try
        Return dt
    End Function

#End Region

#Region "Métodos Privados"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="idTransportadora"></param>
    ''' <remarks></remarks>
    Private Sub SeleccionarPorID(ByVal idTipoAlistamiento As Integer)
        Dim adminBD As New LMDataAccessLayer.LMDataAccess

        adminBD.agregarParametroSQL("@idTipoMovimientoTransporte", idTipoAlistamiento, SqlDbType.Int)
        Try
            adminBD.ejecutarReader("ObtenerTipoAlistamiento", CommandType.StoredProcedure)
            While adminBD.Reader.Read()
                Me._idTipoAlistamiento = CInt(adminBD.Reader("idTipoMovimientoTransporte").ToString)
                Me._nombre = adminBD.Reader("nombre").ToString
                Me._codigo = adminBD.Reader("codigo").ToString
                Me._estado = CInt(adminBD.Reader("activo").ToString)
            End While
        Catch ex As Exception
            Throw New Exception("Imposible obtener tipo de alistamiento con el ID especificado " & idTipoAlistamiento)
        Finally
            adminBD.Dispose()
        End Try
    End Sub
#End Region

End Class
