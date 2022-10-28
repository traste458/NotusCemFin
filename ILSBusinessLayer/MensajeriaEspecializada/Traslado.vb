Imports LMDataAccesLayer
Imports LMDataAccessLayer

Public Class Traslado
#Region "Variables"
    Private _idTraslado As Long
    Private _idServicio As Long
    Private _numeroRadicado As Long
    Private _idTipoTraslado As Integer
    Private _idUsuario As Integer
    Private _fecha As DateTime
#End Region

#Region "Propiedades"
    Public ReadOnly Property IdTraslado() As Long
        Get
            Return _idTipoTraslado
        End Get
    End Property

    Public Property IdServicio() As Long
        Get
            Return _idServicio
        End Get
        Set(ByVal value As Long)
            _idServicio = value
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

    Public Property IdTipoTraslado() As Integer
        Get
            Return _idTipoTraslado
        End Get
        Set(ByVal value As Integer)
            _idTipoTraslado = value
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

    Public ReadOnly Property Fecha() As DateTime
        Get
            Return _fecha
        End Get
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idTraslado As Integer)
        MyBase.New()
        _idTraslado = idTraslado
        CargarDatos()
    End Sub

    Public Sub New(ByVal numeroRadicado As Long)
        MyBase.New()
        _numeroRadicado = numeroRadicado
        CargarDatos()
    End Sub

#End Region

#Region "Metodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If _idTraslado > 0 Or _numeroRadicado > 0 Then
                    If _idTraslado > 0 Then .SqlParametros.Add("@idTraslado", SqlDbType.Int).Value = _idTraslado
                    If _numeroRadicado > 0 Then .SqlParametros.Add("@numeroRadicado", SqlDbType.BigInt).Value = _numeroRadicado
                    .ejecutarReader("ObtenerTrasladoServicioMensajeria", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then CargarResultadoConsulta(.Reader)
                        .Reader.Close()
                    End If
                Else
                    Throw New Exception("No se ha establecido ningun dato para el objeto de traslado. ")
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

#Region "Metodos Publicos"

    Public Sub Crear()
        'Using dbManager As New LMDataAccess
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicio
                .SqlParametros.Add("@numeroRadicado", SqlDbType.BigInt).Value = _numeroRadicado
                .SqlParametros.Add("@idTipoTraslado", SqlDbType.Int).Value = _idTipoTraslado
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario                
                .SqlParametros.Add("@idTraslado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                .iniciarTransaccion()
                .ejecutarNonQuery("AdicionarMsisdnServicioMensajeria", CommandType.StoredProcedure)

                Integer.TryParse(.SqlParametros("@idTraslado").Value.ToString(), _idTraslado)


                .confirmarTransaccion()
            Catch ex As Exception
                .abortarTransaccion()
                Throw ex
            End Try
        End With
        dbManager.Dispose()
        'End Using
    End Sub

    Public Sub Crear(ByVal detalle As DetalleTrasladoColeccion)
        'Using dbManager As New LMDataAccess
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicio
                .SqlParametros.Add("@numeroRadicado", SqlDbType.BigInt).Value = _numeroRadicado
                .SqlParametros.Add("@idTipoTraslado", SqlDbType.Int).Value = _idTipoTraslado
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                .SqlParametros.Add("@idTraslado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                .iniciarTransaccion()
                .ejecutarNonQuery("CrearTrasladoServicioMensajeria", CommandType.StoredProcedure)

                Integer.TryParse(.SqlParametros("@idTraslado").Value.ToString(), _idTraslado)
                If _idTraslado > 0 Then                    
                    detalle.Registrar(dbManager, _idTraslado)
                End If

                .confirmarTransaccion()
            Catch ex As Exception
                .abortarTransaccion()
                Throw ex
            End Try
        End With
        dbManager.Dispose()
        'End Using
    End Sub

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idTraslado").ToString, _idTraslado)
                Integer.TryParse(reader("idServicio").ToString, _idServicio)
                Long.TryParse(reader("numeroRadicado").ToString(), _numeroRadicado)
                Integer.TryParse(reader("idTipoTraslado").ToString(), _idTipoTraslado)
                Integer.TryParse(reader("idUsuario").ToString(), _idUsuario)
                DateTime.TryParse(reader("fecha").ToString(), _fecha)
            End If
        End If

    End Sub

#End Region
End Class
