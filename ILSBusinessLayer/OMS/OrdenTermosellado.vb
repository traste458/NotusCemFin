Imports ILSBusinessLayer.Estructuras
Imports LMDataAccessLayer

Namespace OMS

    Public Class OrdenTermosellado

#Region "Atributos"
        Private _idOrdenTermosellado As Long
        Private _idCreador As Long
        Private _fechaCreacion As Date
        Private _idEstado As Integer
        Private _fechaCierre As Date
        Private _idUsuarioCierre As Long
        Private _creador As String
        Private _usuarioCierre As String
        Private _estado As String
        Private _serial As String
        Private _caja As Integer
        Private _estiba As Integer
        Private _idFactura As Long
        Private _region As String
        Private _cantidadSeriales As Integer
        Private _otb As Long
        Private _idMaterial As Integer
#End Region

#Region "Constructores"

        Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            _idOrdenTermosellado = identificador
            CargarInformacion()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdMaterialTermosellado() As Integer
            Get
                Return _idMaterial
            End Get
            Set(ByVal value As Integer)
                _idMaterial = value
            End Set
        End Property

        Public ReadOnly Property IdOrdenTermosellado() As Long
            Get
                Return _idOrdenTermosellado
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

        Public Property FechaCreacion() As Date
            Get
                Return _fechaCreacion
            End Get
            Set(ByVal value As Date)
                _fechaCreacion = value
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

        Public Property Creador() As String
            Get
                Return _creador
            End Get
            Set(ByVal value As String)
                _creador = value
            End Set
        End Property

        Public Property UsuarioCierre() As String
            Get
                Return _usuarioCierre
            End Get
            Set(ByVal value As String)
                _usuarioCierre = value
            End Set
        End Property

        Public Property Estado() As String
            Get
                Return _estado
            End Get
            Set(ByVal value As String)
                _estado = value
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

        Public Property Caja() As Integer
            Get
                Return _caja
            End Get
            Set(ByVal value As Integer)
                _caja = value
            End Set
        End Property

        Public Property Estiba() As Integer
            Get
                Return _estiba
            End Get
            Set(ByVal value As Integer)
                _estiba = value
            End Set
        End Property

        Public Property IdFactura() As Integer
            Get
                Return _idFactura
            End Get
            Set(ByVal value As Integer)
                _idFactura = value
            End Set
        End Property

        Public Property Region() As String
            Get
                Return _region
            End Get
            Set(ByVal value As String)
                _region = value
            End Set
        End Property

        Public Property OTB() As Long
            Get
                Return _otb
            End Get
            Set(ByVal value As Long)
                _otb = value
            End Set
        End Property

        Public Property CantidadSeriales() As Integer
            Get
                Return _cantidadSeriales
            End Get
            Set(ByVal value As Integer)
                _cantidadSeriales = value
            End Set
        End Property

#End Region

