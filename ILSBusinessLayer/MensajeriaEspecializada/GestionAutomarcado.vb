Imports LMDataAccessLayer
Imports ILSBusinessLayer
Imports ILSBusinessLayer.Enumerados
Imports System.IO
Imports System.Web
Imports BPColSysOP.MetodosComunes

Namespace MensajeriaEspecializada

    Public Class GestionAutomarcado

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

        Public Function ObtenerDatos() As DataTable
            Dim dtDatos As New DataTable
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        If _fechaInicio <> Date.MinValue And _fechaFin <> Date.MinValue Then
                            .SqlParametros.Add("@fechaInicio", SqlDbType.DateTime).Value = _fechaInicio
                            .SqlParametros.Add("@fechaFin", SqlDbType.DateTime).Value = _fechaFin
                        End If

                        dtDatos = .ejecutarDataTable("ObtenerInformacionReporteAutomarcado", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return dtDatos
        End Function

#End Region

#Region "Métodos Compartidos"

        Public Shared Sub GenerarRutaArchivo(ByVal idAutomarcado As Integer, ByVal nombreArchivo As String)

            If Not File.Exists(nombreArchivo) Then
                If Not Directory.Exists(Path.GetDirectoryName(nombreArchivo)) Then
                    Throw New Exception("No existe el directorio de almacenamiento de reportes.")
                End If
                Using dbManager As New LMDataAccess
                    With dbManager
                        Try
                            .iniciarTransaccion()
                            .SqlParametros.Clear()
                            .SqlParametros.Add("@idAutomarcado", SqlDbType.Int).Value = idAutomarcado
                            Dim dvDatos As DataView = .ejecutarDataTable("ObtenerInformacionDetalleReporteAutomarcado", CommandType.StoredProcedure).DefaultView
                            Dim dtDatos As DataTable = dvDatos.ToTable(False, "numeroRadicado", "cliente", "identicacionCliente", _
                                                                       "personaDeContacto", "telefonoDeContacto", "extension", _
                                                                       "tipoTelefono", "indicativo", "ciudad", "departamento", "barrio", "direccion")
                            Dim arrayNombre As New ArrayList
                            With arrayNombre
                                .Add("Número Radicado")
                                .Add("Cliente")
                                .Add("Identicación Cliente")
                                .Add("Persona de Contacto")
                                .Add("Teléfono de Contacto")
                                .Add("Extensión")
                                .Add("Tipo Teléfono")
                                .Add("Indicativo")
                                .Add("Ciudad")
                                .Add("Departamento")
                                .Add("Barrio")
                                .Add("Dirección")
                            End With

                            Dim idUsuario As Integer
                            If HttpContext.Current.Session("usxp001") IsNot Nothing Then Integer.TryParse(HttpContext.Current.Session("usxp001"), idUsuario)
                            .SqlParametros.Clear()
                            .SqlParametros.Add("@idAutomarcado", SqlDbType.Int).Value = idAutomarcado
                            .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                            .ejecutarNonQuery("ActualizarInformacionDetalleReporteAutomarcado", CommandType.StoredProcedure)

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

#End Region

    End Class

End Namespace

