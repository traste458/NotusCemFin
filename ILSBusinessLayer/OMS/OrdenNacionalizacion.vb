Imports ILSBusinessLayer.Estructuras
Imports LMDataAccessLayer

Namespace OMS

    Public Class OrdenNacionalizacion

#Region "Atributos"
        Private _idOrden As Long
        Private _idCreador As Long
        Private _creador As String
        Private _idEstado As Integer
        Private _fechaCreacion As Date
        Private _infoSerial As DataTable
        Private _dtInfoCarga As DataTable
        Private _cantidad As Integer
        Private _nombreArchivoCarga As String
        Private _nombreRutaArchivo As String
        Private _serialesOrden As DataTable
        Private _pedido As Long
        Private _entrega As Long
#End Region

#Region "Propiedades"
        Public ReadOnly Property IdOrden() As Long
            Get
                Return _idOrden
            End Get
        End Property

        Public Property IdCreador() As Long
            Get
                Return _idCreador
            End Get
            Set(ByVal value As Long)
                _idCreador = value
            End Set
        End Property

        Public Property Creador() As String
            Get
                Return _creador
            End Get
            Set(ByVal value As String)
                _creador = value
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

        Public ReadOnly Property InfoSerial() As DataTable
            Get
                If _infoSerial Is Nothing Then CargarListadoSerial()
                Return _infoSerial
            End Get
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

        Public Property NombreArchivo() As String
            Get
                Return _nombreArchivoCarga
            End Get
            Set(ByVal value As String)
                _nombreArchivoCarga = value
            End Set
        End Property

        Public Property NombreRutaArchivo() As String
            Get
                Return _nombreRutaArchivo
            End Get
            Set(ByVal value As String)
                _nombreRutaArchivo = value
            End Set
        End Property

        Public Property Pedido() As Long
            Get
                Return _pedido
            End Get
            Set(ByVal value As Long)
                _pedido = value
            End Set
        End Property

        Public Property Entrega() As Long
            Get
                Return _entrega
            End Get
            Set(ByVal value As Long)
                _entrega = value
            End Set
        End Property
#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            _idOrden = identificador
            CargarInformacion()
        End Sub

#End Region

