Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.IO
Imports LMWebServiceSyncMonitorBusinessLayer
Imports ILSBusinessLayer.Comunes

Public Class ReplicaReprocesosSAP

#Region "Artributos (Campos)"

    Private _dtMaterialesCambio As DataTable
    Private _documentoSap As String
    Private _idTipoMovimiento As Integer
    Private _idInstruccionReproceso As Integer
    Private _fechaCreacion As DateTime
    Private _idUsuario As Integer
    Private _arrEntredaCab As New SAPContabilizacionEntrada.ZmmLgEntradasCab
    Private _arrMaterialesCambio() As SAPContabilizacionEntrada.ZmmLgMateriales
    Private _arrSerialesCambio() As SAPContabilizacionEntrada.ZmmLgSerialnumber
    Private _posContable As Integer
    Private listZmm As New List(Of SAPContabilizacionEntrada.ZmmLgMateriales)
    Private listZmmSer As New List(Of SAPContabilizacionEntrada.ZmmLgSerialnumber)
    Private _docCambioMaterial As Long
    Private _dtError As New DataTable
    Private _idOrdenEnvioLectura As Integer
    Private _idLecturaBodega As Integer
    Private _listaDocumentosMaterial As ArrayList
    Private _listaDocumentosRegion As ArrayList


#End Region

#Region "Propiedades"

    Public Property DtMaterialesCambio As DataTable
        Get
            Return _dtMaterialesCambio
        End Get
        Set(value As DataTable)
            _dtMaterialesCambio = value
        End Set
    End Property

    Public Property DocumentoSap As String
        Get
            Return _documentoSap
        End Get
        Set(value As String)
            _documentoSap = value
        End Set
    End Property

    Public Property IdTipoMovimiento As Integer
        Get
            Return _idTipoMovimiento
        End Get
        Set(value As Integer)
            _idTipoMovimiento = value
        End Set
    End Property

    Public Property IdInstruccionReproceso As Integer
        Get
            Return _idInstruccionReproceso
        End Get
        Set(value As Integer)
            _idInstruccionReproceso = value
        End Set
    End Property

    Public Property FechaCreacion As DateTime
        Get
            Return _fechaCreacion
        End Get
        Set(value As DateTime)
            _fechaCreacion = value
        End Set
    End Property

    Public Property IdUsuario As Integer
        Get
            Return _idUsuario
        End Get
        Set(value As Integer)
            _idUsuario = value
        End Set
    End Property

    Public Property DtError As DataTable
        Get
            Return _dtError
        End Get
        Set(value As DataTable)
            _dtError = value
        End Set
    End Property

    Public Property DocCambioMaterial As Long
        Get
            Return _docCambioMaterial
        End Get
        Set(value As Long)
            _docCambioMaterial = value
        End Set
    End Property

    Public Property IdOrdenEnvioLectura As Integer
        Get
            Return _idOrdenEnvioLectura
        End Get
        Set(value As Integer)
            _idOrdenEnvioLectura = value
        End Set
    End Property

    Public Property IdLecturaBodega As Integer
        Get
            Return _idLecturaBodega
        End Get
        Set(value As Integer)
            _idLecturaBodega = value
        End Set
    End Property

    Public Property ListaDocumentosMaterial As ArrayList
        Get
            If _listaDocumentosMaterial Is Nothing Then _listaDocumentosMaterial = New ArrayList
            Return _listaDocumentosMaterial
        End Get
        Set(value As ArrayList)
            _listaDocumentosMaterial = value
        End Set
    End Property

    Public Property ListaDocumentosRegion As ArrayList
        Get
            If _listaDocumentosRegion Is Nothing Then _listaDocumentosRegion = New ArrayList
            Return _listaDocumentosRegion
        End Get
        Set(value As ArrayList)
            _listaDocumentosRegion = value
        End Set
    End Property


