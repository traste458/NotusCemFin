Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer
Imports LMDataAccessLayer

Namespace OMS

    Public Class InfoTarjetaPrepago

#Region "Variables privadas"

        Private _idRegistro As Integer
        Private _serial As String
        Private _idRegion As Integer
        Private _region As String
        Private _idProducto As Integer
        Private _producto As String
        Private _material As String
        Private _descripcionMaterial As String
        Private _lote As String
        Private _fechaVencimiento As Date
        Private _fechaRegistro As Date
        Private _idOrdenRecepcion As Integer
        Private _fechaRecepcion As Date
        Private _idCaja As Long
        Private _cantidad As Integer
        Private _fechaCargue As Date
        Private _registrada As Boolean

#End Region

#Region "Propiedades Publicas"

        Public Property IdRegistro() As Integer
            Get
                Return _idRegistro
            End Get
            Set(ByVal value As Integer)
                _idRegistro = value
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

        Public Property IdRegion() As Integer
            Get
                Return _idRegion
            End Get
            Set(ByVal value As Integer)
                _idRegion = value
            End Set
        End Property

        Public Property Region() As String
            Get
                Return _region
            End Get
            Protected Friend Set(ByVal value As String)
                _region = value
            End Set
        End Property

        Public Property IdProducto() As Integer
            Get
                Return _idProducto
            End Get
            Set(ByVal value As Integer)
                _idProducto = value
            End Set
        End Property

        Public Property Producto() As String
            Get
                Return _producto
            End Get
            Protected Friend Set(ByVal value As String)
                _producto = value
            End Set
        End Property

        Public Property Material() As String
            Get
                Return _material
            End Get
            Set(ByVal value As String)
                _material = value
            End Set
        End Property

        Public Property DescripcionMaterial() As String
            Get
                Return _descripcionMaterial
            End Get
            Protected Friend Set(ByVal value As String)
                _descripcionMaterial = value
            End Set
        End Property

        Public Property Lote() As String
            Get
                Return _lote
            End Get
            Set(ByVal value As String)
                _lote = value
            End Set
        End Property

        Public Property FechaVencimiento() As Date
            Get
                Return _fechaVencimiento
            End Get
            Set(ByVal value As Date)
                _fechaVencimiento = value
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

        Public Property IdOrdenRecepcion() As Integer
            Get
                Return _idOrdenRecepcion
            End Get
            Set(ByVal value As Integer)
                _idOrdenRecepcion = value
            End Set
        End Property

        Public Property IdCaja() As Long
            Get
                Return _idCaja
            End Get
            Set(ByVal value As Long)
                _idCaja = value
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

        Public Property FechaCargue() As Date
            Get
                Return _fechaCargue
            End Get
            Set(ByVal value As Date)
                _fechaCargue = value
            End Set
        End Property

        Public Property Registrada() As Boolean
            Get
                Return _registrada
            End Get
            Protected Friend Set(ByVal value As Boolean)
                _registrada = value
            End Set
        End Property

#End Region

#Region "Estructuras"

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _serial = ""
            _region = ""
            _producto = ""
            _material = ""
            _descripcionMaterial = ""
            _lote = ""
        End Sub

        Public Sub New(ByVal serial As String)
            Me.New()
            Me.CargarDatos(serial)
        End Sub

#End Region

#Region "Metodos Privados"

        Private Sub CargarDatos(ByVal serial As String)
            Dim db As New LMDataAccess()
            Try
                db.SqlParametros.Add("@serial", SqlDbType.VarChar, 15).Value = serial
                db.ejecutarReader("ObtenerInfoTarjetaPrepago", CommandType.StoredProcedure)
                If db.Reader IsNot Nothing AndAlso db.Reader.Read Then
                    _idRegistro = CInt(db.Reader("idRegistro"))
                    _serial = db.Reader("serial").ToString()
                    Integer.TryParse(db.Reader("idRegion").ToString(), _idRegion)
                    _region = db.Reader("region").ToString
                    _idProducto = CInt(db.Reader("idProducto"))
                    _producto = db.Reader("producto").ToString
                    _material = db.Reader("material").ToString()
                    _descripcionMaterial = db.Reader("descripcionMaterial").ToString
                    _lote = db.Reader("lote").ToString()
                    Date.TryParse(db.Reader("fechaVencimiento").ToString(), _fechaVencimiento)
                    Date.TryParse(db.Reader("fechaRegistro").ToString(), _fechaRegistro)
                    Integer.TryParse(db.Reader("idOrdenRecepcion").ToString, _idOrdenRecepcion)
                    Date.TryParse(db.Reader("fechaRecepcion").ToString(), _fechaRecepcion)
                    Long.TryParse(db.Reader("idCaja").ToString, _idCaja)
                    Integer.TryParse(db.Reader("cantidad").ToString, _cantidad)
                    Date.TryParse(db.Reader("fechaCargue").ToString(), _fechaCargue)
                End If
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                db.Dispose()
            End Try
        End Sub

