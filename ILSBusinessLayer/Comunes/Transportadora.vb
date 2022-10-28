Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Enumerados

Public Class Transportadora

#Region "Atributos"
    Private _idTransportadora As Integer
    Private _nombre As String
    Private _estado As Boolean
    Private _usaGuia As Boolean
    Private _usaPrecintos As Boolean
    Private _aplicaLogisticaInversa As Boolean
    Private _cargaPorImportacion As Integer
    Private _manejaPOS As String
    Private _aplicaDespachoNacional As Boolean
    Private _registrado As Boolean
#End Region

#Region "Propiedades"

    Public Property IdTransportadora() As Integer
        Get
            Return _idTransportadora
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idTransportadora = value
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

    Public Property Estado() As Boolean
        Get
            Return _estado
        End Get
        Set(ByVal value As Boolean)
            _estado = value
        End Set
    End Property

    Public Property UsaGuia() As Boolean
        Get
            Return _usaGuia
        End Get
        Set(ByVal value As Boolean)
            _usaGuia = value
        End Set
    End Property

    Public Property UsaPrecintos() As Boolean
        Get
            Return _usaPrecintos
        End Get
        Set(ByVal value As Boolean)
            _usaPrecintos = value
        End Set
    End Property

    Public Property AplicaLogisticaInversa() As Boolean
        Get
            Return _aplicaLogisticaInversa
        End Get
        Set(ByVal value As Boolean)
            _aplicaLogisticaInversa = value
        End Set
    End Property

    Public Property CargaPorImportacion() As Integer
        Get
            Return _cargaPorImportacion
        End Get
        Set(ByVal value As Integer)
            _cargaPorImportacion = value
        End Set
    End Property

    Public Property AplicaDespachoNacional() As Boolean
        Get
            Return _aplicaDespachoNacional
        End Get
        Set(ByVal value As Boolean)
            _aplicaDespachoNacional = value
        End Set
    End Property

    Public Property Registrado() As Boolean
        Get
            Return _registrado
        End Get
        Protected Friend Set(ByVal value As Boolean)
            _registrado = value
        End Set
    End Property

#End Region

#Region "Constructores"
    Public Sub New()
        _idTransportadora = 0
        _nombre = ""
        _estado = 0
        _usaGuia = False
        _usaPrecintos = False
        _aplicaLogisticaInversa = False
        _cargaPorImportacion = -1
        _manejaPOS = ""
    End Sub

    Public Sub New(ByVal idTransportadora As Integer)
        Me.New()
        Me.SeleccionarPorID(idTransportadora)
    End Sub
#End Region