#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _documentoSap = ""
        _dtError.Columns.Add(New DataColumn("tipo", GetType(String)))
        _dtError.Columns.Add(New DataColumn("Descripción", GetType(String)))

    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CambioMaterialRegion(drs As DataRow(), ByVal cabecera As String)
        'Throw New NotImplementedException
        Dim almacenOrigen As String = drs(0).Item("almacenOrigen").ToString
        Dim almacenDestino As String = drs(0).Item("almacenDestino").ToString
        Dim cantidad As Integer = drs.Length
        Dim centroOrigen As String = drs(0).Item("centroOrigen").ToString
        Dim centroDestino As String = drs(0).Item("centroDestino").ToString
        Dim materialOrigen As String = drs(0).Item("materialOrigen").ToString
        Dim materialDestino As String = drs(0).Item("materialDestino").ToString
        _posContable = _posContable + 1

        _arrEntredaCab.vale = cabecera
        _arrEntredaCab.textoCab = IdInstruccionReproceso

        Dim objSAPMaterial = New SAPContabilizacionEntrada.ZmmLgMateriales
        With objSAPMaterial
            .almacen = almacenOrigen
            .almacenRecept = almacenDestino
            .cantidad = cantidad
            .centro = centroOrigen
            .centroRecept = centroDestino
            .material = materialOrigen
            .materialRecept = materialDestino
            .posContable = _posContable
            listZmm.Add(objSAPMaterial)
        End With

        _arrMaterialesCambio = listZmm.ToArray()


        For index As Integer = 0 To drs.Length - 1
            Dim objSAPSerial = New SAPContabilizacionEntrada.ZmmLgSerialnumber
            Dim serial As String = drs(index).Item("serial").ToString
            With objSAPSerial
                .material = materialOrigen
                .noSerie = serial
                .posContable = _posContable
                listZmmSer.Add(objSAPSerial)
            End With
            _arrSerialesCambio = listZmmSer.ToArray()
        Next

    End Sub

    Private Function ExistenErroresCambio(ByVal oMensajes() As SAPContabilizacionEntrada.Bapiret2) As Boolean
        Dim hayErrores As Boolean = False

        If oMensajes.Length > 0 Then
            For index As Integer = 0 To oMensajes.Length - 1
                ' S=Sucessfully, E=error, A= abort, I = info, W = warning
                If oMensajes(index).type.ToUpper.Equals("E") OrElse oMensajes(index).type.ToUpper.Equals("A") Then
                    hayErrores = True
                    AgregarError(oMensajes(index).type.ToUpper, oMensajes(index).message)
                ElseIf oMensajes(index).type.ToUpper.Equals("S") OrElse oMensajes(index).type.ToUpper.Equals("I") Then
                    If oMensajes(index).message.StartsWith("Generado Doc.Material") Then
                        Long.TryParse(oMensajes(index).messageV1, _docCambioMaterial)
                    End If
                End If
            Next
        End If
        Return hayErrores
    End Function

    Private Function ExistenErroresConsultaSeriales(ByVal wsResultado As SAPConsultarSerial.OutputLgInvSeriales) As Boolean
        Dim hayErrores As Boolean = False

        With wsResultado
            If wsResultado IsNot Nothing Then
                If .oMensajes IsNot Nothing Then
                    If .oMensajes.Length > 0 Then
                        For index As Integer = 0 To wsResultado.oMensajes.Length - 1
                            ' S=Sucessfully, E=error, A= abort, I = info, W = warning
                            If .oMensajes(index).type.ToUpper.Equals("E") OrElse .oMensajes(index).type.ToUpper.Equals("A") Then
                                hayErrores = True
                                AgregarError(.oMensajes(index).type.ToUpper, .oMensajes(index).message)
                            End If
                        Next
                    Else
                        hayErrores = True
                    End If
                Else
                    Throw New Exception("Los seriales no se encontraron en SAP")
                End If
            Else
                hayErrores = True
            End If
        End With
        Return hayErrores
    End Function

    Private Sub AgregarError(ByVal tipo As String, ByVal descripcion As String)
        Dim dr As DataRow = _dtError.NewRow()
        dr("tipo") = tipo
        dr("Descripción") = descripcion
        _dtError.Rows.Add(dr)
        _dtError.AcceptChanges()
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function ValidacionDeCambiosMaterial() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim wsCambioMaterial As New SAPContabilizacionEntrada.WS_ENTRADAS_LG
        Dim infoWs As New InfoUrlWebService(wsCambioMaterial, True)
        Dim wsResultado As New SAPContabilizacionEntrada.OutputContabLg
        Dim cabecera As New ConfigValues("ENTRADA_CABECERA_CAMBIO_MAT")
        Dim tipoMov As New ConfigValues("TIPO_MOV_CAMBIO_MATERIAL")

        Dim dvAux As DataView = DtMaterialesCambio.DefaultView
        dvAux.RowFilter = "materialOrigen<>materialDestino"
        If dvAux.Count > 0 Then 'Hay un cambio de material
            Dim dtAux As DataTable = dvAux.ToTable(True, "materialDestino", "materialOrigen", "centroOrigen")
            Dim drs() As DataRow

            For Each dr As DataRow In dtAux.Rows
                drs = DtMaterialesCambio.Select("materialDestino='" & dr("materialDestino").ToString & "' AND centroOrigen= '" & dr("centroOrigen").ToString & _
                                                "' AND materialOrigen= '" & dr("materialOrigen").ToString & "'")
                CambioMaterialRegion(drs, cabecera.ConfigKeyValue)
            Next
            Dim obj As New ILSBusinessLayer.GeneradorCredencialesWebService
            wsCambioMaterial.Credentials = obj.Credenciales
            wsCambioMaterial.Timeout = 1200000
            wsResultado = wsCambioMaterial.executeZmmLgContabEntradas("R", tipoMov.ConfigKeyValue, _arrEntredaCab, Nothing, _arrMaterialesCambio, _arrSerialesCambio)
            _idTipoMovimiento = TipoMovimientoSap.CambioMaterial
        End If

        If wsResultado IsNot Nothing Then
            With wsResultado
                If .oMensajes IsNot Nothing Then
                    If ExistenErroresCambio(.oMensajes) Then
                        resultado.EstablecerMensajeYValor(1, "No fue posible realizar el cambio de material, por favor verifique el log de errores")
                    Else
                        resultado = RegistrarCambioSAP()
                    End If
                Else
                    resultado.EstablecerMensajeYValor(2, "No fue posible validar si se realizo el cambio de material, el proceso no retorno mensajes de confirmación.")
                End If
            End With
        Else
            resultado.EstablecerMensajeYValor(2, "No fue posible realizar el cambio del material, el proceso no retorno resultados")
        End If

        Return resultado
    End Function

    Public Function ValidacionDeCambiosRegion() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim wsCambioMaterial As New SAPContabilizacionEntrada.WS_ENTRADAS_LG
        Dim infoWs As New InfoUrlWebService(wsCambioMaterial, True)
        Dim wsResultado As New SAPContabilizacionEntrada.OutputContabLg
        Dim cabecera As New ConfigValues("ENTRADA_CABECERA_CAMBIO_REGION")
        Dim tipoMov As New ConfigValues("TIPO_MOV_CAMBIO_REGION")

        Dim dvAux As DataView = DtMaterialesCambio.DefaultView
        dvAux.RowFilter = "centroOrigen<>centroDestino"
        If dvAux.Count > 0 Then 'Hay un cambio de material
            Dim dtAux As DataTable = dvAux.ToTable(True, "centroDestino", "centroOrigen", "materialOrigen")
            Dim drs() As DataRow

            For Each dr As DataRow In dtAux.Rows
                drs = DtMaterialesCambio.Select("centroDestino='" & dr("centroDestino").ToString & "' AND centroOrigen= '" & dr("centroOrigen").ToString & _
                                                "' AND materialOrigen= '" & dr("materialOrigen").ToString & "'")
                CambioMaterialRegion(drs, cabecera.ConfigKeyValue)
            Next
            Dim obj As New ILSBusinessLayer.GeneradorCredencialesWebService
            wsCambioMaterial.Credentials = obj.Credenciales
            wsCambioMaterial.Timeout = 1200000
            wsResultado = wsCambioMaterial.executeZmmLgContabEntradas("R", tipoMov.ConfigKeyValue, _arrEntredaCab, Nothing, _arrMaterialesCambio, _arrSerialesCambio)
            _idTipoMovimiento = TipoMovimientoSap.CambioRegion
        End If

        If wsResultado IsNot Nothing Then
            With wsResultado
                If .oMensajes IsNot Nothing Then
                    If ExistenErroresCambio(.oMensajes) Then
                        resultado.EstablecerMensajeYValor(1, "No fue posible realizar el cambio de Región, por favor verifique el log de errores")
                    Else
                        resultado = RegistrarCambioSAP()
                    End If
                Else
                    resultado.EstablecerMensajeYValor(2, "No fue posible validar si se realizo el cambio de Región, el proceso no retorno mensajes de confirmación.")
                End If
            End With
        Else
            resultado.EstablecerMensajeYValor(2, "No fue posible realizar el cambio del Región, el proceso no retorno resultados")
        End If

        Return resultado
    End Function

    Public Function ValidacionCambiosStock() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim wsCambioMaterial As New SAPContabilizacionEntrada.WS_ENTRADAS_LG
        Dim infoWs As New InfoUrlWebService(wsCambioMaterial, True)
        Dim wsResultado As New SAPContabilizacionEntrada.OutputContabLg
        Dim cabecera As New ConfigValues("ENTRADA_CABECERA_CAMBIO_STOCK")
        Dim tipoMov As New ConfigValues("TIPO_MOV_CAMBIO_STOCK")

        Dim dvAux As DataView = DtMaterialesCambio.DefaultView
        dvAux.RowFilter = "centroOrigen=centroDestino AND materialOrigen = materialDestino "
        If dvAux.Count > 0 Then
            Dim dtAux As DataTable = dvAux.ToTable(True, "centroDestino", "centroOrigen", "materialOrigen", "materialDestino")
            Dim drs() As DataRow

            For Each dr As DataRow In dtAux.Rows
                drs = DtMaterialesCambio.Select("centroDestino='" & dr("centroDestino").ToString & "' AND centroOrigen= '" & dr("centroOrigen").ToString & _
                                                "' AND materialOrigen= '" & dr("materialOrigen").ToString & "' AND materialDestino= '" & dr("materialDestino").ToString & "'")
                CambioMaterialRegion(drs, cabecera.ConfigKeyValue)
            Next
            Dim obj As New ILSBusinessLayer.GeneradorCredencialesWebService
            wsCambioMaterial.Credentials = obj.Credenciales
            wsCambioMaterial.Timeout = 1200000
            wsResultado = wsCambioMaterial.executeZmmLgContabEntradas("R", tipoMov.ConfigKeyValue, _arrEntredaCab, Nothing, _arrMaterialesCambio, _arrSerialesCambio)

        End If

        If wsResultado IsNot Nothing Then
            With wsResultado
                If .oMensajes IsNot Nothing Then
                    If ExistenErroresCambio(.oMensajes) Then
                        resultado.EstablecerMensajeYValor(1, "No fue posible realizar el cambio de Stock, por favor verifique el log de errores")
                    Else
                        resultado.EstablecerMensajeYValor(0, "Se realizo el cambio de Stock correctamente. ")
                    End If
                Else
                    resultado.EstablecerMensajeYValor(2, "No fue posible validar si se realizo el cambio de Stock, el proceso no retorno mensajes de confirmación.")
                End If
            End With
        Else
            resultado.EstablecerMensajeYValor(2, "No fue posible realizar el cambio del Stock, el proceso no retorno resultados")
        End If

        Return resultado
    End Function

    Public Sub ConsultarSeriales(ByRef dtSeriales As DataTable)
        Dim wsConsultarSerial As New SAPConsultarSerial.WS_INV_SERIALES_LG
        Dim wsResultado As New SAPConsultarSerial.OutputLgInvSeriales
        Dim arrSeriales() As SAPConsultarSerial.Zsernr
        Dim maxIndex As Integer = dtSeriales.Rows.Count - 1
        Dim numElemento As Integer = -1
        Dim obj As New ILSBusinessLayer.GeneradorCredencialesWebService
        Dim hayErrores As Boolean
        Dim drAux As DataRow
        Dim numSerial As String
        Dim arrValidarRepetidos As New ArrayList

        wsConsultarSerial.Credentials = obj.Credenciales

        For index As Integer = 0 To maxIndex
            numElemento += 1
            ReDim Preserve arrSeriales(numElemento)
            arrSeriales(numElemento) = New SAPConsultarSerial.Zsernr
            arrSeriales(numElemento).sernr = dtSeriales.Rows(index).Item("serial")
            If (index + 1) Mod 5000 = 0 OrElse index = maxIndex Then
                wsConsultarSerial.Timeout = 600000
                wsResultado = wsConsultarSerial.executeZmmLgInventarioSeriales(Nothing, arrSeriales)
                hayErrores = False
                With wsResultado
                    If Not ExistenErroresConsultaSeriales(wsResultado) Then
                        If .oSeriales.Length > 0 Then
                            For indice As Integer = 0 To .oSeriales.Length - 1
                                If arrValidarRepetidos IsNot Nothing AndAlso arrValidarRepetidos.Contains(.oSeriales(indice).serial.Trim) Then
                                    drAux("inconsistencias") = "Esta repetido en SAP,"
                                    drAux("esValido") = False
                                Else
                                    arrValidarRepetidos.Add(.oSeriales(indice).serial.Trim)
                                    numSerial = ObtenerSerialConLongitudAjustada(.oSeriales(indice).serial.Trim)
                                    'numSerial = .oSeriales(indice).serial.Trim

                                    drAux = dtSeriales.Rows.Find(numSerial)

                                    If drAux IsNot Nothing Then
                                        drAux("esValido") = SerialEsValido(.oSeriales(indice), drAux)
                                    Else
                                        drAux("esValido") = False
                                    End If
                                End If
                            Next
                        Else
                            'resultado.EstablecerMensajeYValor(5, "Ninguno de los seriales asociados a la LB, fue encontrado en SAP")
                            AgregarError("E", "Ninguno de los seriales asociados a la LB, fue encontrado en SAP")
                        End If
                    End If
                End With
                numElemento = -1
            End If
        Next
    End Sub

