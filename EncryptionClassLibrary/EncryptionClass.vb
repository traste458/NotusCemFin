Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Configuration

Namespace LMEncryption

#Region "Symmetric Encryption"
    ''' <summary>
    ''' La Encripción Simetrica utiliza una única llave para Encriptar/Desencriptar
    ''' </summary>
    Public Class SymmetricEncryption

        Private Const DEFAULT_INITIALIZATION_VECTOR As String = "%1Az=-@Jvc"
        Private Const BUFFER_SIZE As Integer = 2048

        Public Enum Provider
            DES
            RC2
            Rijndael
            TripleDES
        End Enum

        Private seData As EncryptionData
        Private seKey As EncryptionData
        Private seIV As EncryptionData
        Private seCrypto As SymmetricAlgorithm
        Private seEncryptedBytes As Byte()
        Private seUseDefaultInitializationVector As Boolean

        Private Sub New()
        End Sub

        ''' <summary>
        ''' Instancia un nuevo objeto de la clase SymmetricEncryption usando el provider especificado
        ''' </summary>
        Public Sub New(ByVal provider As Provider, Optional ByVal useDefaultInitializationVector As Boolean = True)
            Select Case provider
                Case provider.DES
                    seCrypto = New DESCryptoServiceProvider
                Case provider.RC2
                    seCrypto = New RC2CryptoServiceProvider
                Case provider.Rijndael
                    seCrypto = New RijndaelManaged
                Case provider.TripleDES
                    seCrypto = New TripleDESCryptoServiceProvider
            End Select

            '***Se asegura que la llave y el vector de inicialización estén establecidos sin importar que suceda
            Me.Key = RandomKey()
            If useDefaultInitializationVector Then
                Me.IntializationVector = New EncryptionData(DEFAULT_INITIALIZATION_VECTOR)
            Else
                Me.IntializationVector = RandomInitializationVector()
            End If
        End Sub

        ''' <summary>
        ''' Tamaño de la Llave en bytes. Se usa el tamaño de la Llave por defecto para cualquier proveedor; 
        ''' si quiere forzar un tamaño de la Llave específico, establezca esta propiedad
        ''' </summary>
        Public Property KeySizeBytes() As Integer
            Get
                Return Me.seCrypto.KeySize \ 8
            End Get
            Set(ByVal Value As Integer)
                Me.seCrypto.KeySize = Value * 8
                Me.sekey.MaxBytes = Value
            End Set
        End Property

        ''' <summary>
        ''' Tamaño de la Llave en bits. Se usa el tamaño de la Llave por defecto para cualquier proveedor; 
        ''' si quiere forzar un tamaño de la Llave específico, establezca esta propiedad
        ''' </summary>
        Public Property KeySizeBits() As Integer
            Get
                Return Me.seCrypto.KeySize
            End Get
            Set(ByVal Value As Integer)
                Me.seCrypto.KeySize = Value
                Me.seKey.MaxBits = Value
            End Set
        End Property

        ''' <summary>
        ''' La Llave usuada para Encriptar/Desencriptar datos
        ''' </summary>
        Public Property Key() As EncryptionData
            Get
                Return Me.seKey
            End Get
            Set(ByVal Value As EncryptionData)
                Me.seKey = Value
                Me.seKey.MaxBytes = Me.seCrypto.LegalKeySizes(0).MaxSize \ 8
                Me.seKey.MinBytes = Me.seCrypto.LegalKeySizes(0).MinSize \ 8
                Me.seKey.StepBytes = Me.seCrypto.LegalKeySizes(0).SkipSize \ 8
            End Set
        End Property

        ''' <summary>
        ''' Using the default Cipher Block Chaining (CBC) mode, all data blocks are processed using
        ''' the value derived from the previous block; the first data block has no previous data block
        ''' to use, so it needs an InitializationVector to feed the first block
        ''' </summary>
        Public Property IntializationVector() As EncryptionData
            Get
                Return Me.seIV
            End Get
            Set(ByVal Value As EncryptionData)
                Me.seIV = Value
                Me.seIV.MaxBytes = Me.seCrypto.BlockSize \ 8
                Me.seIV.MinBytes = Me.seCrypto.BlockSize \ 8
            End Set
        End Property

        ''' <summary>
        ''' Genera un Vector de Inicialización aleatorio, si no se proporcionó ninguno
        ''' </summary>
        Public Function RandomInitializationVector() As EncryptionData
            Me.seCrypto.GenerateIV()
            Dim edData As New EncryptionData(Me.seCrypto.IV)
            Return edData
        End Function

        ''' <summary>
        ''' Genera una Llave aleatoria, si no se proporcionó ninguna
        ''' </summary>
        Public Function RandomKey() As EncryptionData
            Me.seCrypto.GenerateKey()
            Dim edData As New EncryptionData(Me.seCrypto.Key)
            Return edData
        End Function

        ''' <summary>
        ''' Se asegura de que el objeto de tipo SymmetricAlgorithm tenga una Llave y un Vector de Inicialización
        ''' válidos antes de cualquier intento de Encriptar/Desencriptar
        ''' </summary>
        Private Sub ValidateKeyAndIv(ByVal isEncrypting As Boolean)
            If Me.seKey.IsEmpty Then
                If isEncrypting Then
                    Me.seKey = RandomKey()
                Else
                    Throw New CryptographicException("No se proporcionó una Llave para la operación de Desencripción")
                End If
            End If
            If Me.seIV.IsEmpty Then
                If isEncrypting Then
                    Me.seIV = RandomInitializationVector()
                Else
                    Throw New CryptographicException("No se proporcionó un vector de Inicialización")
                End If
            End If
            Me.seCrypto.Key = Me.seKey.Bytes
            Me.seCrypto.IV = Me.seIV.Bytes
        End Sub

        ''' <summary>
        ''' Encripta los datos especificados utilizando la Llave proporcionada
        ''' </summary>
        Public Function Encrypt(ByVal data As EncryptionData, ByVal key As EncryptionData) As EncryptionData
            Me.Key = key
            Return Encrypt(data)
        End Function

        ''' <summary>
        ''' Encripta los datos especificados utilizando la Llave prestablecida y el
        ''' Vector de Inicializacion Prestablecido
        ''' </summary>
        Public Function Encrypt(ByVal data As EncryptionData) As EncryptionData
            Dim msAux As New IO.MemoryStream

            ValidateKeyAndIv(True)

            Dim csAux As New CryptoStream(msAux, Me.seCrypto.CreateEncryptor(), CryptoStreamMode.Write)
            csAux.Write(data.Bytes, 0, data.Bytes.Length)
            csAux.Close()
            msAux.Close()

            Return New EncryptionData(msAux.ToArray)
        End Function

        ''' <summary>
        ''' Encripta el Stream usando la Llave proporcionada y el Vector de Inicialización proporcionado
        ''' </summary>
        Public Function Encrypt(ByVal myStream As Stream, ByVal key As EncryptionData, ByVal initVector As EncryptionData) As EncryptionData
            Me.IntializationVector = initVector
            Me.Key = key
            Return Encrypt(myStream)
        End Function

        ''' <summary>
        ''' Encripta el Stream usando la Llave especificada
        ''' </summary>
        Public Function Encrypt(ByVal myStream As Stream, ByVal key As EncryptionData) As EncryptionData
            Me.Key = key
            Return Encrypt(myStream)
        End Function

        ''' <summary>
        ''' Encripta el Stream utilizando la Llave y el Vector de Inicialización preestablecidos
        ''' </summary>
        Public Function Encrypt(ByVal myStream As Stream) As EncryptionData
            Dim msAux As New IO.MemoryStream
            Dim arrByte(BUFFER_SIZE) As Byte
            Dim index As Integer

            ValidateKeyAndIv(True)

            Dim csAux As New CryptoStream(msAux, Me.seCrypto.CreateEncryptor(), CryptoStreamMode.Write)
            index = myStream.Read(arrByte, 0, BUFFER_SIZE)
            Do While index > 0
                csAux.Write(arrByte, 0, index)
                index = myStream.Read(arrByte, 0, BUFFER_SIZE)
            Loop

            csAux.Close()
            msAux.Close()

            Return New EncryptionData(msAux.ToArray)
        End Function

        ''' <summary>
        ''' Desencripta los datos especificados usando la Llave proporcionada y el Vector de Inicialización preestablecido
        ''' </summary>
        Public Function Decrypt(ByVal encryptedData As EncryptionData, ByVal key As EncryptionData) As EncryptionData
            Me.Key = key
            Return Decrypt(encryptedData)
        End Function

        ''' <summary>
        ''' Desencripta el Stream especificado usando la Llave proporcionada y el Vector de Inicialización preestablecido
        ''' </summary>
        Public Function Decrypt(ByVal encryptedStream As Stream, ByVal key As EncryptionData) As EncryptionData
            Me.Key = key
            Return Decrypt(encryptedStream)
        End Function

        ''' <summary>
        ''' Desencripta el Stream especificado usando la Llave y el Vector de Inicialización preestablecidos
        ''' </summary>
        Public Function Decrypt(ByVal encryptedStream As Stream) As EncryptionData
            Dim msAux As New System.IO.MemoryStream
            Dim arrByte(BUFFER_SIZE) As Byte

            ValidateKeyAndIv(False)
            Dim csAux As New CryptoStream(encryptedStream, Me.seCrypto.CreateDecryptor(), CryptoStreamMode.Read)

            Dim index As Integer
            index = csAux.Read(arrByte, 0, BUFFER_SIZE)

            Do While index > 0
                msAux.Write(arrByte, 0, index)
                index = csAux.Read(arrByte, 0, BUFFER_SIZE)
            Loop
            csAux.Close()
            msAux.Close()

            Return New EncryptionData(msAux.ToArray)
        End Function

        ''' <summary>
        ''' Desencripta los datos especificados utilizando la Llave y el Vector de Inicialización preestablecidos
        ''' </summary>
        Public Function Decrypt(ByVal encryptedData As EncryptionData) As EncryptionData
            Dim msAux As New System.IO.MemoryStream(encryptedData.Bytes, 0, encryptedData.Bytes.Length)
            Dim arrByte() As Byte = New Byte(encryptedData.Bytes.Length - 1) {}

            ValidateKeyAndIv(False)
            Dim csAux As New CryptoStream(msAux, Me.seCrypto.CreateDecryptor(), CryptoStreamMode.Read)

            Try
                csAux.Read(arrByte, 0, encryptedData.Bytes.Length - 1)
            Catch ex As CryptographicException
                Throw New CryptographicException("Imposible desencriptar datos. La Llave proporcionada parece no ser válida.", ex)
            Finally
                csAux.Close()
            End Try
            Return New EncryptionData(arrByte)
        End Function
    End Class

