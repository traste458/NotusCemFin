Imports ILSBusinessLayer.Estructuras
Imports LMWebServiceSyncMonitorBusinessLayer

Namespace Comunes

    Public Class CambioDeEstadoSAP

#Region "Variables"

        Private _arrEntredaCab As SAPContabilizacionEntrada.ZmmLgEntradasCab
        Private _arrMaterialesCambio() As SAPContabilizacionEntrada.ZmmLgMateriales
        Private _arrSerialesCambio() As SAPContabilizacionEntrada.ZmmLgSerialnumber
        Private _infoSeriales As DataTable
        Private _infoMateriales As DataTable
        Private _infoErrores As DataTable
        Private _idPedido As Integer
        Const TIPODOCUMENTO As String = "R"
        Private _valeMaterial As String
        Private _textoCabecera As String
        Private _stockDestino As TipoStock
        Private _stockOrigen As TipoStock
        Private _tipoCambio As Tipo
        Private _centroCambio As String
        Private _almacenCambio As String
        Private _documentoSAP As String
        Private _campoDocCambioRegion As String = ""

#End Region

#Region "Propiedades"

        Public Property InfoSeriales() As DataTable
            Get
                Return _infoSeriales
            End Get
            Set(ByVal value As DataTable)
                _infoSeriales = value
            End Set
        End Property

        Public ReadOnly Property InfoErrores() As DataTable
            Get
                Return _infoErrores
            End Get
        End Property

        Public ReadOnly Property InfoMateriales() As DataTable
            Get
                Return _infoMateriales
            End Get
        End Property

        Public Property IdPedido() As Integer
            Get
                Return _idPedido
            End Get
            Set(ByVal value As Integer)
                _idPedido = value
            End Set
        End Property

        Public Property StockOrigen() As TipoStock
            Get
                Return _stockOrigen
            End Get
            Set(ByVal value As TipoStock)
                _stockOrigen = value
            End Set
        End Property

        Public Property StockDestino() As TipoStock
            Get
                Return _stockDestino
            End Get
            Set(ByVal value As TipoStock)
                _stockDestino = value
            End Set
        End Property

        Public Property TipoCambio() As Tipo
            Get
                Return _tipoCambio
            End Get
            Set(ByVal value As Tipo)
                _tipoCambio = value
            End Set
        End Property

        Public Property CentroCambio() As String
            Get
                Return _centroCambio
            End Get
            Set(ByVal value As String)
                _centroCambio = value
            End Set
        End Property

        Public Property AlmacenCambio() As String
            Get
                Return _almacenCambio
            End Get
            Set(ByVal value As String)
                _almacenCambio = value
            End Set
        End Property

        Public Property ValeMaterial() As String
            Get
                Return _valeMaterial
            End Get
            Set(ByVal value As String)
                _valeMaterial = value
            End Set
        End Property

        Public Property TextoCabecera() As String
            Get
                Return _textoCabecera
            End Get
            Set(ByVal value As String)
                _textoCabecera = value
            End Set
        End Property

        Public Property DocumentoSAP() As String
            Get
                Return _documentoSAP
            End Get
            Set(ByVal value As String)
                _documentoSAP = value
            End Set
        End Property

        Public Property CampoDocCambioRegion() As String
            Get
                Return _campoDocCambioRegion
            End Get
            Set(ByVal value As String)
                _campoDocCambioRegion = value
            End Set
        End Property

#End Region

#Region "Contructores"

        Public Sub New()
            MyBase.New()
            _infoErrores = ObtenerEstructuraErrores()
        End Sub

#End Region

