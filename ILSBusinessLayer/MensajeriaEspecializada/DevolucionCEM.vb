Imports LMDataAccessLayer

Public Class DevolucionCEM

#Region "Atributos (Campos)"

    Private _numeroRadicado As Long
    Private _idServicio As Long
    Private _idUsuario As Integer
    Private _numeroRuta As Integer
    Private _idNovedad As Integer
    Private _idMotorizado As Integer
    Private _fechaInicial As Date
    Private _fechaFinal As Date
    Private _dbManager As New LMDataAccess

#End Region

#Region "Propiedades"

    Public Property NumeroRadicado() As Long
        Get
            Return _numeroRadicado
        End Get
        Set(ByVal value As Long)
            _numeroRadicado = value
        End Set
    End Property

    Public Property IdServicio As Long
        Get
            Return _idServicio
        End Get
        Set(value As Long)
            _idServicio = value
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

    Public Property NumeroRuta() As Integer
        Get
            Return _numeroRuta
        End Get
        Set(ByVal value As Integer)
            _numeroRuta = value
        End Set
    End Property

    Public Property IdNovedad() As Integer
        Get
            Return _idNovedad
        End Get
        Set(ByVal value As Integer)
            _idNovedad = value
        End Set
    End Property

    Public Property IdMororizado() As Integer
        Get
            Return _idMotorizado
        End Get
        Set(ByVal value As Integer)
            _idMotorizado = value
        End Set
    End Property

    Public Property FechaInicial() As Date
        Get
            Return _fechaInicial
        End Get
        Set(ByVal value As Date)
            _fechaInicial = value
        End Set
    End Property

    Public Property FechaFinal() As Date
        Get
            Return _fechaFinal
        End Get
        Set(ByVal value As Date)
            _fechaFinal = value
        End Set
    End Property

    Property CodigoSucursal As Integer

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function RegistrarDevolucion() As List(Of ResultadoProceso)
        Dim lstResultado As New List(Of ResultadoProceso)
        If _dbManager IsNot Nothing Then _dbManager = New LMDataAccess
        Try
            With _dbManager
                With .SqlParametros
                    .Clear()
                    If _numeroRadicado > 0 Then .Add("@numeroRadicado", SqlDbType.BigInt).Value = _numeroRadicado
                    If _idServicio > 0 Then .Add("@idServicio", SqlDbType.BigInt).Value = _idServicio
                    If Me._numeroRuta > 0 Then .Add("@numeroRuta", SqlDbType.Int).Value = Me._numeroRuta
                    .Add("@idUsuario", SqlDbType.Int).Value = Me._idUsuario
                    If IdNovedad > 0 Then .Add("@idNovedad", SqlDbType.Int).Value = Me._idNovedad
                    If _CodigoSucursal > 0 Then .Add("@codigoSucursal", SqlDbType.Int).Value = Me._CodigoSucursal
                End With
                .ejecutarReader("RegistraDevolucionCEM", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    While .Reader.Read
                        lstResultado.Add(New ResultadoProceso(.Reader("valor").ToString, .Reader("mensaje").ToString))
                    End While
                    .Reader.Close()
                End If
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return lstResultado
    End Function

    Public Function RegistrarDevolucionParcial() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Clear()
                        .Add("@numeroRadicado", SqlDbType.Int).Value = NumeroRadicado
                        .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .iniciarTransaccion()
                    .ejecutarNonQuery("RegistraDevolucionParcialCEM", CommandType.StoredProcedure)

                    If .SqlParametros("@resultado").Value = 0 Then
                        .confirmarTransaccion()
                    Else
                        .abortarTransaccion()
                        Select Case .SqlParametros("@resultado").Value
                            Case 1
                                resultado.EstablecerMensajeYValor(1, "El número de radicado no se encuentra en estado Entregado o con seriales devueltos.")
                            Case 2
                                resultado.EstablecerMensajeYValor(2, "El número de radicado digitado no esta asignado a la ruta relacionada.")
                        End Select
                    End If
                End With
            Catch ex As Exception
                dbManager.abortarTransaccion()
                If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw ex
            End Try
        End Using
        Return resultado
    End Function

    Public Function CargarReporte() As DataTable
        Dim dtDatos As New DataTable
        If _dbManager IsNot Nothing Then _dbManager = New LMDataAccess
        Try
            With _dbManager
                With .SqlParametros
                    .Clear()
                    If NumeroRadicado > 0 Then .Add("@numeroRadicado", SqlDbType.BigInt).Value = NumeroRadicado
                    If _fechaInicial > Date.MinValue AndAlso _fechaFinal > Date.MinValue Then
                        .Add("@fechaInicial", SqlDbType.DateTime).Value = _fechaInicial
                        .Add("@fechaFinal", SqlDbType.DateTime).Value = _fechaFinal
                    End If
                    If IdMororizado > 0 Then .Add("@idMotorizado", SqlDbType.Int).Value = IdMororizado
                End With
                dtDatos = .ejecutarDataTable("ReporteDevolucionesCEM", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function
    
#End Region

End Class
