Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras

Namespace Comunes
    Public Class ClienteExterno

#Region "Variables Privadas"
        Private _idClienteExterno As Integer
        Private _nombre As String        
        Private _activo As Boolean
        Private _error As String
#End Region

#Region "Propiedades"

        Public Property IdConsignatario() As Integer
            Get
                Return _idClienteExterno
            End Get
            Set(ByVal value As Integer)
                _idClienteExterno = value
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

        Public Sub New(ByVal idClienteExterno As Integer)
            Me.New()
            CargarDatos(idClienteExterno)
        End Sub

#End Region

#Region "Metodos Privados"

        Private Sub CargarDatos(ByVal idClienteExterno As Integer)
            Dim db As New LMDataAccess
            Try
                db.SqlParametros.Add("@idClienteExterno", SqlDbType.SmallInt).Value = idClienteExterno
                db.ejecutarReader("ObtenerInfoClienteExterno", CommandType.StoredProcedure)
                If db.Reader.Read Then
                    _idClienteExterno = idClienteExterno
                    _nombre = db.Reader("nombre").ToString()                    
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
                        .Add("@activo", SqlDbType.Bit).Value = _activo
                        .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                    End With

                    Try
                        Dim result As Integer
                        .iniciarTransaccion()
                        .ejecutarNonQuery("CrearInfoClienteExterno", CommandType.StoredProcedure)
                        result = .SqlParametros("@result").Value
                        If result = 0 Then
                            _idClienteExterno = CShort(.SqlParametros("@identity").Value)
                            CargarDatos(_idClienteExterno)
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
            If _idClienteExterno > 0 Then
                Dim db As New LMDataAccess
                Try
                    With db
                        With .SqlParametros
                            .Add("@idClienteExterno", SqlDbType.Int).Value = _idClienteExterno
                            .Add("@nombre", SqlDbType.VarChar).Value = _nombre                            
                            .Add("@activo", SqlDbType.Bit).Value = _activo
                            .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                        End With
                        Dim result As Integer
                        .iniciarTransaccion()
                        .ejecutarNonQuery("ActualizarInfoClienteExterno", CommandType.StoredProcedure)
                        result = .SqlParametros("@result").Value
                        If result = 0 Then
                            CargarDatos(_idClienteExterno)
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
            Dim filtro As New FiltroClienteExterno
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroClienteExterno) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdClienteExterno > 0 Then db.SqlParametros.Add("@idClienteExterno", SqlDbType.Int).Value = .IdClienteExterno
                If .Nombre <> "" Then db.SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = .Nombre
                If .Activo > 0 Then db.SqlParametros.Add("@activo", SqlDbType.Bit).Value = IIf(.Activo = 1, 1, 0)
                If .EsFinanciero <> False Then db.SqlParametros.Add("@esFinanciero", SqlDbType.Bit).Value = .EsFinanciero
                dtDatos = db.ejecutarDataTable("ObtenerInfoClienteExterno", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos

        End Function

#End Region
    End Class
End Namespace
