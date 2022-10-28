Public Class PlanillaRadicacion

#Region "Atributos"
    Private _idUsuario As Integer
    Private _radicado As Long
    Private _idCiudad As Integer
    Private _precinto As String
    Private _observaciones As String
    Private _idPlanilla As Integer
    Private _errores As DataTable
    Private _radicados As DataTable
#End Region

#Region "properties"
    Public Property IdPlanilla As Integer
        Get
            Return _idPlanilla
        End Get
        Set(value As Integer)
            _idPlanilla = value
        End Set
    End Property
    Public Property IdCiudad As Integer
        Get
            Return _idCiudad
        End Get
        Set(value As Integer)
            _idCiudad = value
        End Set
    End Property
    Public Property Precinto As String
        Get
            Return _precinto
        End Get
        Set(value As String)
            _precinto = value
        End Set
    End Property
    Public Property Observaciones As String
        Get
            Return _observaciones
        End Get
        Set(value As String)
            _observaciones = value
        End Set
    End Property
    Public Property IdUsuario As Integer
        Get
            Return _idUsuario
        End Get
        Set(value As Integer)
            _idUsuario = value
        End Set
    End Property

    Public Property Radicado As Long
        Get
            Return _radicado
        End Get
        Set(value As Long)
            _radicado = value
        End Set
    End Property
    Public Property Radicados As DataTable
        Get
            Return _radicados
        End Get
        Set(value As DataTable)
            _radicados = value
        End Set
    End Property
    Public Property Errores As DataTable
        Get
            Return _errores
        End Get
        Set(value As DataTable)
            _errores = value
        End Set
    End Property

#End Region

#Region "public properties"
    Public Function RegistrarRadicadoTransitorio(Optional tipo As Integer = 1) As ResultadoProceso
        Dim resultado As New ResultadoProceso(0, "Exito")
        Try
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim dt As New DataTable
            db.SqlParametros.Clear()
            If IdUsuario > 0 Then db.SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
            If Radicado > 0 Then db.SqlParametros.Add("@radicado", SqlDbType.BigInt).Value = Radicado
            If tipo <> 1 Then db.SqlParametros.Add("@tipoR", SqlDbType.Int).Value = tipo
            dt = db.EjecutarDataTable("InsertarTransitoriaPlanillaRadicacion", CommandType.StoredProcedure)
            Errores = dt
            If Errores.Rows.Count <> 0 Then
                resultado.EstablecerMensajeYValor(-1, "Error al registar radicado")
            End If
        Catch ex As Exception
            resultado = New ResultadoProceso(-1, "Error al procesar peticion: " & ex.Message)
        End Try
        Return resultado
    End Function
    Public Function CargarRegistrosRadicadoTransitorios(Optional tipoCargue As Integer = 1) As ResultadoProceso
        Dim resultado As New ResultadoProceso(0, "Exito")
        Try
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim dt As New DataTable
            db.SqlParametros.Clear()
            If IdUsuario > 0 Then db.SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
            If tipoCargue > 1 Then db.SqlParametros.Add("@tipoCargue", SqlDbType.Int).Value = tipoCargue
            dt = db.EjecutarDataTable("ObtenerInfoPlanillaTransitoria", CommandType.StoredProcedure)
            Radicados = dt
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(-1, "Error al obtener informacion de radicados: " & ex.Message)
        End Try
        Return resultado
    End Function
    Public Function BorrarResgistrosRadicadoTransitorios() As ResultadoProceso
        Dim resultado As New ResultadoProceso(0, "Exito")
        Try
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim dt As New DataTable
            db.SqlParametros.Clear()
            If IdUsuario > 0 Then db.SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
            If Radicado > 0 Then db.SqlParametros.Add("@radicado", SqlDbType.BigInt).Value = Radicado
            db.EjecutarScalar("BorrarInfoPlantillaTransitoria", CommandType.StoredProcedure)
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(-1, "Error al borrar registros: " & ex.Message)
        End Try
        Return resultado
    End Function
    Public Function RegistrarPlanilla() As ResultadoProceso
        Dim resultado As New ResultadoProceso(0, "Exito")
        Try
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.SqlParametros.Clear()
            If IdUsuario > 0 Then db.SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
            db.SqlParametros.Add("@idCliente", SqlDbType.BigInt).Value = Enumerados.ClienteExterno.DAVIVIENDA
            If IdCiudad > 0 Then db.SqlParametros.Add("@ciudad", SqlDbType.Int).Value = IdCiudad
            If Not String.IsNullOrEmpty(Precinto) Then db.SqlParametros.Add("@precinto", SqlDbType.VarChar).Value = Precinto
            If Not String.IsNullOrEmpty(Observaciones) Then db.SqlParametros.Add("@observaciones", SqlDbType.VarChar).Value = Observaciones
            db.SqlParametros.Add("@idPlanilla", SqlDbType.Int).Direction = ParameterDirection.Output
            db.EjecutarScalar("GenerarPlanillaRadicacion", CommandType.StoredProcedure)
            _idPlanilla = db.SqlParametros("@idPlanilla").Value
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(-1, "Error al registrar planilla: " & ex.Message)
        End Try
        Return resultado
    End Function
    Public Function CargarDatosPlanilla() As DataSet
        Dim ds As New DataSet
        Try
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.SqlParametros.Clear()
            If IdPlanilla > 0 Then db.SqlParametros.Add("@idPlanilla", SqlDbType.Int).Value = IdPlanilla
            If IdUsuario > 0 Then db.SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
            If Radicado > 0 Then db.SqlParametros.Add("@radicado", SqlDbType.BigInt).Value = Radicado
            ds = db.EjecutarDataSet("ObtenerPlanillaDatos", CommandType.StoredProcedure)
        Catch ex As Exception

        End Try
        Return ds
    End Function
#End Region

End Class
