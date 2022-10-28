Imports ILSBusinessLayer.Estructuras
Imports LMDataAccessLayer

Namespace OMS

    Public Class OrdenComboDetalle


#Region "variables"
        Private _idOrdenComboDetalle As Long
        Private _idOrdenCombo As Long
        Private _serial1 As String
        Private _serial2 As String
        Private _idUsuario As Long
        Private _sim As String
        Private _mensajeRespuesta As String
#End Region

#Region "propiedades"

        Public ReadOnly Property IdOrdenComboDetalle() As Long
            Get
                Return _idOrdenComboDetalle
            End Get
        End Property

        Public Property IdOrdenCombo() As Long
            Get
                Return _idOrdenCombo
            End Get
            Set(ByVal value As Long)
                _idOrdenCombo = value
            End Set
        End Property

        Public Property Serial1() As String
            Get
                Return _serial1
            End Get
            Set(ByVal value As String)
                _serial1 = value
            End Set
        End Property

        Public Property Serial2() As String
            Get
                Return _serial2
            End Get
            Set(ByVal value As String)
                _serial2 = value
            End Set
        End Property

        Public Property IdUsuario() As Long
            Get
                Return _idUsuario
            End Get
            Set(ByVal value As Long)
                _idUsuario = value
            End Set
        End Property

        Public Property Sim() As String
            Get
                Return _sim
            End Get
            Set(ByVal value As String)
                _sim = value
            End Set
        End Property

        Public ReadOnly Property MensajeRespuesta() As String
            Get
                Return _mensajeRespuesta
            End Get
        End Property

#End Region

#Region "constructores"
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(ByVal serial As String)
            Me.New()
            Me.CargarDatos(serial)            
        End Sub
#End Region

#Region "metodos privados"

        Private Sub CargarDatos(ByVal serial As String)        
            Dim db As New LMDataAccess
            db.SqlParametros.Add("@serial", SqlDbType.VarChar).Value = serial
            db.SqlParametros.Add("@mensajeRespuesta", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output
            Try
                db.ejecutarReader("ObtenerOrdenComboDetalleDesdeSerial", CommandType.StoredProcedure)
                If db.Reader.Read Then
                    _idOrdenComboDetalle = db.Reader("idOrdenComboDetalle")
                    _idOrdenCombo = db.Reader("idOrdenCombo")
                    _serial1 = db.Reader("serial1")
                    _serial2 = db.Reader("serial2")
                    _idUsuario = db.Reader("idUsuario")
                    '_mensajeRespuesta = db.SqlParametros("@mensajeRespuesta").Value.ToString
                End If
            Catch ex As Exception
            Finally
                If Not db.Reader.IsClosed Then db.Reader.Close()
                db.Dispose()
            End Try
        End Sub        

#End Region

#Region "metodos publicos"

        Public Function Crear() As Boolean
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim retorno As Boolean = False
            Dim resultado As Short
            With dbManager
                With .SqlParametros
                    .Add("@idOrdenCombo", SqlDbType.BigInt).Value = _idOrdenCombo
                    .Add("@serial1", SqlDbType.Char).Value = _serial1
                    .Add("@serial2", SqlDbType.Char).Value = _serial2
                    .Add("@idUsuario", SqlDbType.BigInt).Value = _idUsuario
                    .Add("@sim", SqlDbType.Char).Value = _sim
                    .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                    .Add("@mensajeRespuesta", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                End With
                Try
                    .iniciarTransaccion()
                    .ejecutarNonQuery("CrearOrdenComboDetalle", CommandType.StoredProcedure)
                    Short.TryParse(.SqlParametros("@result").Value.ToString, resultado)
                    _mensajeRespuesta = .SqlParametros("@mensajeRespuesta").Value.ToString
                    .confirmarTransaccion()
                    retorno = IIf(resultado = 0, True, False)
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso .estadoTransaccional Then .abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                    Return False
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End With
            Return retorno
        End Function

#End Region

#Region "métodos compartidos"

        Public Overloads Shared Function ObtenerListado(Optional ByVal idOrdenCombo As Integer = 0) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            If idOrdenCombo > 0 Then db.SqlParametros.Add("@idOrdenCombo", SqlDbType.BigInt).Value = idOrdenCombo
            dtDatos = db.ejecutarDataTable("ObtenerOrdenComboDetalle", CommandType.StoredProcedure)
            Return dtDatos

        End Function

#End Region


    End Class
End Namespace


