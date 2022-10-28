Imports System.Drawing
Imports System.IO
Imports System.Drawing.Imaging

Public Class Imagen

#Region "Atributos"

    Private _anchoThumbnail As Integer
    Private _altoThumbnail As Integer

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _anchoThumbnail = 90
        _altoThumbnail = 60
    End Sub

#End Region

#Region "Métodos Privados"

#End Region

#Region "Métodos Públicos"

    Public Function Imagen_ArregloByte(ByRef imagenEntrada As Image, formato As ImageFormat) As Byte()
        Using ms As MemoryStream = New MemoryStream()
            imagenEntrada.Save(ms, formato)
            Return ms.ToArray()
        End Using
    End Function


    Public Function ArregloByte_Imagen(ByRef arregloEntrada As Byte()) As Image
        Using ms As MemoryStream = New MemoryStream(arregloEntrada)
            Return Image.FromStream(ms)
        End Using
    End Function

    Public Sub ArregloByte_Imagen(ByRef arregloEntrada As Byte(), ByVal nombreArchivo As String, ByVal contentType As String)
        Dim formato As ImageFormat
        Using ms As MemoryStream = New MemoryStream(arregloEntrada)
            Using img As Image = Image.FromStream(ms)
                Select Case contentType
                    Case "image/png"
                        formato = ImageFormat.Png
                    Case "image/gif"
                        formato = ImageFormat.Gif
                    Case Else
                        formato = ImageFormat.Jpeg
                End Select
                img.Save(nombreArchivo, formato)
            End Using
        End Using
    End Sub

    Public Function ArregloByte_ImagenThumbnail(ByRef arregloEntrada As Byte()) As Image
        Using ms As MemoryStream = New MemoryStream(arregloEntrada)
            Return Image.FromStream(ms).GetThumbnailImage(_anchoThumbnail, _altoThumbnail, Nothing, 0)
        End Using
    End Function

    Public Sub ArregloByte_ImagenThumbnail(ByRef arregloEntrada As Byte(), ByVal nombreArchivo As String, ByVal contentType As String)
        Dim formato As ImageFormat
        Using ms As MemoryStream = New MemoryStream(arregloEntrada)
            Using img As Image = Image.FromStream(ms).GetThumbnailImage(_anchoThumbnail, _altoThumbnail, Nothing, 0)
                Select Case contentType
                    Case "image/png"
                        formato = ImageFormat.Png
                    Case "image/gif"
                        formato = ImageFormat.Gif
                    Case Else
                        formato = ImageFormat.Jpeg
                End Select
                img.Save(nombreArchivo, formato)
            End Using
        End Using
    End Sub

#End Region

End Class
