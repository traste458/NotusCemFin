Imports ILSBusinessLayer.Comunes

Public Class ServicioNotusExpressDavivienda
    Implements IServicioNotusExpress

    Public Function ActualizarGestionVenta(ByVal idServicio As Integer,
                                                      ByVal idEstado As Integer,
                                                      Optional ByVal justificacion As String = "Servicio modificado desde CEM, por el usuario: Admin") As ResultadoProceso Implements IServicioNotusExpress.ActualizarGestionVenta
        Dim resultado As New ResultadoProceso
        Dim objGestion As New NotusExpressDaviviendaService.NotusExpressDaviviendaWebService
        Dim infoWs As New InfoUrlSidService(objGestion, True)
        Dim WSInfoGestion As New ILSBusinessLayer.NotusExpressDaviviendaService.WsGestionVenta
        Dim Wsresultado As New ILSBusinessLayer.NotusExpressDaviviendaService.ResultadoProceso

        With WSInfoGestion
            .IdServicioNotus = idServicio
            .IdEstadoServicioMensajeria = idEstado
            .ObservacionNovedad = justificacion
            .IdModificador = 1
            'Se adiciona inclusión de envio de novedades a NotusExpress - Carlos Ayala -2015/10/01
            Dim listaNovedades As New ArrayList
            Dim objNovedades As New NovedadServicioMensajeriaColeccion(IdServicio:=idServicio)
            For Each Novedad As NovedadServicioMensajeria In objNovedades
                listaNovedades.Add(Novedad.IdTipoNovedad)
                .ObservacionNovedad = Novedad.Observacion
            Next

            .ListaNovedades = listaNovedades.ToArray
        End With

        Wsresultado = objGestion.ActualizaGestionVenta(WSInfoGestion)
        resultado.Valor = Wsresultado.Valor
        resultado.Mensaje = Wsresultado.Mensaje
        Return resultado
    End Function
End Class