#Region "Metodos Publicos"

        Public Sub Actualizar()
            If IdOrdenTermosellado <> 0 Then
                Dim db As New LMDataAccessLayer.LMDataAccess

                Try
                    db.iniciarTransaccion()
                    With db.SqlParametros
                        .Add("@idOrdenTermosellado", SqlDbType.BigInt).Value = Me.IdOrdenTermosellado
                        .Add("@idUsuarioCierre", SqlDbType.BigInt).Value = Me.IdUsuarioCierre
                        .Add("@idEstado", SqlDbType.Int).Value = Me.IdEstado
                    End With
                    db.ejecutarNonQuery("ActualizarOrdenTermosellado", CommandType.StoredProcedure)
                    db.confirmarTransaccion()
                Catch ex As Exception
                    If db.estadoTransaccional Then db.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.cerrarConexion()
                End Try
            Else
                Throw New DuplicateNameException("La orden de termosellado no pudo ser actualizada.")
            End If
        End Sub

        Public Function ActualizarProductos_serial() As Short
            Dim result As Short = 1

            If IdOrdenTermosellado <> 0 Then
                Dim db As New LMDataAccessLayer.LMDataAccess

                Try
                    db.iniciarTransaccion()

                    ParametrosSerialATermosellar(db)

                    db.ejecutarNonQuery("ActualizarSerialTermosellado", CommandType.StoredProcedure)
                    result = CShort(db.SqlParametros("@result").Value)

                    If result = 1 Then
                        If db.estadoTransaccional Then db.abortarTransaccion()
                        Return result
                        'Throw New Exception("No se pudieron agregar los seriales a la orden de termosellado.")
                    End If

                    db.confirmarTransaccion()
                Catch ex As Exception
                    If db.estadoTransaccional Then db.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End If

            Return result
        End Function

        Public Function Crear() As Short
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim result As Short = 1

            With db
                EstablecerParametros(db)

                Try
                    .iniciarTransaccion()
                    .ejecutarNonQuery("CrearOrdenTermosellado", CommandType.StoredProcedure)
                    result = CShort(.SqlParametros("@result").Value)
                    If result = 0 Then
                        _idOrdenTermosellado = CLng(.SqlParametros("@identity").Value)

                        ParametrosSerialATermosellar(db)

                        .ejecutarNonQuery("ActualizarSerialTermosellado", CommandType.StoredProcedure)
                        result = CShort(.SqlParametros("@result").Value)

                        If result = 1 Then
                            If .estadoTransaccional Then .abortarTransaccion()
                            Return result
                            'Throw New Exception("No se pudieron agregar los seriales a la orden de termosellado.")
                        End If

                        .confirmarTransaccion()
                    Else
                        If .estadoTransaccional Then .abortarTransaccion()
                        Return result
                        'Throw New Exception("Imposible registrar la información de la Orden en la Base de Datos.")
                    End If
                Catch ex As Exception
                    If .estadoTransaccional Then .abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With

            Return result
        End Function
#End Region

