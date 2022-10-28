Imports System.Drawing
Imports System.Drawing.Imaging

Namespace CodeBar39

    Public Class Code39Settings
        Private height As Integer = 80

        Public Property BarCodeHeight() As Integer
            Get
                Return height
            End Get
            Set(value As Integer)
                height = value
            End Set
        End Property

        Private m_drawText As Boolean = True

        Public Property DrawText() As Boolean
            Get
                Return m_drawText
            End Get
            Set(value As Boolean)
                m_drawText = value
            End Set
        End Property

        Private m_leftMargin As Integer = 10

        Public Property LeftMargin() As Integer
            Get
                Return m_leftMargin
            End Get
            Set(value As Integer)
                m_leftMargin = value
            End Set
        End Property

        Private m_rightMargin As Integer = 10

        Public Property RightMargin() As Integer
            Get
                Return m_rightMargin
            End Get
            Set(value As Integer)
                m_rightMargin = value
            End Set
        End Property

        Private m_topMargin As Integer = 10

        Public Property TopMargin() As Integer
            Get
                Return m_topMargin
            End Get
            Set(value As Integer)
                m_topMargin = value
            End Set
        End Property

        Private m_bottomMargin As Integer = 10

        Public Property BottomMargin() As Integer
            Get
                Return m_bottomMargin
            End Get
            Set(value As Integer)
                m_bottomMargin = value
            End Set
        End Property

        Private m_interCharacterGap As Integer = 2

        Public Property InterCharacterGap() As Integer
            Get
                Return m_interCharacterGap
            End Get
            Set(value As Integer)
                m_interCharacterGap = value
            End Set
        End Property

        Private m_wideWidth As Integer = 6

        Public Property WideWidth() As Integer
            Get
                Return m_wideWidth
            End Get
            Set(value As Integer)
                m_wideWidth = value
            End Set
        End Property

        Private m_narrowWidth As Integer = 2

        Public Property NarrowWidth() As Integer
            Get
                Return m_narrowWidth
            End Get
            Set(value As Integer)
                m_narrowWidth = value
            End Set
        End Property

        Private m_font As New Font(FontFamily.GenericSansSerif, 12)

        Public Property Font() As Font
            Get
                Return m_font
            End Get
            Set(value As Font)
                m_font = value
            End Set
        End Property

        Private codeToTextGapHeight As Integer = 10

        Public Property BarCodeToTextGapHeight() As Integer
            Get
                Return codeToTextGapHeight
            End Get
            Set(value As Integer)
                codeToTextGapHeight = value
            End Set
        End Property
    End Class

    Public Class Code39

#Region "Static initialization"

        Shared codes As Dictionary(Of Char, Pattern)

        Shared Sub New()
            Dim chars As Object()() = New Object()() {New Object() {Asc("0"c), "n n n w w n w n n"}, _
                                                      New Object() {Asc("1"c), "w n n w n n n n w"}, _
                                                      New Object() {Asc("2"c), "n n w w n n n n w"}, _
                                                      New Object() {Asc("3"c), "w n w w n n n n n"}, _
                                                      New Object() {Asc("4"c), "n n n w w n n n w"}, _
                                                      New Object() {Asc("5"c), "w n n w w n n n n"}, _
                                                      New Object() {Asc("6"c), "n n w w w n n n n"}, _
                                                      New Object() {Asc("7"c), "n n n w n n w n w"}, _
                                                      New Object() {Asc("8"c), "w n n w n n w n n"}, _
                                                      New Object() {Asc("9"c), "n n w w n n w n n"}, _
                                                      New Object() {Asc("A"c), "w n n n n w n n w"}, _
                                                      New Object() {Asc("B"c), "n n w n n w n n w"}, _
                                                      New Object() {Asc("C"c), "w n w n n w n n n"}, _
                                                      New Object() {Asc("D"c), "n n n n w w n n w"}, _
                                                      New Object() {Asc("E"c), "w n n n w w n n n"}, _
                                                      New Object() {Asc("F"c), "n n w n w w n n n"}, _
                                                      New Object() {Asc("G"c), "n n n n n w w n w"}, _
                                                      New Object() {Asc("H"c), "w n n n n w w n n"}, _
                                                      New Object() {Asc("I"c), "n n w n n w w n n"}, _
                                                      New Object() {Asc("J"c), "n n n n w w w n n"}, _
                                                      New Object() {Asc("K"c), "w n n n n n n w w"}, _
                                                      New Object() {Asc("L"c), "n n w n n n n w w"}, _
                                                      New Object() {Asc("M"c), "w n w n n n n w n"}, _
                                                      New Object() {Asc("N"c), "n n n n w n n w w"}, _
                                                      New Object() {Asc("O"c), "w n n n w n n w n"}, _
                                                      New Object() {Asc("P"c), "n n w n w n n w n"}, _
                                                      New Object() {Asc("Q"c), "n n n n n n w w w"}, _
                                                      New Object() {Asc("R"c), "w n n n n n w w n"}, _
                                                      New Object() {Asc("S"c), "n n w n n n w w n"}, _
                                                      New Object() {Asc("T"c), "n n n n w n w w n"}, _
                                                      New Object() {Asc("U"c), "w w n n n n n n w"}, _
                                                      New Object() {Asc("V"c), "n w w n n n n n w"}, _
                                                      New Object() {Asc("W"c), "w w w n n n n n n"}, _
                                                      New Object() {Asc("X"c), "n w n n w n n n w"}, _
                                                      New Object() {Asc("Y"c), "w w n n w n n n n"}, _
                                                      New Object() {Asc("Z"c), "n w w n w n n n n"}, _
                                                      New Object() {Asc("-"c), "n w n n n n w n w"}, _
                                                      New Object() {Asc("."c), "w w n n n n w n n"}, _
                                                      New Object() {Asc(" "c), "n w w n n n w n n"}, _
                                                      New Object() {Asc("*"c), "n w n n w n w n n"}, _
                                                      New Object() {Asc("$"c), "n w n w n w n n n"}, _
                                                      New Object() {Asc("/"c), "n w n w n n n w n"}, _
                                                      New Object() {Asc("+"c), "n w n n n w n w n"}, _
                                                      New Object() {Asc("%"c), "n n n w n w n w n"}}

            codes = New Dictionary(Of Char, Pattern)()
            For Each c As Object() In chars
                codes.Add(Chr(c(0)), Pattern.Parse(DirectCast(c(1), String)))
            Next
        End Sub

