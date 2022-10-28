
Imports System.Web
Imports LMDataAccessLayer

Public Class RedistribucionSerialesCE

#Region "Variables"
    Private _idCreador As Integer
    Private _logonUser As String
    Private _idOrden As Integer
    Private _OrdenServicio As String
    Private _distribucion As Integer
    Private _idServicio As Integer
    Private _observacion As String
    Private _idRuta As Integer
    Private _automatico As Boolean
    Private _numGuia As String
    Private _creacionServicio As Integer
    Private _origen As String
    Private _reimpresion As String
    Private _gerenciaCliente As Integer
    Private _tipoElemento As Integer
    Private _Marca As Integer
    Private _Modelo As Integer
    Private _Serial As String
    Private _numeroLinea As Integer
    Private _Color As Integer
    Private _fechaAlta As Date
    Private _Garantia As Integer
    Private _idFalla As Integer
    Private _idAccesorio As Integer
    Private _detalle As DataTable
    Private _idOrigen As Integer
    Private _idDestino As Integer

    Private _proceso As String

    Private _dtInfo As DataTable


#End Region

#Region "Propiedades"

    Public Property IdCreador() As Integer
        Get
            Return _idCreador
        End Get
        Set(ByVal value As Integer)
            _idCreador = value
        End Set
    End Property
    Public Property LogonUser() As String
        Get
            Return _logonUser
        End Get
        Set(ByVal value As String)
            _logonUser = value
        End Set
    End Property
    Public Property IdOrden() As Integer
        Get
            Return _idOrden
        End Get
        Set(ByVal value As Integer)
            _idOrden = value
        End Set
    End Property

    Public Property IdRuta() As Integer
        Get
            Return _idRuta
        End Get
        Set(value As Integer)
            _idRuta = value
        End Set
    End Property

    Public Property Automatico() As Boolean
        Get
            Return _automatico
        End Get
        Set(value As Boolean)
            _automatico = value
        End Set
    End Property
    Public Property NumGuia() As String
        Get
            Return _numGuia
        End Get
        Set(value As String)
            _numGuia = value
        End Set
    End Property

    Public Property Origen() As String
        Get
            Return _origen
        End Get
        Set(value As String)
            _origen = value
        End Set
    End Property

    Public Property Reimpresion() As String
        Get
            Return _reimpresion
        End Get
        Set(value As String)
            _reimpresion = value
        End Set
    End Property


    Public Property Observacion() As String
        Get
            Return _observacion
        End Get
        Set(value As String)
            _observacion = value
        End Set
    End Property

    Public Property idServicio() As Integer
        Get
            Return _idServicio
        End Get
        Set(ByVal value As Integer)
            _idServicio = value
        End Set
    End Property

    Public Property gerenciaCliente() As Integer
        Get
            Return _gerenciaCliente
        End Get
        Set(ByVal value As Integer)
            _gerenciaCliente = value
        End Set
    End Property

    Public Property TipoElemento() As Integer
        Get
            Return _tipoElemento
        End Get
        Set(ByVal value As Integer)
            _tipoElemento = value
        End Set
    End Property

    Public Property Marca() As Integer
        Get
            Return _Marca
        End Get
        Set(ByVal value As Integer)
            _Marca = value
        End Set
    End Property
    Public Property Modelo() As Integer
        Get
            Return _Modelo
        End Get
        Set(ByVal value As Integer)
            _Modelo = value
        End Set
    End Property
    Public Property Serial() As String
        Get
            Return _Serial
        End Get
        Set(ByVal value As String)
            _Serial = value
        End Set
    End Property
    Public Property numeroLinea() As Integer
        Get
            Return _numeroLinea
        End Get
        Set(ByVal value As Integer)
            _numeroLinea = value
        End Set
    End Property
    Public Property Color() As Integer
        Get
            Return _Color
        End Get
        Set(ByVal value As Integer)
            _Color = value
        End Set
    End Property
    Public Property FechaAlta() As Date
        Get
            Return _fechaAlta
        End Get
        Set(ByVal value As Date)
            _fechaAlta = value
        End Set
    End Property

    Public Property Garantia() As Boolean
        Get
            Return _Garantia
        End Get
        Set(ByVal value As Boolean)
            _Garantia = value
        End Set
    End Property
    Public Property OrdenServicio() As String
        Get
            Return _OrdenServicio
        End Get
        Set(ByVal value As String)
            _OrdenServicio = value
        End Set
    End Property

    Public Property Distribucion() As Integer
        Get
            Return _distribucion
        End Get
        Set(value As Integer)
            _distribucion = value
        End Set
    End Property

    Public Property IdFalla() As Integer
        Get
            Return _idFalla
        End Get
        Set(ByVal value As Integer)
            _idFalla = value
        End Set
    End Property

    Public Property CreacionServicio() As Integer
        Get
            Return _creacionServicio
        End Get
        Set(value As Integer)
            _creacionServicio = value
        End Set
    End Property

    Public Property IdAccesorio() As Integer
        Get
            Return _idAccesorio
        End Get
        Set(ByVal value As Integer)
            _idAccesorio = value
        End Set
    End Property

    Public Property IdOrigen() As Integer
        Get
            Return _idOrigen
        End Get
        Set(ByVal value As Integer)
            _idOrigen = value
        End Set
    End Property

    Public Property IdDestino() As Integer
        Get
            Return _idDestino
        End Get
        Set(ByVal value As Integer)
            _idDestino = value
        End Set
    End Property

    Public Property DtInfo As DataTable
        Get
            Return _dtInfo
        End Get
        Set(value As DataTable)
            _dtInfo = value
        End Set
    End Property

    Public Property Proceso() As String
        Get
            Return _proceso
        End Get
        Set(value As String)
            _proceso = value
        End Set
    End Property

    Public ReadOnly Property Detalle() As DataTable
        Get
            If _detalle Is Nothing Then CargarDetalle()
            Return _detalle
        End Get
    End Property