#End Region

#Region "Encyption Data"
    ''' <summary>
    ''' Representa los datos a Encriptar/Desencriptar en Hex, Byte, Base64 o String
    ''' </summary>
    Public Class EncryptionData
        Private edArrByte() As Byte
        Private edMaxBytes As Integer
        Private edMinBytes As Integer
        Private edStepBytes As Integer

        ''' <summary>
        ''' Determina la codificación de texto por defecto para todas las instancias de EncryptionData
        ''' </summary>
        Public Shared DefaultEncoding As Text.Encoding = System.Text.Encoding.Default

        ''' <summary>
        ''' Determina la codificación de texto por defecto de esta instancia de EncryptionData
        ''' </summary>
        Public Encoding As Text.Encoding = DefaultEncoding

        ''' <summary>
        ''' Crear una nueva instancia
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Crear una nueva instanci con el array de Bytes especificado
        ''' </summary>
        Public Sub New(ByVal arrByte As Byte())
            Me.edArrByte = arrByte
        End Sub

        ''' <summary>
        ''' Crea una nueva instancia con la cadena especificada
        ''' </summary>
        Public Sub New(ByVal theString As String)
            Me.Text = theString
        End Sub

        ''' <summary>
        ''' Crear una nueva instancia con la cadena especificada, usando la codificación especificada
        ''' para convertir la cadena en un array de bytes
        ''' </summary>
        Public Sub New(ByVal theString As String, ByVal theEncoding As System.Text.Encoding)
            Me.Encoding = theEncoding
            Me.Text = theString
        End Sub

        ''' <summary>
        ''' Retorna true si no hay datos presentes
        ''' </summary>
        Public ReadOnly Property IsEmpty() As Boolean
            Get
                If Me.edArrByte Is Nothing OrElse Me.edArrByte.Length = 0 Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        ''' <summary>
        ''' Intervalos de avance permitidos en bytes para los datos de la clase. Si el valor es 0, no existe limite
        ''' </summary>
        Public Property StepBytes() As Integer
            Get
                Return Me.edStepBytes
            End Get
            Set(ByVal Value As Integer)
                Me.edStepBytes = Value
            End Set
        End Property

        ''' <summary>
        ''' Intervalos de avance permitidos en bits para los datos de la clase. Si el valor es 0, no existe limite
        ''' </summary>
        Public Property StepBits() As Integer
            Get
                Return Me.edStepBytes * 8
            End Get
            Set(ByVal Value As Integer)
                Me.edStepBytes = Value \ 8
            End Set
        End Property

        ''' <summary>
        ''' Mínimo número de bytes permitidos para los datos de la clase. Si el valor es 0, no existe limite
        ''' </summary>
        Public Property MinBytes() As Integer
            Get
                Return Me.edMinBytes
            End Get
            Set(ByVal Value As Integer)
                Me.edMinBytes = Value
            End Set
        End Property

        ''' <summary>
        ''' Mínimo número de bits permitidos para los datos de la clase. Si el valor es 0, no existe limite
        ''' </summary>
        Public Property MinBits() As Integer
            Get
                Return Me.edMinBytes * 8
            End Get
            Set(ByVal Value As Integer)
                Me.edMinBytes = Value \ 8
            End Set
        End Property

        ''' <summary>
        ''' Maximo número de bytes permitidos para los datos de la clase. Si el valor es 0, no existe limite
        ''' </summary>
        Public Property MaxBytes() As Integer
            Get
                Return Me.edMaxBytes
            End Get
            Set(ByVal Value As Integer)
                Me.edMaxBytes = Value
            End Set
        End Property

        ''' <summary>
        ''' Máximo número de bits permitidos para los datos de la clase. Si el valor es 0, no existe limite
        ''' </summary>
        Public Property MaxBits() As Integer
            Get
                Return Me.edMaxBytes * 8
            End Get
            Set(ByVal Value As Integer)
                Me.edMaxBytes = Value \ 8
            End Set
        End Property

        ''' <summary>
        ''' Retorna la representación en Bytes de los datos; 
        ''' Será rellenado al tamaño de MinBytes o recortado al tamaño de MaxBytes de ser necesario
        ''' </summary>
        Public Property Bytes() As Byte()
            Get
                If Me.edMaxBytes > 0 Then
                    If Me.edArrByte.Length > Me.edMaxBytes Then
                        Dim arrBytes(Me.edMaxBytes - 1) As Byte
                        Array.Copy(Me.edArrByte, arrBytes, arrBytes.Length)
                        Me.edArrByte = arrBytes
                    End If
                End If
                If Me.edMinBytes > 0 Then
                    If Me.edArrByte.Length < Me.edMinBytes Then
                        Dim arrByte(Me.edMinBytes - 1) As Byte
                        Array.Copy(Me.edArrByte, arrByte, Me.edArrByte.Length)
                        Me.edArrByte = arrByte
                    End If
                End If
                If Me.edStepBytes > 0 Then
                    Dim stepCheck As Integer = Me.edArrByte.Length Mod Me.StepBytes
                    If stepCheck > 0 Then
                        Dim arrByte(Me.edArrByte.Length - stepCheck + Me.edStepBytes - 1) As Byte
                        Array.Copy(Me.edArrByte, arrByte, Me.edArrByte.Length)
                        Me.edArrByte = arrByte
                    End If
                End If

                Return Me.edArrByte
            End Get
            Set(ByVal Value As Byte())
                Me.edArrByte = Value
            End Set
        End Property

        ''' <summary>
        ''' Establece o devuelve la representación en texto de los bytes, usando la codificación de texto por defecto
        ''' </summary>
        Public Property Text() As String
            Get
                If Me.edArrByte Is Nothing Then
                    Return ""
                Else
                    Dim index As Integer = Array.IndexOf(Me.edArrByte, CType(0, Byte))
                    If index >= 0 Then
                        Return Me.Encoding.GetString(Me.edArrByte, 0, index)
                    Else
                        Return Me.Encoding.GetString(Me.edArrByte)
                    End If
                End If
            End Get
            Set(ByVal Value As String)
                Me.edArrByte = Me.Encoding.GetBytes(Value)
            End Set
        End Property

        ''' <summary>
        ''' Establece o devuelve la representación Hexagecimal de los datos
        ''' </summary>
        Public Property Hex() As String
            Get
                Return Utilities.toHex(Me.edArrByte)
            End Get
            Set(ByVal Value As String)
                Me.edArrByte = Utilities.fromHex(Value)
            End Set
        End Property

        ''' <summary>
        ''' Establece o devuelve la representación Base64 de los datos
        ''' </summary>
        Public Property Base64() As String
            Get
                Return Utilities.toBase64(Me.edArrByte)
            End Get
            Set(ByVal Value As String)
                Me.edArrByte = Utilities.fromBase64(Value)
            End Set
        End Property

        ''' <summary>
        ''' Retorna la representación en texto de los bytes, usando la codificación de texto por defecto
        ''' </summary>
        Public Shadows Function toString() As String
            Return Me.Text
        End Function

        ''' <summary>
        ''' Retorna la representación en Base64 de los datos
        ''' </summary>
        Public Function toBase64() As String
            Return Me.Base64
        End Function

        ''' <summary>
        ''' Retorna la representación Hexagecimal de los datos
        ''' </summary>
        Public Function toHex() As String
            Return Me.Hex
        End Function

        ''' <summary>
        ''' Retorna la representación Hexagecimal de los datos, en letra minúcula
        ''' </summary>
        Public Function toHexLower() As String
            Return Me.Hex.ToLower
        End Function

        ''' <summary>
        ''' Permite encriptar una cadena de entrada utilizando el algoritmo MD5
        ''' </summary>
        Public Sub ComputeMd5Hash(ByVal input As String)
            ' Se crea una nueva instancia de el objeto MD5CryptoServiceProvider
            Using md5Hasher As New MD5CryptoServiceProvider()
                Me.edArrByte = md5Hasher.ComputeHash(Me.Encoding.GetBytes(input))
            End Using
        End Sub

        ''' <summary>
        ''' Retorna la cadena resultante de encriptar una cadena de entrada utilizando el algoritmo MD5
        ''' </summary>
        Public Shared Function getMD5Hash(ByVal input As String) As String
            Dim hashedBytes As Byte()
            ' Se crea una nueva instancia de el objeto MD5CryptoServiceProvider
            Using md5Hasher As New MD5CryptoServiceProvider()
                hashedBytes = md5Hasher.ComputeHash(DefaultEncoding.GetBytes(input))
            End Using
            Dim myString As New StringBuilder

            For Each theByte As Byte In hashedBytes
                myString.Append(theByte.ToString("x2"))
            Next
            Return myString.ToString
        End Function

    End Class
#End Region

#Region "Utilities"

    Friend Class Utilities

        Friend Shared Function toHex(ByVal arrByte() As Byte) As String
            If arrByte Is Nothing OrElse arrByte.Length = 0 Then Return ""
            Dim sbAux As New StringBuilder
            For Each b As Byte In arrByte
                sbAux.Append(String.Format("{0:X2}", b))
            Next
            Return sbAux.ToString
        End Function

        Friend Shared Function fromHex(ByVal hexEncoded As String) As Byte()
            If hexEncoded Is Nothing OrElse hexEncoded.Length = 0 Then Return Nothing
            Try
                Dim length As Integer = Convert.ToInt32(hexEncoded.Length / 2)
                Dim arrByte(length - 1) As Byte
                For index As Integer = 0 To length - 1
                    arrByte(index) = Convert.ToByte(hexEncoded.Substring(index * 2, 2), 16)
                Next
                Return arrByte
            Catch ex As Exception
                Throw New System.FormatException("La cadena proporcionada parece estar codificada en Hexadecimal:" & _
                    Environment.NewLine & hexEncoded & Environment.NewLine, ex)
            End Try
        End Function

        Friend Shared Function fromBase64(ByVal base64Encoded As String) As Byte()
            If base64Encoded Is Nothing OrElse base64Encoded.Length = 0 Then Return Nothing
            Try
                Return Convert.FromBase64String(base64Encoded)
            Catch ex As System.FormatException
                Throw New System.FormatException("La cadena proporcinada no parece estar codificada en Base64:" & _
                    Environment.NewLine & base64Encoded & Environment.NewLine, ex)
            End Try
        End Function

        Friend Shared Function toBase64(ByVal arrByte() As Byte) As String
            If arrByte Is Nothing OrElse arrByte.Length = 0 Then Return ""
            Return Convert.ToBase64String(arrByte)
        End Function

    End Class

#End Region

End Namespace


