Imports LMDataAccessLayer

Public Class CargueMasivoVentasServiciosFinancieros

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

    Public Sub New(informacionServicio As DataTable, dtDetalleReferencias As DataTable)
        MyBase.New()
        _dtInformacionGeneral = informacionServicio
        _dtDetalleReferencias = dtDetalleReferencias
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function Cargar(ByVal idUsuario As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Using dbManager As New LMDataAccess
            Try
                _dtInformacionGeneral.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), idUsuario))

                _dtDetalleReferencias.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), idUsuario))

                With dbManager
                    With .SqlParametros
                        .Clear()
                        .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    End With
                    .EjecutarNonQuery("EliminaformacionGeneral_Carga_ServiciosFinancieros", CommandType.StoredProcedure)

                    .IniciarTransaccion()
                    .InicilizarBulkCopy()
                    If (_dtDetalleReferencias IsNot Nothing AndAlso _dtDetalleReferencias.Rows.Count > 0) Then

                        With .BulkCopy
                            .DestinationTableName = "DetalleReferencias_Carga_ServiciosFinancieros"
                            .ColumnMappings.Add("Identificacion", "Identificacion")
                            .ColumnMappings.Add("material", "material")
                            .ColumnMappings.Add("cantidad", "cantidad")
                            .ColumnMappings.Add("idUsuario", "idUsuario")
                            .WriteToServer(_dtDetalleReferencias)
                        End With
                    End If

                    .InicilizarBulkCopy()
                    With .BulkCopy
                        .DestinationTableName = "InformacionGeneral_Carga_ServiciosFinancieros"
                        .ColumnMappings.Add("TipoServicio", "TipoServicio")
                        .ColumnMappings.Add("Campania", "Campania")
                        .ColumnMappings.Add("Empresa", "Empresa")
                        .ColumnMappings.Add("Identificacion", "Identificacion")
                        .ColumnMappings.Add("Ciudad", "Ciudad")
                        .ColumnMappings.Add("Departamento", "Departamento")
                        .ColumnMappings.Add("direccion", "direccion")
                        .ColumnMappings.Add("fechaAsignacion", "fechaAsignacion")
                        .ColumnMappings.Add("fechaAgenda", "fechaAgenda")
                        .ColumnMappings.Add("idJornada", "idJornada")
                        .ColumnMappings.Add("nombre", "nombre")
                        .ColumnMappings.Add("nombreAutorizado", "nombreAutorizado")
                        .ColumnMappings.Add("barrio", "barrio")
                        .ColumnMappings.Add("telefono", "telefono")
                        .ColumnMappings.Add("tipoTelefono", "tipoTelefono")
                        .ColumnMappings.Add("observacion", "observacion")
                        .ColumnMappings.Add("Fila", "Fila")
                        .ColumnMappings.Add("idUsuario", "idUsuario")
                        .WriteToServer(_dtInformacionGeneral)
                    End With

                    With .SqlParametros
                        .Clear()
                        .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output

                    End With

                    _dtResultado = .EjecutarDataTable("RegistroMasivoServiciosFinancieros", CommandType.StoredProcedure)
                    resultado.Valor = CType(.SqlParametros("@resultado").Value.ToString, Integer)

                    If resultado.Valor = 0 Then
                        .ConfirmarTransaccion()
                    Else
                        .AbortarTransaccion()
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw ex
            End Try
        End Using
        Return resultado
    End Function

#End Region

End Class
