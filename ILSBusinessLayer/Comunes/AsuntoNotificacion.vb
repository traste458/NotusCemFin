Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Enumerados
Imports LMDataAccessLayer

Namespace Comunes

    Public Class AsuntoNotificacion

#Region "Atributos"

        Private _idAsuntoNotificacion As Integer
        Private _nombre As String
        Private _estado As Short
        Private _idUsuarioCreacion As Integer
        Private _fechaCreacion As Date
        Private _usuarioCreacion As String
        Private _idPerfil As Integer

        'Colecciones
        Private _listUsuarios As UsuarioNotificacionColeccion
        Private _listDetalleAsunto As DetalleAsuntoNotificacionColeccion

#End Region

#Region "Propiedades"

        Public Property IdUsuarioNotificacion() As Integer
            Get
                Return _idAsuntoNotificacion
            End Get
            Set(ByVal value As Integer)
                _idAsuntoNotificacion = value
            End Set
        End Property

        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        Public Property Estado() As Short
            Get
                Return _estado
            End Get
            Set(ByVal value As Short)
                _estado = value
            End Set
        End Property

        Public Property UsuarioCreacion() As String
            Get
                Return _usuarioCreacion
            End Get
            Set(ByVal value As String)
                _usuarioCreacion = value
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

        Public Property IdUsuarioCreacion() As Integer
            Get
                Return _idUsuarioCreacion
            End Get
            Set(ByVal value As Integer)
                _idUsuarioCreacion = value
            End Set
        End Property

        Public Property IdPerfil() As Integer
            Get
                Return _idPerfil
            End Get
            Set(ByVal value As Integer)
                _idPerfil = value
            End Set
        End Property

        Public ReadOnly Property ListaUsuarios As UsuarioNotificacionColeccion
            Get
                If _listUsuarios Is Nothing OrElse _listUsuarios.Count = 0 Then _listUsuarios = New UsuarioNotificacionColeccion(Me._idAsuntoNotificacion)
                Return _listUsuarios
            End Get
        End Property

        Public ReadOnly Property ListaDetalleAsunto As DetalleAsuntoNotificacionColeccion
            Get
                If _listDetalleAsunto Is Nothing OrElse _listDetalleAsunto.Count = 0 Then _listDetalleAsunto = New DetalleAsuntoNotificacionColeccion(Me._idAsuntoNotificacion)
                Return _listDetalleAsunto
            End Get
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            _idAsuntoNotificacion = identificador
            CargarInformacion()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            If _idAsuntoNotificacion > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idAsuntoNotificacion", SqlDbType.Int).Value = _idAsuntoNotificacion
                        .ejecutarReader("ObtenerInfoAsuntoNotificacion", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing AndAlso .Reader.Read Then
                            _nombre = .Reader("nombre").ToString
                            Short.TryParse(.Reader("estado").ToString, _estado)
                            _usuarioCreacion = .Reader("usuarioCreacion").ToString
                            Date.TryParse(.Reader("fechaCreacion").ToString, _fechaCreacion)
                            .Reader.Close()
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As Short

        End Function

        Public Function Actualizar() As Short

        End Function

#End Region
#Region "Métodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroAsuntoNotificacion
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroAsuntoNotificacion) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            Try
                With filtro
                    If .IdAsuntoNotificacion > 0 Then db.SqlParametros.Add("@idAsuntoNotificacion", SqlDbType.Int).Value = .IdAsuntoNotificacion
                    If .Nombre IsNot Nothing AndAlso .Nombre.Trim.Length > 0 Then _
                        db.SqlParametros.Add("@nombre", SqlDbType.VarChar, 100).Value = .Nombre.ToString
                    If .Estado > 0 Then db.SqlParametros.Add("@idAsuntoNotificacion", SqlDbType.Bit).Value = .Estado
                    If .IdUsuarioCreacion > 0 Then db.SqlParametros.Add("@idUsuarioCreacion", SqlDbType.Int).Value = .IdUsuarioCreacion
                    If .IdPerfil > 0 Then db.SqlParametros.Add("@idPerfil", SqlDbType.Int).Value = .IdPerfil

                    dtDatos = db.EjecutarDataTable("ObtenerInfoAsuntoNotificacion", CommandType.StoredProcedure)
                    Return dtDatos
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
            Return dtDatos
        End Function

#End Region

#Region "Enumeraciones"

        Public Enum Tipo
            EnvioLectura = 1
            EnvioPrueba = 2
            CreacionInstruccion = 3
            DiferenciasOrdenInventario = 4
            MaterialEnCuarentena = 5
            TransportadoraDespacho = 8
            ValorMaterialDespacho = 9
            LiberacionMaterial = 10
            NacionalizacionProducto = 12
            AutorizacionCambioSoftware = 13
            NotificaciónInstrucciónReproceso = 14
            NotificaciónEnvioLecturaReproceso = 15
            Notificación_Solución_Novedad_POP = 16
            NotificacionCreacionCampaniaPOP = 17
            NotificacionCreacionDistribucionPOP = 18
            NotificacionCreacionInstruccionPOP = 19
            NotificacionCierreDespachoPOP = 20
            NotificacionRecepcionProducto = 21
            SinDisponibilidadInventario = 22
            NotificaciónVencimientoSiembra = 23
            DiferenciaVersionMaterial = 24
            NotificaciónNuevosProductos = 25
            CreacionpedidospapeleriasincargueSAP = 26
            AutorizacionPedidosSinCargueSAP = 27
            ReporteModificacióndeVentas = 28
            RecepcionProductoPapeleria = 29
            Cancelacionpedidopapeleria = 30
            ReporteRecepcionProducto = 31
            ReporteEliminaciónVentas = 32
            NotificacionPedidoServicioTecnico = 33
			Notificación_Entrega_Servicio_Siembra = 34
            Notificación_Devolución_Servicio_Siembra = 35
            Notificación_Novedad_Servicio_Siembra = 36
            ReporteMaterialPOPSinInstruccion = 37
            Notificación_Registro_de_Cliente_en_Lista_Negra = 38
            Notificación_Creación_Servicio_Corporativo = 39
		End Enum
#End Region

    End Class

End Namespace