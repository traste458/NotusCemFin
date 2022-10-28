Imports LMDataAccessLayer
Imports System.IO

Namespace MensajeriaEspecializada

    Public Class CambioMaterialCEM

#Region "Atributos"

        Private _idUsuario As Integer

#End Region

#Region "Constructores"

        Public Sub New(ByVal idUsuario As Integer)
            MyBase.New()
            _idUsuario = idUsuario
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function ProcesarArchivo(ByVal streamArchivo As Stream) As List(Of ResultadoProceso)
            Dim resultado As New List(Of ResultadoProceso)
            Dim sGuid As Guid = Guid.NewGuid
            Dim linea As Integer = 1

            Try
                Using srArchivo As New StreamReader(streamArchivo)
                    Dim dtDatos As DataTable = ObtenerEstructuraDatos()
                    Do
                        Dim strLinea As String = srArchivo.ReadLine()
                        If Not String.IsNullOrEmpty(strLinea) Then
                            Dim arrLinea As String() = strLinea.Split(vbTab)
                            If arrLinea.Length = 3 Then
                                Dim row As DataRow = dtDatos.NewRow()
                                    row("serial") = arrLinea(0)
                                    row("materialActual") = arrLinea(1)
                                    row("materialNuevo") = arrLinea(2)
                                    row("idUsuario") = _idUsuario
                                    row("guid") = sGuid
                                    dtDatos.Rows.Add(row)

                            Else
                                resultado.Add(New ResultadoProceso(linea, "Datos incompletos."))
                            End If
                        End If
                        linea = linea + 1
                    Loop While Not srArchivo.EndOfStream
                    dtDatos.AcceptChanges()

                    If resultado.Count = 0 Then
                        Using dbManager As New LMDataAccess
                            Try
                                With dbManager
                                    .iniciarTransaccion()
                                    .inicilizarBulkCopy()

                                    With .BulkCopy
                                        .DestinationTableName = "ArchivoCambioMaterial"
                                        .ColumnMappings.Add("serial", "serial")
                                        .ColumnMappings.Add("materialActual", "materialActual")
                                        .ColumnMappings.Add("materialNuevo", "materialNuevo")
                                        .ColumnMappings.Add("idUsuario", "idUsuario")
                                        .ColumnMappings.Add("guid", "guid")
                                        .WriteToServer(dtDatos)
                                    End With

                                    .SqlParametros.Clear()
                                    .SqlParametros.Add("@sGuid", SqlDbType.UniqueIdentifier).Value = sGuid
                                    .ejecutarReader("CambiarMaterialSeriales", CommandType.StoredProcedure)

                                    If .Reader IsNot Nothing And .Reader.HasRows Then
                                        While .Reader.Read()
                                            resultado.Add(New ResultadoProceso(1, CStr(.Reader.Item("Descripcion"))))
                                        End While
                                        .Reader.Close()
                                        .abortarTransaccion()
                                    Else
                                        If .Reader IsNot Nothing Then .Reader.Close()
                                        .confirmarTransaccion()
                                    End If
                                End With
                            Catch ex As Exception
                                If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                                Throw ex
                            End Try
                        End Using
                    End If

                End Using
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function

        Private Function ObtenerEstructuraDatos() As DataTable
            Dim dt As New DataTable
            dt.Columns.Add(New DataColumn("serial", GetType(String)))
            dt.Columns.Add(New DataColumn("materialActual", GetType(String)))
            dt.Columns.Add(New DataColumn("materialNuevo", GetType(String)))
            dt.Columns.Add(New DataColumn("idUsuario", GetType(Integer)))
            dt.Columns.Add(New DataColumn("guid", GetType(Guid)))
            Return dt
        End Function

#End Region

    End Class

End Namespace
