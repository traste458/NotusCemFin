Imports ILSBusinessLayer
Imports LMDataAccessLayer

Public Class GestionNovedadServicioMensajeria

#Region "Atributos (Campos)"

    Private _idGestion As Integer
    Private _idNovedad As Integer
    Private _observacion As String
    Private _idUsuario As Integer
    Private _fechaRegistro As Date

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

    Public Property IdNovedad() As Integer
        Get
            Return _idNovedad
        End Get
        Set(ByVal value As Integer)
            _idNovedad = value
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

    Public Property IdUsuario() As Integer
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Integer)
            _idUsuario = value
        End Set
    End Property

    Public Property FechaRegistro() As Date
        Get
            Return _fechaRegistro
        End Get
        Set(ByVal value As Date)
            _fechaRegistro = value
        End Set
    End Property

    Public Property Registrado() As Boolean
        Get
            Return _registrado
        End Get
        Set(ByVal value As Boolean)
            _registrado = value
        End Set
    End Property

#End Region

#Region "Métodos Públicos"

    Public Function Registrar(ByVal idUsuario As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Try
            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Add("@idNovedad", SqlDbType.Int).Value = _idNovedad
                    .SqlParametros.Add("@observacion", SqlDbType.VarChar, 2000).Value = _observacion
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario

                    .SqlParametros.Add("@result", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue

                    .ejecutarReader("RegistrarGestionNovedadServicioMensajeria", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing AndAlso .Reader.Read Then
                        resultado.Valor = .Reader("valor")
                        If resultado.Valor = 0 Then
                            resultado.Mensaje = "La gestión de la novedad fue registrada de manera exitosa."
                        Else
                            resultado.Mensaje = .Reader("mensaje").ToString
                        End If
                        .Reader.Close()
                    Else
                        Throw New Exception("Ocurrió un error interno al registrar la gestión de la novedad.")
                    End If
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return resultado
    End Function

#End Region

End Class
