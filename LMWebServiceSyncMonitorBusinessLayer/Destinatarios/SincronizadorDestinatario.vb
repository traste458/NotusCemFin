Imports LMDataAccessLayer
Imports LMWebServiceSyncMonitorBusinessLayer.SAPMaestroDestinatarios
Imports LMWebServiceSyncMonitorBusinessLayer.ClasesComunes

Public Class SincronizadorDestinatario

#Region "Atributos (Campos)"
    Private ReadOnly ID_TIPO_SINCRONIZACION As Byte = 2
    Private _idLog As Integer
    Private _fecha As Date
    Private _dtDestinatario As DataTable

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable

        With dtAux.Columns
            .Add("codigo", GetType(String))
            .Add("nombre", GetType(String))
            .Add("ciudad", GetType(String))
            .Add("departamento", GetType(String))
            .Add("pais", GetType(String))
            .Add("direccion", GetType(String))
            .Add("nombreContacto", GetType(String))
            .Add("emailContacto", GetType(String))
            .Add("telefono", GetType(String))
            .Add("centro", GetType(String))
            .Add("almacen", GetType(String))
            .Add("codigoPadre", GetType(String))
            .Add("dealer", GetType(String))
            .Add("tipoModificacion", GetType(String))
            .Add("fechaModificacion", GetType(String))
        End With

        Return dtAux
    End Function

    Private Function ObtenerDatoASincronizar() As ResultadoProceso
        Dim returnValue As New ResultadoProceso
        Dim wsSyncDestinatario As New WS_MAESTRO_DESTINATARIO_LG
        Dim resultado As New OutputLgMaestroDestinatario
        Dim strFecha As String = Nothing
        Dim genCredenciales As New GeneradorCredencialesWebService
        Dim infoWs As New InfoUrlWebService(wsSyncDestinatario, True)

        _dtDestinatario = CrearEstructuraDeTabla()
        wsSyncDestinatario.Timeout = 600000
        'If _fecha > Date.MinValue Then strFecha = _fecha.ToString("yyyyMMdd")
        wsSyncDestinatario.Credentials = genCredenciales.Credenciales
        resultado = wsSyncDestinatario.executeZmmLgMaestroDestinatario(strFecha)
        If resultado IsNot Nothing Then
            With resultado
                If resultado.oMensajes IsNot Nothing Then
                    Dim hayError As Boolean = False
                    Dim sbAux As New System.Text.StringBuilder
                    For index As Integer = 0 To .oMensajes.Length - 1
                        If .oMensajes(index).type = "E" Or .oMensajes(index).type = "A" Then
                            returnValue.Valor = 1
                            returnValue.Mensaje = .oMensajes(index).message
                            hayError = True
                            Exit For
                        ElseIf .oMensajes(index).type = "I" Then
                            sbAux.Append(.oMensajes(index).message & vbCrLf)
                        End If
                    Next
                    returnValue.Mensaje = sbAux.ToString
                    If Not hayError Then
                        Dim drAux As DataRow
                        Dim arrInfoCiudad() As String
                        For index As Integer = 0 To .oDestinatarios.Length - 1
                            drAux = _dtDestinatario.NewRow

                            drAux("codigo") = .oDestinatarios(index).codigo
                            drAux("nombre") = .oDestinatarios(index).nombre
                            arrInfoCiudad = .oDestinatarios(index).ciudad.Split("|")
                            If arrInfoCiudad IsNot Nothing AndAlso arrInfoCiudad.GetUpperBound(0) >= 0 Then
                                drAux("ciudad") = arrInfoCiudad(0)
                                If arrInfoCiudad.Length >= 2 Then drAux("departamento") = arrInfoCiudad(1)
                                If arrInfoCiudad.Length >= 3 Then drAux("pais") = arrInfoCiudad(2)
                            End If
                            drAux("direccion") = .oDestinatarios(index).direccion
                            drAux("nombreContacto") = .oDestinatarios(index).nombreContacto
                            drAux("emailContacto") = .oDestinatarios(index).emailContacto
                            drAux("telefono") = .oDestinatarios(index).telefono
                            drAux("centro") = .oDestinatarios(index).centro
                            drAux("almacen") = .oDestinatarios(index).almacen
                            drAux("codigoPadre") = .oDestinatarios(index).padre
                            drAux("dealer") = .oDestinatarios(index).dealer
                            drAux("tipoModificacion") = .oDestinatarios(index).tipoModificacion
                            drAux("fechaModificacion") = .oDestinatarios(index).fechaHoraModificacion
                            _dtDestinatario.Rows.Add(drAux)
                        Next
                    End If
                Else
                    Throw New Exception("Imposible obtener datos desde SAP. No se puede realizar la sincronización de Destinatarios ")
                End If
            End With
        Else
            Throw New Exception("Imposible obtener datos desde SAP. No se puede realizar la sincronización de Destinatarios")
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
            If _dtDestinatario IsNot Nothing AndAlso _dtDestinatario.Rows.Count > 0 Then
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
                            _dtDestinatario.Columns.Add(dcAux)
                            .inicilizarBulkCopy()
                            With .BulkCopy
                                .DestinationTableName = "LogSincMaestroDestinatario"
                                .ColumnMappings.Add("idLog", "idLog")
                                .ColumnMappings.Add("codigo", "codigo")
                                .ColumnMappings.Add("nombre", "nombre")
                                .ColumnMappings.Add("ciudad", "ciudad")
                                .ColumnMappings.Add("departamento", "departamento")
                                .ColumnMappings.Add("pais", "pais")
                                .ColumnMappings.Add("direccion", "direccion")
                                .ColumnMappings.Add("nombreContacto", "nombreContacto")
                                .ColumnMappings.Add("emailContacto", "emailContacto")
                                .ColumnMappings.Add("telefono", "telefono")
                                .ColumnMappings.Add("centro", "centro")
                                .ColumnMappings.Add("almacen", "almacen")
                                .ColumnMappings.Add("codigoPadre", "codigoPadre")
                                .ColumnMappings.Add("dealer", "dealer")
                                .ColumnMappings.Add("tipoModificacion", "tipoModificacion")
                                .ColumnMappings.Add("fechaModificacion", "fechaModificacion")
                                
                                .WriteToServer(_dtDestinatario)
                            End With

                            .confirmarTransaccion()

                            .SqlParametros.Clear()
                            .SqlParametros.Add("@idLog", SqlDbType.Int).Value = _idLog
                            .SqlParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                            .ejecutarNonQuery("SincronizarMaestroDestinatarios", CommandType.StoredProcedure)
                            Dim resultado As Short = CShort(.SqlParametros("@returnValue").Value)
                            If resultado <> 0 Then Throw New Exception("Ocurrió un error inesperado al tratar de sincronizar el maestro de Destinatarios.")
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
