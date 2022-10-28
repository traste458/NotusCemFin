Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras

Namespace OMS

    Public Class ReporteNoConformesFacturas

#Region "Atributos (Campos)"

        Private _facturas As String
        Private _fechaInicial As Date
        Private _fechaFinal As Date
        Private _idProducto As Integer
        Private _soloProductoVirgen As Enumerados.EstadoBinario
        Private _datosReporte As DataTable

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _facturas = ""
            _soloProductoVirgen = Enumerados.EstadoBinario.NoEstablecido
        End Sub

#End Region

#Region "Propiedades"

        Public Property Facturas() As String
            Get
                Return _facturas
            End Get
            Set(ByVal value As String)
                _facturas = value
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

        Public ReadOnly Property DatosReporte() As DataTable
            Get
                If _datosReporte Is Nothing Then CargarDatos()
                Return _datosReporte
            End Get
        End Property

        Public Property IdProducto() As Integer
            Get
                Return _idProducto
            End Get
            Set(ByVal value As Integer)
                _idProducto = value
            End Set
        End Property

        Public Property SoloProductoVirgen() As Enumerados.EstadoBinario
            Get
                Return _soloProductoVirgen
            End Get
            Set(ByVal value As Enumerados.EstadoBinario)
                _soloProductoVirgen = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Public Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _facturas IsNot Nothing AndAlso _facturas.Trim.Length > 0 Then _
                        .SqlParametros.Add("@facturas", SqlDbType.VarChar, 8000).Value = _facturas
                    If _fechaInicial > Date.MinValue Then .SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = _fechaInicial
                    If _fechaFinal > Date.MinValue Then .SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = _fechaFinal
                    If _idProducto > 0 Then .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = Me._idProducto
                    If _soloProductoVirgen <> Enumerados.EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@soloProductoVirgen", SqlDbType.Bit).Value = IIf(_soloProductoVirgen = Enumerados.EstadoBinario.Activo, 1, 0)
                    _datosReporte = .ejecutarDataTable("ReporteNoConformesFacturas", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

#End Region

    End Class

End Namespace