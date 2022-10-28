Imports LMDataAccessLayer
Imports ILSBusinessLayer

Namespace Recibos
    Public Class InfoGuia

#Region "variables"
        Private _idGuia As Long
        Private _idOrdenCompra As Long
        Private _guia As String
        Private _idTransportador As Integer
        Private _idCiudadOrigen As Integer
        Private _fechaSalida As Date
        Private _fechaEsperadaArribo As Date
        Private _idEstado As Integer
        Private _pesoNeto As Decimal
        Private _pesoBruto As Decimal
        Private _idUsuario As Long
        Private _fechaRegistro As Date
#End Region

#Region "propiedades"
        Public Property IdGuia() As Long
            Get
                Return _idGuia
            End Get
            Set(ByVal value As Long)
                _idGuia = value
            End Set
        End Property
        Public Property IdOrdenCompra() As Long
            Get
                Return _idOrdenCompra
            End Get
            Set(ByVal value As Long)
                _idOrdenCompra = value
            End Set
        End Property
        Public Property Guia() As String
            Get
                Return _guia
            End Get
            Set(ByVal value As String)
                _guia = value
            End Set
        End Property
        Public Property IdTransportador() As Integer
            Get
                Return _idTransportador
            End Get
            Set(ByVal value As Integer)
                _idTransportador = value
            End Set
        End Property
        Public Property IdCiudadOrigen() As Integer
            Get
                Return _idCiudadOrigen
            End Get
            Set(ByVal value As Integer)
                _idCiudadOrigen = value
            End Set
        End Property
        Public Property FechaSalida() As Date
            Get
                Return _fechaSalida
            End Get
            Set(ByVal value As Date)
                _fechaSalida = value
            End Set
        End Property
        Public Property FechaEsperadaArribo() As Date
            Get
                Return _fechaEsperadaArribo
            End Get
            Set(ByVal value As Date)
                _fechaEsperadaArribo = value
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
        Public Property PesoNeto() As Decimal
            Get
                Return _pesoNeto
            End Get
            Set(ByVal value As Decimal)
                _pesoNeto = value
            End Set
        End Property
        Public Property PesoBruto() As Decimal
            Get
                Return _pesoBruto
            End Get
            Set(ByVal value As Decimal)
                _pesoBruto = value
            End Set
        End Property
        Public Property IdUsuario() As Long
            Get
                Return _idUsuario
            End Get
            Set(ByVal value As Long)
                _idUsuario = value
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


#End Region

#Region "constructores"
        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idGuia As Long)
            Me.New()
            _idGuia = idGuia
            Me.CargarDatos()
        End Sub

        Public Sub New(ByVal idTransportador As Integer, ByVal guia As String)
            Me.New()
            _idTransportador = idTransportador
            _guia = guia
            Me.CargarDatos(guia)
        End Sub


#End Region

#Region "metodos Privados"
        Private Sub CargarDatos(Optional ByVal numGuia As String = "")
            Dim db As New LMDataAccess
            If _idGuia > 0 Then db.SqlParametros.Add("@idGuia", SqlDbType.BigInt).Value = _idGuia
            If _idTransportador > 0 Then db.SqlParametros.Add("@idTransportador", SqlDbType.Int).Value = _idTransportador
            If numGuia <> "" Then
                db.SqlParametros.Add("@num_guia", SqlDbType.VarChar).Value = numGuia
            Else
                If _guia <> "" Then db.SqlParametros.Add("@guia", SqlDbType.VarChar).Value = _guia
            End If
            Try
                db.ejecutarReader("ObtenerInfoGuia", CommandType.StoredProcedure)
                If db.Reader.Read Then
                    _idOrdenCompra = db.Reader("idOrdenCompra")
                    _idGuia = db.Reader("idGuia")
                    _guia = db.Reader("guia")
                    _idTransportador = db.Reader("idTransportador")
                    _idCiudadOrigen = db.Reader("idCiudadOrigen")
                    _fechaSalida = db.Reader("fechaSalida")
                    _fechaEsperadaArribo = db.Reader("fechaEsperadaArribo")
                    _idEstado = db.Reader("idEstado")
                    _pesoNeto = db.Reader("pesoNeto")
                    _pesoBruto = db.Reader("pesoBruto")
                    _idUsuario = db.Reader("idUsuario")
                    _fechaRegistro = db.Reader("fechaRegistro")
                End If
            Catch ex As Exception
            Finally
                If Not db.Reader.IsClosed Then db.Reader.Close()
                db.Dispose()
            End Try
        End Sub