#End Region

        Private Shared pen As New Pen(Color.Black)
        Private Shared brush As Brush = Brushes.Black

        Private code As String
        Private settings As Code39Settings

        Public Sub New(code As String)
            Me.New(code, New Code39Settings())
        End Sub

        Public Sub New(code As String, settings As Code39Settings)
            For Each c As Char In code
                If Not codes.ContainsKey(c) Then
                    Throw New ArgumentException("Invalid character encountered in specified code.")
                End If
            Next

            If Not code.StartsWith("*") Then
                code = "*" & code
            End If
            If Not code.EndsWith("*") Then
                code = code & "*"
            End If

            Me.code = code
            Me.settings = settings
        End Sub

        Public Function Paint() As Bitmap
            Dim code As String = Me.code.Trim("*"c)

            Dim sizeCodeText As SizeF = Graphics.FromImage(New Bitmap(1, 1)).MeasureString(code, settings.Font)

            Dim w As Integer = settings.LeftMargin + settings.RightMargin
            For Each c As Char In Me.code
                w += codes(c).GetWidth(settings) + settings.InterCharacterGap
            Next
            w -= settings.InterCharacterGap

            Dim h As Integer = settings.TopMargin + settings.BottomMargin + settings.BarCodeHeight

            If settings.DrawText Then
                h += settings.BarCodeToTextGapHeight + CInt(sizeCodeText.Height)
            End If

            Dim bmp As New Bitmap(w, h, PixelFormat.Format32bppArgb)
            Dim g As Graphics = Graphics.FromImage(bmp)

            Dim left As Integer = settings.LeftMargin

            For Each c As Char In Me.code
                left += codes(c).Paint(settings, g, left) + settings.InterCharacterGap
            Next

            If settings.DrawText Then
                Dim tX As Integer = settings.LeftMargin + (w - settings.LeftMargin - settings.RightMargin - CInt(sizeCodeText.Width)) / 2
                If tX < 0 Then
                    tX = 0
                End If
                Dim tY As Integer = settings.TopMargin + settings.BarCodeHeight + settings.BarCodeToTextGapHeight
                g.DrawString(code, settings.Font, brush, tX, tY)
            End If

            Return bmp
        End Function

        Private Class Pattern
            Private nw As Boolean() = New Boolean(8) {}

            Public Shared Function Parse(s As String) As Pattern
                Debug.Assert(s IsNot Nothing)

                s = s.Replace(" ", "").ToLower()

                Debug.Assert(s.Length = 9)
                Debug.Assert(s.Replace("n", "").Replace("w", "").Length = 0)

                Dim p As New Pattern()

                Dim i As Integer = 0
                For Each c As Char In s
                    p.nw(i) = c = "w"c
                    i += 1
                Next

                Return p
            End Function

            Public Function GetWidth(settings As Code39Settings) As Integer
                Dim width As Integer = 0

                For i As Integer = 0 To 8
                    width += (If(nw(i), settings.WideWidth, settings.NarrowWidth))
                Next

                Return width
            End Function

            Public Function Paint(settings As Code39Settings, g As Graphics, left As Integer) As Integer
                '#If DEBUG Then
                '            Dim gray As New Rectangle(left, 0, GetWidth(settings), settings.BarCodeHeight + settings.TopMargin + settings.BottomMargin)
                '            g.FillRectangle(Brushes.Gray, gray)
                '#End If
                Dim x As Integer = left

                Dim w As Integer = 0
                For i As Integer = 0 To 8
                    Dim width As Integer = (If(nw(i), settings.WideWidth, settings.NarrowWidth))

                    If i Mod 2 = 0 Then
                        Dim r As New Rectangle(x, settings.TopMargin, width, settings.BarCodeHeight)
                        g.FillRectangle(brush, r)
                    End If

                    x += width
                    w += width
                Next

                Return w
            End Function

        End Class

    End Class

End Namespace
