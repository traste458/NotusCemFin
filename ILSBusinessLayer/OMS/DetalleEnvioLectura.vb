Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras

Namespace OMS

    Public Class DetalleEnvioLectura

#Region "variables"
        Private _idDetalleEnvioLectura As Long
        Private _idOrdenEnvioLectura As Long
        Private _material As String
        Private _producto As String
        Private _idOrdenTrabajo As Integer
        Private _ordenTrabajo As String
        Private _infoDetalleEnvio As DataTable

#End Region

#Region "propiedades"

        Public ReadOnly Property IdDetalleEnvioLectura() As String
            Get
                Return _idDetalleEnvioLectura
            End Get
        End Property

        Public Property IdOrdenEnvioLectura() As Long
            Get
                Return _idOrdenEnvioLectura
            End Get
            Set(ByVal value As Long)
                _idOrdenEnvioLectura = value
            End Set
        End Property

        Public Property IdOrdenTrabajo() As Long
            Get
                Return _idOrdenTrabajo
            End Get
            Set(ByVal value As Long)
                _idOrdenTrabajo = value
            End Set
        End Property

        Public ReadOnly Property Producto() As String
            Get
                Return _producto
            End Get
        End Property

        Public ReadOnly Property OrdenTrabajo() As String
            Get
                Return _ordenTrabajo
            End Get
        End Property

        Public ReadOnly Property InfoDetalleEnvio() As DataTable
            Get
                If _infoDetalleEnvio Is Nothing Then CargarListadoDetalleEnvio()
                Return _infoDetalleEnvio
            End Get
        End Property

#End Region

#Region "constructores"
        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idDetalleEnvioLectura As Long)
            MyBase.New()
            _idOrdenEnvioLectura = idDetalleEnvioLectura
            CargarInformacion()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            If _idDetalleEnvioLectura <> 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idDetalleEnvioLectura", SqlDbType.Int).Value = _idOrdenEnvioLectura
                        .ejecutarReader("ObtenerInfoDetalle", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                _material = .Reader("material").ToString
                                _ordenTrabajo = .Reader("ordenTrabajo").ToString
                                Integer.TryParse(.Reader("idOrdenTrabajo").ToString, _idOrdenTrabajo)
                            End If
                            .Reader.Close()
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
        End Sub

        Private Sub CargarListadoDetalleEnvio()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idOrdenEnvioLectura", SqlDbType.Int).Value = _idOrdenEnvioLectura
                    _infoDetalleEnvio = .ejecutarDataTable("ObtenerInfoDetalleEnvio", CommandType.StoredProcedure)
                End With
                If _infoDetalleEnvio.PrimaryKey.Count = 0 Then
                    Dim pkColumn(0) As DataColumn
                    pkColumn(0) = _infoDetalleEnvio.Columns("idDetalleEnvioLectura")
                    _infoDetalleEnvio.PrimaryKey = pkColumn
                End If
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

#End Region

#Region "Metodos Compartidos"

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroDetalleEnvioLectura) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDatos As New DataTable()
            Try
                With dbManager
                    With .SqlParametros
                        If filtro.IdDetalleEnvioLectura > 0 Then .Add("@idDetalleEnvioLectura", SqlDbType.BigInt).Value = filtro.IdDetalleEnvioLectura
                        If filtro.IdOrdenEnvioLectura > 0 Then .Add("@idOrdenEnvioLectura", SqlDbType.BigInt).Value = filtro.IdOrdenEnvioLectura
                        If filtro.IdOrdenTrabajo > 0 Then .Add("@idOrdenTrabajo", SqlDbType.Int).Value = filtro.IdOrdenTrabajo
                    End With
                    .TiempoEsperaComando = 300  'timeout de 5 min 
                    dtDatos = .ejecutarDataTable("ObtenerInfoDetalleEnvio", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtDatos
        End Function

#End Region
    End Class

End Namespace