#Region "Metodos Amigos"

    ''' <summary>
    ''' Selecciona todas las transportadoras registradas en la base de datos que cumplan con los filtros especificados
    ''' </summary>
    ''' <param name="filtro">Filtro de búsqueda</param>
    ''' <returns>Datatable con los datos de transportadoras que cumplan con los filtros especificados</returns>
    ''' <remarks></remarks>
    Public Shared Function ListadoTransportadoras(ByVal filtro As FiltroTransportadora) As DataTable
        Dim resultado As New DataTable
        Dim adminBD As New LMDataAccessLayer.LMDataAccess

        Try

            With adminBD
                If filtro.IdTransportadora <> 0 Then .agregarParametroSQL("@idTransportadora", filtro.IdTransportadora, SqlDbType.Int)
                If filtro.UsaGuia <> EstadoBinario.NoEstablecido Then .agregarParametroSQL("@usaGuia", filtro.UsaGuia, SqlDbType.Bit)
                If filtro.UsaPrecinto <> EstadoBinario.NoEstablecido Then .agregarParametroSQL("@usaPrecinto", filtro.UsaGuia, SqlDbType.Bit)
                If filtro.AplicaLogisticaInversa <> EstadoBinario.NoEstablecido Then
                    If filtro.AplicaLogisticaInversa = 1 Then
                        .agregarParametroSQL("@aplicaLogisticaInversa", True, SqlDbType.Bit)
                    Else
                        .agregarParametroSQL("@aplicaLogisticaInversa", False, SqlDbType.Bit)
                    End If
                End If

                If filtro.CargaPorImportacion <> 0 Then .agregarParametroSQL("@cargaPorImportacion", filtro.CargaPorImportacion, SqlDbType.SmallInt)
                If filtro.Activo <> EstadoBinario.NoEstablecido Then .agregarParametroSQL("@estado", filtro.Activo, SqlDbType.Bit)
                If filtro.ManejaPos IsNot Nothing AndAlso filtro.ManejaPos <> "" Then _
                    .agregarParametroSQL("@manejaPOS", filtro.ManejaPos, SqlDbType.VarChar)
                If filtro.IdTipoTransporte <> 0 Then .agregarParametroSQL("@idTipoTransporte", filtro.IdTipoTransporte, SqlDbType.Int)

                resultado = .ejecutarDataTable("SeleccionarTransportadoras", CommandType.StoredProcedure)
            End With

        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar listado de transportadoras: " & ex.Message)
        Finally
            adminBD.Dispose()
        End Try

        Return resultado
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ListadoTipoTransporte() As DataTable
        Dim respuesta As New DataTable
        Dim adminBD As New LMDataAccessLayer.LMDataAccess

        Try
            respuesta = adminBD.ejecutarDataTable("SeleccionarTiposTransporte", CommandType.StoredProcedure)
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar listado de tipos de transporte: " & ex.Message)
        Finally
            adminBD.Dispose()
        End Try

        Return respuesta
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ListadoTipoProductos() As DataTable
        Dim respuesta As New DataTable
        Dim adminBD As New LMDataAccessLayer.LMDataAccess

        Try
            respuesta = adminBD.ejecutarDataTable("SeleccionarTiposProducto", CommandType.StoredProcedure)
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar listado de tipos de producto: " & ex.Message)
        Finally
            adminBD.Dispose()
        End Try

        Return respuesta
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ListadoTipoDestinatarios() As DataTable
        Dim respuesta As New DataTable
        Dim adminBD As New LMDataAccessLayer.LMDataAccess

        Try
            respuesta = adminBD.ejecutarDataTable("SeleccionarTiposDestinatario", CommandType.StoredProcedure)
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar listado de tipos de destinatarios: " & ex.Message)
        Finally
            adminBD.Dispose()
        End Try

        Return respuesta
    End Function


    Public Shared Function ListadoMovimientosTransporte() As DataTable
        Dim respuesta As New DataTable
        Dim adminBD As New LMDataAccessLayer.LMDataAccess

        Try
            respuesta = adminBD.ejecutarDataTable("SeleccionarTiposMovimientoTransporte", CommandType.StoredProcedure)
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar listado de tipos de alistamiento: " & ex.Message)
        Finally
            adminBD.Dispose()
        End Try

        Return respuesta
    End Function

    Public Shared Function ListadoTipoServicio() As DataTable
        Dim respuesta As New DataTable
        Dim adminBD As New LMDataAccessLayer.LMDataAccess
        Try
            respuesta = adminBD.ejecutarDataTable("ObtenerTipoServicio", CommandType.StoredProcedure)
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar listado de tipos de servico de transportadora : " & ex.Message)
        Finally
            adminBD.Dispose()
        End Try

        Return respuesta
    End Function

    ''' <summary>
    ''' Selecciona las transportadoras que cumplan con un determinado tipo de transporte de acuerdo a la información que se encuentre en la matriz de transporte
    ''' </summary>
    ''' <param name="idTipoTransporte">Número identificador del tipo de transporte para el cual se desea filtrar la información
    ''' 1 - Terrestre
    ''' 2 - Aéreo
    ''' 3 - Especial</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function BuscarPorTipoTransporte(ByVal idTipoTransporte As Integer) As DataTable
        Dim adminBD As New LMDataAccessLayer.LMDataAccess
        Dim respuesta As New DataTable

        Try
            If idTipoTransporte <> 0 Then adminBD.agregarParametroSQL("@idTipoTransporte", idTipoTransporte)
            respuesta = adminBD.ejecutarDataTable("BuscarTransportadorasPorTipoTransporte", CommandType.StoredProcedure)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return respuesta
    End Function


    ''' <summary>
    ''' Registra en la base de datos un rango de guías específico para una transportadora
    ''' </summary>
    ''' <param name="idTransportadora"> Identificador de la transportadora a la que se le asignará el rango </param>
    ''' <param name="guiaInicial"> Número de guía inicial en el rango </param>
    ''' <param name="guiaFinal"> Número de guía final en el rango </param>
    ''' <remarks></remarks>
    Public Shared Sub RegistrarRangoGuias(ByVal idTransportadora As Integer, ByVal guiaInicial As String, ByVal guiaFinal As String)
        Dim adminBD As New LMDataAccessLayer.LMDataAccess
        Dim respuesta As Integer = -1

        Try
            adminBD.agregarParametroSQL("@idTransportadora", idTransportadora)
            adminBD.agregarParametroSQL("@guiaInicial", guiaInicial, SqlDbType.VarChar)
            adminBD.agregarParametroSQL("@guiaFinal", guiaFinal, SqlDbType.VarChar)
            adminBD.SqlParametros.Add("@return_value", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

            adminBD.ejecutarNonQuery("RegistrarGuiasTransportadora", CommandType.StoredProcedure)

            respuesta = adminBD.SqlParametros("@return_value").Value


            If respuesta = -1 Then
                Throw New Exception("Ocurrió un error recuperando los valores de retorno de la base de datos")
            ElseIf respuesta = 1 Then
                Throw New Exception("La transportadora indicada ya tiene un rango de guías asignado que aún no ha sido utilizado en su totalidad")
            ElseIf respuesta = 2 Then
                Throw New Exception("La guía inicial proporcionada esta dentro del rango actual y ya fue utilizada.")
            ElseIf respuesta = 3 Then
                Throw New Exception("La guía final proporcionada esta dentro del rango actual y ya fue utilizada.")
            ElseIf respuesta = 4 Then
                Throw New Exception("La guía inicial proporcionada pertenece a un rango anterior, el mismo ya fue utilizado.")
            ElseIf respuesta = 5 Then
                Throw New Exception("La guía final proporcionada pertenece a un rango anterior, el mismo ya fue utilizado.")
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            adminBD.Dispose()
        End Try
    End Sub

#End Region

#Region "Métodos Privados"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="idTransportadora"></param>
    ''' <remarks></remarks>
    Private Sub SeleccionarPorID(ByVal idTransportadora As Integer)
        Dim adminBD As New LMDataAccessLayer.LMDataAccess
        


        Try
            adminBD.agregarParametroSQL("@idTransportadora", idTransportadora, SqlDbType.Int)
            With adminBD
                adminBD.ejecutarReader("SeleccionarTransportadoras", CommandType.StoredProcedure)
                While adminBD.Reader.Read()

                    Me._idTransportadora = CInt(adminBD.Reader("idTransportadora").ToString)
                    Me._nombre = adminBD.Reader("transportadora").ToString
                    Me._estado = CInt(adminBD.Reader("estado").ToString)
                    Me._usaGuia = CBool(adminBD.Reader("usaGuia").ToString)
                    Me._usaPrecintos = CBool(adminBD.Reader("usaPrecinto").ToString)
                    Me._aplicaLogisticaInversa = CBool(adminBD.Reader("aplicaLogisticaInversa").ToString)
                    Me._cargaPorImportacion = CInt(adminBD.Reader("cargaPorImportacion").ToString)
                    Me._manejaPOS = adminBD.Reader("maneja_pos").ToString
                End While
            End With
        Finally
            adminBD.Dispose()
        End Try
    End Sub
#End Region

End Class
