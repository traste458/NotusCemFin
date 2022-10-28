Imports LMDataAccessLayer
    Public Class AdministracionZonaUsuario


#Region "Filtros de Búsqueda"

    Private _usuario As String
#End Region


#Region "Propiedades"

    Public Property Usuario As String
        Get
            Return _usuario
        End Get
        Set(value As String)
            _usuario = value
        End Set
    End Property

#End Region

#Region "Métodos Públicos"

    Public Function CargarUsuariosParaAsignacionZona(Usuario As String, startIndex As Integer, endIndex As Integer) As DataTable
        Dim dbUsuario As New LMDataAccess
        Dim Dtusuario As New DataTable
        With dbUsuario
            If Not String.IsNullOrEmpty(Usuario) Then .SqlParametros.Add("@usuario", SqlDbType.VarChar).Value = String.Format("%{0}%", Usuario)
            If (startIndex > 0) Then .SqlParametros.Add("@startIndex", SqlDbType.Int).Value = startIndex
            If (endIndex > 0) Then .SqlParametros.Add("@endIndex", SqlDbType.Int).Value = endIndex
            .TiempoEsperaComando = 0
            Dtusuario = .ejecutarDataTable("ObtenerUsuarioZona", CommandType.StoredProcedure)
        End With
        If Dtusuario IsNot Nothing Then Dtusuario.Dispose()
        Return Dtusuario
    End Function

    Public Function ConsultarZona(idUsuario As String) As DataTable
        Dim dbZona As New LMDataAccess
        Dim DtZona As New DataTable
        With dbZona
            .SqlParametros.Add("@identificacion", SqlDbType.VarChar).Value = idUsuario
            DtZona = .ejecutarDataTable("ObtieneZonaParaMotorizado", CommandType.StoredProcedure)
        End With
        If DtZona IsNot Nothing Then DtZona.Dispose()
        Return DtZona
    End Function

    Public Function ConsultarTipoServicio(idUsuario As String) As DataTable
        Dim dbTipoServicio As New LMDataAccess
        Dim DtTipoServicio As New DataTable
        With dbTipoServicio
            .SqlParametros.Add("@identificacion", SqlDbType.VarChar).Value = idUsuario
            DtTipoServicio = .ejecutarDataTable("ObtieneTipoServicioUsuarioMotorizado", CommandType.StoredProcedure)
        End With
        If DtTipoServicio IsNot Nothing Then DtTipoServicio.Dispose()
        Return DtTipoServicio
    End Function

    Public Function ModificacionMotorizado(identificacion As String, idCiudad As Integer, ListaIdZona As String, telefono As String, ListaTipoServicio As String, placa As String) As DataTable
        Dim dbMotorizado As New LMDataAccess
        Dim DtMotorizado As New DataTable
        With dbMotorizado
            If identificacion <> "" Then .SqlParametros.Add("@identificacion", SqlDbType.VarChar).Value = identificacion
            If idCiudad > 0 Then .SqlParametros.Add("@idciudad", SqlDbType.Int).Value = idCiudad
            If telefono <> "" Then .SqlParametros.Add("@telefono", SqlDbType.VarChar).Value = telefono
            If placa <> "" Then .SqlParametros.Add("@placa", SqlDbType.VarChar).Value = placa
            If ListaTipoServicio <> "" Then .SqlParametros.Add("@listaTipoServicio", SqlDbType.VarChar).Value = ListaTipoServicio
            If ListaIdZona <> "" Then .SqlParametros.Add("@listaZona", SqlDbType.VarChar).Value = ListaIdZona
            DtMotorizado = .ejecutarDataTable("ActualizarZonasDeLosMotorizados", CommandType.StoredProcedure)
        End With
        If DtMotorizado IsNot Nothing Then DtMotorizado.Dispose()
        Return DtMotorizado
    End Function

#End Region
    End Class
