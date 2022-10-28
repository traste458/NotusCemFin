Imports LMDataAccessLayer
Imports System.Reflection

Public Class Tercero

#Region "Atributos (Campos)"

    Private _idTercero As Decimal
    Private _idTercero2 As String
    Private _tercero As String
    Private _idArea As Decimal
    Private _idCargo As Decimal
    Private _idCiudad As Decimal
    Private _estado As Decimal
    Private _usuario As String
    Private _clave As String
    Private _linea As Decimal
    Private _idCliente As Decimal
    Private _idPos As Decimal
    Private _idSucursal As Decimal
    Private _idBodega As Decimal
    Private _idCentro_costo As Decimal
    Private _idEmpresa_temporal As Decimal
    Private _fecha As DateTime
    Private _idCreador As Decimal
    Private _clave2 As String
    Private _telefono As String
    Private _idPerfil As Decimal
    Private _idCac As Decimal
    Private _email As String
    Private _fechaContratacion As DateTime
    Private _idClasificacionHorario As Integer
    Private _codigoHuellaUsuario As String

#End Region

#Region "Propiedades"


    Public Property IdTercero() As Decimal

        Get
            Return _idTercero
        End Get
        Set(ByVal value As Decimal)
            _idTercero = value
        End Set
    End Property

    Public Property IdTercero2() As String

        Get
            Return _idTercero2
        End Get
        Set(ByVal value As String)
            _idTercero2 = value
        End Set
    End Property

    Public Property Tercero() As String

        Get
            Return _tercero
        End Get
        Set(ByVal value As String)
            _tercero = value
        End Set
    End Property

    Public Property IdArea() As Decimal

        Get
            Return _idArea
        End Get
        Set(ByVal value As Decimal)
            _idArea = value
        End Set
    End Property

    Public Property IdCargo() As Decimal

        Get
            Return _idCargo
        End Get
        Set(ByVal value As Decimal)
            _idCargo = value
        End Set
    End Property

    Public Property IdCiudad() As Decimal

        Get
            Return _idCiudad
        End Get
        Set(ByVal value As Decimal)
            _idCiudad = value
        End Set
    End Property

    Public Property Estado() As Decimal

        Get
            Return _estado
        End Get
        Set(ByVal value As Decimal)
            _estado = value
        End Set
    End Property

    Public Property Usuario() As String

        Get
            Return _usuario
        End Get
        Set(ByVal value As String)
            _usuario = value
        End Set
    End Property

    Public Property Clave() As String

        Get
            Return _clave
        End Get
        Set(ByVal value As String)
            _clave = value
        End Set
    End Property

    Public Property Linea() As Decimal

        Get
            Return _linea
        End Get
        Set(ByVal value As Decimal)
            _linea = value
        End Set
    End Property

    Public Property IdCliente() As Decimal

        Get
            Return _idCliente
        End Get
        Set(ByVal value As Decimal)
            _idCliente = value
        End Set
    End Property

    Public Property IdPos() As Decimal

        Get
            Return _idPos
        End Get
        Set(ByVal value As Decimal)
            _idPos = value
        End Set
    End Property

    Public Property IdSucursal() As Decimal

        Get
            Return _idSucursal
        End Get
        Set(ByVal value As Decimal)
            _idSucursal = value
        End Set
    End Property

    Public Property IdBodega() As Decimal

        Get
            Return _idBodega
        End Get
        Set(ByVal value As Decimal)
            _idBodega = value
        End Set
    End Property

    Public Property IdCentro_costo() As Decimal

        Get
            Return _idCentro_costo
        End Get
        Set(ByVal value As Decimal)
            _idCentro_costo = value
        End Set
    End Property

    Public Property IdEmpresa_temporal() As Decimal

        Get
            Return _idEmpresa_temporal
        End Get
        Set(ByVal value As Decimal)
            _idEmpresa_temporal = value
        End Set
    End Property

    Public Property Fecha() As DateTime

        Get
            Return _fecha
        End Get
        Set(ByVal value As DateTime)
            _fecha = value
        End Set
    End Property

    Public Property IdCreador() As Decimal

        Get
            Return _idCreador
        End Get
        Set(ByVal value As Decimal)
            _idCreador = value
        End Set
    End Property

    Public Property Clave2() As String

        Get
            Return _clave2
        End Get
        Set(ByVal value As String)
            _clave2 = value
        End Set
    End Property

    Public Property Telefono() As String

        Get
            Return _telefono
        End Get
        Set(ByVal value As String)
            _telefono = value
        End Set
    End Property

    Public Property IdPerfil() As Decimal

        Get
            Return _idPerfil
        End Get
        Set(ByVal value As Decimal)
            _idPerfil = value
        End Set
    End Property

    Public Property IdCac() As Decimal

        Get
            Return _idCac
        End Get
        Set(ByVal value As Decimal)
            _idCac = value
        End Set
    End Property

    Public Property Email() As String

        Get
            Return _email
        End Get
        Set(ByVal value As String)
            _email = value
        End Set
    End Property

    Public Property FechaContratacion() As DateTime

        Get
            Return _fechaContratacion
        End Get
        Set(ByVal value As DateTime)
            _fechaContratacion = value
        End Set
    End Property

    Public Property IdClasificacionHorario() As Integer

        Get
            Return _idClasificacionHorario
        End Get
        Set(ByVal value As Integer)
            _idClasificacionHorario = value
        End Set
    End Property

    Public Property CodigoHuellaUsuario() As String

        Get
            Return _codigoHuellaUsuario
        End Get
        Set(ByVal value As String)
            _codigoHuellaUsuario = value
        End Set
    End Property

#End Region

#Region "Constructores"
    Public Sub New()
        MyBase.New()
        _idTercero2 = ""
        _tercero = ""
        _usuario = ""
        _clave = ""
        _clave2 = ""
        _telefono = ""
        _email = ""
        _codigoHuellaUsuario = ""
    End Sub

    Public Sub New(ByVal IdTercero As Decimal)
        MyBase.New()
        _idTercero = IdTercero
        CargarDatos()
    End Sub
#End Region

#Region "Métodos Privados"
    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If _idTercero > 0 Then .SqlParametros.Add("@idTercero", SqlDbType.Int).Value = _idTercero
                .ejecutarReader("ObtenerTerceros", CommandType.StoredProcedure)

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

#Region "Métodos Protegidos"
    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idTercero").ToString(), _idTercero)
                '_idTercero2 = reader("idTercero2").ToString()
                _tercero = reader("tercero").ToString()
                '_usuario = reader("usuario").ToString()
                '_clave = reader("clave").ToString()
                'If Not IsDBNull(reader("fecha")) Then _fecha = CDate(reader("fecha"))
                '_clave2 = reader("clave2").ToString()
                '_telefono = reader("telefono").ToString()
                '_email = reader("email").ToString()
                'If Not IsDBNull(reader("fechaContratacion")) Then _fechaContratacion = CDate(reader("fechaContratacion"))
                'Integer.TryParse(reader("idClasificacionHorario").ToString(), _idClasificacionHorario)
                '_codigoHuellaUsuario = reader("codigoHuellaUsuario").ToString()
            End If
        End If
    End Sub
#End Region

End Class
