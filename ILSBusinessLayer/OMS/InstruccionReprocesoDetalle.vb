Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.IO

Public Class InstruccionReprocesoDetalle

#Region "Atributos (campos)"

    Private _idInstruccionReprocesoDetalle As Integer
    Private _idInstruccionReproceso As Integer
    Private _idRegionOrigen As Integer
    Private _regionOrigen As String
    Private _idRegionDestino As Integer
    Private _regionDestino As String
    Private _materialOrigen As String
    Private _descMaterialOrigen As String
    Private _materialDestino As String
    Private _descMaterialDestino As String
    Private _cantidad As Integer
    Private _idEstado As Integer
    Private _estado As String
    Private _fechaCreacion As Date
    Private _idModificador As Integer
    Private _factura As String
    Private _tipoInstruccion As String
    Private _cantidadDisponible As Integer
    Private _flagEliminacion As Integer
    Private _idTipoProducto As Integer

    Private _registrado As Boolean


#End Region

#Region "Propiedades"

    Public Property IdInstruccion() As Integer
        Get
            Return _idInstruccionReprocesoDetalle
        End Get
        Set(ByVal value As Integer)
            _idInstruccionReprocesoDetalle = value
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

    Public Property IdRegionOrigen() As Integer
        Get
            Return _idRegionOrigen
        End Get
        Set(ByVal value As Integer)
            _idRegionOrigen = value
        End Set
    End Property

    Public Property RegionOrigen() As String
        Get
            Return _regionOrigen
        End Get
        Set(ByVal value As String)
            _regionOrigen = value
        End Set
    End Property

    Public Property IdRegionDestino() As Integer
        Get
            Return _idRegionDestino
        End Get
        Set(ByVal value As Integer)
            _idRegionDestino = value
        End Set
    End Property

    Public Property RegionDestino() As String
        Get
            Return _regionDestino
        End Get
        Set(ByVal value As String)
            _regionDestino = value
        End Set
    End Property

    Public Property MaterialOrigen() As String
        Get
            Return _materialOrigen
        End Get
        Set(ByVal value As String)
            _materialOrigen = value
        End Set
    End Property

    Public Property DescMaterialOrigen() As String
        Get
            Return _descMaterialOrigen
        End Get
        Set(ByVal value As String)
            _descMaterialOrigen = value
        End Set
    End Property

    Public Property MaterialDestino() As String
        Get
            Return _materialDestino
        End Get
        Set(ByVal value As String)
            _materialDestino = value
        End Set
    End Property

    Public Property DescMaterialDestino() As String
        Get
            Return _descMaterialDestino
        End Get
        Set(ByVal value As String)
            _descMaterialDestino = value
        End Set
    End Property

    Public Property Cantidad() As Integer
        Get
            Return _cantidad
        End Get
        Set(ByVal value As Integer)
            _cantidad = value
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

    Public Property IdModificador() As Integer
        Get
            Return _idModificador
        End Get
        Set(ByVal value As Integer)
            _idModificador = value
        End Set
    End Property

    Public Property FechaCreacion() As Date
        Get
            Return _fechaCreacion
        End Get
        Set(ByVal value As Date)
            _fechaCreacion = value
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

    Public Property Factura() As String
        Get
            Return _factura
        End Get
        Set(ByVal value As String)
            _factura = value
        End Set
    End Property

    Public Property TipoInstruccion() As String
        Get
            Return _tipoInstruccion
        End Get
        Set(ByVal value As String)
            _tipoInstruccion = value
        End Set
    End Property

    Public Property CantidadDisponible As Integer
        Get
            Return _cantidadDisponible
        End Get
        Set(value As Integer)
            _cantidadDisponible = value
        End Set
    End Property

    Public Property FlagEliminacion As Integer
        Get
            Return _flagEliminacion
        End Get
        Set(value As Integer)
            _flagEliminacion = value
        End Set
    End Property

    Public Property IdTipoProducto As Integer
        Get
            Return _idTipoProducto
        End Get
        Set(value As Integer)
            _idTipoProducto = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _regionOrigen = ""
        _regionDestino = ""
        _materialOrigen = ""
        _descMaterialOrigen = ""
        _materialDestino = ""
        _descMaterialDestino = ""
        _estado = ""
    End Sub

    Public Sub New(ByVal idInstruccionReprocesoDetalle As Integer)
        MyBase.New()
        _idInstruccionReprocesoDetalle = idInstruccionReprocesoDetalle
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Add("@idInstruccionReprocesoDetalle", SqlDbType.Int).Value = _idInstruccionReprocesoDetalle

                .ejecutarReader("ConsultarInstruccionDetalle", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    If .Reader.Read Then
                        CargarResultadoConsulta(.Reader)
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

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Return resultado
    End Function

    Public Function Actualizar(Optional justificacion As String = "") As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim noResultado As Integer = -1
        If _idModificador > 0 Then
            Dim dbManager As New LMDataAccess
            With dbManager
                Try
                    With .SqlParametros
                        .Add("@idInstruccionReprocesoDetalle", SqlDbType.Int).Value = _idInstruccionReprocesoDetalle
                        .Add("@idModificador", SqlDbType.Int).Value = _idModificador
                        If _idInstruccionReproceso > 0 Then .Add("@idInstruccionReproceso", SqlDbType.Int).Value = _idInstruccionReproceso
                        If _idRegionOrigen > 0 Then .Add("@idRegionOrigen", SqlDbType.Int).Value = _idRegionOrigen
                        If _idRegionDestino > 0 Then .Add("@idRegionDestino", SqlDbType.Int).Value = _idRegionDestino
                        If Not String.IsNullOrEmpty(_materialOrigen) Then .Add("@materialOrigen", SqlDbType.VarChar, 450).Value = _materialOrigen
                        If Not String.IsNullOrEmpty(_materialOrigen) Then .Add("@materialDestino", SqlDbType.VarChar, 450).Value = _materialDestino
                        If Not String.IsNullOrEmpty(justificacion) Then .Add("@justificacion", SqlDbType.VarChar, 450).Value = justificacion
                        If _cantidad > 0 Then .Add("@cantidad", SqlDbType.Int).Value = _cantidad
                        If _idEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                    End With
                    .iniciarTransaccion()
                    .ejecutarNonQuery("ActualizarInstruccionReprocesoDetalle", CommandType.StoredProcedure)

                    Integer.TryParse(.SqlParametros("@resultado").Value.ToString(), noResultado)

                    If noResultado = 0 Then
                        .confirmarTransaccion()
                        resultado.EstablecerMensajeYValor(0, "Se realizo la actualización satisfactoriamente.")
                    ElseIf noResultado = 1 Then
                        .abortarTransaccion()
                        resultado.EstablecerMensajeYValor(1, "No se encontro el identificador de la instrucción consultada, por favor intente nuevamente.")
                    ElseIf noResultado = 2 Then
                        .abortarTransaccion()
                        resultado.EstablecerMensajeYValor(2, "El detalle de la instrucción ya cuenta con seriales leidos, no se puede realizar la anulación.")
                    Else
                        .abortarTransaccion()
                        resultado.EstablecerMensajeYValor(9, "Se generó un error inesperado al realizar la actualización, por favor intente el registro nuevamente.")
                    End If
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message)
                End Try
            End With
        Else
            resultado.EstablecerMensajeYValor(10, "No se han proporcionado todos los datos requeridos para actualizar el registro. ")
        End If
        Return resultado

    End Function

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idInstruccionReprocesoDetalle"), _idInstruccionReprocesoDetalle)
                Integer.TryParse(reader("idInstruccionReproceso"), _idInstruccionReproceso)
                Integer.TryParse(reader("idRegionOrigen"), _idRegionOrigen)
                If Not IsDBNull(reader("regionOrigen")) Then _regionOrigen = reader("regionOrigen").ToString
                Integer.TryParse(reader("idRegionDestino"), _idRegionDestino)
                If Not IsDBNull(reader("regionDestino")) Then _regionDestino = reader("regionDestino").ToString
                If Not IsDBNull(reader("materialOrigen")) Then _materialOrigen = reader("materialOrigen").ToString
                If Not IsDBNull(reader("descMaterialOrigen")) Then _descMaterialOrigen = reader("descMaterialOrigen").ToString
                If Not IsDBNull(reader("materialDestino")) Then _materialDestino = reader("materialDestino").ToString
                If Not IsDBNull(reader("descMaterialDestino")) Then _descMaterialDestino = reader("descMaterialDestino").ToString
                Integer.TryParse(reader("cantidad"), _cantidad)
                Integer.TryParse(reader("idEstado"), _idEstado)
                If Not IsDBNull(reader("estado")) Then _estado = reader("estado").ToString
                If Not IsDBNull(reader("fechaCreacion")) Then _fechaCreacion = CDate(reader("fechaCreacion"))
                If Not IsDBNull(reader("factura")) Then _factura = reader("factura").ToString
                If Not IsDBNull(reader("tipoInstruccion")) Then _tipoInstruccion = reader("tipoInstruccion").ToString
                Integer.TryParse(reader("cantidadDisponible"), _cantidadDisponible)
                Integer.TryParse(reader("flagEliminacion"), _flagEliminacion)
                Integer.TryParse(reader("idTipoProducto"), _idTipoProducto)
            End If
        End If

    End Sub

#End Region

#Region "Enumerados"

    Public Enum Estados
        Creada = 147
        Proceso = 148
        Cerrada = 149
        Anulada = 150
    End Enum

#End Region

End Class
