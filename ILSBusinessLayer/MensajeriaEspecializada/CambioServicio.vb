Imports LMDataAccessLayer

Public Class CambioServicio

#Region "Atributos (Campos)"

    Private _idServicioMensajeria As Integer
    Private _imei As String
    Private _iccid As String
    Private _msisdn As String
    Private _factura As String
    Private _remision As String
    Private _facturaImei As String
    Private _remisionImei As String
    Private _facturaIccid As String
    Private _remisionIccid As String
    Private _hayNovedad As Boolean
    Private _idMin As Integer
    Private _idTipoSIM As Short
    Private _numeroCambioServicio As String

#End Region

#Region "Propiedades"

    Public Property IdServicioMensajeria() As Integer
        Get
            Return _idServicioMensajeria
        End Get
        Set(ByVal value As Integer)
            _idServicioMensajeria = value
        End Set
    End Property

    Public Property Imei() As String
        Get
            Return _imei
        End Get
        Set(ByVal value As String)
            _imei = value
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

    Public Property Msisdn() As String
        Get
            Return _msisdn
        End Get
        Set(ByVal value As String)
            _msisdn = value
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

    Public Property FacturaImei() As String
        Get
            Return _facturaImei
        End Get
        Set(ByVal value As String)
            _facturaImei = value
        End Set
    End Property

    Public Property RemisionImei() As String
        Get
            Return _remisionImei
        End Get
        Set(ByVal value As String)
            _remisionImei = value
        End Set
    End Property

    Public Property FacturaIccid() As String
        Get
            Return _facturaIccid
        End Get
        Set(ByVal value As String)
            _facturaIccid = value
        End Set
    End Property

    Public Property RemisionIccid() As String
        Get
            Return _remisionIccid
        End Get
        Set(ByVal value As String)
            _remisionIccid = value
        End Set
    End Property

    Public Property HayNovedad() As Boolean
        Get
            Return _hayNovedad
        End Get
        Set(ByVal value As Boolean)
            _hayNovedad = value
        End Set
    End Property

    Public Property IdMin() As Integer
        Get
            Return _idMin
        End Get
        Set(ByVal value As Integer)
            _idMin = value
        End Set
    End Property

    Public Property IdTipoSIM() As Integer
        Get
            Return _idTipoSIM
        End Get
        Set(ByVal value As Integer)
            _idTipoSIM = value
        End Set
    End Property

    Public Property NumeroCambioServicio() As String
        Get
            Return _numeroCambioServicio
        End Get
        Set(value As String)
            _numeroCambioServicio = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _imei = ""
        _iccid = ""
        _msisdn = ""
        _factura = ""
        _remision = ""
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function RegistrarCambioDeServicio() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If _idServicioMensajeria > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                    If Not String.IsNullOrEmpty(Imei) Then .SqlParametros.Add("@imei", SqlDbType.VarChar, 50).Value = _imei
                    If Not String.IsNullOrEmpty(Iccid) Then .SqlParametros.Add("@iccid", SqlDbType.VarChar, 50).Value = _iccid
                    If Not String.IsNullOrEmpty(Msisdn) Then .SqlParametros.Add("@msisdn", SqlDbType.VarChar, 50).Value = _msisdn
                    If Not String.IsNullOrEmpty(Factura) Then .SqlParametros.Add("@factura", SqlDbType.VarChar, 50).Value = _factura
                    If Not String.IsNullOrEmpty(Remision) Then .SqlParametros.Add("@remision", SqlDbType.VarChar, 50).Value = _remision
                    .SqlParametros.Add("@hayNovedad", SqlDbType.Bit).Value = _hayNovedad
                    If _idMin > 0 Then .SqlParametros.Add("@idMin ", SqlDbType.Int).Value = _idMin
                    If _idTipoSIM > 0 Then .SqlParametros.Add("@idTipoSIM", SqlDbType.SmallInt).Value = _idTipoSIM
                    If Not String.IsNullOrEmpty(NumeroCambioServicio) Then .SqlParametros.Add("@numCambioServicio", SqlDbType.VarChar, 50).Value = _numeroCambioServicio
                    .SqlParametros.Add("@result", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue

                    .iniciarTransaccion()
                    .ejecutarNonQuery("RegistrarCambioDeServicio", CommandType.StoredProcedure)

                    If Not IsDBNull(.SqlParametros("@result").Value) Then
                        resultado.Valor = CShort(.SqlParametros("@result").Value)
                        If resultado.Valor = 0 Then
                            If String.IsNullOrEmpty(imei) Or String.IsNullOrEmpty(iccid) Then
                                resultado.Mensaje = "El cambio de servicio fue registrado satisfactoriamente."
                            Else
                                resultado.Mensaje = "Los cambios de servicio fueron registrados satisfactoriamente."
                            End If
                            .confirmarTransaccion()
                        Else
                            Select Case resultado.Valor
                                Case 1
                                    resultado.Mensaje = "El Imei proporcionado no figura como despachado en el servicio actual"
                                Case 2
                                    resultado.Mensaje = "El Iccid proporcionado no figura como despachado en el servicio actual"
                                Case 3
                                    resultado.Mensaje = "El Msisdn ya se encuentra asociado a un Iccid del mismo radicado"
                                Case 4
                                    resultado.Mensaje = "El Msisdn ya se encuentra asociado a un Imei del mismo radicado"
                                Case Else
                                    resultado.Mensaje = "Ocurrió un error inesperado al registrar cambio de servicio. Por favor intente nuevamente."

                            End Select
                            .abortarTransaccion()
                        End If
                    Else
                        Throw New Exception("Ocurrió un error interno al registrar serial. Por favor intente nuevamente")
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        Else
            resultado.EstablecerMensajeYValor(10, "No se han propocionado todos los datos requeridos para realizar la confirmación. ")
        End If

        Return resultado
    End Function

    Public Function RegistrarCambioDeServicioArchivo() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If _idServicioMensajeria > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                    If Not String.IsNullOrEmpty(Msisdn) Then .SqlParametros.Add("@msisdn", SqlDbType.VarChar, 50).Value = _msisdn
                    If Not String.IsNullOrEmpty(Imei) Then .SqlParametros.Add("@imei", SqlDbType.VarChar, 50).Value = _imei
                    If Not String.IsNullOrEmpty(FacturaImei) Then .SqlParametros.Add("@facturaImei", SqlDbType.VarChar, 50).Value = _facturaImei
                    If Not String.IsNullOrEmpty(RemisionImei) Then .SqlParametros.Add("@remisionImei", SqlDbType.VarChar, 50).Value = _remisionImei
                    If Not String.IsNullOrEmpty(Iccid) Then .SqlParametros.Add("@iccid", SqlDbType.VarChar, 50).Value = _iccid
                    If Not String.IsNullOrEmpty(FacturaIccid) Then .SqlParametros.Add("@facturaIccid", SqlDbType.VarChar, 50).Value = _facturaIccid
                    If Not String.IsNullOrEmpty(RemisionIccid) Then .SqlParametros.Add("@remisionIccid", SqlDbType.VarChar, 50).Value = _remisionIccid
                    .SqlParametros.Add("@hayNovedad", SqlDbType.Bit).Value = _hayNovedad
                    If _idMin > 0 Then .SqlParametros.Add("@idMin ", SqlDbType.Int).Value = _idMin
                    If _idTipoSIM > 0 Then .SqlParametros.Add("@idTipoSIM", SqlDbType.SmallInt).Value = _idTipoSIM
                    .SqlParametros.Add("@result", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue

                    .iniciarTransaccion()
                    .ejecutarNonQuery("RegistrarCambioDeServicioArchivo", CommandType.StoredProcedure)

                    If Not IsDBNull(.SqlParametros("@result").Value) Then
                        resultado.Valor = CShort(.SqlParametros("@result").Value)
                        If resultado.Valor = 0 Then
                            .confirmarTransaccion()
                        Else
                            .abortarTransaccion()
                        End If
                    Else
                        Throw New Exception("Ocurrió un error interno al registrar serial. Por favor intente nuevamente")
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        Else
            resultado.EstablecerMensajeYValor(10, "No se han propocionado todos los datos requeridos para realizar la confirmación. ")
        End If

        Return resultado
    End Function

    Public Function validarSerialServicio() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If _idServicioMensajeria > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                    If Not String.IsNullOrEmpty(Imei) Then .SqlParametros.Add("@imei", SqlDbType.VarChar, 50).Value = _imei
                    If Not String.IsNullOrEmpty(Iccid) Then .SqlParametros.Add("@iccid", SqlDbType.VarChar, 50).Value = _iccid
                    .SqlParametros.Add("@result", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue

                    .ejecutarNonQuery("ValidacionSerialVsServicio", CommandType.StoredProcedure)
                    If Not IsDBNull(.SqlParametros("@result").Value) Then
                        resultado.Valor = CShort(.SqlParametros("@result").Value)
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End If
        Return resultado
    End Function

#End Region

End Class
