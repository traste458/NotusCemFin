Imports ILSBusinessLayer
Imports LMDataAccessLayer

Namespace Inventario

    Public Class BloqueoInventario

#Region "Atributos"

        Private _idBloqueo As Integer
        Private _idBodega As Integer
        Private _fechaRegistro As Date
        Private _idUsuario As Integer
		Private _usuario As String
        Private _idEstado As Integer
        Private _estado As String
        Private _fechaInicio As Date
        Private _fechaFin As Date
        Private _idUnidadNegocio As Short
        Private _idDestinatario As Integer
        Private _idTipoBloqueo As Short
        Private _observacion As String
        Private _documentoSAP As String
        Private _justificacion As String
        Private _idProcesoJustificacion As Integer
        Private _tipoServicioLista As New List(Of Enumerados.TipoServicio)
        Private _productoBloqueoColeccion As New DetalleProductoBloqueoColeccion
        Private _serialBloqueoColeccion As New DetalleSerialBloqueoColeccion

        Private _dtErrores As DataTable

        Private _accion As Enumerados.AccionItem
        Private _registrado As Boolean
#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idBodega As Integer, ByVal fechaRegistro As Date, ByVal idUsuario As Integer, ByVal idEstado As Integer, _
                       ByVal fechaInicio As Date, ByVal fechaFin As Date, ByVal idUnidadNegocio As Short, _
                       ByVal idDestinatario As Integer, ByVal idTipoBloqueo As Short, ByVal observacion As String)

            MyBase.New()
            _idBodega = idBodega
            _fechaRegistro = fechaRegistro
            _idUsuario = idUsuario
            _idEstado = idEstado
            _fechaInicio = fechaInicio
            _fechaFin = fechaFin
            _idUnidadNegocio = idUnidadNegocio
            _idDestinatario = idDestinatario
            _idTipoBloqueo = idTipoBloqueo
            _observacion = observacion

        End Sub

        Public Sub New(ByVal idBloqueo As Integer)
            MyBase.New()
            _idBloqueo = idBloqueo
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdBloqueo() As Integer
            Get
                Return _idBloqueo
            End Get
            Set(ByVal value As Integer)
                _idBloqueo = value
            End Set
        End Property

        Public Property IdBodega() As Integer
            Get
                Return _idBodega
            End Get
            Set(ByVal value As Integer)
                _idBodega = value
            End Set
        End Property

        Public Property FechaRegistro() As Date
            Get
                Return _fechaRegistro
            End Get
            Set(ByVal value As Date)
                _fechaRegistro = value
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

        Public Property Usuario As String
            Get
                Return _usuario
            End Get
            Set(value As String)
                _usuario = value
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

        Public Property Estado As String
            Get
                Return _estado
            End Get
            Set(value As String)
                _estado = value
            End Set
        End Property
        Public Property FechaInicio() As Date
            Get
                Return _fechaInicio
            End Get
            Set(ByVal value As Date)
                _fechaInicio = value
            End Set
        End Property

        Public Property FechaFin() As Date
            Get
                Return _fechaFin
            End Get
            Set(ByVal value As Date)
                _fechaFin = value
            End Set
        End Property

        Public Property IdUnidadNegocio() As Short
            Get
                Return _idUnidadNegocio
            End Get
            Set(ByVal value As Short)
                _idUnidadNegocio = value
            End Set
        End Property

        Public Property IdDestinatario() As Integer
            Get
                Return _idDestinatario
            End Get
            Set(ByVal value As Integer)
                _idDestinatario = value
            End Set
        End Property

        Public Property IdTipoBloqueo() As Short
            Get
                Return _idTipoBloqueo
            End Get
            Set(ByVal value As Short)
                _idTipoBloqueo = value
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

        Public Property DocumentoSAP As String
            Get
                Return _documentoSAP
            End Get
            Set(value As String)
                _documentoSAP = value
            End Set
        End Property

        Public Property Justificacion As String
            Get
                Return _justificacion
            End Get
            Set(value As String)
                _justificacion = value
            End Set
        End Property

        Public Property IdProcesoJustificacion As Integer
            Get
                Return _idProcesoJustificacion
            End Get
            Set(value As Integer)
                _idProcesoJustificacion = value
            End Set
        End Property
        Public Property TipoServicioLista() As List(Of Enumerados.TipoServicio)
            Get
                Return _tipoServicioLista
            End Get
            Set(ByVal value As List(Of Enumerados.TipoServicio))
                _tipoServicioLista = value
            End Set
        End Property

        Public Property ProductoBloqueoColeccion() As DetalleProductoBloqueoColeccion
            Get
                If _productoBloqueoColeccion.Count = 0 AndAlso _idBloqueo > 0 Then
                    Dim objProductos As New DetalleProductoBloqueoColeccion()
                    With objProductos
                        objProductos.IdBloqueo = New List(Of Integer) From {_idBloqueo}
                        .CargarDatos()
                    End With
                    _productoBloqueoColeccion = objProductos
                End If
                Return _productoBloqueoColeccion
            End Get
            Set(ByVal value As DetalleProductoBloqueoColeccion)
                _productoBloqueoColeccion = value
            End Set
        End Property

        Public Property SerialBloqueoColeccion() As DetalleSerialBloqueoColeccion
            Get
                Return _serialBloqueoColeccion
            End Get
            Set(ByVal value As DetalleSerialBloqueoColeccion)
                _serialBloqueoColeccion = value
            End Set
        End Property


        Public Property Accion() As Enumerados.AccionItem
            Get
                Return _accion
            End Get
            Set(ByVal value As Enumerados.AccionItem)
                _accion = value
            End Set
        End Property

        Public Property Errores As DataTable
            Get
                If _dtErrores Is Nothing Then EstructuraErrores()
                Return _dtErrores
            End Get
            Set(value As DataTable)
                _dtErrores = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            Dim dbManager As New LMDataAccess

            Try
                With dbManager
                    .SqlParametros.Add("@idBloqueo", SqlDbType.Int).Value = _idBloqueo
                    .ejecutarReader("ObtieneBloqueoInventario", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then

                            Integer.TryParse(.Reader("idBloqueo").ToString(), _idBloqueo)
                            Integer.TryParse(.Reader("idBodega").ToString(), _idBodega)
                            _fechaRegistro = CDate(.Reader("fechaRegistro"))
                            Integer.TryParse(.Reader("idUsuario").ToString(), _idUsuario)
							If Not IsDBNull(.Reader("usuario")) Then _usuario = (.Reader("usuario"))
                            Integer.TryParse(.Reader("idEstado").ToString(), _idEstado)
							If Not IsDBNull(.Reader("estado")) Then _estado = (.Reader("estado"))
                            _fechaInicio = CDate(.Reader("fechaInicio"))
                            If Not IsDBNull(.Reader("fechaFin")) Then _fechaFin = CDate(.Reader("fechaFin"))
                            Short.TryParse(.Reader("idUnidadNegocio").ToString(), _idUnidadNegocio)
                            Integer.TryParse(.Reader("idDestinatario").ToString(), _idDestinatario)
                            Short.TryParse(.Reader("idTipoBloqueo").ToString(), _idTipoBloqueo)
                            _observacion = .Reader("observacion").ToString()
							If Not IsDBNull(.Reader("documentoSAP")) Then _documentoSAP = (.Reader("documentoSAP"))

                            _registrado = True
                        End If
                    End If
                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
        Private Sub EstructuraErrores()
            Try
                _dtErrores = New DataTable()
                With _dtErrores
                    .Columns.Add(New DataColumn("id", GetType(Long)))
                    .Columns.Add(New DataColumn("mensaje", GetType(String)))
                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Private Sub AdicionarError(id As Integer, mensaje As String)
            Try
                Dim drFila As DataRow = Errores.NewRow()
                With drFila
                    .Item("id") = id
                    .Item("mensaje") = mensaje
                End With
                Errores.Rows.Add(drFila)
                Errores.AcceptChanges()
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso

            If (_idBodega > 0 AndAlso _
                _idUsuario > 0 AndAlso _
                _idEstado > 0 AndAlso _
                Not _fechaInicio.Equals(Date.MinValue) AndAlso _
                _idUnidadNegocio > 0 AndAlso _
                _idTipoBloqueo > 0 AndAlso _
                Not String.IsNullOrEmpty(_observacion)) Then 

                Dim dbManager As New LMDataAccess

                With dbManager
                    Try
                        'Realiza las validaciones de disponibilidad en tablas Transitorias
                        If ProductoBloqueoColeccion.Count > 0 Then
                            .SqlParametros.Clear()
                            .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                            .ejecutarNonQuery("LiberaDatosTransitoriosBloqueoInventario", CommandType.StoredProcedure)

                            Dim productoBloqueoDataTable As DataTable = ProductoBloqueoColeccion.GenerarDataTable()
                            With productoBloqueoDataTable
                                .Columns.Add(New DataColumn("idUsuario", GetType(Integer), _idUsuario))
                                .Columns.Add(New DataColumn("idBodega", GetType(Integer), _idBodega))
                                .AcceptChanges()
                            End With

                            .inicilizarBulkCopy()
                            With .BulkCopy
                                .DestinationTableName = "TransitoriaBloqueoInventarioDetalleProducto"
                                .ColumnMappings.Add("idUsuario", "idUsuario")
                                .ColumnMappings.Add("idBodega", "idBodega")
                                .ColumnMappings.Add("idProducto", "idProducto")
                                .ColumnMappings.Add("material", "material")
                                .ColumnMappings.Add("cantidad", "cantidad")
                                .WriteToServer(productoBloqueoDataTable)
                            End With
                        End If

                        If SerialBloqueoColeccion.Count > 0 Then
                            .SqlParametros.Clear()
                            .SqlParametros.Add("@idUdsuario", SqlDbType.Int).Value = _idUsuario
                            .ejecutarNonQuery("LiberaDatosTransitoriosBloqueoInventario", CommandType.StoredProcedure)

                            Dim serialBloqueoDataTable As DataTable = SerialBloqueoColeccion.GenerarDataTable()
                            With serialBloqueoDataTable
                                .Columns.Add(New DataColumn("idUsuario", GetType(Integer), _idUsuario))
                                .Columns.Add(New DataColumn("idBodega", GetType(Integer), _idBodega))
                                .AcceptChanges()
                            End With

                            .inicilizarBulkCopy()
                            With .BulkCopy
                                .DestinationTableName = "TransitoriaBloqueoInventarioDetalleSerial"
                                .ColumnMappings.Add("idUsuario", "idUsuario")
                                .ColumnMappings.Add("idBodega", "idBodega")
                                .ColumnMappings.Add("serial", "serial")
                                .WriteToServer(serialBloqueoDataTable)
                            End With
                        End If

                        With .SqlParametros
                            .Clear()
                            .Add("@idBodega", SqlDbType.Int).Value = _idBodega
                            If Not _fechaRegistro.Equals(Date.MinValue) Then .Add("@fechaRegistro", SqlDbType.DateTime).Value = _fechaRegistro
                            .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                            .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                            .Add("@fechaInicio", SqlDbType.DateTime).Value = _fechaInicio
                            If Not _fechaFin.Equals(Date.MinValue) Then .Add("@fechaFin", SqlDbType.DateTime).Value = _fechaFin
                            .Add("@idUnidadNegocio", SqlDbType.SmallInt).Value = IdUnidadNegocio
                            If _idDestinatario > 0 Then _
                                .Add("@idDestinatario", SqlDbType.SmallInt).Value = IdDestinatario
                            .Add("@idTipoBloqueo", SqlDbType.SmallInt).Value = _idTipoBloqueo
                            .Add("@observacion", SqlDbType.VarChar, 255).Value = _observacion
							If Not String.IsNullOrEmpty(_documentoSAP) Then .Add("@documentoSAP", SqlDbType.VarChar, 30).Value = _documentoSAP

                            .Add("@idBloqueo", SqlDbType.Int).Direction = ParameterDirection.Output
                        End With

                        .iniciarTransaccion()

                        'Si existe el bloqueo, se adicionan los detalles solamente
                        If _idBloqueo = 0 Then
                            .ejecutarScalar("RegistraBloqueoInventario", CommandType.StoredProcedure)
                            Integer.TryParse(.SqlParametros("@idBloqueo").Value.ToString(), _idBloqueo)
                        End If

                        If _idBloqueo <> 0 Then
                            If ProductoBloqueoColeccion.Count > 0 Then
                                'Se realizá la validación de disponibilidad en inventario sobre los datos transitorios.
                                Dim resultadoComprobacion As Integer

                                .SqlParametros.Clear()
                                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                                .SqlParametros.Add("@idTipo", SqlDbType.SmallInt).Value = Enumerados.TipoValidacionBloqueo.Producto
                                .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                                .ejecutarReader("ComprobarDisponibilidadInventario", CommandType.StoredProcedure)
                                If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                                    While .Reader.Read()
                                        AdicionarError(CInt(.Reader("id")), .Reader("mensaje").ToString())
                                    End While
                                End If
                                If Not .Reader.IsClosed Then .Reader.Close()

                                resultadoComprobacion = CInt(.SqlParametros("@resultado").Value)
                                If resultadoComprobacion = 0 Then
                                    .SqlParametros.Clear()
                                    .SqlParametros.Add("@idBloqueo", SqlDbType.Int).Value = _idBloqueo
                                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                                    .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.InputOutput

                                    .ejecutarNonQuery("RegistraBloqueoInventarioProducto", CommandType.StoredProcedure)

                                    Dim resultadoBloqueoProducto As Short = CShort(.SqlParametros("@resultado").Value)
                                    If resultadoBloqueoProducto <> 0 Then
                                        resultado.EstablecerMensajeYValor(1, "Error inesperado al intentar registrar el bloqueo para Producto.")
                                        .abortarTransaccion()
                                    End If
                                Else
                                    Select Case resultadoComprobacion
                                        Case 1 : resultado.EstablecerMensajeYValor(1, "Cantidad de inventario es inferior a la solicitada por el bloqueo.")
                                        Case Else : resultado.EstablecerMensajeYValor(9, "Error inesperado al intentar comprobar disponibilidad para Producto.")
                                    End Select
                                    .abortarTransaccion()
                                End If
                            End If

                            If SerialBloqueoColeccion.Count > 0 Then
                                'Se realizá la validación de disponibilidad en inventario sobre los datos transitorios.
                                Dim resultadoComprobacion As Integer

                                .SqlParametros.Clear()
                                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                                .SqlParametros.Add("@idTipo", SqlDbType.SmallInt).Value = Enumerados.TipoValidacionBloqueo.Serial
                                .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                                .ejecutarNonQuery("ComprobarDisponibilidadInventario", CommandType.StoredProcedure)
                                resultadoComprobacion = CInt(.SqlParametros("@resultado").Value)

                                If resultadoComprobacion = 0 Then

                                    .SqlParametros.Clear()
                                    .SqlParametros.Add("@idBloqueo", SqlDbType.Int).Value = _idBloqueo
                                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                                    .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.InputOutput

                                    .ejecutarNonQuery("RegistraBloqueoInventarioSerial", CommandType.StoredProcedure)

                                    Dim resultadoBloqueoSerial As Short = CShort(.SqlParametros("@resultado").Value)
                                    If resultadoBloqueoSerial <> 0 Then
                                        resultado.EstablecerMensajeYValor(1, "Error inesperado al intentar comprobar disponibilidad para Serial.")
                                        .abortarTransaccion()
                                    End If
                                Else
                                    Select Case resultadoComprobacion
                                        Case 1 : resultado.EstablecerMensajeYValor(1, "Existen elementos que ya se encuentran bloqueados o no está disponibles.")
                                        Case Else : resultado.EstablecerMensajeYValor(8, "Error inesperado al intentar comprobar disponibilidad para Serial.")
                                    End Select
                                    .abortarTransaccion()
                                End If
                            End If

                            If .estadoTransaccional Then
                                .confirmarTransaccion()
                                resultado.EstablecerMensajeYValor(0, "Se realizó el bloqueo correctamente.")
                            End If
                        Else
                            resultado.EstablecerMensajeYValor(9, "Se generó un error al tratar de generar el bloqueo de inventario.")
                            .abortarTransaccion()
                        End If
                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                        Throw New Exception(ex.Message, ex)
                    End Try

                End With
                dbManager.Dispose()
            Else
                resultado.EstablecerMensajeYValor(10, "No se han proporcionado todos los valores requeridos para poder realizar el registro.")
            End If

            Return resultado
        End Function

        Public Function Actualizar() As ResultadoProceso
            Dim resultado As New ResultadoProceso

            If (_idBloqueo > 0) Then

                Dim dbManager As New LMDataAccess
                With dbManager
                    Try
                        With .SqlParametros
                            .Add("@idBloqueo", SqlDbType.Int).Value = _idBloqueo
                            If _idBodega > 0 Then .Add("@idBodega", SqlDbType.Int).Value = _idBodega
                            If Not _fechaRegistro.Equals(Date.MinValue) Then .Add("@fechaRegistro", SqlDbType.DateTime).Value = _fechaRegistro
                            If _idUsuario > 0 Then .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                            If _idEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                            If Not _fechaInicio.Equals(Date.MinValue) Then .Add("@fechaInicio", SqlDbType.DateTime).Value = _fechaInicio
                            If Not _fechaFin.Equals(Date.MinValue) Then .Add("@fechaFin", SqlDbType.DateTime).Value = _fechaFin
                            If _idUnidadNegocio > 0 Then .Add("@idUnidadNegocio", SqlDbType.SmallInt).Value = _idUnidadNegocio
                            If _idDestinatario > 0 Then .Add("@idDestinatario", SqlDbType.Int).Value = _idDestinatario
                            If _idTipoBloqueo > 0 Then .Add("@idTipoBloqueo", SqlDbType.SmallInt).Value = _idTipoBloqueo
                            If Not String.IsNullOrEmpty(_observacion) Then .Add("@observacion", SqlDbType.VarChar).Value = _observacion
                            If Not String.IsNullOrEmpty(_justificacion) Then .Add("@justificacion", SqlDbType.VarChar, 450).Value = _justificacion
                            If _idProcesoJustificacion > 0 Then .Add("@idProcesoJustificacion", SqlDbType.Int).Value = _idProcesoJustificacion
                            .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                        End With

                        .iniciarTransaccion()
                        .ejecutarScalar("ActualizaBloqueoInventario", CommandType.StoredProcedure)
                        .confirmarTransaccion()

                        If CInt(.SqlParametros("@resultado").Value) = 0 Then
                            resultado.EstablecerMensajeYValor(0, "Se realizó la actualización del bloqueo correctamente.")
                        Else
                            resultado.EstablecerMensajeYValor(1, "No se realizó la actualización de ningún registro.")
                        End If

                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                        Throw New Exception(ex.Message, ex)
                    End Try
                End With
                dbManager.Dispose()

            Else
                resultado.EstablecerMensajeYValor(10, "No se han proporcionado todos los valores requeridos para poder realizar la actualización.")
            End If

            Return resultado
        End Function

        Public Function DesbloquearProducto(ByVal ProductoBloqueoColeccion As DetalleProductoBloqueoColeccion) As ResultadoProceso
            Dim resultado As New ResultadoProceso

            Dim idBloqueo As New List(Of Integer)
            Dim idProducto As New List(Of Integer)

            If ProductoBloqueoColeccion.Count > 0 Then
                For Each itemProducto As DetalleProductoBloqueo In ProductoBloqueoColeccion
                    If Not idBloqueo.Contains(itemProducto.IdBloqueo) Then idBloqueo.Add(itemProducto.IdBloqueo)
                    idProducto.Add(itemProducto.IdProducto)
                Next

                Dim dbManager As New LMDataAccess
                With dbManager
					.TiempoEsperaComando = 600
                    Try
                        .SqlParametros.Add("@listaIdBloqueo", SqlDbType.VarChar).Value = String.Join(",", idBloqueo.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                        .SqlParametros.Add("@listaIdProducto", SqlDbType.VarChar).Value = String.Join(",", idProducto.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())

						
                        .iniciarTransaccion()
                        .ejecutarNonQuery("EliminaBloqueoInventario", CommandType.StoredProcedure)
                        
						.confirmarTransaccion()
                        resultado.EstablecerMensajeYValor(0, "Se realizó el desbloqueo de producto correctamente.")
                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                        Throw New Exception(ex.Message, ex)
                    End Try
                End With
                dbManager.Dispose()
            Else
                resultado.EstablecerMensajeYValor(1, "La colección de productos a desbloquear no tiene elementos.")
            End If

            Return resultado
        End Function

        Public Function DesbloquearSerial(ByVal SerialBloqueoColeccion As DetalleSerialBloqueoColeccion) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim idSerial As New List(Of String)

            If SerialBloqueoColeccion.Count > 0 Then
                For Each itemSerial As DetalleSerialBloqueo In SerialBloqueoColeccion
                    idSerial.Add(itemSerial.Serial)
                Next

                'Using dbManager As New LMDataAccess
                Dim dbManager As New LMDataAccess
                With dbManager
                    Try
                        .SqlParametros.Add("@listaSerial", SqlDbType.VarChar).Value = String.Join(",", idSerial.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())

                        .iniciarTransaccion()
                        .ejecutarNonQuery("EliminaBloqueoInventario", CommandType.StoredProcedure)
                        .confirmarTransaccion()

                        resultado.EstablecerMensajeYValor(0, "Se realizó el desbloqueo de serial correctamente.")
                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                        Throw New Exception(ex.Message, ex)
                    End Try
                End With
                dbManager.Dispose()
                'End Using

            Else
                resultado.EstablecerMensajeYValor(1, "La colección de seriales a desbloquear no tiene elementos.")
            End If

            Return resultado
        End Function

        Public Function BloqueoSerialPapeleria(ByVal listserial As List(Of String), material As String) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Dim noResultado As Integer = 0
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                        .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        .Add("@idTipoBloqueo", SqlDbType.Int).Value = _idTipoBloqueo
                        .Add("@material", SqlDbType.VarChar, 50).Value = material
                        If Not String.IsNullOrEmpty(_observacion) Then .Add("@observacion", SqlDbType.VarChar, 250).Value = _observacion
                        If Not String.IsNullOrEmpty(_documentoSAP) Then .Add("@documentoSap", SqlDbType.VarChar, 250).Value = _documentoSAP
                        If Not listserial Is Nothing AndAlso listserial.Count > 0 Then _
                        .Add("@listserial", SqlDbType.VarChar).Value = String.Join(",", listserial.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .iniciarTransaccion()
                    .ejecutarReader("RegistrarBloqueoPapeleriaSerial", CommandType.StoredProcedure)

                    If .Reader.HasRows Then noResultado = 1

                    If noResultado = 0 Then
                        If Not .Reader.IsClosed Then .Reader.Close()
                        .confirmarTransaccion()
                        resultado.EstablecerMensajeYValor(noResultado, "Bloqueo generado satisfactoriamente.")
                    
                    Else
                        resultado.EstablecerMensajeYValor(noResultado, "Se genero un error al intentar crear el bloqueo.")
                        If .Reader IsNot Nothing Then
                            If .Reader.HasRows Then
                                While .Reader.Read()
                                    Dim filaError As DataRow = Errores.NewRow()
                                    filaError("id") = .Reader("id")
                                    filaError("mensaje") = .Reader("mensaje")
                                    Errores.Rows.Add(filaError)
                                End While
                                Errores.AcceptChanges()
                            End If
                        End If
                        .Reader.Close()
                        .abortarTransaccion()
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
            End Try
            Return resultado
        End Function

        Public Function BloqueoSerialMaterial(ByVal listMaterial As List(Of String), ByVal material As String) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                        .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        .Add("@idTipoBloqueo", SqlDbType.Int).Value = _idTipoBloqueo
                        .Add("@material", SqlDbType.VarChar, 20).Value = material
                        If Not String.IsNullOrEmpty(_observacion) Then .Add("@observacion", SqlDbType.VarChar, 250).Value = _observacion
                        If Not String.IsNullOrEmpty(_documentoSAP) Then .Add("@documentoSap", SqlDbType.VarChar, 250).Value = _documentoSAP
                        If Not listMaterial Is Nothing AndAlso listMaterial.Count > 0 Then _
                        .Add("@listMaterial", SqlDbType.VarChar).Value = String.Join(",", listMaterial.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .iniciarTransaccion()
                    .ejecutarNonQuery("RegistrarBloqueoPapeleriaMaterial", CommandType.StoredProcedure)

                    If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        .confirmarTransaccion()
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                    Else
                        .abortarTransaccion()
                        resultado.EstablecerMensajeYValor(400, "No se logró establecer respuesta del servidor, por favor inténtelo nuevamente.")
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
            End Try
            Return resultado
        End Function
#End Region

    End Class

End Namespace

