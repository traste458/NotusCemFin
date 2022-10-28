Imports ILSBusinessLayer.Estructuras
Imports LMDataAccessLayer
Namespace Recibos
    Public Class PalletNovedad

#Region "variables"
        Private _idPalletNovedad As Long
        Private _idPallet As Long
        Private _idNovedad As Integer
        Private _fechaRegistro As Date
#End Region

#Region "propiedades"

        Public ReadOnly Property IdPalletNovedad() As Long
            Get
                Return _idPalletNovedad
            End Get
        End Property

        Public Property IdPallet() As Long
            Get
                Return _idPallet
            End Get
            Set(ByVal value As Long)
                _idPallet = value
            End Set
        End Property

        Public Property IdNovedad() As Integer
            Get
                Return _idNovedad
            End Get
            Set(ByVal value As Integer)
                _idNovedad = value
            End Set
        End Property

        Public ReadOnly Property FechaRegistro() As Date
            Get
                Return _fechaRegistro
            End Get
        End Property

#End Region

#Region "constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idPalletNovedad As Long)
            Me.New()
            _idPalletNovedad = idPalletNovedad
            Me.CargarDatos()
        End Sub

        Public Sub New(ByVal idPallet As Long, ByVal idNovedad As Integer)
            Me.New()
            _idPallet = idPallet
            _idNovedad = idNovedad
            Me.CargarDatos()
        End Sub

#End Region

#Region "metodos privados"

        Private Sub CargarDatos()
            Dim db As New LMDataAccess
            If _idPalletNovedad > 0 Then db.SqlParametros.Add("@idPalletNovedad", SqlDbType.BigInt).Value = _idPalletNovedad
            If _idPallet > 0 Then db.SqlParametros.Add("@idPallet", SqlDbType.BigInt).Value = _idPallet
            If _idNovedad > 0 Then db.SqlParametros.Add("@idNovedad", SqlDbType.Int).Value = _idNovedad
            Try
                db.ejecutarReader("ObtenerPalletNovedad", CommandType.StoredProcedure)
                If db.Reader.Read Then
                    _idPalletNovedad = db.Reader("idPalletNovedad")
                    _idPallet = db.Reader("idPallet")
                    _idNovedad = db.Reader("idNovedad")
                    _fechaRegistro = db.Reader("fechaRegistro").ToString                    
                End If
            Catch ex As Exception
            Finally
                db.Dispose()
            End Try
        End Sub

#End Region

#Region "metodos publicos"

        Public Function Crear() As Boolean
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim retorno As Boolean
            With db
                With .SqlParametros
                    .Add("@idPallet", SqlDbType.BigInt).Value = _idPallet
                    .Add("@idNovedad", SqlDbType.Int).Value = _idNovedad                    
                    .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                End With

                Try
                    Dim result As Integer = 0
                    .iniciarTransaccion()
                    .ejecutarNonQuery("CrearPalletNovedad", CommandType.StoredProcedure)
                    result = .SqlParametros("@result").Value
                    If result = 0 Then
                        _idNovedad = CLng(.SqlParametros("@identity").Value)
                        .confirmarTransaccion()
                        retorno = True
                    Else
                        Throw New Exception("Imposible registrar la información del Pallet Novedad.")
                    End If

                Catch ex As Exception
                    If .estadoTransaccional Then .abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    .cerrarConexion()
                    .Dispose()
                End Try
            End With
            Return retorno
        End Function

        Public Sub Actualizar()
            If IdPalletNovedad <> 0 Then
                Dim db As New LMDataAccessLayer.LMDataAccess

                Try
                    db.iniciarTransaccion()
                    With db.SqlParametros
                        .Add("@idNovedad", SqlDbType.Int).Value = _idNovedad
                        .Add("@idPallet", SqlDbType.BigInt).Value = _idPallet
                    End With
                    db.ejecutarNonQuery("ActualizarPalletNovedad", CommandType.StoredProcedure)
                    db.confirmarTransaccion()
                Catch ex As Exception
                    If db.estadoTransaccional Then db.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    db.cerrarConexion()
                End Try
            Else
                Throw New DuplicateNameException("El Pallet Novedad aún no ha sido registrada en la Base de Datos.")
            End If
        End Sub
#End Region

#Region "metodos compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroPalletNovedad
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroPalletNovedad) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdNovedad > 0 Then db.SqlParametros.Add("@idNovedad", SqlDbType.Int).Value = .IdNovedad
                If .IdPallet > 0 Then db.SqlParametros.Add("@idPallet", SqlDbType.Int).Value = .IdPallet                
                dtDatos = db.ejecutarDataTable("ObtenerPalletNovedad", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos

        End Function

#End Region

    End Class
End Namespace

