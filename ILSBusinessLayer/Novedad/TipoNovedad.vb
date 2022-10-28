Imports ILSBusinessLayer.Estructuras
Imports LMDataAccessLayer
Namespace Novedad
    Public Class TipoNovedad

#Region "variables"
        Private _idTipoNovedad As Integer
        Private _descripcion As String
#End Region

#Region "propiedades"
        Public ReadOnly Property IdTipoNovedad() As Integer
            Get
                Return _idTipoNovedad
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

#End Region

#Region "constructores"

        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(ByVal idTipoNovedad As Integer)
            Me.New()
            Me.CargarDatos(idTipoNovedad)
            _idTipoNovedad = idTipoNovedad
        End Sub

#End Region

#Region "metodos privados"

        Private Sub CargarDatos(ByVal idTipoNovedad As Integer)
            Dim db As New LMDataAccess
            db.SqlParametros.Add("@idTipoNovedad", SqlDbType.Int).Value = idTipoNovedad
            Try
                db.ejecutarReader("ObtenerTipoNovedadILS", CommandType.StoredProcedure)
                If db.Reader.Read Then                    
                    _descripcion = db.Reader("descripcion").ToString
                End If
            Catch ex As Exception
            Finally
                db.Dispose()
            End Try
        End Sub

#End Region

#Region "metodos compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroTipoNovedadILS
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroTipoNovedadILS) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro                
                If .IdTipoNovedad > 0 Then db.SqlParametros.Add("@idTipoNovedad", SqlDbType.Int).Value = .IdTipoNovedad
                If .Descripcion <> "" Then db.SqlParametros.Add("@descripcion", SqlDbType.VarChar).Value = .Descripcion                
                dtDatos = db.ejecutarDataTable("ObtenerTipoNovedadILS", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos

        End Function

#End Region


    End Class
End Namespace
