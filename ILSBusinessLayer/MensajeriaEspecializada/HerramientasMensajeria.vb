Imports LMDataAccessLayer
Imports System.Web
Imports GemBox.Spreadsheet
Imports System.Drawing

Namespace MensajeriaEspecializada

    Public Module HerramientasMensajeria

#Region "Métodos Públicos"
        Public Function ConsultaTipoServicioActivos() As DataTable
            Dim dtDatos As New DataTable

            Using _dbManager As New LMDataAccess
                With _dbManager
                    dtDatos = .EjecutarDataTable("ObtieneTipoServicioActivos", CommandType.StoredProcedure)
                End With
            End Using

            Return dtDatos
        End Function
        Public Function ConsultarEstado() As DataTable
            Dim _dbManager As New LMDataAccess
            Dim dtDatos As New DataTable

            Try
                With _dbManager
                    .TiempoEsperaComando = 0
                    dtDatos = .EjecutarDataTable("ConsultarEstado", CommandType.StoredProcedure)
                End With
            Finally
                If _dbManager IsNot Nothing Then _dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Function ConsultarEstadoMesaControl() As DataTable
            Dim _dbManager As New LMDataAccess
            Dim dtDatos As New DataTable

            Try
                With _dbManager
                    .TiempoEsperaComando = 0
                    dtDatos = .EjecutarDataTable("ConsultarEstadoMesaControl", CommandType.StoredProcedure)
                End With
            Finally
                If _dbManager IsNot Nothing Then _dbManager.Dispose()
            End Try
            Return dtDatos
        End Function
        Public Function ConsultarEstado(ByVal idEntidad As Enumerados.Entidad) As DataTable
            Dim _dbManager As New LMDataAccess
            Dim dtDatos As New DataTable

            Try
                With _dbManager
                    .TiempoEsperaComando = 0
                    .SqlParametros.Add("@idEntidad", SqlDbType.Int).Value = idEntidad
                    dtDatos = .EjecutarDataTable("ConsultarEstadoEntidad", CommandType.StoredProcedure)
                End With
            Finally
                If _dbManager IsNot Nothing Then _dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Function ConsultarEstadoReapertura(ByVal idEstadoActual As Integer)
            Dim _dbManager As New LMDataAccess
            Dim dtDatos As New DataTable

            Try
                With _dbManager
                    .TiempoEsperaComando = 0
                    .SqlParametros.Add("@idEstadoActual", SqlDbType.Int).Value = idEstadoActual
                    dtDatos = .EjecutarDataTable("ObtenerEstadosReaperturaServicioMensajeria", CommandType.StoredProcedure)
                End With
            Finally
                If _dbManager IsNot Nothing Then _dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Function ConsultarBodega(Optional ByVal idCiudad As Integer = -1, Optional ByVal idUsuarioConsulta As Integer = 0) As DataTable
            Dim _dbManager As New LMDataAccess
            Dim dtDatos As New DataTable

            Try
                With _dbManager
                    .TiempoEsperaComando = 0
                    If idCiudad > -1 And idCiudad > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = idCiudad
                    If idUsuarioConsulta > 0 Then .SqlParametros.Add("@idUsuarioConsulta", SqlDbType.Int).Value = idUsuarioConsulta
                    dtDatos = .EjecutarDataTable("ConsultarBodega", CommandType.StoredProcedure)
                End With
            Finally
                If _dbManager IsNot Nothing Then _dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Function ConsultarCiudadCampania(ByVal idCampania As Integer) As DataTable
            Dim _dbManager As New LMDataAccess
            Dim dtDatos As New DataTable

            Try
                With _dbManager
                    .TiempoEsperaComando = 0
                    .SqlParametros.Add("@idCampania", SqlDbType.Int).Value = idCampania
                    dtDatos = .EjecutarDataTable("ObtenerInfoCiudad", CommandType.StoredProcedure)
                End With
            Finally
                If _dbManager IsNot Nothing Then _dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Function ConsultarBaseCliente() As DataTable
            Dim _dbManager As New LMDataAccess
            Dim dtDatos As New DataTable

            Try
                With _dbManager
                    .TiempoEsperaComando = 0
                    dtDatos = .EjecutarDataTable("ConsultarBaseClientes", CommandType.StoredProcedure)
                End With
            Finally
                If _dbManager IsNot Nothing Then _dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Function ConsultaJornadaMensajeria() As DataTable
            Dim dtDatos As New DataTable

            Using _dbManager As New LMDataAccess
                With _dbManager
                    .TiempoEsperaComando = 0
                    dtDatos = .EjecutarDataTable("ObtieneJornadaMensajeria", CommandType.StoredProcedure)
                End With
            End Using

            Return dtDatos
        End Function

        Public Function ConsultaClientesEmpresa() As DataTable
            Dim dtDatos As New DataTable

            Using _dbManager As New LMDataAccess
                With _dbManager
                    .TiempoEsperaComando = 0
                    dtDatos = .EjecutarDataTable("ObtieneClientesEmpresa", CommandType.StoredProcedure)
                End With
            End Using

            Return dtDatos
        End Function

        Public Function ConsultaAgrupacionServicio() As DataTable
            Dim dtDatos As New DataTable

            Using _dbManager As New LMDataAccess
                With _dbManager
                    .TiempoEsperaComando = 0
                    dtDatos = .EjecutarDataTable("ObtieneAgrupacionServicio", CommandType.StoredProcedure)
                End With
            End Using

            Return dtDatos
        End Function

        Public Function ConsultaTipoServicio(Optional ByVal idUsuarioConsulta As Integer = 0) As DataTable
            Dim dtDatos As New DataTable

            Using _dbManager As New LMDataAccess
                With _dbManager
                    .TiempoEsperaComando = 0
                    If idUsuarioConsulta > 0 Then .SqlParametros.Add("@idUsuarioConsulta", SqlDbType.Int).Value = idUsuarioConsulta
                    dtDatos = .EjecutarDataTable("ObtieneTipoServicio", CommandType.StoredProcedure)
                End With
            End Using

            Return dtDatos
        End Function
        Public Function ConsultaCamposActualizarServicioMensajeria() As DataTable
            Dim dtDatos As New DataTable

            Using _dbManager As New LMDataAccess
                With _dbManager
                    dtDatos = .EjecutarDataTable("ConsultaCamposActualizarServicioMensajeria", CommandType.StoredProcedure)
                End With
            End Using

            Return dtDatos
        End Function
        Public Function ConsultaTipoServicioFinacieros() As DataTable
            Dim dtDatos As New DataTable

            Using _dbManager As New LMDataAccess
                With _dbManager
                    .TiempoEsperaComando = 0
                    dtDatos = .EjecutarDataTable("ObtieneTipoServicioFinacieros", CommandType.StoredProcedure)
                End With
            End Using

            Return dtDatos
        End Function
        Public Function ConsultaTipoServicio(ByVal permiteCerar As Boolean) As DataTable
            Dim dtDatos As New DataTable

            Using _dbManager As New LMDataAccess
                With _dbManager
                    .TiempoEsperaComando = 0
                    .SqlParametros.Add("@permiteCrear", SqlDbType.Bit).Value = permiteCerar
                    dtDatos = .EjecutarDataTable("ObtieneTipoServicio", CommandType.StoredProcedure)
                End With
            End Using

            Return dtDatos
        End Function

        Public Function ConsultaClausula() As DataTable
            Dim dtDatos As DataTable
            Using _dbManager As New LMDataAccess
                With _dbManager
                    .TiempoEsperaComando = 0
                    dtDatos = .EjecutarDataTable("ObtieneClausula", CommandType.StoredProcedure)
                End With
            End Using
            Return dtDatos
        End Function

        Public Function ConsultaRegion() As DataTable
            Dim dtDatos As DataTable
            Using _dbManager As New LMDataAccess
                With _dbManager
                    .TiempoEsperaComando = 0
                    dtDatos = .EjecutarDataTable("ObtenerRegiones", CommandType.StoredProcedure)
                End With
            End Using
            Return dtDatos
        End Function

        Public Function ConsultaMaterial(ByVal idTipoProducto As Integer) As DataTable
            Dim dtDatos As DataTable
            Using _dbManager As New LMDataAccess
                With _dbManager
                    .TiempoEsperaComando = 0
                    .SqlParametros.Add("@idTipoProducto", SqlDbType.Int).Value = idTipoProducto
                    dtDatos = .EjecutarDataTable("ObtenerMaterialesCEM", CommandType.StoredProcedure)
                End With
            End Using
            Return dtDatos
        End Function

        Public Function ObtenerCaracteresValidos() As DataTable
            Dim dtDatos As DataTable
            Using _dbManager As New LMDataAccess
                With _dbManager
                    .TiempoEsperaComando = 0
                    dtDatos = .EjecutarDataTable("ObtieneCaracteres", CommandType.StoredProcedure)
                End With
            End Using
            Return dtDatos
        End Function

        Public Function ConsultaClausula(ByVal clausula As String) As DictionaryEntry
            Dim result As DictionaryEntry
            Using _dbManager As New LMDataAccess
                With _dbManager
                    .TiempoEsperaComando = 0
                    .SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = clausula
                    .ejecutarReader("ObtieneClausula", CommandType.StoredProcedure)
                    If Not .Reader Is Nothing AndAlso .Reader.HasRows Then
                        If .Reader.Read() Then result = New DictionaryEntry(CInt(.Reader("idClausula")), .Reader("nombre").ToString())
                    End If
                End With
            End Using
            Return result
        End Function

        Public Function ConsultaZona() As DataTable
            Dim dtDatos As New DataTable

            Using _dbManager As New LMDataAccess
                With _dbManager
                    .TiempoEsperaComando = 0
                    dtDatos = .EjecutarDataTable("ObtieneZona", CommandType.StoredProcedure)
                End With
            End Using
            Return dtDatos
        End Function

        Public Function ConsultaZonaServicioMensajeria(Optional ByVal idZona As Integer = 0, Optional ByVal idCiudad As Integer = 0) As DataTable

            Dim dtDatos As New DataTable
            Using _dbManager As New LMDataAccess
                With _dbManager
                    .TiempoEsperaComando = 0
                    If idZona > 0 Then .SqlParametros.Add("@idZona", SqlDbType.Int).Value = idZona
                    If idCiudad > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = idCiudad
                    dtDatos = .EjecutarDataTable("ObtieneZonaServicioMensajeria", CommandType.StoredProcedure)
                End With
            End Using
            Return dtDatos
        End Function

        Public Function ObtenerMateriales() As DataTable
            Dim dtDatos As DataTable
            Dim _dbManager As New LMDataAccess

            With _dbManager
                .TiempoEsperaComando = 0
                dtDatos = .EjecutarDataTable("ObtenerMateriales", CommandType.StoredProcedure)
            End With
            _dbManager.Dispose()

            Return dtDatos
        End Function

        Public Function ObtenerMinesMensajeria() As DataTable
            Dim dtDatos As DataTable
            Dim _dbManager As New LMDataAccess
            With _dbManager
                .TiempoEsperaComando = 0
                dtDatos = .EjecutarDataTable("ObtenerMinServicioMensajeria", CommandType.StoredProcedure)
            End With
            _dbManager.Dispose()
            Return dtDatos
        End Function

        Public Function ConsultarMotorizado(Optional ByVal idCiudad As Integer = 0) As DataTable
            Dim dtDatos As New DataTable
            Dim _dbManager As New LMDataAccess
            Try
                With _dbManager
                    .TiempoEsperaComando = 0
                    If idCiudad > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = idCiudad
                    dtDatos = .EjecutarDataTable("ConsultarMotorizado", CommandType.StoredProcedure)
                End With
            Finally
                If _dbManager IsNot Nothing Then _dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Function ObtenerListadoCiudadesCEM(Optional ByVal idPais As Integer = 0) As DataTable
            Dim dtDatos As New DataTable
            Dim _dbManager As New LMDataAccess
            Try
                With _dbManager
                    .TiempoEsperaComando = 0
                    If idPais > 0 Then .SqlParametros.Add("@idPais", SqlDbType.Int).Value = idPais
                    dtDatos = .EjecutarDataTable("ObtenerListadoCiudadesCEM", CommandType.StoredProcedure)
                End With
            Finally
                If _dbManager IsNot Nothing Then _dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Function ObtenerInfoPermisosOpcionesRestringidas() As DataTable
            Dim dtPermisos As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                Dim nombreFormulario As String = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Path)

                If HttpContext.Current.Session("dtInfoPermisosOpcRestringidas") Is Nothing OrElse _
                    CType(HttpContext.Current.Session("dtInfoPermisosOpcRestringidas"), DataTable).Select("nombreFormulario='" & nombreFormulario & "'").Length = 0 Then
                    With dbManager
                        .TiempoEsperaComando = 0
                        .SqlParametros.Add("@nombreFormulario", SqlDbType.VarChar, 100).Value = nombreFormulario
                        dtPermisos = .EjecutarDataTable("ObtenerInfoPermisosOpcionesMensajeriaRestringidas", CommandType.StoredProcedure)
                        HttpContext.Current.Session("dtInfoPermisosOpcRestringidas") = dtPermisos
                    End With
                Else
                    dtPermisos = HttpContext.Current.Session("dtInfoPermisosOpcRestringidas")
                End If
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtPermisos
        End Function

        Public Function ObtenerInfoRestriccionEstadoOpcionFuncional(ByVal nombreControl As String) As DataTable
            Dim dtPermisos As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                Dim nombreFormulario As String = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Path)

                With dbManager
                    .TiempoEsperaComando = 0
                    .SqlParametros.Add("@nombreFormulario", SqlDbType.VarChar, 100).Value = nombreFormulario
                    If Not String.IsNullOrEmpty(nombreControl) Then .SqlParametros.Add("@nombreControl", SqlDbType.VarChar, 100).Value = nombreControl
                    dtPermisos = .EjecutarDataTable("ObtenerInfoRestriccionEstadoOpcionFuncional", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtPermisos
        End Function

        Public Function ObtenerInfoRestriccionEstadoOpcionFuncional() As DataTable
            Dim dtPermisos As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                Dim nombreFormulario As String = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Path)
                If HttpContext.Current.Session("dtInfoRestriccionOpcEstado") Is Nothing OrElse _
                    CType(HttpContext.Current.Session("dtInfoRestriccionOpcEstado"), DataTable).Select("nombreFormulario='" & nombreFormulario & "'").Length = 0 Then
                    With dbManager
                        .TiempoEsperaComando = 0
                        .SqlParametros.Add("@nombreFormulario", SqlDbType.VarChar, 100).Value = nombreFormulario
                        dtPermisos = .EjecutarDataTable("ObtenerInfoRestriccionEstadoOpcionFuncional", CommandType.StoredProcedure)
                    End With
                    HttpContext.Current.Session("dtInfoRestriccionOpcEstado") = dtPermisos
                Else
                    dtPermisos = HttpContext.Current.Session("dtInfoRestriccionOpcEstado")
                End If
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtPermisos
        End Function

        Public Function ExisteRestriccionAutocargadoDatos() As Boolean
            Dim dbManager As New LMDataAccess
            Dim resultado As Boolean = False
            Try
                Dim nombreFormulario As String = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Path)
                Dim idPerfil As Integer = 0
                If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.Session("usxp009") IsNot Nothing Then
                    Integer.TryParse(HttpContext.Current.Session("usxp009").ToString, idPerfil)
                End If

                With dbManager
                    .TiempoEsperaComando = 0
                    .SqlParametros.Add("@nombreFormulario", SqlDbType.VarChar, 100).Value = nombreFormulario
                    .SqlParametros.Add("@idPerfil", SqlDbType.Int).Value = idPerfil
                    .SqlParametros.Add("@autocargar", SqlDbType.Bit).Value = False
                    .ejecutarReader("ObtenerInfoRestriccionAutocargadoDatosCEM", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing AndAlso .Reader.HasRows Then resultado = True
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return resultado
        End Function

        Public Function EsVisibleOpcionRestringida(ByVal nombreControl As String, ByVal idCiudadBodega As Integer, Optional ByVal idTipo As Integer = 0) As Boolean
            If HttpContext.Current.Session("dtInfoPermisosOpcRestringidas") IsNot Nothing Then
                Dim idPerfil As Integer
                Dim idCiudadUsuario As Integer

                If HttpContext.Current.Session("usxp009") IsNot Nothing Then Integer.TryParse(HttpContext.Current.Session("usxp009").ToString, idPerfil)
                If HttpContext.Current.Session("usxp007") IsNot Nothing Then Integer.TryParse(HttpContext.Current.Session("usxp007").ToString, idCiudadUsuario)

                Dim dvPermiso As DataView = CType(HttpContext.Current.Session("dtInfoPermisosOpcRestringidas"), DataTable).Copy().DefaultView
                dvPermiso.RowFilter = "nombreControl = '" & nombreControl & "'"
                If dvPermiso.Count > 0 Then
                    If CInt(dvPermiso.Item(0).Item("idTipo")) <> 0 Then dvPermiso.RowFilter += " AND idTipo=" & idTipo.ToString
                    
                    dvPermiso.RowFilter += " AND (idPerfil = " & idPerfil.ToString & " OR idPerfil = -1)"
                    If dvPermiso.Count > 0 Then
                        If CBool(dvPermiso.Item(0).Item("validarCiudad")) Then
                            If idCiudadBodega = idCiudadUsuario Then
                                Return True
                            Else
                                Return False
                            End If
                        Else
                            If dvPermiso.Count > 0 Then
                                If CInt(dvPermiso.Item(0).Item("idTipoDenegado")) <> 0 Then
                                    dvPermiso.RowFilter += "AND idTipoDenegado IN(" & idTipo.ToString & ")"
                                    If dvPermiso.Count > 0 Then
                                        Return False
                                    Else
                                        Return True
                                    End If
                                Else
                                    Return True
                                End If
                            End If
                        End If
                    Else
                        Return False
                    End If
                Else
                    Return True
                End If
            Else
                Throw New Exception("Imposible obtener la información de opciones restringidas.")
            End If
        End Function

        Public Function ExistePermisoSobreOpcionRestringida(ByVal nombreControl As String, ByVal idCiudadBodega As Integer, Optional ByVal idTipo As Integer = 0) As Boolean
            Dim dtPermisos As DataTable

            dtPermisos = HerramientasMensajeria.ObtenerInfoPermisosOpcionesRestringidas()

            If dtPermisos IsNot Nothing Then
                Dim idPerfil As Integer
                Dim idCiudadUsuario As Integer

                If HttpContext.Current.Session("usxp009") IsNot Nothing Then Integer.TryParse(HttpContext.Current.Session("usxp009").ToString, idPerfil)
                If HttpContext.Current.Session("usxp007") IsNot Nothing Then Integer.TryParse(HttpContext.Current.Session("usxp007").ToString, idCiudadUsuario)

                Dim dvPermiso As DataView = dtPermisos.Copy().DefaultView
                dvPermiso.RowFilter = "nombreControl = '" & nombreControl & "'"

                If dvPermiso.Count > 0 Then
                    If CInt(dvPermiso.Item(0).Item("idTipo")) <> 0 Then dvPermiso.RowFilter += " AND idTipo=" & idTipo.ToString

                    dvPermiso.RowFilter += " and idPerfil = " & idPerfil.ToString
                    If dvPermiso.Count > 0 Then
                        If CBool(dvPermiso.Item(0).Item("validarCiudad")) Then
                            If idCiudadBodega = idCiudadUsuario Then
                                Return True
                            Else
                                Return False
                            End If
                        Else
                            Return True
                        End If
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End Function

        Public Function EsVisibleSegunEstado(ByVal nombreControl As String, ByVal idEstado As Integer) As Boolean
            Dim dtRestriccion As DataTable
            'If HttpContext.Current.Session("dtInfoRestriccionOpcEstado") Is Nothing Then
            dtRestriccion = ObtenerInfoRestriccionEstadoOpcionFuncional()
            'HttpContext.Current.Session("dtInfoRestriccionOpcEstado") = dtRestriccion
            'Else
            'dtRestriccion = CType(HttpContext.Current.Session("dtInfoRestriccionOpcEstado"), DataTable)
            'If Not String.IsNullOrEmpty(dtRestriccion.DefaultView.RowFilter) Then dtRestriccion.DefaultView.RowFilter = ""
            'End If

            If dtRestriccion IsNot Nothing Then
                Dim idPerfil As Integer
                If HttpContext.Current.Session("usxp009") IsNot Nothing Then Integer.TryParse(HttpContext.Current.Session("usxp009").ToString, idPerfil)

                Dim dvRestriccion As DataView = dtRestriccion.Copy().DefaultView
                dvRestriccion.RowFilter = "nombreControl = '" & nombreControl & "' AND ISNULL(idPerfil," & idPerfil.ToString & ") = " & idPerfil.ToString

                If dvRestriccion.Count > 0 Then
                    dvRestriccion.RowFilter += " AND idEstado = " & idEstado.ToString
                    If dvRestriccion.Count > 0 Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return True
                End If
            Else
                Return False
            End If
        End Function

        Public Function EstaRestringidoPorEstado(ByVal nombreControl As String, ByVal idEstado As Integer) As Boolean
            Dim dtRestriccion As DataTable
            'If HttpContext.Current.Session("dtInfoRestriccionOpcEstado") Is Nothing Then
            dtRestriccion = ObtenerInfoRestriccionEstadoOpcionFuncional()
            'HttpContext.Current.Session("dtInfoRestriccionOpcEstado") = dtRestriccion
            'Else
            'dtRestriccion = CType(HttpContext.Current.Session("dtInfoRestriccionOpcEstado"), DataTable)
            'If Not String.IsNullOrEmpty(dtRestriccion.DefaultView.RowFilter) Then dtRestriccion.DefaultView.RowFilter = ""
            'End If

            If dtRestriccion IsNot Nothing Then
                Dim idPerfil As Integer
                If HttpContext.Current.Session("usxp009") IsNot Nothing Then Integer.TryParse(HttpContext.Current.Session("usxp009").ToString, idPerfil)

                Dim dvRestriccion As DataView = dtRestriccion.Copy().DefaultView
                dvRestriccion.RowFilter = "nombreControl = '" & nombreControl & "' AND ISNULL(idPerfil," & idPerfil.ToString & ") = " & idPerfil.ToString

                If dvRestriccion.Count > 0 Then
                    dvRestriccion.RowFilter += " AND idEstado = " & idEstado.ToString
                    If dvRestriccion.Count > 0 Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return True
                End If
            Else
                Return True
            End If
        End Function

        Public Function ObtenerListaEstadosPorDefecto(ByVal idFuncionalidad As Enumerados.FuncionalidadMensajeria) As ArrayList
            Dim dbManager As New LMDataAccess
            Dim arrEstado As New ArrayList
            Dim idUsuario As Integer = 1
            Dim idPerfil As Integer
            If HttpContext.Current IsNot Nothing Then
                If HttpContext.Current.Session("usxp001") IsNot Nothing Then _
                    Integer.TryParse(HttpContext.Current.Session("usxp001").ToString, idUsuario)

                If HttpContext.Current.Session("usxp009") IsNot Nothing Then _
                    Integer.TryParse(HttpContext.Current.Session("usxp009").ToString, idPerfil)
            End If

            If idUsuario > 0 Then
                Try
                    With dbManager
                        .TiempoEsperaComando = 0
                        If idFuncionalidad > 0 Then .SqlParametros.Add("@idFuncionalidad", SqlDbType.Int).Value = idFuncionalidad
                        If idPerfil > 0 Then .SqlParametros.Add("@idPerfil", SqlDbType.Int).Value = idPerfil
                        If idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .ejecutarReader("ObtenerEstadoPorDefectoPerfilMensajeria", CommandType.StoredProcedure)

                        If .Reader IsNot Nothing Then
                            While .Reader.Read
                                arrEstado.Add(CInt(.Reader("idEstado")))
                            End While
                            .Reader.Close()
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
            Return arrEstado
        End Function

        Public Function ObtenerCiudadesCem(Optional ByVal idCiudadPadre As Integer = 0, Optional ByVal ciudadesCercanas As Enumerados.EstadoBinario = Enumerados.EstadoBinario.Activo, Optional ByVal idBodega As Integer = 0) As DataTable
            Dim dtCiudades As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .TiempoEsperaComando = 0
                    If idCiudadPadre > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = idCiudadPadre
                    If ciudadesCercanas <> Enumerados.EstadoBinario.NoEstablecido Then .SqlParametros.Add("@ciudadCercana", SqlDbType.Bit).Value = ciudadesCercanas
                    If idBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = idBodega

                    dtCiudades = .EjecutarDataTable("ObtenerCiudadesBodegasCEM", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtCiudades
        End Function

        Public Function ObtenerCiudadesCallCenter(ByVal idUsuario As Integer, Optional ByVal idCiudadPadre As Integer = 0, Optional ByVal ciudadesCercanas As Enumerados.EstadoBinario = Enumerados.EstadoBinario.Activo, Optional ByVal idBodega As Integer = 0) As DataTable
            Dim dtCiudades As New DataTable
            Using dbManager As New LMDataAccess
                With dbManager
                    .TiempoEsperaComando = 0
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    If idCiudadPadre > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = idCiudadPadre
                    If ciudadesCercanas <> Enumerados.EstadoBinario.NoEstablecido Then .SqlParametros.Add("@ciudadCercana", SqlDbType.Bit).Value = ciudadesCercanas
                    If idBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = idBodega
                    .SqlParametros.Add("@idUnidadNegocio", SqlDbType.Int).Value = Enumerados.UnidadNegocio.MensajeriaEspecializada

                    dtCiudades = .EjecutarDataTable("ObtenerCiudadesBodegasCallCenter", CommandType.StoredProcedure)
                End With
            End Using
            Return dtCiudades
        End Function

        Public Function ObtenerCallCenter(ByVal idUsuario As Integer, Optional ByVal idCallCenter As Integer = 0, Optional ByVal nombre As String = "", Optional ByVal nombreContacto As String = "", _
                                          Optional ByVal telefonoContacto As String = "", Optional ByVal activo As Enumerados.EstadoBinario = Enumerados.EstadoBinario.Activo) As DataTable
            Dim dtCallCenter As New DataTable
            Using dbManager As New LMDataAccess
                With dbManager
                    .TiempoEsperaComando = 0
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    If idCallCenter > 0 Then .SqlParametros.Add("@idCallCenter", SqlDbType.Int).Value = idCallCenter
                    If Not String.IsNullOrEmpty(nombre) Then .SqlParametros.Add("@nombre", SqlDbType.VarChar, 255).Value = nombre
                    If Not String.IsNullOrEmpty(nombreContacto) Then .SqlParametros.Add("@nombreContacto", SqlDbType.VarChar, 255).Value = nombreContacto
                    If Not String.IsNullOrEmpty(nombreContacto) Then .SqlParametros.Add("@telefonoContacto", SqlDbType.VarChar, 50).Value = nombreContacto
                    If activo <> Enumerados.EstadoBinario.NoEstablecido Then .SqlParametros.Add("@activo", SqlDbType.Bit).Value = activo
                    dtCallCenter = .EjecutarDataTable("ObtieneCallCenters", CommandType.StoredProcedure)
                End With
            End Using
            Return dtCallCenter
        End Function

        Public Function ConsultaPrioridad() As DataTable
            Dim dtDatos As DataTable
            Using _dbManager As New LMDataAccess
                With _dbManager
                    .TiempoEsperaComando = 0
                    dtDatos = .EjecutarDataTable("ObtienePrioridad", CommandType.StoredProcedure)
                End With
            End Using
            Return dtDatos
        End Function

        Public Sub ObtenerInformacionSeriales(ByRef dtDatos As DataTable, ByVal seriales As ArrayList)
            Using _dbManager As New LMDataAccess
                With _dbManager
                    .TiempoEsperaComando = 0
                    .SqlParametros.Add("@listaSerial", SqlDbType.VarChar).Value = Join(seriales.ToArray, ",")
                    .LlenarDataTable(dtDatos, "ObtieneInformacionSerial", CommandType.StoredProcedure)
                End With
            End Using
        End Sub

        Public Sub ObtenerMateriaCantidadPorRadicado(ByRef dtDatos As DataTable, ByVal radicados As ArrayList)
            Using _dbManager As New LMDataAccess
                With _dbManager
                    .TiempoEsperaComando = 0
                    .SqlParametros.Add("@listaradicados", SqlDbType.VarChar).Value = Join(radicados.ToArray, ",")
                    .LlenarDataTable(dtDatos, "ObtenerMateriaCantidadPorRadicado", CommandType.StoredProcedure)
                End With
            End Using
        End Sub

        Public Sub exportarDatosAExcelGemBox(ByVal contextHttp As HttpContext, ByVal dtDatos As DataTable, ByVal ruta As String, _
                                                Optional ByVal nombreColumnas As ArrayList = Nothing, Optional ByVal showFooter As Boolean = True)
            SpreadsheetInfo.SetLicense("EVIF-6YOV-FYFL-M3H6")
            Dim ef As New ExcelFile
            Dim ws As ExcelWorksheet

            ws = ef.Worksheets.Add("Datos")
            ws.ExtractToDataTable(dtDatos, dtDatos.Rows.Count, ExtractDataOptions.StopAtFirstEmptyRow, ws.Rows(0), ws.Columns(0))
            ws.InsertDataTable(dtDatos, "A1", True)

            For i As Integer = 0 To dtDatos.Columns.Count - 1
                If Not nombreColumnas Is Nothing Then
                    ws.Cells(0, i).Value = nombreColumnas(i)
                Else
                    ws.Cells(0, i).Value = dtDatos.Columns(i).ColumnName
                End If
                With ws.Cells(0, i).Style
                    .FillPattern.SetPattern(FillPatternStyle.Solid, Color.DarkBlue, Color.DarkBlue)
                    .Font.Color = Color.White
                    .Font.Weight = ExcelFont.BoldWeight
                    .Borders.SetBorders(MultipleBorders.Top, Color.FromName("black"), LineStyle.Thin)
                    .Borders.SetBorders(MultipleBorders.Right, Color.FromName("black"), LineStyle.Thin)
                    .Borders.SetBorders(MultipleBorders.Left, Color.FromName("black"), LineStyle.Thin)
                    .Borders.SetBorders(MultipleBorders.Bottom, Color.FromName("black"), LineStyle.Thin)
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            Next

            If showFooter Then
                ws.Cells.GetSubrangeAbsolute(dtDatos.Rows.Count + 1, 0, (dtDatos.Rows.Count + 1), dtDatos.Columns.Count - 1).Merged = True
                With ws.Cells("A" & (dtDatos.Rows.Count + 2).ToString).Style
                    .FillPattern.SetPattern(FillPatternStyle.Solid, Color.LightGray, Color.LightGray)
                    .Font.Color = Color.DarkBlue
                    .Font.Weight = ExcelFont.BoldWeight
                    .Borders.SetBorders(MultipleBorders.Top, Color.FromName("black"), LineStyle.Thin)
                    .Borders.SetBorders(MultipleBorders.Right, Color.FromName("black"), LineStyle.Thin)
                    .Borders.SetBorders(MultipleBorders.Left, Color.FromName("black"), LineStyle.Thin)
                    .Borders.SetBorders(MultipleBorders.Bottom, Color.FromName("black"), LineStyle.Thin)
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
                ws.Cells("A" & (dtDatos.Rows.Count + 2).ToString).Value = dtDatos.Rows.Count & " Registro(s) Encontrado(s)"
            End If

            For index As Integer = 0 To dtDatos.Columns.Count - 1
                ws.Columns(index).AutoFit()
            Next

            ef.SaveXls(ruta)
        End Sub

        Public Function ConsultarSubproceso() As DataTable
            Dim _dbManager As New LMDataAccess
            Dim dtDatos As New DataTable

            Try
                With _dbManager
                    .TiempoEsperaComando = 0
                    dtDatos = .EjecutarDataTable("ConsultarSubrocesoCEM", CommandType.StoredProcedure)
                End With
            Finally
                If _dbManager IsNot Nothing Then _dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Function ConsultarTipoSim() As DataTable
            Dim _dbManager As New LMDataAccess
            Dim dtDatos As New DataTable

            Try
                With _dbManager
                    .TiempoEsperaComando = 0
                    dtDatos = .EjecutarDataTable("ConsultarTipoSim", CommandType.StoredProcedure)
                End With
            Finally
                If _dbManager IsNot Nothing Then _dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Function ObtieneMediosDePago() As DataTable
            Dim dtDatos As DataTable
            Using dbManager As New LMDataAccess
                With dbManager
                    .TiempoEsperaComando = 0
                    dtDatos = dbManager.EjecutarDataTable("ObtieneMediosDePago", CommandType.StoredProcedure)
                End With
            End Using
            Return dtDatos
        End Function

        Public Function ObtieneTiposDeMigracion(ByVal activo As Nullable(Of Boolean)) As DataTable
            Dim dtDatos As DataTable
            Using dbManager As New LMDataAccess
                With dbManager
                    .TiempoEsperaComando = 0
                    If activo IsNot Nothing Then .SqlParametros.Add("@activo", SqlDbType.Bit).Value = activo
                    dtDatos = .EjecutarDataTable("ObtieneTiposDeMigracion", CommandType.StoredProcedure)
                End With
            End Using
            Return dtDatos
        End Function

        Public Function ObtenerDisponibilidadInventarioParaNotificacion(Optional ByVal listaNumeroRadicado As String = "", _
                                                                        Optional ByVal flagMasivo As Integer = 0, Optional ByVal idUsuario As Integer = 0) As DataTable
            Dim dtDatos As New DataTable
            Dim dbManager As New LMDataAccess
            With dbManager
                .TiempoEsperaComando = 0
                With .SqlParametros

                    If Not String.IsNullOrEmpty(listaNumeroRadicado) Then .Add("@numeroRadicado", SqlDbType.VarChar, 2000).Value = listaNumeroRadicado
                    If flagMasivo > 0 Then .Add("@flagMasivo", SqlDbType.Int).Value = flagMasivo
                    If idUsuario > 0 Then .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                End With
                dtDatos = .EjecutarDataTable("ReporteNotificacionesCEMDetalle", CommandType.StoredProcedure)
            End With
            Return dtDatos
        End Function

        Public Function ObtenerServiciosSiembraSinDisponibilidadParaNotificacion(ByVal idTipoServicio As Integer) As DataTable
            Dim dtDatos As New DataTable
            Dim dbManager As New LMDataAccess
            With dbManager
                .TiempoEsperaComando = 0
                With .SqlParametros

                    If idTipoServicio > 0 Then .Add("@idTipoServicio", SqlDbType.Int).Value = idTipoServicio
                End With
                dtDatos = .EjecutarDataTable("ReporteNotificacionesServiciosSindisponibilidad", CommandType.StoredProcedure)
            End With
            Return dtDatos
        End Function

        Public Sub VerificarDisponibilidadMaterial(ByVal idServicioMensajeria As Long, ByVal idServicioTipo As Long)
            Dim dbManager As New LMDataAccess
            With dbManager
                .TiempoEsperaComando = 0
                .SqlParametros.Add("@idServicioTipo", SqlDbType.BigInt).Value = idServicioTipo
                .SqlParametros.Add("@idServicioMensajeria", SqlDbType.BigInt).Value = idServicioMensajeria
                .EjecutarNonQuery("VerificarDisponibilidadMaterial", CommandType.StoredProcedure)
            End With
        End Sub

        Public Function ConsultarHistorialReagenda(ByVal idServicio As Integer) As DataTable
            Dim _dbManager As New LMDataAccess
            Dim dtDatos As New DataTable

            Try
                With _dbManager
                    .TiempoEsperaComando = 0
                    .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = idServicio
                    dtDatos = .EjecutarDataTable("ConsultarHistorialReagenda", CommandType.StoredProcedure)
                End With
            Finally
                If _dbManager IsNot Nothing Then _dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Function ObtenerMsisdnTemporales(ByVal idUsuario As Integer) As DataTable
            Dim dtDatos As DataTable
            Using _dbManager As New LMDataAccess
                With _dbManager
                    .TiempoEsperaComando = 0
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    dtDatos = .EjecutarDataTable("ObtenerMsisdnTemporales", CommandType.StoredProcedure)
                End With
            End Using
            Return dtDatos
        End Function

        Public Function ObtenerFormaPago() As DataTable
            Dim dtDatos As DataTable
            Using _dbManager As New LMDataAccess
                With _dbManager
                    .TiempoEsperaComando = 0
                    dtDatos = .EjecutarDataTable("ObtenerFormaPago", CommandType.StoredProcedure)
                End With
            End Using
            Return dtDatos
        End Function
		
		Public Function ObtenerTipoCampanias(ByVal estado As Boolean) As DataTable
            Dim dtTipos As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .TiempoEsperaComando = 0
                    .SqlParametros.Add("@estado", SqlDbType.Bit).Value = estado
                    dtTipos = .EjecutarDataTable("ObtenerTipoCampanias", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtTipos
        End Function

        Public Function MensajeNotificacionConfirmacionVentaCorporativa(ByVal idServicio As Integer) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .TiempoEsperaComando = 0
                    With .SqlParametros
                        .Add("@idServicio", SqlDbType.Int).Value = idServicio
                        .Add("@mensaje", SqlDbType.VarChar, 5000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .EjecutarNonQuery("MensajeNotificacionConfirmacionVentaCorporativa", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                    Else
                        resultado.EstablecerMensajeYValor(300, "No se logró establecer la respuesta del servidor.")
                    End If

                End With
            Catch ex As Exception
                dbManager.Dispose()
                resultado.EstablecerMensajeYValor(400, "Se presentó un error al generar el mensaje de confirmación: " & ex.Message)
            End Try
            Return resultado
        End Function

        Public Function ConsultarTipoServicioDisponible() As DataTable
            Dim dtDatos As New DataTable
            Dim dbManager As New LMDataAccess
            With dbManager
                .TiempoEsperaComando = 0
                dtDatos = .EjecutarDataTable("ConsultarTipoServicioDisponible", CommandType.StoredProcedure)
            End With
            Return dtDatos
        End Function

        Public Sub EliminarUnidadNegocioTransitoria(ByVal idUsuario As Integer)
            Dim dtDatos As New DataTable
            Dim dbManager As New LMDataAccess
            With dbManager
                .TiempoEsperaComando = 0
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                dtDatos = .EjecutarDataTable("EliminarUnidadNegocioTransitoria", CommandType.StoredProcedure)
            End With
        End Sub

        Public Function ObtenerInformacionEmpresa() As DataTable
            Dim dbManager As New LMDataAccess
            Dim dt As New DataTable
            Try
                With dbManager
                    dt = .EjecutarDataTable("ObtenerEmpresa", CommandType.StoredProcedure)

                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dt
        End Function

        Public Function ObtenerInformacionBanco() As DataTable
            Dim dbManager As New LMDataAccess
            Dim dt As New DataTable
            Try
                With dbManager
                    dt = .EjecutarDataTable("ObtenerInformacionBanco", CommandType.StoredProcedure)

                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dt
        End Function

        Public Function ObtenerTipoServiciosPresence() As DataTable
            Dim _dbManager As New LMDataAccess
            Dim dt As New DataTable
            Try
                With _dbManager
                    dt = .EjecutarDataTable("ObtenerTipoServiciosPresence", CommandType.StoredProcedure)

                End With
            Finally
                If _dbManager IsNot Nothing Then _dbManager.Dispose()
            End Try
            Return dt
        End Function



#Region "Novedades"

        Public Function ObtenerTiposDeNovedad(Optional ByVal idProceso As Integer = 0, _
                                              Optional ByVal idEstado As Integer = 0, _
                                              Optional ByVal gestionable As Enumerados.EstadoBinario = Enumerados.EstadoBinario.NoEstablecido, _
                                              Optional ByVal idTipoServicio As Integer = 0) As DataTable
            Dim dtDatos As DataTable

            Dim _dbManager As New LMDataAccess

            With _dbManager
                If idProceso > 0 Then .SqlParametros.Add("@idProceso", SqlDbType.Int).Value = idProceso
                If idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = idEstado
                If gestionable <> Enumerados.EstadoBinario.NoEstablecido Then .SqlParametros.Add("@gestionable", SqlDbType.Bit).Value = IIf(gestionable = Enumerados.EstadoBinario.Activo, 1, 0)
                If idTipoServicio > 0 Then .SqlParametros.Add("@idTipoServicio", SqlDbType.Int).Value = idTipoServicio
                .TiempoEsperaComando = 0
                dtDatos = .EjecutarDataTable("ObtenerTiposNovedadMensajeriaEspecializada", CommandType.StoredProcedure)
            End With
            _dbManager.Dispose()
            Return dtDatos
        End Function

        Public Function ValidarNovedadEnProcesoActual(ByVal idServicio As Long) As Boolean
            Dim bExisteNovedad As Boolean
            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Add("@idServicio", SqlDbType.BigInt).Value = idServicio
                    .SqlParametros.Add("@return", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue
                    .TiempoEsperaComando = 0
                    .EjecutarNonQuery("ExisteNovedadActualServicioMensajeria", CommandType.StoredProcedure)
                    bExisteNovedad = .SqlParametros("@return").Value
                End With
            End Using
            Return bExisteNovedad
        End Function

        Public Function ConsultarHistorialCambioEstado(ByVal idServicio As Integer, ByVal idUsusario As Long) As DataTable
            Dim _dbManager As New LMDataAccess
            Dim dtDatos As New DataTable

            Try
                With _dbManager
                    .TiempoEsperaComando = 0
                    .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = idServicio
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsusario
                    dtDatos = .EjecutarDataTable("ConsultarHistorialCambioEstado", CommandType.StoredProcedure)
                End With
            Finally
                If _dbManager IsNot Nothing Then _dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

#End Region


#End Region

#Region "Métodos para Servicio Técnico"

        Public Function ObtenerInfoEquiposRecibidosST(Optional ByVal idServicio As Integer = 0, Optional ByVal numRadicado As Long = 0) As DataTable
            Dim dtInfoRecepcion As New DataTable
            Using dbManager As New LMDataAccess
                With dbManager
                    .TiempoEsperaComando = 0
                    If idServicio > 0 Then .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = idServicio
                    If numRadicado > 0 Then .SqlParametros.Add("@numRadicado", SqlDbType.BigInt).Value = numRadicado
                    dtInfoRecepcion = .EjecutarDataTable("ObtenerInfoRecepcionServicioTecnico", CommandType.StoredProcedure)
                    If dtInfoRecepcion.Rows.Count = 0 Then Throw New Exception("No se encontró información de recepción de Servicio Técnico.<br>Por favor verifique que se haya realizado la recepción vía WAP.")
                End With
            End Using
            Return dtInfoRecepcion
        End Function

        Public Function ObtenerInfoRadicadosEnRutasActivas(Optional ByVal idServicio As Integer = 0, Optional ByVal numRadicado As Long = 0) As DataTable
            Dim dtInfoRutas As New DataTable
            Using dbManager As New LMDataAccess
                With dbManager
                    .TiempoEsperaComando = 0
                    If idServicio > 0 Then .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = idServicio
                    If numRadicado > 0 Then .SqlParametros.Add("@numRadicado", SqlDbType.BigInt).Value = numRadicado
                    dtInfoRutas = .EjecutarDataTable("ObtenerInfoRadicadosEnRutasActivas", CommandType.StoredProcedure)
                End With
            End Using
            Return dtInfoRutas
        End Function

        Public Function ObtenerProveedoresST() As DataTable
            Dim dtDatos As New DataTable()
            Using dbManager As New LMDataAccess
                With dbManager
                    .TiempoEsperaComando = 0
                    dtDatos = .EjecutarDataTable("ObtenerProveedoresServicioTecnico", CommandType.StoredProcedure)
                    If dtDatos.Rows.Count = 0 Then Throw New Exception("No se encontraron proveedores de servicio técnico configurados.")
                End With
            End Using
            Return dtDatos
        End Function

#End Region

#Region "Métodos para Ventas Telefónicas"

        Public Function ObtieneCampaniasDisponiblesUsuario(ByVal idUsuario As Integer) As DataTable
            Dim dtDatos As DataTable
            Using dbManager As New LMDataAccess
                With dbManager
                    .TiempoEsperaComando = 0
                    If idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    If idUsuario > 0 Then .SqlParametros.Add("@idUsuarioConsulta", SqlDbType.Int).Value = idUsuario
                    dtDatos = .EjecutarDataTable("ObtieneCampaniasVentasUsuario", CommandType.StoredProcedure)
                End With
            End Using
            Return dtDatos
        End Function

        Public Function ObtieneCantidadServiciosPendientes(ByVal idTipoServicio As Enumerados.TipoServicio) As Integer
            Dim respuesta As Integer
            Using dbManager As New LMDataAccess
                With dbManager
                    .TiempoEsperaComando = 0
                    .SqlParametros.Add("@idTipoServicio", SqlDbType.Int).Value = idTipoServicio
                    respuesta = .EjecutarScalar("ObtieneCantidadServiciosPendientes", CommandType.StoredProcedure)
                End With
            End Using
            Return respuesta
        End Function

        Public Function ObtieneClasesSIM() As DataTable
            Dim dtDatos As DataTable
            Using dbManager As New LMDataAccess
                dbManager.TiempoEsperaComando = 0
                dtDatos = dbManager.EjecutarDataTable("ObtieneClasesSIM", CommandType.StoredProcedure)
            End Using
            Return dtDatos
        End Function

        Public Function ObtieneRestriccionAgenda() As DataSet
            Dim dsDatos As DataSet
            Using dbManager As New LMDataAccess
                dbManager.TiempoEsperaComando = 0
                dsDatos = dbManager.EjecutarDataSet("ObtieneRestriccionesAgendaVentas", CommandType.StoredProcedure)
                If dsDatos.Tables.Count = 2 Then
                    dsDatos.Tables(0).TableName = "Horario"
                    dsDatos.Tables(1).TableName = "Restriccion"
                End If
            End Using
            Return dsDatos
        End Function

        Public Function ObtieneTiposPlanVenta() As DataTable
            Dim dtDatos As New DataTable
            Using dbManager As New LMDataAccess()
                With dbManager
                    dbManager.TiempoEsperaComando = 0
                    dtDatos = .EjecutarDataTable("ObtieneTiposPlanVenta", CommandType.StoredProcedure)
                End With
            End Using
            Return dtDatos
        End Function

        Public Function ObtieneTiposClientes() As DataTable
            Dim dtDatos As New DataTable
            Using dbManager As New LMDataAccess()
                With dbManager
                    .TiempoEsperaComando = 0
                    dtDatos = .EjecutarDataTable("ObtenerTipoCliente", CommandType.StoredProcedure)
                End With
            End Using
            Return dtDatos
        End Function

        Public Function ObtieneTiposIdentificacion() As DataTable
            Dim dtDatos As New DataTable
            Using dbManager As New LMDataAccess()

                With dbManager
                    .TiempoEsperaComando = 0
                    dtDatos = .EjecutarDataTable("ObtenerTipoIdentificacion", CommandType.StoredProcedure)
                End With
            End Using
            Return dtDatos
        End Function

#End Region

#Region "Métodos para Siembra"

        Public Function ObtieneUsuariosGerenciaDisponible(ByVal idTipoPersona As Enumerados.TipoPersonaSiembra) As DataTable
            Dim dtDatos As New DataTable
            Using dbManager As New LMDataAccess
                With dbManager
                    .TiempoEsperaComando = 0
                    .SqlParametros.Add("@tipoPersona", SqlDbType.Int).Value = idTipoPersona
                    dtDatos = .EjecutarDataTable("ObtieneUsuariosGerenciaDisponiblesJerarquiaPerfil", CommandType.StoredProcedure)
                End With
            End Using
            Return dtDatos
        End Function

        Public Function ObtienePersonalEnGerencia() As DataTable
            Dim dtDatos As New DataTable
            Using dbManager As New LMDataAccess
                With dbManager
                    .TiempoEsperaComando = 0
                    dtDatos = .EjecutarDataTable("ObtienePersonalEnGerencia", CommandType.StoredProcedure)
                End With
            End Using
            Return dtDatos
        End Function

        Public Function ObtieneCiudadesPersonalEnGerencia(ByVal idUsuario As Integer) As DataTable
            Dim dtDatos As New DataTable
            Using dbManager As New LMDataAccess
                With dbManager
                    .TiempoEsperaComando = 0
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    dtDatos = .EjecutarDataTable("ObtieneCiudadesPersonalEnGerencia", CommandType.StoredProcedure)
                End With
            End Using
            Return dtDatos
        End Function

#End Region

#Region "Métodos para EDUCLIC"
        Public Function ObtenerEstablecimientosCallCenter(idUsuario As Integer, Optional ByVal idCiudadPadre As Integer = 0, Optional ByVal ciudadesCercanas As Enumerados.EstadoBinario = Enumerados.EstadoBinario.Activo, Optional ByVal idBodega As Integer = 0) As DataTable
            Dim dtEstablecimientos As New DataTable
            Using dbManager As New LMDataAccess
                With dbManager
                    .TiempoEsperaComando = 0
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    If idCiudadPadre > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = idCiudadPadre
                    If ciudadesCercanas <> Enumerados.EstadoBinario.NoEstablecido Then .SqlParametros.Add("@ciudadCercana", SqlDbType.Bit).Value = ciudadesCercanas
                    If idBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = idBodega
                    .SqlParametros.Add("@idUnidadNegocio", SqlDbType.Int).Value = Enumerados.UnidadNegocio.MensajeriaEspecializada

                    dtEstablecimientos = .EjecutarDataTable("ObtenerEstablecimientosCallCenter", CommandType.StoredProcedure)
                End With
            End Using
            Return dtEstablecimientos
        End Function
#End Region

#Region "Metodos Servicios Financieros"
        Public Function ObtenerSucursalesFinancieras() As DataTable
            Dim dtDatos As New DataTable
            Using dbManager As New LMDataAccess()

                With dbManager
                    .TiempoEsperaComando = 0
                    dtDatos = .EjecutarDataTable("ObtenerSucursalesFinancieras", CommandType.StoredProcedure)
                End With
            End Using
            Return dtDatos
        End Function

        Public Function ObtenerTransportadoras() As DataTable
            Dim dtDatos As DataTable
            Using dbManager As New LMDataAccess
                With dbManager
                    dtDatos = .EjecutarDataTable("ObtenerTransportadoras", CommandType.StoredProcedure)
                End With
            End Using
            Return dtDatos
        End Function
#End Region
#Region "ggtGuiasTransporte"
        Public Function ObtenerTransportadoras(Optional ByVal listaIdTransportadoras As String = "") As DataTable
            Dim dtDatos As DataTable
            Using dbManager As New LMDataAccess
                With dbManager
                    If IsNothing(listaIdTransportadoras) = False Then
                        If listaIdTransportadoras.Trim <> "" Then .SqlParametros.Add("@idtransp", SqlDbType.VarChar, 200).Value = listaIdTransportadoras
                    End If
                    dtDatos = .EjecutarDataTable("ObtenerTransportadoras", CommandType.StoredProcedure)
                End With
            End Using
            Return dtDatos
        End Function
#End Region
    End Module

End Namespace