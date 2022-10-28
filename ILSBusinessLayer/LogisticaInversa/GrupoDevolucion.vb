Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras

Namespace LogisticaInversa
    Public Class GrupoDevolucion

#Region "Variables"

        Private _idGrupoDevolucion As Long
        Private _idGrupo As Long
        Private _idGrupoDevolucion2 As String
        Private _idCreador As Long
        Private _estado As Integer
        Private _fecha As Date
        Private _fechaCierre As Date

#End Region

#Region "Propiedades"

        Public Property IdGrupoDevolucion() As Long
            Get
                Return _idGrupoDevolucion
            End Get
            Set(ByVal value As Long)
                _idGrupoDevolucion = value
            End Set
        End Property

        Public Property IdGrupo() As Long
            Get
                Return _idGrupo
            End Get
            Set(ByVal value As Long)
                _idGrupo = value
            End Set
        End Property

        Public Property IdGrupoDevolucion2() As Long
            Get
                Return _idGrupoDevolucion2
            End Get
            Set(ByVal value As Long)
                _idGrupoDevolucion2 = value
            End Set
        End Property

        Public Property IdCreador() As Long
            Get
                Return _idCreador
            End Get
            Set(ByVal value As Long)
                _idCreador = value
            End Set
        End Property

        Public Property Estado() As Integer
            Get
                Return _estado
            End Get
            Set(ByVal value As Integer)
                _estado = value
            End Set
        End Property

        Public Property Fecha() As Date
            Get
                Return _fecha
            End Get
            Set(ByVal value As Date)
                _fecha = value
            End Set
        End Property

        Public Property FechaCierre() As Date
            Get
                Return _fechaCierre
            End Get
            Set(ByVal value As Date)
                _fechaCierre = value
            End Set
        End Property

#End Region

#Region "Constructores"

#End Region

#Region "Publicos"

#End Region

#Region "Privados"

#End Region

#Region "Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroGrupoDevolucion
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroGrupoDevolucion) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdGrupoDevolucion > 0 Then db.SqlParametros.Add("@idGrupoDevolucion", SqlDbType.BigInt).Value = .IdGrupoDevolucion
                If .IdGrupo > 0 Then db.SqlParametros.Add("@idGrupo", SqlDbType.BigInt).Value = .IdGrupo
                If .IdGrupoDevolucion2 <> String.Empty Then db.SqlParametros.Add("@idGrupoDevolucion2", SqlDbType.VarChar).Value = .IdGrupoDevolucion2
                If .IdCreador > 0 Then db.SqlParametros.Add("@idCreador", SqlDbType.BigInt).Value = .IdCreador
                If .Estado > 0 Then db.SqlParametros.Add("@estado", SqlDbType.Int).Value = .Estado
                If .FechaInicial > Date.MinValue Then db.SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = .FechaInicial
                If .FechaFinal > Date.MinValue Then db.SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = .FechaFinal
                dtDatos = db.ejecutarDataTable("ObtenerGrupoDevolucion", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos

        End Function
#End Region


    End Class
End Namespace
