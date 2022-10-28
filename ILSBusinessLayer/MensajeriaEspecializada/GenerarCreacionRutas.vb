Imports LMDataAccessLayer
Imports ILSBusinessLayer
Imports ILSBusinessLayer.Enumerados
Imports System.Web

Namespace MensajeriaEspecializada

    Public Class GenerarCreacionRutas

#Region "Atributos (Filtros)"

        Private _idServicioMensajeria As Integer
        Private _numeroRadicado As Long
        Private _identificacion As String
        Private _idTercero As Integer
        Private _fechaAgendaInicial As Date
        Private _fechaAgendaFinal As Date
        Private _idJornada As Integer
        Private _idTipoServicio As Integer

#End Region

#Region "Propiedades"

        Public Property IdServicioMensajeria() As Integer
            Get
                Return _idServicioMensajeria
            End Get
            Set(ByVal value As Integer)
                _idServicioMensajeria = value
            End Set
        End Property

        Public Property NumeroRadicado() As Long
            Get
                Return _numeroRadicado
            End Get
            Set(ByVal value As Long)
                _numeroRadicado = value
            End Set
        End Property

        Public Property Identificacion() As String
            Get
                Return _identificacion
            End Get
            Set(ByVal value As String)
                _identificacion = value
            End Set
        End Property

        Public Property FechaAgendaInicial() As Date
            Get
                Return _fechaAgendaInicial
            End Get
            Set(ByVal value As Date)
                _fechaAgendaInicial = value
            End Set
        End Property

        Public Property FechaAgendaFinal() As Date
            Get
                Return _fechaAgendaFinal
            End Get
            Set(ByVal value As Date)
                _fechaAgendaFinal = value
            End Set
        End Property

        Public Property IdJornada() As Integer
            Get
                Return _idJornada
            End Get
            Set(ByVal value As Integer)
                _idJornada = value
            End Set
        End Property

        Public Property IdTercero() As Integer
            Get
                Return _idTercero
            End Get
            Set(ByVal value As Integer)
                _idTercero = value
            End Set
        End Property

        Public Property IdTipoServicio As Integer
            Get
                Return _idTipoServicio
            End Get
            Set(value As Integer)
                _idTipoServicio = value
            End Set
        End Property

#End Region

#Region "Constructores"

#End Region

