Imports ILSBusinessLayer
Imports LMDataAccessLayer

Public Class MaterialEquipoClaseSIM

#Region "Atributos (Campos)"

    Private _idMaterial As Integer
    Private _idClase As Integer
    Private _idRegion As Integer
    Private _nombreRegion As String
    Private _nombreClase As String
    Private _material As String
    Private _descripcionMaterial As String
    Private _precioNormal As Double
    Private _precioBlanco As Double
    Private _iva As Single

    Private _registrado As Boolean

#End Region

#Region "Construtores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idMaterial As Integer)
        _idMaterial = idMaterial
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Public Property idRegion As String
        Get
            Return _idRegion
        End Get
        Set(value As String)
            _idRegion = value
        End Set
    End Property

    Public Property nombreRegion As String
        Get
            Return _nombreRegion
        End Get
        Set(value As String)
            _nombreRegion = value
        End Set
    End Property

    Public Property IdMaterial As Integer
        Get
            Return _idMaterial
        End Get
        Set(value As Integer)
            _idMaterial = value
        End Set
    End Property

    Public Property IdClase As Integer
        Get
            Return _idClase
        End Get
        Set(value As Integer)
            _idClase = value
        End Set
    End Property

    Public Property NombreClase As String
        Get
            Return _nombreClase
        End Get
        Set(value As String)
            _nombreClase = value
        End Set
    End Property

    Public Property Material As String
        Get
            Return _material
        End Get
        Set(value As String)
            _material = value
        End Set
    End Property

    Public Property DescripcionMaterial As String
        Get
            Return _descripcionMaterial
        End Get
        Set(value As String)
            _descripcionMaterial = value
        End Set
    End Property

    Public Property PrecioNormal As Double
        Get
            Return _precioNormal
        End Get
        Set(value As Double)
            _precioNormal = value
        End Set
    End Property

    Public Property PrecioBlanco As Double
        Get
            Return _precioBlanco
        End Get
        Set(value As Double)
            _precioBlanco = value
        End Set
    End Property

    Public Property Iva As Single
        Get
            Return _iva
        End Get
        Set(value As Single)
            _iva = value
        End Set
    End Property


    Public Property Registrado As Boolean
        Get
            Return _registrado
        End Get
        Set(value As Boolean)
            _registrado = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If _idMaterial > 0 Then .SqlParametros.Add("@idListaMaterial", SqlDbType.Int).Value = CStr(_idMaterial)
                .ejecutarReader("ConsultaItemMaterialEquipoClaseSIM", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    If .Reader.Read Then CargarResultadoConsulta(.Reader)
                    If Not .Reader.IsClosed Then .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function Crear() As ResultadoProceso
        Dim resultado As New ResultadoProceso

        Return resultado
    End Function

    Public Function Modificar() As ResultadoProceso
        Dim resultado As New ResultadoProceso

        Return resultado
    End Function

    Public Function Eliminar() As ResultadoProceso
        Dim resultado As New ResultadoProceso

        Return resultado
    End Function

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                    If reader("idMaterial").ToString <> 0 Then
                        Integer.TryParse(reader("idMaterial").ToString, _idMaterial)
                        Integer.TryParse(reader("idClase").ToString, _idClase)
                        If Not IsDBNull(reader("NombreRegion")) Then _nombreRegion = reader("NombreRegion").ToString
                        Integer.TryParse(reader("IdRegion").ToString, _idRegion)
                        If Not IsDBNull(reader("nombreClase")) Then _nombreClase = reader("nombreClase").ToString
                        If Not IsDBNull(reader("material")) Then _material = reader("material").ToString
                        If Not IsDBNull(reader("descripcionMaterial")) Then _descripcionMaterial = reader("descripcionMaterial").ToString
                        If Not IsDBNull(reader("precioNormal")) Then _precioNormal = reader("precioNormal")
                        If Not IsDBNull(reader("precioBlanco")) Then _precioBlanco = reader("precioBlanco")
                        If Not IsDBNull(reader("iva")) Then _iva = reader("iva")
                        _registrado = True
                    Else
                        If Not IsDBNull(reader("NombreRegion")) Then _nombreRegion = reader("NombreRegion").ToString
                        Integer.TryParse(reader("IdRegion").ToString, _idRegion)
                    End If
            End If
        End If
    End Sub

#End Region

End Class
