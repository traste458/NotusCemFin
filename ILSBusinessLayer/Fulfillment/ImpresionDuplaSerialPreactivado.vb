Imports LMDataAccessLayer
Imports ZebraPrinting.ZebraLabels

Namespace Fulfillment

    Public Class ImpresionDuplaSerialPreactivado

#Region "Atributos"

        Private _infoDupla As GeneradorDuplaPreactivada

#End Region

#Region "Constructores"

        Public Sub New(ByVal serial As String)
            _infoDupla = New GeneradorDuplaPreactivada(serial)
        End Sub

        Public Sub New(ByVal infoDupla As GeneradorDuplaPreactivada)
            _infoDupla = infoDupla
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Sub Imprimir()
            If _infoDupla Is Nothing OrElse Not _infoDupla.Registrado Then Throw New Exception("No fue posible recuperar la información de la dupla a imprimir. Por favor intente nuevamente")
            Dim objPrint As ZebraPrint = Nothing

            Try
                objPrint = New ZebraPrint
                Dim comilla As String = Chr(34)
                Dim puerto As String = "LPT1"

                If Not EsNuloOVacio(ConfigurationManager.AppSettings("puertoImpresion")) Then _
                    puerto = ConfigurationManager.AppSettings("puertoImpresion").ToString

                With objPrint
                    .StartWrite(puerto)
                    .Write("q384")
                    .Write("S2")
                    .Write("N")
                    '.Write("A50,0,0,3,1,1,N," & comilla & "PROHIBIDA SU VENTA" & comilla)
                    .Write("A15,15,0,1,1,1,N," & comilla & "Serial: " & _infoDupla.Serial & comilla)
                    .Write("B15,35,0,1,2,8,35,N," & comilla & _infoDupla.Serial & comilla)
                    .Write("A15,75,0,3,1,1,N," & comilla & "Msisdn: " & _infoDupla.Msisdn & comilla)
                    .Write("B15,95,0,1,2,8,35,N," & comilla & _infoDupla.Msisdn & comilla)

                    'Algoritmo para escribir en varias líneas el nombre de la referencia,
                    'cuando el mismo supera el máximo número de caracteres permitidos por línea
                    Dim valorY As Integer = 135
                    Dim maxTamanio As Integer = 38
                    Dim referencia As String = _infoDupla.Referencia.Trim

                    While referencia.Length > maxTamanio
                        Dim arr() As String = referencia.Split(" ")
                        referencia = ""

                        For i As Integer = 0 To arr.GetUpperBound(0)
                            If (referencia & " " & arr(i)).Length > maxTamanio Then
                                .Write("A15," & valorY & ",0,1,1,1,N," & comilla & referencia & comilla)
                                referencia = arr(i).Trim
                                valorY += 15
                            Else
                                referencia += IIf(referencia.Length > 0, " ", "") + arr(i).Trim
                            End If
                        Next
                    End While
                    If Not EsNuloOVacio(referencia) Then
                        .Write("A15," & valorY & ",0,1,1,1,N," & comilla & referencia & comilla)
                        valorY += 15
                    End If
                    '**************************************************************************'
                    .Write("A15," & valorY & ",0,1,1,1,N," & comilla & "Material: " & _infoDupla.Material & comilla)
                    If Not EsNuloOVacio(_infoDupla.CodigoEan) Then
                        valorY += 15
                        .Write("A15," & valorY & ",0,1,1,1,N," & comilla & "Ean: " & _infoDupla.CodigoEan & comilla)
                    End If
                    .Write("P1")
                End With
            Finally
                objPrint.EndWrite()
            End Try
        End Sub

        Public Sub ImprimirEspecial()
            If _infoDupla Is Nothing OrElse Not _infoDupla.Registrado Then Throw New Exception("No fue posible recuperar la información de la dupla a imprimir. Por favor intente nuevamente")
            Dim objPrint As ZebraPrint = Nothing

            Try
                objPrint = New ZebraPrint
                Dim comilla As String = Chr(34)
                Dim puerto As String = "LPT1"

                If Not EsNuloOVacio(ConfigurationManager.AppSettings("puertoImpresion")) Then _
                    puerto = ConfigurationManager.AppSettings("puertoImpresion").ToString

                With objPrint
                    .StartWrite(puerto)
                    .Write("q384")
                    .Write("S2")
                    .Write("N")
                    '.Write("A30,0,0,4,1,1,N," & comilla & "PROHIBIDA SU VENTA" & comilla)
                    .Write("A30,15,0,3,1,1,N," & comilla & "Serial: " & _infoDupla.Serial & comilla)
                    .Write("B30,35,0,1,2,8,35,N," & comilla & _infoDupla.Serial & comilla)
                    .Write("A30,95,0,3,1,1,N," & comilla & "Imei: " & _infoDupla.Msisdn & comilla)
                    .Write("B30,115,0,1,2,8,35,N," & comilla & _infoDupla.Msisdn & comilla)

                    ''Algoritmo para escribir en varias líneas el nombre de la referencia,
                    ''cuando el mismo supera el máximo número de caracteres permitidos por línea
                    'Dim valorY As Integer = 135
                    'Dim maxTamanio As Integer = 38
                    'Dim referencia As String = _infoDupla.Referencia.Trim

                    'While referencia.Length > maxTamanio
                    '    Dim arr() As String = referencia.Split(" ")
                    '    referencia = ""

                    '    For i As Integer = 0 To arr.GetUpperBound(0)
                    '        If (referencia & " " & arr(i)).Length > maxTamanio Then
                    '            .Write("A15," & valorY & ",0,1,1,1,N," & comilla & referencia & comilla)
                    '            referencia = arr(i).Trim
                    '            valorY += 15
                    '        Else
                    '            referencia += IIf(referencia.Length > 0, " ", "") + arr(i).Trim
                    '        End If
                    '    Next
                    'End While
                    'If Not EsNuloOVacio(referencia) Then
                    '    .Write("A15," & valorY & ",0,1,1,1,N," & comilla & referencia & comilla)
                    '    valorY += 15
                    'End If
                    ''**************************************************************************'
                    '.Write("A15," & valorY & ",0,1,1,1,N," & comilla & "Material: " & _infoDupla.Material & comilla)
                    'If Not EsNuloOVacio(_infoDupla.CodigoEan) Then
                    '    valorY += 15
                    '    .Write("A15," & valorY & ",0,1,1,1,N," & comilla & "Ean: " & _infoDupla.CodigoEan & comilla)
                    'End If
                    .Write("P1")
                End With
            Finally
                objPrint.EndWrite()
            End Try
        End Sub

#End Region
    End Class

End Namespace


