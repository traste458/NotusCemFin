Imports LMDataAccessLayer
Imports System.IO
Imports System.Web

Namespace InventarioFisico
    Public Class ReporteDeOrdenesDeInventarioFisico

#Region "Atributos"

        Private _idEstado As Short
        Private _cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
        End Sub

        Public Sub New(ByVal idEstado As Short)
            _idEstado = idEstado
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdEstado As Short
            Get
                Return _idEstado
            End Get
            Set(value As Short)
                _idEstado = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

#End Region

#Region "Métodos Públicos"

        Public Function GenerarArchivo(nombreReporte As String) As ResultadoProceso
            Dim resultado As New ResultadoProceso(-1, "Proceso no iniciado")
            If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.Server IsNot Nothing Then
                Dim nombreArchivo As String = nombreReporte.Replace(" ", "") & String.Format("{0:hhmm}", Now) & ".txt"
                Dim ruta As String = HttpContext.Current.Server.MapPath("~/archivos_planos/" & nombreArchivo)
                Using dbManager As New LMDataAccess
                    With dbManager
                        If _idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        .ejecutarReader("ReporteOrdenesDeInventarioFisico", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            Dim numRegistros As Integer = 0

                            Using manejadorArchivo As StreamWriter = File.CreateText(ruta)
                                If File.Exists(ruta) Then
                                    manejadorArchivo.WriteLine("USUARIO" & vbTab & "ID INVENTARIO" & vbTab & "MATERIAL" & vbTab & _
                                                           "REGIÓN" & vbTab & "LÍNEA" & vbTab & "CANTIDAD" & vbTab & "FECHA Y HORA")
                                    While .Reader.Read
                                        manejadorArchivo.WriteLine(.Reader("Auditor").ToString & vbTab & _
                                                                   .Reader("Idinventario").ToString & vbTab & _
                                                                   .Reader("Material").ToString & vbTab & _
                                                                   .Reader("Region").ToString & vbTab & _
                                                                   .Reader("Linea").ToString & vbTab & _
                                                                   .Reader("Cantidad").ToString & vbTab & _
                                                                   .Reader("FechaCierre").ToString)
                                        numRegistros += 1
                                    End While
                                    manejadorArchivo.WriteLine("Total Registros: " & numRegistros.ToString)
                                    resultado.EstablecerMensajeYValor(0, ruta)
                                Else
                                    resultado.EstablecerMensajeYValor(200, "No fue posible crear el archivo en el servidor. Por favor intente nuevamente.")
                                End If
                            End Using

                            .Reader.Close()
                        Else
                            resultado.EstablecerMensajeYValor(100, "No fue posible obtener resultados de la base da datos. Por favor intente nuevamente")
                        End If
                    End With
                End Using

            Else
                resultado.EstablecerMensajeYValor(500, "Imposible determinar el entorno de ejecución del reporte. Por favor contacte a IT Development")
            End If
            Return resultado
        End Function


#End Region

    End Class

End Namespace