#Region "Métodos Públicos"

        Public Function GenerarPoolEntrega() As DataTable
            Dim dtDatos As New DataTable
            Dim idUsuario As Integer = 0
            If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.Session("usxp001") IsNot Nothing Then _
                Integer.TryParse(HttpContext.Current.Session("usxp001").ToString, idUsuario)

            Using _dbManager = New LMDataAccess
                Try
                    With _dbManager
                        With .SqlParametros
                            .Clear()
                            If _idServicioMensajeria > 0 Then .Add("@idServicioMensajeria", SqlDbType.Int).Value = _idServicioMensajeria
                            If _numeroRadicado > 0 Then .Add("@numeroRadicado", SqlDbType.BigInt).Value = _numeroRadicado
                            If _identificacion <> String.Empty Then .Add("@identificacion", SqlDbType.VarChar).Value = _identificacion
                            If _fechaAgendaInicial <> Date.MinValue And _fechaAgendaInicial <> Date.MinValue Then
                                .Add("@fechaAgendaInicio", SqlDbType.DateTime).Value = _fechaAgendaInicial
                                .Add("@fechaAgendaFin", SqlDbType.DateTime).Value = _fechaAgendaFinal
                            End If
                            If _idJornada > 0 Then .Add("@idJornada", SqlDbType.Int).Value = _idJornada
                            If _idTercero > 0 Then .Add("@idTercero", SqlDbType.Int).Value = _idTercero
                            If _idTipoServicio > 0 Then .Add("@idTipoServicio", SqlDbType.Int).Value = _idTipoServicio
                            If idUsuario > 0 Then .Add("@idUsuarioConsulta", SqlDbType.Int).Value = idUsuario
                        End With
                        dtDatos = .ejecutarDataTable("ObtenerInformacionRutaServicioMensajeria", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return dtDatos
        End Function

        Public Function GenerarPoolEntregaVentas() As DataTable
            Dim dtDatos As New DataTable
            Dim idUsuario As Integer = 0
            If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.Session("usxp001") IsNot Nothing Then _
                Integer.TryParse(HttpContext.Current.Session("usxp001").ToString, idUsuario)

            Using _dbManager = New LMDataAccess
                Try
                    With _dbManager
                        With .SqlParametros
                            .Clear()
                            If _idServicioMensajeria > 0 Then .Add("@idServicioMensajeria", SqlDbType.Int).Value = _idServicioMensajeria
                            If _numeroRadicado > 0 Then .Add("@numeroRadicado", SqlDbType.BigInt).Value = _numeroRadicado
                            If _identificacion <> String.Empty Then .Add("@identificacion", SqlDbType.VarChar).Value = _identificacion
                            If _fechaAgendaInicial <> Date.MinValue And _fechaAgendaInicial <> Date.MinValue Then
                                .Add("@fechaAgendaInicio", SqlDbType.DateTime).Value = _fechaAgendaInicial
                                .Add("@fechaAgendaFin", SqlDbType.DateTime).Value = _fechaAgendaFinal
                            End If
                            If _idJornada > 0 Then .Add("@idJornada", SqlDbType.Int).Value = _idJornada
                            If _idTercero > 0 Then .Add("@idTercero", SqlDbType.Int).Value = _idTercero
                            If idUsuario > 0 Then .Add("@idUsuarioConsulta", SqlDbType.Int).Value = idUsuario
                        End With
                        dtDatos = .ejecutarDataTable("ObtenerInformacionRutaServicioMensajeriaVenta", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return dtDatos
        End Function

        Public Function GenerarPoolEntregaServicioTecnico() As DataTable
            Dim dtDatos As New DataTable
            Dim idUsuario As Integer = 0
            If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.Session("usxp001") IsNot Nothing Then _
                Integer.TryParse(HttpContext.Current.Session("usxp001").ToString, idUsuario)

            Using _dbManager = New LMDataAccess
                Try
                    With _dbManager
                        With .SqlParametros
                            .Clear()
                            If _idServicioMensajeria > 0 Then .Add("@idServicioMensajeria", SqlDbType.Int).Value = _idServicioMensajeria
                            If _numeroRadicado > 0 Then .Add("@numeroRadicado", SqlDbType.BigInt).Value = _numeroRadicado
                            If _identificacion <> String.Empty Then .Add("@identificacion", SqlDbType.VarChar).Value = _identificacion
                            If _fechaAgendaInicial <> Date.MinValue And _fechaAgendaInicial <> Date.MinValue Then
                                .Add("@fechaAgendaInicio", SqlDbType.DateTime).Value = _fechaAgendaInicial
                                .Add("@fechaAgendaFin", SqlDbType.DateTime).Value = _fechaAgendaFinal
                            End If
                            If _idJornada > 0 Then .Add("@idJornada", SqlDbType.Int).Value = _idJornada
                            If _idTercero > 0 Then .Add("@idTercero", SqlDbType.Int).Value = _idTercero
                            If idUsuario > 0 Then .Add("@idUsuarioConsulta", SqlDbType.Int).Value = idUsuario
                        End With
                        dtDatos = .ejecutarDataTable("ObtenerInformacionRutaEntregaSTServicioMensajeria", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return dtDatos
        End Function

        Public Function GenerarPoolRecoleccion() As DataTable
            Dim dtDatos As New DataTable
            Dim idUsuario As Integer = 0
            If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.Session("usxp001") IsNot Nothing Then _
                Integer.TryParse(HttpContext.Current.Session("usxp001").ToString, idUsuario)
            Using _dbManager = New LMDataAccess
                Try
                    With _dbManager
                        With .SqlParametros
                            .Clear()
                            If _idServicioMensajeria > 0 Then .Add("@idServicioMensajeria", SqlDbType.Int).Value = _idServicioMensajeria
                            If _numeroRadicado > 0 Then .Add("@numeroRadicado", SqlDbType.BigInt).Value = _numeroRadicado
                            If _identificacion <> String.Empty Then .Add("@identificacion", SqlDbType.VarChar).Value = _identificacion
                            If _fechaAgendaInicial <> Date.MinValue And _fechaAgendaInicial <> Date.MinValue Then
                                .Add("@fechaAgendaInicio", SqlDbType.DateTime).Value = _fechaAgendaInicial
                                .Add("@fechaAgendaFin", SqlDbType.DateTime).Value = _fechaAgendaFinal
                            End If
                            If _idJornada > 0 Then .Add("@idJornada", SqlDbType.Int).Value = _idJornada
                            If _idTercero > 0 Then .Add("@idTercero", SqlDbType.Int).Value = _idTercero
                            If idUsuario > 0 Then .Add("@idUsuarioConsulta", SqlDbType.Int).Value = idUsuario
                        End With
                        dtDatos = .ejecutarDataTable("ObtenerInformacionRutaRecoleccionServicioMensajeria", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return dtDatos
        End Function

        Public Function GenerarPoolEnvioServicioTecnico() As DataTable
            Dim dtDatos As New DataTable
            Dim idUsuario As Integer = 0
            If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.Session("usxp001") IsNot Nothing Then _
                Integer.TryParse(HttpContext.Current.Session("usxp001").ToString, idUsuario)
            Using _dbManager = New LMDataAccess
                Try
                    With _dbManager
                        With .SqlParametros
                            If idUsuario > 0 Then .Add("@idUsuarioConsulta", SqlDbType.Int).Value = idUsuario
                        End With
                        dtDatos = .ejecutarDataTable("ObtenerInformacionOrdenDespachoServicioMensajeria", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return dtDatos
        End Function

        Public Function GenerarPoolRecoleccionServicioTecnico() As DataTable
            Dim dtDatos As New DataTable
            Dim idUsuario As Integer = 0
            If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.Session("usxp001") IsNot Nothing Then _
                Integer.TryParse(HttpContext.Current.Session("usxp001").ToString, idUsuario)
            Using _dbManager = New LMDataAccess
                Try
                    With _dbManager
                        With .SqlParametros
                            If idUsuario > 0 Then .Add("@idUsuarioConsulta", SqlDbType.Int).Value = idUsuario
                        End With
                        dtDatos = .ejecutarDataTable("ObtenerInformacionOrdenRecoleccionServicioMensajeria", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using

            Return dtDatos
        End Function

        Public Function GenerarPoolRecoleccionServicioSiembra() As DataTable
            Dim dtDatos As New DataTable
            Dim idUsuario As Integer = 0

            If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.Session("usxp001") IsNot Nothing Then _
                Integer.TryParse(HttpContext.Current.Session("usxp001").ToString, idUsuario)

            Using _dbManager = New LMDataAccess
                Try
                    With _dbManager
                        With .SqlParametros
                            If idUsuario > 0 Then .Add("@idUsuarioConsulta", SqlDbType.Int).Value = idUsuario
                            If _idServicioMensajeria > 0 Then .Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                        End With
                        dtDatos = .ejecutarDataTable("ObtenerInformacionRutaRecoleccionSiembra", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using

            Return dtDatos
        End Function

#End Region

    End Class

End Namespace

