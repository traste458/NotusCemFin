Imports ILSBusinessLayer
Imports LMDataAccessLayer

Public Class CiudadesCEM

#Region "Atributos"

    Private _idCiudad As Integer
    Private _ciudad As String
    Private _nombreCiudad As String
    Private _nombreDepartamento As String
    Private _idBodega As Integer

    Private _registrado As Boolean

#End Region

#Region "Propiedades"

    Public Property IdCiudad As Integer
        Get
            Return _idCiudad
        End Get
        Set(value As Integer)
            _idCiudad = value
        End Set
    End Property

    Public Property Ciudad As String
        Get
            Return _ciudad
        End Get
        Set(value As String)
            _ciudad = value
        End Set
    End Property

    Public Property NombreCiudad As String
        Get
            Return _nombreCiudad
        End Get
        Set(value As String)
            _nombreCiudad = value
        End Set
    End Property

    Public Property NombreDepartamento As String
        Get
            Return _nombreDepartamento
        End Get
        Set(value As String)
            _nombreDepartamento = value
        End Set
    End Property

    Public Property IdBodega As Integer
        Get
            Return _idBodega
        End Get
        Set(value As Integer)
            _idBodega = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idCiudad As Integer)
        MyBase.New()
        _idCiudad = idCiudad
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If _idCiudad > 0 Then .SqlParametros.Add("@lidCiudad", SqlDbType.VarChar, 2000).Value = CStr(_idCiudad)
                .ejecutarReader("ObtenerCiudadesBodegasCEM", CommandType.StoredProcedure)
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

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idCiudad"), _idCiudad)
                If Not IsDBNull(reader("Ciudad")) Then _ciudad = (reader("Ciudad").ToString)
                If Not IsDBNull(reader("nombreCiudad")) Then _nombreCiudad = (reader("nombreCiudad").ToString)
                If Not IsDBNull(reader("nombreDepartamento")) Then _nombreDepartamento = (reader("nombreDepartamento").ToString)
                Integer.TryParse(reader("idBodega"), _idBodega)
            End If
        End If
    End Sub

#End Region

End Class
