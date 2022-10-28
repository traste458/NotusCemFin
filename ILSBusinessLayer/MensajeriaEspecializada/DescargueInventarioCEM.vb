Imports LMDataAccesLayer
Imports ILSBusinessLayer.Inventario
Imports LMDataAccessLayer

Namespace MensajeriaEspecializada
    Public Class DescargueInventarioCEM
        Inherits InventarioBodegaSateliteColeccion

#Region "Constructores"

        Public Sub New(ByVal seriales As List(Of String))
            MyBase.New()
            Me.Serial = seriales
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub registraError(ByRef dtDatos As DataTable, ByVal linea As Integer, ByVal mensaje As String)
            Try
                Dim row As DataRow = dtDatos.NewRow()
                row.Item("mensaje") = "Linea " & linea & ": " & mensaje
                dtDatos.Rows.Add(row)
                dtDatos.AcceptChanges()
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

#End Region

#Region "Método Públicos"

        Public Function ValidarDescargue(ByVal idUsuario As Integer, ByVal dtSeriales As DataTable) As DataTable
            Dim dtReturn As New DataTable
            Try
                dtSeriales.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), idUsuario))

                dtReturn.Columns.Add(New DataColumn("Mensaje", GetType(String)))
                Using dbManager As New LMDataAccess

                    With dbManager
                        With .SqlParametros
                            .Clear()
                            .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        End With
                        .ejecutarNonQuery("EliminaTempDescargueInventarioCEM", CommandType.StoredProcedure)
                        .inicilizarBulkCopy()
                        With .BulkCopy
                            .DestinationTableName = "TempDescargueInventarioCEM"
                            .ColumnMappings.Add("Serial", "Serial")
                            .ColumnMappings.Add("idUsuario", "idUsuario")
                            .WriteToServer(dtSeriales)
                        End With

                        .SqlParametros.Clear()
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .ejecutarReader("ValidarDescargueInventarioCEM", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                            While .Reader.Read()
                                registraError(dtReturn, CInt(.Reader("idError")), .Reader("Descripcion").ToString())
                            End While
                        End If
                    End With
                End Using
            Catch ex As Exception
                Throw ex
            End Try
            Return dtReturn
        End Function

#End Region

    End Class
End Namespace


