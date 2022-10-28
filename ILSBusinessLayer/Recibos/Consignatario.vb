Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras

Namespace Recibos
    Public Class Consignatario

#Region "Variables Privadas"
        Private _idConsignatario As Integer
        Private _nombre As String
        Private _propio As Boolean
        Private _predeterminado As Boolean
        Private _activo As Boolean
        Private _error As String
#End Region

#Region "Propiedades"

        Public Property IdConsignatario() As Integer
            Get
                Return _idConsignatario
            End Get
            Set(ByVal value As Integer)
                _idConsignatario = value
            End Set
        End Property

        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        Public Property Propio() As Boolean
            Get
                Return _propio
            End Get
            Set(ByVal value As Boolean)
                _propio = value
            End Set
        End Property

        Public Property Predeterminado() As Boolean
            Get
                Return _predeterminado
            End Get
            Set(ByVal value As Boolean)
                _predeterminado = value
            End Set
        End Property

        Public Property Activo() As Boolean
            Get
                Return _activo
            End Get
            Set(ByVal value As Boolean)
                _activo = value
            End Set
        End Property

        Public ReadOnly Property InfoError() As String
            Get
                Return _error
            End Get
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _error = String.Empty
        End Sub

        Public Sub New(ByVal idConsignatario As Integer)
            Me.New()
            CargarDatos(idConsignatario)
        End Sub

        Public Sub New(ByVal preestablecido As Boolean)
            If preestablecido Then
                Dim db As New LMDataAccess
                Try
                    db.SqlParametros.Add("@predeterminado", SqlDbType.Bit).Value = True                    
                    db.ejecutarReader("ObtenerInfoConsignatario", CommandType.StoredProcedure)
                    If db.Reader.Read Then
                        _idConsignatario = CInt(db.Reader("idConsignatario").ToString)
                        _nombre = db.Reader("nombre").ToString()
                        Boolean.TryParse(db.Reader("propio").ToString, _propio)
                        Boolean.TryParse(db.Reader("predeterminado").ToString, _predeterminado)
                        Boolean.TryParse(db.Reader("activo").ToString, _activo)
                    End If
                Catch ex As Exception
                    _error = "Error al cargar los datos. " & ex.Message
                Finally
                    If Not db.Reader.IsClosed Then db.Reader.Close()
                    db.Dispose()
                End Try            
            End If
        End Sub
#End Region

#Region "Metodos Privados"

        Private Sub CargarDatos(ByVal idConsignatario As Integer)
            Dim db As New LMDataAccess
            Try
                db.SqlParametros.Add("@idConsignatario", SqlDbType.SmallInt).Value = idConsignatario
                db.ejecutarReader("ObtenerInfoConsignatario", CommandType.StoredProcedure)
                If db.Reader.Read Then
                    _idConsignatario = idConsignatario
                    _nombre = db.Reader("nombre").ToString()
                    Boolean.TryParse(db.Reader("propio").ToString, _propio)
                    Boolean.TryParse(db.Reader("predeterminado").ToString, _predeterminado)
                    Boolean.TryParse(db.Reader("activo").ToString, _activo)
                End If
            Catch ex As Exception
                _error = "Error al cargar los datos. " & ex.Message
            Finally
                If Not db.Reader.IsClosed Then db.Reader.Close()
                db.Dispose()
            End Try
        End Sub

#End Region

#Region "Metodos Publicos"

        Public Function Crear() As Boolean
            Dim db As New LMDataAccess
            Dim retorno As Boolean = False
            If _nombre <> String.Empty Then
                With db
                    With .SqlParametros                        
                        .Add("@nombre", SqlDbType.VarChar).Value = _nombre
                        .Add("@propio", SqlDbType.Bit).Value = _propio
                        .Add("@predeterminado", SqlDbType.Bit).Value = _predeterminado
                        .Add("@activo", SqlDbType.Bit).Value = _activo
                        .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                    End With

                    Try
                        Dim result As Integer
                        .iniciarTransaccion()
                        .ejecutarNonQuery("CrearInfoConsignatario", CommandType.StoredProcedure)
                        result = .SqlParametros("@result").Value
                        If result = 0 Then
                            _idConsignatario = CShort(.SqlParametros("@identity").Value)
                            CargarDatos(_idConsignatario)
                            .confirmarTransaccion()
                            retorno = True
                        End If
                    Catch ex As Exception
                        Me._error = ex.Message
                        Throw New Exception(ex.Message)
                    Finally
                        If Not db.Reader.IsClosed Then db.Reader.Close()
                        db.Dispose()
                    End Try
                End With
            End If
            Return retorno
        End Function

        Public Function Actualizar() As Boolean
            Dim retorno As Boolean = False
            If _idConsignatario > 0 Then
                Dim db As New LMDataAccess
                Try
                    With db
                        With .SqlParametros
                            .Add("@idConsignatario", SqlDbType.Int).Value = _idConsignatario
                            .Add("@nombre", SqlDbType.VarChar).Value = _nombre
                            .Add("@propio", SqlDbType.Bit).Value = _propio
                            .Add("@predeterminado", SqlDbType.Bit).Value = _predeterminado
                            .Add("@activo", SqlDbType.Bit).Value = _activo
                            .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                        End With
                        Dim result As Integer
                        .iniciarTransaccion()
                        .ejecutarNonQuery("ActualizarInfoConsignatario", CommandType.StoredProcedure)
                        result = .SqlParametros("@result").Value
                        If result = 0 Then
                            CargarDatos(_idConsignatario)
                            .confirmarTransaccion()
                            retorno = True
                        End If
                    End With
                Catch ex As Exception
                    Me._error = ex.Message
                    Throw New Exception(ex.Message)
                Finally
                    If Not db.Reader.IsClosed Then db.Reader.Close()
                    db.Dispose()
                End Try
            End If
            Return retorno
        End Function

#End Region

#Region "Metodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroConsignatario
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroConsignatario) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdConsignatario > 0 Then db.SqlParametros.Add("@idConsignatario", SqlDbType.Int).Value = .IdConsignatario
                If .Nombre <> "" Then db.SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = .Nombre
                If .Propio > 0 Then db.SqlParametros.Add("@propio", SqlDbType.Bit).Value = IIf(.Propio = 1, 1, 0)
                If .Predeterminado > 0 Then db.SqlParametros.Add("@predeterminado", SqlDbType.Bit).Value = IIf(.Predeterminado = 1, 1, 0)
                If .Activo > 0 Then db.SqlParametros.Add("@activo", SqlDbType.Bit).Value = IIf(.Activo = 1, 1, 0)
                dtDatos = db.ejecutarDataTable("ObtenerInfoConsignatario", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos

        End Function

#End Region

    End Class
End Namespace
