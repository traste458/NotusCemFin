Imports LMDataAccessLayer
Imports LMWebServiceSyncMonitorBusinessLayer.SAPConsultarInventario
Imports LMWebServiceSyncMonitorBusinessLayer.ClasesComunes

Public Class SincronizadorInventarioPorStock
#Region "Atributos (Campos)"
    Private ReadOnly ID_TIPO_SINCRONIZACION As Byte = 5
    Private _idLog As Integer
    Private _arrCentro() As SAPConsultarInventario.WsorWerk
    Private _dtInventario As DataTable

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarListaCentros()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .ejecutarReader("ObtenerRegiones", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    Dim numReg As Integer = 0
                    While .Reader.Read
                        ReDim Preserve _arrCentro(numReg)
                        _arrCentro(numReg) = New SAPConsultarInventario.WsorWerk
                        _arrCentro(numReg).werks = .Reader("centro").ToString
                        numReg += 1
                    End While
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable

        With dtAux.Columns
            .Add("centro", GetType(Integer))
            .Add("almacen", GetType(Integer))
            .Add("material", GetType(Integer))
            .Add("tipoMaterial", GetType(String))
            .Add("lote", GetType(String))
            .Add("cantLibreUtilizacion", GetType(Integer))
            .Add("cantControlCalidad", GetType(Integer))
            .Add("cantBloqueado", GetType(Integer))
        End With

        Return dtAux
    End Function

    Private Function ObtenerDatoASincronizar() As ResultadoProceso
        Dim returnValue As New ResultadoProceso
        Dim wsSyncInventario As New SAPConsultarInventario.WS_INVENTARIO_LG
        Dim resultado As New SAPConsultarInventario.OutputInvLg
        Dim genCredenciales As New GeneradorCredencialesWebService
        Dim infoWs As New InfoUrlWebService(wsSyncInventario, True)
        _dtInventario = CrearEstructuraDeTabla()
        wsSyncInventario.Timeout = 600000
        CargarListaCentros()
        wsSyncInventario.Credentials = genCredenciales.Credenciales
        resultado = wsSyncInventario.executeZmmLgInventario(_arrCentro, Nothing, Nothing, Nothing)
        If resultado IsNot Nothing Then
            With resultado
                If resultado.oMensajes IsNot Nothing Then
                    Dim hayError As Boolean = False
                    For index As Integer = 0 To .oMensajes.Length - 1
                        If .oMensajes(index).type = "E" Or .oMensajes(index).type = "A" Then
                            returnValue.Valor = 1
                            returnValue.Mensaje = .oMensajes(index).message
                            hayError = True
                            Exit For
                        End If
                    Next
                    If Not hayError Then
                        Dim drAux As DataRow
                        For index As Integer = 0 To .oMateriales.Length - 1
                            drAux = _dtInventario.NewRow
                            drAux("centro") = .oMateriales(index).centro
                            drAux("almacen") = .oMateriales(index).almacen
                            drAux("material") = .oMateriales(index).material
                            drAux("tipoMaterial") = .oMateriales(index).tipoMaterial
                            drAux("lote") = .oMateriales(index).lote
                            drAux("cantLibreUtilizacion") = .oMateriales(index).cantLibreutil
                            drAux("cantControlCalidad") = .oMateriales(index).cantConcalidad
                            drAux("cantBloqueado") = .oMateriales(index).cantBloqueado
                            _dtInventario.Rows.Add(drAux)
                        Next
                    End If
                Else
                    Throw New Exception("Imposible obtener datos desde SAP. No se puede realizar la sincronización del Inventario por Stock ")
                End If
            End With
        Else
            Throw New Exception("Imposible obtener datos desde SAP. No se puede realizar la sincronización del Inventario por Stock ")
        End If

        Return returnValue
    End Function

#End Region

#Region "Métodos Públicos"

    Public Function Sincronizar() As ResultadoProceso
        Dim returnValue As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        returnValue = ObtenerDatoASincronizar()
        If returnValue.Valor = 0 Then
            If _dtInventario IsNot Nothing AndAlso _dtInventario.Rows.Count > 0 Then
                With dbManager
                    .SqlParametros.Add("@idTipoSincronizacion", SqlDbType.TinyInt).Value = ID_TIPO_SINCRONIZACION
                    .SqlParametros.Add("@idLog", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    Try
                        .iniciarTransaccion()
                        .ejecutarNonQuery("RegistrarLogSincronizacionMaestro", CommandType.StoredProcedure)
                        Integer.TryParse(.SqlParametros("@idLog").Value.ToString, _idLog)
                        If _idLog <> 0 Then
                            Dim dcAux As New DataColumn("idLog", GetType(String))
                            dcAux.DefaultValue = _idLog
                            _dtInventario.Columns.Add(dcAux)
                            .inicilizarBulkCopy()
                            With .BulkCopy
                                .DestinationTableName = "LogSincInventarioPorStock"
                                .ColumnMappings.Add("idLog", "idLog")
                                .ColumnMappings.Add("centro", "centro")
                                .ColumnMappings.Add("almacen", "almacen")
                                .ColumnMappings.Add("material", "material")
                                .ColumnMappings.Add("tipoMaterial", "tipoMaterial")
                                .ColumnMappings.Add("lote", "lote")
                                .ColumnMappings.Add("cantLibreUtilizacion", "cantLibreUtilizacion")
                                .ColumnMappings.Add("cantControlCalidad", "cantControlCalidad")
                                .ColumnMappings.Add("cantBloqueado", "cantBloqueado")
                                .WriteToServer(_dtInventario)
                            End With
                            .confirmarTransaccion()
                            .iniciarTransaccion()
                            .SqlParametros.Clear()
                            .SqlParametros.Add("@idLog", SqlDbType.Int).Value = _idLog
                            .SqlParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                            .ejecutarNonQuery("SincronizarInventarioPorStock", CommandType.StoredProcedure)
                            Dim resultado As Short = CShort(.SqlParametros("@returnValue").Value)
                            If resultado <> 0 Then Throw New Exception("Ocurrió un error inesperado al tratar de sincronizar Inventario por Stock.")
                            .confirmarTransaccion()
                        Else
                            returnValue.EstablecerValorYMensaje(1, "Ocurrió un error inesperado al tratar de generar Log de Sincronización. ")
                            dbManager.abortarTransaccion()
                        End If
                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                        Throw New Exception(ex.Message, ex)
                    End Try
                End With
            End If
        End If
        Return returnValue
    End Function

#End Region
End Class