#End Region

#Region "metodos Publicos"

        Public Function Crear() As Boolean
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim retorno As Boolean
            With db
                With .SqlParametros
                    .Add("@idOrdenCompra", SqlDbType.BigInt).Value = _idOrdenCompra
                    .Add("@guia", SqlDbType.VarChar).Value = _guia.ToString
                    .Add("@idTransportador", SqlDbType.Int).Value = _idTransportador
                    .Add("@idCiudadOrigen", SqlDbType.Int).Value = _idCiudadOrigen
                    .Add("@fechaSalida", SqlDbType.SmallDateTime).Value = _fechaSalida
                    .Add("@fechaEsperadaArribo", SqlDbType.SmallDateTime).Value = _fechaEsperadaArribo
                    .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                    .Add("@pesoNeto", SqlDbType.Decimal).Value = _pesoNeto
                    .Add("@pesoBruto", SqlDbType.Decimal).Value = _pesoBruto
                    .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                End With

                Try
                    Dim result As Integer = 0
                    .iniciarTransaccion()
                    .ejecutarNonQuery("CrearInfoGuia", CommandType.StoredProcedure)
                    result = .SqlParametros("@result").Value
                    If result = 0 Then
                        _idGuia = CLng(.SqlParametros("@identity").Value)
                        .confirmarTransaccion()
                        retorno = True
                    End If

                Catch ex As Exception
                    If .estadoTransaccional Then .abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    .cerrarConexion()
                    .Dispose()
                End Try
            End With
            Return retorno
        End Function

        Public Sub Actualizar()
            If _idGuia <> 0 Then
                Dim db As New LMDataAccess

                Try
                    db.iniciarTransaccion()
                    With db.SqlParametros
                        .Add("@idGuia", SqlDbType.BigInt).Value = _idGuia
                        .Add("@idOrdenCompra", SqlDbType.Int).Value = _idOrdenCompra
                        .Add("@guia", SqlDbType.VarChar).Value = _guia
                        .Add("@idTransportador", SqlDbType.Int).Value = _idTransportador
                        .Add("@idCiudadOrigen", SqlDbType.Int).Value = _idCiudadOrigen
                        .Add("@fechaSalida", SqlDbType.SmallDateTime).Value = _fechaSalida
                        .Add("@fechaEsperadaArribo", SqlDbType.SmallDateTime).Value = _fechaEsperadaArribo
                        .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        .Add("@pesoNeto", SqlDbType.Decimal).Value = _pesoNeto
                        .Add("@pesoBruto", SqlDbType.Decimal).Value = _pesoBruto
                        .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    End With
                    db.ejecutarNonQuery("ActualizarInfoGuia", CommandType.StoredProcedure)
                    db.confirmarTransaccion()
                Catch ex As Exception
                    If db.estadoTransaccional Then db.abortarTransaccion()                    
                Finally
                    db.Dispose()
                End Try
            Else
                Throw New DuplicateNameException("La guia aún no ha sido registrada en la Base de Datos.")
            End If
        End Sub

        Public Sub Eliminar()
            If _idGuia <> 0 Then
                Dim db As New LMDataAccess

                Try
                    db.iniciarTransaccion()
                    With db.SqlParametros
                        .Add("@idGuia", SqlDbType.BigInt).Value = _idGuia
                        .Add("@idUsuario", SqlDbType.BigInt).Value = _idUsuario

                    End With
                    db.ejecutarNonQuery("EliminarInfoGuia", CommandType.StoredProcedure)
                    db.confirmarTransaccion()
                Catch ex As Exception
                    If db.estadoTransaccional Then db.abortarTransaccion()
                Finally
                    db.Dispose()
                End Try
            Else
                Throw New DuplicateNameException("La guia aún no ha sido registrada en la Base de Datos.")
            End If
        End Sub

#End Region

