Imports ILSBusinessLayer
Imports LMDataAccessLayer

Public Class DetalleSerialServicioMensajeria

#Region "Atributos (Campos)"

    Protected Friend _idDetalle As Integer
    Protected Friend _idMaterialServicio As Integer
    Protected Friend _idServicio As Integer
    Protected Friend _material As String
    Protected Friend _descripcionMaterial As String
    Protected Friend _serial As String
    Protected Friend _idUsuarioRegistra As Integer
    Protected Friend _fechaRegistro As Date
    Protected Friend _msisdn As String
    Protected Friend _iccid As String
    Protected Friend _factura As String
    Protected Friend _remision As String
    Protected Friend _idUsuarioLegaliza As Integer
    Protected Friend _usuarioLegaliza As String
    Protected Friend _fechaLegalizacion As Date
    Protected Friend _idNovedadLegalizacion As Integer
    Protected Friend _planillaLegalizacion As String
    Protected Friend _idTipoProducto As Integer
    Protected Friend _devuelto As Boolean
    Protected Friend _idTipoSIM As Short
    Protected Friend _legalizaCliente As Boolean
    Protected Friend _reporteTipoSIM As Boolean
    Protected Friend _requierePrestamoEquipo As Boolean
    Protected Friend _serialPrestamo As String
    Protected Friend _idEstadoSerial As Short
    Protected Friend _estadoSerial As String
    Protected Friend _ordenServicio As String
    Protected Friend _generaCosto As Enumerados.EstadoBinario
    Protected Friend _clienteAceptaCosto As Enumerados.EstadoBinario
    Protected Friend _fechaEntregaServicioTecnico As Date
    Protected Friend _fechaEntregaServicioTecnicoString As String
    Protected Friend _numeroRadicado As Long
    Protected Friend _idEstadoDevolucion As Integer
    Protected Friend _estadoDevolucion As String
    Protected Friend _referenciaReparado As String
    Protected Friend _referenciaPrestamo As String

    Protected Friend _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _generaCosto = Enumerados.EstadoBinario.NoEstablecido
        _clienteAceptaCosto = Enumerados.EstadoBinario.NoEstablecido
    End Sub

    Public Sub New(ByVal idDetalle As Integer)
        MyBase.New()
        If idDetalle > 0 Then
            _idDetalle = idDetalle
            CargarDatos()
        End If
    End Sub

    Public Sub New(ByVal serial As String)
        MyBase.New()
        If Not String.IsNullOrEmpty(serial) Then
            _serial = serial
            CargarDatos()
        End If
    End Sub

    Public Sub New(ByVal idServicio As Integer, ByVal serial As String)
        MyBase.New()
        If Not String.IsNullOrEmpty(serial) Then
            _idServicio = idServicio
            _serial = serial
            CargarDatos()
        End If
    End Sub
#End Region

