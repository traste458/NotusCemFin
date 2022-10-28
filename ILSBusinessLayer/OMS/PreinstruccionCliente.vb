Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer
Imports LMDataAccessLayer

Namespace OMS
    Public Class PreinstruccionCliente

#Region "Variables privadas"

        Private _idPreinstruccion As Integer
        Private _idDetalleOrdenCompra As Integer
        Private _detalleOrdenCompra As Recibos.DetalleOrdenCompra
        Private _idFactura As Integer
        Private _prioridad As Short
        Private _idEstado As Integer
        Private _cantidadInstruccionada As Integer
        Private _idUsuario As Integer
        Private _fechaRegistro As DateTime
        Private _error As String
        Private _porcentajeRegion As PreinstruccionPorcentajeRegion
        Private _porcentajeSubdistribucion As PreinstruccionPorcentajeSubdistribucion
        Private _porcentajeTipoInstruccion As PreinstruccionPorcentajeTipoInstruccion
        Private _cantidadDistribucion As PreinstruccionCantidadDistribucion
        Private _materiales As String
        Private _validarPorcentaje As Boolean
        Private _cambioInstruccion As Boolean
#End Region

#Region "Propiedades Publicas"

        Public Property ValidarPorcentaje() As Boolean
            Get
                Return _validarPorcentaje
            End Get
            Set(ByVal value As Boolean)
                _validarPorcentaje = value
            End Set
        End Property

        Public Property Materiales() As String
            Get
                Return _materiales
            End Get
            Set(ByVal value As String)
                _materiales = value
            End Set
        End Property

        Public ReadOnly Property CantidadDistribucion() As PreinstruccionCantidadDistribucion
            Get
                Return _cantidadDistribucion
            End Get
        End Property

        Public ReadOnly Property PorcentajeTipoInstruccion() As PreinstruccionPorcentajeTipoInstruccion
            Get
                Return _porcentajeTipoInstruccion
            End Get
        End Property

        Public ReadOnly Property PorcentajeRegion() As PreinstruccionPorcentajeRegion
            Get
                Return _porcentajeRegion
            End Get
        End Property

        Public ReadOnly Property PorcentajeSubdistribucion() As PreinstruccionPorcentajeSubdistribucion
            Get
                Return _porcentajeSubdistribucion
            End Get
        End Property

        Public Property IdPreinstruccion() As Integer
            Get
                Return _idPreinstruccion
            End Get
            Set(ByVal value As Integer)
                _idPreinstruccion = value
            End Set
        End Property

        Public Property IdDetalleOrdenCompra() As Integer
            Get
                Return _idDetalleOrdenCompra
            End Get
            Set(ByVal value As Integer)
                _idDetalleOrdenCompra = value
            End Set
        End Property

        Public ReadOnly Property DetalleOrdenCompra() As Recibos.DetalleOrdenCompra
            Get
                If Not _detalleOrdenCompra Is Nothing Then
                    Return _detalleOrdenCompra
                Else
                    If _idDetalleOrdenCompra > 0 Then
                        _detalleOrdenCompra = New Recibos.DetalleOrdenCompra(_idDetalleOrdenCompra)
                        Return _detalleOrdenCompra
                    Else
                        Return New Recibos.DetalleOrdenCompra()
                    End If
                End If
            End Get
        End Property

        Public Property IdFactura() As Integer
            Get
                Return _idFactura
            End Get
            Set(ByVal value As Integer)
                _idFactura = value
            End Set
        End Property

        Public Property Prioridad() As Short
            Get
                Return _prioridad
            End Get
            Set(ByVal value As Short)
                _prioridad = value
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

        Public Property CantidadInstruccionada() As Integer
            Get
                Return _cantidadInstruccionada
            End Get
            Set(ByVal value As Integer)
                _cantidadInstruccionada = value
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

        Public ReadOnly Property FechaRegistro() As DateTime
            Get
                Return _fechaRegistro
            End Get
        End Property

        Public Property ValidarPorcentajes() As Boolean
            Get
                Return _validarPorcentaje
            End Get
            Set(ByVal value As Boolean)
                _validarPorcentaje = value
            End Set
        End Property

        Public Property CambioInstruccion() As Boolean
            Get
                Return _cambioInstruccion
            End Get
            Set(ByVal value As Boolean)
                _cambioInstruccion = value
            End Set
        End Property

        Public ReadOnly Property InfoError() As String
            Get
                Return _error
            End Get
        End Property