#Region "métodos compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New Estructuras.FiltroInfoGuia
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As Estructuras.FiltroInfoGuia) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdGuia > 0 Then db.SqlParametros.Add("@idGuia", SqlDbType.BigInt).Value = .IdGuia
                If .IdOrdenCompra > 0 Then db.SqlParametros.Add("@idOrdenCompra", SqlDbType.Int).Value = .IdOrdenCompra
                If .Guia IsNot Nothing AndAlso .Guia.Trim.Length > 0 Then db.SqlParametros.Add("@guia", SqlDbType.VarChar).Value = .Guia
                If .IdTransportador > 0 Then db.SqlParametros.Add("@idTransportador", SqlDbType.Int).Value = .IdTransportador
                If .IdCiudadOrigen > 0 Then db.SqlParametros.Add("@idCiudadOrigen", SqlDbType.Int).Value = .IdCiudadOrigen
                If .Activo > 0 Then db.SqlParametros.Add("@idEstado", SqlDbType.Int).Value = .Activo
                If .IdFactura > 0 Then db.SqlParametros.Add("@idFactura", SqlDbType.Int).Value = .IdFactura
                If .EstadoOrdenCompra > 0 Then db.SqlParametros.Add("@estadoOrdenCompra", SqlDbType.Int).Value = .EstadoOrdenCompra
                If .ListaEstado IsNot Nothing AndAlso .ListaEstado.Count Then db.SqlParametros.Add("@listaEstado", SqlDbType.VarChar).Value = Join(.ListaEstado.ToArray, ",")
                dtDatos = db.ejecutarDataTable("ObtenerInfoGuia", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos

        End Function

        Public Overloads Shared Function VerificarGuia(ByVal idFactura As Long, ByVal idTransp As Integer, ByVal guia As String) As Boolean
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            Dim retorno As Boolean = False
            With db
                .SqlParametros.Add("@idFactura", SqlDbType.BigInt).Value = idFactura
                .SqlParametros.Add("@idTransp", SqlDbType.Int).Value = idTransp
                .SqlParametros.Add("@guia", SqlDbType.VarChar).Value = guia                
                .SqlParametros.Add("@verificarFacturaGuia", SqlDbType.Bit).Value = 1
                dtDatos = .ejecutarDataTable("ObtenerInfoGuia", CommandType.StoredProcedure)
            End With
            If dtDatos.Rows.Count > 0 Then
                retorno = True
            End If
            Return retorno
        End Function

        Public Overloads Shared Function VerificarGuia(ByVal idFactura As Long, ByVal guia As String) As Boolean
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            Dim retorno As Boolean = False
            With db
                .SqlParametros.Add("@idFactura", SqlDbType.BigInt).Value = idFactura                
                .SqlParametros.Add("@guia", SqlDbType.VarChar).Value = guia
                .SqlParametros.Add("@verificarFacturaGuia", SqlDbType.Bit).Value = 1
                dtDatos = .ejecutarDataTable("ObtenerInfoGuia", CommandType.StoredProcedure)
            End With
            If dtDatos.Rows.Count > 0 Then
                retorno = True
            End If
            Return retorno
        End Function

        Public Overloads Shared Function VerificarGuia(ByVal idTransp As Integer, ByVal guia As String) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable            
            With db
                .SqlParametros.Add("@idTransp", SqlDbType.Int).Value = idTransp
                .SqlParametros.Add("@guia", SqlDbType.VarChar).Value = guia
                .SqlParametros.Add("@verificarFacturaGuia", SqlDbType.Bit).Value = 1
                dtDatos = .ejecutarDataTable("ObtenerInfoGuia", CommandType.StoredProcedure)
            End With
            Return dtDatos
        End Function

        Public Overloads Shared Function VerificarGuiaObtenerGuia(ByVal idFactura As Long, ByVal guia As String) As InfoGuia
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable

            With db
                .SqlParametros.Add("@idFactura", SqlDbType.BigInt).Value = idFactura
                .SqlParametros.Add("@guia", SqlDbType.VarChar).Value = guia
                .SqlParametros.Add("@verificarFacturaGuia", SqlDbType.Bit).Value = 1
                dtDatos = .ejecutarDataTable("ObtenerInfoGuia", CommandType.StoredProcedure)
            End With
            Dim guiaInfo As New InfoGuia(CLng(dtDatos.Rows(0)("idGuia")))
            Return guiaInfo
        End Function


#End Region

    End Class
End Namespace

