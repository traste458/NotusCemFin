Public Class FabricadorDetalleMsisdnEnServicioMensajeria

#Region "Métodos Públicos"

    Public Function Fabricar(ByVal idTipoServicio As Enumerados.TipoServicio) As IDetalleMsisdnEnServicioMensajeria
        Dim objFrabricado As IDetalleMsisdnEnServicioMensajeria
        Select Case idTipoServicio
            Case Enumerados.TipoServicio.Reposicion
                objFrabricado = New DetalleMsisdnEnServicioMensajeria
            Case Enumerados.TipoServicio.Siembra
                objFrabricado = New DetalleMsisdnEnServicioMensajeriaTipoSiembra
            Case Else
                objFrabricado = New DetalleMsisdnEnServicioMensajeria
        End Select
        Return objFrabricado
    End Function

#End Region

End Class
