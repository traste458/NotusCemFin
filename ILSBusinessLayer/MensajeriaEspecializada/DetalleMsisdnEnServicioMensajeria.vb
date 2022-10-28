Imports ILSBusinessLayer
Imports LMDataAccessLayer

Public Class DetalleMsisdnEnServicioMensajeria
    Implements IDetalleMsisdnEnServicioMensajeria

#Region "Atributos (Campos)"

    Protected _idRegistro As Integer
    Protected _idServicioMensajeria As Integer
    Protected _idTipoServicio As Integer
    Protected _msisdn As Long
    Protected _activaEquipoAnterior As Boolean
    Protected _activaEquipoAnteriorTexto As String
    Protected _comseguro As Boolean
    Protected _comseguroTexto As String
    Protected _precioConIva As Double
    Protected _precioSinIva As Double
    Protected _idClausula As Integer
    Protected _clausula As String
    Protected _registrado As Boolean
    Protected _bloquear As Boolean
    Protected _tieneCambioServicio As String
    Protected _numeroReserva As String
    Protected _idRegion As Short
    Protected _nombreRegion As String
    Protected _lista28 As Boolean
    Protected _lista28Texto As String

    Protected _idPlan As Integer
    Protected _nombrePlan As String
    Protected _fechaDevolucion As Date
    Protected _cantidadMaterial As Integer
    Protected _cantidadMaterialLeida As Integer

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idRegistro As Integer)
        MyBase.New()
        _idRegistro = idRegistro
        CargarDatos()
    End Sub

    Public Sub New(ByVal idServicio As Integer, ByVal msisdn As Long)
        MyBase.New()
        _idServicioMensajeria = idServicio
        _msisdn = msisdn
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdRegistro() As Integer Implements IDetalleMsisdnEnServicioMensajeria.IdRegistro
        Get
            Return _idRegistro
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idRegistro = value
        End Set
    End Property

    Public Property IdServicioMensajeria() As Integer Implements IDetalleMsisdnEnServicioMensajeria.IdServicioMensajeria
        Get
            Return _idServicioMensajeria
        End Get
        Set(ByVal value As Integer)
            _idServicioMensajeria = value
        End Set
    End Property

    Public Property IdTipoServicio() As Integer Implements IDetalleMsisdnEnServicioMensajeria.IdTipoServicio
        Get
            Return _idTipoServicio
        End Get
        Set(ByVal value As Integer)
            _idTipoServicio = value
        End Set
    End Property

    Public Property MSISDN() As String Implements IDetalleMsisdnEnServicioMensajeria.MSISDN
        Get
            Return _msisdn
        End Get
        Set(ByVal value As String)
            _msisdn = value
        End Set
    End Property

    Public Property ActivaEquipoAnterior() As Boolean Implements IDetalleMsisdnEnServicioMensajeria.ActivaEquipoAnterior
        Get
            Return _activaEquipoAnterior
        End Get
        Set(ByVal value As Boolean)
            _activaEquipoAnterior = value
        End Set
    End Property

    Public Property ActivaEquipoAnteriorTexto() As String Implements IDetalleMsisdnEnServicioMensajeria.ActivaEquipoAnteriorTexto
        Get
            Return _activaEquipoAnteriorTexto
        End Get
        Set(ByVal value As String)
            _activaEquipoAnteriorTexto = value
        End Set
    End Property

    Public Property Comseguro() As Boolean Implements IDetalleMsisdnEnServicioMensajeria.Comseguro
        Get
            Return _comseguro
        End Get
        Set(ByVal value As Boolean)
            _comseguro = value
        End Set
    End Property

    Public Property ComseguroTexto() As String Implements IDetalleMsisdnEnServicioMensajeria.ComseguroTexto
        Get
            Return _comseguroTexto
        End Get
        Set(ByVal value As String)
            _comseguroTexto = value
        End Set
    End Property

    Public Property PrecioConIva() As Double Implements IDetalleMsisdnEnServicioMensajeria.PrecioConIva
        Get
            Return _precioConIva
        End Get
        Set(ByVal value As Double)
            _precioConIva = value
        End Set
    End Property

    Public Property PrecioSinIva() As Double Implements IDetalleMsisdnEnServicioMensajeria.PrecioSinIva
        Get
            Return _precioSinIva
        End Get
        Set(ByVal value As Double)
            _precioSinIva = value
        End Set
    End Property

    Public Property IdClausula() As Integer Implements IDetalleMsisdnEnServicioMensajeria.IdClausula
        Get
            Return _idClausula
        End Get
        Set(ByVal value As Integer)
            _idClausula = value
        End Set
    End Property

    Public Property Clausula() As String Implements IDetalleMsisdnEnServicioMensajeria.Clausula
        Get
            Return _clausula
        End Get
        Set(ByVal value As String)
            _clausula = value
        End Set
    End Property

    Public Property Registrado() As Boolean Implements IDetalleMsisdnEnServicioMensajeria.Registrado
        Get
            Return _registrado
        End Get
        Set(ByVal value As Boolean)
            _registrado = value
        End Set
    End Property

    Public Property Bloquear As Boolean Implements IDetalleMsisdnEnServicioMensajeria.Bloquear
        Get
            Return _bloquear
        End Get
        Set(value As Boolean)
            _bloquear = value
        End Set
    End Property

    Public Property TieneCambioServicio() As String
        Get
            Return _tieneCambioServicio
        End Get
        Set(ByVal value As String)
            _tieneCambioServicio = value
        End Set
    End Property

    Public Property NumeroReserva As String Implements IDetalleMsisdnEnServicioMensajeria.NumeroReserva
        Get
            Return _numeroReserva
        End Get
        Set(value As String)
            _numeroReserva = value
        End Set
    End Property

    Public Property IdRegion As Short
        Get
            Return _idRegion
        End Get
        Set(value As Short)
            _idRegion = value
        End Set
    End Property

    Public Property NombreRegion As String Implements IDetalleMsisdnEnServicioMensajeria.NombreRegion
        Get
            Return _nombreRegion
        End Get
        Set(value As String)
            _nombreRegion = value
        End Set
    End Property

    Public Property Lista28 As Boolean Implements IDetalleMsisdnEnServicioMensajeria.Lista28
        Get
            Return _lista28
        End Get
        Set(value As Boolean)
            _lista28 = value
        End Set
    End Property

    Public Property Lista28Texto As String Implements IDetalleMsisdnEnServicioMensajeria.Lista28Texto
        Get
            Return _lista28Texto
        End Get
        Set(value As String)
            _lista28Texto = value
        End Set
    End Property

    Public Overridable Property IdPlan As Integer Implements IDetalleMsisdnEnServicioMensajeria.IdPlan
        Get
            Return _idPlan
        End Get
        Set(value As Integer)
            _idPlan = value
        End Set
    End Property

    Public Overridable Property NombrePlan As String Implements IDetalleMsisdnEnServicioMensajeria.NombrePlan
        Get
            Return _nombrePlan
        End Get
        Protected Friend Set(value As String)
            _nombrePlan = value
        End Set
    End Property

    Public Overridable Property FechaDevolucion As Date Implements IDetalleMsisdnEnServicioMensajeria.FechaDevolucion
        Get
            Return _fechaDevolucion
        End Get
        Set(value As Date)
            _fechaDevolucion = value
        End Set
    End Property

    Public Overridable Property CantidadMaterial() As Integer Implements IDetalleMsisdnEnServicioMensajeria.CantidadMaterial
        Get
            Return _cantidadMaterial
        End Get
        Set(ByVal value As Integer)
            _cantidadMaterial = value
        End Set
    End Property

    Public Overridable Property CantidadMaterialLeida() As Integer Implements IDetalleMsisdnEnServicioMensajeria.CantidadMaterialLeida
        Get
            Return _cantidadMaterialLeida
        End Get
        Protected Friend Set(ByVal value As Integer)
            _cantidadMaterialLeida = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If _idRegistro > 0 Then .SqlParametros.Add("@idRegistro", SqlDbType.Int).Value = _idRegistro
                If _idServicioMensajeria > 0 Then .SqlParametros.Add("@idServicioMensajeria", SqlDbType.Int).Value = _idServicioMensajeria
                If _msisdn > 0 Then .SqlParametros.Add("@msisdn", SqlDbType.BigInt).Value = _msisdn
                .ejecutarReader("ObtenerDetalleMsisdnEnServicioMensajeria", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    If .Reader.Read Then CargarResultadoConsulta(.Reader)
                    .Reader.Close()
                End If
            End With
        Catch ex As Exception
            Throw ex
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Overridable Function Adicionar(Optional objDataAccess As LMDataAccess = Nothing) As ResultadoProceso Implements IDetalleMsisdnEnServicioMensajeria.Adicionar
        Dim dbManager As New LMDataAccess
        Dim resultado As New ResultadoProceso

        If objDataAccess IsNot Nothing Then dbManager = objDataAccess
        With dbManager
            Try
                .SqlParametros.Clear()
                .SqlParametros.Add("@idTipoServicio", SqlDbType.Int).Value = _idTipoServicio
                .SqlParametros.Add("@msisdn", SqlDbType.BigInt).Value = _msisdn
                If _activaEquipoAnteriorTexto IsNot Nothing Then .SqlParametros.Add("@activaEquipoAnterior", SqlDbType.Bit).Value = _activaEquipoAnteriorTexto
                If _comseguroTexto IsNot Nothing Then .SqlParametros.Add("@comseguro", SqlDbType.Bit).Value = _comseguroTexto
                If _precioConIva > 0 Then .SqlParametros.Add("@precioConIva", SqlDbType.Money).Value = _precioConIva
                If _precioSinIva > 0 Then .SqlParametros.Add("@precioSinIva", SqlDbType.Money).Value = _precioSinIva
                If _idClausula > 0 Then .SqlParametros.Add("@idClausula", SqlDbType.Int).Value = _idClausula
                If _numeroReserva IsNot Nothing Then .SqlParametros.Add("@numeroReserva", SqlDbType.VarChar).Value = _numeroReserva
                .SqlParametros.Add("@lista28", SqlDbType.Bit).Value = _lista28
                .SqlParametros.Add("@idMsisdn", SqlDbType.Int).Direction = ParameterDirection.Output
                .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                If Not dbManager.estadoTransaccional Then .iniciarTransaccion()
                .ejecutarNonQuery("AdicionarMsisdnServicioMensajeria", CommandType.StoredProcedure)

                resultado.Valor = .SqlParametros("@resultado").Value
                If resultado.Valor = 0 Then
                    If objDataAccess Is Nothing Then .confirmarTransaccion()
                    _idRegistro = .SqlParametros("@idMsisdn").Value
                    resultado.Mensaje = "Registro exitoso de MSISDN"
                Else
                    .abortarTransaccion()
                    resultado.Mensaje = "Se generó un error inesperado al intentar registrar MSISDN [" + resultado.Valor + "]"
                End If
            Catch ex As Exception
                .abortarTransaccion()
                Throw ex
            End Try
        End With

        Return resultado
    End Function

    Public Sub Modificar() Implements IDetalleMsisdnEnServicioMensajeria.Modificar
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                .SqlParametros.Add("@idRegistro", SqlDbType.Int).Value = _idRegistro
                If _msisdn > 0 Then .SqlParametros.Add("@msisdn", SqlDbType.BigInt).Value = _msisdn
                If Not String.IsNullOrEmpty(_numeroReserva) Then .SqlParametros.Add("@numeroReserva", SqlDbType.VarChar).Value = _numeroReserva
                .SqlParametros.Add("@activaEquipoAnterior", SqlDbType.Bit).Value = _activaEquipoAnterior
                .SqlParametros.Add("@comSeguro", SqlDbType.Bit).Value = _comseguro
                If _precioConIva > 0 Then .SqlParametros.Add("@precioConIVA", SqlDbType.Money).Value = _precioConIva
                If _precioSinIva > 0 Then .SqlParametros.Add("@precioSinIVA", SqlDbType.Money).Value = _precioSinIva
                If _idClausula > 0 Then .SqlParametros.Add("@idClausula", SqlDbType.Int).Value = _idClausula
                If _fechaDevolucion <> Date.MinValue Then .SqlParametros.Add("@fechaDevolucion", SqlDbType.Date).Value = _fechaDevolucion
                .SqlParametros.Add("@lista28", SqlDbType.Bit).Value = _lista28

                .iniciarTransaccion()
                .ejecutarNonQuery("ModificarMsisdnServicioMensajeria", CommandType.StoredProcedure)
                .confirmarTransaccion()
            Catch ex As Exception
                .abortarTransaccion()
                Throw ex
            End Try
        End With
        dbManager.Dispose()
    End Sub

    Public Sub Eliminar() Implements IDetalleMsisdnEnServicioMensajeria.Eliminar
        'Using dbManager As New LMDataAccess
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                .SqlParametros.Add("@idRegistro", SqlDbType.Int).Value = _idRegistro

                .iniciarTransaccion()
                .ejecutarNonQuery("EliminarMsisdnServicioMensajeria", CommandType.StoredProcedure)
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

    Protected Friend Overridable Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader) Implements IDetalleMsisdnEnServicioMensajeria.CargarResultadoConsulta
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idRegistro").ToString, _idRegistro)
                Integer.TryParse(reader("idTipoServicio").ToString, _idTipoServicio)
                _msisdn = reader("msisdn").ToString
                _numeroReserva = reader("numeroReserva").ToString
                _activaEquipoAnteriorTexto = reader("activaEquipoAnteriorTexto").ToString
                _comseguroTexto = reader("comseguroTexto").ToString
                _lista28Texto = reader("lista28Texto").ToString
                If Not IsDBNull(reader("activaEquipoAnterior")) Then Boolean.TryParse(reader("activaEquipoAnterior"), _activaEquipoAnterior)
                If Not IsDBNull(reader("comseguro")) Then Boolean.TryParse(reader("comseguro"), _comseguro)
                If Not IsDBNull(reader("lista28")) Then Boolean.TryParse(reader("lista28"), _lista28)
                If Not IsDBNull(reader("precioConIva")) Then Double.TryParse(reader("precioConIva"), _precioConIva)
                If Not IsDBNull(reader("precioSinIva")) Then Double.TryParse(reader("precioSinIva"), _precioSinIva)
                If Not IsDBNull(reader("idClausula")) Then Integer.TryParse(reader("idClausula"), _idClausula)
                If Not IsDBNull(reader("clausula")) Then _clausula = reader("clausula").ToString
                _tieneCambioServicio = reader("tieneCambioServicio").ToString
                _registrado = True
                _bloquear = CBool(reader("bloquear").ToString())
                Integer.TryParse(reader("idServicioMensajeria"), _idServicioMensajeria)
                _numeroReserva = reader("numeroReserva").ToString()
                If Not IsDBNull(reader("idRegion")) Then Short.TryParse(reader("idRegion"), _idRegion)
                If Not IsDBNull(reader("nombreRegion")) Then _nombreRegion = reader("nombreRegion")
                If Not IsDBNull(reader("fechaDevolucion")) Then Date.TryParse(reader("fechaDevolucion"), _fechaDevolucion)
                If Not IsDBNull(reader("idPlan")) Then Integer.TryParse(reader("idPlan"), _idPlan)
                If Not IsDBNull(reader("nombrePlan")) Then _nombrePlan = reader("nombrePlan")
                Integer.TryParse(reader("cantidadMateriales"), _cantidadMaterial)
                Integer.TryParse(reader("cantidadMaterialesLeidos"), _cantidadMaterialLeida)
            End If
        End If

    End Sub

#End Region

End Class