#End Region

#Region "constructores"
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal idOrden As Long)
        Me.New()
        'Me.CargarDatos(idOrden)
        '_idOrden = idOrden
    End Sub
#End Region

#Region "Metodos Publicos"

    Public Function Crear(Optional ByVal ordenesRecepcion As ArrayList = Nothing) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                Using dtAux As DataTable = _detalle.Copy
                    ' Ingresa el detalle obtenido en la grilla
                    resultado = RegistrarDetalleOrden(dtAux, dbManager)
                End Using

                _detalle.Dispose()
                _detalle = Nothing
                .IniciarTransaccion()
                If resultado.Valor = 1 Then
                    With .SqlParametros
                        .Clear()
                        .Add("@idOrigen", SqlDbType.Int).Value = _idOrigen
                        .Add("@idDestino", SqlDbType.Int).Value = _idDestino
                        .Add("@observacion", SqlDbType.VarChar).Value = _observacion
                        .Add("@idUsuario", SqlDbType.BigInt).Value = _idCreador
                        .Add("@nombreEquipo", SqlDbType.VarChar).Value = _logonUser
                        .Add("@idServicioCreado", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .Add("@guia", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                        .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With

                    Dim result As Short = 0
                    Dim mensaje As String

                    .EjecutarNonQuery("RegistrarRedistribucionCE", CommandType.StoredProcedure)
                    Short.TryParse(.SqlParametros("@result").Value.ToString, result)
                    mensaje = .SqlParametros("@mensaje").Value.ToString
                    resultado.EstablecerMensajeYValor(result, mensaje)
                    If result = 1 Then
                        _idRuta = CLng(.SqlParametros("@idServicioCreado").Value)
                        _numGuia = CStr(.SqlParametros("@guia").Value)
                        .ConfirmarTransaccion()
                    ElseIf result = 99 Then
                        .AbortarTransaccion()
                    Else
                        .AbortarTransaccion()
                        Throw New Exception("Imposible registrar la información de la Orden en la Base de Datos.")
                    End If
                Else
                    Throw New Exception(resultado.Mensaje)
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso .EstadoTransaccional Then .AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return resultado
    End Function

    Public Function CrearRetorno(Optional ByVal ordenesRecepcion As ArrayList = Nothing) As Boolean
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim retorno As Boolean
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                Using dtAux As DataTable = _detalle.Copy
                    resultado = RegistrarDetalleOrdenRetorno(dtAux, dbManager)
                End Using

                _detalle.Dispose()
                _detalle = Nothing
                .IniciarTransaccion()
                If resultado.Valor = 1 Then
                    With .SqlParametros
                        .Clear()
                        .Add("@idUsuario", SqlDbType.BigInt).Value = _idCreador
                        .Add("@nombreEquipo", SqlDbType.VarChar).Value = _logonUser
                        .Add("@idServicioCreado", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With

                    Dim result As Short = 0

                    .EjecutarNonQuery("RegistrarRecepcionReparacionCE", CommandType.StoredProcedure)
                    Short.TryParse(.SqlParametros("@result").Value.ToString, result)

                    If result = 1 Then
                        _idRuta = CLng(.SqlParametros("@idServicioCreado").Value)
                        retorno = True
                        .ConfirmarTransaccion()
                    Else
                        .AbortarTransaccion()
                        Throw New Exception("Imposible registrar la información de la Orden en la Base de Datos.")
                    End If
                    retorno = True
                Else
                    .AbortarTransaccion()
                    Throw New Exception(resultado.Mensaje)
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso .EstadoTransaccional Then .AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return retorno
    End Function

    ''' <summary>
    ''' Actualizar la Orden de Servicio de una Orden VIP
    ''' </summary>
    ''' <returns>true o false si se inserta el servicio</returns>
    ''' <remarks></remarks>
    Public Function ActualizarOrdenServicio() As Boolean
        Dim Resultado As Boolean = False
        Dim idUsuario As Integer = 0
        Dim idCiudad As Integer = 944
        If HttpContext.Current.Session("usxp001") IsNot Nothing Then Integer.TryParse(HttpContext.Current.Session("usxp001"), idUsuario)
        If HttpContext.Current.Session("usxp007") IsNot Nothing Then Integer.TryParse(HttpContext.Current.Session("usxp007"), idCiudad)

        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        With dbManager
            Try
                With .SqlParametros
                    '.Add("@idServicioMensajeria", SqlDbType.Int).Direction = ParameterDirection.Output
                    .Add("@idServicio", SqlDbType.Int).Value = _idServicio
                    .Add("@numeroRadicado", SqlDbType.Int).Value = CInt(_OrdenServicio)
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With

                Dim result As Short = 0

                .EjecutarNonQuery("ActualizarServicioVIP", CommandType.StoredProcedure)
                Short.TryParse(.SqlParametros("@resultado").Value.ToString, result)
                If result = 1 Then
                    '_idOrden = CLng(.SqlParametros("@idOrdenCompra").Value)
                    Resultado = True
                Else
                    Throw New Exception("Imposible actualizar la información de la Orden en la Base de Datos.")
                End If


            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso .EstadoTransaccional Then .AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With

        Return Resultado
    End Function

    Public Sub AdicionarDetalle(ByVal dtInfoDetalle As DataTable)
        If _detalle Is Nothing Then _detalle = GenerarEstructuraTablaDetalle()
        For Each drOrigenDetalle As DataRow In dtInfoDetalle.Rows
            _detalle.ImportRow(drOrigenDetalle)
        Next
    End Sub

    Public Sub AdicionarDetalleRetornoHub(ByVal dtInfoDetalle As DataTable)
        If _detalle Is Nothing Then _detalle = GenerarEstructuraTablaDetalleRetorno()
        For Each drOrigenDetalle As DataRow In dtInfoDetalle.Rows
            _detalle.ImportRow(drOrigenDetalle)
        Next
    End Sub

    Public Function ObtenerRemisionRedistribucionSerialesCE() As DataSet
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim _dsDatos As New DataSet
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    If _idServicio > 0 Then .Add("@idServicio", SqlDbType.Int).Value = _idServicio
                    If _numGuia IsNot Nothing Then .Add("@numGuia", SqlDbType.VarChar).Value = _numGuia
                    If _idRuta > 0 Then .Add("@idRuta", SqlDbType.Int).Value = _idRuta
                    If _origen IsNot Nothing Then .Add("@origen", SqlDbType.VarChar).Value = _origen
                    If _reimpresion IsNot Nothing Then .Add("@reimpresion", SqlDbType.VarChar).Value = _reimpresion
                End With
                _dsDatos = .EjecutarDataSet("ObtenerInformacionRemisionRedistribucionSerialesCE", CommandType.StoredProcedure)
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso .EstadoTransaccional Then .AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return _dsDatos
    End Function

    Public Function ValidarSerialInventarioOrigen() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@serial", SqlDbType.VarChar).Value = _Serial
                    .Add("@idOrigen", SqlDbType.Int).Value = _idOrigen
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With

                Dim _result As Integer

                .EjecutarNonQuery("ValidarSerialInventarioOrigen", CommandType.StoredProcedure)
                Integer.TryParse(.SqlParametros("@resultado").Value.ToString, _result)

                If _result = 1 Then
                    resultado.EstablecerMensajeYValor(1, "Serial existe.")
                ElseIf _result = 0 Then
                    resultado.EstablecerMensajeYValor(0, "Serial no existe en el sistema.")
                ElseIf _result = 999 Then
                    resultado.EstablecerMensajeYValor(999, "Serial digitado existe en el sistema pero no pertenece al origen seleccionado.")
                ElseIf _result = 500 Then
                    resultado.EstablecerMensajeYValor(999, "Serial digitado existe en el sistema pero se encuentra inactivo.")
                End If
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
        End Try
        Return resultado
    End Function

    Public Function ObtenerOrigenDestinoServicio() As ResultadoProceso
        Dim dbManager As New LMDataAccess
        Dim _result As New ResultadoProceso
        Try
            With dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@numeroOrdenes", SqlDbType.VarChar, 100).Value = _OrdenServicio
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                _dtInfo = .EjecutarDataTable("ObtenerOrigenDestinoServicio", CommandType.StoredProcedure)

                Integer.TryParse(.SqlParametros("@resultado").Value.ToString(), _result.Valor)

            End With
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
        End Try
        Return _result
    End Function

    Public Function ObtenerRemisionRetornoSinHUB() As DataSet
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim _dsDatos As New DataSet
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idRuta", SqlDbType.Int).Value = _idRuta
                End With
                _dsDatos = .EjecutarDataSet("ObtenerInformacionRemisionSinHub", CommandType.StoredProcedure)
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso .EstadoTransaccional Then .AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return _dsDatos
    End Function

    'Public Function AnularGuia(ByVal guia As String, ByVal idusuario As Integer, ByVal observacion As String) As ResultadoProceso
    '    Dim dbManager As New LMDataAccessLayer.LMDataAccess
    '    Dim resultado As New ResultadoProceso

    '    Using dbManager
    '        With dbManager
    '            With .SqlParametros
    '                .Clear()
    '                .Add("@idUsuario", SqlDbType.BigInt).Value = idusuario
    '                .Add("@guia", SqlDbType.VarChar).Value = guia
    '                .Add("@mensaje", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output
    '                .Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
    '            End With

    '            .EjecutarNonQuery("AnularGuiaSinHub", CommandType.StoredProcedure)

    '            Dim mensaje As String
    '            Dim result As Integer

    '            Integer.TryParse(.SqlParametros("@result").Value.ToString, result)
    '            mensaje = .SqlParametros("@mensaje").Value

    '            If result = 1 Then
    '                resultado.EstablecerMensajeYValor(result, mensaje)
    '            Else
    '                resultado.EstablecerMensajeYValor(result, mensaje)
    '            End If
    '        End With
    '    End Using
    '    Return resultado
    'End Function

    Public Function AnularGuia(ByVal guia As String, ByVal idusuario As Integer, ByVal observacion As String) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso

        Using dbManager
            With dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.BigInt).Value = idusuario
                    .Add("@guia", SqlDbType.VarChar).Value = guia
                    .Add("@observacion", SqlDbType.VarChar).Value = observacion
                    .Add("@mensaje", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With

                .EjecutarNonQuery("AnularGuiaSinHub", CommandType.StoredProcedure)

                Dim mensaje As String
                Dim result As Integer

                Integer.TryParse(.SqlParametros("@result").Value.ToString, result)
                mensaje = .SqlParametros("@mensaje").Value

                If result = 1 Then
                    resultado.EstablecerMensajeYValor(result, mensaje)
                Else
                    resultado.EstablecerMensajeYValor(result, mensaje)
                End If
            End With
        End Using
        Return resultado
    End Function

    Public Function CrearReDistribucion() As Boolean
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim retorno As Boolean
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                Using dtAux As DataTable = _detalle.Copy
                    resultado = RegistrarDetalleOrdenRetorno(dtAux, dbManager)
                End Using

                _detalle.Dispose()
                _detalle = Nothing
                .IniciarTransaccion()
                If resultado.Valor = 1 Then
                    With .SqlParametros
                        .Clear()
                        .Add("@idUsuario", SqlDbType.BigInt).Value = _idCreador
                        .Add("@nombreEquipo", SqlDbType.VarChar).Value = _logonUser
                        .Add("@proceso", SqlDbType.VarChar).Value = _proceso
                        .Add("@idServicioCreado", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With

                    Dim result As Short = 0

                    .EjecutarNonQuery("RegistrarReDistribucionSinHub", CommandType.StoredProcedure)
                    Short.TryParse(.SqlParametros("@result").Value.ToString, result)

                    If result = 1 Then
                        _idRuta = CLng(.SqlParametros("@idServicioCreado").Value)
                        retorno = True
                        .ConfirmarTransaccion()
                    Else
                        .AbortarTransaccion()
                        Throw New Exception("Imposible registrar la información de la Orden en la Base de Datos.")
                    End If
                    retorno = True
                Else
                    .AbortarTransaccion()
                    Throw New Exception(resultado.Mensaje)
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso .EstadoTransaccional Then .AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return retorno
    End Function

    Function ValidarTac() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@serial", SqlDbType.VarChar).Value = _Serial
                    .Add("@idModelo", SqlDbType.Int).Value = _Modelo
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With

                Dim _result As Integer

                .EjecutarNonQuery("ValidarCodigoTacSinHub", CommandType.StoredProcedure)
                Integer.TryParse(.SqlParametros("@resultado").Value.ToString, _result)

                resultado.Valor = _result

            End With
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
        End Try
        Return resultado
    End Function

#End Region

#Region "Metodos Privados"

    Private Function GenerarEstructuraTablaDetalle() As DataTable
        Dim dtDetalle As New DataTable
        With dtDetalle.Columns
            .Add("idProducto", GetType(Integer))
            .Add("serial", GetType(String))
            .Add("producto", GetType(String))
            .Add("codigo", GetType(String))
            '.Add("idColor", GetType(Integer))
            '.Add("idElemento", GetType(Integer))
            '.Add("idAccesorio", GetType(String))
            '.Add("idFallas", GetType(String))
            '.Add("elemento", GetType(String))
            '.Add("marca", GetType(String))
            '.Add("modelo", GetType(String))
            '.Add("color", GetType(String))
            '.Add("fallas", GetType(String))
            '.Add("accesorios", GetType(String))
            '.Add("tipoContenedor", GetType(Integer))
            '.Add("idContenedor", GetType(String))
            .Add("bodegaOrigen", GetType(String))
            .Add("idOrigen", GetType(String))
            .Add("idDestino", GetType(Integer))
        End With
        Dim pkKeys() As DataColumn = {dtDetalle.Columns("idProducto")}
        dtDetalle.PrimaryKey = pkKeys
        Return dtDetalle
    End Function

    Private Function GenerarEstructuraTablaDetalleRetorno() As DataTable
        Dim dtDetalle As New DataTable
        With dtDetalle.Columns
            .Add("idDetalleOrden", GetType(Integer))
            .Add("numeroOrden", GetType(String))
            .Add("serial", GetType(String))
            .Add("idFalla", GetType(Integer))
            .Add("falla", GetType(String))
            .Add("diagnostico", GetType(String))
            .Add("idOrigen", GetType(String))
            .Add("idDestino", GetType(String))
            .Add("idTipoContenedor", GetType(Integer))
            .Add("tipoContenedor", GetType(String))
            .Add("idContenedor", GetType(Integer))
            .Add("valorManoObra", GetType(Double))
            .Add("valorRepuesto", GetType(Double))
            .Add("idTipoGarantia", GetType(Integer))
            .Add("tipoGarantia", GetType(String))
        End With
        Dim pkKeys() As DataColumn = {dtDetalle.Columns("idDetalleOrden")}
        dtDetalle.PrimaryKey = pkKeys
        Return dtDetalle
    End Function

    Private Function RegistrarDetalleOrden(ByVal dtDetalle As DataTable, ByVal dbManager As LMDataAccess) As ResultadoProceso
        Dim dcAux As DataColumn = Nothing
        Dim libRetorno As Integer
        Dim resultado As New ResultadoProceso

        If dtDetalle.Columns.Contains("idUsuario") Then dtDetalle.Columns.Remove("idUsuario")
        dcAux = New DataColumn("idUsuario", GetType(Integer))
        dcAux.DefaultValue = _idCreador
        dtDetalle.Columns.Add(dcAux)

        If dtDetalle.Columns.Contains("nombreEquipo") Then dtDetalle.Columns.Remove("nombreEquipo")
        dcAux = New DataColumn("nombreEquipo", GetType(String))
        dcAux.DefaultValue = _logonUser
        dtDetalle.Columns.Add(dcAux)

        With dbManager
            With .SqlParametros
                .Clear()
                .Add("@idUsuario", SqlDbType.Int).Value = _idCreador
                .Add("@logonUser", SqlDbType.VarChar).Value = _logonUser
                .Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            End With

            .EjecutarNonQuery("LiberarRegistrosTransitoriaRedistribucionCE", CommandType.StoredProcedure)
            Integer.TryParse(.SqlParametros("@result").Value.ToString, libRetorno)

            If libRetorno = 1 Then
                '***Se registra el Detalle de la Orden***'
                .InicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "TransitoriaRedistribucionCE"
                    .ColumnMappings.Add("serial", "serial")
                    .ColumnMappings.Add("producto", "producto")
                    .ColumnMappings.Add("codigo", "codigo")
                    '.ColumnMappings.Add("idColor", "idColor")
                    '.ColumnMappings.Add("idElemento", "idElemento")
                    '.ColumnMappings.Add("idAccesorio", "idAccesorio")
                    '.ColumnMappings.Add("idFallas", "idFalla")
                    .ColumnMappings.Add("idUsuario", "idUsuario")
                    .ColumnMappings.Add("nombreEquipo", "nombreEquipo")
                    .ColumnMappings.Add("idOrigen", "idOrigen")
                    .ColumnMappings.Add("idDestino", "idDestino")
                    '.ColumnMappings.Add("tipoContenedor", "idTipoContenedor")
                    '.ColumnMappings.Add("idContenedor", "idContenedor")
                    '.ColumnMappings.Add("fallas", "falla")
                    '.ColumnMappings.Add("accesorios", "accesorios")
                    .WriteToServer(dtDetalle)
                End With

                resultado.EstablecerMensajeYValor(1, "Se inserto correctamente el detalle en la tabla transitoria")
            Else
                resultado.EstablecerMensajeYValor(0, "Error al insertar el detalle en la tabla transitoria")
            End If
            .SqlParametros.Clear()
        End With
        Return resultado
    End Function

    Private Function RegistrarDetalleOrdenRetorno(ByVal dtDetalle As DataTable, ByVal dbManager As LMDataAccess) As ResultadoProceso
        Dim dcAux As DataColumn = Nothing
        Dim libRetorno As Integer
        Dim resultado As New ResultadoProceso

        If dtDetalle.Columns.Contains("idUsuario") Then dtDetalle.Columns.Remove("idUsuario")
        dcAux = New DataColumn("idUsuario", GetType(Integer))
        dcAux.DefaultValue = _idCreador
        dtDetalle.Columns.Add(dcAux)

        If dtDetalle.Columns.Contains("nombreEquipo") Then dtDetalle.Columns.Remove("nombreEquipo")
        dcAux = New DataColumn("nombreEquipo", GetType(String))
        dcAux.DefaultValue = _logonUser
        dtDetalle.Columns.Add(dcAux)
        With dbManager
            With .SqlParametros
                .Clear()
                .Add("@idUsuario", SqlDbType.Int).Value = _idCreador
                .Add("@logonUser", SqlDbType.VarChar).Value = _logonUser
                .Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            End With
            .EjecutarNonQuery("LiberarRegistrosTransitoriaServicioSinHub", CommandType.StoredProcedure)
            Integer.TryParse(.SqlParametros("@result").Value.ToString, libRetorno)
            If libRetorno = 1 Then
                '***Se registra el Detalle de la Orden***'
                .InicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "TransitoriaServicioSinHub"
                    .ColumnMappings.Add("numeroOrden", "numeroOrden")
                    .ColumnMappings.Add("serial", "serial")
                    .ColumnMappings.Add("idFalla", "idFalla")
                    .ColumnMappings.Add("diagnostico", "diagnostico")
                    .ColumnMappings.Add("idOrigen", "idOrigen")
                    .ColumnMappings.Add("idDestino", "idDestino")
                    .ColumnMappings.Add("idUsuario", "idUsuario")
                    .ColumnMappings.Add("nombreEquipo", "nombreEquipo")
                    .ColumnMappings.Add("idTipoContenedor", "idTipoContenedor")
                    .ColumnMappings.Add("idContenedor", "idContenedor")
                    .ColumnMappings.Add("valorManoObra", "ValorManoObra")
                    .ColumnMappings.Add("valorRepuesto", "ValorRepuesto")
                    .ColumnMappings.Add("idTipoGarantia", "TipoGarantia")
                    .WriteToServer(dtDetalle)
                End With
                resultado.EstablecerMensajeYValor(1, "Se inserto correctamente el detalle en la tabla transitoria")
            Else
                resultado.EstablecerMensajeYValor(0, "Error al insertar el detalle en la tabla transitoria")
            End If
            .SqlParametros.Clear()
        End With
        Return resultado
    End Function

    Private Sub CargarDetalle()
        If _detalle Is Nothing Then _detalle = GenerarEstructuraTablaDetalle()
        If _idOrden > 0 Then
            Dim dbManager As New LMDataAccess
            Dim drDetalle As DataRow

            With dbManager
                .SqlParametros.Add("@idOrden", SqlDbType.Int).Value = _idOrden
                .ejecutarReader("ObtenerInfoDetalleOrdenCompra", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    While .Reader.Read
                        drDetalle = _detalle.NewRow
                        drDetalle("idDetalleOrden") = .Reader("idDetalle").ToString
                        drDetalle("fabricante") = .Reader("fabricante").ToString
                        drDetalle("idFabricante") = .Reader("idFabricante").ToString
                        drDetalle("producto") = .Reader("producto").ToString
                        drDetalle("idProducto") = .Reader("idProducto").ToString
                        drDetalle("tipoUnidad") = .Reader("tipoUnidad").ToString
                        drDetalle("idTipoUnidad") = .Reader("idTipoUnidad").ToString
                        drDetalle("cantidad") = .Reader("cantidad").ToString
                        drDetalle("valorUnitario") = .Reader("valorUnitario").ToString
                        drDetalle("observacion") = .Reader("observacion").ToString
                        drDetalle("idTipoDetalle") = .Reader("idTipoDetalle").ToString
                        _detalle.Rows.Add(drDetalle)
                        drDetalle.AcceptChanges()
                    End While
                    .Reader.Close()
                End If
            End With
        End If
    End Sub

#End Region


End Class
