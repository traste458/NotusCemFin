Imports LMDataAccessLayer

Namespace MensajeriaEspecializada

    Public Class ReporteGeneralVentasWebBLL

#Region "Atributos (Filtros de búsqueda)"

        Private _idEstado As Integer
        Private _numRadicado As Long
        Private _listNumRadicado As ArrayList
        Private _msisdn As String
        Private _listMsisdn As ArrayList
        Private _fechaEntregaInicio As DateTime
        Private _fechaEntregaFin As DateTime
        Private _fechaRegistroInicio As DateTime
        Private _fechaRegistroFin As DateTime
        Private _cadena As String
        Private _idUsuario As Integer

#End Region

#Region "Propiedades"

        Public Property IdEstado As Integer
            Get
                Return _idEstado
            End Get
            Set(value As Integer)
                _idEstado = value
            End Set
        End Property

        Public Property NumeroRadicado As Long
            Get
                Return _numRadicado
            End Get
            Set(value As Long)
                _numRadicado = value
            End Set
        End Property

        Public Property ListaNumeroRadicado As ArrayList
            Get
                If _listNumRadicado Is Nothing Then _listNumRadicado = New ArrayList
                Return _listNumRadicado
            End Get
            Set(value As ArrayList)
                _listNumRadicado = value
            End Set
        End Property

        Public Property Msisdn As String
            Get
                Return _msisdn
            End Get
            Set(value As String)
                _msisdn = value
            End Set
        End Property

        Public Property ListaMsisdn As ArrayList
            Get
                If _listMsisdn Is Nothing Then _listMsisdn = New ArrayList
                Return _listNumRadicado
            End Get
            Set(value As ArrayList)
                _listMsisdn = value
            End Set
        End Property

        Public Property FechaEntregaInicio As DateTime
            Get
                Return _fechaEntregaInicio
            End Get
            Set(value As DateTime)
                _fechaEntregaInicio = value
            End Set
        End Property

        Public Property FechaEntregaFin As DateTime
            Get
                Return _fechaEntregaFin
            End Get
            Set(value As DateTime)
                _fechaEntregaFin = value
            End Set
        End Property

        Public Property FechaRegistroInicio As DateTime
            Get
                Return _fechaRegistroInicio
            End Get
            Set(value As DateTime)
                _fechaRegistroInicio = value
            End Set
        End Property

        Public Property FechaRegistroFin As DateTime
            Get
                Return _fechaRegistroFin
            End Get
            Set(value As DateTime)
                _fechaRegistroFin = value
            End Set
        End Property

        Public Property Cadena As String
            Get
                Return _cadena
            End Get
            Set(value As String)
                _cadena = value
            End Set
        End Property

        Public Property IdUsuario As Integer
            Get
                Return _idUsuario
            End Get
            Set(value As Integer)
                _idUsuario = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region "Métodos Púbicos"

        Public Function ObtenerReporte() As DataTable
            Dim dtReporte As New DataTable
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        If _idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        If _numRadicado > 0 Then .SqlParametros.Add("@numRadicado", SqlDbType.Int).Value = _numRadicado
                        If _listNumRadicado IsNot Nothing AndAlso _listNumRadicado.Count > 0 Then .SqlParametros.Add("@listNumRadicado", SqlDbType.VarChar).Value = Join(_listNumRadicado.ToArray, ",")
                        If Not String.IsNullOrEmpty(_msisdn) Then .SqlParametros.Add("@msisdn", SqlDbType.VarChar).Value = _msisdn
                        If _listMsisdn IsNot Nothing AndAlso _listMsisdn.Count > 0 Then .SqlParametros.Add("@listMsisdn", SqlDbType.VarChar).Value = Join(_listMsisdn.ToArray, ",")
                        If _fechaEntregaInicio > Date.MinValue Then .SqlParametros.Add("@fechaEntregaInicio", SqlDbType.DateTime).Value = _fechaEntregaInicio
                        If _fechaEntregaFin > Date.MinValue Then .SqlParametros.Add("@fechaEntregaFin", SqlDbType.DateTime).Value = _fechaEntregaFin
                        If _fechaRegistroInicio > Date.MinValue Then .SqlParametros.Add("@fechaRegistroInicio", SqlDbType.DateTime).Value = _fechaRegistroInicio
                        If _fechaRegistroFin > Date.MinValue Then .SqlParametros.Add("@fechaRegistroFin", SqlDbType.DateTime).Value = _fechaRegistroFin
                        If Not String.IsNullOrEmpty(_cadena) Then .SqlParametros.Add("@cadena", SqlDbType.VarChar).Value = _cadena
                        If _idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario

                        dtReporte = .ejecutarDataTable("ReporteGeneralVentasWeb", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return dtReporte
        End Function

#End Region

    End Class

End Namespace
