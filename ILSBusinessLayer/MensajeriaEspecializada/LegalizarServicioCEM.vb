Imports LMDataAccessLayer
Imports System.Reflection

Public Class LegalizarServicioCEM

#Region "Atributos (Campos)"

    Private _numeroRadicado As Long
    Private _idServicio As Integer
    Private _idUsuarioLegaliza As Integer
    Private _idDetalle As Integer
    Private _nuevoMsisdn As String
    Private _idTipoNovedad As Integer
    Private _planillaLegalizacion As String
    Private _clienteLegaliza As Enumerados.EstadoBinario
    Private _numeroContrato As Long

    Private _dbManager As New LMDataAccess

#End Region

#Region "Propiedades"

    Public Property NumeroRadicado() As Long
        Get
            Return _numeroRadicado
        End Get
        Set(ByVal value As Long)
            _numeroRadicado = value
        End Set
    End Property

    Public Property IdServicio As Integer
        Get
            Return _idServicio
        End Get
        Set(value As Integer)
            _idServicio = value
        End Set
    End Property

    Public Property IdUsuarioLegaliza() As Integer
        Get
            Return _idUsuarioLegaliza
        End Get
        Set(ByVal value As Integer)
            _idUsuarioLegaliza = value
        End Set
    End Property

    Public Property IdDetalle() As Integer
        Get
            Return _idDetalle
        End Get
        Set(ByVal value As Integer)
            _idDetalle = value
        End Set
    End Property

    Public Property NuevoMsisdn() As String
        Get
            Return _nuevoMsisdn
        End Get
        Set(ByVal value As String)
            _nuevoMsisdn = value
        End Set
    End Property

    Public Property IdTipoNovedad() As Integer
        Get
            Return _idTipoNovedad
        End Get
        Set(ByVal value As Integer)
            _idTipoNovedad = value
        End Set
    End Property

    Public Property PlanillaLegalizacion() As String
        Get
            Return _planillaLegalizacion
        End Get
        Set(ByVal value As String)
            _planillaLegalizacion = value
        End Set
    End Property

    Public Property ClienteLegaliza() As Enumerados.EstadoBinario
        Get
            Return _clienteLegaliza
        End Get
        Set(ByVal value As Enumerados.EstadoBinario)
            _clienteLegaliza = value
        End Set
    End Property

    Public Property NumeroContrato() As Long
        Get
            Return _numeroContrato
        End Get
        Set(value As Long)
            _numeroContrato = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _nuevoMsisdn = ""
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function BuscarDupla() As DataTable
        Dim dtDatos As New DataTable
        If _dbManager IsNot Nothing Then _dbManager = New LMDataAccess
        Try
            With _dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@numeroRadicado", SqlDbType.Int).Value = _numeroRadicado
                End With
                dtDatos = .ejecutarDataTable("ObtenerDuplaLegalizacion", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

    Public Function RegistrarLegalizacion() As List(Of ResultadoProceso)
        Dim lstResultado As New List(Of ResultadoProceso)
        If _dbManager Is Nothing Then _dbManager = New LMDataAccess
        With _dbManager
            With .SqlParametros
                .Clear()
                .Add("@idDetalle", SqlDbType.Int).Value = _idDetalle
                .Add("@idUsuarioLegaliza", SqlDbType.Int).Value = _idUsuarioLegaliza
                If Not String.IsNullOrEmpty(NuevoMsisdn) Then _
                    .Add("@nuevoMsisdn", SqlDbType.VarChar, 20).Value = _nuevoMsisdn
                If IdTipoNovedad > 0 Then _
                    .Add("@idTipoNovedad", SqlDbType.Int).Value = _idTipoNovedad
                If Not String.IsNullOrEmpty(_planillaLegalizacion) Then _
                    .Add("@planillaLegalizacion", SqlDbType.VarChar, 30).Value = _planillaLegalizacion
                If _clienteLegaliza <> Enumerados.EstadoBinario.NoEstablecido Then _
                    .Add("@legalizaCliente", SqlDbType.Bit).Value = IIf(_clienteLegaliza = Enumerados.EstadoBinario.Activo, 1, 0)

                .Add("@return", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
            End With

            .ejecutarReader("RegistrarLegalizacion", CommandType.StoredProcedure)
            If .Reader IsNot Nothing Then
                While .Reader.Read
                    lstResultado.Add(New ResultadoProceso(.Reader("valor").ToString, .Reader("mensaje").ToString))
                End While
                .Reader.Close()
            End If
        End With
        Return lstResultado
    End Function

    Public Function RegistrarLegalizacion(ByVal dtLegalizacion As DataTable) As List(Of ResultadoProceso)
        Dim resultado As New List(Of ResultadoProceso)
        If _dbManager Is Nothing Then _dbManager = New LMDataAccess
        With _dbManager
            Dim sGuid As Guid = Guid.NewGuid()
            Try
                Dim cData As New DataColumn("guid", GetType(Guid))
                cData.DefaultValue = sGuid
                dtLegalizacion.Columns.Add(cData)

                With .SqlParametros
                    .Clear()
                    .Add("@guid", SqlDbType.UniqueIdentifier).Value = sGuid
                End With

                .iniciarTransaccion()
                .inicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "RegistroLegalizacion"
                    .ColumnMappings.Add("guid", "guid")
                    .ColumnMappings.Add("idDetalle", "idDetalle")
                    .ColumnMappings.Add("idUsuarioLegaliza", "idUsuarioLegaliza")
                    .ColumnMappings.Add("nuevoMsisdn", "nuevoMsisdn")
                    .ColumnMappings.Add("planillaLegalizacion", "planillaLegalizacion")
                    .ColumnMappings.Add("ClienteLegaliza", "legalizaCliente")
                    .ColumnMappings.Add("IdTipoNovedad", "idTipoNovedad")
                    .WriteToServer(dtLegalizacion)
                End With

                .ejecutarReader("RegistrarLegalizacionBatch", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    While .Reader.Read
                        resultado.Add(New ResultadoProceso(.Reader("valor").ToString, .Reader("mensaje").ToString))
                    End While
                    .Reader.Close()
                End If
                .confirmarTransaccion()
            Catch ex As Exception
                If _dbManager.estadoTransaccional Then _dbManager.abortarTransaccion()
                Throw ex
            End Try
        End With

        Return resultado
    End Function

    Public Function ValidarRadicado() As List(Of ResultadoProceso)
        Dim lstResultado As New List(Of ResultadoProceso)
        If _dbManager Is Nothing Then _dbManager = New LMDataAccess
        With _dbManager
            With .SqlParametros
                .Clear()
                If _numeroRadicado > 0 Then .Add("@numeroRadicado", SqlDbType.BigInt).Value = _numeroRadicado
                If _idServicio > 0 Then .Add("@idServicio", SqlDbType.Int).Value = IdServicio
                .Add("@return", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
            End With
            .ejecutarReader("ValidarNumeroRadicado", CommandType.StoredProcedure)
            If .Reader IsNot Nothing Then
                While .Reader.Read
                    lstResultado.Add(New ResultadoProceso(.Reader("valor").ToString, .Reader("mensaje").ToString))
                End While
                .Reader.Close()
            End If

        End With
        Return lstResultado
    End Function

    Public Function CerrarRadicado() As List(Of ResultadoProceso)
        Dim lstResultado As New List(Of ResultadoProceso)
        If _dbManager Is Nothing Then _dbManager = New LMDataAccess
        With _dbManager
            With .SqlParametros
                .Clear()
                If _numeroRadicado > 0 Then .Add("@numeroRadicado", SqlDbType.BigInt).Value = _numeroRadicado
                If _idServicio > 0 Then .Add("@idServicio", SqlDbType.Int).Value = _idServicio
                If _numeroContrato > 0 Then .Add("@numeroContrato", SqlDbType.BigInt).Value = _numeroContrato
                If _idUsuarioLegaliza > 0 Then .Add("@idUsuario", SqlDbType.Int).Value = _idUsuarioLegaliza
                .Add("@return", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
            End With
            .ejecutarReader("CerrarNumeroRadicado", CommandType.StoredProcedure)
            If .Reader IsNot Nothing Then
                While .Reader.Read
                    lstResultado.Add(New ResultadoProceso(.Reader("valor").ToString, .Reader("mensaje").ToString))
                End While
                .Reader.Close()
            End If
        End With
        Return lstResultado
    End Function

    Public Shared Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim objDetalleProductoBloqueo As Type = GetType(LegalizarServicioCEM)
        Dim pInfo As PropertyInfo

        For Each pInfo In objDetalleProductoBloqueo.GetProperties
            If pInfo.PropertyType.Namespace = "System" Or pInfo.PropertyType.Namespace = "ILSBusinessLayer.Enumerados" Then
                With dtAux
                    .Columns.Add(pInfo.Name, pInfo.PropertyType)
                End With
            End If
        Next

        Return dtAux
    End Function

#End Region

End Class
