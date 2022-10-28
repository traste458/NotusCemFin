Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras

Namespace Recibos
    Public Class FacturaGuia

#Region "Variables"
        Private _idFacturaGuia As Long
        Private _idFactura As Long
        Private _idGuia As Long
        Private _cantidad As Integer
        Private _muestreo As Integer
        Private _infoFactura As InfoFactura
        Private _infoGuia As InfoGuia
        Private _prioridad As Integer
#End Region

#Region "Propiedades"
        Public Property InformacionGuia() As InfoGuia
            Get
                If _infoGuia Is Nothing Then _infoGuia = New InfoGuia(_idGuia)
                Return _infoGuia
            End Get
            Set(ByVal value As InfoGuia)
                _infoGuia = value
            End Set
        End Property

        Public Property InformacionFactura() As InfoFactura
            Get
                If _infoFactura Is Nothing Then _infoFactura = New InfoFactura(_idFactura)
                Return _infoFactura
            End Get
            Set(ByVal value As InfoFactura)
                _infoFactura = value
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

        Public Property IdGuia() As Long
            Get
                Return _idGuia
            End Get
            Set(ByVal value As Long)
                _idGuia = value
            End Set
        End Property

        Public Property IdFactura() As Long
            Get
                Return _idFactura
            End Get
            Set(ByVal value As Long)
                _idFactura = value
            End Set
        End Property

        Public ReadOnly Property IdFacturaGuia() As Long
            Get
                Return _idFacturaGuia
            End Get

        End Property

        Public Property Muestreo() As Integer
            Get
                Return _muestreo
            End Get
            Set(ByVal value As Integer)
                _muestreo = value
            End Set
        End Property

        Public Property Prioridad() As Integer
            Get
                Return _prioridad
            End Get
            Set(ByVal value As Integer)
                _prioridad = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idFacturaGuia As Long)
            Me.New()
            _idFacturaGuia = idFacturaGuia
            Me.CargarDatos()
        End Sub

        Public Sub New(ByVal idFactura As Long, ByVal idGuia As Long)
            Me.New()
            _idFactura = idFactura
            _idGuia = idGuia
            Me.CargarDatos()
        End Sub

#End Region

