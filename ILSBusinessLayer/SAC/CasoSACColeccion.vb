Imports System.IO
Imports LMDataAccessLayer
Imports ILSBusinessLayer.Enumerados

Namespace SAC

    Public Class CasoSACColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idCaso As Integer
        Private _consecutivo As String
        Private _idCliente As Short
        Private _idTipoCliente As Short
        Private _idTipoServicio As Integer
        Private _idClaseServicio As Short
        Private _idRemitente As Integer
        Private _fechaInicial As Date
        Private _fechaFinal As Date
        Private _idTipoFecha As Byte
        Private _idGeneradorInconformidad As Short
        Private _generoCobro As EstadoBinario
        Private _idResponsableCobro As Short
        Private _idTramitador As Integer
        Private _idEstado As Short
        Private _idUsuarioRegistra As Integer
        Private _idUsuarioCierra As Integer
        Private _idUnidadNegocio As Short
        Private _consecutivoServicio As Integer
        Private _minFiltro As String
        Private _numeroRadicado As String
        Private _ArraynumeroRadicado As ArrayList
        Private _ArrayNumerosCaso As ArrayList

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idCaso As Integer)
            MyBase.New()
            _idCaso = idCaso
            CargarDatos()
        End Sub

        Public Sub New(ByVal consecutivo As String)
            MyBase.New()
            _consecutivo = consecutivo
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As CasoSAC
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As CasoSAC)
                If value IsNot Nothing Then
                    Me.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property IdCaso() As Integer
            Get
                Return _idCaso
            End Get
            Set(ByVal value As Integer)
                _idCaso = value
            End Set
        End Property

        Public Property Consecutivo() As String
            Get
                Return _consecutivo
            End Get
            Set(ByVal value As String)
                _consecutivo = value
            End Set
        End Property

        Public Property IdCliente() As Short
            Get
                Return _idCliente
            End Get
            Set(ByVal value As Short)
                _idCliente = value
            End Set
        End Property

        Public Property IdTipoCliente() As Short
            Get

            End Get
            Set(ByVal value As Short)

            End Set
        End Property

        Public Property IdTipoServicio() As Integer
            Get
                Return _idTipoServicio
            End Get
            Set(ByVal value As Integer)
                _idTipoServicio = value
            End Set
        End Property

        Public Property IdClaseServicio() As Short
            Get
                Return _idClaseServicio
            End Get
            Set(ByVal value As Short)
                _idClaseServicio = value
            End Set
        End Property

        Public Property IdRemitente() As Integer
            Get
                Return _idRemitente
            End Get
            Set(ByVal value As Integer)
                _idRemitente = value
            End Set
        End Property

        Public Property FechaInicial() As Date
            Get
                Return _fechaInicial
            End Get
            Set(ByVal value As Date)
                _fechaInicial = value
            End Set
        End Property

        Public Property FechaFinal() As Date
            Get
                Return _fechaFinal
            End Get
            Set(ByVal value As Date)
                _fechaFinal = value
            End Set
        End Property

        Public Property IdTipoFecha() As Byte
            Get
                Return _idTipoFecha
            End Get
            Set(ByVal value As Byte)
                _idTipoFecha = value
            End Set
        End Property

        Public Property IdGeneradorInconformidad() As Integer
            Get
                Return _idGeneradorInconformidad
            End Get
            Set(ByVal value As Integer)
                _idGeneradorInconformidad = value
            End Set
        End Property

        Public Property GeneroCobro() As EstadoBinario
            Get
                Return _generoCobro
            End Get
            Set(ByVal value As EstadoBinario)
                _generoCobro = value
            End Set
        End Property

        Public Property IdResponsableCobro() As Short
            Get
                Return _idResponsableCobro
            End Get
            Set(ByVal value As Short)
                _idResponsableCobro = value
            End Set
        End Property

        Public Property IdTramitador() As Integer
            Get
                Return _idTramitador
            End Get
            Set(ByVal value As Integer)
                _idTramitador = value
            End Set
        End Property

        Public Property IdEstado() As Short
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Short)
                _idEstado = value
            End Set
        End Property

        Public Property IdUsuarioRegistra() As Integer
            Get
                Return _idUsuarioRegistra
            End Get
            Set(ByVal value As Integer)
                _idUsuarioRegistra = value
            End Set
        End Property

        Public Property IdUsuarioCierra() As Integer
            Get
                Return _idUsuarioCierra
            End Get
            Set(ByVal value As Integer)
                _idUsuarioCierra = value
            End Set
        End Property

        Public Property IdUnidadNegocio() As Short
            Get
                Return _idUnidadNegocio
            End Get
            Set(ByVal value As Short)
                _idUnidadNegocio = value
            End Set
        End Property

        Public Property ConsecutivoServicio() As Integer
            Get
                Return _consecutivoServicio
            End Get
            Set(ByVal value As Integer)
                _consecutivoServicio = value
            End Set
        End Property

        Public Property MinFiltro() As String
            Get
                Return _minFiltro
            End Get
            Set(ByVal value As String)
                _minFiltro = value
            End Set
        End Property

        Public Property NumeroRadicado() As String
            Get
                Return _numeroRadicado
            End Get
            Set(ByVal value As String)
                _numeroRadicado = value
            End Set
        End Property

        Public Property ArrayNumeroRadicado() As ArrayList
            Get
                Return _ArraynumeroRadicado
            End Get
            Set(ByVal value As ArrayList)
                _ArraynumeroRadicado = value
            End Set
        End Property

        Public Property ArrayNumerosdeCasos() As ArrayList
            Get
                Return _ArrayNumerosCaso
            End Get
            Set(ByVal value As ArrayList)
                _ArrayNumerosCaso = value
            End Set
        End Property