#End Region

#Region "Metodos Publicos"

        Public Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If _serial.Trim.Length > 0 AndAlso _idRegion > 0 AndAlso _idProducto > 0 AndAlso _material.Trim.Length > 0 _
                AndAlso _fechaVencimiento > Date.MinValue AndAlso _idOrdenRecepcion > 0 AndAlso _idCaja > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@lote", SqlDbType.VarChar, 12).Value = _serial
                        .SqlParametros.Add("@idRegion", SqlDbType.SmallInt).Value = _idRegion
                        .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = _idProducto
                        .SqlParametros.Add("@material", SqlDbType.VarChar, 10).Value = _material
                        .SqlParametros.Add("@fechaVencimiento", SqlDbType.SmallDateTime).Value = _fechaVencimiento
                        .SqlParametros.Add("@idOrdenRecepcion", SqlDbType.Int).Value = _idOrdenRecepcion
                        .SqlParametros.Add("@idCaja", SqlDbType.BigInt).Value = _idCaja
                        .SqlParametros.Add("@cantidadProducida", SqlDbType.Int).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .iniciarTransaccion()
                        .ejecutarNonQuery("RegistrarInfoTarjetaPrepago", CommandType.StoredProcedure)
                        Long.TryParse(.SqlParametros("@returnValue").Value.ToString, resultado.Valor)
                        If resultado.Valor = 0 Then
                            resultado.Mensaje = .SqlParametros("@cantidadProducida").Value.ToString
                            .confirmarTransaccion()
                        Else
                            .abortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    resultado.EstablecerMensajeYValor(7, "Error al tratar de registrar lote. " & ex.Message)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado.EstablecerMensajeYValor(8, "No se ha establecido el valor de uno o más campos requeridos. Por favor verifique")
            End If
            Return resultado
        End Function

        Public Function Actualizar()
        End Function

#End Region

#Region "Metodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New Estructuras.FiltroInfoTarjetaPrepago
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As Estructuras.FiltroInfoTarjetaPrepago) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdRegistro > 0 Then db.SqlParametros.Add("@idRegistro", SqlDbType.Int).Value = .IdRegistro
                If .Serial <> "" Then db.SqlParametros.Add("@serial", SqlDbType.VarChar).Value = .Serial
                If .IdRegion > 0 Then db.SqlParametros.Add("@idRegion", SqlDbType.Int).Value = .IdRegion
                If .Centro <> "" Then db.SqlParametros.Add("@centro", SqlDbType.VarChar).Value = .Centro
                If .IdProducto > 0 Then db.SqlParametros.Add("@idProducto", SqlDbType.Int).Value = .IdProducto
                If .Material <> "" Then db.SqlParametros.Add("@material", SqlDbType.VarChar).Value = .Material
                If .Lote <> "" Then db.SqlParametros.Add("@lote", SqlDbType.VarChar).Value = .Lote
                If .FechaVencimientoInicial > DateTime.MinValue Then db.SqlParametros.Add("@fechaVencimientoInicial", SqlDbType.DateTime).Value = .FechaVencimientoInicial
                If .FechaVencimientoFinal > DateTime.MinValue Then db.SqlParametros.Add("@fechaVencimientoFinal", SqlDbType.DateTime).Value = .FechaVencimientoFinal
                If .FechaRegistroInicial > DateTime.MinValue Then db.SqlParametros.Add("@fechaRegistroInicial", SqlDbType.DateTime).Value = .FechaRegistroInicial
                If .FechaRegistroFinal > DateTime.MinValue Then db.SqlParametros.Add("@fechaRegistroFinal", SqlDbType.DateTime).Value = .FechaRegistroFinal
                If .IdOrdenRecepcion > 0 Then db.SqlParametros.Add("@idOrdenRecepcion", SqlDbType.Int).Value = .IdOrdenRecepcion
                If .FechaCargueInicial > DateTime.MinValue Then db.SqlParametros.Add("@fechaCargueInicial", SqlDbType.DateTime).Value = .FechaCargueInicial
                If .FechaCargueFinal > DateTime.MinValue Then db.SqlParametros.Add("@fechaCargueFinal", SqlDbType.DateTime).Value = .FechaCargueFinal
                If .Cargado <> Enumerados.EstadoBinario.NoEstablecido Then db.SqlParametros.Add("@cargado", SqlDbType.SmallInt).Value = .Cargado
                dtDatos = db.ejecutarDataTable("ObtenerInfoTarjetaPrepago", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos
        End Function

#End Region

    End Class
End Namespace