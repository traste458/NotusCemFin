Imports LMDataAccessLayer
Imports ZebraPrinting.ZebraLabels

Namespace Fulfillment

    Public Class ImpresionImeiConTarjetaRaspeGane

#Region "Atributos"

        Private _imei As String
        Private _producto As String
        Private _subproducto As String
        Private _material As String
        Private _consecutivo As String
        Private _codigoEan As String
        Private _codigoHomologacion As String
        Private _cargado As Boolean
        Private _fechaProduccion As Date
        Private _secuencia As Integer
        Private _unidadesCaja As Integer
        Private _caja As Short
        Private _orden As String

#End Region

#Region "Constructores"

        Public Sub New(ByVal imei As String)
            _imei = imei
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Public ReadOnly Property Cargado As Boolean
            Get
                Return _cargado
            End Get
        End Property

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            If Not EsNuloOVacio(Me._imei) AndAlso Not _cargado Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@imei", SqlDbType.VarChar, 20).Value = Me._imei.Trim
                        .ejecutarReader("ConsultarInfoImpresionSerialConTarjetaRaspeGane", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing AndAlso .Reader.Read Then
                            _imei = .Reader("imei").ToString
                            _producto = .Reader("producto").ToString
                            _subproducto = .Reader("subproducto").ToString
                            _material = .Reader("material").ToString
                            _consecutivo = .Reader("consecutivo").ToString
                            _codigoEan = .Reader("codigoEan").ToString
                            _codigoHomologacion = .Reader("codigoHomologacion").ToString
                            Date.TryParse(.Reader("fechaProduccion").ToString, _fechaProduccion)
                            _secuencia = .Reader("secuencia").ToString
                            Integer.TryParse(.Reader("unidadesCaja").ToString, _unidadesCaja)
                            Integer.TryParse(.Reader("caja").ToString, _caja)
                            _orden = .Reader("orden").ToString
                        End If
                        _cargado = True
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Sub Imprimir()
            Dim objPrint As ZebraPrint = Nothing
            CargarDatos()
            Try
                objPrint = New ZebraPrint
                Dim comilla As String = Chr(34)
                Dim puerto As String = "LPT1"
                Dim infoAdicional As String

                infoAdicional = IIf(_fechaProduccion > Date.MinValue, "F.P:" & _fechaProduccion.ToString("dd/MM/yyyy") & "    ", "") & _
                    "TEL: " & _secuencia.ToString.Trim & "/" & _unidadesCaja.ToString.Trim & " - " & _caja.ToString & "    " & _orden.Trim

                If Not EsNuloOVacio(ConfigurationManager.AppSettings("puertoImpresion")) Then _
                    puerto = ConfigurationManager.AppSettings("puertoImpresion").ToString

                With objPrint
                    .StartWrite(puerto)
                    .Write("q592")
                    .Write("S2")
                    .Write("N")
                    .Write("A20,10,0,3,1,1,N," & comilla & Me._producto.Trim.ToUpper & comilla)
                    If Me._subproducto.Trim.Length <= 35 Then
                        .Write("A20,35,0,4,1,1,N," & comilla & Me._subproducto.Trim.ToUpper & comilla)
                    Else
                        .Write("A30,35,0,1,1,3,N," & comilla & Me._subproducto.Trim.ToUpper & comilla)
                    End If
                    .Write("A20,65,0,4,1,1,N," & comilla & "MATERIAL" & comilla)
                    .Write("A160,65,0,4,1,1,N," & comilla & Me._material.Trim.ToUpper & comilla)
                    .Write("A20,95,0,3,1,1,N," & comilla & "IMEI" & comilla)
                    .Write("B120,100,0,1,2,8,40,B," & comilla & Me._imei.Trim & comilla)
                    If IsNumeric(Me._consecutivo) Then
                        .Write("A20,175,0,3,1,1,N," & comilla & "COD. TAR" & comilla)
                        .Write("B145,180,0,1,2,8,40,B," & comilla & CLng(Me._consecutivo).ToString("00000") & comilla)
                    Else
                        .Write("A20,175,0,3,1,1,N," & comilla & "PIN" & comilla)
                        .Write("B145,180,0,1,2,8,40,B," & comilla & Me._consecutivo.Trim & comilla)
                    End If
                    If Not EsNuloOVacio(Me._codigoEan) Then
                        .Write("A505,240,3,3,1,1,N," & comilla & "EAN" & comilla)
                        .Write("B525,295,3,1,2,2,40,B," & comilla & Me._codigoEan.Trim & comilla)
                    End If
                    If Not EsNuloOVacio(Me._codigoHomologacion) Then
                        .Write("A350,240,0,2,1,2,N," & comilla & "C.H:" & Me._codigoHomologacion.Trim & comilla)
                    End If
                    .Write("A20,280,0,1,1,2,N," & comilla & infoAdicional.Trim & comilla)
                    .Write("P1")
                End With
            Finally
                objPrint.EndWrite()
            End Try

        End Sub

#End Region

    End Class

End Namespace