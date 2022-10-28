Imports LMDataAccessLayer
Imports LMWebServiceSyncMonitorBusinessLayer
Imports LMWebServiceSyncMonitorBusinessLayer.ClasesComunes

Public Class SincronizadorMaestroMateriales

#Region "Atributos (Campos)"

    Private ReadOnly ID_TIPO_SINCRONIZACION As Byte = 1
    Private _idSincronizacion As Integer
    Private _fechaCreacion As Date
    Private _fechaModificacion As Date
    Private _dtDetalleMaterial As DataTable

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        CargarUltimaFechaSincronizacion()
        If _fechaModificacion = Date.MinValue And _fechaCreacion = Date.MinValue Then _fechaModificacion = New Date(2011, 10, 1)
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarUltimaFechaSincronizacion()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Add("@idTipoSincronizacion", SqlDbType.TinyInt).Value = ID_TIPO_SINCRONIZACION
                .ejecutarReader("ObtenerUltimaFechaSincronizacionMaestro", CommandType.StoredProcedure)
                If .Reader IsNot Nothing AndAlso .Reader.Read() Then
                    Date.TryParse(.Reader("fechaSincronizacion").ToString, _fechaModificacion)
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
            .Add("material", GetType(Integer))
            .Add("descripcion", GetType(String))
            .Add("centro", GetType(Integer))
            .Add("tipoMaterial", GetType(String))
            .Add("clasificacion", GetType(String))
            .Add("codigoEan", GetType(String))
            .Add("fechaUltimaModificacion", GetType(Date))
            .Add("tipoModificacion", GetType(String))
        End With

        Return dtAux
    End Function

    Private Function ObtenerDatoASincronizar() As ResultadoProceso
        Dim returnValue As New ResultadoProceso
        Dim wsSyncMaterial As New SAPMaestroMateriales.WS_MAESTRO_MATERIAL_LG
        Dim resultado As New SAPMaestroMateriales.OutputLgMaestroMaterial
        Dim strFechaCreacion As String = Nothing
        Dim strFechaModificacion As String = Nothing
        Dim genCredenciales As New GeneradorCredencialesWebService
        Dim infoWs As New InfoUrlWebService(wsSyncMaterial, True)
        _dtDetalleMaterial = CrearEstructuraDeTabla()
        If _fechaCreacion > Date.MinValue Then strFechaCreacion = _fechaCreacion.ToString("yyyyMMddHHmmss")
        If _fechaModificacion > Date.MinValue Then strFechaModificacion = _fechaModificacion.ToString("yyyyMMddHHmmss")
        wsSyncMaterial.Timeout = 6000000
        wsSyncMaterial.Credentials = genCredenciales.Credenciales
        resultado = wsSyncMaterial.executeZmmLgMaestroMaterial(strFechaCreacion, strFechaModificacion)
        If resultado IsNot Nothing Then
            If resultado.oMensajes IsNot Nothing Then
                Dim hayError As Boolean = False
                For index As Integer = 0 To resultado.oMensajes.Length - 1
                    If resultado.oMensajes(index).type = "E" Or resultado.oMensajes(index).type = "A" Then
                        returnValue.Valor = 1
                        returnValue.Mensaje = resultado.oMensajes(index).message
                        hayError = True
                        Exit For
                    End If
                Next
                If Not hayError Then
                    Dim drAux As DataRow
                    Dim fechaAux As Date
                    For index As Integer = 0 To resultado.oMateriales.Length - 1
                        drAux = _dtDetalleMaterial.NewRow
                        drAux("material") = resultado.oMateriales(index).material
                        drAux("descripcion") = resultado.oMateriales(index).descripcion
                        drAux("centro") = resultado.oMateriales(index).region
                        drAux("tipoMaterial") = resultado.oMateriales(index).tipoMaterial
                        drAux("clasificacion") = resultado.oMateriales(index).clasificacion
                        drAux("codigoEan") = resultado.oMateriales(index).codEan11
                        If Date.TryParse(resultado.oMateriales(index).fechaHoraModificacion, fechaAux) Then _
                            drAux("fechaUltimaModificacion") = fechaAux
                        drAux("tipoModificacion") = resultado.oMateriales(index).tipoModificacion
                        _dtDetalleMaterial.Rows.Add(drAux)
                    Next
                End If
            Else
                Throw New Exception("Imposible obtener datos desde SAP. No se puede realizar la sincronización de Materiales ")
            End If
        Else
            Throw New Exception("Imposible obtener datos desde SAP. No se puede realizar la sincronización de Materiales ")
        End If

        Return returnValue
    End Function

#End Region

#Region "Métodos Públicos"

    Public Function Sincronizar() As ResultadoProceso
        Dim returnValue As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Dim idLog As Integer = 0
        returnValue = ObtenerDatoASincronizar()
        If returnValue.Valor = 0 Then
            If _dtDetalleMaterial IsNot Nothing AndAlso _dtDetalleMaterial.Rows.Count > 0 Then
                With dbManager
                    .SqlParametros.Add("@idTipoSincronizacion", SqlDbType.TinyInt).Value = ID_TIPO_SINCRONIZACION
                    .SqlParametros.Add("@idLog", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    Try
                        .iniciarTransaccion()
                        .ejecutarNonQuery("RegistrarLogSincronizacionMaestro", CommandType.StoredProcedure)
                        Integer.TryParse(.SqlParametros("@idLog").Value.ToString, idLog)
                        If idLog <> 0 Then
                            Dim dcAux As New DataColumn("idLog", GetType(String))
                            dcAux.DefaultValue = idLog
                            _dtDetalleMaterial.Columns.Add(dcAux)
                            .TiempoEsperaComando = 1200
                            .inicilizarBulkCopy()
                            With .BulkCopy
                                .BulkCopyTimeout = 900
                                .BatchSize = 20000
                                .DestinationTableName = "LogSincronizacionMaestroMaterialDetalle"
                                .ColumnMappings.Add("idLog", "idLog")
                                .ColumnMappings.Add("material", "material")
                                .ColumnMappings.Add("descripcion", "descripcion")
                                .ColumnMappings.Add("centro", "centro")
                                .ColumnMappings.Add("tipoMaterial", "tipoMaterial")
                                .ColumnMappings.Add("clasificacion", "clasificacion")
                                .ColumnMappings.Add("codigoEan", "codigoEan")
                                .ColumnMappings.Add("fechaUltimaModificacion", "fechaUltimaModificacion")
                                .ColumnMappings.Add("tipoModificacion", "tipoModificacion")
                                .WriteToServer(_dtDetalleMaterial)
                            End With
                            .confirmarTransaccion()
                        Else
                            returnValue.Valor = 1
                            returnValue.Mensaje = "Ocurrió un error inesperado al tratar de generar Log de Sincronización. "
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
