Imports LMDataAccessLayer
Namespace Facturacion
    Public Class EventoFacturacion

#Region "Variables Privadas"
        Private _idEvento As Integer
        Private _evento As String
        Private _activo As Boolean
        Private _idUsuario As Integer
        Private _fechaRegistro As DateTime
#End Region

#Region "Propiedades"

        Public ReadOnly Property IdEvento() As Integer
            Get
                Return _idEvento
            End Get
        End Property

        Public Property Evento() As String
            Get
                Return _evento
            End Get
            Set(ByVal value As String)
                _evento = value
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

        Public Property IdUsuario() As Integer
            Get
                Return _idUsuario
            End Get
            Set(ByVal value As Integer)
                _idUsuario = value
            End Set
        End Property

        Public Property FechaRegistro() As DateTime
            Get
                Return _fechaRegistro
            End Get
            Set(ByVal value As DateTime)
                _fechaRegistro = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idEvento As Integer)
            Me.New()
            Me.CargarDatos(idEvento)
            _idEvento = idEvento
        End Sub

#End Region

#Region "Metodos Privados"

        Private Sub CargarDatos(ByVal idEvento As Long)
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.SqlParametros.Add("@idEvento", SqlDbType.Int).Value = idEvento
            Try
                db.ejecutarReader("ObtenerEventoFacturacion", CommandType.StoredProcedure)
                If db.Reader.Read Then
                    _evento = db.Reader("evento")
                    _activo = db.Reader("activo")
                    _idUsuario = db.Reader("idUsuario")
                    _fechaRegistro = db.Reader("fechaRegistro")
                End If
            Finally
                If Not db.Reader.IsClosed Then db.Reader.Close()
                db.Dispose()
            End Try
        End Sub

#End Region

#Region "Metodos Publicos"

#End Region

#Region "Metodos Compartidos"

        Public Shared Function ObtenerListado() As DataTable
            Dim filtro As New filtroEventoFacturable
            Return ObtenerListado(filtro)
        End Function

        Public Shared Function ObtenerListado(ByVal filtro As filtroEventoFacturable) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .idCentroCosto > 0 Then db.SqlParametros.Add("@idCentroCosto", SqlDbType.BigInt).Value = .idCentroCosto
                If .idEventoFacturable > 0 Then db.SqlParametros.Add("@idEventoFacturable", SqlDbType.BigInt).Value = .idEventoFacturable
            End With
            dtDatos = db.ejecutarDataTable("ObtenerListadoEventoFacturacion", CommandType.StoredProcedure)
            Return dtDatos
        End Function

        Public Shared Function ObtenerTipoProductosAsociados(Optional ByVal idEventoFActurable As Integer = 0) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            If idEventoFActurable > 0 Then db.agregarParametroSQL("@idEventoFacturable", idEventoFActurable, SqlDbType.Int)
            dtDatos = db.ejecutarDataTable("ObtenerEventoFacturableTipoProducto", CommandType.StoredProcedure)
            Return dtDatos
        End Function

#End Region
        Public Structure filtroEventoFacturable
            Dim idCentroCosto As Integer
            Dim idEventoFacturable As Integer
        End Structure
    End Class
End Namespace