#Region "Propiedades"

    Public Property IdDetalle() As Integer
        Get
            Return _idDetalle
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idDetalle = value
        End Set
    End Property

    Public Property IdMaterialServicio() As Integer
        Get
            Return _idMaterialServicio
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idMaterialServicio = value
        End Set
    End Property

    Public Property IdServicio() As Integer
        Get
            Return _idServicio
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idServicio = value
        End Set
    End Property

    Public Property Material() As String
        Get
            Return _material
        End Get
        Set(ByVal value As String)
            _material = value
        End Set
    End Property

    Public Property DescripcionMaterial() As String
        Get
            Return _descripcionMaterial
        End Get
        Set(ByVal value As String)
            _descripcionMaterial = value
        End Set
    End Property

    Public Property Serial() As String
        Get
            Return _serial
        End Get
        Set(ByVal value As String)
            _serial = value
        End Set
    End Property

    Public Property IdUsuarioRegistra() As Integer
        Get
            Return _idUsuarioRegistra
        End Get
        Set(ByVal value As Integer)
            _idUsuarioRegistra = value
        End Set
    End Property

    Public Property FechaRegistro() As Date
        Get
            Return _fechaRegistro
        End Get
        Protected Friend Set(ByVal value As Date)
            _fechaRegistro = value
        End Set
    End Property

    Public Property Msisdn() As String
        Get
            Return _msisdn
        End Get
        Set(ByVal value As String)
            _msisdn = value
        End Set
    End Property

    Public Property Iccid() As String
        Get
            Return _iccid
        End Get
        Set(ByVal value As String)
            _iccid = value
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

    Public Property Remision() As String
        Get
            Return _remision
        End Get
        Set(ByVal value As String)
            _remision = value
        End Set
    End Property

    Public Property IdUsuarioLegaliza() As Integer
        Get
            Return _idUsuarioLegaliza
        End Get
        Set(ByVal value As Integer)
            _idUsuarioLegaliza = value
        End Set
    End Property

    Public Property UsuarioLegaliza() As String
        Get
            Return _usuarioLegaliza
        End Get
        Set(ByVal value As String)
            _usuarioLegaliza = value
        End Set
    End Property

    Public Property FechaLegalizacion() As Date
        Get
            Return _fechaLegalizacion
        End Get
        Set(ByVal value As Date)
            _fechaLegalizacion = value
        End Set
    End Property

    Public Property IdNovedadLegalizacion() As Integer
        Get
            Return _idNovedadLegalizacion
        End Get
        Set(ByVal value As Integer)
            _idNovedadLegalizacion = value
        End Set
    End Property

    Public Property PlanillaLegalizacion() As String
        Get
            Return _planillaLegalizacion
        End Get
        Set(ByVal value As String)
            _planillaLegalizacion = value
        End Set
    End Property

    Public Property IdTipoProducto() As Integer
        Get
            Return _idTipoProducto
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idTipoProducto = value
        End Set
    End Property

    Public Property Devuelto() As Boolean
        Get
            Return _devuelto
        End Get
        Set(ByVal value As Boolean)
            _devuelto = value
        End Set
    End Property

    Public Property IdTipoSIM() As Short
        Get
            Return _idTipoSIM
        End Get
        Set(ByVal value As Short)
            _idTipoSIM = value
        End Set
    End Property

    Public Property LegalizaCliente() As Boolean
        Get
            Return _legalizaCliente
        End Get
        Set(ByVal value As Boolean)
            _legalizaCliente = value
        End Set
    End Property

    Public Property ReporteTipoSIM() As Boolean
        Get
            Return _reporteTipoSIM
        End Get
        Set(ByVal value As Boolean)
            _reporteTipoSIM = value
        End Set
    End Property

    Public Property RequierePrestamoEquipo() As Boolean
        Get
            Return _requierePrestamoEquipo
        End Get
        Set(ByVal value As Boolean)
            _requierePrestamoEquipo = value
        End Set
    End Property

    Public Property SerialPrestamo() As String
        Get
            Return _serialPrestamo
        End Get
        Set(ByVal value As String)
            _serialPrestamo = value
        End Set
    End Property

    Public Property IdEstadoSerial() As Short
        Get
            Return _idEstadoSerial
        End Get
        Set(ByVal value As Short)
            _idEstadoSerial = value
        End Set
    End Property

    Public Property EstadoSerial As String
        Get
            Return _estadoSerial
        End Get
        Set(value As String)
            _estadoSerial = value
        End Set
    End Property

    Public Property OrdenServicio() As String
        Get
            Return _ordenServicio
        End Get
        Set(ByVal value As String)
            _ordenServicio = value
        End Set
    End Property

    Public Property GeneraCosto() As Enumerados.EstadoBinario
        Get
            Return _generaCosto
        End Get
        Set(ByVal value As Enumerados.EstadoBinario)
            _generaCosto = value
        End Set
    End Property

    Public Property ClienteAceptaCosto() As Enumerados.EstadoBinario
        Get
            Return _clienteAceptaCosto
        End Get
        Set(ByVal value As Enumerados.EstadoBinario)
            _clienteAceptaCosto = value
        End Set
    End Property

    Public Property FechaEntregaServicioTecnico() As Date
        Get
            Return _fechaEntregaServicioTecnico
        End Get
        Set(ByVal value As Date)
            _fechaEntregaServicioTecnico = value
        End Set
    End Property

    Public Property FechaEntregaServicioTecnicoString As String
        Get
            Return _fechaEntregaServicioTecnicoString
        End Get
        Set(value As String)
            _fechaEntregaServicioTecnicoString = value
        End Set
    End Property

    Public Property NumeroRadicado() As Long
        Get
            Return _numeroRadicado
        End Get
        Set(value As Long)
            _numeroRadicado = value
        End Set
    End Property

    Public Property IdEstadoDevolucion As Integer
        Get
            Return _idEstadoDevolucion
        End Get
        Set(value As Integer)
            _idEstadoDevolucion = value
        End Set
    End Property

    Public Property EstadoDevolucion As String
        Get
            Return _estadoDevolucion
        End Get
        Set(value As String)
            _estadoDevolucion = value
        End Set
    End Property

    Public Property ReferenciaReparado As String
        Get
            Return _referenciaReparado
        End Get
        Set(value As String)
            _referenciaReparado = value
        End Set
    End Property

    Public Property ReferenciaPrestamo As String
        Get
            Return _referenciaPrestamo
        End Get
        Set(value As String)
            _referenciaPrestamo = value
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

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        If Not String.IsNullOrEmpty(_serial) OrElse _idDetalle > 0 OrElse _idServicio > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If Not String.IsNullOrEmpty(_serial) Then .SqlParametros.Add("@serial", SqlDbType.VarChar, 50).Value = _serial
                    If _idDetalle > 0 Then .SqlParametros.Add("@idDetalle", SqlDbType.Int).Value = _idDetalle
                    If _idServicio > 0 Then .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicio

                    .ejecutarReader("ObtenerDetalleSerialServicioMensajeria", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then CargarResultadoConsulta(.Reader)
                        If Not dbManager.Reader.IsClosed Then dbManager.Reader.Close()
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End If

    End Sub

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idDetalle").ToString, _idDetalle)
                Integer.TryParse(reader("idMaterialServicio").ToString, _idMaterialServicio)
                Integer.TryParse(reader("idServicio").ToString, _idServicio)
                _material = reader("material").ToString
                _descripcionMaterial = reader("descripcionMaterial").ToString
                _serial = reader("serial").ToString
                Integer.TryParse(reader("idUsuarioRegistra").ToString, _idUsuarioRegistra)
                _fechaRegistro = CDate(reader("fechaRegistro"))
                _msisdn = reader("msisdn").ToString
                _iccid = reader("iccid").ToString
                _factura = reader("facturaCambioServicio").ToString
                _remision = reader("remisionCambioServicio").ToString
                Integer.TryParse(reader("idUsuarioLegaliza").ToString, _idUsuarioLegaliza)
                _usuarioLegaliza = reader("usuarioLegaliza").ToString
                If Not IsDBNull(reader("fechaLegalizacion")) Then _fechaLegalizacion = CDate(reader("fechaLegalizacion"))
                Integer.TryParse(reader("idNovedadLegalizacion").ToString, _idNovedadLegalizacion)
                _planillaLegalizacion = reader("planillaLegalizacion").ToString
                Integer.TryParse(reader("idTipoProducto").ToString, _idTipoProducto)
                _devuelto = CBool(reader("devuelto"))
                If Not IsDBNull(reader("idTipoSIM")) Then Integer.TryParse(reader("idTipoSIM").ToString(), _idTipoSIM)
                If Not IsDBNull(reader("legalizaCliente")) Then Boolean.TryParse(reader("legalizaCliente"), _legalizaCliente)
                If Not IsDBNull(reader("reporteTipoSIM")) Then Boolean.TryParse(reader("reporteTipoSIM"), _reporteTipoSIM)
                If Not IsDBNull(reader("requierePrestamoEquipo")) Then Boolean.TryParse(reader("requierePrestamoEquipo"), _requierePrestamoEquipo)
                _serialPrestamo = reader("serialPrestamo").ToString
                If Not IsDBNull(reader("idEstadoSerial")) Then Integer.TryParse(reader("idEstadoSerial"), _idEstadoSerial)
                If Not IsDBNull(reader("estadoSerial")) Then _estadoSerial = CStr(reader("estadoSerial"))
                If Not IsDBNull(reader("ordenServicio")) Then _ordenServicio = reader("ordenServicio").ToString
                If Not IsDBNull(reader("generaCosto")) Then Boolean.TryParse(reader("generaCosto"), _generaCosto)
                If Not IsDBNull(reader("clienteAceptaCosto")) Then Boolean.TryParse(reader("clienteAceptaCosto"), _clienteAceptaCosto)
                If Not IsDBNull(reader("fechaEntregaServicioTecnico")) Then _fechaEntregaServicioTecnico = CDate(reader("fechaEntregaServicioTecnico"))
                If Not IsDBNull(reader("fechaEntregaServicioTecnico")) Then _fechaEntregaServicioTecnicoString = CStr(reader("fechaEntregaServicioTecnico"))
                If Not IsDBNull(reader("numeroRadicado")) Then _numeroRadicado = CLng(reader("numeroRadicado"))
                If Not IsDBNull(reader("idEstadoDevolucion")) Then _idEstadoDevolucion = CInt(reader("idEstadoDevolucion"))
                If Not IsDBNull(reader("estadoDevolucion")) Then _estadoDevolucion = CStr(reader("estadoDevolucion"))
                If Not IsDBNull(reader("modeloSerial")) Then _referenciaReparado = CStr(reader("modeloSerial"))
                If Not IsDBNull(reader("modeloPrestamo")) Then _referenciaPrestamo = CStr(reader("modeloPrestamo"))

                _registrado = True
            End If
        End If

    End Sub

#End Region

#Region "Métodos Publicos"

    Public Function Eliminar(ByVal idUsuario As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If (_idDetalle > 0 OrElse Not String.IsNullOrEmpty(_serial)) AndAlso _idServicio > 0 AndAlso idUsuario > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idServicio", SqlDbType.BigInt).Value = _idServicio
                    .SqlParametros.Add("@idUsuario", SqlDbType.BigInt).Value = idUsuario
                    If Not String.IsNullOrEmpty(_serial) Then
                        .SqlParametros.Add("@serial", SqlDbType.VarChar, 50).Value = _serial
                    ElseIf _idDetalle > 0 Then
                        .SqlParametros.Add("@idDetalle", SqlDbType.Int).Value = _idDetalle
                    End If
                    .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                    .ejecutarNonQuery("EliminarSerialDeServicioMensajeria", CommandType.StoredProcedure)
                    If Integer.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        If resultado.Valor <> 0 Then
                            Select Case resultado.Valor
                                Case 1
                                    resultado.Mensaje = "No se ha proporcionado información correspondiente al Serial a desvincular"
                                Case 2
                                    resultado.Mensaje = "Serial no está vinculado a ningún servicio"
                                Case 3
                                    resultado.Mensaje = "Serial está vinculado a un servicio diferente al proporcionado"
                                Case Else
                                    resultado.Mensaje = "Ocurrió un error inesperado al tratar de desvincular el serial"
                            End Select

                        End If
                    Else
                        Throw New Exception("Imposible evaluar la respuesta del servidor.")
                    End If
                End With
            Catch ex As Exception

                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        Else
            resultado.EstablecerMensajeYValor(10, "No se han proporcionado los datos mínimos para reconocer el registro a eliminar.")
        End If

        Return resultado
    End Function

    Public Function Actualizar(ByVal idUsuario As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If (_idDetalle > 0 OrElse Not String.IsNullOrEmpty(_serial)) AndAlso _idServicio > 0 AndAlso idUsuario > 0 Then
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idDetalle", SqlDbType.BigInt).Value = _idDetalle
                        .SqlParametros.Add("@idServicio", SqlDbType.BigInt).Value = _idServicio
                        If Not String.IsNullOrEmpty(_serial) Then .SqlParametros.Add("@serial", SqlDbType.VarChar, 50).Value = _serial
                        If _idMaterialServicio > 0 Then .SqlParametros.Add("@idMaterialServicio", SqlDbType.Int).Value = _idMaterialServicio
                        If Not String.IsNullOrEmpty(_material) Then .SqlParametros.Add("@material", SqlDbType.VarChar, 50).Value = _material
                        If _idUsuarioRegistra > 0 Then .SqlParametros.Add("@idUsuarioRegistra", SqlDbType.Int).Value = _idUsuarioRegistra
                        If _fechaRegistro > Date.MinValue Then .SqlParametros.Add("@fechaRegistro", SqlDbType.DateTime).Value = _fechaRegistro
                        If Not String.IsNullOrEmpty(_msisdn) Then .SqlParametros.Add("@msisdn", SqlDbType.VarChar).Value = _msisdn
                        If Not String.IsNullOrEmpty(_iccid) Then .SqlParametros.Add("@iccid", SqlDbType.VarChar).Value = _iccid
                        If Not String.IsNullOrEmpty(_factura) Then .SqlParametros.Add("@facturaCambioServicio", SqlDbType.VarChar).Value = _factura
                        If Not String.IsNullOrEmpty(_remision) Then .SqlParametros.Add("@remisionCambioServicio", SqlDbType.VarChar).Value = _remision
                        If _idUsuarioLegaliza > 0 Then .SqlParametros.Add("@idUsuarioLegaliza", SqlDbType.Int).Value = _idUsuarioLegaliza
                        If _fechaLegalizacion > Date.MinValue Then .SqlParametros.Add("@fechaLegalizacion", SqlDbType.DateTime).Value = _fechaLegalizacion
                        If _idNovedadLegalizacion > 0 Then .SqlParametros.Add("@idNovedadLegalizacion", SqlDbType.Int).Value = _idNovedadLegalizacion
                        If Not String.IsNullOrEmpty(_planillaLegalizacion) Then .SqlParametros.Add("@planillaLegalizacion", SqlDbType.VarChar).Value = _planillaLegalizacion
                        .SqlParametros.Add("@devuelto", SqlDbType.Bit).Value = _devuelto
                        If _idTipoSIM > 0 Then .SqlParametros.Add("@idTipoSIM", SqlDbType.Int).Value = _idTipoSIM
                        .SqlParametros.Add("@legalizaCliente", SqlDbType.Bit).Value = _legalizaCliente
                        .SqlParametros.Add("@reporteTipoSIM", SqlDbType.Bit).Value = _reporteTipoSIM
                        .SqlParametros.Add("@requierePrestamoEquipo", SqlDbType.Bit).Value = _requierePrestamoEquipo
                        If Not String.IsNullOrEmpty(_serialPrestamo) Then .SqlParametros.Add("@serialPrestamo", SqlDbType.VarChar).Value = _serialPrestamo
                        If _idEstadoSerial > 0 Then .SqlParametros.Add("@idEstadoSerial", SqlDbType.SmallInt).Value = _idEstadoSerial
                        If Not String.IsNullOrEmpty(_ordenServicio) Then .SqlParametros.Add("@ordenServicio", SqlDbType.VarChar).Value = _ordenServicio
                        If _generaCosto <> Enumerados.EstadoBinario.NoEstablecido Then .SqlParametros.Add("@generaCosto", SqlDbType.Bit).Value = CBool(IIf(_generaCosto = Enumerados.EstadoBinario.Activo, True, False))
                        If _clienteAceptaCosto <> Enumerados.EstadoBinario.NoEstablecido Then .SqlParametros.Add("@clienteAceptaCosto", SqlDbType.Bit).Value = CBool(IIf(_clienteAceptaCosto = Enumerados.EstadoBinario.Activo, True, False))
                        If _fechaEntregaServicioTecnico > Date.MinValue Then .SqlParametros.Add("@fechaEntregaServicioTecnico", SqlDbType.DateTime).Value = _fechaEntregaServicioTecnico
                        If _idEstadoDevolucion > 0 Then .SqlParametros.Add("@idEstadoDevolucion", SqlDbType.VarChar).Value = _idEstadoDevolucion

                        .SqlParametros.Add("@idUsuario", SqlDbType.BigInt).Value = idUsuario
                        .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue

                        .iniciarTransaccion()
                        .ejecutarNonQuery("ActualizarSerialDeServicioMensajeria", CommandType.StoredProcedure)
                        If Integer.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                            If resultado.Valor = 0 Then
                                .confirmarTransaccion()
                            Else
                                Select Case resultado.Valor
                                    Case 1
                                        resultado.Mensaje = "No se ha proporcionado información correspondiente al Serial a desvincular."
                                    Case Else
                                        resultado.Mensaje = "Ocurrió un error inesperado al tratar de actualizar el serial."
                                End Select
                                .abortarTransaccion()
                            End If
                        Else
                            Throw New Exception("Imposible evaluar la respuesta del servidor.")
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw ex
                End Try
            End Using
        Else
            resultado.EstablecerMensajeYValor(10, "No se han proporcionado los datos mínimos para reconocer el registro a eliminar.")
        End If
        Return resultado
    End Function

    Public Function MarcarDevolucion(ByVal idUsuario As Integer, ByVal numRadicado As Long) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If (_idDetalle > 0 OrElse Not String.IsNullOrEmpty(_serial)) AndAlso _idServicio > 0 AndAlso idUsuario > 0 Then
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@numRadicado", SqlDbType.BigInt).Value = numRadicado
                        .SqlParametros.Add("@listaSeriales", SqlDbType.VarChar).Value = _serial
                        .SqlParametros.Add("@idUsuario", SqlDbType.BigInt).Value = idUsuario

                        .iniciarTransaccion()
                        .ejecutarReader("MarcarSerialesDevolucion", CommandType.StoredProcedure)

                        If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                            If .Reader.Read() Then
                                resultado.Valor = CInt(.Reader.Item("valor"))
                                resultado.Mensaje = CStr(.Reader.Item("mensaje"))

                                .Reader.Close()
                                If resultado.Valor = 0 Then
                                    .confirmarTransaccion()
                                Else
                                    .abortarTransaccion()
                                End If
                            End If
                        Else
                            Throw New Exception("Imposible evaluar la respuesta del servidor.")
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                End Try
            End Using
        Else
            resultado.EstablecerMensajeYValor(10, "No se han proporcionado los datos mínimos para reconocer el registro a eliminar.")
        End If
        Return resultado
    End Function

    Public Function CargarDatosArchivo(ByVal idServicio As Integer) As DataTable
        Dim _dbManager As New LMDataAccess
        Dim dtDatos As New DataTable

        Try
            With _dbManager
                .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = idServicio
                dtDatos = .ejecutarDataTable("obtenerFacturaRemisionServicioMensajeria", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

#End Region

End Class
