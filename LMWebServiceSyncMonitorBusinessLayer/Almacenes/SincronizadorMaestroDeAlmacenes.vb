Imports LMDataAccessLayer
Imports LMWebServiceSyncMonitorBusinessLayer.SAPMaestroAlmacenes

Public Class SincronizadorMaestroDeAlmacenes

#Region "Atributos (Campos)"
    Private ReadOnly ID_TIPO_SINCRONIZACION As Byte = 3
    Private _idLog As Integer
    Private _arrCentro() As SAPMaestroAlmacenes.WsorWerk
    Private _dtDetalleAlmacen As DataTable

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
                        _arrCentro(numReg) = New SAPMaestroAlmacenes.WsorWerk
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
            .Add("centro", GetType(String))
            .Add("almacen", GetType(String))
            .Add("descripcion", GetType(String))
        End With

        Return dtAux
    End Function

    Private Function ObtenerDatoASincronizar() As ClasesComunes.ResultadoProceso
        Dim returnValue As New ClasesComunes.ResultadoProceso
        Dim wsSyncAlmacen As New SAPMaestroAlmacenes.WS_MAESTRO_CENTRO_LG
        Dim resultado As New SAPMaestroAlmacenes.OutputLgMaestroCentro
        Dim genCredenciales As New ClasesComunes.GeneradorCredencialesWebService
        Dim infoWs As New InfoUrlWebService(wsSyncAlmacen, True)
        _dtDetalleAlmacen = CrearEstructuraDeTabla()
        wsSyncAlmacen.Timeout = 600000
        'CargarListaCentros()
        wsSyncAlmacen.Credentials = genCredenciales.Credenciales
        resultado = wsSyncAlmacen.executeZmmLgMaestroCentro(_arrCentro)
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
                        For index As Integer = 0 To .oCentros.Length - 1
                            drAux = _dtDetalleAlmacen.NewRow
                            drAux("centro") = .oCentros(index).centro
                            drAux("almacen") = .oCentros(index).almacen
                            drAux("descripcion") = .oCentros(index).descAlmacen
                            _dtDetalleAlmacen.Rows.Add(drAux)
                        Next
                    End If
                Else
                    Throw New Exception("Imposible obtener datos desde SAP. No se puede realizar la sincronización del maestro de Almacenes ")
                End If
            End With
        Else
            Throw New Exception("Imposible obtener datos desde SAP. No se puede realizar la sincronización del maestro de Almacenes  ")
        End If

        Return returnValue
    End Function

#End Region

#Region "Métodos Públicos"

    Public Function Sincronizar() As ClasesComunes.ResultadoProceso
        Dim returnValue As New ClasesComunes.ResultadoProceso
        Dim dbManager As New LMDataAccess
        returnValue = ObtenerDatoASincronizar()
        If returnValue.Valor = 0 Then
            If _dtDetalleAlmacen IsNot Nothing AndAlso _dtDetalleAlmacen.Rows.Count > 0 Then
                With dbManager
                    .SqlParametros.Add("@idTipoSincronizacion", SqlDbType.TinyInt).Value = ID_TIPO_SINCRONIZACION
                    .SqlParametros.Add("@idLog", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    Try
                        .iniciarTransaccion()
                        .ejecutarNonQuery("RegistrarLogSincronizacionMaestro", CommandType.StoredProcedure)
                        Integer.TryParse(.SqlParametros("@idLog").Value.ToString, _idLog)
                        If _idLog <> 0 Then
                            Dim dcAux As New DataColumn("idLog", GetType(Integer))
                            dcAux.DefaultValue = _idLog
                            _dtDetalleAlmacen.Columns.Add(dcAux)
                            .inicilizarBulkCopy()
                            With .BulkCopy
                                .DestinationTableName = "LogSincMaestroAlmacen"
                                .ColumnMappings.Add("idLog", "idLog")
                                .ColumnMappings.Add("centro", "centro")
                                .ColumnMappings.Add("almacen", "almacen")
                                .ColumnMappings.Add("descripcion", "descripcion")
                                .WriteToServer(_dtDetalleAlmacen)
                            End With
                            .confirmarTransaccion()
                            .SqlParametros.Clear()
                            .SqlParametros.Add("@idLog", SqlDbType.Int).Value = _idLog
                            .SqlParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                            .ejecutarNonQuery("SincronizarMaestroAlmacen", CommandType.StoredProcedure)
                            Dim resultado As Short = CShort(.SqlParametros("@returnValue").Value)
                            If resultado <> 0 Then Throw New Exception("Ocurrió un error inesperado al tratar de sincronizar Maestro de Almacenes.")
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
