Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text

Namespace ZebraLabels
    ''' <summary>
    ''' This interface expose all methods implemented to de COM world
    ''' 
    ''' Programmer: José Vélez Correa
    ''' </summary>
    Public Interface AxLMZebraPrinting
        ''' <summary>
        ''' This function must be called first.  Printer path must be a COM Port or a UNC path.
        ''' </summary>
        Sub StartWrite(ByVal printerPath As String)

        ''' <summary> 
        ''' This will write a command to the printer. 
        ''' </summary> 
        Sub Write(ByVal rawLine As String)

        ''' <summary> 
        ''' This will write a command to the printer. 
        ''' </summary> 
        Sub WriteWithEncoding(ByVal rawLine As String)
    
        ''' <summary> 
        ''' This function must be called after writing to the zebra printer. 
        ''' </summary> 
        Sub EndWrite()
    End Interface

    ''' <summary>
    ''' This class can print zebra labels to either a network share, LPT, or COM port.
    ''' 
    ''' Programmer: Rick Chronister
    ''' </summary>
    ''' <remarks>Only tested for network share, but in theory works for LPT and COM.</remarks>
    Public Class ZebraPrint
        Implements AxLMZebraPrinting

#Region " Private constants "
        Public Const GENERIC_READ As Integer = &H80000000
        Private Const GENERIC_WRITE As Integer = &H40000000
        Private Const OPEN_EXISTING As Integer = 3
        Private Const FILE_SHARE_READ = &H1
        Private Const FILE_SHARE_WRITE = &H2
        Private Const FILE_ATTRIBUTE_NORMAL As Integer = &H80

#End Region

#Region " Private members "
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Private _SafeFileHandle As Microsoft.Win32.SafeHandles.SafeFileHandle
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Private _fileWriter As StreamWriter
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Private _outFile As FileStream
#End Region

#Region " private structures "
        ''' <summary>
        ''' Structure for CreateFile.  Used only to fill requirement
        ''' </summary>
        <StructLayout(LayoutKind.Sequential)> _
       Public Structure SECURITY_ATTRIBUTES
            Private nLength As Integer
            Private lpSecurityDescriptor As Integer
            Private bInheritHandle As Integer
        End Structure
#End Region

#Region " com calls "
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="lpFileName"></param>
        ''' <param name="dwDesiredAccess"></param>
        ''' <param name="dwShareMode"></param>
        ''' <param name="lpSecurityAttributes"></param>
        ''' <param name="dwCreationDisposition"></param>
        ''' <param name="dwFlagsAndAttributes"></param>
        ''' <param name="hTemplateFile"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Declare Function CreateFile Lib "kernel32" Alias "CreateFileA" (ByVal lpFileName As String, ByVal dwDesiredAccess As FileAccess, ByVal dwShareMode As Integer, <MarshalAs(UnmanagedType.Struct)> ByRef lpSecurityAttributes As SECURITY_ATTRIBUTES, ByVal dwCreationDisposition As Integer, ByVal dwFlagsAndAttributes As Integer, ByVal hTemplateFile As Integer) As Microsoft.Win32.SafeHandles.SafeFileHandle
#End Region

#Region " Public methods "

        ''' <summary>
        ''' This function must be called first.  Printer path must be a COM Port or a UNC path.
        ''' </summary>
        Public Sub StartWrite(ByVal printerPath As String) Implements AxLMZebraPrinting.StartWrite
            Dim SA As SECURITY_ATTRIBUTES
            Dim portIsOpen As Boolean = False
            Dim tryCount As Integer = 0

            Do
                tryCount += 1
                'Create connection
                _SafeFileHandle = CreateFile(printerPath, GENERIC_WRITE, 0, SA, FileMode.OpenOrCreate, 0, IntPtr.Zero)
                'Create file stream
                Try
                    _outFile = New FileStream(_SafeFileHandle, FileAccess.Write)
                    _fileWriter = New StreamWriter(_outFile)
                    portIsOpen = True
                Catch ex As Exception
                    portIsOpen = False
                End Try
            Loop While portIsOpen = False AndAlso tryCount < 5
            If Not portIsOpen Then Throw New Exception("No se pudo encontrar la impresora")
        End Sub

        ''' <summary>
        ''' This will write a command to the printer.
        ''' </summary>
        Public Sub Write(ByVal rawLine As String) Implements AxLMZebraPrinting.Write
            If _fileWriter IsNot Nothing Then
                _fileWriter.WriteLine(rawLine)
            End If
        End Sub

        ''' <summary>
        ''' This will write a command to the printer.
        ''' </summary>
        Public Sub WriteWithEncoding(ByVal rawLine As String) Implements AxLMZebraPrinting.WriteWithEncoding
            Dim encDefaultEncoding As Encoding = Encoding.Default
            If _outFile IsNot Nothing Then
                _outFile.Write(encDefaultEncoding.GetBytes(rawLine), 0, encDefaultEncoding.GetByteCount(rawLine))
            End If
        End Sub

        ''' <summary>
        ''' This function must be called after writing to the zebra printer.
        ''' </summary>
        Public Sub EndWrite() Implements AxLMZebraPrinting.EndWrite
            'Clean up
            Try
                If _fileWriter IsNot Nothing Then
                    _fileWriter.Flush()
                    _fileWriter.Close()
                    _outFile.Close()
                End If
            Finally
                If _SafeFileHandle IsNot Nothing Then _SafeFileHandle.Close()
                If _SafeFileHandle IsNot Nothing Then _SafeFileHandle.Dispose()
                _SafeFileHandle = Nothing
                _fileWriter = Nothing
                _outFile = Nothing
            End Try
        End Sub
#End Region

    End Class
End Namespace
