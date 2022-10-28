Imports ILSBusinessLayer.Estructuras
Imports LMDataAccessLayer
Imports System.Net.Mail
Imports System.Text
Imports ILSBusinessLayer.Comunes

Namespace OMS

    Public Class OrdenEnvioNacionalizacion

#Region "Atributos"
        Private _idEnvio As Long
        Private _idFacturaGuia As Long
        Private _idCreador As Long
        Private _idEstado As Integer
        Private _fechaCreacion As Date
        Private _fechaCierre As Date
        Private _idUsuarioCierre As Long
        Private _nombreDocumentoImportacion As String
        Private _observacion As String
        Private _numeroFactura As String
        Private _numeroGuia As String
        Private _infoDetalleEnvio As DataTable
        Private _infoSerial As DataTable
        Private _dtInfoCarga As DataTable
        Private _idUsuarioCarga As Integer
        Private _cantidad As Integer

        'Envio Notificación Nacionalización


#End Region

#Region "Constructores"
        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            _idEnvio = identificador
            CargarInformacion()
        End Sub
#End Region

#Region "Propiedades"
        Public ReadOnly Property IdEnvio() As Long
            Get
                Return _idEnvio
            End Get
        End Property

        Public Property IdFacturaGuia() As Long
            Get
                Return _idFacturaGuia
            End Get
            Set(ByVal value As Long)
                _idFacturaGuia = value
            End Set
        End Property

        Public Property IdCreador() As Long
            Get
                Return _idCreador
            End Get
            Set(ByVal value As Long)
                _idCreador = value
            End Set
        End Property

        Public Property IdEstado() As Long
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Long)
                _idEstado = value
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

        Public Property FechaCierre() As Date
            Get
                Return _fechaCierre
            End Get
            Set(ByVal value As Date)
                _fechaCierre = value
            End Set
        End Property

        Public Property IdUsuarioCierre() As Long
            Get
                Return _idUsuarioCierre
            End Get
            Set(ByVal value As Long)
                _idUsuarioCierre = value
            End Set
        End Property

        Public Property NombreDocumentoImportacion() As String
            Get
                Return _nombreDocumentoImportacion
            End Get
            Set(ByVal value As String)
                _nombreDocumentoImportacion = value
            End Set
        End Property

        Public Property Observacion() As String
            Get
                Return _observacion
            End Get
            Set(ByVal value As String)
                _observacion = value
            End Set
        End Property

        Public Property NumeroFactura() As String
            Get
                Return _numeroFactura
            End Get
            Set(ByVal value As String)
                _numeroFactura = value
            End Set
        End Property

        Public Property NumeroGuia() As String
            Get
                Return _numeroGuia
            End Get
            Set(ByVal value As String)
                _numeroGuia = value
            End Set
        End Property

        Public ReadOnly Property InfoDetalleEnvio() As DataTable
            Get
                If _infoDetalleEnvio Is Nothing Then CargarListadoDetalle()
                Return _infoDetalleEnvio
            End Get
        End Property

        Public ReadOnly Property InfoSerial() As DataTable
            Get
                If _infoSerial Is Nothing Then CargarListadoSerial()
                Return _infoSerial
            End Get
        End Property

        Public Property IdUsuarioCarga() As Integer
            Get
                Return _idUsuarioCarga
            End Get
            Set(ByVal value As Integer)
                _idUsuarioCarga = value
            End Set
        End Property

        Public Property dtInfoCarga() As DataTable
            Get
                Return _dtInfoCarga
            End Get
            Set(ByVal value As DataTable)
                _dtInfoCarga = value
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
#End Region