#Region "Método Público"

        Public Function GenerarCambio() As ResultadoProceso
            Dim wsCambioMaterial As New SAPContabilizacionEntrada.WS_ENTRADAS_LG
            Dim infoWs As New InfoUrlWebService(wsCambioMaterial, True)
            Dim wsResultado As New SAPContabilizacionEntrada.OutputContabLg
            Dim objCredencialesWS As New GeneradorCredencialesWebService()
            Dim resultadoEjecucion As New ResultadoProceso
            Dim hayErrores As Boolean = False
            If (_infoSeriales IsNot Nothing AndAlso _infoSeriales.Rows.Count > 0) Then
                ' Adicion de columna para almacenar documento en seriales que tienen cambio de region
                If _campoDocCambioRegion.Trim.Length = 0 Then
                    _campoDocCambioRegion = "documentoCambioRegion"
                End If
                Dim dcDato As New DataColumn(_campoDocCambioRegion, Type.GetType("System.String"))
                If Not _infoSeriales.Columns.Contains(_campoDocCambioRegion) Then _infoSeriales.Columns.Add(dcDato)

                _infoMateriales = ObtenerMaterialesCambioSAP(_infoSeriales)
                If (_infoMateriales IsNot Nothing AndAlso _infoMateriales.Rows.Count > 0) Then

                    resultadoEjecucion = ValidarDiferenciaRegion()

                    If resultadoEjecucion.Valor = 0 Then
                        Dim clasemovimiento As Integer
                        clasemovimiento = ObtenerTipoMovimiento()

                        If clasemovimiento > 0 Then
                            wsCambioMaterial.Credentials = objCredencialesWS.Credenciales
                            wsCambioMaterial.Timeout = 1200000

                            resultadoEjecucion.EstablecerMensajeYValor(0, "Ejecución Satisfactoria")

                            ' Actualiza la información para realizar el cambio
                            resultadoEjecucion = ObtenerInfoCambio()
                            If resultadoEjecucion.Valor <> 0 Then Return resultadoEjecucion

                            wsResultado = wsCambioMaterial.executeZmmLgContabEntradas(TIPODOCUMENTO, clasemovimiento, _arrEntredaCab, Nothing, _arrMaterialesCambio, _arrSerialesCambio)

                            If wsResultado IsNot Nothing Then
                                With wsResultado
                                    If .oMensajes IsNot Nothing Then
                                        If .oMensajes.Length > 0 Then
                                            For index As Integer = 0 To .oMensajes.Length - 1
                                                ' S=Sucessfully, E=error, A= abort, I = info, W = warning
                                                If .oMensajes(index).type.ToUpper.Equals("E") OrElse .oMensajes(index).type.ToUpper.Equals("A") Then
                                                    hayErrores = True
                                                    AgregarError(.oMensajes(index).type.ToUpper, .oMensajes(index).message)
                                                Else
                                                    'If .oMensajes(index).message.StartsWith("Generado Doc.Material") Then
                                                    _documentoSAP = .oMensajes(index).messageV1.Trim
                                                    If _documentoSAP.Trim.Length = 0 Then
                                                        resultadoEjecucion.EstablecerMensajeYValor(9, "No fue posible Obtener el documento de cambio de estado en SAP para la cuarentena.")
                                                    End If
                                                    'End If
                                                End If
                                            Next

                                            If hayErrores Then
                                                With resultadoEjecucion
                                                    If _infoErrores IsNot Nothing AndAlso _infoErrores.Rows.Count > 0 Then
                                                        resultadoEjecucion.EstablecerMensajeYValor(1, "No fue posible realizar el cambio de estado en SAP, por favor verificar el log de errores.")
                                                    Else
                                                        resultadoEjecucion.EstablecerMensajeYValor(1, "No fue posible realizar el cambio de estado en SAP.")
                                                    End If
                                                End With
                                            End If
                                        Else
                                            resultadoEjecucion.EstablecerMensajeYValor(2, "No se encontraron mensajes relacionados con el cambio en SAP. ")
                                        End If
                                    Else
                                        resultadoEjecucion.EstablecerMensajeYValor(3, "No fue posible validar si se realizo el cambio de estado en SAP.")
                                    End If
                                End With
                            Else
                                resultadoEjecucion.EstablecerMensajeYValor(4, "No fue posible realizar el cambio de estado en SAP.")
                            End If
                        Else
                            resultadoEjecucion.EstablecerMensajeYValor(5, "No fue posible obtener el tipo de movimiento para el cambio en SAP")
                        End If
                    Else
                        resultadoEjecucion.EstablecerMensajeYValor(6, "No fue posible validar si existen seriales con region distinta a la del cambio de estado en SAP")
                    End If
                Else
                    resultadoEjecucion.EstablecerMensajeYValor(7, "No fue posible obtener la lista materiales para realizar el cambio")
                End If
            Else
                resultadoEjecucion.EstablecerMensajeYValor(8, "No fue posible obtener la lista de seriales para realizar el cambio")
            End If

            Return resultadoEjecucion
        End Function

        Public Function ObtenerInfoCambio() As ResultadoProceso
            Dim filtro As String = ""
            Dim cantidad As Integer = 0
            ReDim Preserve _arrMaterialesCambio(_infoMateriales.Rows.Count - 1)
            ReDim Preserve _arrSerialesCambio(_infoSeriales.Rows.Count - 1)
            Dim resultadoEjecucion As New ResultadoProceso

            If Not _infoMateriales.Columns.Contains("posContable") Then _infoMateriales.Columns.Add("posContable", GetType(Integer))

            _arrEntredaCab = New SAPContabilizacionEntrada.ZmmLgEntradasCab
            _arrEntredaCab.vale = _valeMaterial
            _arrEntredaCab.textoCab = _textoCabecera

            resultadoEjecucion.Valor = 0
            resultadoEjecucion.Mensaje = "Ejecución Satisfactoria"

            Dim centro As String = ""
            Dim almacen As String = ""

            For index As Integer = 0 To _infoMateriales.Rows.Count - 1
                _arrMaterialesCambio(index) = New SAPContabilizacionEntrada.ZmmLgMateriales
                _infoMateriales.Rows(index)("posContable") = index + 1
                _arrMaterialesCambio(index).posContable = index + 1
                centro = _infoMateriales.Rows(index)("centro_SAP").ToString()
                almacen = _infoMateriales.Rows(index)("almacen_SAP").ToString()

                _arrMaterialesCambio(index).centro = centro
                _arrMaterialesCambio(index).almacen = almacen

                _arrMaterialesCambio(index).material = _infoMateriales.Rows(index)("material_SAP").ToString()

                filtro = "material_SAP = '" & _infoMateriales.Rows(index)("material_SAP").ToString() & _
                "' AND centro_SAP = '" & centro & "' AND almacen_SAP = '" & almacen & "'"

                Integer.TryParse(_infoSeriales.Compute("COUNT(serial)", filtro), cantidad)
                _arrMaterialesCambio(index).cantidad = cantidad
                _arrMaterialesCambio(index).posDocumento = "0000"
            Next

            Dim drAux() As DataRow
            For i As Integer = 0 To _infoSeriales.Rows.Count - 1
                _arrSerialesCambio(i) = New SAPContabilizacionEntrada.ZmmLgSerialnumber
                filtro = "material_SAP = '" & _infoSeriales.Rows(i)("material_SAP").ToString() & _
                "' AND centro_SAP = '" & _infoSeriales.Rows(i)("centro_SAP").ToString() & "' AND almacen_SAP = '" & _infoSeriales.Rows(i)("almacen_SAP").ToString() & "'"

                drAux = _infoMateriales.Select(filtro)
                If drAux IsNot Nothing AndAlso drAux.Length > 0 Then
                    _arrSerialesCambio(i).posContable = CInt(drAux(0)("posContable").ToString())
                Else
                    resultadoEjecucion.EstablecerMensajeYValor(4, "No fue posible asignar la posicion contable para realizar el cambio de Estado")

                    Return resultadoEjecucion
                End If
                _arrSerialesCambio(i).material = _infoSeriales.Rows(i)("material_SAP").ToString
                _arrSerialesCambio(i).noSerie = _infoSeriales.Rows(i)("serial").ToString
            Next

            Return resultadoEjecucion
        End Function
