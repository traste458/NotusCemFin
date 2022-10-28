Imports System.Web
Imports LMDataAccessLayer
Imports ILSBusinessLayer.Comunes
Imports System.IO

Public Class CargueArchivoMinesSiembra

#Region "Propiedades"
    Property _idRegistro As Integer
    Property _idciudad As Integer
    Property _CiudadEntrega As String
    Property _Departamento As String
    Property _NombreEmpresa As String
    Property _NIT As String
    Property _TelefonoFijoEmpresa As String
    Property _Ext As String
    Property _NombreRepresentanteLegal As String
    Property _NumeroCedulaRepresentante As String
    Property _TelefonoCelularRepresentante As String
    Property _NombrepersonaAutorizada As String
    Property _NumeroCedulaAutorizado As String
    Property _CargoPersonaAutorizada As String
    Property _TelefonoPersonaAutorizada As String
    Property _Direccion As String
    Property _Barrio As String
    Property _ClienteClaro As Boolean
    Property _Observacion As String
    Property _ObservacionDireccion As String
    Property _idbodega As Integer
    Property _idUsuario As Integer
    Property _EstadoValidacion As Integer
    Property _minsColeccion As MsisdnEnServicioSiembraColeccion
    Property _estructuraTablaBase As DataTable
    Property _estructuraTabla As DataTable
    Property _estructuraTablaErrores As DataTable
    Property IdGerencia As Integer
    Property IdCoordinador As Integer
    Property IdConsultor As Integer
    Property _resultado As Integer
    Property IdEstado As Integer
    Property IdServicioMensajeria As Int64
#End Region


