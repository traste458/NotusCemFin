Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras

Namespace OMS

    Public Class SerialesNCRecuperados

#Region "Campos"

        Private _serial As String
        Private _idOrden As Integer
        Private _idNoConformidad As Integer
        Private _idUsuarioCreador As Integer
        Private _fechaNoConformidad As Date
        Private _idUsuarioRecuperacion As Integer
        Private _fechaRecuperacion As Date
        Private _usuarioCreador As String
        Private _usuarioRecuperacion As String
        Private _noConformidad As String
        Private _ordenTrabajo As String

#End Region

#Region "Propiedades"

        Public ReadOnly Property Serial() As String
            Get
                Return _serial
            End Get
        End Property

        Public Property IdOrden() As Integer
            Get
                Return _idOrden
            End Get
            Set(ByVal value As Integer)
                _idOrden = value
            End Set
        End Property

        Public Property IdNoConformidad() As Integer
            Get
                Return _idNoConformidad
            End Get
            Set(ByVal value As Integer)
                _idNoConformidad = value
            End Set
        End Property

        Public Property IdUsuarioCreador() As Integer
            Get
                Return _idUsuarioCreador
            End Get
            Set(ByVal value As Integer)
                _idUsuarioCreador = value
            End Set
        End Property

        Public Property FechaNoConformidad() As Date
            Get
                Return _fechaNoConformidad
            End Get
            Set(ByVal value As Date)
                _fechaNoConformidad = value
            End Set
        End Property

        Public Property IdUsuarioRecuperacion() As Integer
            Get
                Return _idUsuarioRecuperacion
            End Get
            Set(ByVal value As Integer)
                _idUsuarioRecuperacion = value
            End Set
        End Property

        Public Property FechaRecuperacion() As Date
            Get
                Return _fechaRecuperacion
            End Get
            Set(ByVal value As Date)
                _fechaRecuperacion = value
            End Set
        End Property

        Public Property UsuarioCreador() As String
            Get
                Return _usuarioCreador
            End Get
            Set(ByVal value As String)
                _usuarioCreador = value
            End Set
        End Property

        Public Property UsuarioRecuperacion() As String
            Get
                Return _usuarioRecuperacion
            End Get
            Set(ByVal value As String)
                _usuarioRecuperacion = value
            End Set
        End Property

        Public Property NoConformidad() As String
            Get
                Return _noConformidad
            End Get
            Set(ByVal value As String)
                _noConformidad = value
            End Set
        End Property

        Public Property OrdenTrabajo() As String
            Get
                Return _ordenTrabajo
            End Get
            Set(ByVal value As String)
                _ordenTrabajo = value
            End Set
        End Property

#End Region

#Region "Contructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            _serial = Serial
            CargarInformacion()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            If _serial > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@serial", SqlDbType.VarChar, 20).Value = _serial
                        .ejecutarReader("ObtenerInfoSerialesNCRecuperados", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                Integer.TryParse(.Reader("idOrden").ToString, _idOrden)
                                _ordenTrabajo = .Reader("ordenTrabajo").ToString()
                                Integer.TryParse(.Reader("idnoconformidad").ToString(), _idNoConformidad)
                                _noConformidad = .Reader("noConformidad").ToString()
                                Integer.TryParse(.Reader("idUsuarioCreador").ToString, _idUsuarioCreador)
                                _usuarioCreador = .Reader("usuarioCreador").ToString()
                                Date.TryParse(.Reader("fechaNoConformidad").ToString, _fechaNoConformidad)
                                Integer.TryParse(.Reader("idUsuarioRecuperacion").ToString, _idUsuarioRecuperacion)
                                _usuarioRecuperacion = .Reader("usuarioRecuperacion").ToString()
                                Date.TryParse(.Reader("fechaRecuperacion").ToString, _fechaRecuperacion)
                            End If

                            If Not .Reader.IsClosed Then .Reader.Close()
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
        End Sub

#End Region


#Region "Métodos Públicos"

#End Region


#Region "Métodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroSerialesNCRecuperados
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroSerialesNCRecuperados) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdOrden > 0 Then db.SqlParametros.Add("@idOrden", SqlDbType.BigInt).Value = .IdOrden
                If .IdFabricante > 0 Then db.SqlParametros.Add("@idFabricante", SqlDbType.Int).Value = .IdFabricante
                If .IdProveedor > 0 Then db.SqlParametros.Add("@idProveedor", SqlDbType.Int).Value = .IdProveedor
                If .IdFactura > 0 Then db.SqlParametros.Add("@IdFactura", SqlDbType.Int).Value = .IdFactura
                If .TipoFecha > 0 Then db.SqlParametros.Add("@tipoFecha", SqlDbType.TinyInt).Value = .TipoFecha
                If .FechaInicial <> Date.MinValue Then db.SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = .FechaInicial
                If .FechaFinal <> Date.MinValue Then db.SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = .FechaFinal

                db.TiempoEsperaComando = 300 'timeout de 5 min 
                dtDatos = db.ejecutarDataTable("ObtenerInfoSerialesNCRecuperados", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos
        End Function

#End Region

    End Class

End Namespace
