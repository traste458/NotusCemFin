Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras

Namespace Recibos
    Public Class TipoRecepcion
#Region "variables"
        Private _idTipoRecepcion As Integer
        Private _descripcion As String
        Private _estado As Boolean
        Private _requiereConsignatario As Boolean
#End Region

#Region "propiedades"
        Public ReadOnly Property IdTipoRecepcion() As Integer
            Get
                Return _idTipoRecepcion
            End Get
        End Property

        Public Property Descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
        End Property

        Public Property Estado() As Boolean
            Get
                Return _estado
            End Get
            Set(ByVal value As Boolean)
                _estado = value
            End Set
        End Property

        Public Property RequiereConsignatario() As Boolean
            Get
                Return _requiereConsignatario
            End Get
            Set(ByVal value As Boolean)
                _requiereConsignatario = value
            End Set
        End Property
#End Region


#Region "constructores"
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(ByVal idTipoRecepcion As Integer)
            Me.New()
            Me.CargarDatos(idTipoRecepcion)
            _idTipoRecepcion = idTipoRecepcion
        End Sub
#End Region

#Region "metodos privados"
        Private Sub CargarDatos(ByVal idTipoRecepcion As Long)
            Dim db As New LMDataAccess
            db.SqlParametros.Add("@idTipoRecepcion", SqlDbType.Int).Value = idTipoRecepcion
            Try
                db.ejecutarReader("ObtenerTipoRecepcion", CommandType.StoredProcedure)
                If db.Reader.Read Then
                    _estado = db.Reader("estado")
                    _descripcion = db.Reader("descripcion")
                    Boolean.TryParse(db.Reader("requiereConsignatario"), _requiereConsignatario)
                End If
            Catch ex As Exception
            Finally
                If Not db.Reader.IsClosed Then db.Reader.Close()
                db.Dispose()
            End Try
        End Sub
#End Region

#Region "métodos compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroGeneral            
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroGeneral) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .Identificador > 0 Then db.SqlParametros.Add("@idTipoRecepcion", SqlDbType.BigInt).Value = .Identificador
                If .Nombre <> "" Then db.SqlParametros.Add("@descripcion", SqlDbType.VarChar).Value = .Nombre.ToString
                If .Activo > 0 Then db.SqlParametros.Add("@estado", SqlDbType.Bit).Value = IIf(filtro.Activo = 1, 1, 0)
                dtDatos = db.ejecutarDataTable("ObtenerTipoRecepcion", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos
        End Function

#End Region

    End Class
End Namespace

