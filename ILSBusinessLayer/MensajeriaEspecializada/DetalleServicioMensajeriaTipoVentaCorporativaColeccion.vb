Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Public Class DetalleServicioMensajeriaTipoVentaCorporativaColeccion
    Inherits CollectionBase

#Region "Atributos"

    Private _idServicioMensajeria As Integer
    Private _cargado As Boolean

    Private _msisdn As String
    Private _regional As String
    Private _materialEquipo As String
    Private _descripcionMaterialEq As String
    Private _imei As String
    Private _materialSIM As String
    Private _descripcionMaterialSIM As String
    Private _iccid As String
    Private _precioUnitario As String
    Private _precioEspecial As String
    Private _idTipoProducto As Integer
    Private _valorEquipo As String
    Private _codigoCuenta As String

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idServicio As Integer)
        Me.New()
        _idServicioMensajeria = idServicio
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdServicioMensajeria As Integer
        Get
            Return _idServicioMensajeria
        End Get
        Set(ByVal value As Integer)
            _idServicioMensajeria = value
        End Set
    End Property

    Public Property Msisdn As String
        Get
            Return _msisdn
        End Get
        Set(ByVal value As String)
            _msisdn = value
        End Set
    End Property

    Public Property Regional As String
        Get
            Return _regional
        End Get
        Set(ByVal value As String)
            _regional = value
        End Set
    End Property

    Public Property PrecioUnitario As String
        Get
            Return _precioUnitario
        End Get
        Set(ByVal value As String)
            _precioUnitario = value
        End Set
    End Property

    Public Property MaterialEquipo As String
        Get
            Return _materialEquipo
        End Get
        Set(ByVal value As String)
            _materialEquipo = value
        End Set
    End Property

    Public Property DescripcionMaterialEq As String
        Get
            Return _descripcionMaterialEq
        End Get
        Set(ByVal value As String)
            _descripcionMaterialEq = value
        End Set
    End Property

    Public Property Imei As String
        Get
            Return _imei
        End Get
        Set(ByVal value As String)
            _imei = value
        End Set
    End Property

    Public Property MaterialSIM As String
        Get
            Return _materialSIM
        End Get
        Set(ByVal value As String)
            _materialSIM = value
        End Set
    End Property

    Public Property DescripcionMaterialSIM As String
        Get
            Return _descripcionMaterialSIM
        End Get
        Set(ByVal value As String)
            _descripcionMaterialSIM = value
        End Set
    End Property

    Public Property Iccid As String
        Get
            Return _iccid
        End Get
        Set(ByVal value As String)
            _iccid = value
        End Set
    End Property

    Public Property PrecioEspecial As String
        Get
            Return _precioEspecial
        End Get
        Set(ByVal value As String)
            _precioEspecial = value
        End Set
    End Property

    Public Property IdTipoProducto As Integer
        Get
            Return _idTipoProducto
        End Get
        Set(ByVal value As Integer)
            _idTipoProducto = value
        End Set
    End Property

    Public Property ValorEquipo As String
        Get
            Return _valorEquipo
        End Get
        Set(value As String)
            _valorEquipo = value
        End Set
    End Property

    Public Property CodigoCuenta As String
        Get
            Return _codigoCuenta
        End Get
        Set(value As String)
            _codigoCuenta = value
        End Set
    End Property


#End Region

#Region "Métodos Públicos"

    Public Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            Me.Clear()
            With dbManager
                If Me._idServicioMensajeria > 0 Then .SqlParametros.Add("@idServicioMensajeria", SqlDbType.Int).Value = Me._idServicioMensajeria

                .ejecutarReader("ObtenerDetalleServicioMensajeriaTipoVentaCorporativa", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim elDetalle As DetalleServicioMensajeriaTipoVentaCorporativaColeccion

                    While .Reader.Read
                        elDetalle = New DetalleServicioMensajeriaTipoVentaCorporativaColeccion
                        elDetalle.CargarResultadoConsulta(.Reader)
                        _cargado = True
                        Me.InnerList.Add(elDetalle)
                    End While
                    .Reader.Close()
                End If
            End With
            _cargado = True
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                _msisdn = reader("Msisdn").ToString()
                _regional = reader("Regional").ToString()
                _materialEquipo = reader("MaterialEquipo").ToString()
                _descripcionMaterialEq = reader("DescripcionMaterialEq").ToString()
                _imei = reader("Imei").ToString()
                _materialSIM = reader("MaterialSIM").ToString()
                _descripcionMaterialSIM = reader("DescripcionMaterialSIM").ToString()
                _iccid = reader("Iccid").ToString()
                _precioUnitario = reader("PrecioUnitario").ToString()
                _precioEspecial = reader("PrecioEspecial").ToString()
                If Not IsDBNull(reader("valorEquipo")) Then _valorEquipo = reader("valorEquipo")
                If Not IsDBNull(reader("codigoCuenta")) Then _codigoCuenta = reader("codigoCuenta")
                _cargado = True
            End If
        End If

    End Sub

#End Region

End Class
