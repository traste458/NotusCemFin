Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras

Namespace OMS


    Public Class EnvioPruebaSerial

#Region "Atributos"

        Private _idOrdenEnvioPrueba As Integer
        Private _idEnvioPruebaSerial As Integer
        Private _serial As String
        Private _idEstado As Integer
        Private _fechaRecibido As String
        Private _idUsuarioRecibe As Integer
        Private _estado As String
        Private _usuarioRecibe As String
        Private _detalle As DataTable
        Private _guia As String
        Private _factura As String

#End Region

#Region "Constructores"

        Sub New()
            MyBase.New()
        End Sub


        Sub New(ByVal _idEnvioPruebaSerial As Integer)
            MyBase.New()
            _idOrdenEnvioPrueba = IdEnvioPruebaSerial
            CargarInformacion()
        End Sub

#End Region


#Region "Propiedades"

        Public Property IdOrdenEnvioPrueba() As Integer
            Get
                Return _idOrdenEnvioPrueba
            End Get
            Set(ByVal value As Integer)
                _idOrdenEnvioPrueba = value
            End Set
        End Property

        Public Property IdEnvioPruebaSerial() As Integer
            Get
                Return IdEnvioPruebaSerial
            End Get
            Set(ByVal value As Integer)
                _idEnvioPruebaSerial = value
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

        Public Property IdEstado() As Integer
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Integer)
                _idEstado = value
            End Set
        End Property

        Public ReadOnly Property FechaRecibido() As String
            Get
                Return _fechaRecibido
            End Get
        End Property

        Public Property IdUsuarioRecibe() As Integer
            Get
                Return _idUsuarioRecibe
            End Get
            Set(ByVal value As Integer)
                _idUsuarioRecibe = value
            End Set
        End Property

        Public ReadOnly Property Estado() As String
            Get
                Return _estado
            End Get
        End Property

        Public ReadOnly Property UsuarioRecibe() As String
            Get
                Return _usuarioRecibe
            End Get
        End Property

        Public ReadOnly Property Detalle() As DataTable
            Get
                Return _detalle
            End Get
        End Property

        Public ReadOnly Property Factura() As String
            Get
                Return _factura
            End Get
        End Property

        Public ReadOnly Property Guia() As String
            Get
                Return _guia
            End Get
        End Property
#End Region

#Region "Metodos Privados"
        Private Overloads Sub CargarInformacion()
            Try
                Dim filtro As New FiltroEnvioPruebaSerial
                filtro.idEvioPruebaSerial = _idEnvioPruebaSerial
                _detalle = ListarSeriales(filtro)

            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

        End Sub

        Private Function EstructuraDetalle() As DataTable
            Dim dtEstructura As New DataTable
            Dim pk(0) As DataColumn
            With dtEstructura
                .Columns.Add("idOrdenEnvioPrueba", GetType(Integer))
                .Columns.Add("idOrdenTrabajo", GetType(Integer))
                .Columns.Add("serial", GetType(String))
                .Columns.Add("idEstado", GetType(Short))
                .Columns.Add("conNovedad", GetType(Boolean))
                pk(0) = .Columns("serial")
                .PrimaryKey = pk
            End With
            Return dtEstructura
        End Function

#End Region

#Region "Metodos Publicos"

        Public Function Crear(ByVal db As LMDataAccess) As Boolean
            Dim resultado As Boolean
            With db
                .SqlParametros.Clear()
                .SqlParametros.Add("@idOrdenEnvioPrueba", SqlDbType.Int).Value = _idOrdenEnvioPrueba
                resultado = .ejecutarNonQuery("CrearEnvioPruebaDetalleSerial", CommandType.StoredProcedure)
            End With
            Return resultado
        End Function
        Public Function Actualizar() As Boolean
            Dim dbManager As New LMDataAccess
            Dim resultado As Boolean
            With dbManager
                If _idOrdenEnvioPrueba <> 0 Then .SqlParametros.Add("@idOrdenEnvioPrueba", SqlDbType.Int).Value = _idOrdenEnvioPrueba
                If _serial IsNot Nothing AndAlso _serial <> "" Then .SqlParametros.Add("@serial", SqlDbType.VarChar, 20).Value = _serial
                If _idEstado <> 0 Then .SqlParametros.Add("@idEstado", SqlDbType.SmallInt).Value = _idEstado
                If _idUsuarioRecibe <> 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuarioRecibe
                resultado = .ejecutarNonQuery("ActualizarEnvioPruebaDetalleSerial", CommandType.StoredProcedure)
            End With
            Return resultado
        End Function

        Public Function Eliminar(ByVal db As LMDataAccess) As Boolean

        End Function

        Public Sub Adicionar(ByVal db As LMDataAccess)
        End Sub

        Public Function ObtenerSerialesPorEnvio(ByVal idOrdenEnvioPrueba As Integer) As DataTable
            Dim filtro As New FiltroEnvioPruebaSerial
            Dim dtDatos As DataTable
            filtro.idOrdenEnvioPrueba = idOrdenEnvioPrueba
            dtDatos = ListarSeriales(filtro)
            Return dtDatos
        End Function

        Public Function ObtenerSerialesPorSerial(ByVal serial As String) As DataTable
            Dim filtro As New FiltroEnvioPruebaSerial
            Dim dtDatos As DataTable
            filtro.serial = serial
            dtDatos = ListarSeriales(filtro)
            Return dtDatos
        End Function

        Public Function ListarSeriales(ByVal filtro As FiltroEnvioPruebaSerial) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDatos As New DataTable
            Try
                With dbManager
                    If filtro.idOrdenEnvioPrueba <> 0 Then .SqlParametros.Add("@idOrdenEnvioPrueba", SqlDbType.Int).Value = filtro.idOrdenEnvioPrueba
                    If filtro.idEvioPruebaSerial <> 0 Then .SqlParametros.Add("@idEnvioPruebaSerial", SqlDbType.Int).Value = filtro.idEvioPruebaSerial
                    If filtro.idEstado <> 0 Then .SqlParametros.Add("@idEstado", SqlDbType.SmallInt).Value = filtro.idEstado
                    If filtro.serial IsNot Nothing AndAlso filtro.serial.Trim.Length > 0 Then .SqlParametros.Add("@serial", SqlDbType.VarChar, 20).Value = filtro.serial
                    dtDatos = .ejecutarDataTable("ObtenerSerialesEnvioPrueba", CommandType.StoredProcedure)
                End With
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Function CrearDetalleParaPedido(ByVal idEnvio As Integer) As DataTable
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Try
                dbManager.agregarParametroSQL("@idEnvioPrueba", idEnvio, SqlDbType.Int)
                Return dbManager.ejecutarDataTable("ObtenerDetallePedidoProductoPruebas", CommandType.StoredProcedure)
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

        End Function
#End Region

#Region "Metodos Compartidos"


#End Region
 

    End Class
End Namespace