#Region "Metodos Privados"

        Private Sub CargarInformacion()
            Dim db As New LMDataAccess
            With db
                With .SqlParametros
                    .Add("@idOrdenTermosellado", SqlDbType.BigInt).Value = _idOrdenTermosellado
                End With

                Try
                    .ejecutarReader("ObtenerOrdenTermosellado", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            _creador = .Reader("creador").ToString()
                            _usuarioCierre = .Reader("usuarioCierre").ToString()
                            _estado = .Reader("estado").ToString()
                            _cantidadSeriales = .Reader("cantidadLeida").ToString()
                            If Not IsDBNull(.Reader("idCreador")) Then _idCreador = .Reader("idCreador").ToString
                            If Not IsDBNull(.Reader("fechaCreacion")) Then _fechaCreacion = .Reader("fechaCreacion").ToString
                            If Not IsDBNull(.Reader("fechaCierre")) Then _fechaCierre = .Reader("fechaCierre").ToString
                        End If
                        If Not .Reader.IsClosed Then .Reader.Close()
                    End If
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
        End Sub

        Private Sub EstablecerParametros(ByRef db As LMDataAccess)
            With db.SqlParametros
                .Add("@idCreador", SqlDbType.BigInt).Value = _idCreador
                .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                .Add("@idMaterial", SqlDbType.Int).Value = _idMaterial
                .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
            End With
        End Sub

        Private Sub ParametrosSerialATermosellar(ByRef db As LMDataAccess)
            With db.SqlParametros
                .Clear()
                If _idOrdenTermosellado <> 0 Then .Add("@idOrdenTermosellado", SqlDbType.BigInt).Value = _idOrdenTermosellado
                If Not String.IsNullOrEmpty(Serial) Then .Add("@serial", SqlDbType.VarChar, 15).Value = Serial
                If Caja <> 0 Then .Add("@caja", SqlDbType.Int).Value = Caja
                If Estiba <> 0 Then .Add("@Estiba", SqlDbType.Int).Value = Estiba
                If IdFactura <> 0 Then .Add("@idFactura", SqlDbType.BigInt).Value = IdFactura
                If Not String.IsNullOrEmpty(Region) Then .Add("@region", SqlDbType.VarChar, 10).Value = Region
                If OTB <> 0 Then .Add("@otb", SqlDbType.BigInt).Value = OTB
                If IdCreador <> 0 Then .Add("@idUsuario", SqlDbType.BigInt).Value = IdCreador
                .Add("@existencia", SqlDbType.SmallInt).Direction = ParameterDirection.Output
                .Add("@result", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
            End With
        End Sub

        Public Sub TermosellarLB(ByVal idLectura As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim resultado As Integer

            db.agregarParametroSQL("@idLectura", idLectura, SqlDbType.Int)
            db.agregarParametroSQL("@idOrdenTermosellado", _idOrdenTermosellado, SqlDbType.Int)
            db.agregarParametroSQL("@idUsuario", _idCreador, SqlDbType.Int)
            db.SqlParametros.Add("@retorno", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue

            Try
                db.iniciarTransaccion()
                db.ejecutarNonQuery("EstablecerTermoselladoLB", CommandType.StoredProcedure)
                Integer.TryParse(db.SqlParametros("@retorno").Value.ToString(), resultado)

                If resultado > 0 Then
                    If db.estadoTransaccional Then db.abortarTransaccion()
                    Throw New Exception(resultado)
                Else
                    Me.CargarInformacion()
                    db.confirmarTransaccion()
                End If
            Catch ex As Exception
                Throw New Exception(ex.Message)
                db.abortarTransaccion()
            Finally
                db.Dispose()
            End Try
        End Sub
#End Region

#Region "Métodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroTermosellado
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroTermosellado) As DataTable
            Dim dtDatos As New DataTable
            Dim db As New LMDataAccess
            With db
                With .SqlParametros
                    If filtro.IdOrdenTermosellado <> 0 Then .Add("@idOrdenTermosellado", SqlDbType.BigInt).Value = filtro.IdOrdenTermosellado
                End With
                Try
                    dtDatos = .ejecutarDataTable("ObtenerOrdenTermosellado", CommandType.StoredProcedure)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
            Return dtDatos
        End Function

        Public Shared Function ObtenerPorId(ByVal identificador As Long) As DataTable
            Dim filtro As New FiltroTermosellado
            filtro.IdOrdenTermosellado = identificador
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ValidarOrdenAbierta() As DataTable
            Dim filtro As New FiltroTermosellado
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ValidarOrdenAbierta(ByVal filtro As FiltroTermosellado) As DataTable
            Dim dtDatos As New DataTable
            Dim db As New LMDataAccess
            With db
                With .SqlParametros
                    If filtro.IdCreador <> 0 Then .Add("@idCreador", SqlDbType.Int).Value = filtro.IdCreador
                End With
                Try
                    dtDatos = .ejecutarDataTable("ValidarOrdenAbierta", CommandType.StoredProcedure)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
            Return dtDatos
        End Function

        Public Shared Function ValidarOrdenAbiertaPorId(ByVal identificador As Long) As DataTable
            Dim filtro As New FiltroTermosellado
            filtro.IdOrdenTermosellado = identificador
            Dim dtDatos As DataTable = ValidarOrdenAbierta(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ListadoSerialesSinTermosellar(ByVal filtro As FiltroTermosellado) As DataTable
            Dim dtDatos As New DataTable
            Dim db As New LMDataAccess
            With db
                With .SqlParametros
                    If filtro.IdFactura <> 0 Then .Add("@idFactura", SqlDbType.BigInt).Value = filtro.IdFactura
                    If Not String.IsNullOrEmpty(filtro.Region) Then .Add("@region", SqlDbType.VarChar, 10).Value = filtro.Region
                    If filtro.Estiba <> 0 Then .Add("@estiba", SqlDbType.Int).Value = filtro.Estiba
                    If filtro.Caja <> 0 Then .Add("@caja", SqlDbType.Int).Value = filtro.Caja
                    If Not String.IsNullOrEmpty(filtro.Serial) Then .Add("@serial", SqlDbType.VarChar, 15).Value = filtro.Serial
                    If filtro.idFacturaGuia > 0 Then .Add("@idFacturaGuia", SqlDbType.Int).Value = filtro.idFacturaGuia
                End With
                Try
                    dtDatos = .ejecutarDataTable("ObtenerSerialesSinTermosellar", CommandType.StoredProcedure)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
            Return dtDatos
        End Function

        Public Shared Function ObtenerSerialProducidoPorSerial(ByVal serial As String) As DataTable
            Dim dtDatos As New DataTable
            Dim db As New LMDataAccess
            With db
                With .SqlParametros
                    If Not String.IsNullOrEmpty(serial) Then .Add("@serial", SqlDbType.VarChar, 15).Value = serial
                End With
                Try
                    dtDatos = .ejecutarDataTable("ObtenerSerialProducido", CommandType.StoredProcedure)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
            Return dtDatos
        End Function

        Public Shared Function ValidarExistenciaSerialesSinTermosellar(ByVal filtro As FiltroTermosellado) As Boolean
            Dim db As New LMDataAccess
            Dim result As Boolean = False
            With db
                With .SqlParametros
                    If filtro.IdFactura <> 0 Then .Add("@idFactura", SqlDbType.BigInt).Value = filtro.IdFactura
                    If Not String.IsNullOrEmpty(filtro.Region) Then .Add("@region", SqlDbType.VarChar, 10).Value = filtro.Region
                    If filtro.Estiba <> 0 Then .Add("@estiba", SqlDbType.Int).Value = filtro.Estiba
                    If filtro.Caja <> 0 Then .Add("@caja", SqlDbType.Int).Value = filtro.Caja
                    If Not String.IsNullOrEmpty(filtro.Serial) Then .Add("@serial", SqlDbType.VarChar, 15).Value = filtro.Serial
                    If Not String.IsNullOrEmpty(filtro.OTB) Then .Add("@otb", SqlDbType.VarChar, 50).Value = filtro.OTB
                    .Add("@result", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue
                End With

                Try
                    .ejecutarNonQuery("ValidarExistenciaSerialesSinTermosellar", CommandType.StoredProcedure)
                    result = CType(.SqlParametros("@result").Value, Boolean)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With

            Return result
        End Function

        Public Shared Function ValidarExistenciaSeriales(ByVal filtro As FiltroTermosellado) As Short
            Dim db As New LMDataAccess
            Dim result As Short
            With db
                With .SqlParametros
                    If filtro.IdFactura <> 0 Then .Add("@idFactura", SqlDbType.BigInt).Value = filtro.IdFactura
                    If Not String.IsNullOrEmpty(filtro.Region) Then .Add("@region", SqlDbType.VarChar, 10).Value = filtro.Region
                    If filtro.Estiba <> 0 Then .Add("@estiba", SqlDbType.Int).Value = filtro.Estiba
                    If filtro.Caja <> 0 Then .Add("@caja", SqlDbType.Int).Value = filtro.Caja
                    If Not String.IsNullOrEmpty(filtro.Serial) Then .Add("@serial", SqlDbType.VarChar, 15).Value = filtro.Serial
                    If Not String.IsNullOrEmpty(filtro.OTB) Then .Add("@otb", SqlDbType.VarChar, 50).Value = filtro.OTB
                    .Add("@result", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue
                End With

                Try
                    .ejecutarNonQuery("ValidarExistenciaSeriales", CommandType.StoredProcedure)
                    Short.TryParse(.SqlParametros("@result").Value.ToString, result)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With

            Return result
        End Function

        Public Shared Function ObtenerConsultaTermosellado(ByVal filtro As Estructuras.FiltroTermosellado) As DataTable
            Dim db As New LMDataAccess
            With filtro
                If .ordenCompra <> "" Then db.agregarParametroSQL("@ordencompra", .ordenCompra)
                If .factura <> "" Then db.agregarParametroSQL("@factura", .factura)
                If .guia <> "" Then db.agregarParametroSQL("@guia", .guia)
                If .pendienteTermosellado Then db.agregarParametroSQL("@pendienteTermosellado", .pendienteTermosellado, SqlDbType.Bit)
            End With
            Dim dt As DataTable = db.ejecutarDataTable("ObtenerReporteTermosellado", CommandType.StoredProcedure)
            Return dt
        End Function

#End Region

#Region "Enums"

        Public Enum EstadoTermosellado
            abierta = 26
            cerrada = 27
        End Enum

#End Region

    End Class
End Namespace