#Region "Metodos Publicos"

        Public Function Actualizar(ByVal ListaCantidadRegiones As List(Of Estructuras.FiltroEnvioSerial)) As Short
            Dim resultado As Integer

            If IdEnvio <> 0 Then
                Dim db As New LMDataAccessLayer.LMDataAccess

                Try
                    db.iniciarTransaccion()
                    With db.SqlParametros
                        .Add("@idEnvio", SqlDbType.BigInt).Value = Me.IdEnvio
                        .Add("@idEstado", SqlDbType.Int).Value = Me.IdEstado
                        .Add("@idUsuarioCierre", SqlDbType.BigInt).Value = Me.IdUsuarioCierre
                        .Add("@nombreDocumentoImportacion", SqlDbType.VarChar, 250).Value = Me.NombreDocumentoImportacion
                        .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    db.ejecutarNonQuery("ActualizarOrdenEnvioNacionalizacion", CommandType.StoredProcedure)

                    resultado = CShort(db.SqlParametros("@returnValue").Value)

                    If resultado = 0 Then
                        For Each objRegion As Estructuras.FiltroEnvioSerial In ListaCantidadRegiones
                            With db.SqlParametros
                                .Clear()
                                .Add("@idEnvio", SqlDbType.BigInt).Value = Me.IdEnvio
                                .Add("@numeroNacionalizacion", SqlDbType.VarChar, 50).Value = objRegion.NumeroNacionalizacion
                                .Add("@idRegion", SqlDbType.Int).Value = objRegion.idRegion
                                .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                            End With
                            db.ejecutarNonQuery("ActualizarEnvioSerial", CommandType.StoredProcedure)
                        Next
                        db.confirmarTransaccion()
                    End If

                    If db.estadoTransaccional Then db.abortarTransaccion()
                Catch ex As Exception
                    If db.estadoTransaccional Then db.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            Else
                Throw New Exception("La Orden de envio a Nacionalizacion aún no ha sido registrada en la Base de Datos.")
            End If

            Return resultado

        End Function

        Public Sub Crear(ByVal al As ArrayList)
            Dim idsOrdenes As String = String.Empty
            Dim db As New LMDataAccess
            With db
                EstablecerParametros(db)

                Try
                    Dim result As Integer = 0
                    .iniciarTransaccion()

                    ' cambiar por array list el data table
                    idsOrdenes = Join(al.ToArray, ",")

                    .ejecutarNonQuery("CrearOrdenEnvioNacionalizacion", CommandType.StoredProcedure)
                    result = .SqlParametros("@result").Value
                    If result = 0 Then
                        _idEnvio = CLng(.SqlParametros("@identity").Value)

                        With .SqlParametros
                            .Clear()
                            .Add("@idEnvio", SqlDbType.BigInt).Value = _idEnvio
                            .Add("@listaOrden", SqlDbType.VarChar, 8000).Value = idsOrdenes
                            .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                        End With

                        ' Agrega las ordenes seleccionadas y las relaciona con el envio
                        ' A los seriales que pertenecen a las ordenes que no han sido nacionalizados tambien se agregan.
                        .ejecutarNonQuery("CrearEnvioNacionalizacionDetalle", CommandType.StoredProcedure)
                        result = .SqlParametros("@result").Value

                        If result <> 0 Then
                            If .estadoTransaccional Then .abortarTransaccion()
                            Throw New Exception("Imposible registrar la información de la Orden de Envio en la Base de Datos.")
                        End If

                        .confirmarTransaccion()
                    Else
                        If .estadoTransaccional Then .abortarTransaccion()
                        Throw New Exception("Imposible registrar la información de la Orden de Envio en la Base de Datos.")
                    End If

                Catch ex As Exception
                    If .estadoTransaccional Then .abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
        End Sub

        Public Function BorrarNacionalizacionTemporal() As Short
            Dim dbManager As New LMDataAccess
            Dim resultado As Short

            If _idUsuarioCarga > 0 AndAlso _idEnvio > 0 Then

                Try
                    With dbManager
                        .iniciarTransaccion()
                        .SqlParametros.Add("@idEnvio", SqlDbType.BigInt).Value = _idEnvio
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuarioCarga
                        .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                        .ejecutarNonQuery("BorrarNacionalizacionTemporal", CommandType.StoredProcedure)

                        Short.TryParse(.SqlParametros("@returnValue").Value, resultado)
                        If resultado = 0 Then
                            .confirmarTransaccion()
                        Else
                            If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception("Error al tratar de borrar los datos cargados temporalmente en BD." & ex.Message)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 3
            End If

            Return resultado
        End Function

        Public Function CargarDatosParaValidacion() As Short
            Dim dbManager As New LMDataAccess
            Dim resultado As Short
            If _dtInfoCarga IsNot Nothing AndAlso _dtInfoCarga.Rows.Count > 0 Then
                Try
                    With dbManager
                        .iniciarTransaccion()
                        .inicilizarBulkCopy()
                        With .BulkCopy
                            .DestinationTableName = "NacionalizacionSerialesTemporal"
                            .ColumnMappings.Add("serial", "serial")
                            .ColumnMappings.Add("numeroNacionalizacion", "numeroNacionalizacion")
                            .ColumnMappings.Add("numlinea", "numlinea")
                            .ColumnMappings.Add("idEnvio", "idEnvio")
                            .ColumnMappings.Add("idUsuario", "idUsuario")
                            .WriteToServer(_dtInfoCarga)
                        End With
                        .confirmarTransaccion()
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception("Error al tratar de cargar temporalmente los datos a la BD para realizar validaciones complementarias." & ex.Message)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 3
            End If

            Return resultado
        End Function

        Public Function BuscarErroresDeIntegridad(ByRef dtError As DataTable) As Boolean
            Dim dbManager As New LMDataAccess
            Dim resultado As Short = 0
            If _idEnvio > 0 Then
                Try
                    With dbManager
                        .iniciarTransaccion()
                        .SqlParametros.Add("@idEnvio", SqlDbType.BigInt).Value = _idEnvio
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = IdUsuarioCarga
                        .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .llenarDataTable(dtError, "BuscarErroresNacionalizacion", CommandType.StoredProcedure)

                        resultado = CShort(.SqlParametros("@returnValue").Value)
                        If resultado = 0 Then
                            .confirmarTransaccion()
                        Else
                            If .estadoTransaccional Then .abortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    Throw New Exception("Error al tratar de validar la existencia de registro con error. " & ex.Message)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                Throw New Exception("No se pudo obtener la información para validar errores en los datos suministrados.")
            End If

            Return resultado
        End Function

        Public Function ActualizarNacionalizacionSeriales() As Short
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim resultado As Short = 0
            If IdEnvio <> 0 Then
                Try
                    With dbManager
                        .iniciarTransaccion()

                        With .SqlParametros
                            .Add("@idEnvio", SqlDbType.BigInt).Value = _idEnvio
                            .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                            .Add("@idUsuarioCierre", SqlDbType.BigInt).Value = _idUsuarioCierre
                            .Add("@nombreDocumentoImportacion", SqlDbType.VarChar, 250).Value = _nombreDocumentoImportacion
                            .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        End With
                        .ejecutarNonQuery("ActualizarOrdenEnvioNacionalizacion", CommandType.StoredProcedure)
                        resultado = CShort(.SqlParametros("@returnValue").Value)

                        If resultado = 0 Then
                            .SqlParametros.Clear()
                            .SqlParametros.Add("@idEnvio", SqlDbType.BigInt).Value = _idEnvio
                            .SqlParametros.Add("@idUsuario", SqlDbType.BigInt).Value = _idUsuarioCierre
                            .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                            .ejecutarNonQuery("ActualizarNacionalizacionSeriales", CommandType.StoredProcedure)

                            resultado = CShort(.SqlParametros("@returnValue").Value)
                            If resultado = 0 Then
                                .confirmarTransaccion()
                            End If
                        End If

                        If .estadoTransaccional Then .abortarTransaccion()
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                Throw New Exception("La Orden de envio de Nacionalizacion aún no ha sido registrada en la Base de Datos.")
            End If

            Return resultado
        End Function

