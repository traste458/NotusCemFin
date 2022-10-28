Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.IO
Imports LMWebServiceSyncMonitorBusinessLayer
Imports ILSBusinessLayer.Comunes

Public Class EquiposRecuperadosSAP

#Region "Artributos (Campos)"

    Private _dtError As New DataTable

#End Region

#Region "Propiedades"

    Public Property DtError As DataTable
        Get
            Return _dtError
        End Get
        Set(value As DataTable)
            _dtError = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _dtError.Columns.Add(New DataColumn("tipo", GetType(String)))
        _dtError.Columns.Add(New DataColumn("Descripción", GetType(String)))
    End Sub

#End Region

#Region "Metodos Privados"
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
            wsConsultarSerial.Timeout = 600000
            wsResultado = wsConsultarSerial.executeZmmLgInventarioSeriales(Nothing, arrSeriales)
            hayErrores = False
            With wsResultado
                If Not ExistenErroresConsultaSeriales(wsResultado) Then
                    If .oSeriales.Length > 0 Then
                        For indice As Integer = 0 To .oSeriales.Length - 1
                            numSerial = .oSeriales(indice).serial.Trim
                            drAux = dtSeriales.Rows.Find(numSerial)
                            If drAux IsNot Nothing Then
                                CargarInformacionSerial(.oSeriales(indice), drAux)
                            End If
                        Next
                    Else
                        AgregarError("E", "Ninguno de los seriales, fue encontrado en SAP")
                    End If
                End If
            End With
            numElemento = -1
        Next
    End Sub

#End Region

#Region "Metodos Compartidos"

    Private Sub CargarInformacionSerial(ByVal infoSerial As SAPConsultarSerial.ZmmLgInvSeriales, ByRef drAux As DataRow) 'As Boolean
        Dim estadoSAP As Integer = 0
        drAux("ubicacionSAP") = infoSerial.centro & " - " & infoSerial.almacen
        drAux("estatusSAP") = infoSerial.estado.ToString
        If Integer.TryParse(infoSerial.tipoStock, estadoSAP) Then
            drAux("estadoSAP") = estadoSAP.ToString
        End If
    End Sub

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

    Public Function ObtenerSerialConLongitudAjustada(ByVal serial As String) As String
        Dim numSerial As String = ""
        Dim InfoLongitudesSeriales As New ArrayList

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

#End Region

End Class
