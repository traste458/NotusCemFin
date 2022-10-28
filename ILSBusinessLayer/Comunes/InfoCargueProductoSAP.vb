Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer
Imports LMDataAccessLayer

Namespace Comunes
    Public Class InfoCargueProductoSAP

#Region "Variables privadas"

        Private _idInfoCargue As Integer
        Private _idCargue As Integer
        Private _serial As String
        Private _idRegion As Integer
        Private _idProducto As Integer
        Private _material As String
        Private _lote As String        
        Private _fechaRegistro As DateTime
        Private _idOrdenRecepcion As Integer
        Private _fechaCargue As DateTime

#End Region

#Region "Propiedades Publicas"

        Public Property IdInfoCargue() As Integer
            Get
                Return _idInfoCargue
            End Get
            Set(ByVal value As Integer)
                _idInfoCargue = value
            End Set
        End Property

        Public Property IdCargue() As Integer
            Get
                Return _idCargue
            End Get
            Set(ByVal value As Integer)
                _idCargue = value
            End Set
        End Property

        Public Property Serial() As String
            Get
                Return _serial
            End Get
            Set(ByVal value As String)
                _serial = value
            End Set
        End Property

        Public Property IdRegion() As Integer
            Get
                Return _idRegion
            End Get
            Set(ByVal value As Integer)
                _idRegion = value
            End Set
        End Property

        Public Property IdProducto() As Integer
            Get
                Return _idProducto
            End Get
            Set(ByVal value As Integer)
                _idProducto = value
            End Set
        End Property

        Public Property Material() As String
            Get
                Return _material
            End Get
            Set(ByVal value As String)
                _material = value
            End Set
        End Property

        Public Property Lote() As String
            Get
                Return _lote
            End Get
            Set(ByVal value As String)
                _lote = value
            End Set
        End Property

        Public Property FechaRegistro() As DateTime
            Get
                Return _fechaRegistro
            End Get
            Set(ByVal value As DateTime)
                _fechaRegistro = value
            End Set
        End Property

        Public Property IdOrdenRecepcion() As Integer
            Get
                Return _idOrdenRecepcion
            End Get
            Set(ByVal value As Integer)
                _idOrdenRecepcion = value
            End Set
        End Property

        Public Property FechaCargue() As DateTime
            Get
                Return _fechaCargue
            End Get
            Set(ByVal value As DateTime)
                _fechaCargue = value
            End Set
        End Property

#End Region

#Region "Estructuras"

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idInfoCargue As Integer)
            Me.New()
            Me.CargarDatos(idInfoCargue)
        End Sub

#End Region

#Region "Metodos Privados"

        Private Sub CargarDatos(ByVal idInfoCargue As Integer)
            Dim db As New LMDataAccess()
            Try
                db.SqlParametros.Add("@idInfoCargue", SqlDbType.Int).Value = idInfoCargue
                db.ejecutarReader("ObtenerInfoCargueProductoSAP", CommandType.StoredProcedure)
                If db.Reader IsNot Nothing AndAlso db.Reader.Read Then
                    _idInfoCargue = CInt(db.Reader("idInfoCargue"))
                    _idCargue = CInt(db.Reader("idCargue"))
                    _serial = db.Reader("serial").ToString()
                    Integer.TryParse(db.Reader("idRegion").ToString(), _idRegion)
                    _idProducto = CInt(db.Reader("idProducto"))
                    _material = db.Reader("material").ToString()
                    _lote = db.Reader("lote").ToString()                    
                    DateTime.TryParse(db.Reader("fechaRegistro").ToString(), _fechaRegistro)
                    _idOrdenRecepcion = CInt(db.Reader("idOrdenRecepcion"))
                    DateTime.TryParse(db.Reader("fechaCargue").ToString(), _fechaCargue)
                End If
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                db.Dispose()
            End Try
        End Sub

#End Region

#Region "Metodos Publicos"

        Public Function Crear()
        End Function

        Public Function Actualizar()
        End Function

#End Region

#Region "Metodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New Estructuras.FiltroInfoCargueProductoSAP
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As Estructuras.FiltroInfoCargueProductoSAP) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdInfoCargue > 0 Then db.SqlParametros.Add("@idInfoCargue", SqlDbType.Int).Value = .IdInfoCargue
                If .IdCargue > 0 Then db.SqlParametros.Add("@idCargue", SqlDbType.Int).Value = .IdCargue
                If .Serial <> "" Then db.SqlParametros.Add("@serial", SqlDbType.VarChar).Value = .Serial
                If .IdRegion > 0 Then db.SqlParametros.Add("@idRegion", SqlDbType.Int).Value = .IdRegion
                If .IdProducto > 0 Then db.SqlParametros.Add("@idProducto", SqlDbType.Int).Value = .IdProducto
                If .Material <> "" Then db.SqlParametros.Add("@material", SqlDbType.VarChar).Value = .Material
                If .Lote <> "" Then db.SqlParametros.Add("@lote", SqlDbType.VarChar).Value = .Lote
                If .FechaRegistroInicial > DateTime.MinValue Then db.SqlParametros.Add("@fechaRegistroInicial", SqlDbType.DateTime).Value = .FechaRegistroInicial
                If .FechaRegistroFinal > DateTime.MinValue Then db.SqlParametros.Add("@fechaRegistroFinal", SqlDbType.DateTime).Value = .FechaRegistroFinal
                If .IdOrdenRecepcion > 0 Then db.SqlParametros.Add("@idOrdenRecepcion", SqlDbType.Int).Value = .IdOrdenRecepcion
                If .FechaCargueInicial > DateTime.MinValue Then db.SqlParametros.Add("@fechaCargueInicial", SqlDbType.DateTime).Value = .FechaCargueInicial
                If .FechaCargueFinal > DateTime.MinValue Then db.SqlParametros.Add("@fechaCargueFinal", SqlDbType.DateTime).Value = .FechaCargueFinal
                If .Cargado <> Enumerados.EstadoBinario.NoEstablecido Then db.SqlParametros.Add("@cargado", SqlDbType.Bit).Value = .Cargado
                dtDatos = db.ejecutarDataTable("ObtenerInfoCargueProductoSAP", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos
        End Function

#End Region

    End Class
End Namespace
