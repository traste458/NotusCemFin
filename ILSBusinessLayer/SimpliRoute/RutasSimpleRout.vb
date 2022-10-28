Imports System.Text
Imports System.Net
Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports LMDataAccessLayer

Public Class RutasSimpleRout
    Private _driver As String
    Private _vehicle As String
    Private _bodega As String
    Private _mensaje As String
    Private _id As String
    Private _idUsuario As Integer
    Private _idRuta As String
    Private _userDriver As String
    Private _listaBodegas As ArrayList
    Private _listaTipoServicio As ArrayList
    Private _jornada As Integer
    Private _dtDatos As DataTable
    Private _idSistemaNotus As Integer
    Private _cadenaConexion As String
    Private _order As String


    Public Property Driver As String
        Get
            Return _driver
        End Get
        Set(value As String)
            _driver = Value
        End Set
    End Property

    Public Property Vehicle As String
        Get
            Return _vehicle
        End Get
        Set(value As String)
            _vehicle = value
        End Set
    End Property

    Public Property Id As String
        Get
            Return _id
        End Get
        Set(value As String)
            _id = value
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

    Public Property ListaBodegas As ArrayList
        Get
            Return _listaBodegas
        End Get
        Set(value As ArrayList)
            _listaBodegas = value
        End Set
    End Property

    Public Property ListaTipoServicio As ArrayList
        Get
            Return _listaTipoServicio
        End Get
        Set(value As ArrayList)
            _listaTipoServicio = value
        End Set
    End Property

    Public Property UserDriver As String
        Get
            Return _userDriver
        End Get
        Set(value As String)
            _userDriver = value
        End Set
    End Property

    Public Property IdRuta As String
        Get
            Return _idRuta
        End Get
        Set(value As String)
            _idRuta = value
        End Set
    End Property

    Public Property Jornada As Integer
        Get
            Return _jornada
        End Get
        Set(value As Integer)
            _jornada = value
        End Set
    End Property

    Public Property Bodega As String
        Get
            Return _bodega
        End Get
        Set(value As String)
            _bodega = value
        End Set
    End Property

    Public Property Mensaje As String
        Get
            Return _mensaje
        End Get
        Set(value As String)
            _mensaje = value
        End Set
    End Property

    Public Property DtDatos As DataTable
        Get
            Return _dtDatos
        End Get
        Set(value As DataTable)
            _dtDatos = value
        End Set
    End Property

    Public Property IdSistemaNotus As Integer
        Get
            Return _idSistemaNotus
        End Get
        Set(value As Integer)
            _idSistemaNotus = value
        End Set
    End Property

    Public Property CadenaConexion As String
        Get
            Return _cadenaConexion
        End Get
        Set(value As String)
            _cadenaConexion = value
        End Set
    End Property

    Public Property Order As String
        Get
            Return _order
        End Get
        Set(value As String)
            _order = value
        End Set
    End Property

    Public Function ConsultarVisitas()
        Dim db As New LMDataAccess
        Dim dt As New DataTable
        Dim res As New ResultadoProceso
        Try
            With db
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                .TiempoEsperaComando = 0
                dt = .EjecutarDataTable("ObtenerVisitasSinRuta", CommandType.StoredProcedure)
            End With

        Catch ex As Exception
            res.Valor = 0
            res.Mensaje = "Error al obtener los datos" & ex.Message
        End Try

        Return dt
    End Function

    Public Function ConsultarVisitasUsuario()
        Dim db As New LMDataAccess
        Dim dt As New DataTable
        Dim res As New ResultadoProceso
        Try
            With db
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                dt = .EjecutarDataTable("ObtenerVisitasSinRutaPorUsuario", CommandType.StoredProcedure)
            End With

        Catch ex As Exception
            res.Valor = 0
            res.Mensaje = "Error al obtener los datos" & ex.Message
        End Try

        Return dt
    End Function


    Public Function ActualizarMotorizadoAsignadoRuta() As ResultadoProceso
        Dim db As New LMDataAccess(CadenaConexion)
        Dim dt As New DataTable
        Dim Resultado As New ResultadoProceso
        Try
            With db
                .TiempoEsperaComando = 0
                .SqlParametros.Add("@driver", SqlDbType.VarChar, (50)).Value = Driver
                .SqlParametros.Add("@vehicle", SqlDbType.VarChar, (50)).Value = Vehicle
                .SqlParametros.Add("@order", SqlDbType.VarChar, (20)).Value = Order
                .SqlParametros.Add("@id", SqlDbType.Int).Value = Id
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                .EjecutarNonQuery("ActualizarMotorizadoVisitas", CommandType.StoredProcedure)
                Resultado.Valor = .SqlParametros("@resultado").Value
            End With

        Catch ex As Exception
            Resultado.Valor = 0
            Resultado.Mensaje = "Error al actualizar visitas"
        End Try

        Return Resultado
    End Function

    Public Function ActualizarMotorizadoTerceros() As ResultadoProceso
        Dim db As New LMDataAccess(CadenaConexion)
        Dim dt As New DataTable
        Dim Resultado As New ResultadoProceso
        Try
            With db
                .TiempoEsperaComando = 0
                .SqlParametros.Add("@driver", SqlDbType.VarChar, (50)).Value = Driver
                .SqlParametros.Add("@documento", SqlDbType.VarChar, (50)).Value = UserDriver
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                .EjecutarNonQuery("IngresarMotorizadoTerceroSimpliRoute", CommandType.StoredProcedure)
                Resultado.Valor = .SqlParametros("@resultado").Value
            End With

            If Resultado.Valor = 1 Then
                Resultado.Mensaje = "El Motorizado: " + Driver + ", no existe en el sistema."
            End If

        Catch ex As Exception
            Resultado.Valor = 1
            Resultado.Mensaje = "Error al validar el documento"
        End Try

        Return Resultado
    End Function


    Public Function ValidarMotorizadoSimpli() As DataTable
        Dim db As New LMDataAccess(CadenaConexion)
        Dim dt As New DataTable
        Dim Resultado As New ResultadoProceso

        Try

            With db
                .TiempoEsperaComando = 0
                .SqlParametros.Add("@driver", SqlDbType.VarChar, (10)).Value = Driver
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                dt = .EjecutarDataTable("ValidarDatosSimpliRoute", CommandType.StoredProcedure)
                Resultado.Valor = 1
            End With

        Catch ex As Exception
            Resultado.Valor = 0
            Resultado.Mensaje = "Error al validar motorizado"
        End Try

        Return dt
    End Function

    Public Function ObtenerErroresValidacion() As DataTable
        Dim db As New LMDataAccess
        Dim dt As New DataTable
        Dim Resultado As New ResultadoProceso

        Try

            With db
                .TiempoEsperaComando = 0
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                dt = .EjecutarDataTable("ObtenerLogErroresValidacion", CommandType.StoredProcedure)
                Resultado.Valor = 1
            End With

        Catch ex As Exception
            Resultado.Valor = 0
            Resultado.Mensaje = "No existen errores en el cargue"
        End Try

        Return dt
    End Function

    Public Function BorrarDatosLogdeErrores() As DataTable
        Dim db As New LMDataAccess
        Dim dt As New DataTable
        Dim Resultado As New ResultadoProceso

        Try
            With db
                .TiempoEsperaComando = 0
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                dt = .EjecutarDataTable("BorrarLogDeErroresSimpliRoute", CommandType.StoredProcedure)
                Resultado.Valor = 1
            End With

        Catch ex As Exception
            Resultado.Valor = 0
            Resultado.Mensaje = "No es posible borrar los datos de errores"
        End Try

        Return dt
    End Function

    Public Function ObtenerMotorizadoPorRadicado() As DataTable
        Dim db As New LMDataAccess
        Dim dt As New DataTable
        Dim Resultado As New ResultadoProceso

        Try

            With db
                If Id <> "" Then .SqlParametros.Add("@numeroServicio", SqlDbType.Int).Value = Id
                If IdRuta <> 0 Then .SqlParametros.Add("@idRuta", SqlDbType.Int).Value = IdRuta
                .TiempoEsperaComando = 0
                dt = .EjecutarDataTable("ConsultarMotirazadoPorRadicadoSimpliRout", CommandType.StoredProcedure)
                Resultado.Valor = 1
            End With

        Catch ex As Exception
            Resultado.Valor = 0
            Resultado.Mensaje = "No fue posible obtener motorizados asociados"
        End Try

        Return dt
    End Function

    Public Function IngresoDatosTransitoriaSimpliRoute()
        Dim db As New LMDataAccess
        Dim dt As New DataTable
        Dim Resultado As New ResultadoProceso

        Try

            With db
                .TiempoEsperaComando = 0
                With .SqlParametros
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@id", SqlDbType.Int).Value = Id
                End With
                .EjecutarNonQuery("InsertarRutasTransitoriaSimpliRoute", CommandType.StoredProcedure)
            End With

        Catch ex As Exception
            Resultado.Valor = 1
            Resultado.Mensaje = "Error al ingresar a la temporal"
        End Try

        Return Resultado

    End Function

    Public Function IngresarErrorToken()
        Dim db As New LMDataAccess
        Dim dt As New DataTable
        Dim Resultado As New ResultadoProceso

        Try

            With db
                .TiempoEsperaComando = 0
                With .SqlParametros
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    If (Bodega <> "") Then .Add("@bodega", SqlDbType.VarChar, 200).Value = Bodega
                    .Add("@mensaje", SqlDbType.VarChar, 200).Value = Mensaje
                End With
                .EjecutarNonQuery("InsertarLogErroresTokenBodSimpliRoute", CommandType.StoredProcedure)
            End With

        Catch ex As Exception
            Resultado.Valor = 1
            Resultado.Mensaje = "Error al ingresar a la temporal"
        End Try

        Return Resultado

    End Function

    Public Function crearRutasPorMotorizado() As ResultadoProceso
        Dim db As New LMDataAccess

        Dim Resultado As New ResultadoProceso

        Try

            With db
                With .SqlParametros
                    db.TiempoEsperaComando = 0
                    .Add("@mensaje", SqlDbType.VarChar, (100)).Direction = ParameterDirection.Output
                    .Add("@cantRegistros", SqlDbType.Int).Direction = ParameterDirection.Output
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                End With
                DtDatos = .EjecutarDataTable("RegistrarRutaSimpliRoute", CommandType.StoredProcedure)
                Resultado.Mensaje = .SqlParametros("@mensaje").Value
                Resultado.Valor = .SqlParametros("@cantRegistros").Value
            End With

        Catch ex As Exception
            Resultado.Valor = 1
            Resultado.Mensaje = "Error al ingresar rutas"
        End Try

        Return Resultado
    End Function

    Public Function ConsultarServiciosParaEliminarVisita(ByVal filtro As Estructuras.FiltroConsultaVisitasSimpliRoute) As DataTable
        Try
            Dim db As New LMDataAccess
            Dim dt As New DataTable


            With filtro
                'objConsulta.ListaBodega.AddRange(listaBodegas)

                If filtro.FechaInicial <> Date.MinValue Then db.SqlParametros.Add("@fechaInicio", SqlDbType.SmallDateTime).Value = filtro.FechaInicial
                If .FechaFinal <> Date.MinValue Then db.SqlParametros.Add("@fechaFin", SqlDbType.SmallDateTime).Value = .FechaFinal

                If .ciudad > 0 Then db.SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = .ciudad

                If .bodega IsNot Nothing AndAlso .bodega.Count > 0 Then
                    db.SqlParametros.Add("@listaBodega", SqlDbType.VarChar).Value = Join(.bodega.ToArray, ",")
                End If

                If .tipoServicio IsNot Nothing AndAlso .tipoServicio.Count > 0 Then
                    db.SqlParametros.Add("@listaTipoServicio", SqlDbType.VarChar).Value = Join(.tipoServicio.ToArray, ",")
                End If

                If .jornada > 0 Then db.SqlParametros.Add("@jornada", SqlDbType.Int).Value = .jornada

                db.SqlParametros.AddWithValue("@tbIdRadicadoServicioMensajeria", .tbRadicados)

                db.TiempoEsperaComando = 0
                dt = db.EjecutarDataTable("ConsultarServiciosParaEliminarVisita", CommandType.StoredProcedure)
            End With
            Return dt
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function


    Public Sub CrearVisitasSimpliRoute(ByVal data As DataTable)

        Dim dbManager As New LMDataAccess(CadenaConexion)
        Try
            With dbManager
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                .SqlParametros.AddWithValue("@tbInformacionSimpliRoute", data)
                .EjecutarNonQuery("RegistrarVisitasSimpliRoute", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

    End Sub

    Public Function ConsultarServiciosListosParaVisita(ByVal filtro As Estructuras.FiltroConsultaVisitasSimpliRoute) As DataTable
        Try
            Dim db As New LMDataAccess
            Dim dt As New DataTable


            With filtro
                'objConsulta.ListaBodega.AddRange(listaBodegas)

                If filtro.FechaInicial <> Date.MinValue Then db.SqlParametros.Add("@fechaInicio", SqlDbType.SmallDateTime).Value = filtro.FechaInicial
                If .FechaFinal <> Date.MinValue Then db.SqlParametros.Add("@fechaFin", SqlDbType.SmallDateTime).Value = .FechaFinal

                If .ciudad > 0 Then db.SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = .ciudad

                If .bodega IsNot Nothing AndAlso .bodega.Count > 0 Then
                    db.SqlParametros.Add("@listaBodega", SqlDbType.VarChar).Value = Join(.bodega.ToArray, ",")
                End If

                If .tipoServicio IsNot Nothing AndAlso .tipoServicio.Count > 0 Then
                    db.SqlParametros.Add("@listaTipoServicio", SqlDbType.VarChar).Value = Join(.tipoServicio.ToArray, ",")
                End If

                If .jornada > 0 Then db.SqlParametros.Add("@jornada", SqlDbType.Int).Value = .jornada

                db.SqlParametros.AddWithValue("@tbIdRadicadoServicioMensajeria", .tbRadicados)

                db.TiempoEsperaComando = 0
                dt = db.EjecutarDataTable("ConsultarServiciosListosParaVisita", CommandType.StoredProcedure)
            End With
            Return dt
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function RegistrarActualizarEstados(dt As DataTable, cadenaConexion As String) As DataTable
        Try
            Dim db As New LMDataAccess(cadenaConexion)
            With db
                .TiempoEsperaComando = 0
                .SqlParametros.AddWithValue("@tbInformacionSimpliRoute", dt)
                .EjecutarNonQuery("RegistrarActualizarEstadoVisitasSimpliRoute", CommandType.StoredProcedure)

            End With
            Return dt
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Sub RegistrarLogSimpliRoute(ByVal cadenaRequest As String, cadenaResponse As String, ByVal url As String)

        Dim db As New LMDataAccessLayer.LMDataAccess
        Try
            db.SqlParametros.Clear()
            With db.SqlParametros
                .Add("@cadenaRequest", SqlDbType.VarChar).Value = cadenaRequest
                .Add("@cadenaResponse", SqlDbType.VarChar).Value = cadenaResponse
                .Add("@urlApi", SqlDbType.VarChar).Value = url
            End With

            db.EjecutarNonQuery("RegistrarLogSimpliRoute", CommandType.StoredProcedure)
        Catch ex As Exception
        End Try
    End Sub

    Public Function ConsultarSistemasNotus() As DataTable
        Try
            Dim db As New LMDataAccess
            Dim dt As New DataTable
            With db
                .TiempoEsperaComando = 0
                If (IdSistemaNotus > 0) Then .SqlParametros.Add("@idSistemaNotus", SqlDbType.Int).Value = IdSistemaNotus
                dt = .EjecutarDataTable("ConsultarSistemasNotus", CommandType.StoredProcedure)

            End With
            Return dt
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function





End Class
