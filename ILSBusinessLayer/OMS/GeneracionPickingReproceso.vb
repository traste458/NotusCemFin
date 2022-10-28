Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.IO

Public Class GeneracionPickingReproceso

#Region "Atributos (campos)"

    Dim _idPickingInstruccionReproceso As Integer
    Dim _idInstruccionReproceso As Integer
    Dim _idEstado As Integer
    Dim _estado As String
    Dim _fechaCreacion As DateTime
    Dim _idUsuarioCreacion As Integer
    Dim _usuarioCreacion As String
    Dim _idInstruccionesReprocesoLista As ArrayList

    Dim _detallePickingTable As DataTable
    Dim _registrado As Boolean

#End Region

#Region "Propiedades"

    Public Property IdPickingInstruccionReproceso() As Integer
        Get
            Return _idPickingInstruccionReproceso
        End Get
        Set(ByVal value As Integer)
            _idPickingInstruccionReproceso = value
        End Set
    End Property

    Public Property IdInstruccionReproceso() As Integer
        Get
            Return _idInstruccionReproceso
        End Get
        Set(ByVal value As Integer)
            _idInstruccionReproceso = value
        End Set
    End Property

    Public Property IdEstado() As Integer
        Get
            Return _idEstado
        End Get
        Set(ByVal value As Integer)
            _idEstado = value
        End Set
    End Property

    Public Property Estado() As String
        Get
            Return _estado
        End Get
        Set(ByVal value As String)
            _estado = value
        End Set
    End Property

    Public Property FechaCreacion() As DateTime
        Get
            Return _fechaCreacion
        End Get
        Set(ByVal value As DateTime)
            _fechaCreacion = value
        End Set
    End Property

    Public Property IdUsuarioCreacion() As Integer
        Get
            Return _idUsuarioCreacion
        End Get
        Set(ByVal value As Integer)
            _idUsuarioCreacion = value
        End Set
    End Property

    Public Property UsuarioCreacion() As String
        Get
            Return _usuarioCreacion
        End Get
        Set(ByVal value As String)
            _usuarioCreacion = value
        End Set
    End Property

    Public Property DetallePickingTable() As DataTable
        Get
            Return _detallePickingTable
        End Get
        Set(ByVal value As DataTable)
            _detallePickingTable = value
        End Set
    End Property

    Public Property Registrado() As Boolean
        Get
            Return _registrado
        End Get
        Set(ByVal value As Boolean)
            _registrado = value
        End Set
    End Property

    Public Property IdInstruccionesReprocesoLista() As ArrayList
        Get
            If _idInstruccionesReprocesoLista Is Nothing Then _idInstruccionesReprocesoLista = New ArrayList
            Return _idInstruccionesReprocesoLista
        End Get
        Set(ByVal value As ArrayList)
            _idInstruccionesReprocesoLista = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _estado = ""
        _usuarioCreacion = ""
    End Sub

    Public Sub New(ByVal IdPickingInstruccionReproceso As Integer)
        MyBase.New()
        _idPickingInstruccionReproceso = IdPickingInstruccionReproceso
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Add("@idPickingInstruccionReproceso", SqlDbType.Int).Value = _idPickingInstruccionReproceso

                .ejecutarReader("ObtenerInformacionPickingReproceso", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    If .Reader.Read Then
                        Integer.TryParse(.Reader("idPickingInstruccionReproceso"), _idPickingInstruccionReproceso)
                        Integer.TryParse(.Reader("idInstruccionReproceso"), _idInstruccionReproceso)
                        Integer.TryParse(.Reader("idEstado"), _idEstado)
                        If Not IsDBNull(.Reader("estado")) Then _estado = .Reader("estado").ToString
                        If Not IsDBNull(.Reader("fechaCreacion")) Then _fechaCreacion = CDate(.Reader("fechaCreacion"))
                        Integer.TryParse(.Reader("idUsuarioCreacion"), _idUsuarioCreacion)
                        If Not IsDBNull(.Reader("usuarioCreacion")) Then _usuarioCreacion = .Reader("usuarioCreacion").ToString

                        .Reader.Close()

                        _detallePickingTable = .ejecutarDataTable("ConsultarPickingDetalleReproceso", CommandType.StoredProcedure)

                        _registrado = True

                    End If
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function Registrar(ByVal idUsuario As Integer) As DataTable
        Dim dtResultado As New DataTable
        Dim noResultado As Integer = -1

        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                With .SqlParametros
                    .Add("@listaInstruccion", SqlDbType.VarChar, 650).Value = Join(_idInstruccionesReprocesoLista.ToArray(), ",")
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With

                .iniciarTransaccion()
                dtResultado = .ejecutarDataTable("RegistrarPickingReproceso", CommandType.StoredProcedure)
                Integer.TryParse(.SqlParametros("@resultado").Value.ToString(), noResultado)
                If dtResultado.Rows.Count = 0 Then
                    .confirmarTransaccion()
                Else
                    .abortarTransaccion()
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message)
            End Try
        End With
        Return dtResultado
    End Function

    Public Function Actualizar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim noResultado As Integer = -1

        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                With .SqlParametros
                    .Add("@idPickingInstruccionReproceso", SqlDbType.Int).Value = _idPickingInstruccionReproceso
                    If _idInstruccionReproceso > 0 Then .Add("@idInstruccionReproceso", SqlDbType.Int).Value = _idInstruccionReproceso
                    If _idEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                .iniciarTransaccion()
                .ejecutarNonQuery("ActualizarPickingReproceso", CommandType.StoredProcedure)

                Integer.TryParse(.SqlParametros("@resultado").Value.ToString(), noResultado)

                If noResultado = 0 Then
                    .confirmarTransaccion()
                    resultado.EstablecerMensajeYValor(0, "Se realizo la actualización satisfactoriamente.")
                ElseIf noResultado = 1 Then
                    .abortarTransaccion()
                    resultado.EstablecerMensajeYValor(1, "No se encontro el identificador del picking consultado, por favor intente nuevamente.")
                Else
                    .abortarTransaccion()
                    resultado.EstablecerMensajeYValor(9, "Se generó un error inesperado al realizar la actualización, por favor intente el registro nuevamente.")
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message)
            End Try
        End With

        Return resultado
    End Function

#End Region

#Region "Enumerados"

    Public Enum Estados
        Creada = 1
    End Enum
#End Region

End Class
