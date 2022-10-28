Imports ILSBusinessLayer
Imports LMDataAccessLayer

Public Class DiasNoHabiles

#Region "Aributos (Campos)"

    Private _idDia As Integer
    Private _fecha As Date
    Private _estado As Nullable(Of Boolean)
    Private _nombreDia As String

    Protected Friend _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(fecha As Date)
        MyBase.New()
        _fecha = fecha
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdDia As Integer
        Get
            Return _idDia
        End Get
        Set(value As Integer)
            _idDia = value
        End Set
    End Property

    Public Property Fecha As Date
        Get
            Return _fecha
        End Get
        Set(value As Date)
            _fecha = value
        End Set
    End Property

    Public Property Estado As Boolean
        Get
            Return _estado
        End Get
        Set(value As Boolean)
            _estado = value
        End Set
    End Property

    Public Property NombreDia As String
        Get
            Return _nombreDia
        End Get
        Protected Friend Set(value As String)
            _nombreDia = value
        End Set
    End Property

    Public Property Registrado As Boolean
        Get
            Return _registrado
        End Get
        Set(value As Boolean)
            _registrado = False
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        If _fecha <> Date.MinValue Or _estado IsNot Nothing Then
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        If _fecha <> Date.MinValue Then
                            .SqlParametros.Add("@fechaInicial", SqlDbType.DateTime).Value = _fecha
                            .SqlParametros.Add("@fechaFinal", SqlDbType.DateTime).Value = _fecha
                        End If
                        If _estado IsNot Nothing Then .SqlParametros.Add("@estado", SqlDbType.Bit).Value = _estado

                        .ejecutarReader("ObtenerDiasNoHabiles", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read() Then
                                If .Reader.HasRows Then
                                    Integer.TryParse(.Reader("idDia"), _idDia)
                                    Date.TryParse(.Reader("fecha"), _fecha)
                                    _estado = .Reader("estado")
                                    _nombreDia = .Reader("nombreDia")

                                    _registrado = True
                                End If
                            End If
                        End If
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
        End If
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    .iniciarTransaccion()

                    .SqlParametros.Add("@fecha", SqlDbType.Date).Value = _fecha
                    .SqlParametros.Add("@activo", SqlDbType.Bit).Value = True
                    .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                    .ejecutarNonQuery("RegistrarDiasNoHabiles", CommandType.StoredProcedure)
                    Dim respuesta As Integer = .SqlParametros("@respuesta").Value
                    If respuesta = 0 Then
                        .confirmarTransaccion()
                    Else
                        .abortarTransaccion()
                        Select Case respuesta
                            Case 1 : resultado.EstablecerMensajeYValor(respuesta, "La fecha ya se encuentra configurada cómo día no hábil.")
                            Case Else : resultado.EstablecerMensajeYValor(respuesta, "Se genero un error inesperado al intentar registrar [" & respuesta & "]")
                        End Select
                    End If
                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return resultado
    End Function

    Function Actualizar() As ResultadoProceso
        Dim resultado As New ResultadoProceso()
        Using dbManager As New LMDataAccess
            Try
                If _idDia > 0 Then
                    With dbManager
                        .SqlParametros.Add("@idDia", SqlDbType.Int).Value = _idDia
                        If _fecha <> Date.MinValue Then .SqlParametros.Add("@fecha", SqlDbType.Date).Value = _fecha
                        If _estado IsNot Nothing Then .SqlParametros.Add("@estado", SqlDbType.Bit).Value = _estado

                        .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("ActualizarDiasNoHabiles", CommandType.StoredProcedure)
                        Dim respuesta As Integer = .SqlParametros("@respuesta").Value
                        If respuesta = 0 Then
                            .confirmarTransaccion()
                        Else
                            .abortarTransaccion()
                            resultado.EstablecerMensajeYValor(respuesta, "Se genero un error inesperado al intentar actualizar [" & respuesta & "]")
                        End If
                    End With
                Else
                    resultado.EstablecerMensajeYValor(100, "No se proporcionaron los datos necesario para actualizar el registro")
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return resultado
    End Function

#End Region

End Class
