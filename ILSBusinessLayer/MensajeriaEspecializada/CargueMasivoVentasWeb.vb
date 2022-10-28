Imports LMDataAccessLayer

Public Class CargueMasivoVentasWeb

#Region "Atributos"

    Private _dtDetalleReferencias As DataTable
    Private _dtInformacionGeneral As DataTable
    Private _dtResultado As DataTable

#End Region

#Region "Propiedades"

    Public Property Referencias As DataTable
        Get
            Return _dtDetalleReferencias
        End Get
        Set(value As DataTable)
            _dtDetalleReferencias = value
        End Set
    End Property

    Public Property InformacionGeneral As DataTable
        Get
            Return _dtInformacionGeneral
        End Get
        Set(value As DataTable)
            _dtInformacionGeneral = value
        End Set
    End Property

    Public Property DatosResultado As DataTable
        Get
            Return _dtResultado
        End Get
        Set(value As DataTable)
            _dtResultado = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _dtDetalleReferencias = New DataTable()
        _dtInformacionGeneral = New DataTable()
    End Sub

    Public Sub New(ByVal referencias As DataTable, informacionServicio As DataTable)
        MyBase.New()
        _dtDetalleReferencias = referencias
        _dtInformacionGeneral = informacionServicio
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function Cargar(ByVal idUsuario As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Using dbManager As New LMDataAccess
            Try
                _dtDetalleReferencias.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), idUsuario))
                _dtInformacionGeneral.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), idUsuario))

                With dbManager
                    With .SqlParametros
                        .Clear()
                        .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    End With
                    .ejecutarNonQuery("EliminaRegistroMasivoServicioMensajeria", CommandType.StoredProcedure)

                    .iniciarTransaccion()
                    .inicilizarBulkCopy()
                    With .BulkCopy
                        .DestinationTableName = "DetalleReferencias_Equipos_Carga_Radicados"
                        .ColumnMappings.Add("NUMERO DE RADICADO", "numeroRadicado")
                        .ColumnMappings.Add("MATERIAL", "material")
                        .ColumnMappings.Add("CANTIDAD", "cantidad")
                        .ColumnMappings.Add("idUsuario", "idUsuario")
                        .WriteToServer(_dtDetalleReferencias)
                    End With
                    
                    .inicilizarBulkCopy()
                    With .BulkCopy
                        .DestinationTableName = "InformacionGeneral_Carga_Radicados"
                        .ColumnMappings.Add("TIPO DE SERVICIO", "TipoServicio")
                        .ColumnMappings.Add("PRIORIDAD", "idPrioridad")
                        .ColumnMappings.Add("CADENA", "usuarioEjecutor")
                        .ColumnMappings.Add("IDENTIFICACION", "identicacion")
                        .ColumnMappings.Add("CIUDAD", "Ciudad")
                        .ColumnMappings.Add("DEPARTAMENTO", "Departamento")
                        .ColumnMappings.Add("DIRECCION", "direccion")
                        .ColumnMappings.Add("FECHA DE ASIGNACION", "fechaAsignacion")
                        .ColumnMappings.Add("NUMERO DE RADICADO", "numeroRadicado")
                        .ColumnMappings.Add("FECHA MAXIMA ENTREGA", "fechaVencimientoReserva")
                        .ColumnMappings.Add("NOMBRE CLIENTE", "nombre")
                        .ColumnMappings.Add("PERSONA AUTORIZADA", "nombreAutorizado")
                        .ColumnMappings.Add("BARRIO", "barrio")
                        .ColumnMappings.Add("TELEFONO", "telefono")
                        .ColumnMappings.Add("TIPO DE TELEFONO", "tipoTelefono")
                        .ColumnMappings.Add("OBSERVACIONES (OPCIONAL)", "observacion")
                        .ColumnMappings.Add("idUsuario", "idUsuario")
                        .WriteToServer(_dtInformacionGeneral)
                    End With

                    With .SqlParametros
                        .Clear()
                        .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output

                    End With

                    _dtResultado = .ejecutarDataTable("RegistroMasivoServicioTipoVentaWeb", CommandType.StoredProcedure)
                    resultado.Valor = CType(.SqlParametros("@resultado").Value.ToString, Integer)

                    If resultado.Valor = 0 Then
                        .confirmarTransaccion()
                    Else
                        .abortarTransaccion()
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw ex
            End Try
        End Using
        Return resultado
    End Function

#End Region

End Class
