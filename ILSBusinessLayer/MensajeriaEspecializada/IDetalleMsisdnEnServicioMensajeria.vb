Imports LMDataAccessLayer

Public Interface IDetalleMsisdnEnServicioMensajeria

#Region "Propiedades"

    Property IdRegistro() As Integer

    Property IdServicioMensajeria() As Integer

    Property IdTipoServicio() As Integer

    Property MSISDN() As String

    Property ActivaEquipoAnterior() As Boolean

    Property ActivaEquipoAnteriorTexto() As String

    Property Comseguro() As Boolean

    Property ComseguroTexto() As String

    Property PrecioConIva() As Double

    Property PrecioSinIva() As Double
    
    Property NumeroReserva As String

    Property IdClausula() As Integer

    Property Clausula() As String

    Property Lista28() As Boolean

    Property Lista28Texto As String

    Property IdPlan As Integer

    Property NombrePlan As String

    Property NombreRegion As String

    Property FechaDevolucion As Date

    Property CantidadMaterial() As Integer

    Property CantidadMaterialLeida() As Integer

    Property Bloquear() As Boolean

    Property Registrado() As Boolean

#End Region

#Region "Métodos Públicos"

    Function Adicionar(Optional objDataAccess As LMDataAccess = Nothing) As ResultadoProceso

    Sub Modificar()

    Sub Eliminar()

    Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)

#End Region

End Interface
