Imports LMDataAccessLayer
Public Class AdministraciondeCambiodeEstadosCEM

#Region "Filtros de Búsqueda"

    Private _idRegistro As Integer
    Private _idTipoServicio As Integer
    Private _idEstadoInicial As Integer
    Private _idEstadoFinal As Integer
    Private _ValidaDisponibilidad As Boolean
    Private _ValidaCupos As Boolean
    Private _CargaInventario As Boolean
    Private _LiberaDisponibilidadInventario As Boolean
    Private _idUsuario As Integer
    Private _nombreEquipo As String
    
#End Region


#Region "Propiedades"
    Public Property IdRegistero As Integer
        Get
            Return _idRegistro
        End Get
        Set(value As Integer)
            _idRegistro = value
        End Set
    End Property
    Public Property IdTipoServicio As Integer
        Get
            Return _idTipoServicio
        End Get
        Set(value As Integer)
            _idTipoServicio = value
        End Set
    End Property

    Public Property IdEstadoInicial As Integer
        Get
            Return _idEstadoInicial
        End Get
        Set(value As Integer)
            _idEstadoInicial = value
        End Set
    End Property
    Public Property IdEstadoFinal As Integer
        Get
            Return _idEstadoFinal
        End Get
        Set(value As Integer)
            _idEstadoFinal = value
        End Set
    End Property

    Public Property ValidaDisponibilidad As Boolean
        Get
            Return _ValidaDisponibilidad
        End Get
        Set(value As Boolean)
            _ValidaDisponibilidad = value
        End Set
    End Property
    Public Property ValidaCupos As Boolean
        Get
            Return _ValidaCupos
        End Get
        Set(value As Boolean)
            _ValidaCupos = value
        End Set
    End Property
    Public Property CargaInventario As Boolean
        Get
            Return _CargaInventario
        End Get
        Set(value As Boolean)
            _CargaInventario = value
        End Set
    End Property
    Public Property LiberaDisponibilidadInventario As Boolean
        Get
            Return _LiberaDisponibilidadInventario
        End Get
        Set(value As Boolean)
            _LiberaDisponibilidadInventario = value
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


    Public Property NombreEquipo() As String
        Get
            Return _nombreEquipo
        End Get
        Set(ByVal value As String)
            _nombreEquipo = value
        End Set
    End Property

    
   
