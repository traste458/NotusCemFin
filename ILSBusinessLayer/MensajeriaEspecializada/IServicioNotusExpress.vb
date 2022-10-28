Public Interface IServicioNotusExpress
    Function ActualizarGestionVenta(ByVal idServicio As Integer,
                                                      ByVal idEstado As Integer,
                                                      Optional ByVal justificacion As String = "Servicio modificado desde CEM, por el usuario: Admin") As ResultadoProceso
End Interface
