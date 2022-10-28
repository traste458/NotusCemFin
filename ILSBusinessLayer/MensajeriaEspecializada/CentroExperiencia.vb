Imports LMDataAccessLayer

Public Class CentroExperiencia

#Region "Atributos"
    Private _idCentro As Integer
    Private _nombre As String
    Private _estado As Integer
    Private _idUsuario As Integer
    Private _tipoCentro As String

#End Region

#Region "Propiedades"

    Public Property IdCentro() As Integer
        Get
            Return _idCentro
        End Get
        Set(value As Integer)
            _idCentro = value
        End Set
    End Property

    Public Property Nombre() As String
        Get
            Return _nombre
        End Get
        Set(value As String)
            _nombre = value
        End Set
    End Property

    Public Property Estado() As Integer
        Get
            Return _estado
        End Get
        Set(value As Integer)
            _estado = value
        End Set
    End Property

    Public Property IdUsuario() As Integer
        Get
            Return _idUsuario
        End Get
        Set(value As Integer)
            _idUsuario = value
        End Set
    End Property

    Public Property TipoCentro As String
        Get
            Return _tipoCentro
        End Get
        Set(value As String)
            _tipoCentro = value
        End Set
    End Property

#End Region

#Region "Constructores"
    Public Sub New()
        MyBase.New()
    End Sub
#End Region

#Region "Metodos Publicos"

    Public Function ObtenerBoedegasTraslado() As DataTable
        Dim dtResultado As New DataTable
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Clear()
                dtResultado = .EjecutarDataTable("BuscarBodegas", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtResultado
    End Function

    Public Function ObtenerInformacionProdSerial(serial As String, bodegaOrigen As Integer) As DataTable
        Dim dtResultado As New DataTable
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@serial", SqlDbType.VarChar).Value = serial
                    .Add("@idOrigen", SqlDbType.Int).Value = bodegaOrigen
                End With
                dtResultado = .EjecutarDataTable("CargarInformacionProductoSerial", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtResultado
    End Function

    Public Function ObtenerInformacionTrasladoInventario(guia As String) As DataTable
        Dim dtResultado As New DataTable
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@numeroGuia", SqlDbType.VarChar).Value = guia
                End With
                dtResultado = .EjecutarDataTable("ConsultarTrasladoSerial", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtResultado
    End Function

    Public Function ConfirmarEntregaSeriales(guia As String) As Integer
        Dim result As Integer
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@numeroGuia", SqlDbType.VarChar).Value = guia
                End With
                result = .EjecutarNonQuery("ConfirmarEntregaTrasladoInventario", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return result
    End Function

#End Region

End Class