#End Region
#Region "Métodos Públicos"

    Public Function ConsultarEstadosServicios() As DataTable
        Dim dt As New DataTable
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    If (_idRegistro > 0) Then .Add("@idRegistro", SqlDbType.Int).Value = _idRegistro
                    If (_idUsuario > 0) Then .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    If (_idTipoServicio > 0) Then .Add("@idServicio", SqlDbType.VarChar).Value = _idTipoServicio
                End With
                dt = .EjecutarDataTable("ConsultarConfiguracionCambioEstadoMasivo", CommandType.StoredProcedure)
                Return dt
            Catch ex As Exception
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
    End Function
    Public Function CargarMaterialesComboTipomaterial(Material As String, startIndex As Integer, endIndex As Integer) As DataTable
        Dim dbManager As New LMDataAccess
        Dim Dtmateriales As New DataTable
        With dbManager
            If Not String.IsNullOrEmpty(Material) Then .SqlParametros.Add("@material", SqlDbType.VarChar).Value = String.Format("%{0}%", Material)
            If (startIndex > 0) Then .SqlParametros.Add("@startIndex", SqlDbType.Int).Value = startIndex
            If (endIndex > 0) Then .SqlParametros.Add("@endIndex", SqlDbType.Int).Value = endIndex
            .TiempoEsperaComando = 0
            Dtmateriales = .ejecutarDataTable("ObtenerMaterialTipoMatrial", CommandType.StoredProcedure)
        End With
        If dbManager IsNot Nothing Then dbManager.Dispose()
        Return Dtmateriales
    End Function
    Public Function ObtieneClasesSIM() As DataTable
        Dim dtDatos As DataTable
        Using dbManager As New LMDataAccess
            dtDatos = dbManager.ejecutarDataTable("ObtieneClasesSIM", CommandType.StoredProcedure)
        End Using
        Return dtDatos
    End Function
    Public Function RegistrarConfiguracionCambioEstado() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    If (_idRegistro > 0) Then .Add("@idRegistro", SqlDbType.Int).Value = _idRegistro
                    If (_idUsuario > 0) Then .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    If (_idTipoServicio > 0) Then .Add("@idTipoServicio", SqlDbType.VarChar).Value = _idTipoServicio
                    If (_idEstadoInicial > 0) Then .Add("@idEstadoInicial", SqlDbType.VarChar).Value = _idEstadoInicial
                    If (_idEstadoFinal > 0) Then .Add("@idEstadoFinal", SqlDbType.VarChar).Value = _idEstadoFinal
                    .Add("@ValidaDisponibilidad", SqlDbType.Bit).Value = _ValidaDisponibilidad
                    .Add("@ValidaCupos", SqlDbType.Bit).Value = _ValidaCupos
                    .Add("@CargaInventario", SqlDbType.Bit).Value = _CargaInventario
                    .Add("@LiberaDisponibilidadInventario", SqlDbType.Bit).Value = _LiberaDisponibilidadInventario
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                .ejecutarReader("RegistrarConfiguracionCambioEstadoMasivo", CommandType.StoredProcedure)
                If CInt(.SqlParametros("@resultado").Value) = 0 Then
                    resultado.EstablecerMensajeYValor(0, "La asignacion se realizo correctamente ")
                ElseIf CInt(.SqlParametros("@resultado").Value) = 1 Then
                    resultado.EstablecerMensajeYValor(1, "Ya esxite una comfiguracion igual para estado inicial estado final Tipo de Servicio.")
                Else
                    resultado.EstablecerMensajeYValor(10, "Se genero un error al realizar la Asignacion favor verificar")
                End If
            Catch ex As Exception
                resultado.EstablecerMensajeYValor(1, "Se genero un error al realizar la Asignacion favor verificar." & ex.Message)
                Throw New Exception(ex.Message, ex)
            End Try
        End With
        dbManager.Dispose()
        Return resultado
    End Function
    Public Function ConsultaTipoServicio() As DataTable
        Dim dtDatos As New DataTable

        Using _dbManager As New LMDataAccess
            With _dbManager
                If _idUsuario > 0 Then .SqlParametros.Add("@idUsuarioConsulta", SqlDbType.Int).Value = _idUsuario
                dtDatos = .ejecutarDataTable("ObtieneTipoServicioCEM", CommandType.StoredProcedure)
            End With
        End Using
        Return dtDatos
    End Function

    Public Function RegistrarCambioEstadoMasivo(ByVal dtUsuarioEjecutor As DataTable, ByRef resultado As Int32) As DataTable
        Dim dt As New DataTable
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .Add("@nombreEquipo", SqlDbType.NVarChar).Value = _nombreEquipo
                End With
                .EjecutarNonQuery("EliminaRegistroTrancitoriaActualizarCambioEstadoMasivo", CommandType.StoredProcedure)
                .InicilizarBulkCopy()
                .TiempoEsperaComando = 0
                With .BulkCopy
                    .DestinationTableName = "TrancitoriaActualizarCambioEstadoMasivo"
                    .ColumnMappings.Add("Fila", "Fila")
                    .ColumnMappings.Add("Radicado", "Radicado")
                    .ColumnMappings.Add("idJornada", "idJornada")
                    .ColumnMappings.Add("EstadoActual", "EstadoInicial")
                    .ColumnMappings.Add("NuevoEstado", "EstadoFinal")
                    .ColumnMappings.Add("FechaAgenda", "FechaAgenda")
                    .ColumnMappings.Add("FechaConfirmacion", "FechaConfirmacion")
                    .ColumnMappings.Add("FechaCierre", "FechaCierre")
                    .ColumnMappings.Add("Observacion", "Observacion")
                    .ColumnMappings.Add("Direccion", "Direccion")
                    .ColumnMappings.Add("nombreEquipo", "nombreEquipo")
                    .ColumnMappings.Add("idUsuario", "idUsuario")
                    .WriteToServer(dtUsuarioEjecutor)
                End With

                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .Add("@nombreEquipo", SqlDbType.NVarChar).Value = _nombreEquipo
                    .Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                .TiempoEsperaComando = 0
                dt = .EjecutarDataTable("EjecutarActualizacionCambioEstadoMasivo", CommandType.StoredProcedure)
                Dim resul As Integer = CType(.SqlParametros("@resultado").Value.ToString, Integer)
                If resul = 0 Then
                    resultado = 0
                    Return dt
                    Exit Function
                Else
                    resultado = 1
                    Return dt
                End If

            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
    End Function

#End Region
End Class
