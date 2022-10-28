Imports LMWebServiceSyncMonitorBusinessLayer
Namespace Comunes

    Public Class ConsultarSerialesSAP

#Region "Variables"

        Private _infoSeriales As DataTable
        Private _infoErrores As DataTable
        Private _arrLongitudesPermitidas As ArrayList

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
#End Region

#Region "Contructores"

        Public Sub New()
            MyBase.New()
            _infoErrores = ObtenerEstructuraErrores()
            ObtenerLongitudesSeriales()
        End Sub

#End Region

#Region "Método Público"

        Public Function ConsultarSerialesEnSAP() As ResultadoProceso
            Dim resultadoEjecucion As New ResultadoProceso
            If _infoSeriales IsNot Nothing AndAlso _infoSeriales.Rows.Count > 0 Then
                Dim wsConsultarSerial As New SAPConsultarSerial.WS_INV_SERIALES_LG
                Dim infoWs As New InfoUrlWebService(wsConsultarSerial, True)
                Dim wsResultado As New SAPConsultarSerial.OutputLgInvSeriales
                Dim arrSeriales() As SAPConsultarSerial.Zsernr
                Dim maxIndex As Integer = _infoSeriales.Rows.Count - 1
                Dim numElemento As Integer = -1
                Dim obj As New ILSBusinessLayer.GeneradorCredencialesWebService
                Dim drAux() As DataRow
                Dim numSerial As String
                wsConsultarSerial.Credentials = obj.Credenciales
                Dim arrValidarRepetidos As New ArrayList

                resultadoEjecucion.EstablecerMensajeYValor(0, "Ejecución Satisfactoria")

                If Not _infoSeriales.Columns.Contains("tipoStock") Then _infoSeriales.Columns.Add("tipoStock", GetType(String))
                If Not _infoSeriales.Columns.Contains("material_SAP") Then _infoSeriales.Columns.Add("material_SAP", GetType(String))
                If Not _infoSeriales.Columns.Contains("centro_SAP") Then _infoSeriales.Columns.Add("centro_SAP", GetType(String))
                If Not _infoSeriales.Columns.Contains("almacen_SAP") Then _infoSeriales.Columns.Add("almacen_SAP", GetType(String))
                If Not _infoSeriales.Columns.Contains("estado_SAP") Then _infoSeriales.Columns.Add("estado_SAP", GetType(String))
                AdicionarColumnaDataTable(_infoSeriales, "existeEnSAP", "System.Boolean", "False")
                AdicionarColumnaDataTable(_infoSeriales, "centroPropio", "System.Boolean", "False")

                For index As Integer = 0 To maxIndex
                    numElemento += 1
                    ReDim Preserve arrSeriales(numElemento)
                    arrSeriales(numElemento) = New SAPConsultarSerial.Zsernr
                    arrSeriales(numElemento).sernr = _infoSeriales.Rows(index).Item("serial")
                    If (index + 1) Mod 5000 = 0 OrElse index = maxIndex Then
                        wsConsultarSerial.Timeout = 600000
                        wsResultado = wsConsultarSerial.executeZmmLgInventarioSeriales(Nothing, arrSeriales)
                        With wsResultado
                            resultadoEjecucion = ExistenErroresConsultaSeriales(wsResultado)

                            If resultadoEjecucion.Valor <> 0 Then Return resultadoEjecucion

                            Dim material As Integer

                            For indice As Integer = 0 To .oSeriales.Length - 1
                                If arrValidarRepetidos IsNot Nothing AndAlso arrValidarRepetidos.Contains(.oSeriales(indice).serial.Trim) Then
                                    drAux(0)("inconsistencias") = "Esta repetido en SAP,"
                                Else
                                    arrValidarRepetidos.Add(.oSeriales(indice).serial.Trim)

                                    numSerial = ObtenerSerialConLongitudAjustada(.oSeriales(indice).serial.Trim)
                                    drAux = _infoSeriales.Select("serial = '" & numSerial & "'")
                                    If drAux IsNot Nothing AndAlso drAux.Length > 0 Then
                                        drAux(0)("existeEnSAP") = True
                                        drAux(0)("tipoStock") = .oSeriales(indice).tipoStock
                                        drAux(0)("centro_SAP") = .oSeriales(indice).centro
                                        drAux(0)("almacen_SAP") = .oSeriales(indice).almacen
                                        drAux(0)("estado_SAP") = .oSeriales(indice).estado
                                        If Integer.TryParse(.oSeriales(indice).material, material) Then
                                            drAux(0)("material_SAP") = material.ToString
                                        Else
                                            drAux(0)("material_SAP") = .oSeriales(indice).material
                                        End If
                                    End If
                                End If
                            Next
                        End With
                        numElemento = -1
                    End If
                Next

                ValidarCentroLMSeriales()
            Else
                resultadoEjecucion.EstablecerMensajeYValor(1, "No se suministraron los seriales para realizar la consulta en SAP.")
            End If

            Return resultadoEjecucion
        End Function

#End Region

