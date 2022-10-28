Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras

Namespace Recibos
    Public Class TipoUnidad

#Region "variables"
        Private _idTipoUnidad As Integer
        Private _descripcion As String
        Private _estado As Boolean
#End Region

#Region "propiedades"
        Public ReadOnly Property IdTipoUnidad() As Integer
            Get
                Return _idTipoUnidad
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
#End Region

#Region "constructores"
        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idTipoUnidad As Integer)
            MyBase.New()
            _idTipoUnidad = idTipoUnidad
            CargarDatos()
        End Sub
#End Region

#Region "metodos Privados"
        Private Sub CargarDatos()
            Dim db As New LMDataAccess
            db.SqlParametros.Add("@idTipoUnidad", SqlDbType.BigInt).Value = _idTipoUnidad
            Try
                db.ejecutarReader("ObtenerInfoTipoUnidad", CommandType.StoredProcedure)
                If db.Reader.Read Then
                    _descripcion = db.Reader("descripcion").ToString
                End If
            Catch ex As Exception
            Finally
                If Not db.Reader.IsClosed Then db.Reader.Close()
                db.Dispose()
            End Try
        End Sub
#End Region

#Region "metodos Compartidos"
        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroTipoUnidad
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroTipoUnidad) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdTipoUnidad > 0 Then db.SqlParametros.Add("@idTipoUnidad", SqlDbType.SmallInt).Value = .IdTipoUnidad
                If .Descripcion > 0 Then db.SqlParametros.Add("@descripcion", SqlDbType.VarChar).Value = .Descripcion.ToString
                If .Activo > 0 Then db.SqlParametros.Add("@estado", SqlDbType.Bit).Value = .Activo                
                dtDatos = db.ejecutarDataTable("ObtenerInfoTipoUnidad", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos

        End Function

#End Region
    End Class
End Namespace

