Imports System
Imports System.Drawing
Imports LMDataAccessLayer
Imports System.Data.SqlTypes

Public Class ActualizaPing

#Region "Variables"
    Private _resulConSerial As DataTable
    Private _idUsuario As Integer
    Private _pinNuevo As String
    Private _serial As String
    Private _pinAnterior As String


#End Region

#Region "Propiedades"

    Public Property pinNuevo() As String
        Get
            Return _pinNuevo
        End Get
        Set(ByVal Value As String)
            _pinNuevo = Value
        End Set
    End Property

    Public Property pinAnterior() As String
        Get
            Return _pinAnterior
        End Get
        Set(ByVal Value As String)
            _pinAnterior = Value
        End Set
    End Property

    Public Property serial() As String
        Get
            Return _serial
        End Get
        Set(ByVal Value As String)
            _serial = Value
        End Set
    End Property


    Public Property idUsuario() As Integer
        Get
            Return _idUsuario
        End Get
        Set(ByVal Value As Integer)
            _idUsuario = Value
        End Set
    End Property

    Public Property resulConSerial() As DataTable
        Get
            Return _resulConSerial
        End Get
        Set(ByVal Value As DataTable)
            _resulConSerial = Value
        End Set
    End Property




#End Region
#Region "Metodos Publicos"
    Private Function fillData(ByVal reader As IDataReader) As ActualizaPing
        Dim _data As New ActualizaPing()
        _data.serial = CType(reader("Serial"), String)
        _data.pinAnterior = CStr(If(reader("Pin") Is DBNull.Value, Nothing, CType(reader("Pin"), String)))
        '_data.pinAnterior = IIf(reader("Pin") = System.DBNull.Value, Nothing, CType(reader("Pin"), String))

        Return _data

    End Function
    Public Sub ValidaActualizacion(ByVal pin As ActualizaPing, ByRef errorvalida As String, ByRef bandera As Integer)
        Dim reader As IDataReader = Nothing
        Dim reader1 As IDataReader = Nothing
        Dim resultado As New ActualizaPing()
        Dim db As New LMDataAccess
        Dim db1 As New LMDataAccess
        Try
            db.SqlParametros.Add("@Pin", SqlDbType.VarChar).Value = pin.pinNuevo
            db.TiempoEsperaComando = 900
            reader = db.ejecutarReader("Obtieneserialpin", CommandType.StoredProcedure)
            If reader.Read() Then
                resultado = fillData(reader)
            End If
            If (resultado.serial IsNot Nothing) Then
                If (pin.serial = resultado.serial) Then
                    'Errorvalida = Errorvalida & "El Pin ya se encuentra asociado al serial <br><br>"
                Else
                    bandera = 1
                    errorvalida = String.Format("{0} El pin {1} Esta asociado al serial {2}", errorvalida, resultado.pinAnterior, resultado.serial)
                End If

            End If
            If (errorvalida Is Nothing) Then
                db1.SqlParametros.Add("@Serial", SqlDbType.VarChar).Value = pin.serial
                db1.TiempoEsperaComando = 900
                reader1 = db1.ejecutarReader("Obtieneserialpin", CommandType.StoredProcedure)
                If reader1.Read() Then
                    resultado = fillData(reader1)
                End If
                If (resultado.serial IsNot Nothing) Then
                    If (resultado.pinAnterior Is Nothing) Then
                        errorvalida = errorvalida + "El Serial no tiene Pin asociado "
                        bandera = 2
                    End If
                    If (pin.pinNuevo.Equals(resultado.pinAnterior)) Then
                        errorvalida = errorvalida & "El Pin ya se encuentra asociado al serial "
                    End If

                Else
                    errorvalida = errorvalida + "El seria no existe por favor verifique  "
                End If

            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If Not db Is Nothing Then db.Dispose()
            If Not db1 Is Nothing Then db1.Dispose()

        End Try

    End Sub
    Public Function ConsultaSerial(ByVal serial As String) As DataTable
        Dim db As New LMDataAccess
        Try
            If (serial <> "" Or serial IsNot Nothing) Then
                db.SqlParametros.Add("@Serial", SqlDbType.VarChar).Value = serial.Trim()
                db.TiempoEsperaComando = 900
                _resulConSerial = db.ejecutarDataTable("Obtieneserialpin", CommandType.StoredProcedure)
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If Not db Is Nothing Then db.Dispose()
        End Try
        Return _resulConSerial
    End Function

    Public Function ConsultaSerialva(ByVal serial As String) As ActualizaPing
        Dim reader As IDataReader = Nothing
        Dim resultado As New ActualizaPing()
        Dim db As New LMDataAccess
        Try
            db.SqlParametros.Add("@Serial", SqlDbType.VarChar).Value = serial
            db.TiempoEsperaComando = 900
            reader = db.ejecutarReader("Obtieneserialpin", CommandType.StoredProcedure)
            If reader.Read() Then
                Return fillData(reader)
            End If
            Return Nothing
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If Not db Is Nothing Then db.Dispose()
        End Try
        Return Nothing
    End Function

    Public Sub ActualizarPin(ByVal datos As ActualizaPing)
        Dim db As New LMDataAccess
        Try
            db.TiempoEsperaComando = 600

            db.SqlParametros.Add("@Serial", SqlDbType.VarChar).Value = datos.serial
            db.SqlParametros.Add("@Pin_Ante", SqlDbType.VarChar).Value = datos.pinAnterior
            db.SqlParametros.Add("@Pin_Nuevo", SqlDbType.VarChar).Value = datos.pinNuevo
            db.SqlParametros.Add("@idUsuario", SqlDbType.VarChar).Value = datos.idUsuario
            db.ejecutarNonQuery("EjecutarCambioPin", CommandType.StoredProcedure)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If Not db Is Nothing Then db.Dispose()
        End Try

    End Sub
#End Region
End Class