#End Region

#Region "Estructuras"

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _porcentajeRegion = New PreinstruccionPorcentajeRegion
            _porcentajeSubdistribucion = New PreinstruccionPorcentajeSubdistribucion
            _porcentajeTipoInstruccion = New PreinstruccionPorcentajeTipoInstruccion
            _cantidadDistribucion = New PreinstruccionCantidadDistribucion
        End Sub

        Public Sub New(ByVal idPreinstruccion As Integer)
            Me.New()
            Me.CargarDatos(idPreinstruccion)
        End Sub

#End Region

#Region "Metodos Privados"

        Private Sub CargarDatos(ByVal idPreinstruccion As Integer)
            Dim db As New LMDataAccess()
            Try
                db.SqlParametros.Add("@idPreinstruccion", SqlDbType.Int).Value = idPreinstruccion
                db.ejecutarReader("ObtenerPreinstruccionCliente", CommandType.StoredProcedure)
                If db.Reader IsNot Nothing AndAlso db.Reader.Read Then
                    _idPreinstruccion = idPreinstruccion
                    _idDetalleOrdenCompra = CInt(db.Reader("idDetalleOrdenCompra"))
                    _idFactura = CInt(db.Reader("idFactura"))
                    _prioridad = CShort(db.Reader("prioridad"))
                    _idEstado = CInt(db.Reader("idEstado"))
                    _cantidadInstruccionada = CInt(db.Reader("cantidadInstruccionada"))
                    _idUsuario = CInt(db.Reader("idUsuario"))
                    _fechaRegistro = CDate(db.Reader("fechaRegistro"))
                End If
            Catch ex As Exception
                _error = "Error al cargar los datos. " & ex.Message
                Throw New Exception(ex.Message)
            Finally
                db.Dispose()
            End Try
        End Sub

#End Region

