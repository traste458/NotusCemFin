Imports LMDataAccessLayer
Imports System.String
Imports ILSBusinessLayer.MensajeriaEspecializada.HerramientasMensajeria

Public Class HorarioVenta

#Region "Atributos"

    Private _idHorario As Short
    Private _idJornada As Short
    Private _nombreJornada As String
    Private _horaInicial As TimeSpan
    Private _horaFinal As TimeSpan
    Private _activo As Nullable(Of Boolean)

#End Region

#Region "Propiedades"

    Public Property IdHorario As Short
        Get
            Return _idHorario
        End Get
        Set(value As Short)
            _idHorario = value
        End Set
    End Property

    Public Property IdJornada As Short
        Get
            Return _idJornada
        End Get
        Set(value As Short)
            _idJornada = value
        End Set
    End Property

    Public Property HoraInicial As TimeSpan
        Get
            Return _horaInicial
        End Get
        Set(value As TimeSpan)
            _horaInicial = value
        End Set
    End Property

    Public Property NombreJornada As String
        Get
            Return _nombreJornada
        End Get
        Protected Friend Set(value As String)
            _nombreJornada = value
        End Set
    End Property

    Public Property HoraFinal As TimeSpan
        Get
            Return _horaFinal
        End Get
        Set(value As TimeSpan)
            _horaFinal = value
        End Set
    End Property

    Public Property Activo As Boolean
        Get
            Return _activo
        End Get
        Set(value As Boolean)
            _activo = value
        End Set
    End Property

#End Region

#Region "Contructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idHorario As Short)
        MyBase.New()
        _idHorario = idHorario
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idHorario > 0 Then .SqlParametros.Add("@idHorario", SqlDbType.SmallInt).Value = _idHorario
                    If _idJornada > 0 Then .SqlParametros.Add("@idJornada", SqlDbType.SmallInt).Value = _idJornada
                    If _activo IsNot Nothing Then .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo

                    .ejecutarReader("ObtenerHorariosVentas", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idHorario").ToString, _idHorario)
                            Integer.TryParse(.Reader("idJornada").ToString, _idJornada)
                            _nombreJornada = .Reader("nombreJornada")
                            _horaInicial = .Reader("horaInicial")
                            _horaFinal = .Reader("horaFinal")
                            _activo = .Reader("activo")
                        End If
                        .Reader.Close()
                    End If
                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Using
    End Sub

#End Region

#Region "Métodos Públicos"

    Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Using dbManager As New LMDataAccess
            Try
                If _idJornada > 0 Then
                    With dbManager
                        .SqlParametros.Add("@idJornada", SqlDbType.SmallInt).Value = _idJornada
                        .SqlParametros.Add("@horaInicio", SqlDbType.Time).Value = _horaInicial
                        .SqlParametros.Add("@horaFin", SqlDbType.Time).Value = _horaFinal
                        .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo
                        .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                        .iniciarTransaccion()
                        .ejecutarNonQuery("RegistrarHorarioVenta", CommandType.StoredProcedure)

                        Dim respuesta As Integer = .SqlParametros("@respuesta").Value
                        If respuesta = 0 Then
                            .confirmarTransaccion()
                        Else
                            resultado.EstablecerMensajeYValor(respuesta, "Se genereo un error inesperado al intentar realizar el registro [" & respuesta & "]")
                            .abortarTransaccion()
                        End If
                    End With
                Else
                    resultado.EstablecerMensajeYValor(100, "No se proporcionaron los datos suficientes para realizar el registro.")
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return resultado
    End Function

    Function Actualizar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Using dbManager As New LMDataAccess
            Try
                If _idHorario > 0 Then
                    With dbManager
                        .SqlParametros.Add("@idHorario", SqlDbType.SmallInt).Value = _idHorario
                        .SqlParametros.Add("@idJornada", SqlDbType.SmallInt).Value = _idJornada
                        .SqlParametros.Add("@horaInicio", SqlDbType.Time).Value = _horaInicial
                        .SqlParametros.Add("@horaFin", SqlDbType.Time).Value = _horaFinal
                        .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo

                        .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                        .iniciarTransaccion()
                        .ejecutarNonQuery("ActualizarHorarioVenta", CommandType.StoredProcedure)

                        Dim respuesta As Integer = .SqlParametros("@respuesta").Value
                        If respuesta = 0 Then
                            .confirmarTransaccion()
                        Else
                            .abortarTransaccion()
                        End If
                    End With
                Else
                    resultado.EstablecerMensajeYValor(100, "No se proporcionaron los datos suficientes para actualizar el registro.")
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return resultado
    End Function

#End Region

End Class
