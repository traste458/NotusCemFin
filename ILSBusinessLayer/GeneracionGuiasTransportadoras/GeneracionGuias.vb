Public Class GeneracionGuias
    Public Class Resultado
        Public Codigo As Integer
        Public Mensaje() As String
        Public EsExitoso As Boolean
        Public listaResultado() As ResultadoGuia
    End Class
    Public Class ResultadoGuia
        Public Guia As String
        Public URLGuia As String
        Public ValorFlete As Decimal
        Public ValorSobreFlete As Decimal
        Public ValorOtro As Decimal
        Public PesoFacturado As Decimal
    End Class
    Public Class dtoConexion
        Public NombreTransportadora As String
        Public login As String
        Public pwd As String
        Public idSistemaProceso As Integer
        Public CodigoCuenta As String
        Public idCuentaTransp? As Integer
        Public NombreCuenta As String
        Public CodFacturacion As String
        Public NombreCargue As String
        Public idUsuario As Decimal
        Public idTransportadora As Integer
        Public numGrupoGuia As Decimal
    End Class

    Public Class dtoGuias
        Public idGuia As Decimal
        Public DestinatarioNombre As String
        Public DestinatarioNombreAutorizado As String
        Public DestinatarioIdentificacion As String
        Public DestinatarioTelefono As String
        Public DestinatarioCiudad As String
        Public DestinatarioCiudadNombre As String

        Public DestinatarioDireccion As String
        Public DestinatarioBarrio As String
        Public DestinatarioDireccionObservaciones As String
        Public NotasGuia50caracteres As String

        Public RemiteNombre As String
        Public RemiteNit As String
        Public RemiteDireccion As String
        Public RemiteCiudad As String
        Public RemiteCiudadNombre As String

        Public DiceContener As String
        Public Num_ValorDeclaradoTotal As Decimal

        Public NumeroPiezas As Integer
        Public Num_PesoTotal As Decimal
        Public Num_VolumenTotal As Decimal
        Public UnidadEmpaque As String
        Public Des_MedioTransporte As Integer

        Public idTipoEnvio As Integer
        Public DatoMaterialDetalle() As dtoDetalleMaterial

        Public Guia As String
        Public GuiaBase64 As String
        Public URLGuia As String
        Public ValorFlete As Decimal
        Public ValorSobreFlete As Decimal
        Public ValorOtro As Decimal
        Public PesoFacturado As Decimal

        Public Codigo As Integer
        Public Mensaje() As String
        Public EsExitoso As Boolean
    End Class

    Public Class dtoPaquete
        Public largo As Decimal
        Public ancho As Decimal
        Public alto As Decimal
        Public peso As Decimal
        Public Declarao As Decimal

    End Class

    Public Class dtoDetalleMaterial
        Public idGuia As Decimal
        Public numeroPedido As Decimal
        Public codMaterial As String
        Public descripcion As String
        Public cantidad As Integer
        Public valorUnitario As Decimal
        Public serial As String
    End Class

    Public Class dtoGeneracionGuias
        Public DatoConexion As dtoConexion
        Public DatoGuias() As dtoGuias
        'Public DatoPaquete As dtoPaquete
        Public EsExitoso As Boolean
        Public MensajeError As String
        Public StikerGuia() As dtoStickerGuia
    End Class

    Public Class dtoGenerarGuiaStickerServiEntrega
        Public guia As String
        Public login As String
        Public pwd As String
        Public id_CodFacturacion As String
    End Class
    Public Class dtoStickerGuia
        Public guiaByte As Byte()
        Public guiaURL As String
        Public nombreGuia As String
        Public guiaNumero As String
    End Class
End Class
