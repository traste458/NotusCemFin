Imports LMDataAccessLayer
Imports ILSBusinessLayer
Imports ILSBusinessLayer.Comunes
Imports System.IO

Public Class CobroFabricante

#Region "Atributos"

#End Region

#Region "Constructores"

#End Region

#Region "Metodos Publicos"

    Public Function RegistrarCobroFabricante(objDatos As wsCobroFabricante) As ResultadoProceso
        Dim resultado As New ILSBusinessLayer.ResultadoProceso(-1, "Cobro a fabricante no registrado.")
        Dim dbManager As New LMDataAccess
        Dim _idCobroNotus As Integer = 0
        Dim dtDetalle As DataTable
        dtDetalle = objDatos.ObjDatos.GenerarDataTable
        Try
            With dbManager
                With .SqlParametros
                    .Add("@idCobro", SqlDbType.Int).Value = objDatos.IdCobro
                    .Add("@fabricante", SqlDbType.VarChar).Value = objDatos.fabricante
                    .Add("@archivo", SqlDbType.Binary).Value = objDatos.Archivo
                    .Add("@nombreArchivo", SqlDbType.VarChar).Value = objDatos.NombreArchivo
                    If objDatos.Observacion <> "" Then .Add("@observacion", SqlDbType.VarChar).Value = objDatos.Observacion
                    .Add("@usuario", SqlDbType.VarChar).Value = objDatos.usuario
                    .Add("@mensaje", SqlDbType.VarChar, 400).Direction = ParameterDirection.Output
                    .Add("@idCobroNotus", SqlDbType.Int).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                End With
                .iniciarTransaccion()
                .ejecutarNonQuery("RegistrarCobroFabricanteServicioTecnico", CommandType.StoredProcedure)
                If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                    If resultado.Valor = 0 Then
                        Integer.TryParse(.SqlParametros("@idCobroNotus").Value.ToString, _idCobroNotus)
                        With dtDetalle
                            .Columns.Add("idCobro", GetType(System.Int32))
                        End With
                        For Each dr As DataRow In dtDetalle.Rows
                            dr("idCobro") = _idCobroNotus
                        Next
                        .inicilizarBulkCopy()
                        With .BulkCopy
                            .DestinationTableName = "detalle_CobroFabricante"
                            .ColumnMappings.Add("idCobro", "idCobro")
                            .ColumnMappings.Add("serial", "serial")
                            .ColumnMappings.Add("ods", "ods")
                            .WriteToServer(dtDetalle)
                        End With
                        If .estadoTransaccional Then .confirmarTransaccion()
                        resultado.EstablecerMensajeYValor(.SqlParametros("@resultado").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                    Else
                        If .estadoTransaccional Then .abortarTransaccion()
                        resultado.EstablecerMensajeYValor(.SqlParametros("@resultado").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                    End If
                Else
                    If .estadoTransaccional Then .abortarTransaccion()
                    resultado.EstablecerMensajeYValor(-1, "No se pudo evaluar el resultado de registro arrojado por la base de  datos. Por favor intente nuevamente.")
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return resultado
    End Function

#End Region

End Class