#End Region

#Region "Métodos Públicos"

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As CasoSAC)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As CasoSAC)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As CasoSACColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As CasoSAC)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function ExisteIdCaso(ByVal idCaso As Integer) As Boolean
            Dim existe As Boolean = False
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), CasoSAC)
                    If .IdCaso = idCaso Then
                        existe = True
                        Exit For
                    End If
                End With
            Next
            Return existe
        End Function

        Public Function ExisteConsecutivo(ByVal consecutivo As String) As Boolean
            Dim existe As Boolean = False
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), CasoSAC)
                    If .Consecutivo = consecutivo Then
                        existe = True
                        Exit For
                    End If
                End With
            Next
            Return existe
        End Function

        Public Function IndiceDe(ByVal idCaso As Integer) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), CasoSAC)
                    If .IdCaso = idCaso Then
                        indice = index
                        Exit For
                    End If
                End With
            Next
            Return indice
        End Function

        Public Function IndiceDe(ByVal consecutivo As String) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), CasoSAC)
                    If .Consecutivo = consecutivo Then
                        indice = index
                        Exit For
                    End If
                End With
            Next
            Return indice
        End Function

        Public Function GenerarDataTable() As DataTable
            Dim dtAux As New DataTable
            Dim drAux As DataRow
            Dim elCaso As CasoSAC

            Dim miTipo As Type = GetType(CasoSAC)
            With dtAux
                .Columns.Add("idCaso", GetType(Integer))
                .Columns.Add("consecutivo", GetType(String))
                .Columns.Add("consecutivoServicio", GetType(Integer))
                .Columns.Add("idCliente", GetType(Short))
                .Columns.Add("cliente", GetType(String))
                .Columns.Add("idTipoServicio", GetType(Integer))
                .Columns.Add("tipoDeServicio", GetType(String))
                .Columns.Add("idClaseDeServicio", GetType(Short))
                .Columns.Add("claseDeServicio", GetType(String))
                .Columns.Add("idRemitente", GetType(Integer))
                .Columns.Add("remitente", GetType(String))
                .Columns.Add("descripcion", GetType(String))
                .Columns.Add("fechaDeRecepcion", GetType(Date))
                .Columns.Add("idGeneradorInconformidad", GetType(Integer))
                .Columns.Add("generadorInconformidad", GetType(String))
                .Columns.Add("respuesta", GetType(String))
                .Columns.Add("fechaRespuesta", GetType(Date))
                .Columns.Add("generoCobro", GetType(Boolean))
                .Columns.Add("valorCobro", GetType(Decimal))
                .Columns.Add("idResponsableCobro", GetType(Short))
                .Columns.Add("responsableCobro", GetType(String))
                .Columns.Add("idTramitador", GetType(Integer))
                .Columns.Add("tramitador", GetType(String))
                .Columns.Add("fechaRegistro", GetType(Date))
                .Columns.Add("idEstado", GetType(Short))
                .Columns.Add("estado", GetType(String))
                .Columns.Add("idUsuarioRegistra", GetType(Integer))
                .Columns.Add("usuarioRegistra", GetType(String))
                .Columns.Add("idUsuarioCierra", GetType(Integer))
                .Columns.Add("usuarioCierra", GetType(String))
                .Columns.Add("fechaCierre", GetType(Date))
                .Columns.Add("observacion", GetType(String))
            End With

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                elCaso = CType(Me.InnerList(index), CasoSAC)
                If elCaso IsNot Nothing Then
                    With elCaso
                        drAux("idCaso") = .IdCaso
                        drAux("consecutivo") = .Consecutivo
                        drAux("consecutivoServicio") = .ConsecutivoServicio
                        drAux("idCliente") = .IdCliente
                        drAux("cliente") = .Cliente
                        drAux("idTipoServicio") = .IdTipoServicio
                        drAux("tipoDeServicio") = .TipoDeServicio
                        drAux("idClaseDeServicio") = .IdClaseServicio
                        drAux("claseDeServicio") = .ClaseDeServicio
                        drAux("idRemitente") = .IdRemitente
                        drAux("remitente") = .Remitente
                        drAux("descripcion") = .Descripcion
                        If .FechaDeRecepcion > Date.MinValue Then drAux("fechaDeRecepcion") = .FechaDeRecepcion
                        drAux("idGeneradorInconformidad") = .IdGeneradorInconformidad
                        drAux("generadorInconformidad") = .GeneradorInconformidad
                        drAux("respuesta") = .Respuesta
                        If .FechaRespuesta > Date.MinValue Then drAux("fechaRespuesta") = .FechaRespuesta
                        drAux("generoCobro") = .GeneroCobro
                        drAux("valorCobro") = .ValorCobro
                        drAux("idResponsableCobro") = .IdResponsableCobro
                        drAux("responsableCobro") = .ResponsableCobro
                        drAux("idTramitador") = .IdTramitador
                        drAux("tramitador") = .Tramitador
                        drAux("fechaRegistro") = .FechaRegistro
                        drAux("idEstado") = .IdEstado
                        drAux("estado") = .Estado
                        drAux("idUsuarioRegistra") = .IdUsuarioRegistra
                        drAux("usuarioRegistra") = .UsuarioRegistra
                        drAux("idUsuarioCierra") = .IdUsuarioCierra
                        drAux("usuarioCierra") = .UsuarioCierra
                        If .FechaCierre > Date.MinValue Then drAux("fechaCierre") = .FechaCierre
                        drAux("observacion") = .Observacion
                        dtAux.Rows.Add(drAux)
                    End With

                End If
            Next
            Return dtAux
        End Function

        Public Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                Me.Clear()
                Dim idPerfil As Integer
                If System.Web.HttpContext.Current.Session("usxp009") IsNot Nothing Then _
                    Integer.TryParse(System.Web.HttpContext.Current.Session("usxp009").ToString(), idPerfil)
                Dim usuarioUnidad As New UsuarioPerfilUnidadNegocio(idPerfil)
                Me._idUnidadNegocio = usuarioUnidad.IdUnidadNegocio
                With dbManager
                    If Me._idCaso > 0 Then .SqlParametros.Add("@idCaso", SqlDbType.Int).Value = Me._idCaso
                    If Me._consecutivo IsNot Nothing AndAlso Me._consecutivo.Trim.Length > 0 Then _
                        .SqlParametros.Add("@consecutivo", SqlDbType.VarChar, 30).Value = Me._consecutivo.Trim
                    If Me._idCliente > 0 Then .SqlParametros.Add("@idCliente", SqlDbType.SmallInt).Value = Me._idCliente
                    If Me._idTipoCliente > 0 Then .SqlParametros.Add("@idTipoCliente", SqlDbType.SmallInt).Value = Me._idTipoCliente
                    If Me._idTipoServicio > 0 Then .SqlParametros.Add("@idTipoServicio", SqlDbType.Int).Value = Me._idTipoServicio
                    If Me._idRemitente > 0 Then .SqlParametros.Add("@idRemitente", SqlDbType.Int).Value = Me._idRemitente
                    If Me._idClaseServicio > 0 Then .SqlParametros.Add("@idClaseServicio", SqlDbType.SmallInt).Value = Me._idClaseServicio
                    If Me._fechaInicial > Date.MinValue OrElse Me._fechaFinal > Date.MinValue Then
                        If Me._fechaInicial > Date.MinValue And Me._fechaFinal = Date.MinValue Then Me._fechaFinal = Me._fechaInicial
                        If Me._fechaInicial = Date.MinValue And Me._fechaFinal > Date.MinValue Then Me._fechaInicial = Me._fechaFinal
                        .SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = Me._fechaInicial
                        .SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = Me._fechaFinal
                    End If
                    If Me._idTipoFecha Then .SqlParametros.Add("@idTipoFecha", SqlDbType.TinyInt).Value = Me._idTipoFecha
                    If Me._idGeneradorInconformidad Then .SqlParametros.Add("@idGeneradorInconformidad", SqlDbType.SmallInt).Value = Me._idGeneradorInconformidad
                    If Me._generoCobro Then .SqlParametros.Add("@generoCobro", SqlDbType.Bit).Value = Me._generoCobro
                    If Me._idResponsableCobro Then .SqlParametros.Add("@idResponsableCobro", SqlDbType.Int).Value = Me._idResponsableCobro
                    If Me._idTramitador Then .SqlParametros.Add("@idTramitador", SqlDbType.Int).Value = Me._idTramitador
                    If Me._idEstado Then .SqlParametros.Add("@idEstado", SqlDbType.SmallInt).Value = Me._idEstado
                    If Me._idUsuarioRegistra Then .SqlParametros.Add("@idUsuarioRegistra", SqlDbType.Int).Value = Me._idUsuarioRegistra
                    If Me._idUsuarioCierra Then .SqlParametros.Add("@idUsuarioCierra", SqlDbType.Int).Value = Me._idUsuarioCierra
                    .SqlParametros.Add("@idUnidadNegocio", SqlDbType.TinyInt).Value = Me._idUnidadNegocio
                    If Me._consecutivoServicio Then .SqlParametros.Add("@consecutivoServicio", SqlDbType.Int).Value = Me._consecutivoServicio
                    If Not String.IsNullOrEmpty(Me._minFiltro) Then .SqlParametros.Add("@serial", SqlDbType.VarChar).Value = Me._minFiltro

                    .ejecutarReader("ConsultarCasoSAC", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim elCaso As CasoSAC
                        Dim idCaso As Integer
                        Dim idClaseServicio As Short
                        Dim fechaAux As Date

                        While .Reader.Read
                            elCaso = New CasoSAC
                            Integer.TryParse(.Reader("idCaso").ToString, idCaso)
                            elCaso.EstablecerIdentificador(idCaso)
                            elCaso.EstablecerConsecutivo(.Reader("consecutivo").ToString)
                            Short.TryParse(.Reader("idCliente").ToString, elCaso.IdCliente)
                            elCaso.EstablecerCliente(.Reader("cliente").ToString)
                            Integer.TryParse(.Reader("idTipoServicio").ToString, elCaso.IdTipoServicio)
                            elCaso.EstablecerTipoDeServicio(.Reader("tipoDeServicio").ToString)
                            Short.TryParse(.Reader("idClaseServicio").ToString, idClaseServicio)
                            elCaso.EstablecerIdClaseServicio(idClaseServicio)
                            elCaso.EstablecerClaseDeServicio(.Reader("claseDeServicio").ToString)
                            Short.TryParse(.Reader("idRemitente").ToString, elCaso.IdRemitente)
                            elCaso.EstablecerRemitente(.Reader("remitente").ToString)
                            elCaso.Descripcion = .Reader("descripcion").ToString
                            Date.TryParse(.Reader("fechaRecepcion").ToString, elCaso.FechaDeRecepcion)
                            Short.TryParse(.Reader("idGeneradorInconformidad").ToString, elCaso.IdGeneradorInconformidad)
                            elCaso.EstablecerGeneradorInconformidad(.Reader("generadorInconformidad").ToString)
                            elCaso.Respuesta = .Reader("respuesta").ToString
                            Date.TryParse(.Reader("fechaRespuesta").ToString, elCaso.FechaRespuesta)
                            Boolean.TryParse(.Reader("generoCobro").ToString, elCaso.GeneroCobro)
                            Decimal.TryParse(.Reader("valorCobro").ToString, elCaso.ValorCobro)
                            Short.TryParse(.Reader("idResponsableCobro").ToString, elCaso.IdResponsableCobro)
                            Integer.TryParse(.Reader("idTramitador").ToString, elCaso.IdTramitador)
                            elCaso.EstablecerTramitador(.Reader("tramitador").ToString)
                            Date.TryParse(.Reader("fechaRegistro").ToString, fechaAux)
                            elCaso.EstablecerFechaRegistro(fechaAux)
                            Short.TryParse(.Reader("idEstado").ToString, elCaso.IdEstado)
                            elCaso.EstablecerEstado(.Reader("estado").ToString)
                            Integer.TryParse(.Reader("idUsuarioRegistra").ToString, elCaso.IdUsuarioRegistra)
                            elCaso.EstablecerUsuarioRegistra(.Reader("usuarioRegistra").ToString)
                            Integer.TryParse(.Reader("idUsuarioCierra").ToString, elCaso.IdUsuarioCierra)
                            Date.TryParse(.Reader("fechaCierre").ToString, fechaAux)
                            elCaso.Observacion = .Reader("observaciones").ToString
                            elCaso.EstablecerFechaCierre(fechaAux)
                            Byte.TryParse(.Reader("idUnidadNegocio").ToString(), elCaso.IdUnidadNegocio)
                            Integer.TryParse(.Reader("consecutivoServicio").ToString(), elCaso.ConsecutivoServicio)
                            elCaso.MarcarComoRegistrado()

                            Me.InnerList.Add(elCaso)
                        End While
                        .Reader.Close()
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Public Function CargarReporteCasosSacCem(ByVal nombreArchivo As String, ByVal pNombrePlantilla As String) As InfoResultado

            Dim _resul As New InfoResultado
            Try
                Dim dtDatos As DataTable
                Me.Clear()
                Using dbManager As New LMDataAccess
                    With dbManager
                        With .SqlParametros
                            If Me._ArrayNumerosCaso IsNot Nothing AndAlso Me._ArrayNumerosCaso.Count > 0 Then .Add("@consecutivo", SqlDbType.VarChar, 30).Value = Join(Me.ArrayNumerosdeCasos.ToArray, ",")
                            If Me._idTipoCliente > 0 Then .Add("@idTipoCliente", SqlDbType.SmallInt).Value = Me._idTipoCliente
                            If Me._idTipoServicio > 0 Then .Add("@idTipoServicio", SqlDbType.Int).Value = Me._idTipoServicio
                            If Me._idClaseServicio > 0 Then .Add("@idClaseServicio", SqlDbType.SmallInt).Value = Me._idClaseServicio
                            If Me._ArraynumeroRadicado IsNot Nothing AndAlso _ArraynumeroRadicado.Count > 0 Then .Add("@numeroRadicado", SqlDbType.SmallInt).Value = Join(Me._ArraynumeroRadicado.ToArray, ",")
                            If Me._fechaInicial > Date.MinValue OrElse Me._fechaFinal > Date.MinValue Then
                                If Me._fechaInicial > Date.MinValue And Me._fechaFinal = Date.MinValue Then Me._fechaFinal = Me._fechaInicial
                                If Me._fechaInicial = Date.MinValue And Me._fechaFinal > Date.MinValue Then Me._fechaInicial = Me._fechaFinal
                                .Add("@fechaInicial", SqlDbType.SmallDateTime).Value = Me._fechaInicial
                                .Add("@fechaFinal", SqlDbType.SmallDateTime).Value = Me._fechaFinal
                                If Me._idTipoFecha Then .Add("@idTipoFecha", SqlDbType.TinyInt).Value = Me._idTipoFecha
                            End If
                            If Me._idEstado > 0 Then .Add("@idEstado", SqlDbType.SmallInt).Value = Me._idEstado
                            If Me._idUnidadNegocio > 0 Then .Add("@idUnidadNegocio", SqlDbType.SmallInt).Value = Me._idUnidadNegocio
                        End With
                        .TiempoEsperaComando = 600
                        _resul = .GenerarArchivoExcel("ReporteCasosSacCEM", nombreArchivo, CommandType.StoredProcedure, pNombrePlantilla, "Reporte Caso SAC", 4)

                        ' dtDatos = dbManager.ejecutarDataTable("ReporteCasosSacCEM", CommandType.StoredProcedure)
                    End With
                End Using
                Return _resul
            Catch ex As Exception
                Throw New Exception("Imposible recuperar los filtros de búsqueda aplicados" + ex.Message)
            End Try
        End Function

        Public Function CargarReporteCasosSacCem() As DataTable
            Dim dbManager As New LMDataAccess

            Try
                Dim dtDatos As DataTable
                Me.Clear()
                With dbManager
                    If Me._ArrayNumerosCaso IsNot Nothing AndAlso Me._ArrayNumerosCaso.Count > 0 Then .SqlParametros.Add("@consecutivo", SqlDbType.VarChar, 30).Value = Join(Me.ArrayNumerosdeCasos.ToArray, ",")
                    If Me._idTipoCliente > 0 Then .SqlParametros.Add("@idTipoCliente", SqlDbType.SmallInt).Value = Me._idTipoCliente
                    If Me._idTipoServicio > 0 Then .SqlParametros.Add("@idTipoServicio", SqlDbType.Int).Value = Me._idTipoServicio
                    If Me._idClaseServicio > 0 Then .SqlParametros.Add("@idClaseServicio", SqlDbType.SmallInt).Value = Me._idClaseServicio
                    If Me._ArraynumeroRadicado IsNot Nothing AndAlso _ArraynumeroRadicado.Count > 0 Then .SqlParametros.Add("@numeroRadicado", SqlDbType.SmallInt).Value = Join(Me._ArraynumeroRadicado.ToArray, ",")
                    If Me._fechaInicial > Date.MinValue OrElse Me._fechaFinal > Date.MinValue Then
                        If Me._fechaInicial > Date.MinValue And Me._fechaFinal = Date.MinValue Then Me._fechaFinal = Me._fechaInicial
                        If Me._fechaInicial = Date.MinValue And Me._fechaFinal > Date.MinValue Then Me._fechaInicial = Me._fechaFinal
                        .SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = Me._fechaInicial
                        .SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = Me._fechaFinal
                        If Me._idTipoFecha Then .SqlParametros.Add("@idTipoFecha", SqlDbType.TinyInt).Value = Me._idTipoFecha
                    End If
                    If Me._idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.SmallInt).Value = Me._idEstado
                    If Me._idEstado > 0 Then .SqlParametros.Add("@idUnidadNegocio", SqlDbType.SmallInt).Value = Me._idUnidadNegocio

                    dtDatos = dbManager.EjecutarDataTable("ReporteCasosSacCEM", CommandType.StoredProcedure)
                End With
                Return dtDatos
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Function

        Public Function ObtenerDatosReporteCEM() As DataTable
            Dim dbManager As New LMDataAccess
            Dim dt As New DataTable
            Try
                Me.Clear()
                Dim idPerfil As Integer
                If System.Web.HttpContext.Current.Session("usxp009") IsNot Nothing Then _
                    Integer.TryParse(System.Web.HttpContext.Current.Session("usxp009").ToString(), idPerfil)
                Dim usuarioUnidad As New UsuarioPerfilUnidadNegocio(idPerfil)
                Me._idUnidadNegocio = usuarioUnidad.IdUnidadNegocio
                With dbManager
                    If Me._idCaso > 0 Then .SqlParametros.Add("@idCaso", SqlDbType.Int).Value = Me._idCaso
                    If Me._consecutivo IsNot Nothing AndAlso Me._consecutivo.Trim.Length > 0 Then _
                        .SqlParametros.Add("@consecutivo", SqlDbType.VarChar, 30).Value = Me._consecutivo.Trim
                    If Me._idCliente > 0 Then .SqlParametros.Add("@idCliente", SqlDbType.SmallInt).Value = Me._idCliente
                    If Me._idTipoCliente > 0 Then .SqlParametros.Add("@idTipoCliente", SqlDbType.SmallInt).Value = Me._idTipoCliente
                    If Me._idTipoServicio > 0 Then .SqlParametros.Add("@idTipoServicio", SqlDbType.Int).Value = Me._idTipoServicio
                    If Me._idRemitente > 0 Then .SqlParametros.Add("@idRemitente", SqlDbType.Int).Value = Me._idRemitente
                    If Me._idClaseServicio > 0 Then .SqlParametros.Add("@idClaseServicio", SqlDbType.SmallInt).Value = Me._idClaseServicio
                    If Me._fechaInicial > Date.MinValue OrElse Me._fechaFinal > Date.MinValue Then
                        If Me._fechaInicial > Date.MinValue And Me._fechaFinal = Date.MinValue Then Me._fechaFinal = Me._fechaInicial
                        If Me._fechaInicial = Date.MinValue And Me._fechaFinal > Date.MinValue Then Me._fechaInicial = Me._fechaFinal
                        .SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = Me._fechaInicial
                        .SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = Me._fechaFinal
                    End If
                    If Me._idTipoFecha Then .SqlParametros.Add("@idTipoFecha", SqlDbType.TinyInt).Value = Me._idTipoFecha
                    If Me._idGeneradorInconformidad Then .SqlParametros.Add("@idGeneradorInconformidad", SqlDbType.SmallInt).Value = Me._idGeneradorInconformidad
                    If Me._generoCobro Then .SqlParametros.Add("@generoCobro", SqlDbType.Bit).Value = Me._generoCobro
                    If Me._idResponsableCobro Then .SqlParametros.Add("@idResponsableCobro", SqlDbType.Int).Value = Me._idResponsableCobro
                    If Me._idTramitador Then .SqlParametros.Add("@idTramitador", SqlDbType.Int).Value = Me._idTramitador
                    If Me._idEstado Then .SqlParametros.Add("@idEstado", SqlDbType.SmallInt).Value = Me._idEstado
                    If Me._idUsuarioRegistra Then .SqlParametros.Add("@idUsuarioRegistra", SqlDbType.Int).Value = Me._idUsuarioRegistra
                    If Me._idUsuarioCierra Then .SqlParametros.Add("@idUsuarioCierra", SqlDbType.Int).Value = Me._idUsuarioCierra
                    If Me._idUnidadNegocio Then .SqlParametros.Add("@idUnidadNegocio", SqlDbType.TinyInt).Value = Me._idUnidadNegocio
                    If Me._consecutivoServicio Then .SqlParametros.Add("@consecutivoServicio", SqlDbType.Int).Value = Me._consecutivoServicio
                    If Not String.IsNullOrEmpty(Me._minFiltro) Then .SqlParametros.Add("@serial", SqlDbType.VarChar).Value = Me._minFiltro

                    dt = .EjecutarDataTable("ConsultarCasoSACCEM", CommandType.StoredProcedure)

                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dt
        End Function

#End Region

#Region "Métodos Compartidos"

        Public Shared Function ObtenerTodosEnDataTable() As DataTable
            Dim dtAux As New DataTable
            Dim dbManager As New LMDataAccess
            Dim idPerfil As Integer
            Try
                If System.Web.HttpContext.Current.Session("usxp009") IsNot Nothing Then _
                    Integer.TryParse(System.Web.HttpContext.Current.Session("usxp009").ToString(), idPerfil)
                Dim usuarioUnidad As New UsuarioPerfilUnidadNegocio(idPerfil)
                dbManager.SqlParametros.Add("@idUnidadNegocio", SqlDbType.TinyInt).Value = usuarioUnidad.IdUnidadNegocio
                dtAux = dbManager.EjecutarDataTable("ConsultarCasoSAC", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtAux
        End Function

#End Region

    End Class

End Namespace
