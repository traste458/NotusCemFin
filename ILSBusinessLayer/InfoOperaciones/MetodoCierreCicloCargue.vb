Imports LMDataAccessLayer
Imports ILSBusinessLayer.Enumerados

Public Class MetodoCierreCicloCargue

#Region "Variables Privadas"

    Private _idMetodo As Integer
    Private _nombre As String
    Private _descripcion As String
    Private _activo As Boolean

#End Region

#Region "Propiedades"

    Public Property IdMetodo() As Integer
        Get
            Return _idMetodo
        End Get
        Set(ByVal value As Integer)
            _idMetodo = value
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

    Public Property Descripcion() As String
        Get
            Return _descripcion
        End Get
        Set(ByVal value As String)
            _descripcion = value
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

    Public Sub New(ByVal idMetodo As Integer)
        MyBase.New()
        _idMetodo = idMetodo
        CargarDatos(idMetodo)
    End Sub

#End Region

#Region "Metodos Privados"

    Private Sub CargarDatos(ByVal idMetodo As Integer)
        Dim db As New LMDataAccess
        Try
            If _idMetodo > 0 Then
                db.SqlParametros.Add("@idMetodo", SqlDbType.Int).Value = idMetodo
                db.ejecutarReader("ObtenerMetodoCierreCicloCargue", CommandType.StoredProcedure)
                If Not db.Reader Is Nothing Then
                    If db.Reader.Read Then
                        _idMetodo = idMetodo
                        _nombre = db.Reader("nombre").ToString()
                        _descripcion = db.Reader("descripcion").ToString()
                        Boolean.TryParse(db.Reader("activo").ToString(), _activo)
                    End If
                    db.Reader.Close()
                End If
            Else
                Throw New Exception("No se ha establecido ningún metodo.")
            End If
        Catch ex As Exception
            Throw New Exception("Error al cargar los datos. " & ex.Message)
        Finally
            If Not db Is Nothing Then db.Dispose()
        End Try
    End Sub

#End Region

#Region "Metodos Compartidos"

    Public Shared Function ObtenerTodos() As DataTable
        Dim dt As New DataTable
        Dim db As New LMDataAccess
        Try
            dt = db.ejecutarDataTable("ObtenerMetodoCierreCicloCargue", CommandType.StoredProcedure)
        Catch ex As Exception
            Throw New Exception("Error al obtener todos. " & ex.Message)
        Finally
            If Not db Is Nothing Then db.Dispose()
        End Try
        Return dt
    End Function

    Public Shared Function ObtenerTodos(ByVal filtro As MetodoCierreCicloCargue.Filtro) As DataTable
        Dim dt As New DataTable
        Dim db As New LMDataAccess
        Try
            With filtro
                If .IdMetodo > 0 Then db.SqlParametros.Add("@idMetodo", SqlDbType.Int).Value = .IdMetodo
                If Not String.IsNullOrEmpty(.Nombre) Then db.SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = .Nombre
                If Not String.IsNullOrEmpty(.Descripcion) Then db.SqlParametros.Add("@descripcion", SqlDbType.VarChar).Value = .Descripcion
                If .Activo <> EstadoBinario.NoEstablecido Then db.SqlParametros.Add("@activo", SqlDbType.Bit).Value = .Activo
            End With
            dt = db.ejecutarDataTable("ObtenerMetodoCierreCicloCargue", CommandType.StoredProcedure)
        Catch ex As Exception
        Finally
            If Not db Is Nothing Then
                db.Dispose()
            End If
        End Try
        Return dt
    End Function

    Public Shared Function ObtenerDetalle(ByVal idMetodo As Integer, ByVal idUsuario As Integer, Optional ByRef cantidadProgramada As Object = Nothing) As DataTable
        Dim db As New LMDataAccess
        Dim dt As New DataTable
        Try
            db.TiempoEsperaComando = 600000
            db.SqlParametros.Add("@idMetodo", SqlDbType.Int).Value = idMetodo
            db.SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
            If Not cantidadProgramada Is Nothing Then db.SqlParametros.Add("@cantidadProgramada", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            dt = db.ejecutarDataTable("ObtenerDetalleMetodoAgrupacionCierreCiclo", CommandType.StoredProcedure)
            If Not cantidadProgramada Is Nothing Then Integer.TryParse(db.SqlParametros("@cantidadProgramada").Value.ToString(), cantidadProgramada)
        Catch ex As Exception
            Throw New Exception("Error al obtener el detalle de agrupación. " & ex.Message)
        Finally
            If Not db Is Nothing Then db.Dispose()
        End Try
        Return dt
    End Function

    Public Shared Function Procesar(ByVal idUsuario As Integer, ByVal tipo As TipoProceso, Optional ByVal idMetodo As Integer = 0, Optional ByVal agrupado As ArrayList = Nothing) As Integer
        Dim db As New LMDataAccess
        Dim idProgramado As Integer
        Try
            db.TiempoEsperaComando = 600000
            If idMetodo > 0 Then
                db.SqlParametros.Add("@idMetodo", SqlDbType.Int).Value = idMetodo
                db.SqlParametros.Add("@agrupados", SqlDbType.VarChar, 1000).Value = Join(agrupado.ToArray, ",")
                If tipo = TipoProceso.Programar Then
                    db.SqlParametros.Add("@idProgramado", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End If
            End If

            db.SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
            db.SqlParametros.Add("@idAccion", SqlDbType.SmallInt).Value = tipo
            db.ejecutarNonQuery("EjecutarCierreCicloCargue", CommandType.StoredProcedure)
            If tipo = TipoProceso.Programar Then
                Integer.TryParse(db.SqlParametros("@idProgramado").Value.ToString(), idProgramado)
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If Not db Is Nothing Then db.Dispose()
        End Try
        Return idProgramado
    End Function

#End Region

#Region "Estructuras"

    Public Structure Filtro
        Public IdMetodo As Integer
        Public Nombre As String
        Public Descripcion As String
        Public Activo As EstadoBinario
    End Structure

    Public Enum TipoProceso
        Cargar = 1
        Programar = 2
    End Enum

#End Region


End Class