#End Region

#Region "Metodos Privados"

        Private Sub CargarInformacion()
            Dim db As New LMDataAccess
            With db
                With .SqlParametros
                    .Add("@idEnvio", SqlDbType.BigInt).Value = _idEnvio
                End With

                Try
                    .ejecutarReader("ObtenerOrdenEnvioNacionalizacion", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            _numeroFactura = .Reader("factura").ToString
                            _numeroGuia = .Reader("guia").ToString
                            _fechaCreacion = .Reader("fechaCreacion")
                            _idFacturaGuia = .Reader("idFacturaGuia").ToString()
                            Integer.TryParse(.Reader("cantidad").ToString(), _cantidad)
                            If Not IsDBNull(.Reader("fechaCierre")) Then _fechaCreacion = .Reader("fechaCierre")
                        End If

                        If Not .Reader.IsClosed Then .Reader.Close()
                    End If
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
        End Sub

        Private Sub CargarListadoDetalle()

        End Sub

        Private Sub CargarListadoSerial()

        End Sub

        Private Sub EstablecerParametros(ByRef db As LMDataAccess)
            With db.SqlParametros
                .Add("@idFacturaGuia", SqlDbType.BigInt).Value = _idFacturaGuia
                .Add("@idCreador", SqlDbType.BigInt).Value = _idCreador
                .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                .Add("@observacion", SqlDbType.VarChar, 250).Value = _observacion
                .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
            End With
        End Sub

        ''' <summary>
        ''' Carga los datos de un datatable en la tabla ConsultaOrdenTrabajo.
        ''' </summary>
        ''' <param name="dtdatos">datatable con la informacion a cargar</param>
        ''' <remarks>Permite cargar los id de Orden para realizar el filtro</remarks>
        Private Sub CargarDatos(ByVal dtdatos As DataTable)
            Dim dbManager As New LMDataAccess
            With dbManager

                With .SqlParametros
                    .Add("@idUsuario", SqlDbType.Int).Value = _idCreador
                End With
                .ejecutarDataTable("EliminarConsultaOrdenTrabajo", CommandType.StoredProcedure)

                .inicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "ConsultaOrdenTrabajo"
                    .ColumnMappings.Add("idOrden", "idOrden")
                    .ColumnMappings.Add("idUsuario", "idUsuario")
                    .WriteToServer(dtdatos)
                End With
            End With
        End Sub

        Public Function ObtenerInfoNacionalizacionPedidoEnvioPrueba(ByVal idEnvio As Integer) As DataTable
            Dim dm As New LMDataAccess
            Dim dtDatos As New DataTable
            Dim resultado As Integer
            Try
                With dm
                    .SqlParametros.Add("@idEnvioNacionalizacion", SqlDbType.Int).Value = idEnvio
                    .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    dtDatos = .ejecutarDataTable("ObtenerNacionalizacionPedidoEnvioPrueba", CommandType.StoredProcedure)
                    resultado = .SqlParametros("@resultado").Value
                End With

                Return dtDatos
            Finally
                If dm IsNot Nothing Then dm.Dispose()
            End Try
            
        End Function

        Public Function NotificarNacionalizacion(ByVal dtDatos As DataTable) As ResultadoProceso
            Dim Notificacion As New AdministradorCorreo
            Dim DestinosPP As New MailAddressCollection
            Dim DestinosCC As New MailAddressCollection
            Dim respuestaEnvio As ResultadoProceso
            Dim sbContenido As New StringBuilder

            Try

                With sbContenido
                    .Append("Se notifica la nacionalizacion de los seriales correspondientes para los siguientes pedidos pendientes de despacho:")
                    .Append("<br/><table Style='border: 1px solid #C0C0C0; width: 400px;'>")
                    .Append("<tr Style='background-color: #9900CC; color: #FFFFFF'><td colspan='2'><b>Lista De Pedido:</b></td></tr>")
                    .Append("<tr Style='background-color: #9900CC; color: #FFFFFF'>")
                    .Append("<td><b>Pedido:</b></td><td><b>Número Nacionalización</b></td></tr>")
                    For Each dr As DataRow In dtDatos.Rows
                        .Append("<tr><td>" & dr("idPedido") & "</td>")
                        .Append("<td>" & dr("numeroNacionalizacion") & "</td></tr>")
                    Next
                    .Append("</table><br/><br/>")
                End With

                With Notificacion

                    CargarDestinatarios(AsuntoNotificacion.Tipo.NacionalizacionProducto, DestinosPP, DestinosCC)

                    .Titulo = "Nacionalización De Producto Para Pruebas"
                    .Asunto = "Notificación de nacionalización de producto de envio de prueba"
                    .TextoMensaje = sbContenido.ToString
                    .FirmaMensaje = "Logytech Mobile S.A.S <br />PBX. 57(1) 4395237 Ext 174 - 135"
                    .Receptor = DestinosPP
                    .Copia = DestinosCC
                    If Not .EnviarMail() Then
                        respuestaEnvio.Valor = 1
                        respuestaEnvio.Mensaje = "Ocurrió un error inesperado y no fué posible enviar la notificación"
                    End If
                End With

            Catch ex As Exception

            End Try
        End Function

        Private Function CargarDestinatarios(ByVal tipo As Comunes.AsuntoNotificacion.Tipo, ByVal destinoPP As MailAddressCollection, ByVal destinoCC As MailAddressCollection) As MailAddressCollection
            Dim ConfiguracionUsuario As New UsuarioNotificacion
            Dim filtro As New FiltroUsuarioNotificacion
            Dim dtDestinos As New DataTable
            Dim strDestinoPP = "", strDestinoCC As String = ""

            filtro.IdAsuntoNotificacion = tipo
            filtro.Separador = ","
            Try
                dtDestinos = ConfiguracionUsuario.ObtenerDestinatarioNotificacion(filtro)
                For Each fila As DataRow In dtDestinos.Rows
                    strDestinoPP += fila.Item("destinoPara")
                    strDestinoCC += fila.Item("destinoCopia")
                Next

                destinoPP.Add(strDestinoPP)
                destinoCC.Add(strDestinoCC)

            Finally
                If dtDestinos IsNot Nothing Then dtDestinos.Rows.Clear()
            End Try

        End Function

#End Region

#Region "Métodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroEnvio
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroEnvio) As DataTable
            Dim dtDatos As New DataTable
            Dim db As New LMDataAccess
            With db
                With .SqlParametros
                    If Not String.IsNullOrEmpty(filtro.Serial) Then .Add("@serial", SqlDbType.VarChar, 15).Value = filtro.Serial
                    If Not String.IsNullOrEmpty(filtro.CodigoOrden) Then .Add("@codigoOrden", SqlDbType.VarChar, 50).Value = filtro.CodigoOrden
                    If filtro.IdOrden <> 0 Then .Add("@idOrden", SqlDbType.BigInt).Value = filtro.IdOrden
                    If filtro.IdEnvio <> 0 Then .Add("@idEnvio", SqlDbType.BigInt).Value = filtro.IdEnvio
                    If filtro.IdFactura <> 0 Then .Add("@idFactura", SqlDbType.BigInt).Value = filtro.IdFactura
                    If filtro.IdGuia <> 0 Then .Add("@idGuia", SqlDbType.BigInt).Value = filtro.IdGuia
                End With
                Try
                    dtDatos = .ejecutarDataTable("ObtenerOrdenEnvioNacionalizacion", CommandType.StoredProcedure)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
            Return dtDatos
        End Function

        Public Shared Function ObtenerPorId(ByVal identificador As Long) As DataTable
            Dim filtro As New FiltroEnvio
            filtro.IdEnvio = identificador
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Shared Function ObtenerOrdenTrabajoEnvio(ByVal filtro As FiltroEnvio) As DataTable
            Dim dtDatos As New DataTable
            Dim db As New LMDataAccess
            With db
                With .SqlParametros
                    'If Not String.IsNullOrEmpty(filtro.Serial) Then .Add("@serial", SqlDbType.VarChar, 15).Value = filtro.Serial
                    'If Not String.IsNullOrEmpty(filtro.CodigoOrden) Then .Add("@codigoOrden", SqlDbType.VarChar, 50).Value = filtro.CodigoOrden
                    'If filtro.IdOrden <> 0 Then .Add("@idOrden", SqlDbType.BigInt).Value = filtro.IdOrden
                    'If filtro.IdEnvio <> 0 Then .Add("@idEnvio", SqlDbType.BigInt).Value = filtro.IdEnvio
                    If filtro.IncluirNoConformes = Enumerados.EstadoBinario.Activo Then _
                    .Add("@incluirNoConformes", SqlDbType.Bit).Value = filtro.IncluirNoConformes
                    If filtro.IdFactura <> 0 Then .Add("@idFactura", SqlDbType.BigInt).Value = filtro.IdFactura
                    If filtro.IdGuia <> 0 Then .Add("@idGuia", SqlDbType.BigInt).Value = filtro.IdGuia
                End With
                Try
                    dtDatos = .ejecutarDataTable("ObtenerOrdenTrabajoEnvio", CommandType.StoredProcedure)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
            Return dtDatos
        End Function

        Public Shared Function ObtenerRegionesEnvio(ByVal identificador As Long) As DataTable
            Dim dtDatos As New DataTable
            Dim db As New LMDataAccess
            With db
                If identificador <> 0 Then .SqlParametros.Add("@idEnvio", SqlDbType.BigInt).Value = identificador
                Try
                    dtDatos = .ejecutarDataTable("ObtenerRegionesEnvio", CommandType.StoredProcedure)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
            Return dtDatos
        End Function

        Public Shared Function ObtenerSerialesEnvio(ByVal identificador As Long) As DataTable
            Dim dtDatos As New DataTable
            Dim db As New LMDataAccess
            With db
                If identificador <> 0 Then .SqlParametros.Add("@idEnvio", SqlDbType.BigInt).Value = identificador
                Try
                    dtDatos = .ejecutarDataTable("ObtenerSerialesEnvio", CommandType.StoredProcedure)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
            Return dtDatos
        End Function

#End Region

#Region "Enums"

        Public Enum Estado
            EnviadoANacionalizacion = 24
            Nacionalizado
        End Enum

#End Region

    End Class
End Namespace