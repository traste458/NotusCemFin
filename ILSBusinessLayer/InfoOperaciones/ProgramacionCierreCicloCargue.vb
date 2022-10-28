Imports LMDataAccessLayer
Imports ILSBusinessLayer.Enumerados

Public Class ProgramacionCierreCicloCargue

#Region "Variables"

    Private _idPrograma As Long
    Private _fecha As DateTime
    Private _idUsuario As Integer
    Private _activo As Boolean

#End Region

#Region "Propiedades"

    Public Property IdPrograma() As Long
        Get
            Return _idPrograma
        End Get
        Set(ByVal value As Long)
            _idPrograma = value
        End Set
    End Property

    Public Property Fecha() As DateTime
        Get
            Return _fecha
        End Get
        Set(ByVal value As DateTime)
            _fecha = value
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

    Public Property Activo() As Boolean
        Get
            Return _activo
        End Get
        Set(ByVal value As Boolean)
            _activo = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idPrograma As Long)
        MyBase.New()
        _idPrograma = idPrograma
        CargarDatos(idPrograma)
    End Sub

#End Region

#Region "Metodos Privados"

    Private Sub CargarDatos(ByVal idPrograma As Long)
        Dim db As New LMDataAccess
        Try
            db.SqlParametros.Add("@idPrograma", SqlDbType.BigInt).Value = idPrograma
            db.ejecutarReader("ObtenerDatosProgramadoCierreCiclo", CommandType.StoredProcedure)
            If db.Reader.Read Then
                DateTime.TryParse(db.Reader("fecha").ToString(), _fecha)
                Integer.TryParse(db.Reader("idUsuario").ToString(), _idUsuario)
                Boolean.TryParse(db.Reader("activo").ToString(), _activo)
            End If
            db.Reader.Close()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If Not db Is Nothing Then db.Dispose()
        End Try
    End Sub

#End Region

#Region "Metodos Publicos"

    Public Sub Anular()
        Dim db As New LMDataAccess
        Try
            If _idPrograma > 0 Then
                db.SqlParametros.Add("@idProgramado", SqlDbType.BigInt).Value = _idPrograma
                db.ejecutarNonQuery("AnularProgramacionCierreCiclo", CommandType.StoredProcedure)
            Else
                Throw New Exception("Objeto sin establecer valor.")
            End If
        Catch ex As Exception
            Throw New Exception("Error al anular. " & ex.Message)
        Finally
            If Not db Is Nothing Then db.Dispose()
        End Try
    End Sub

#End Region

#Region "Metodos Compartidos"

    Public Shared Function ObtenerTodos() As DataTable
        Dim db As New LMDataAccess
        Dim dt As New DataTable
        Try
            dt = db.ejecutarDataTable("ObtenerDatosProgramadoCierreCiclo", CommandType.StoredProcedure)
        Catch ex As Exception
            Throw New Exception("Error al obtener todos. " & ex.Message)
        Finally
            If Not db Is Nothing Then db.Dispose()
        End Try
        Return dt
    End Function

    Public Shared Function ObtenerTodos(ByVal filtrar As ProgramacionCierreCicloCargue.Filtro) As DataTable
        Dim db As New LMDataAccess
        Dim dt As New DataTable
        Try
            With filtrar
                If .IdProgramado > 0 Then db.SqlParametros.Add("@idProgramado", SqlDbType.BigInt).Value = .IdProgramado
                If .IdUsuario > 0 Then db.SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = .IdUsuario
                If .Activo <> EstadoBinario.NoEstablecido Then db.SqlParametros.Add("@activo", SqlDbType.Bit).Value = .Activo
            End With
            dt = db.ejecutarDataTable("ObtenerDatosProgramadoCierreCiclo", CommandType.StoredProcedure)
        Catch ex As Exception
            Throw New Exception("Error al obtener todos. " & ex.Message)
        Finally
            If Not db Is Nothing Then db.Dispose()
        End Try
        Return dt
    End Function

#End Region

#Region "Estructuras"

    Public Structure Filtro
        Public IdProgramado As Long        
        Public IdUsuario As Integer
        Public Activo As EstadoBinario
    End Structure

#End Region

End Class
