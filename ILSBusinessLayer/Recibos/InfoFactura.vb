Imports LMDataAccessLayer

Namespace Recibos

    Public Class InfoFactura
        Inherits DetalleOrdenCompra
#Region "variables"
        Private _idFactura As Long
        Private _idDetalleOrdenCompra As Long
        Private _factura As String
        Private _facturaInterna As String
        Private _cantidad As Integer
        Private _idCiudadCompra As Integer
        Private _idEstado As Long
        Private _idUsuario As Long
        Private _fechaRegistro As Date
        Private _numeroOrdenCompra As String
        Private _estado As String
        Private _cantidadConGuia As Integer

#End Region

#Region "propiedades"

        Public ReadOnly Property Estado() As String
            Get
                Return _estado
            End Get

        End Property

        Public ReadOnly Property NumeroOrdenCompra() As String
            Get
                Return _numeroOrdenCompra
            End Get

        End Property

        Public Property IdFactura() As Long
            Get
                Return _idFactura
            End Get
            Set(ByVal value As Long)
                _idFactura = value
            End Set
        End Property

        Public Property IdDetalleOrdenCompra() As Long
            Get
                Return _idDetalleOrdenCompra
            End Get
            Set(ByVal value As Long)
                _idDetalleOrdenCompra = value
            End Set
        End Property

        Public Overloads Property Factura() As String
            Get
                Return _factura
            End Get
            Set(ByVal value As String)
                _factura = value
            End Set
        End Property

        Public Property FacturaInterna() As String
            Get
                Return _facturaInterna
            End Get
            Set(ByVal value As String)
                _facturaInterna = value
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

        Public Property CantidadConGuia() As Integer
            Get
                Return _cantidadConGuia
            End Get
            Protected Friend Set(ByVal value As Integer)
                _cantidadConGuia = value
            End Set
        End Property

        Public Property IdCiudadCompra() As Integer
            Get
                Return _idCiudadCompra
            End Get
            Set(ByVal value As Integer)
                _idCiudadCompra = value
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

        Public Overloads Property IdUsuario() As Long
            Get
                Return _idUsuario
            End Get
            Set(ByVal value As Long)
                _idUsuario = value
            End Set
        End Property

        Public Overloads Property fechaRegistro() As Date
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
        Public Sub New(ByVal idFactura As Long)
            Me.New()
            _idFactura = idFactura
            Me.CargarDatos()
        End Sub
#End Region

#Region "metodos Privados"
        Private Sub CargarDatos()
            Dim db As New LMDataAccess
            db.SqlParametros.Add("@idFactura", SqlDbType.BigInt).Value = _idFactura
            Try
                db.ejecutarReader("ObtenerInfoFactura", CommandType.StoredProcedure)
                If db.Reader.Read Then
                    _idDetalleOrdenCompra = db.Reader("idDetalleOrdenCompra")
                    _factura = db.Reader("factura")
                    _facturaInterna = db.Reader("facturaInterna")
                    _cantidad = db.Reader("cantidad")
                    _idCiudadCompra = db.Reader("idCiudadCompra")
                    _idEstado = db.Reader("idEstado")
                    _idUsuario = db.Reader("idUsuario")
                    _fechaRegistro = db.Reader("fechaRegistro")
                    Fabricante = db.Reader("fabricante")
                    _numeroOrdenCompra = db.Reader("numeroOrden")
                    IdProducto = db.Reader("IdProducto")
                    _estado = db.Reader("estadoFactura")
                    IdOrden = db.Reader("IdOrden")
                End If
            Catch ex As Exception
            Finally
                If Not db.Reader.IsClosed Then db.Reader.Close()
                db.Dispose()
            End Try
        End Sub
#End Region