#End Region

#Region "Métodos Privados"

        Private Function ObtenerTipoMovimiento() As Integer
            Dim cms As New ClaseMovimientoStockSAP()
            Dim filtro As New FiltroClaseMovimientoStockSAP
            Dim infoTipo As DataTable
            Dim tipoMovimiento As Integer = 0

            With cms
                filtro.StockOrigen = CInt(_stockOrigen)
                filtro.StockDestino = CInt(_stockDestino)
                infoTipo = .ObtenerListado(filtro)
            End With

            If infoTipo IsNot Nothing AndAlso infoTipo.Rows.Count > 0 Then
                tipoMovimiento = infoTipo(0)("movimiento").ToString() 'drAux(0)("movimiento").ToString
            End If

            Return tipoMovimiento
        End Function

        Private Function ObtenerMaterialesCambioSAP(ByVal dtInfoSeriales As DataTable) As DataTable
            Dim dv As New DataView(dtInfoSeriales)

            Return dv.ToTable(True, New String() {"material_SAP", "centro_SAP", "almacen_SAP"})
        End Function

        ''' <summary>
        ''' Registra los errores encontrados en las validaciones y los guarda en un datatable
        ''' </summary>
        ''' <param name="linea">Linea del archivo en donde se dió el error</param>
        ''' <param name="descripcion">descripción del error ocurrido</param>
        Private Sub AgregarError(ByVal tipo As String, ByVal descripcion As String)
            Dim dr As DataRow = _infoErrores.NewRow()
            dr("tipo") = tipo
            dr("descripcion") = descripcion
            _infoErrores.Rows.Add(dr)
        End Sub

        Private Function ObtenerEstructuraErrores() As DataTable
            Dim dtAux As New DataTable
            With dtAux.Columns
                .Add("tipo", GetType(String))
                .Add("descripcion", GetType(String))
            End With
            Return dtAux
        End Function

        Private Function ValidarDiferenciaRegion() As ResultadoProceso
            Dim rs As New ResultadoProceso

            rs.EstablecerMensajeYValor(0, "Ejecución Satisfactoria")
            If _tipoCambio = Tipo.Cuarentena Or _tipoCambio = Tipo.EnvioPrueba Then
                Dim dtInfoSerialesCambioRegion As DataTable
                Dim dtMaterialCambioRegion As DataTable
                Dim filtro As String = ""

                filtro = "(centro_SAP <> " & _centroCambio & " OR almacen_SAP <> " & _almacenCambio & _
                ") AND (LEN(serial) <> 17)"
                '" AND (serial NOT LIKE '571010%' AND serial NOT LIKE '571011%' AND serial NOT LIKE '571012%')"

                dtInfoSerialesCambioRegion = AplicarFiltro(_infoSeriales, filtro)

                If dtInfoSerialesCambioRegion IsNot Nothing AndAlso dtInfoSerialesCambioRegion.Rows.Count > 0 Then
                    dtMaterialCambioRegion = ObtenerMaterialesCambioSAP(dtInfoSerialesCambioRegion)
                    If dtMaterialCambioRegion IsNot Nothing AndAlso dtMaterialCambioRegion.Rows.Count > 0 Then
                        Dim cr As New CambioRegionSAP
                        With cr
                            .ValeMaterial = "CAMBIO DE REGION"
                            .TextoCabecera = _textoCabecera
                            .CentroDestino = _centroCambio
                            .AlmacenDestino = _almacenCambio
                            .InfoSeriales = dtInfoSerialesCambioRegion
                            rs = .GenerarCambio()
                            If rs.Valor = 0 Then
                                ActualizarRegionSeriales(dtInfoSerialesCambioRegion, .DocumentoSAP)
                            Else
                                _infoErrores = .InfoErrores
                            End If
                        End With
                    Else
                        rs.EstablecerMensajeYValor(1, "No fue posible obtener los materiales para realizar el cambio de region de los seriales en SAP.")
                    End If
                End If
            End If

            Return rs
        End Function

        Private Sub ActualizarRegionSeriales(ByVal dtDatos As DataTable, ByVal documentoSAPCambioRegion As String)
            Dim drAux() As DataRow
            Dim serial As String = ""
            For index As Integer = 0 To dtDatos.Rows.Count - 1
                serial = dtDatos.Rows(index)("serial").ToString

                drAux = _infoSeriales.Select("serial = '" & serial & "'")
                If drAux IsNot Nothing And drAux.Length > 0 Then
                    drAux(0)("centro_SAP") = _centroCambio
                    drAux(0)("almacen_SAP") = _almacenCambio
                    drAux(0)(_campoDocCambioRegion) = documentoSAPCambioRegion
                End If
            Next

            _infoSeriales.AcceptChanges()

            _infoMateriales = ObtenerMaterialesCambioSAP(_infoSeriales)
        End Sub

        ''' <summary>
        ''' Realiza el filtro de datos en un data table
        ''' </summary>
        ''' <param name="dt">datos a filtrar</param>
        ''' <param name="filtro">filtro realizado</param>
        ''' <returns>datatable filtrado</returns>
        ''' <remarks></remarks>
        Private Function AplicarFiltro(ByVal dt As DataTable, ByVal filtro As String) As DataTable
            dt.DefaultView.RowFilter = filtro
            Return dt.DefaultView.ToTable()
        End Function

#End Region

#Region "Enums"

        Public Enum Tipo
            Cuarentena = 1
            LiberacionCuarentena
            EnvioPrueba
        End Enum

        Public Enum TipoStock
            LibreUtilizacion = 1
            ControlCalidad = 2
            Bloqueado = 7
        End Enum
#End Region

    End Class

End Namespace