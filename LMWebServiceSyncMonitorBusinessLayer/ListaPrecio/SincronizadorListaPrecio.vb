Imports LMDataAccessLayer
Imports LMWebServiceSyncMonitorBusinessLayer.SAPMaestroListaPrecio
Imports LMWebServiceSyncMonitorBusinessLayer.ClasesComunes

Public Class SincronizadorListaPrecio

#Region "Atributos (Campos)"
    Private ReadOnly ID_TIPO_SINCRONIZACION As Byte = 4
    Private _idSincronizacion As Integer
    Private _fechaModificacion As Date
    Private _arrCentro() As SAPMaestroListaPrecio.WsorWerk
    Private _dtListaPrecio As DataTable

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        CargarUltimaFechaSincronizacion()
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
                        _arrCentro(numReg) = New SAPMaestroListaPrecio.WsorWerk
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
            .Add("centro", GetType(Integer))
            .Add("material", GetType(Integer))
            .Add("tipoLista", GetType(String))
            .Add("valor", GetType(Double))
            .Add("tipoModificacion", GetType(String))
            .Add("fechaModificacion", GetType(Date))
            .Add("materialEquivalente", GetType(Integer))
        End With

        Return dtAux
    End Function

    Private Function ObtenerDatoASincronizar() As ResultadoProceso
        Dim returnValue As New ResultadoProceso
        Dim wsSyncListaPrecio As New SAPMaestroListaPrecio.WS_MAESTRO_LPRECIOS_LG
        Dim resultado As New SAPMaestroListaPrecio.OutputLgMaestroLprecios
        Dim strFechaModificacion As String = Nothing
        Dim genCredenciales As New GeneradorCredencialesWebService
        Dim infoWs As New InfoUrlWebService(wsSyncListaPrecio, True)
        
        _dtListaPrecio = CrearEstructuraDeTabla()
        'If _fechaModificacion > Date.MinValue Then strFechaModificacion = _fechaModificacion.ToString("yyyyMMddHHmmss")
        _fechaModificacion = Now.Date
        While _fechaModificacion.DayOfWeek <> DayOfWeek.Sunday
            _fechaModificacion = _fechaModificacion.AddDays(-1)
        End While
        wsSyncListaPrecio.Timeout = 1200000
        CargarListaCentros()
        wsSyncListaPrecio.Credentials = genCredenciales.Credenciales
        resultado = wsSyncListaPrecio.executeZmmLgMaestroLprecios(strFechaModificacion, _arrCentro)
        If resultado IsNot Nothing Then
            With resultado
                If resultado.oMensajes IsNot Nothing Then
                    Dim hayError As Boolean = False
                    For index As Integer = 0 To .oMensajes.Length - 1
                        If (.oMensajes(index).type = "E" Or .oMensajes(index).type = "A") AndAlso _
                            Not .oMensajes(index).message.Contains("No encontro precio del material equivalente") Then
                            returnValue.Valor = 1
                            returnValue.Mensaje = .oMensajes(index).message
                            hayError = True
                            Exit For
                        End If
                    Next
                    If Not hayError Then
                        Dim drAux As DataRow
                        Dim fechaAux As Date
                        Dim provider As System.Globalization.CultureInfo = System.Globalization.CultureInfo.InvariantCulture
                        Dim arrInfoTipoLista() As String
                        For index As Integer = 0 To .oLprecios.Length - 1
                            drAux = _dtListaPrecio.NewRow
                            drAux("centro") = .oLprecios(index).centro
                            drAux("material") = .oLprecios(index).material
                            arrInfoTipoLista = .oLprecios(index).tipoLista.Split("|")
                            If arrInfoTipoLista.Length > 1 Then
                                drAux("tipoLista") = arrInfoTipoLista(0)
                                If arrInfoTipoLista(1) IsNot Nothing Then drAux("materialEquivalente") = arrInfoTipoLista(1)
                            Else
                                drAux("tipoLista") = .oLprecios(index).tipoLista
                            End If
                            drAux("valor") = .oLprecios(index).valorUnitario
                            drAux("tipoModificacion") = .oLprecios(index).tipoModificacion
                            If Date.TryParseExact(resultado.oLprecios(index).fechaHoraModificacion, "yyyyMMddHHmmss", _
                                provider, Globalization.DateTimeStyles.None, fechaAux) Then drAux("fechaModificacion") = fechaAux
                            _dtListaPrecio.Rows.Add(drAux)
                        Next
                    End If
                Else
                    Throw New Exception("Imposible obtener datos desde SAP. No se puede realizar la sincronización de Lista de Precios ")
                End If
            End With
        Else
            Throw New Exception("Imposible obtener datos desde SAP. No se puede realizar la sincronización de Lista de Precios ")
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
            If _dtListaPrecio IsNot Nothing AndAlso _dtListaPrecio.Rows.Count > 0 Then
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
                            _dtListaPrecio.Columns.Add(dcAux)
                            .inicilizarBulkCopy()
                            With .BulkCopy
                                .DestinationTableName = "LogSincMaestroListaPrecio"
                                .ColumnMappings.Add("idLog", "idLog")
                                .ColumnMappings.Add("centro", "centro")
                                .ColumnMappings.Add("material", "material")
                                .ColumnMappings.Add("tipoLista", "tipoLista")
                                .ColumnMappings.Add("valor", "valor")
                                .ColumnMappings.Add("tipoModificacion", "tipoModificacion")
                                .ColumnMappings.Add("fechaModificacion", "fechaModificacion")
                                .ColumnMappings.Add("materialEquivalente", "materialEquivalente")
                                .WriteToServer(_dtListaPrecio)
                            End With
                            .SqlParametros.Clear()
                            .SqlParametros.Add("@idLog", SqlDbType.Int).Value = idLog
                            .SqlParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                            .ejecutarNonQuery("SincronizarMaestroListaPrecio", CommandType.StoredProcedure)
                            Dim resultado As Short = CShort(.SqlParametros("@returnValue").Value)
                            If resultado <> 0 Then Throw New Exception("Ocurrió un error inesperado al tratar de sincronizar Maestro de Lista de Precios.")
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