#Region "metodos Privados"
        Private Sub CargarDatos()
            Dim db As New LMDataAccess
            If _idFacturaGuia > 0 Then db.SqlParametros.Add("@idFacturaGuia", SqlDbType.BigInt).Value = _idFacturaGuia
            If _idGuia > 0 Then db.SqlParametros.Add("@idGuia", SqlDbType.BigInt).Value = _idGuia
            If _idFactura > 0 Then db.SqlParametros.Add("@idFactura", SqlDbType.BigInt).Value = _idFactura
            Try
                db.ejecutarReader("ObtenerInfoFacturaGuia", CommandType.StoredProcedure)
                If db.Reader IsNot Nothing AndAlso db.Reader.Read Then
                    _idFacturaGuia = db.Reader("idFacturaGuia")
                    _idFactura = db.Reader("idFactura")
                    _idGuia = db.Reader("idGuia")
                    _cantidad = db.Reader("cantidad")
                    _muestreo = db.Reader("muestreo")
                    _prioridad = db.Reader("prioridad")
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
                    .Add("@idFactura", SqlDbType.Int).Value = _idFactura
                    .Add("@idGuia", SqlDbType.Int).Value = _idGuia
                    .Add("@cantidad", SqlDbType.Int).Value = _cantidad
                    If _muestreo > 0 Then .Add("@muestreo", SqlDbType.SmallInt).Value = _muestreo
                    .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                End With

                Try
                    Dim result As Integer = 0
                    .iniciarTransaccion()
                    .ejecutarNonQuery("CrearFacturaGuia", CommandType.StoredProcedure)
                    result = .SqlParametros("@result").Value
                    If result = 0 Then
                        _idFacturaGuia = CLng(.SqlParametros("@identity").Value)
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

        Public Function Actualizar() As Short
            Dim resultado As Short = 0
            If _idFacturaGuia <> 0 And _cantidad <> 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@idFacturaGuia", SqlDbType.BigInt).Value = _idFacturaGuia
                            .Add("@cantidad", SqlDbType.Int).Value = _cantidad
                            .Add("@prioridad", SqlDbType.Int).Value = _prioridad
                            If _muestreo > 0 Then .Add("@muestreo", SqlDbType.SmallInt).Value = _muestreo
                            .Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        End With
                        .iniciarTransaccion()
                        .ejecutarNonQuery("ActualizarFacturaGuia", CommandType.StoredProcedure)
                        Short.TryParse(.SqlParametros("@returnValue").Value.ToString, resultado)

                        If resultado = 0 Then .confirmarTransaccion()
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 2
            End If
            Return resultado
        End Function

        Public Sub Actualizar(ByVal dbManager As LMDataAccess)
            Dim resultado As Short = 0
            If _idFacturaGuia <> 0 And _cantidad <> 0 Then
                Try
                    With dbManager
                        With .SqlParametros
                            .Clear()
                            .Add("@idFacturaGuia", SqlDbType.BigInt).Value = _idFacturaGuia
                            .Add("@cantidad", SqlDbType.Int).Value = _cantidad
                            If _muestreo > 0 Then .Add("@muestreo", SqlDbType.SmallInt).Value = _muestreo
                        End With
                        .ejecutarNonQuery("ActualizarFacturaGuia", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                End Try
            End If            
        End Sub


        Public Sub Eliminar()
            If _idGuia <> 0 Or _idFactura <> 0 Then
                Dim db As New LMDataAccess

                Try
                    db.iniciarTransaccion()
                    With db.SqlParametros                       
                        If _idGuia > 0 Then .Add("@idGuia", SqlDbType.BigInt).Value = _idGuia
                        If _idFactura > 0 Then .Add("@idFactura", SqlDbType.BigInt).Value = _idFactura
                    End With
                    db.ejecutarNonQuery("EliminarFacturaGuia", CommandType.StoredProcedure)
                    db.confirmarTransaccion()
                Catch ex As Exception
                    If db.estadoTransaccional Then db.abortarTransaccion()
                Finally
                    db.Dispose()
                End Try
            Else
                Throw New DuplicateNameException("La factura guia aún no ha sido registrada en la Base de Datos.")
            End If
        End Sub

#End Region

#Region "Metodos Compartidos"

        Public Shared Function ValidarFacturaGuiaTieneOrdenes(ByVal filtro As FiltroFacturaGuia) As Boolean
            Dim db As New LMDataAccess
            Dim result As Boolean = False
            With db
                If filtro.IdFacturaGuia <> 0 Then .SqlParametros.Add("@idFacturaGuia", SqlDbType.BigInt).Value = filtro.IdFacturaGuia
                If filtro.IdFactura <> 0 Then .SqlParametros.Add("@idFactura", SqlDbType.BigInt).Value = filtro.IdFactura
                If filtro.IdGuia <> 0 Then .SqlParametros.Add("@idGuia", SqlDbType.BigInt).Value = filtro.IdGuia
                .SqlParametros.Add("@result", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue

                Try
                    .ejecutarNonQuery("ValidarFacturaGuiaTieneOrdenes", CommandType.StoredProcedure)
                    result = CType(.SqlParametros("@result").Value, Boolean)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With

            Return result
        End Function

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroFacturaGuia
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroFacturaGuia) As DataTable
            Dim dtDatos As New DataTable
            Dim db As New LMDataAccess
            With db
                With .SqlParametros
                    If filtro.IdFacturaGuia <> 0 Then .Add("@IdFacturaGuia", SqlDbType.BigInt).Value = filtro.IdFacturaGuia
                    If filtro.IdFactura <> 0 Then .Add("@idFactura", SqlDbType.BigInt).Value = filtro.IdFactura
                    If filtro.IdGuia <> 0 Then .Add("@idGuia", SqlDbType.BigInt).Value = filtro.IdGuia
                    If filtro.idDetalleOrdenCompra <> 0 Then .Add("@idDetalleOrdenCompra", SqlDbType.Int).Value = filtro.idDetalleOrdenCompra
                End With
                Try
                    dtDatos = .ejecutarDataTable("ObtenerFacturaGuia", CommandType.StoredProcedure)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
            Return dtDatos
        End Function

        Public Overloads Shared Function CantidadPorFactura(ByVal idFactura As Long) As Integer
            Dim filtro As New Estructuras.FiltroFacturaGuia
            Dim dt As New DataTable
            Dim totalCantidad As Integer = 0
            Dim i As Integer
            filtro.IdFactura = idFactura
            dt = ObtenerListado(filtro)
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    totalCantidad += CInt(dt.Rows(i)("cantidad"))
                Next

            End If

            Return totalCantidad
        End Function

        Public Shared Function ObtenerPool() As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim dt As DataTable = db.ejecutarDataTable("ObtenerPoolFacturaGuia", CommandType.StoredProcedure)
            Return dt
        End Function

#End Region

    End Class

End Namespace