#Region "Métodos Privados"

        Private Function ExistenErroresConsultaSeriales(ByVal wsResultado As SAPConsultarSerial.OutputLgInvSeriales) As ResultadoProceso
            Dim hayErrores As Boolean = False
            Dim resultadoEjecucion As New ResultadoProceso

            resultadoEjecucion.EstablecerMensajeYValor(0, "Ejecución Satisfactoria. ")
            With wsResultado
                If wsResultado IsNot Nothing Then
                    If .oMensajes IsNot Nothing Then
                        If .oMensajes.Length > 0 Then
                            Dim drDato As DataRow
                            For index As Integer = 0 To wsResultado.oMensajes.Length - 1
                                ' S=Sucessfully, E=error, A= abort, I = info, W = warning
                                If .oMensajes(index).type.ToUpper.Equals("E") OrElse .oMensajes(index).type.ToUpper.Equals("A") Then
                                    hayErrores = True
                                    drDato = _infoErrores.NewRow
                                    drDato("tipo") = .oMensajes(index).type.ToUpper
                                    drDato("descripcion") = .oMensajes(index).message
                                    _infoErrores.Rows.Add(drDato)
                                End If
                            Next

                            If Not hayErrores Then
                                If .oSeriales Is Nothing Or .oSeriales.Length = 0 Then
                                    resultadoEjecucion.EstablecerMensajeYValor(5, "No se encontraron los seriales en SAP.")
                                End If
                            Else
                                resultadoEjecucion.EstablecerMensajeYValor(4, "Se presentaron errores al consultar los seriales en SAP. ")
                            End If
                        Else
                            resultadoEjecucion.EstablecerMensajeYValor(3, "Se presentaron errores al consultar los seriales en SAP. ")
                        End If
                    Else
                        resultadoEjecucion.EstablecerMensajeYValor(2, "La consulta de los seriales en SAP no retorno información.")
                    End If
                Else
                    resultadoEjecucion.EstablecerMensajeYValor(1, "No fue posible realizar la consulta de los seriales en SAP. ")
                End If
            End With

            Return resultadoEjecucion
        End Function

        Private Function ObtenerSerialConLongitudAjustada(ByVal serial As String) As String
            Dim numSerial As String

            If IsNumeric(serial) Then serial = CLng(serial).ToString

            Try
                Dim diferencia As Integer = 0
                For index As Integer = 0 To _arrLongitudesPermitidas.Count - 1
                    If _arrLongitudesPermitidas(index) = serial.Length Then
                        numSerial = serial
                        Exit For
                    ElseIf _arrLongitudesPermitidas(index) > serial.Length Then
                        diferencia = _arrLongitudesPermitidas(index) - serial.Length
                        numSerial = Join(ArrayList.Repeat("0", diferencia).ToArray(), "") & serial.Trim
                        Exit For
                    End If
                Next
            Catch ex As Exception
                Throw New Exception("Error al tratar de ajustar la longitud de caracteres del serial, por favor contactar al Administrador")
            End Try
            Return numSerial
        End Function

        Private Sub ObtenerLongitudesSeriales()
            Try
                Dim cv As New ConfigValues("LONGITUDES_PERMITIDAS_SERIAL")
                _arrLongitudesPermitidas = New ArrayList(cv.ConfigKeyValue.Split(","))
            Catch ex As Exception
                Throw New Exception("No fue posible obtener el listado de longitudes de seriales permitidos " & ex.Message)
            End Try
        End Sub

        Private Function ObtenerEstructuraErrores() As DataTable
            Dim dtAux As New DataTable
            With dtAux.Columns
                .Add("tipo", GetType(String))
                .Add("descripcion", GetType(String))
            End With
            Return dtAux
        End Function

        Private Sub AdicionarColumnaDataTable(ByRef dt As DataTable, ByVal nombreColumna As String, ByVal tipoDato As String, Optional ByVal valorDefecto As String = "")
            Dim dcDato As New DataColumn(nombreColumna, Type.GetType(tipoDato))
            dcDato.DefaultValue = valorDefecto

            If Not dt.Columns.Contains(nombreColumna) Then dt.Columns.Add(dcDato)

            If dcDato IsNot Nothing Then dcDato.Dispose()
        End Sub

        Private Sub ValidarCentroLMSeriales()
            Dim dtRegion As DataTable
            Dim filtro As String

            dtRegion = ObtenerRegiones()

            If dtRegion IsNot Nothing AndAlso dtRegion.Rows.Count > 0 Then
                For i As Integer = 0 To dtRegion.Rows.Count - 1
                    filtro &= dtRegion.Rows(i)("centro") & ","
                Next

                If filtro.Trim.Length > 0 And filtro.EndsWith(",") Then
                    filtro = "centro_SAP IN (" & filtro.Substring(0, filtro.Length - 1) & ")"
                End If

                Dim dr() As DataRow
                dr = _infoSeriales.Select(filtro)

                If dr IsNot Nothing AndAlso dr.Length > 0 Then

                    For j As Integer = 0 To dr.Length - 1
                        dr(j)("centroPropio") = True
                    Next
                End If
            End If
        End Sub

        Private Function ObtenerRegiones() As DataTable
            Dim dt As New DataTable
            Try
                dt = Region.ObtenerTodas
            Catch ex As Exception
                Throw New Exception("Error al tratar de obtener el listado de regiones para validar los centros LM. " & ex.Message)
            End Try
            Return dt
        End Function


#End Region

    End Class

End Namespace