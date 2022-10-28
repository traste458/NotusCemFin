Imports LMDataAccessLayer
Imports System.String
Imports ILSBusinessLayer.MensajeriaEspecializada
Imports System.Reflection
Imports System.Web

Public Class Campania

#Region "Atributos"

    Protected Friend _idCampania As Integer
    Protected Friend _nombre As String
    Protected Friend _fechaInicio As Date
    Protected Friend _fechaFin As Date
    Protected Friend _fechaFinGestionCem As Date
    Protected Friend _fechaFinRadicado As Date
    Private _esFinanciero As Integer
    Private _idClienteExterno As Integer
    Private _cliente As String
    Private _nit As String
    Protected Friend _activo As Nullable(Of Boolean)
    Protected Friend _idSistema As Integer
    Private _metaCliente As Integer
    Private _metaCallcenter As Integer
    Private _fechaLlegada As String
    Private _listIdTipoServicio As ArrayList
    Private _listPlanes As List(Of Integer)
    Private _listCallCenters As List(Of Integer)
    Private _listDocumentos As List(Of Short)

#End Region

#Region "Propiedades"

    Public Property IdCampania As Integer
        Get
            Return _idCampania
        End Get
        Set(value As Integer)
            _idCampania = value
        End Set
    End Property

    Public Property Nombre As String
        Get
            Return _nombre
        End Get
        Set(value As String)
            _nombre = value
        End Set
    End Property

    Public Property FechaInicio As Date
        Get
            Return _fechaInicio
        End Get
        Set(value As Date)
            _fechaInicio = value
        End Set
    End Property

    Public Property FechaFin As Date
        Get
            Return _fechaFin
        End Get
        Set(value As Date)
            _fechaFin = value
        End Set
    End Property

    Public Property FechaFinGestionCem As Date
        Get
            Return _fechaFinGestionCem
        End Get
        Set(value As Date)
            _fechaFinGestionCem = value
        End Set
    End Property

    Public Property FechaFinRadicado As Date
        Get
            Return _fechaFinRadicado
        End Get
        Set(value As Date)
            _fechaFinRadicado = value
        End Set
    End Property

    Public Property EsFinanciero As Integer
        Get
            Return _esFinanciero
        End Get
        Set(value As Integer)
            _esFinanciero = value
        End Set
    End Property

    Public Property IdClienteExterno As Integer
        Get
            Return _idClienteExterno
        End Get
        Set(value As Integer)
            _idClienteExterno = value
        End Set
    End Property

    Public Property Cliente As String
        Get
            Return _cliente
        End Get
        Set(value As String)
            _cliente = value
        End Set
    End Property
    Public Property Nit As String
        Get
            Return _nit
        End Get
        Set(value As String)
            _nit = value
        End Set
    End Property

    Public Property Activo As Boolean
        Get
            Return _activo
        End Get
        Set(value As Boolean)
            _activo = value
        End Set
    End Property

    Public Property IdSistema As Integer
        Get
            Return _idSistema
        End Get
        Set(value As Integer)
            _idSistema = value
        End Set
    End Property

    Public Property MetaCliente As Integer
        Get
            Return _metaCliente
        End Get
        Set(value As Integer)
            _metaCliente = value
        End Set
    End Property

    Public Property MetaCallcenter As Integer
        Get
            Return _metaCallcenter
        End Get
        Set(value As Integer)
            _metaCallcenter = value
        End Set
    End Property

    Public Property FechaLlegada As String
        Get
            Return _fechaLlegada
        End Get
        Set(value As String)
            _fechaLlegada = value
        End Set
    End Property

    Public Property ListaPlanes As List(Of Integer)
        Get
            Return _listPlanes
        End Get
        Set(value As List(Of Integer))
            _listPlanes = value
        End Set
    End Property

    Public Property ListaCallCenters As List(Of Integer)
        Get
            Return _listCallCenters
        End Get
        Set(value As List(Of Integer))
            _listCallCenters = value
        End Set
    End Property

    Public Property ListaDocumentos As List(Of Short)
        Get
            Return _listDocumentos
        End Get
        Set(value As List(Of Short))
            _listDocumentos = value
        End Set
    End Property

    Public Property ListaTipoServicio As ArrayList
        Get
            If _listIdTipoServicio Is Nothing Then _listIdTipoServicio = New ArrayList
            Return _listIdTipoServicio
        End Get
        Set(value As ArrayList)
            _listIdTipoServicio = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idCampania As Integer)
        MyBase.New()
        _idCampania = idCampania
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Public Sub CargarDatos()
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idCampania > 0 Then .SqlParametros.Add("@idCampania", SqlDbType.Int).Value = _idCampania
                    If Not String.IsNullOrEmpty(_nombre) Then .SqlParametros.Add("@nombreCampania", SqlDbType.VarChar).Value = _nombre
                    If _activo IsNot Nothing Then .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo

                    .ejecutarReader("ObtieneCampaniasVentas", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idCampania").ToString, _idCampania)
                            _nombre = .Reader("nombre").ToString
                            _fechaInicio = .Reader("fechaInicio")
                            If Not IsDBNull(.Reader("fechaFin")) Then _fechaFin = .Reader("fechaFin")
                            _activo = .Reader("activo")
                            If Not String.IsNullOrEmpty(.Reader("esFinanciero")) Then Integer.TryParse(.Reader("esFinanciero").ToString, _esFinanciero)
                            If Not String.IsNullOrEmpty(.Reader("idClienteExterno")) Then Integer.TryParse(.Reader("idClienteExterno").ToString, _idClienteExterno)
                        End If
                        .Reader.Close()
                    End If
                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Using
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Using dbManager As New LMDataAccess
            Try
                If Not String.IsNullOrEmpty(_nombre) And _fechaInicio > Date.MinValue Then
                    With dbManager
                        .SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = _nombre
                        .SqlParametros.Add("@fechaInicio", SqlDbType.DateTime).Value = _fechaInicio
                        If _fechaFin <> Date.MinValue Then .SqlParametros.Add("@fechaFin", SqlDbType.DateTime).Value = _fechaFin
                        .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo

                        If _listPlanes IsNot Nothing AndAlso _listPlanes.Count > 0 Then _
                            .SqlParametros.Add("@listaPlanes", SqlDbType.VarChar).Value = Join(",", _listPlanes.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                        If _listCallCenters IsNot Nothing AndAlso _listCallCenters.Count > 0 Then _
                            .SqlParametros.Add("@listCallCenters", SqlDbType.VarChar).Value = Join(",", _listCallCenters.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                        If _listDocumentos IsNot Nothing AndAlso _listDocumentos.Count > 0 Then _
                            .SqlParametros.Add("@listDocumentos", SqlDbType.VarChar).Value = Join(",", _listDocumentos.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                        If _listPlanes IsNot Nothing AndAlso _listPlanes.Count > 0 Then _
                            .SqlParametros.Add("@listaPlanes", SqlDbType.VarChar).Value = Join(",", _listPlanes.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                        If _listIdTipoServicio IsNot Nothing AndAlso _listIdTipoServicio.Count > 0 Then
                            Dim arrayTipoServicio() As String = _listIdTipoServicio.ToArray().Select(Function(x) x.ToString()).ToArray()
                            .SqlParametros.Add("@listIdTipoServicio", SqlDbType.VarChar).Value = Join(",", arrayTipoServicio)
                        End If

                        .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                        .IniciarTransaccion()
                        .EjecutarNonQuery("RegistrarCampaniaVenta", CommandType.StoredProcedure)

                        Dim respuesta As Integer = .SqlParametros("@respuesta").Value
                        If respuesta = 0 Then
                            .ConfirmarTransaccion()
                        Else
                            Select Case respuesta
                                Case 1 : resultado.EstablecerMensajeYValor(respuesta, "El nombre de la campaña ya se encuentra registrado")
                            End Select
                            .AbortarTransaccion()
                        End If
                    End With
                Else
                    resultado.EstablecerMensajeYValor(100, "No se proporcionaron los datos suficientes para realizar el registro.")
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return resultado
    End Function

    Public Function Actualizar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Using dbManager As New LMDataAccess
            Try
                If _idCampania > 0 And Not String.IsNullOrEmpty(_nombre) And _fechaInicio > Date.MinValue Then
                    With dbManager
                        .SqlParametros.Add("@idCampania", SqlDbType.Int).Value = _idCampania
                        .SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = _nombre
                        .SqlParametros.Add("@fechaInicio", SqlDbType.DateTime).Value = _fechaInicio
                        If _fechaFin <> Date.MinValue Then .SqlParametros.Add("@fechaFin", SqlDbType.DateTime).Value = _fechaFin
                        .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo

                        If _listPlanes IsNot Nothing AndAlso _listPlanes.Count > 0 Then _
                            .SqlParametros.Add("@listaPlanes", SqlDbType.VarChar).Value = Join(",", _listPlanes.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                        If _listCallCenters IsNot Nothing AndAlso _listCallCenters.Count > 0 Then _
                            .SqlParametros.Add("@listCallCenters", SqlDbType.VarChar).Value = Join(",", _listCallCenters.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                        If _listDocumentos IsNot Nothing AndAlso _listDocumentos.Count > 0 Then _
                            .SqlParametros.Add("@listDocumentos", SqlDbType.VarChar).Value = Join(",", _listDocumentos.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                        If _listIdTipoServicio IsNot Nothing AndAlso _listIdTipoServicio.Count > 0 Then
                            Dim arrayTipoServicio() As String = _listIdTipoServicio.ToArray().Select(Function(x) x.ToString()).ToArray()
                            .SqlParametros.Add("@listIdTipoServicio", SqlDbType.VarChar).Value = Join(",", arrayTipoServicio)
                        End If
                        .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                        .IniciarTransaccion()
                        .EjecutarNonQuery("ActualizarCampaniaVenta", CommandType.StoredProcedure)

                        Dim respuesta As Integer = .SqlParametros("@respuesta").Value
                        If respuesta = 0 Then
                            .ConfirmarTransaccion()
                        Else
                            .AbortarTransaccion()
                        End If
                    End With
                Else
                    resultado.EstablecerMensajeYValor(100, "No se proporcionaron los datos suficientes para actualizar el registro.")
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return resultado
    End Function

    'Public Function Consulta(ByRef dsDatos As DataSet) As ResultadoProceso
    '    Dim resultado As New ResultadoProceso
    '    Dim dbManager As New LMDataAccess
    '    Dim dtDatos As New DataTable

    '    With dbManager
    '        If _idCampania > 0 Then .SqlParametros.Add("@idCampania", SqlDbType.Int).Value = _idCampania
    '        If Not String.IsNullOrEmpty(_nombre) Then .SqlParametros.Add("@nombreCampania", SqlDbType.VarChar).Value = _nombre
    '        If _activo IsNot Nothing Then .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo
    '        If _listIdTipoServicio IsNot Nothing AndAlso _listIdTipoServicio.Count > 0 Then _
    '                .SqlParametros.Add("@listIdTipoServicio", SqlDbType.VarChar).Value = Join(_listIdTipoServicio.ToArray(), ",")
    '        dtDatos = .EjecutarDataTable("ObtieneCampaniasVentas", CommandType.StoredProcedure)
    '    End With
    '    If dtDatos.Rows.Count > 0 Then
    '        resultado.EstablecerMensajeYValor(0, "Registros encontrados satisfactoriamente.")
    '    Else
    '        resultado.EstablecerMensajeYValor(1, "No se encontraron registros disponibles.")
    '    End If
    '    dsDatos.Tables.Add(dtDatos)
    '    Return resultado
    'End Function

#End Region

End Class
