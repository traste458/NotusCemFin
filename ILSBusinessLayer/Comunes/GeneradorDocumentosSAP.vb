Imports System.IO
Imports System.Text
Imports LMWebServiceSyncMonitorBusinessLayer

Public Class GeneradorDocumentosSAP

    Public Enum tipoDoc
        Remision = 1
        Factura = 2
        Material = 3
        Caja = 4
    End Enum

    Public Enum modoTratamientoDoc As Integer
        primerProceso = 1
        reimpresion = 2
    End Enum

#Region "Variables"
    Private _tipoDocumento As tipoDoc
    Private _modoTratamiento As modoTratamientoDoc
    Private _idDocumento As String
    Private _contadorIntentosGenerar As Integer
    Private _anioEjercicio As Integer
    Private _rutaDocumento As String
    Private _nombreArchivo As String
#End Region


#Region "Propiedades"
    Public Property NombreArchivo() As String
        Get
            Return _nombreArchivo
        End Get
        Set(ByVal value As String)
            _nombreArchivo = value
        End Set
    End Property

    Public ReadOnly Property RutaDocumento() As String
        Get
            Return _rutaDocumento
        End Get
    End Property

    Public Property TipoDocumento() As tipoDoc
        Get
            Return _tipoDocumento
        End Get
        Set(ByVal value As tipoDoc)
            _tipoDocumento = value
        End Set
    End Property

    Public Property idDocumento() As String
        Get
            Return _idDocumento
        End Get
        Set(ByVal value As String)
            _idDocumento = value
        End Set
    End Property

    Public Property ModoTratamiento() As modoTratamientoDoc
        Get
            Return _modoTratamiento
        End Get
        Set(ByVal value As modoTratamientoDoc)
            _modoTratamiento = value
        End Set
    End Property

    Private ReadOnly Property IdentificadorDocumento() As String
        Get
            Select Case _tipoDocumento
                Case 1 : Return "R"
                Case 2 : Return "F"
                Case 3 : Return "D"
                Case 4 : Return "C"
                Case Else : Return ""
            End Select
        End Get
    End Property

    Public Property AnioEjercicio() As Integer
        Get
            Return _anioEjercicio
        End Get
        Set(ByVal value As Integer)
            _anioEjercicio = value
        End Set
    End Property


#End Region

    Public Function GenerarDocumento(ByVal ruta As String) As ResultadoProceso
        Dim genResult As New ResultadoProceso
        Dim hayError As Boolean = False
        Dim adminDocumentoSAP As New SAPImpresionDocumentos.WS_PDF_LG
        Dim resultado As New SAPImpresionDocumentos.OutputLgPdf()
        Dim infoUrlWs As New InfoUrlWebService(adminDocumentoSAP, True)
        Dim retorno As String = ""
        _rutaDocumento = ruta & "\" & _nombreArchivo & IdentificadorDocumento & "_" & _idDocumento & ".pdf"
        _contadorIntentosGenerar += 1

        Do
            hayError = False
            Dim genCredencialesWS As New GeneradorCredencialesWebService()
            adminDocumentoSAP.Credentials = genCredencialesWS.Credenciales            
            resultado = adminDocumentoSAP.executeZmmLgGeneraPdf(Me.IdentificadorDocumento, _modoTratamiento, _idDocumento, _anioEjercicio)
            If resultado IsNot Nothing AndAlso resultado.oMensajes IsNot Nothing AndAlso resultado.oMensajes.Length > 0 Then
                If resultado.oPdf IsNot Nothing Then
                    If resultado.oMensajes IsNot Nothing Then
                        For indx As Integer = 0 To resultado.oMensajes.Length - 1
                            If genResult.Valor <> 0 Then genResult.EstablecerMensajeYValor(0, "")
                            If resultado.oMensajes(indx).type = "E" Or resultado.oMensajes(indx).type = "A" Then
                                hayError = True
                                With genResult
                                    .Valor = 3
                                    .Mensaje = "Error al tratar de obtener archivo asociado al documento: " & _
                                        idDocumento & " desde SAP: " & resultado.oMensajes(indx).message
                                End With
                                Exit For
                            ElseIf resultado.oMensajes(indx).message.Contains("Documento ya fue impreso") Then
                                hayError = True
                                genResult.EstablecerMensajeYValor(5, resultado.oMensajes(indx).message)
                            End If
                        Next
                    End If

                    'If resultado.oMensajes.Length > 0 AndAlso resultado.oMensajes(0).type <> "S" Then hayError = True
                    If Not hayError Then
                        Dim fs As New FileStream(_rutaDocumento, FileMode.Create)
                        Try
                            fs.Write(resultado.oPdf, 0, resultado.oPdf.Length)
                        Finally
                            fs.Dispose()
                        End Try
                    Else
                        If genResult.Valor = 5 Then _modoTratamiento = modoTratamientoDoc.reimpresion
                    End If
                Else
                    With genResult
                        .Valor = 1
                        .Mensaje = "No fue posible obtener el archivo desde SAP o no existe archivo"
                    End With
                End If
            Else
                With genResult
                    .Valor = 2
                    .Mensaje = "No se obtuvo respuesta por parte del Web Service. Por favor contacte a IT Development"
                End With
            End If
            _contadorIntentosGenerar += 1
        Loop While (hayError And _contadorIntentosGenerar < 3)

        Return genResult
    End Function
End Class
