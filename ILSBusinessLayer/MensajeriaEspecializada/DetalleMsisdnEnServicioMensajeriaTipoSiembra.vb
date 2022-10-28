Imports ILSBusinessLayer
Imports LMDataAccessLayer

Public Class DetalleMsisdnEnServicioMensajeriaTipoSiembra
    Inherits DetalleMsisdnEnServicioMensajeria

#Region "Atributos"

    Private _idPaquete As Integer
    Private _nombrePaquete As String

#End Region

#Region "Propiedades"

    Public Overrides Property IdPlan As Integer
        Get
            Return _idPlan
        End Get
        Set(value As Integer)
            _idPlan = value
        End Set
    End Property

    Public Overrides Property NombrePlan As String
        Get
            Return _nombrePlan
        End Get
        Protected Friend Set(value As String)
            _nombrePlan = value
        End Set
    End Property

    Public Overrides Property FechaDevolucion As Date
        Get
            Return _fechaDevolucion
        End Get
        Set(value As Date)
            _fechaDevolucion = value
        End Set
    End Property

    Public Overrides Property CantidadMaterial() As Integer
        Get
            Return _cantidadMaterial
        End Get
        Set(ByVal value As Integer)
            _cantidadMaterial = value
        End Set
    End Property

    Public Overrides Property CantidadMaterialLeida() As Integer
        Get
            Return _cantidadMaterialLeida
        End Get
        Protected Friend Set(ByVal value As Integer)
            _cantidadMaterialLeida = value
        End Set
    End Property

    Public Property IdPaquete As Integer
        Get
            Return _idPaquete
        End Get
        Set(value As Integer)
            _idPaquete = value
        End Set
    End Property

    Public Property NombrePaquete As String
        Get
            Return _nombrePaquete
        End Get
        Protected Friend Set(value As String)
            _nombrePaquete = value
        End Set
    End Property

#End Region

#Region "Métodos Públicos"

    Public Overrides Function Adicionar(Optional objDataAccess As LMDataAccess = Nothing) As ResultadoProceso
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
                If _idPlan > 0 Then .SqlParametros.Add("@idPlan", SqlDbType.Int).Value = _idPlan
                If _idRegion > 0 Then .SqlParametros.Add("@idRegion", SqlDbType.Int).Value = _idRegion
                If _fechaDevolucion <> Date.MinValue Then .SqlParametros.Add("@fechaDevolucion", SqlDbType.DateTime).Value = _fechaDevolucion
                If _idPaquete > 0 Then .SqlParametros.Add("@idPaquete", SqlDbType.Int).Value = _idPaquete
                .SqlParametros.Add("@idMsisdn", SqlDbType.Int).Direction = ParameterDirection.Output
                .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                If Not objDataAccess.estadoTransaccional Then .iniciarTransaccion()
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

#End Region

#Region "Métodos Protegidos"

    Protected Friend Overrides Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idRegistro").ToString, _idRegistro)
                Integer.TryParse(reader("idTipoServicio").ToString, _idTipoServicio)
                _msisdn = reader("msisdn").ToString
                Integer.TryParse(reader("idServicioMensajeria"), _idServicioMensajeria)
                If Not IsDBNull(reader("idPlan")) Then Integer.TryParse(reader("idPlan"), _idPlan)
                If Not IsDBNull(reader("nombrePlan")) Then _nombrePlan = reader("nombrePlan")
                _fechaDevolucion = CDate(reader("fechaDevolucion"))
                _cantidadMaterial = reader("cantidadMateriales")
                _cantidadMaterialLeida = reader("cantidadMaterialesLeidos")
                _registrado = True
            End If
        End If

    End Sub

#End Region

End Class
