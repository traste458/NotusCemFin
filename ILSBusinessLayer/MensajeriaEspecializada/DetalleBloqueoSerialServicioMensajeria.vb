Imports LMDataAccessLayer

Public Class DetalleBloqueoSerialServicioMensajeria

#Region "Atributos (Campos)"

    Private _iccid As String
    Private _numeroRadicado As Long
    Private _idUsuario As Integer
    Private _idBloqueo As Integer
    Private _idBodega As Integer
    Private _dbManager As New LMDataAccess

#End Region

#Region "Propiedades"

    Public Property Iccid() As String
        Get
            Return _iccid
        End Get
        Set(ByVal value As String)
            _iccid = value
        End Set
    End Property

    Public Property NumeroRadicado() As Long
        Get
            Return _numeroRadicado
        End Get
        Set(ByVal value As Long)
            _numeroRadicado = value
        End Set
    End Property

    Public Property IdUsuario() As Integer
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Integer)
            _idUsuario = value
        End Set
    End Property

    Public Property IdBloqueo() As Integer
        Get
            Return _idBloqueo
        End Get
        Set(ByVal value As Integer)
            _idBloqueo = value
        End Set
    End Property

    Public Property IdBodega() As Integer
        Get
            Return _idBodega
        End Get
        Set(ByVal value As Integer)
            _idBodega = value
        End Set
    End Property



#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function ValidarDatos() As List(Of ResultadoProceso)
        Dim resultado As New List(Of ResultadoProceso)
        If _dbManager Is Nothing Then _dbManager = New LMDataAccess
        With _dbManager
            With .SqlParametros
                .Clear()
                .Add("@iccid", SqlDbType.VarChar, 20).Value = _iccid
                .Add("@numeroRadicado", SqlDbType.BigInt).Value = _numeroRadicado
                .Add("@idBodega", SqlDbType.Int).Direction = ParameterDirection.Output
            End With
            .ejecutarReader("ValidarBloqueoSerialCEM", CommandType.StoredProcedure)
            If .Reader IsNot Nothing Then
                While .Reader.Read
                    resultado.Add(New ResultadoProceso(.Reader("valor").ToString, .Reader("mensaje").ToString))
                    .Reader.NextResult()
                End While
                Integer.TryParse(.SqlParametros("@idBodega").Value.ToString(), _idBodega)
                .Reader.Close()
            End If
        End With
        Return resultado
    End Function

    Public Function RegistrarDatos() As List(Of ResultadoProceso)
        Dim resultado As New List(Of ResultadoProceso)
        If _dbManager Is Nothing Then _dbManager = New LMDataAccess
        With _dbManager
            With .SqlParametros
                .Clear()
                .Add("@iccid", SqlDbType.VarChar, 20).Value = _iccid
                .Add("@numeroRadicado", SqlDbType.BigInt).Value = _numeroRadicado
                .Add("@idBloqueo", SqlDbType.Int).Value = _idBloqueo
            End With
            .ejecutarReader("RegistrarBloqueoSerialCEM", CommandType.StoredProcedure)
            If .Reader IsNot Nothing Then
                While .Reader.Read
                    resultado.Add(New ResultadoProceso(.Reader("valor").ToString, .Reader("mensaje").ToString))
                End While
                .Reader.Close()
            End If
        End With
        Return resultado
    End Function

    Public Function CargarDatos() As DataTable
        Dim dtDatos As New DataTable
        If _dbManager IsNot Nothing Then _dbManager = New LMDataAccess
        Try
            With _dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@iccid", SqlDbType.VarChar, 20).Value = _iccid
                End With
                dtDatos = .ejecutarDataTable("ReporteBloqueoCEM", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

#End Region

End Class