#End Region

#Region "Métodos Compartidos"

    Public Function RegistrarCambioSAP() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim db As LMDataAccessLayer.LMDataAccess
        Try
            db = New LMDataAccess
            With db
                With .SqlParametros
                    .Clear()
                    .Add("@documentoSAP", SqlDbType.VarChar, 20).Value = _docCambioMaterial
                    .Add("@idTipoMoviento", SqlDbType.Int).Value = _idTipoMovimiento
                    .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    If _idInstruccionReproceso > 0 Then .Add("@idInstruccionReproceso", SqlDbType.Int).Value = _idInstruccionReproceso
                    If _idOrdenEnvioLectura > 0 Then .Add("@idOrdenEnvioLectura", SqlDbType.Int).Value = _idOrdenEnvioLectura
                    If _idLecturaBodega > 0 Then .Add("@idLecturaBodega", SqlDbType.Int).Value = _idLecturaBodega
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@mensaje", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output
                End With
                .ejecutarNonQuery("RegistrarDocumentoSAP", CommandType.StoredProcedure)
                If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                Else
                    resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta del servidor. Por favor intente nuevamente.")
                End If
            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
        Return resultado
    End Function

    Public Function RegistrarCambioSAPLB() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim db As LMDataAccessLayer.LMDataAccess
        Try
            db = New LMDataAccess
            With db
                With .SqlParametros
                    .Clear()
                    If _listaDocumentosMaterial IsNot Nothing AndAlso _listaDocumentosMaterial.Count > 0 Then _
                        .Add("@listaDocumentosMaterial", SqlDbType.VarChar).Value = Join(_listaDocumentosMaterial.ToArray(), ",")
                    If _listaDocumentosRegion IsNot Nothing AndAlso _listaDocumentosRegion.Count > 0 Then _
                        .Add("@listaDocumentosRegion", SqlDbType.VarChar).Value = Join(_listaDocumentosRegion.ToArray(), ",")
                    .Add("@idLecturaBodega", SqlDbType.Int).Value = _idLecturaBodega
                    .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@mensaje", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output
                End With
                .ejecutarNonQuery("RegistrarDocumentoSAPLB", CommandType.StoredProcedure)
                If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                Else
                    resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta del servidor. Por favor intente nuevamente.")
                End If
            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
        Return resultado
    End Function

    Private Function SerialEsValido(ByVal infoSerial As SAPConsultarSerial.ZmmLgInvSeriales, ByRef drAux As DataRow) As Boolean
        Dim esValido As Boolean = True
        Dim noValido As String = ""
        Dim material As Integer
        Dim centroOrigen As String
        Dim almacenOrigen As String
        
        If Integer.TryParse(infoSerial.material, material) Then
            drAux("materialOrigen") = material.ToString
        Else
            drAux("materialOrigen") = infoSerial.material
        End If

        If Integer.TryParse(infoSerial.centro, centroOrigen) Then
            drAux("centroOrigen") = centroOrigen.ToString
        Else
            drAux("centroOrigen") = infoSerial.centro
        End If

        If Integer.TryParse(infoSerial.almacen, almacenOrigen) Then
            drAux("almacenOrigen") = almacenOrigen.ToString
        Else
            drAux("almacenOrigen") = infoSerial.almacen
        End If

        noValido = drAux("inconsistencias").ToString()

        drAux("tipoStock") = infoSerial.tipoStock
        If Not infoSerial.tipoStock.Trim = "01" Then
            noValido &= "tipo Stock no es 01,"
            esValido = False
        End If

        If Not infoSerial.estado.Trim = "ALMA" Then
            noValido &= "estado no es ALMA,"
            esValido = False
        End If

        If noValido.Trim.Length > 0 Then drAux("inconsistencias") = noValido

        Return esValido
    End Function

    Public Function ObtenerSerialConLongitudAjustada(ByVal serial As String) As String
        Dim numSerial As String
        Dim InfoLongitudesSeriales As ArrayList

        If IsNumeric(serial) Then serial = CLng(serial).ToString
        Try
            Dim arrLongitudesPermitidas As ArrayList
            If InfoLongitudesSeriales Is Nothing Then
                arrLongitudesPermitidas = ObtenerLongitudesSeriales()
            Else
                arrLongitudesPermitidas = InfoLongitudesSeriales
            End If

            Dim diferencia As Integer = 0
            For index As Integer = 0 To arrLongitudesPermitidas.Count - 1
                If arrLongitudesPermitidas(index) = serial.Length Then
                    numSerial = serial
                    Exit For
                ElseIf arrLongitudesPermitidas(index) > serial.Length Then
                    diferencia = arrLongitudesPermitidas(index) - serial.Length
                    numSerial = Join(ArrayList.Repeat("0", diferencia).ToArray(), "") & serial.Trim
                    Exit For
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Error al tratar de ajustar la longitud de caracteres del serial, por favor contactar al proceso ITDEVELOPMENT")
        End Try
        Return numSerial
    End Function

    Private Function ObtenerLongitudesSeriales() As ArrayList
        Dim arrLongitudesPermitidas As ArrayList
        Try
            Dim cv As New ConfigValues("LONGITUDES_PERMITIDAS_SERIAL")
            arrLongitudesPermitidas = New ArrayList(cv.ConfigKeyValue.Split(","))
        Catch ex As Exception
            Throw New Exception("No fue posible obtener el listado de longitudes de seriales permitidad " & ex.Message)
        End Try

        Return arrLongitudesPermitidas
    End Function

#End Region

#Region "Enumerados"

    Public Enum TipoMovimientoSap
        CambioRegion = 1
        CambioMaterial = 2
        CambioMaterialRegion = 3
    End Enum

    Public Enum tipoCargueSap
        CargueInstruccionReproceso = 1
        CargueLecturaLB = 2
    End Enum

#End Region

    'Public Shared Function ObtenerSerialPrueba() As DataTable
    '    Dim db As New LMDataAccess
    '    Return db.ejecutarDataTable("SerialesCambiosSAP", CommandType.StoredProcedure)
    'End Function

End Class
