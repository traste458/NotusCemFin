Imports ILSBusinessLayer
Imports LMDataAccessLayer

Public Class GestionServicioTecnico

#Region "Atributos (Campos)"

    Private _idGestion As Integer
    Private _idDetalleSerial As Long
    Private _fecha As Date
    Private _idUsuario As Integer
    Private _nombreUsuario As String
    Private _observacion As String

    Private _registrado As Boolean
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdGestion() As Integer
        Get
            Return _idGestion
        End Get
        Set(ByVal value As Integer)
            _idGestion = value
        End Set
    End Property

    Public Property IdDetalleSerial() As Long
        Get
            Return _idDetalleSerial
        End Get
        Set(ByVal value As Long)
            _idDetalleSerial = value
        End Set
    End Property

    Public Property Fecha() As Date
        Get
            Return _fecha
        End Get
        Set(ByVal value As Date)
            _fecha = value
        End Set
    End Property

    Public Property IdUsuario() As Integer
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Integer)
            _idUsuario = value
        End Set
    End Property

    Public Property NombreUsuario() As String
        Get
            Return _nombreUsuario
        End Get
        Protected Friend Set(ByVal value As String)
            _nombreUsuario = value
        End Set
    End Property


    Public Property Observacion() As String
        Get
            Return _observacion
        End Get
        Set(ByVal value As String)
            _observacion = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess

        Try
            With dbManager
                .SqlParametros.Add("@idGestion", SqlDbType.Int).Value = _idGestion
                .ejecutarReader("ObtenerGestionServicioTecnico", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    If .Reader.Read Then
                        Integer.TryParse(.Reader("idGestion").ToString, _idGestion)
                        Long.TryParse(.Reader("idDetalleSerial").ToString, _idDetalleSerial)
                        Date.TryParse(.Reader("fecha").ToString, _fecha)
                        Integer.TryParse(.Reader("idUsuario").ToString, _idUsuario)
                        _nombreUsuario = .Reader("nombreUsuario").ToString
                        _observacion = .Reader("observacion").ToString

                        _registrado = True
                    End If
                    .Reader.Close()
                End If

            End With
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function Registrar(ByVal idUsuario As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso

        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idDetalleSerial", SqlDbType.BigInt).Value = _idDetalleSerial
                    .SqlParametros.Add("@fecha", SqlDbType.DateTime).Value = _fecha
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .SqlParametros.Add("@observacion", SqlDbType.VarChar).Value = _observacion

                    .ejecutarReader("RegistrarGestionServicioTecnico", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing AndAlso .Reader.Read Then
                        resultado.Valor = .Reader("valor")
                        If resultado.Valor = 0 Then
                            resultado.Mensaje = "La gestión fue registrada de manera exitosa."
                        Else
                            resultado.Mensaje = .Reader("mensaje").ToString
                        End If
                        .Reader.Close()
                    Else
                        Throw New Exception("Ocurrió un error interno al registrar la gestión. Por favor intente nuevamente.")
                    End If
                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Using

        Return resultado
    End Function

#End Region

End Class
