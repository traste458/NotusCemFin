Imports LMDataAccessLayer
Imports System.String
Imports ILSBusinessLayer.MensajeriaEspecializada
Imports System.Reflection

Public Class CampaniaFinanciero
    Inherits Campania

#Region "Atributos"

    Private _listCiudades As List(Of Integer)
    Private _listBodegas As List(Of Integer)
    Private _listProductoExterno As List(Of Integer)
    Private _listDocumentoFinanciero As List(Of Integer)
    Private _listTiposDeServicio As List(Of Integer)
    Private _idClienteExterno As Integer
    Private _idEmpresa As Integer
    Private _idTipoCampania As Integer
    Private _documento As String
    Private _idSistema As Integer
    Private _codEstrategia As String
    Private _codigoEstrategiaActualizar As String

#End Region

#Region "Propiedades"

    Public Property ListCiudades As List(Of Integer)
        Get
            If _listCiudades Is Nothing Then _listCiudades = New List(Of Integer)
            Return _listCiudades
        End Get
        Set(value As List(Of Integer))
            _listCiudades = value
        End Set
    End Property

    Public Property ListBodegas As List(Of Integer)
        Get
            If _listBodegas Is Nothing Then _listBodegas = New List(Of Integer)
            Return _listBodegas
        End Get
        Set(value As List(Of Integer))
            _listBodegas = value
        End Set
    End Property

    Public Property ListProductoExterno As List(Of Integer)
        Get
            If _listProductoExterno Is Nothing Then _listProductoExterno = New List(Of Integer)
            Return _listProductoExterno
        End Get
        Set(value As List(Of Integer))
            _listProductoExterno = value
        End Set
    End Property

    Public Property ListDocumentoFinanciero As List(Of Integer)
        Get
            If _listDocumentoFinanciero Is Nothing Then _listDocumentoFinanciero = New List(Of Integer)
            Return _listDocumentoFinanciero
        End Get
        Set(value As List(Of Integer))
            _listDocumentoFinanciero = value
        End Set
    End Property

    Public Property IdClienteExterno As Integer
        Get
            Return _idClienteExterno
        End Get
        Set(value As Integer)
            _idClienteExterno = value
        End Set
    End Property

    Public Property IdEmpresa As Integer
        Get
            Return _idEmpresa
        End Get
        Set(value As Integer)
            _idEmpresa = value
        End Set
    End Property

    Public Property ListTiposDeServicio As List(Of Integer)
        Get
            If _listTiposDeServicio Is Nothing Then _listTiposDeServicio = New List(Of Integer)
            Return _listTiposDeServicio
        End Get
        Set(value As List(Of Integer))
            _listTiposDeServicio = value
        End Set
    End Property

    Public Property IdTipoCampania As Integer
        Get
            Return _idTipoCampania
        End Get
        Set(value As Integer)
            _idTipoCampania = value
        End Set
    End Property

    Public Property Documento As String
        Get
            Return _documento
        End Get
        Set(value As String)
            _documento = value
        End Set
    End Property

    Public Property IdSistema As Integer
        Get
            Return _idSistema
        End Get
        Set(value As Integer)
            _idSistema = value
        End Set
    End Property

    Public Property CodEstrategia As String
        Get
            Return _codEstrategia
        End Get
        Set(value As String)
            _codEstrategia = value
        End Set
    End Property

    Public Property CodigoEstrategiaActualizar As String
        Get
            Return _codigoEstrategiaActualizar
        End Get
        Set(value As String)
            _codigoEstrategiaActualizar = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function RegistrarFinanciero() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@nombre", SqlDbType.VarChar).Value = _nombre
                    .Add("@fechaInicio", SqlDbType.DateTime).Value = _fechaInicio
                    If _fechaFin <> Date.MinValue Then .Add("@fechaFin", SqlDbType.DateTime).Value = _fechaFin
                    If _fechaFinGestionCem <> Date.MinValue Then .Add("@fechaFinGestionCem", SqlDbType.DateTime).Value = _fechaFinGestionCem
                    If _fechaFinRadicado <> Date.MinValue Then .Add("@fechaFinRadicado", SqlDbType.DateTime).Value = _fechaFinRadicado
                    .Add("@idClienteExterno", SqlDbType.Int).Value = _idClienteExterno
                    .Add("@idEmpresa", SqlDbType.Int).Value = _idEmpresa
                    .Add("@idSistemaOrigen", SqlDbType.Int).Value = _idSistema
                    .Add("@activo", SqlDbType.Bit).Value = _activo
                    .Add("@idTipoCampania", SqlDbType.Int).Value = _idTipoCampania
                    If MetaCliente > 0 Then .Add("@metaCliente", SqlDbType.Int).Value = MetaCliente
                    If MetaCallcenter > 0 Then .Add("@metaCallcenter", SqlDbType.Int).Value = MetaCallcenter
                    If FechaLlegada IsNot Nothing Then .Add("@fechaLlegada", SqlDbType.VarChar).Value = FechaLlegada
                    If _listCiudades IsNot Nothing AndAlso _listCiudades.Count > 0 Then _
                        .Add("@listCiudades", SqlDbType.VarChar).Value = Join(",", _listCiudades.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                    If _listBodegas IsNot Nothing AndAlso _listBodegas.Count > 0 Then _
                        .Add("@listBodegas", SqlDbType.VarChar).Value = Join(",", _listBodegas.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                    If _listProductoExterno IsNot Nothing AndAlso _listProductoExterno.Count > 0 Then _
                        .Add("@listProductoExterno", SqlDbType.VarChar).Value = Join(",", _listProductoExterno.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                    If _listDocumentoFinanciero IsNot Nothing AndAlso _listDocumentoFinanciero.Count > 0 Then _
                        .Add("@listDocumentoFinanciero", SqlDbType.VarChar).Value = Join(",", _listDocumentoFinanciero.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                    If _listTiposDeServicio IsNot Nothing AndAlso _listTiposDeServicio.Count > 0 Then _
                        .Add("@listIdTipoServicio", SqlDbType.VarChar).Value = Join(",", _listTiposDeServicio.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                    .Add("@idCampaniaNotus", SqlDbType.Int).Direction = ParameterDirection.Output
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .IniciarTransaccion()
                .EjecutarNonQuery("RegistrarCampaniaFinanciero", CommandType.StoredProcedure)

                If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                    If Not IsDBNull(.SqlParametros("@idCampaniaNotus").Value) Then
                        resultado.Valor = .SqlParametros("@idCampaniaNotus").Value
                    Else
                        resultado.Valor = .SqlParametros("@resultado").Value
                    End If
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                    If resultado.Valor > 0 Then
                        .ConfirmarTransaccion()
                    Else
                        .AbortarTransaccion()
                    End If
                Else
                    .AbortarTransaccion()
                    resultado.EstablecerMensajeYValor(400, "No se logró establecer respuesta del servidor, por favor intentelo nuevamente.")
                End If

            End With
        Catch ex As Exception
            If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            resultado.EstablecerMensajeYValor(500, "Se presentó un error al generar el registro: " & ex.Message)
        End Try
        Return resultado
    End Function

    Public Function ActualizarFinanciero() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@idCampania", SqlDbType.Int).Value = _idCampania
                    If Not String.IsNullOrEmpty(_nombre) Then .Add("@nombre", SqlDbType.VarChar).Value = _nombre
                    If _fechaInicio > Date.MinValue Then .Add("@fechaInicio", SqlDbType.DateTime).Value = _fechaInicio
                    If _idClienteExterno > 0 Then .Add("@idClienteExterno", SqlDbType.Int).Value = _idClienteExterno
                    If _idEmpresa > 0 Then .Add("@idEmpresa", SqlDbType.Int).Value = _idEmpresa
                    If _fechaFin <> Date.MinValue Then .Add("@fechaFin", SqlDbType.DateTime).Value = _fechaFin
                    If _fechaFinGestionCem <> Date.MinValue Then .Add("@fechaFinGestionCem", SqlDbType.DateTime).Value = _fechaFinGestionCem
                    If _fechaFinRadicado <> Date.MinValue Then .Add("@fechaFinRadicado", SqlDbType.DateTime).Value = _fechaFinRadicado
                    .Add("@activo", SqlDbType.Bit).Value = _activo
                    .Add("@metaCliente", SqlDbType.Int).Value = MetaCliente
                    .Add("@metaCallcenter", SqlDbType.Int).Value = MetaCallcenter
                    .Add("@idTipoCampania", SqlDbType.Int).Value = _idTipoCampania
                    .Add("@idSistemaOrigen", SqlDbType.Int).Value = _idSistema
                    If FechaLlegada IsNot Nothing Then .Add("@fechaLlegada", SqlDbType.VarChar).Value = FechaLlegada
                    If _listCiudades IsNot Nothing AndAlso _listCiudades.Count > 0 Then _
                        .Add("@listCiudades", SqlDbType.VarChar).Value = Join(",", _listCiudades.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                    If _listBodegas IsNot Nothing AndAlso _listBodegas.Count > 0 Then _
                        .Add("@listBodegas", SqlDbType.VarChar).Value = Join(",", _listBodegas.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                    If _listProductoExterno IsNot Nothing AndAlso _listProductoExterno.Count > 0 Then _
                        .Add("@listProductoExterno", SqlDbType.VarChar).Value = Join(",", _listProductoExterno.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                    If _listDocumentoFinanciero IsNot Nothing AndAlso _listDocumentoFinanciero.Count > 0 Then _
                        .Add("@listDocumentoFinanciero", SqlDbType.VarChar).Value = Join(",", _listDocumentoFinanciero.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                    If _listTiposDeServicio IsNot Nothing AndAlso _listTiposDeServicio.Count > 0 Then _
                        .Add("@listIdTipoServicio", SqlDbType.VarChar).Value = Join(",", _listTiposDeServicio.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .IniciarTransaccion()
                .EjecutarNonQuery("ActualizarCampaniaFinanciero", CommandType.StoredProcedure)

                If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                    If resultado.Valor = 0 Then
                        .ConfirmarTransaccion()
                    Else
                        .AbortarTransaccion()
                    End If
                Else
                    .AbortarTransaccion()
                    resultado.EstablecerMensajeYValor(400, "No se logró establecer respuesta del servidor, por favor intentelo nuevamente.")
                End If

            End With
        Catch ex As Exception
            If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            resultado.EstablecerMensajeYValor(500, "Se presentó un error al generar el registro: " & ex.Message)
        End Try
        Return resultado
    End Function

    Public Function ValidarDocumentoCampania() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@idCampania", SqlDbType.Int).Value = _idCampania
                    .Add("@documento", SqlDbType.VarChar).Value = _documento
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .EjecutarNonQuery("ValidaDocumentoPorCampania", CommandType.StoredProcedure)
                If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                Else
                    resultado.EstablecerMensajeYValor(400, "No se logró establecer respuesta del servidor, por favor intentelo nuevamente.")
                End If

            End With
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(500, "Se presentó un error al consultar la información: " & ex.Message)
        End Try
        Return resultado
    End Function

    Public Function Sincronizacion() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim objCampania As New NotusExpressService.NotusExpressService
        Dim wsDatos As New NotusExpressService.WsRegistroCampania
        Dim Wsresultado As New NotusExpressService.ResultadoProceso
        With wsDatos
            .Nombre = _nombre
            .FechaInicio = _fechaInicio
            .IdClienteExterno = _idClienteExterno
            .IdSistema = _idSistema
            .FechaFin = _fechaFin
            .Activo = _activo
            .IdTipoCampania = _idTipoCampania
            If _listBodegas IsNot Nothing AndAlso _listBodegas.Count > 0 Then _
                .ListBodegas = _listBodegas.ToArray
            If _listProductoExterno IsNot Nothing AndAlso _listProductoExterno.Count > 0 Then _
                .ListProductoExterno = _listProductoExterno.ToArray
            If _listDocumentoFinanciero IsNot Nothing AndAlso _listDocumentoFinanciero.Count > 0 Then _
                .ListDocumentoFinanciero = _listDocumentoFinanciero.ToArray
            If _listTiposDeServicio IsNot Nothing AndAlso _listTiposDeServicio.Count > 0 Then _
                .ListTiposDeServicio = _listTiposDeServicio.ToArray
        End With

        Wsresultado = objCampania.RegistraCampania(wsDatos)
        resultado.Valor = Wsresultado.Valor
        resultado.Mensaje = Wsresultado.Mensaje
        Return resultado
    End Function

    Public Function SincronizarActualizacion() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim objCampania As New NotusExpressService.NotusExpressService
        Dim wsDatos As New NotusExpressService.WsRegistroCampania
        Dim Wsresultado As New NotusExpressService.ResultadoProceso
        With wsDatos
            .IdCampania = _idCampania
            .Nombre = _nombre
            .FechaInicio = _fechaInicio
            .IdClienteExterno = _idClienteExterno
            .IdSistema = _idSistema
            .FechaFin = _fechaFin
            .Activo = _activo
            .IdTipoCampania = _idTipoCampania
            If _listBodegas IsNot Nothing AndAlso _listBodegas.Count > 0 Then _
                .ListBodegas = _listBodegas.ToArray
            If _listProductoExterno IsNot Nothing AndAlso _listProductoExterno.Count > 0 Then _
                .ListProductoExterno = _listProductoExterno.ToArray
            If _listDocumentoFinanciero IsNot Nothing AndAlso _listDocumentoFinanciero.Count > 0 Then _
                .ListDocumentoFinanciero = _listDocumentoFinanciero.ToArray
            If _listTiposDeServicio IsNot Nothing AndAlso _listTiposDeServicio.Count > 0 Then _
                .ListTiposDeServicio = _listTiposDeServicio.ToArray
        End With
        Wsresultado = objCampania.ActualizaCampania(wsDatos)
        resultado.Valor = Wsresultado.Valor
        resultado.Mensaje = Wsresultado.Mensaje
        Return resultado
    End Function

    Public Function GuardarCodEstrategia() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@NuevoCodigo", SqlDbType.VarChar).Value = _codEstrategia
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@Mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                End With
                .EjecutarNonQuery("GuardarNuevoCodEstrategia", CommandType.StoredProcedure)
                resultado.Valor = .SqlParametros("@resultado").Value
                resultado.Mensaje = .SqlParametros("@Mensaje").Value
            End With
        Catch ex As Exception
            If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            resultado.EstablecerMensajeYValor(500, "Se presentó un error al guardar el registro: " & ex.Message)
        End Try
        Return resultado
    End Function

    Public Function ActualizarCodEstrategia() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@CodigoAnterior", SqlDbType.VarChar).Value = _codEstrategia
                    .Add("@CodigoNuevo", SqlDbType.VarChar).Value = _codigoEstrategiaActualizar
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@Mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                End With
                .EjecutarNonQuery("ActualizarCodEstrategia", CommandType.StoredProcedure)
                resultado.Valor = .SqlParametros("@resultado").Value
                resultado.Mensaje = .SqlParametros("@mensaje").Value
            End With
        Catch ex As Exception
            If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            resultado.EstablecerMensajeYValor(500, "Se presentó un error al actualizar el registro: " & ex.Message)
        End Try
        Return resultado
    End Function

    Public Function EliminarCodEstrategia() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@CodigoEliminar", SqlDbType.VarChar).Value = _codEstrategia
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@Mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                End With
                .EjecutarNonQuery("EliminarCodEstrategia", CommandType.StoredProcedure)
                resultado.Valor = .SqlParametros("@resultado").Value
                resultado.Mensaje = .SqlParametros("@mensaje").Value
            End With
        Catch ex As Exception
            If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            resultado.EstablecerMensajeYValor(500, "Se presentó un error al eliminar el registro: " & ex.Message)
        End Try
        Return resultado
    End Function

    Public Function AsociarCodEstrategiaCampania() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@idCampania", SqlDbType.VarChar).Value = _idCampania
                    .Add("@codEstrategia", SqlDbType.VarChar).Value = _codEstrategia
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@Mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                End With
                .EjecutarNonQuery("AsociarEstrategiaCampania", CommandType.StoredProcedure)
                resultado.Valor = .SqlParametros("@resultado").Value
                resultado.Mensaje = .SqlParametros("@mensaje").Value
            End With
        Catch ex As Exception
            If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            resultado.EstablecerMensajeYValor(500, "Se presentó un error al asociar el registro: " & ex.Message)
        End Try
        Return resultado
    End Function


#End Region

End Class
