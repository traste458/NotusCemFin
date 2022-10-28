Imports LMDataAccessLayer

Public Class SoporteSimLockVerificacionFactura

#Region "Atributos"
    Private _idSoporte As Integer
    Private _idVerificacion As Integer
    Private _nombreOriginal As String
    Private _rutaCompleta As String
    Private _datosBinarios As Byte()
    Private _idTipoSoporte As Byte
    Private _contentType As String
    Private _fechaRegistro As Date
    Private _idUsuarioRegistro As Integer
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
    End Sub

    Public Sub New(ByVal identificador As Integer)
        _idVerificacion = identificador
        CargarInformacion()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdSoporte As Integer        Get            Return _idSoporte        End Get        Set(ByVal value As Integer)            _idSoporte = value        End Set    End Property
    Public Property IdVerificacion As Integer        Get            Return _idVerificacion        End Get        Set(ByVal value As Integer)            _idVerificacion = value        End Set    End Property
    Public Property NombreOriginal As String        Get            Return _nombreOriginal        End Get        Set(ByVal value As String)            _nombreOriginal = value        End Set    End Property
    Public Property RutaCompleta As String        Get            Return _rutaCompleta        End Get        Set(ByVal value As String)            _rutaCompleta = value        End Set    End Property
    Public Property DatosBinarios As Byte()        Get            Return _datosBinarios        End Get        Set(ByVal value As Byte())            _datosBinarios = value        End Set    End Property    Public Property ContentType As String        Get
            Return _contentType
        End Get
        Set(value As String)
            _contentType = value
        End Set
    End Property
    Public Property IdTipoSoporte As Byte        Get            Return _idTipoSoporte        End Get        Set(ByVal value As Byte)            _idTipoSoporte = value        End Set    End Property
    Public Property FechaRegistro As Date        Get            Return _fechaRegistro        End Get        Set(ByVal value As Date)            _fechaRegistro = value        End Set    End Property
    Public Property IdUsuarioRegistro As Integer        Get            Return _idUsuarioRegistro        End Get        Set(ByVal value As Integer)            _idUsuarioRegistro = value        End Set    End Property

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

    Private Sub CargarInformacion()
        If _idVerificacion > 0 Then
            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Add("@idVerificacion", SqlDbType.Int).Value = _idVerificacion
                    .ejecutarReader("ObtenerInformacionDeSoporteDeVerificacionDeFacturas", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            CargarValorDePropiedades(.Reader)
                        End If
                        .Reader.Close()
                    End If
                End With
            End Using
        End If
    End Sub

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarValorDePropiedades(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing AndAlso reader.HasRows Then
            Integer.TryParse(reader("idSoporte").ToString, _idSoporte)
            Integer.TryParse(reader("idVerificacion").ToString, _idVerificacion)
            _nombreOriginal = reader("nombreOriginal").ToString
            _rutaCompleta = reader("rutaCompleta").ToString
            If Not IsDBNull(reader("datosBinarios")) Then _datosBinarios = reader("datosBinarios")
            _contentType = reader("contentType").ToString
            Byte.TryParse(reader("idTipoSoporte").ToString, _idTipoSoporte)
            Date.TryParse(reader("fechaRegistro").ToString, _fechaRegistro)
            Integer.TryParse(reader("idUsuarioRegistro").ToString, _idUsuarioRegistro)
            _registrado = True
        End If
    End Sub

#End Region

End Class