#Region "Metodos Publicos"

        Public Function Crear(ByVal dtErrores As DataTable, Optional ByVal db As LMDataAccess = Nothing)
            Dim retorno As Boolean
            If db Is Nothing Then db = New LMDataAccess
            db.SqlParametros.Clear()
            Try
                If _idEstado <> 74 Then
                    If _idDetalleOrdenCompra > 0 AndAlso _prioridad > 0 AndAlso _cantidadInstruccionada > 0 AndAlso _idUsuario > 0 Then
                        With db
                            With .SqlParametros
                                .Add("@idDetalleOrdenCompra", SqlDbType.Int).Value = _idDetalleOrdenCompra
                                .Add("@idFactura", SqlDbType.Int).Value = _idFactura
                                .Add("@prioridad", SqlDbType.Int).Value = _prioridad
                                .Add("@cantidadInstruccionada", SqlDbType.Int).Value = _cantidadInstruccionada
                                .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                                .Add("@validaPorcentaje", SqlDbType.Bit).Value = _validarPorcentaje
                                .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                                .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                            End With

                            Dim result As Integer
                            .ejecutarNonQuery("CrearPreinstruccionCliente", CommandType.StoredProcedure)
                            result = .SqlParametros("@result").Value
                            If result = 0 Then
                                _idPreinstruccion = CInt(.SqlParametros("@identity").Value)
                                Me.ProcesarDependencias(dtErrores, db)
                                retorno = True
                            Else
                                result = 1
                                _error = "Error al crear la instrucción."
                                Throw New Exception(_error)
                            End If
                        End With

                    Else
                        _error = "Datos incompletos para crear la instrucción."
                        Throw New Exception(_error)
                    End If
                End If
            Catch ex As Exception
                _error = "Error al crear la instrucción. " & ex.Message
                Throw New Exception(ex.Message)
            End Try
            Return retorno
        End Function

        Public Function Actualizar(Optional ByVal dtErrores As DataTable = Nothing, Optional ByVal db As LMDataAccess = Nothing)
            Dim retorno As Boolean
            If db Is Nothing Then db = New LMDataAccess
            db.SqlParametros.Clear()
            Try
                If _prioridad > 0 AndAlso _cantidadInstruccionada > 0 AndAlso _idUsuario > 0 Then
                    With db
                        With .SqlParametros
                            .Add("@idPreinstruccion", SqlDbType.Int).Value = _idPreinstruccion
                            .Add("@prioridad", SqlDbType.Int).Value = _prioridad
                            .Add("@cantidadInstruccionada", SqlDbType.Int).Value = _cantidadInstruccionada
                            .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                            .Add("@validaPorcentaje", SqlDbType.Bit).Value = _validarPorcentaje
                            If _idEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                            .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                        End With

                        Dim result As Integer
                        .ejecutarNonQuery("ActualizarPreinstruccionCliente", CommandType.StoredProcedure)
                        result = .SqlParametros("@result").Value
                        If result <> 0 Then
                            _error = "Error al crear la instrucción."
                            Throw New Exception(_error)
                        End If
                        If _idEstado <> 74 Then Me.ProcesarDependencias(dtErrores, db)
                    End With
                ElseIf _idEstado <> 74 Then
                    _error = "Datos incompletos para crear la instrucción."
                    Throw New Exception(_error)
                End If
            Catch ex As Exception
                _error = "Error al crear la instrucción. " & ex.Message
                Throw New Exception(ex.Message)
            End Try
            Return retorno
        End Function

        Public Function EliminarInstruccionesInternas(Optional ByVal dtErrores As DataTable = Nothing, Optional ByVal db As LMDataAccess = Nothing)
            Dim retorno As Boolean
            If db Is Nothing Then db = New LMDataAccess
            db.SqlParametros.Clear()
            db.iniciarTransaccion()
            Try
                If _prioridad > 0 AndAlso _cantidadInstruccionada > 0 AndAlso _idUsuario > 0 Then
                    With db
                        With .SqlParametros
                            .Add("@idPreinstruccion", SqlDbType.Int).Value = _idPreinstruccion
                            .Add("@prioridad", SqlDbType.Int).Value = _prioridad
                            .Add("@cantidadInstruccionada", SqlDbType.Int).Value = _cantidadInstruccionada
                            .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                            .Add("@validaPorcentaje", SqlDbType.Bit).Value = _validarPorcentaje
                            If _idEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                            .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                        End With
                        Dim result As Integer
                        .ejecutarNonQuery("EliminarPreinstruccionCliente", CommandType.StoredProcedure)
                        result = .SqlParametros("@result").Value
                        .confirmarTransaccion()
                        If result <> 0 Then
                            _error = "Error al crear la instrucción."

                            Throw New Exception(_error)
                        End If
                        If _idEstado <> 74 Then Me.ProcesarDependencias(dtErrores, db)
                    End With
                ElseIf _idEstado <> 74 Then
                    _error = "Datos incompletos para crear la instrucción."

                    Throw New Exception(_error)
                End If
            Catch ex As Exception
                db.abortarTransaccion()
                _error = "Error al crear la instrucción. " & ex.Message
                Throw New Exception(ex.Message)
            End Try
            Return retorno
        End Function

        Private Sub ProcesarDependencias(ByVal dtErrores As DataTable, Optional ByVal db As LMDataAccess = Nothing)
            _porcentajeRegion.IdDetalleOrdenCompra = _idDetalleOrdenCompra
            _porcentajeSubdistribucion.IdDetalleOrdenCompra = _idDetalleOrdenCompra
            _porcentajeTipoInstruccion.IdDetalleOrdenCompra = _idDetalleOrdenCompra
            _cantidadDistribucion.IdDetalleOrdenCompra = _idDetalleOrdenCompra

            _porcentajeRegion.IdPreinstruccion = _idPreinstruccion
            _porcentajeRegion.ValidarPorcentaje = _validarPorcentaje
            _porcentajeRegion.Procesar(db, dtErrores)

            _porcentajeSubdistribucion.IdPreinstruccion = _idPreinstruccion
            _porcentajeSubdistribucion.ValidarPorcentaje = _validarPorcentaje
            _porcentajeSubdistribucion.Procesar(db, dtErrores)

            _porcentajeTipoInstruccion.IdPreinstruccion = _idPreinstruccion
            _porcentajeTipoInstruccion.ValidarPorcentaje = _validarPorcentaje
            _porcentajeTipoInstruccion.Procesar(db, dtErrores)

            _cantidadDistribucion.IdPreinstruccion = _idPreinstruccion

            '**** Datos de la prealerta ****/
            Dim detalleOrdenCompraObj As New Recibos.DetalleOrdenCompra(_idDetalleOrdenCompra)
            Dim ordenCompraObj As New Recibos.OrdenCompra(detalleOrdenCompraObj.IdOrden)

            If _cantidadDistribucion.TotalInstruccionado > _cantidadInstruccionada Then
                Recibos.DetalleOrdenCompra.RegistrarError(dtErrores, ordenCompraObj.NumeroOrden, "las cantidades exceden el valor de la instrucción")
            Else
                _cantidadDistribucion.Procesar(db, dtErrores)
            End If

            If _materiales.Trim() <> "" Then
                Dim objMaterial As New MaterialPreinstruccionCliente
                _materiales = "'" + Join(_materiales.Split(","), "','") + "'"
                objMaterial.IdPreinstruccion = _idPreinstruccion
                objMaterial.Procesar(_materiales, db)
            Else
                Recibos.DetalleOrdenCompra.RegistrarError(dtErrores, ordenCompraObj.NumeroOrden, "la instrucción no tiene materiales asociados")
            End If

            If Not _validarPorcentaje Then
                db.SqlParametros.Clear()
                db.agregarParametroSQL("@idPreInstruccion", _idPreinstruccion, SqlDbType.Int)
                db.ejecutarNonQuery("CalcularPorcentajesPreinstruccionDesdeCantidad", CommandType.StoredProcedure)
            End If

        End Sub

        Public Shared Function ObtenerPropiedadesCampo(ByVal idCampo As String) As PropiedadesCampo
            Dim htPropiedades As New Hashtable
            Dim arrPorpiedes() As String = idCampo.Split("|")
            For Each propiedad As String In arrPorpiedes
                Dim valores() As String = propiedad.Split("_")
                htPropiedades.Add(valores(0), valores(1))
            Next
            Dim propiedades As New PropiedadesCampo
            With propiedades
                .id = idCampo
                .valor = htPropiedades("TextoCaja")
                .tipoCampo = htPropiedades("TipoCampo")
                .idRegion = htPropiedades("IdRegion")
                .idTipoInstruccion = htPropiedades("IdTipoInstruccion")
                .idSubInstruccion = htPropiedades("IdSubInstruccion")
            End With
            Return propiedades
        End Function

        Public Sub AsignarGuias(ByVal idPreinstrucciones As ArrayList)
            Dim db As New LMDataAccess
            Dim lista As String = Join(idPreinstrucciones.ToArray(), ",")

            db.agregarParametroSQL("@idPreinstrucciones", lista)
            db.agregarParametroSQL("@idUsuario", IdUsuario, SqlDbType.Int)
            db.SqlParametros.Add("@preinstruccionesSinAsignar", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
            Try
                db.iniciarTransaccion()
                db.ejecutarNonQuery("EjecutarAsignacionDeGuiasPreinstrucciones", CommandType.StoredProcedure)
                db.confirmarTransaccion()
                _error = db.SqlParametros("@preinstruccionesSinAsignar").Value.ToString()
            Catch ex As Exception
                If db.estadoTransaccional Then db.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                db.Dispose()
            End Try

        End Sub

        Public Structure PropiedadesCampo
            Dim id As String
            Dim valor As String
            Dim tipoCampo As Integer
            Dim idRegion As Integer
            Dim idTipoInstruccion As Integer
            Dim idSubInstruccion As Integer
        End Structure

        Public Shared Function ConsultarTipoSoftware() As DataTable
            Dim dtDatos As DataTable
            Dim dbMannager As New LMDataAccess

            Try
                With dbMannager
                    dtDatos = .ejecutarDataTable("ConsultaTipoSoftware", CommandType.StoredProcedure)
                End With
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                If dbMannager IsNot Nothing Then dbMannager.Dispose()
            End Try

            Return dtDatos
        End Function

        Public Shared Function ConsultarTipoAduanera() As DataTable
            Dim dtDatos As DataTable
            Dim dbMannager As New LMDataAccess

            Try
                With dbMannager
                    dtDatos = .ejecutarDataTable("ConsultaTipoIntermediariasAduaneras", CommandType.StoredProcedure)
                End With
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                If dbMannager IsNot Nothing Then dbMannager.Dispose()
            End Try

            Return dtDatos
        End Function

        Public Shared Function CargarTipoAduanera(ByVal dtDatos As DataTable, ByVal idUsuario As Integer) As Boolean
            Dim dbMannager As New LMDataAccess
            dtDatos.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), idUsuario))
            Try
                With dbMannager
                    .agregarParametroSQL("@idUsuario", idUsuario, SqlDbType.BigInt)
                    .ejecutarNonQuery("BorrarTablaTransitoriaIntermediariasAduaneras", CommandType.StoredProcedure)
                    .inicilizarBulkCopy()
                    .BulkCopy.DestinationTableName = "IntermediariasAduanerasTransitorias"
                    .BulkCopy.WriteToServer(dtDatos)
                End With
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                If dbMannager IsNot Nothing Then dbMannager.Dispose()
            End Try
        End Function

        Public Function IngresarTipoAduaneraFactura(ByRef dtError As DataTable, ByVal idUsuario As Integer) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess

            With dbManager
                .SqlParametros.Clear()
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                dtError = .ejecutarDataTable("IngresarTipoAduaneraFactura", CommandType.StoredProcedure)
                If dtError Is Nothing OrElse dtError.Rows.Count = 0 Then
                    resultado.EstablecerMensajeYValor(0, "Se realizo el registro del tipo de Intermediaria Aduanera correctamente.")
                Else
                    resultado.EstablecerMensajeYValor(2, "No se pudo realizar el registro de la información. Por favor verificar el Log de Resultados")
                End If
            End With
            Return resultado
        End Function

        Public Shared Function CargarTipoSoftware(ByVal dtDatos As DataTable, ByVal idUsuario As Integer) As Boolean
            Dim dbMannager As New LMDataAccess
            dtDatos.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), idUsuario))
            Try
                With dbMannager
                    .agregarParametroSQL("@idUsuario", idUsuario, SqlDbType.BigInt)
                    .ejecutarNonQuery("BorrarTablaTransitoriaSoftware", CommandType.StoredProcedure)
                    .inicilizarBulkCopy()
                    .BulkCopy.DestinationTableName = "SoftwareTransitorios"
                    .BulkCopy.WriteToServer(dtDatos)
                End With
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                If dbMannager IsNot Nothing Then dbMannager.Dispose()
            End Try
        End Function

        Public Function IngresarTipoSoftwareFactura(ByRef dtError As DataTable, ByVal idUsuario As Integer) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess

            With dbManager
                .SqlParametros.Clear()
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                dtError = .ejecutarDataTable("IngresarTipoSoftwareFactura", CommandType.StoredProcedure)
                If dtError Is Nothing OrElse dtError.Rows.Count = 0 Then
                    resultado.EstablecerMensajeYValor(0, "Se realizo el registro de tipo de Software correctamente. ")
                Else
                    resultado.EstablecerMensajeYValor(2, "No se pudo realizar el registro de la información. Por favor verificar el Log de Resultados")
                End If
            End With
            Return resultado
        End Function