#Region "Metodos Publicos"

        Public Function Crear() As Short
            Dim idsOrdenes As String = String.Empty
            Dim db As New LMDataAccess
            Dim resultado As Short = 0

            With db
                EstablecerParametros(db)
                .TiempoEsperaComando = 600
                Try
                    .iniciarTransaccion()
                    .ejecutarNonQuery("CrearOrdenNacionalizacion", CommandType.StoredProcedure)

                    Short.TryParse(.SqlParametros("@result").Value, resultado)

                    If resultado = 0 Then
                        _idOrden = CLng(.SqlParametros("@idOrden").Value)
                        .confirmarTransaccion()
                    Else
                        If .estadoTransaccional Then .abortarTransaccion()
                    End If
                Catch ex As Exception
                    If .estadoTransaccional Then .abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With

            Return resultado
        End Function

        Public Function BorrarSerialNacionalizacionTemporal() As Short
            Dim dbManager As New LMDataAccess
            Dim resultado As Short

            If _idCreador > 0 Then

                Try
                    With dbManager
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idCreador
                        .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                        .TiempoEsperaComando = 120
                        .iniciarTransaccion()

                        .ejecutarNonQuery("BorrarSerialNacionalizacionTemporal", CommandType.StoredProcedure)

                        Short.TryParse(.SqlParametros("@returnValue").Value, resultado)

                        If resultado = 0 Then .confirmarTransaccion()

                        If .estadoTransaccional Then dbManager.abortarTransaccion()
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
                        .TiempoEsperaComando = 120
                        .iniciarTransaccion()
                        .inicilizarBulkCopy()
                        With .BulkCopy
                            .DestinationTableName = "SerialNacionalizacionTemporal"
                            .ColumnMappings.Add("serial", "serial")
                            .ColumnMappings.Add("numeroNacionalizacion", "numeroNacionalizacion")
                            .ColumnMappings.Add("numlinea", "numlinea")
                            .ColumnMappings.Add("idUsuario", "idUsuario")
                            .WriteToServer(_dtInfoCarga)
                        End With
                        .confirmarTransaccion()

                        If .estadoTransaccional Then .abortarTransaccion()
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
            Try
                With dbManager
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idCreador
                    .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                    .TiempoEsperaComando = 180
                    .iniciarTransaccion()
                    .llenarDataTable(dtError, "BuscarErroresSerialNacionalizacion", CommandType.StoredProcedure)

                    resultado = CShort(.SqlParametros("@returnValue").Value)

                    If resultado = 0 Then .confirmarTransaccion()

                    If .estadoTransaccional Then .abortarTransaccion()
                End With
            Catch ex As Exception
                Throw New Exception("Error al tratar de validar los errores de nacionalización. " & ex.Message)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return resultado
        End Function


        Public Function BuscarSerialesPrueba(ByRef dtSerialesPrueba As DataTable) As ResultadoProceso
            Dim dbManager As New LMDataAccess
            Dim rp As New ResultadoProceso

            If _idCreador > 0 Then
                Try
                    With dbManager
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idCreador
                        .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                        .iniciarTransaccion()
                        .llenarDataTable(dtSerialesPrueba, "BuscarSerialesPrueba", CommandType.StoredProcedure)

                        rp.EstablecerMensajeYValor(CShort(.SqlParametros("@returnValue").Value), "Resultado Ejecución")
                    End With
                Catch ex As Exception
                    Throw New Exception("Error al tratar de validar la existencia de seriales de prueba. " & ex.Message)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                Throw New Exception("No se suministro la información completa para consultar los seriales de prueba.")
            End If

            Return rp
        End Function

        Public Function Actualizar() As Short
            Dim resultado As Integer

            If _idOrden > 0 Then
                Dim db As New LMDataAccessLayer.LMDataAccess

                Try
                    With db

                        With .SqlParametros
                            .Add("@idOrden", SqlDbType.BigInt).Value = Me._idOrden
                            .Add("@entrega", SqlDbType.BigInt).Value = _entrega
                            .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        End With
                        .TiempoEsperaComando = 1200
                        .iniciarTransaccion()

                        .ejecutarNonQuery("ActualizarOrdenNacionalizacion", CommandType.StoredProcedure)

                        resultado = CShort(.SqlParametros("@returnValue").Value)

                        If resultado = 0 Then
                            .confirmarTransaccion()
                        Else
                            If .estadoTransaccional Then .abortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    If db.estadoTransaccional Then db.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            Else
                resultado = 8 ' No se han suministrado los datos completos 
            End If

            Return resultado

        End Function

        Public Function ExisteOrdenSinNacionalizacion() As Boolean
            Dim db As New LMDataAccess
            Dim resultado As Boolean = False

            With db
                With .SqlParametros
                    .Add("@result", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue
                End With

                Try
                    .ejecutarNonQuery("ExisteOrdenSinNacionalizacion", CommandType.StoredProcedure)
                    resultado = CType(.SqlParametros("@result").Value, Boolean)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With

            Return resultado
        End Function

        Public Function ObtenerInfoDetalleOrden() As DataTable

            Dim dt As New DataTable()
            Dim db As New LMDataAccessLayer.LMDataAccess

            If _idOrden > 0 Then

                Try
                    With db
                        With .SqlParametros
                            .Add("@idOrden", SqlDbType.BigInt).Value = _idOrden
                        End With

                        dt = .ejecutarDataTable("ObtenerDetalleOrdenNacionalizacion", CommandType.StoredProcedure)
                    End With
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End If

            Return dt
        End Function

        Public Function ActualizarEntregaSeriales() As Short
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim resultado As Short

            If _idOrden > 0 AndAlso _infoSerial IsNot Nothing AndAlso _infoSerial.Rows.Count > 0 Then
                Try
                    With dbManager
                        .TiempoEsperaComando = 1200
                        .iniciarTransaccion()

                        .SqlParametros.Add("@idOrden", SqlDbType.SmallInt).Value = _idOrden
                        .SqlParametros.Add("@pedido", SqlDbType.BigInt).Value = _pedido
                        .SqlParametros.Add("@serial", SqlDbType.VarChar, 20)
                        .SqlParametros.Add("@entrega", SqlDbType.BigInt)
                        .SqlParametros.Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue

                        'Se agrega los parametros para actualizacion de seriales
                        Dim vista As New DataView(_infoSerial)

                        vista.RowStateFilter = DataViewRowState.ModifiedCurrent
                        For Each fila As DataRowView In vista
                            .SqlParametros("@serial").Value = fila("serial")
                            .SqlParametros("@entrega").Value = fila("entrega")
                            .ejecutarNonQuery("ActualizarSerialEntrega", CommandType.StoredProcedure)

                            Short.TryParse(.SqlParametros("@result").Value, resultado)
                            If resultado <> 0 Then Exit For
                        Next

                        If resultado = 0 Then
                            .confirmarTransaccion()
                        End If

                        If .estadoTransaccional Then .abortarTransaccion()
                    End With
                Catch ex As Exception
                    Throw New Exception(ex.Message)
                    dbManager.abortarTransaccion()
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 4
            End If

            Return resultado

        End Function

        Public Function ActualizarCambioMaterialSeriales() As Short
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim resultado As Short

            If _idOrden > 0 AndAlso _infoSerial IsNot Nothing AndAlso _infoSerial.Rows.Count > 0 Then
                Try
                    With dbManager
                        .iniciarTransaccion()

                        .SqlParametros.Add("@idOrden", SqlDbType.SmallInt).Value = _idOrden
                        .SqlParametros.Add("@serial", SqlDbType.VarChar, 20)
                        .SqlParametros.Add("@cambioMaterial", SqlDbType.BigInt)
                        .SqlParametros.Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue

                        'Se agrega los parametros para actualizacion de seriales
                        Dim vista As New DataView(_infoSerial)

                        vista.RowStateFilter = DataViewRowState.ModifiedCurrent
                        For Each fila As DataRowView In vista
                            .SqlParametros("@serial").Value = fila("serial")
                            .SqlParametros("@cambioMaterial").Value = fila("cambioMaterial")
                            .ejecutarNonQuery("ActualizarCambioMaterialSeriales", CommandType.StoredProcedure)

                            Short.TryParse(.SqlParametros("@result").Value, resultado)
                            If resultado <> 0 Then Exit For
                        Next

                        If resultado = 0 Then
                            .confirmarTransaccion()
                        End If

                        If .estadoTransaccional Then .abortarTransaccion()
                    End With
                Catch ex As Exception
                    Throw New Exception(ex.Message)
                    dbManager.abortarTransaccion()
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 4
            End If

            Return resultado

        End Function

        Public Function EditarSerialesOrden(ByVal serial As String, ByVal entrega As Long) As Boolean
            Dim resultado As Boolean = False
            If _infoSerial Is Nothing Then CargarListadoSerial()

            If _infoSerial IsNot Nothing AndAlso _infoSerial.Rows.Count > 0 Then

                Dim pk(0) As DataColumn

                pk(0) = _infoSerial.Columns("serial")
                _infoSerial.PrimaryKey = pk

                If _infoSerial.Rows.Find(serial) IsNot Nothing Then
                    _infoSerial.Rows.Find(serial).BeginEdit()
                    _infoSerial.Rows.Find(serial).Item("serial") = serial
                    _infoSerial.Rows.Find(serial).Item("entrega") = entrega
                    _infoSerial.Rows.Find(serial).EndEdit()
                    resultado = True
                End If
            End If

            Return resultado
        End Function

        Public Function EditarDocCambioMaterialSerial(ByVal serial As String, ByVal cambioMaterial As Long) As Boolean
            Dim resultado As Boolean = False
            If _infoSerial Is Nothing Then CargarListadoSerial()

            If _infoSerial IsNot Nothing AndAlso _infoSerial.Rows.Count > 0 Then

                Dim pk(0) As DataColumn

                pk(0) = _infoSerial.Columns("serial")
                _infoSerial.PrimaryKey = pk

                If _infoSerial.Rows.Find(serial) IsNot Nothing Then
                    _infoSerial.Rows.Find(serial).BeginEdit()
                    _infoSerial.Rows.Find(serial).Item("serial") = serial
                    _infoSerial.Rows.Find(serial).Item("cambioMaterial") = cambioMaterial
                    _infoSerial.Rows.Find(serial).EndEdit()
                    resultado = True
                End If
            End If

            Return resultado
        End Function

#End Region

#Region "Metodos Privados"

        Private Sub CargarInformacion()
            Dim db As New LMDataAccess
            With db
                Try
                    .SqlParametros.Add("@idOrden", SqlDbType.BigInt).Value = _idOrden

                    .ejecutarReader("ObtenerInfoOrdenNacionalizacion", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            _creador = .Reader("creador")
                            _fechaCreacion = .Reader("fechaCreacion")
                        End If
                        If Not .Reader.IsClosed Then .Reader.Close()
                    End If
                Catch ex As Exception
                    Throw New Exception("Error a tratar de obtener la informacion de la orden " & ex.Message)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
        End Sub

        Private Sub EstablecerParametros(ByRef db As LMDataAccess)
            With db.SqlParametros
                .Clear()
                .Add("@idCreador", SqlDbType.BigInt).Value = _idCreador
                .Add("@nombreRealArchivo", SqlDbType.VarChar, 250).Value = _nombreArchivoCarga
                .Add("@nombreRutaArchivo", SqlDbType.VarChar, 500).Value = _nombreRutaArchivo
                If _pedido > 0 Then .Add("@pedido", SqlDbType.BigInt).Value = _pedido
                .Add("@idOrden", SqlDbType.BigInt).Direction = ParameterDirection.Output
                .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
            End With
        End Sub

        Private Sub CargarListadoSerial()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .TiempoEsperaComando = 600
                    .SqlParametros.Add("@idOrden", SqlDbType.Int).Value = _idOrden
                    _infoSerial = .ejecutarDataTable("ObtenerSerialesOrdenNacionalizacion", CommandType.StoredProcedure)
                End With
                If _infoSerial.PrimaryKey.Count = 0 Then
                    Dim pkColumn() As DataColumn = {_infoSerial.Columns("serial")}
                    _infoSerial.PrimaryKey = pkColumn
                End If
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Private Function CrearEstructuraSerialesOrden() As DataTable
            Dim dtAux As New DataTable
            With dtAux.Columns
                .Add("serial", GetType(String))
                .Add("factura", GetType(String))
                .Add("declaracion", GetType(String))
                .Add("entrega", GetType(String))
                .Add("material_LM", GetType(String))
                .Add("centro", GetType(String))
                .Add("almacen", GetType(String))
            End With
            Dim pk() As DataColumn = {dtAux.Columns("serial")}
            dtAux.PrimaryKey = pk
            Return dtAux
        End Function

#End Region

#Region "Métodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroOrdenNacionalizacion
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroOrdenNacionalizacion) As DataTable
            Dim dtDatos As New DataTable
            Dim db As New LMDataAccess
            With db
                Try
                    With .SqlParametros
                        If filtro.IdOrden > 0 Then .Add("@idOrden", SqlDbType.BigInt).Value = filtro.IdOrden
                        If filtro.IdCreador > 0 Then .Add("@idCreador", SqlDbType.Int).Value = filtro.IdCreador
                        If filtro.IdEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = filtro.IdEstado
                        If filtro.FechaInicial > Date.MinValue Then .Add("@fechaInicial", SqlDbType.DateTime).Value = filtro.FechaInicial
                        If filtro.FechaFinal > Date.MinValue Then .Add("@fechaFinal", SqlDbType.DateTime).Value = filtro.FechaFinal
                        If filtro.IdFactura > 0 Then .Add("@idFactura", SqlDbType.Int).Value = filtro.IdFactura
                        If filtro.Serial IsNot Nothing AndAlso filtro.Serial.Length > 0 Then .Add("@serial", SqlDbType.VarChar, 20).Value = filtro.Serial
                    End With
                    .TiempoEsperaComando = 600
                    dtDatos = .ejecutarDataTable("ObtenerInfoOrdenNacionalizacion", CommandType.StoredProcedure)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
            Return dtDatos
        End Function

        Public Function EntregaFueContabilizada(ByVal entrega As Long) As Boolean
            Dim dtDatos As New DataTable
            Dim db As New LMDataAccess

            If entrega > 0 Then
                Try
                    With db
                        With .SqlParametros
                            .Add("@entrega", SqlDbType.BigInt).Value = entrega
                            .Add("@result", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue
                        End With
                        .TiempoEsperaComando = 600
                        .ejecutarDataTable("EntregaFueContabilizada", CommandType.StoredProcedure)
                        Return CType(.SqlParametros("@result").Value, Boolean)
                    End With
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try

            End If
            Return False
        End Function
#End Region

#Region "Enums"

        Public Enum Estado
            Creada = 79
            CargadaSap
        End Enum

#End Region

    End Class

End Namespace