#Region "Métodos Privados"

    Private Sub EstructuraDatosBase()
        Try
            Dim dtDatos As New DataTable
            If _estructuraTablaBase Is Nothing Then
                With dtDatos.Columns
                    .Add(New DataColumn("msisdn", GetType(String)))
                    .Add(New DataColumn("region", GetType(String)))
                    .Add(New DataColumn("precioUnitarioSinDescuento", GetType(Integer)))
                    .Add(New DataColumn("precioUnitario", GetType(Integer)))
                    .Add(New DataColumn("precioEspecial", GetType(String)))
                    .Add(New DataColumn("materialEquipo", GetType(String)))
                    .Add(New DataColumn("requiereSim", GetType(String)))
                    .Add(New DataColumn("codigoCuenta", GetType(String)))
                    .Add(New DataColumn("nombrePlan", GetType(String)))
                    .Add(New DataColumn("tipoPlanVozDatos", GetType(String)))
                    .Add(New DataColumn("valorCargoBasicoPlanSinImpuesto", GetType(String)))
                    .Add(New DataColumn("paquete", GetType(String)))
                    .Add(New DataColumn("clausula", GetType(String)))
                    .Add(New DataColumn("valorClausula", GetType(String)))
                    .Add(New DataColumn("ventaEquipoContado", GetType(String)))
                    .Add(New DataColumn("ventaEquipoCuotas", GetType(String)))
                    .Add(New DataColumn("numeroCuotasVenta", GetType(String)))
                    .Add(New DataColumn("nombreCanalVenta", GetType(String)))
                    .Add(New DataColumn("codigoCanalVenta", GetType(String)))
                    .Add(New DataColumn("solicitudServicioNumero", GetType(String)))
                    .Add(New DataColumn("contratoCompraVentaEquipo", GetType(String)))
                    .Add(New DataColumn("nombreEjecutivoVenta", GetType(String)))
                End With
                dtDatos.AcceptChanges()
                _estructuraTablaBase = dtDatos
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function RegistrarMsisdnTemporales() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            Dim idUsuario As Integer = CInt(HttpContext.Current.Session("usxp001"))
            With dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With

                .EjecutarNonQuery("RegistrarMsisdnTemporalesCorporativo", CommandType.StoredProcedure)

                If (Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor)) Then
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                Else
                    resultado.EstablecerMensajeYValor(400, "No se logró establecer respuesta del servidor, por favor intentelo nuevamente.")
                End If

            End With

        Catch ex As Exception
            If dbManager IsNot Nothing Then dbManager.Dispose()
            resultado.EstablecerMensajeYValor(500, "Se generó un error al almacenar los mines: " & ex.Message)
        End Try
        Return resultado
    End Function

    Public Function EliminarMsisdnTemporal(ByVal msisdn As List(Of String)) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            Dim idUsuario As Integer = CInt(HttpContext.Current.Session("usxp001"))
            With dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .Add("@msisdn", SqlDbType.VarChar).Value = String.Join(",", msisdn.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With

                .EjecutarNonQuery("EliminarMsisdnTemporal", CommandType.StoredProcedure)

                If (Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor)) Then
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                Else
                    resultado.EstablecerMensajeYValor(400, "No se logró establecer respuesta del servidor, por favor intentelo nuevamente.")
                End If

            End With

        Catch ex As Exception
            If dbManager IsNot Nothing Then dbManager.Dispose()
            resultado.EstablecerMensajeYValor(500, "Se generó un error al eliminar los mines: " & ex.Message)
        End Try
        Return resultado
    End Function

    Public Function CargarRadicado(ByVal dtDetalleMines As DataTable, ByVal dtInformacionGeneral As DataTable, ByVal idUsuario As Integer, ByRef resultado As Int32)
        Dim dbManager As New LMDataAccess
        dtDetalleMines.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), idUsuario))
        dtInformacionGeneral.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), idUsuario))
        With dbManager
            Try
                Dim objCargueMines As New MsisdnEnServicioSiembraColeccion()
                With objCargueMines
                    ._idUsuario = idUsuario
                    .EliminarRegistrosTransitorias()
                End With
                .TiempoEsperaComando = 0
                .InicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "TransitoriaInformacionGeneralCrearSerivicioSiembra"
                    .ColumnMappings.Add("CiudadEntrega", "CiudadEntrega")
                    .ColumnMappings.Add("Departamento", "Departamento")
                    .ColumnMappings.Add("NombreEmpresa", "NombreEmpresa")
                    .ColumnMappings.Add("NIT", "NIT")
                    .ColumnMappings.Add("TelefonoFijoEmpresa", "TelefonoFijoEmpresa")
                    .ColumnMappings.Add("Ext", "Ext")
                    .ColumnMappings.Add("NombreRepresentanteLegal", "NombreRepresentanteLegal")
                    .ColumnMappings.Add("NumeroCedulaRepresentante", "NumeroCedulaRepresentante")
                    .ColumnMappings.Add("TelefonoCelularRepresentante", "TelefonoCelularRepresentante")
                    .ColumnMappings.Add("NombrepersonaAutorizada", "NombrepersonaAutorizada")
                    .ColumnMappings.Add("NumeroCedulaAutorizado", "NumeroCedulaAutorizado")
                    .ColumnMappings.Add("CargoPersonaAutorizada", "CargoPersonaAutorizada")
                    .ColumnMappings.Add("TelefonoPersonaAutorizada", "TelefonoPersonaAutorizada")
                    .ColumnMappings.Add("Direccion", "Direccion")
                    .ColumnMappings.Add("Barrio", "Barrio")
                    .ColumnMappings.Add("ClienteClaro", "ClienteClaro")
                    .ColumnMappings.Add("OBSERVACIONES", "Observacion")
                    .ColumnMappings.Add("idUsuario", "idUsuario")
                    .WriteToServer(dtInformacionGeneral)
                End With
                .InicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "TransitoriaMinesSiembra"
                    .ColumnMappings.Add("MSISDN", "msisdn")
                    .ColumnMappings.Add("tipo", "tipo")
                    .ColumnMappings.Add("Plan", "nombrePlan")
                    .ColumnMappings.Add("FechaDevolucion", "fechaDevolucion")
                    .ColumnMappings.Add("Equipo", "material")
                    .ColumnMappings.Add("TipoSIM", "tipoSim")
                    .ColumnMappings.Add("Paquete", "Paquete")
                    .ColumnMappings.Add("idUsuario", "idUsuario")
                    .WriteToServer(dtDetalleMines)
                End With
                ValidarTransitoriaServiciosiembra()

            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
    End Function

    Public Overloads Function RegistrarServicioSiembra()
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    If _idciudad > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = _idciudad
                    If _CiudadEntrega <> String.Empty Then .SqlParametros.Add("@CiudadEntrega", SqlDbType.VarChar).Value = _CiudadEntrega
                    If _NombreEmpresa <> String.Empty Then .SqlParametros.Add("@NombreEmpresa", SqlDbType.VarChar).Value = _NombreEmpresa
                    If _NIT <> String.Empty Then .SqlParametros.Add("@NIT", SqlDbType.VarChar).Value = _NIT
                    If _TelefonoFijoEmpresa <> String.Empty Then .SqlParametros.Add("@TelefonoFijoEmpresa", SqlDbType.VarChar).Value = _TelefonoFijoEmpresa
                    If _Ext <> String.Empty Then .SqlParametros.Add("@Ext", SqlDbType.VarChar).Value = _Ext
                    If Not String.IsNullOrEmpty(_NombreRepresentanteLegal) Then .SqlParametros.Add("@NombreRepresentanteLegal", SqlDbType.VarChar).Value = _NombreRepresentanteLegal
                    If Not String.IsNullOrEmpty(_NumeroCedulaRepresentante) Then .SqlParametros.Add("@NumeroCedulaRepresentante", SqlDbType.VarChar).Value = _NumeroCedulaRepresentante
                    If Not String.IsNullOrEmpty(_TelefonoCelularRepresentante) Then .SqlParametros.Add("@TelefonoCelularRepresentante", SqlDbType.VarChar).Value = _TelefonoCelularRepresentante
                    If _NombrepersonaAutorizada <> String.Empty Then .SqlParametros.Add("@NombrepersonaAutorizada", SqlDbType.VarChar).Value = _NombrepersonaAutorizada
                    If Not String.IsNullOrEmpty(_NumeroCedulaAutorizado) Then .SqlParametros.Add("@NumeroCedulaAutorizado", SqlDbType.VarChar).Value = _NumeroCedulaAutorizado
                    If Not String.IsNullOrEmpty(_CargoPersonaAutorizada) Then .SqlParametros.Add("@CargoPersonaAutorizada", SqlDbType.VarChar).Value = _CargoPersonaAutorizada
                    If Not String.IsNullOrEmpty(_TelefonoPersonaAutorizada) Then .SqlParametros.Add("@TelefonoPersonaAutorizada", SqlDbType.VarChar).Value = _TelefonoPersonaAutorizada
                    If _Direccion <> String.Empty Then .SqlParametros.Add("@Direccion", SqlDbType.VarChar).Value = _Direccion
                    If _ObservacionDireccion <> String.Empty Then .SqlParametros.Add("@ObservacionDireccion", SqlDbType.VarChar).Value = _ObservacionDireccion
                    If _Barrio <> String.Empty Then .SqlParametros.Add("@Barrio", SqlDbType.VarChar).Value = _Barrio
                    If _IdGerencia > 0 Then .SqlParametros.Add("@IdGerencia", SqlDbType.Int).Value = _IdGerencia
                    If _IdCoordinador > 0 Then .SqlParametros.Add("@IdCoordinador", SqlDbType.Int).Value = _IdCoordinador
                    If _IdConsultor > 0 Then .SqlParametros.Add("@IdConsultor", SqlDbType.Int).Value = _IdConsultor
                    .SqlParametros.Add("@ClienteClaro", SqlDbType.Bit).Value = _ClienteClaro
                    If _Observacion <> String.Empty Then .SqlParametros.Add("@Observacion", SqlDbType.VarChar).Value = _Observacion
                    .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output

                    .EjecutarScalar("ActualizarTransitoriaInformacionGeneralSerivicioSiembra", CommandType.StoredProcedure)
                    Integer.TryParse(.SqlParametros("@resultado").Value, _resultado)
                    If _resultado = 1 Then
                        ValidarTransitoriaServiciosiembra()
                        If _EstadoValidacion = 1 Then
                            RegistrarServiciosiembraArchivo()
                        End If
                    End If

                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Using

    End Function

    Public Sub CargarDatos()
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.BigInt).Value = _idUsuario
                    .TiempoEsperaComando = 0
                    .ejecutarReader("ObtenerInfoTransitoriaInformacionCrearSerivicioSiembra", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            CargarResultadoConsulta(.Reader)
                        End If
                        .Reader.Close()
                    End If

                    If _idUsuario > 0 Then _minsColeccion = New MsisdnEnServicioSiembraColeccion(_idUsuario)
                    If _idUsuario > 0 Then
                        ValidarTransitoriaServiciosiembra()
                    End If
                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Using
    End Sub
    Public Sub ValidarTransitoriaServiciosiembra()
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idUsuario > 0 Then
                        With .SqlParametros
                            .Clear()
                            .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                            .Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                        End With
                        .TiempoEsperaComando = 0
                        _estructuraTablaErrores = .EjecutarDataTable("ValidacionServicioSiembraPorArchivo", CommandType.StoredProcedure)
                        _EstadoValidacion = CType(.SqlParametros("@resultado").Value.ToString, Integer)

                    End If
                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Using

    End Sub

    Public Sub RegistrarServiciosiembraArchivo()
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idUsuario > 0 Then
                        .IniciarTransaccion()
                        With .SqlParametros
                            .Clear()
                            .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                            .Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                            .Add("@idServicioMensajeria", SqlDbType.Decimal).Direction = ParameterDirection.Output
                        End With
                        .TiempoEsperaComando = 0
                        _estructuraTablaErrores = .EjecutarDataTable("RegistrarServiciosiembraPorArchivo", CommandType.StoredProcedure)
                        _EstadoValidacion = CType(.SqlParametros("@resultado").Value.ToString, Integer)
                        If _EstadoValidacion = 1 Then
                            IdServicioMensajeria = CType(.SqlParametros("@idServicioMensajeria").Value.ToString, Int64)
                            .ConfirmarTransaccion()
                        Else
                            .AbortarTransaccion()
                        End If
                    End If
                End With
            Catch ex As Exception
                If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw ex
            End Try
        End Using

    End Sub
    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                If Not IsDBNull(reader("idRegistro")) Then _idRegistro = Convert.ToInt64(reader("idRegistro"))
                If Not IsDBNull(reader("idCiudad")) Then _idciudad = CInt(reader("idCiudad"))
                If Not IsDBNull(reader("CiudadEntrega")) Then _CiudadEntrega = reader("CiudadEntrega").ToString()
                If Not IsDBNull(reader("Departamento")) Then _Departamento = reader("Departamento").ToString()
                If Not IsDBNull(reader("NombreEmpresa")) Then _NombreEmpresa = reader("NombreEmpresa").ToString()
                If Not IsDBNull(reader("NIT")) Then _NIT = reader("NIT").ToString()
                If Not IsDBNull(reader("TelefonoFijoEmpresa")) Then _TelefonoFijoEmpresa = reader("TelefonoFijoEmpresa").ToString()
                If Not IsDBNull(reader("Ext")) Then _Ext = reader("Ext").ToString()
                If Not IsDBNull(reader("NombreRepresentanteLegal")) Then _NombreRepresentanteLegal = reader("NombreRepresentanteLegal").ToString()
                If Not IsDBNull(reader("NumeroCedulaRepresentante")) Then _NumeroCedulaRepresentante = reader("NumeroCedulaRepresentante").ToString()
                If Not IsDBNull(reader("TelefonoCelularRepresentante")) Then _TelefonoCelularRepresentante = reader("TelefonoCelularRepresentante").ToString()
                If Not IsDBNull(reader("NombrepersonaAutorizada")) Then _NombrepersonaAutorizada = reader("NombrepersonaAutorizada").ToString()
                If Not IsDBNull(reader("NumeroCedulaAutorizado")) Then _NumeroCedulaAutorizado = reader("NumeroCedulaAutorizado").ToString()
                If Not IsDBNull(reader("CargoPersonaAutorizada")) Then _CargoPersonaAutorizada = reader("CargoPersonaAutorizada").ToString()
                If Not IsDBNull(reader("TelefonoPersonaAutorizada")) Then _TelefonoPersonaAutorizada = reader("TelefonoPersonaAutorizada").ToString()
                If Not IsDBNull(reader("Direccion")) Then _Direccion = reader("Direccion").ToString()
                If Not IsDBNull(reader("Barrio")) Then _Barrio = reader("Barrio").ToString()
                If Not IsDBNull(reader("ClienteClaro")) Then _ClienteClaro = reader("ClienteClaro")
                If Not IsDBNull(reader("Observacion")) Then _Observacion = reader("Observacion").ToString()
                If Not IsDBNull(reader("idbodega")) Then _idbodega = CInt(reader("idbodega"))
                If Not IsDBNull(reader("idUsuario")) Then _idUsuario = CInt(reader("idUsuario"))

            End If
        End If

    End Sub
#End Region

End Class