#End Region

#Region "Metodos Compartidos"

        Public Shared Function ObtenerInfoDetalladaPreinstruccion(ByVal filtro As FiltroPreinstruccion) As DataSet
            Dim db As New LMDataAccess
            Dim dsDatos As New DataSet

            With filtro
                If .IdDetalleOrdenCompra > 0 Then db.SqlParametros.Add("@idDetalleOrdenCompra", SqlDbType.Int).Value = .IdDetalleOrdenCompra
                If Not String.IsNullOrEmpty(.NumeroOrdenCompra) Then _
                    db.SqlParametros.Add("@numeroOrdenCompra", SqlDbType.VarChar, 50).Value = .NumeroOrdenCompra.Trim
                If .IdFactura > 0 Then db.SqlParametros.Add("@idFactura", SqlDbType.Int).Value = .IdFactura
                If Not String.IsNullOrEmpty(.Factura) Then _
                    db.SqlParametros.Add("@factura", SqlDbType.VarChar, 50).Value = .Factura.Trim
                If .IdFabricante > 0 Then db.SqlParametros.Add("@idFabricante", SqlDbType.Int).Value = .IdFabricante
                If .IdProducto > 0 Then db.SqlParametros.Add("@idProducto", SqlDbType.Int).Value = .IdProducto
                If .FechaInicial > Date.MinValue And .FechaFinal > Date.MinValue Then
                    db.SqlParametros.Add("@fechaCreacionInicial", SqlDbType.SmallDateTime).Value = .FechaInicial
                    db.SqlParametros.Add("@fechaCreacionFinal", SqlDbType.SmallDateTime).Value = .FechaFinal
                End If
                If .IdEstado > 0 Then db.SqlParametros.Add("@idEstado", SqlDbType.Int).Value = .IdEstado
                If db.SqlParametros.Count = 0 Then db.SqlParametros.Add("@soloPrealertasPendientes", SqlDbType.Bit).Value = True
                db.TiempoEsperaComando = 600
                dsDatos = db.EjecutarDataSet("ObtenerInfoDetalladaPreinstruccion", CommandType.StoredProcedure)
                With dsDatos
                    If .Tables(0) IsNot Nothing Then .Tables(0).TableName = "dtPreinstruccion"
                    If .Tables(1) IsNot Nothing Then .Tables(1).TableName = "dtPorcentajes"
                    If .Tables(2) IsNot Nothing Then .Tables(2).TableName = "dtCantidades"
                End With
                Dim drPreinstruccionCantidad As New DataRelation("preinstruccionCantidades", dsDatos.Tables("dtPreinstruccion").Columns("idPreinstruccion"), dsDatos.Tables("dtCantidades").Columns("idPreinstruccion"))
                Dim drPreinstruccionPorcentaje As New DataRelation("preinstruccionPorcentajes", dsDatos.Tables("dtPreinstruccion").Columns("idPreinstruccion"), dsDatos.Tables("dtPorcentajes").Columns("idPreinstruccion"))
                Return dsDatos
            End With
            Return dsDatos
        End Function

        Public Shared Function ObtenerTodo(Optional ByVal idOrdenCompra As Integer = 0, _
                                           Optional ByVal idDetalleOrdenCompra As Integer = 0, _
                                           Optional ByVal idFactura As Integer = -1) As DataSet
            Try
                Dim ds As New DataSet("DatosPreinstruccion")
                Dim dtPreinstruccion As DataTable
                Dim dtPorcentajes As DataTable
                Dim dtCantidades As DataTable
                Dim filtro As New Estructuras.FiltroPreinstruccionCliente
                If idOrdenCompra > 0 Then filtro.IdOrdenCompra = idOrdenCompra
                If idDetalleOrdenCompra > 0 Then filtro.IdDetalleOrdenCompra = idDetalleOrdenCompra

                dtPreinstruccion = PreinstruccionCliente.ObtenerListado(filtro)
                dtPorcentajes = PreinstruccionCliente.ObtenerDatosPorcentaje(idOrdenCompra:=idOrdenCompra, idDetalleOrdenCompra:=idDetalleOrdenCompra)
                dtCantidades = PreinstruccionCliente.ObtenerDatosCantidades(idOrdenCompra:=idOrdenCompra, idDetalleOrdenCompra:=idDetalleOrdenCompra)

                dtPreinstruccion.TableName = "dtPreinstruccion"
                dtPorcentajes.TableName = "dtPorcentajes"
                dtCantidades.TableName = "dtCantidades"

                ds.Tables.Add(dtPreinstruccion)
                ds.Tables.Add(dtPorcentajes)
                ds.Tables.Add(dtCantidades)
                'Dim drPreinstruccionCantidad As New DataRelation("preinstruccionCantidades", dtPreinstruccion.Columns("idPreinstruccion"), dtCantidades.Columns("idPreinstruccion"))
                'Dim drPreinstruccionPorcentaje As New DataRelation("preinstruccionPorcentajes", dtPreinstruccion.Columns("idPreinstruccion"), dtPorcentajes.Columns("idPreinstruccion"))
                Return ds
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Function

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New Estructuras.FiltroPreinstruccionCliente
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As Estructuras.FiltroPreinstruccionCliente) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdPreinstruccion > 0 Then db.SqlParametros.Add("@idPreinstruccion", SqlDbType.Int).Value = .IdPreinstruccion
                If .IdDetalleOrdenCompra > 0 Then db.SqlParametros.Add("@idDetalleOrdenCompra", SqlDbType.Int).Value = .IdDetalleOrdenCompra
                If .IdFactura > 0 Then db.SqlParametros.Add("@idFactura", SqlDbType.Int).Value = .IdFactura
                If .Prioridad > 0 Then db.SqlParametros.Add("@prioridad", SqlDbType.Int).Value = .Prioridad
                If .IdEstado > 0 Then db.SqlParametros.Add("@idEstado", SqlDbType.Int).Value = .IdEstado
                If .CantidadInstruccionada > 0 Then db.SqlParametros.Add("@cantidadInstruccionada", SqlDbType.SmallInt).Value = .CantidadInstruccionada
                If .IdUsuario > 0 Then db.SqlParametros.Add("@idUsuario", SqlDbType.SmallInt).Value = .IdUsuario
                If .NoAnulada > 0 Then db.SqlParametros.Add("@noAnulada", SqlDbType.Int).Value = 1
                If db.SqlParametros.Count = 0 Then db.SqlParametros.Add("@soloPrealertasPendientes", SqlDbType.Bit).Value = True
                db.TiempoEsperaComando = 600
                dtDatos = db.ejecutarDataTable("ObtenerPreinstruccionCliente", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos
        End Function

        Public Shared Function ObtenerDatosPorcentaje(Optional ByVal idPreinstruccion As Integer = -1, _
                Optional ByVal idOrdenCompra As Integer = 0, Optional ByVal idDetalleOrdenCompra As Integer = 0) As DataTable
            Dim db As New LMDataAccess
            Dim dt As New DataTable
            If idPreinstruccion <> -1 Then db.SqlParametros.Add("@idPreInstruccion", SqlDbType.Int).Value = idPreinstruccion
            If idOrdenCompra > 0 Then db.SqlParametros.Add("@idOrdenCompra", SqlDbType.Int).Value = idOrdenCompra
            If idDetalleOrdenCompra > 0 Then db.SqlParametros.Add("@idDetalleOrdenCompra", SqlDbType.Int).Value = idDetalleOrdenCompra
            If db.SqlParametros.Count = 0 Then db.SqlParametros.Add("@soloPrealertasPendientes", SqlDbType.Bit).Value = True

            db.TiempoEsperaComando = 600
            dt = db.ejecutarDataTable("ObtenerCamposPorcentajePreinstruccion", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Shared Function ObtenerDatosCantidades(Optional ByVal idPreinstruccion As Integer = -1, _
                Optional ByVal idOrdenCompra As Integer = 0, Optional ByVal idDetalleOrdenCompra As Integer = 0) As DataTable
            Dim db As New LMDataAccess
            Dim dt As New DataTable
            If idPreinstruccion <> -1 Then db.SqlParametros.Add("@idPreInstruccion", SqlDbType.Int).Value = idPreinstruccion
            If idOrdenCompra > 0 Then db.SqlParametros.Add("@idOrdenCompra", SqlDbType.Int).Value = idOrdenCompra
            If idDetalleOrdenCompra > 0 Then db.SqlParametros.Add("@idDetalleOrdenCompra", SqlDbType.Int).Value = idDetalleOrdenCompra
            If db.SqlParametros.Count = 0 Then db.SqlParametros.Add("@soloPrealertasPendientes", SqlDbType.Bit).Value = True

            db.TiempoEsperaComando = 600
            dt = db.ejecutarDataTable("ObtenerCamposCantidadPreinstruccion", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Shared Function EstructuraAdicionarInstruccion() As DataTable
            Dim dtRetorno As New DataTable
            With dtRetorno
                .Columns.Add("idDetalleOrdenCompra", GetType(Integer))
                .Columns.Add("noInstruccion", GetType(Integer))
                .Columns.Add("cantidadAsignada", GetType(Integer))
            End With
            Dim primariColumns() As DataColumn = {dtRetorno.Columns("idDetalleOrdenCompra"), dtRetorno.Columns("noInstruccion")}
            dtRetorno.PrimaryKey = primariColumns
            Return dtRetorno
        End Function

        Public Shared Function AdicionarInstruccion(ByVal dtInstruccion As DataTable, ByVal idDetalleOrdenCompra As Integer, ByVal idFactura As Integer, ByVal cantidadAsignada As Integer, ByVal idUsuario As Integer) As Integer
            Try
                Dim valor As Integer
                Dim idPreinstruccionNueva As Integer
                'Dim noInstruccion As Integer                
                Integer.TryParse(dtInstruccion.Compute("MAX(prioridad)", "idEstado<>74").ToString, valor)
                Integer.TryParse(dtInstruccion.Compute("MAX(idTemp)", "").ToString, idPreinstruccionNueva)
                'If valor.ToString = "" Then
                '    noInstruccion = 1
                'Else
                '    Integer.TryParse(valor.ToString(), noInstruccion)
                '    noInstruccion += 1
                'End If
                Dim drAux As DataRow
                drAux = dtInstruccion.NewRow
                'drAux("noInstruccion") = noInstruccion
                drAux("idPreinstruccion") = 0
                drAux("idTemp") = idPreinstruccionNueva + 1
                drAux("idFactura") = idFactura
                drAux("idDetalleOrdenCompra") = idDetalleOrdenCompra
                drAux("materiales") = String.Empty
                drAux("prioridad") = valor + 1
                drAux("idEstado") = 73
                drAux("cantidadInstruccionada") = cantidadAsignada
                drAux("cantidadAsignada") = cantidadAsignada
                drAux("idUsuario") = idUsuario
                drAux("instruccionada") = 0
                drAux("totalLeida") = 0
                'drAux("validaPorcentaje") = 1


                dtInstruccion.Rows.Add(drAux)
                Return idPreinstruccionNueva + 1
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Function

        Public Shared Sub AdicionarPorcentajes(ByVal dtPorcentajes As DataTable, ByVal idPreinstruccionNueva As Integer)
            Try
                Dim dtPorcentajesNueva As DataTable
                dtPorcentajesNueva = OMS.PreinstruccionCliente.ObtenerDatosPorcentaje(0)
                Dim drAux As DataRow
                drAux = dtPorcentajes.NewRow
                For Each columna As DataColumn In dtPorcentajesNueva.Columns
                    drAux(columna.ColumnName) = dtPorcentajesNueva.Rows(0)(columna.ColumnName)
                Next
                drAux("idTemp") = idPreinstruccionNueva

                dtPorcentajes.Rows.Add(drAux)
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Sub

        Public Shared Sub AdicionarCantidades(ByVal dtCantidades As DataTable, ByVal idPreinstruccionNueva As Integer)
            Try
                Dim dtCantidadesNueva As DataTable
                dtCantidadesNueva = OMS.PreinstruccionCliente.ObtenerDatosCantidades(0)
                Dim drAux As DataRow
                drAux = dtCantidades.NewRow
                For Each columna As DataColumn In dtCantidadesNueva.Columns
                    drAux(columna.ColumnName) = dtCantidadesNueva.Rows(0)(columna.ColumnName)
                Next
                drAux("idTemp") = idPreinstruccionNueva

                dtCantidades.Rows.Add(drAux)
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Sub

        Public Shared Function ObtenerPreinstruccionesPorAsignacionDeGuia() As DataTable
            Dim db As New LMDataAccess
            Dim dt As DataTable = db.ejecutarDataTable("ObtenerPreinstruccionesPorAsignacionDeGuia", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Shared Function ObtenerInstrucciones(ByVal FiltroPreinstruccion As Estructuras.FiltroPreinstruccion) As DataTable
            Dim db As New LMDataAccess
            With FiltroPreinstruccion
                If .IdOrdenCompra > 0 Then db.SqlParametros.Add("@idOrdenCompra", SqlDbType.Int).Value = .IdOrdenCompra
                If .IdDetalleOrdenCompra > 0 Then db.SqlParametros.Add("@idDetalleOrdenCompra", SqlDbType.BigInt).Value = .IdDetalleOrdenCompra
                If .IdDetalleOrdenCompra > 0 And .IdFactura <> -1 Then db.SqlParametros.Add("@idFactura", SqlDbType.Int).Value = .IdFactura
                If .NumeroOrdenCompra <> String.Empty Then db.SqlParametros.Add("@numeroOrdenCompra", SqlDbType.VarChar).Value = .NumeroOrdenCompra
                If .Factura <> String.Empty Then db.SqlParametros.Add("@factura", SqlDbType.VarChar).Value = .Factura
                If .IdProducto > 0 Then db.SqlParametros.Add("@idProducto", SqlDbType.Int).Value = .IdProducto
                If .IdFabricante > 0 Then db.SqlParametros.Add("@idFabricante", SqlDbType.Int).Value = .IdFabricante
                If .FechaInicial <> Date.MinValue Then db.SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = .FechaInicial
                If .FechaFinal <> Date.MinValue Then db.SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = .FechaFinal
                If .IdEstado > 0 Then db.SqlParametros.Add("@idEstado", SqlDbType.Int).Value = .IdEstado
                If .MostrarPool <> Enumerados.EstadoBinario.NoEstablecido Then
                    db.SqlParametros.Add("@mostrarSoloPool", SqlDbType.Bit).Value = IIf(.MostrarPool = Enumerados.EstadoBinario.Activo, 1, 0)
                End If
            End With
            Dim dt As DataTable = db.ejecutarDataTable("ObtenerInstruccionClienteExterno", CommandType.StoredProcedure)
            Return dt
        End Function
#End Region

    End Class
End Namespace
