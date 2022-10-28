Imports LMDataAccessLayer
Imports ILSBusinessLayer
Imports ILSBusinessLayer.Enumerados
Imports System.IO
Imports System.Web
Imports BPColSysOP.MetodosComunes

Namespace MensajeriaEspecializada

    Public Class GestionTipoSIMEspeciales

#Region "Atributos (Campos)"

        Private _fechaInicio As Date
        Private _fechaFin As Date

#End Region

#Region "Propiedades"

        Public Property FechaInicio() As Date
            Get
                Return _fechaInicio
            End Get
            Set(ByVal value As Date)
                _fechaInicio = value
            End Set
        End Property

        Public Property FechaFin() As Date
            Get
                Return _fechaFin
            End Get
            Set(ByVal value As Date)
                _fechaFin = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function GenerarReporte(ByVal idUsuario As Integer) As ResultadoProceso
            Dim resultado As New ResultadoProceso()
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = Enumerados.EstadoServicio.Entregado
                        .SqlParametros.Add("@return", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("GenerarInformacionReporteTipoSIMEspeciales", CommandType.StoredProcedure)

                        resultado.Valor = .SqlParametros("@return").Value
                        If resultado.Valor = 0 Then
                            resultado.Mensaje = "Se realizó la generación del reporte exitosamente."
                        Else
                            Select Case resultado.Valor
                                Case 1
                                    resultado.Mensaje = "No se encontraron registros para generar el reporte."
                                Case 2
                                    resultado.Mensaje = "Se generó un error inseperado."
                            End Select
                        End If
                    End With
                Catch ex As Exception
                    If dbManager.estadoConexion Then dbManager.abortarTransaccion()
                    Throw ex
                End Try
            End Using
            Return resultado
        End Function

        Public Function ObtenerDatos() As DataTable
            Dim dtDatos As New DataTable
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        If _fechaInicio <> Date.MinValue And _fechaFin <> Date.MinValue Then
                            .SqlParametros.Add("@fechaInicio", SqlDbType.DateTime).Value = _fechaInicio
                            .SqlParametros.Add("@fechaFin", SqlDbType.DateTime).Value = _fechaFin
                        End If

                        dtDatos = .ejecutarDataTable("ObtenerInformacionReporteTipoSIM", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return dtDatos
        End Function

#End Region

#Region "Métodos compartidos"

        Public Shared Sub GenerarRutaArchivo(ByVal idReporte As Integer, ByVal nombreArchivo As String)

            If Not File.Exists(nombreArchivo) Then
                If Not Directory.Exists(Path.GetDirectoryName(nombreArchivo)) Then
                    Throw New Exception("No existe el directorio de almacenamiento de reportes.")
                End If
                Using dbManager As New LMDataAccess
                    With dbManager
                        Try
                            .iniciarTransaccion()
                            .SqlParametros.Clear()
                            .SqlParametros.Add("@idReporte", SqlDbType.Int).Value = idReporte
                            Dim dvDatos As DataView = .ejecutarDataTable("ObtenerInformacionDetalleReporteTipoSIM", CommandType.StoredProcedure).DefaultView
                            Dim dtDatos As DataTable = dvDatos.ToTable(False, "numeroRadicado", "cliente", "min", _
                                                                       "sim", "valorSimSinIVA", "valorSimIVA", _
                                                                       "valorSimTotal", "ciudad", "tipoSIM", "observacion")
                            Dim arrayNombre As New ArrayList
                            With arrayNombre
                                .Add("NÚMERO RADICADO")
                                .Add("EMPRESA")
                                .Add("MIN")
                                .Add("SIM CARD ENTREGADA")
                                .Add("VALOR SIM CARD SIN IVA")
                                .Add("IVA SIM CARD")
                                .Add("VALOR TOTAL")
                                .Add("CIUDAD")
                                .Add("TIPO SIM")
                                .Add("OBSERVACION")
                            End With

                            HerramientasMensajeria.exportarDatosAExcelGemBox(HttpContext.Current, dtDatos, nombreArchivo, arrayNombre, True)

                            .confirmarTransaccion()
                        Catch ex As Exception
                            .abortarTransaccion()
                            Throw ex
                        End Try
                    End With
                End Using
            End If
        End Sub

        Public Shared Sub GenerarReporteControl(ByVal nombreArchivo As String, Optional ByVal fechaInicio As Date = Nothing, Optional ByVal fechaFin As Date = Nothing)
            If Not Directory.Exists(Path.GetDirectoryName(nombreArchivo)) Then
                Throw New Exception("No existe el directorio de almacenamiento de reportes.")
            End If

            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Clear()
                        If fechaInicio <> Date.MinValue Then .SqlParametros.Add("@fechaInicio", SqlDbType.Int).Value = fechaInicio
                        If fechaFin <> Date.MinValue Then .SqlParametros.Add("@fechaFin", SqlDbType.Int).Value = fechaFin

                        Dim dvDatos As DataView = .ejecutarDataTable("ConsultarInformacionReporteTipoSIMEspeciales", CommandType.StoredProcedure).DefaultView
                        Dim dtDatos As DataTable = dvDatos.ToTable(False, "numeroRadicado", "Estado", "cliente", "min", _
                                                                   "sim", "valorSimSinIVA", "valorSimIVA", _
                                                                   "valorSimTotal", "ciudad", "tipoSIM")
                        Dim arrayNombre As New ArrayList
                        With arrayNombre
                            .Add("NÚMERO RADICADO")
                            .Add("ESTADO")
                            .Add("EMPRESA")
                            .Add("MIN")
                            .Add("SIM CARD ENTREGADA")
                            .Add("VALOR SIM CARD SIN IVA")
                            .Add("IVA SIM CARD")
                            .Add("VALOR TOTAL")
                            .Add("CIUDAD")
                            .Add("TIPO SIM")
                            .Add("OBSERVACION")
                        End With

                        HerramientasMensajeria.exportarDatosAExcelGemBox(HttpContext.Current, dtDatos, nombreArchivo, arrayNombre, True)
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
        End Sub

#End Region

    End Class

End Namespace