#Region "metodos Publicos"

        Public Overloads Function Crear() As Boolean
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim retorno As Boolean
            With db
                With .SqlParametros
                    .Add("@idDetalleOrdenCompra", SqlDbType.BigInt).Value = _idDetalleOrdenCompra
                    .Add("@factura", SqlDbType.VarChar).Value = _factura.ToString
                    .Add("@cantidad", SqlDbType.Int).Value = _cantidad
                    .Add("@idCiudadCompra", SqlDbType.Int).Value = _idCiudadCompra
                    .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                    .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                End With

                Try
                    Dim result As Integer = 0
                    .iniciarTransaccion()
                    .ejecutarNonQuery("CrearInfoFactura", CommandType.StoredProcedure)
                    result = .SqlParametros("@result").Value
                    If result = 0 Then
                        _idFactura = CLng(.SqlParametros("@identity").Value)
                        .confirmarTransaccion()
                        retorno = True
                    End If

                Catch ex As Exception
                    If .estadoTransaccional Then .abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    .Dispose()
                End Try
            End With
            Return retorno
        End Function

        Public Function Actualizar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If _idFactura <> 0 Then
                Dim db As New LMDataAccess

                Try
                    db.iniciarTransaccion()
                    With db.SqlParametros
                        .Add("@idFactura", SqlDbType.BigInt).Value = _idFactura
                        .Add("@idDetalleOrdenCompra", SqlDbType.Int).Value = _idDetalleOrdenCompra
                        .Add("@factura", SqlDbType.VarChar).Value = _factura.ToString
                        .Add("@cantidad", SqlDbType.Int).Value = _cantidad
                        .Add("@idCiudadCompra", SqlDbType.Int).Value = _idCiudadCompra
                        .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    End With
                    db.ejecutarNonQuery("ActualizarInfoFactura", CommandType.StoredProcedure)
                    If Integer.TryParse(db.SqlParametros("@resultado").Value, resultado.Valor) Then
                        resultado.Mensaje = db.SqlParametros("@mensaje").Value.ToString
                        If resultado.Valor = 0 Then
                            db.confirmarTransaccion()
                        Else
                            db.abortarTransaccion()
                        End If
                    Else
                        resultado.EstablecerMensajeYValor("10", "Imposible determinar la respuesta del servidor. Por favor intente nuevamente")
                    End If
                Catch ex As Exception
                    If db IsNot Nothing AndAlso db.estadoTransaccional Then db.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            Else
                Throw New DuplicateNameException("La facutra aún no ha sido registrada en la Base de Datos.")
            End If
            Return resultado
        End Function

        Public Sub Eliminar()
            If _idFactura <> 0 Then
                Dim db As New LMDataAccess

                Try
                    db.iniciarTransaccion()
                    With db.SqlParametros
                        .Add("@idFactura", SqlDbType.BigInt).Value = _idFactura
                    End With
                    db.ejecutarNonQuery("EliminarInfoFactura", CommandType.StoredProcedure)
                    db.confirmarTransaccion()
                Catch ex As Exception
                    If db.estadoTransaccional Then db.abortarTransaccion()
                Finally
                    db.Dispose()
                End Try
            Else
                Throw New DuplicateNameException("La factura aún no ha sido registrada en la Base de Datos.")
            End If
        End Sub

#End Region

#Region "métodos compartidos"
        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New Estructuras.FiltroInfoFactura
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As Estructuras.FiltroInfoFactura) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdFactura > 0 Then db.SqlParametros.Add("@idFactura", SqlDbType.BigInt).Value = .IdFactura
                If .IdDetalleOrdenCompra > 0 Then db.SqlParametros.Add("@idDetalleOrdenCompra", SqlDbType.Int).Value = .IdDetalleOrdenCompra
                If .Factura <> "" Then db.SqlParametros.Add("@factura", SqlDbType.VarChar).Value = .Factura.ToString
                If .IdCiudadCompra > 0 Then db.SqlParametros.Add("@idCiudadCompra", SqlDbType.Int).Value = .IdCiudadCompra
                If .IdGuia > 0 Then db.SqlParametros.Add("@idGuia", SqlDbType.Int).Value = .IdGuia
                If .IdEstado > 0 Then db.SqlParametros.Add("@idEstado", SqlDbType.Int).Value = .IdEstado
                If .IdOrdenCompra > 0 Then db.SqlParametros.Add("@idOrdenCompra", SqlDbType.Int).Value = .IdOrdenCompra
                If .IdProveedor > 0 Then db.SqlParametros.Add("@idProveedor", SqlDbType.Int).Value = .IdProveedor
                If .ListaEstado IsNot Nothing AndAlso .ListaEstado.Count Then db.SqlParametros.Add("@listaEstado", SqlDbType.VarChar).Value = Join(.ListaEstado.ToArray, ",")
                dtDatos = db.ejecutarDataTable("ObtenerInfoFactura", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos

        End Function

        Public Overloads Shared Function ExisteFactura(ByVal factura As String, ByVal idProveedor As Integer) As Boolean
            Dim retorno As Boolean = False
            Dim filtro As New Estructuras.FiltroInfoFactura
            Dim dt As New DataTable
            filtro.Factura = factura
            filtro.IdProveedor = idProveedor
            dt = ObtenerListado(filtro)
            If dt.Rows.Count > 0 Then
                retorno = True
            End If
            Return retorno
        End Function

        Public Overloads Shared Function CantidadEnFactura(ByVal idDetalleOrdenCompra As Integer) As Integer
            Dim filtro As New Estructuras.FiltroInfoFactura
            Dim dt As New DataTable
            Dim totalCantidad As Integer = 0
            Dim i As Integer
            filtro.IdDetalleOrdenCompra = idDetalleOrdenCompra
            dt = ObtenerListado(filtro)
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    totalCantidad += CInt(dt.Rows(i)("cantidad"))
                Next
            End If

            Return totalCantidad
        End Function

#End Region

    End Class
End Namespace

