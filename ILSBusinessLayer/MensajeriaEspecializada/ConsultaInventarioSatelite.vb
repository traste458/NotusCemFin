Imports LMDataAccessLayer
Imports ILSBusinessLayer
Imports ILSBusinessLayer.Enumerados

Public Class ConsultaInventarioSatelite

#Region "Atributos (Campos)"

    Private _idCiudad As Integer
    Private _idBodega As Integer
    Private _bodega As String
    Private _centro As String
    Private _almacen As String
    Private _material As String
    Private _idEstado As Integer
    Private _listaBodegas As ArrayList
    Private _idUsuario As Int32
    Private _nombreArchivo As String
    Private _nombrePlantilla As String
    Private _resultado As New InfoResultado
#End Region

#Region "Propiedades"

    Public Property IdCiudad() As Integer
        Get
            Return _idCiudad
        End Get
        Set(ByVal value As Integer)
            _idCiudad = value
        End Set
    End Property

    Public Property IdBodega() As Integer
        Get
            Return _idBodega
        End Get
        Set(ByVal value As Integer)
            _idBodega = value
        End Set
    End Property

    Public Property Bodega() As String
        Get
            Return _bodega
        End Get
        Set(ByVal value As String)
            _bodega = value
        End Set
    End Property

    Public Property Centro() As String
        Get
            Return _centro
        End Get
        Set(ByVal value As String)
            _centro = value
        End Set
    End Property

    Public Property Almacen() As String
        Get
            Return _almacen
        End Get
        Set(ByVal value As String)
            _almacen = value
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

    Public Property IdEstado() As Integer
        Get
            Return _idEstado
        End Get
        Set(ByVal value As Integer)
            _idEstado = value
        End Set
    End Property

    Public Property ListaBodega As ArrayList
        Get
            If _listaBodegas Is Nothing Then _listaBodegas = New ArrayList
            Return _listaBodegas
        End Get
        Set(value As ArrayList)
            _listaBodegas = value
        End Set
    End Property

    Public Property NombreArchivo() As String
        Get
            Return _nombreArchivo
        End Get
        Set(ByVal value As String)
            _nombreArchivo = value
        End Set
    End Property
    Public Property NombrePlantilla() As String
        Get
            Return _nombrePlantilla
        End Get
        Set(ByVal value As String)
            _nombrePlantilla = value
        End Set
    End Property


    Public Property Resultado() As InfoResultado
        Get
            Return _resultado
        End Get
        Set(ByVal value As InfoResultado)
            _resultado = value
        End Set
    End Property

    Public Property idUsuario() As Int32
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Int32)
            _idUsuario = value
        End Set
    End Property
#End Region

#Region "Constructores"

   
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idBodega As Integer)
        MyBase.New()
        _idBodega = idBodega
    End Sub

#End Region

#Region "Métodos"

    Public Function GenerarDataTable(ByVal serializada As Boolean, ByRef cantidadRegistros As Int32) As DataTable
        Dim dtDatos As New DataTable

        Using dbManager = New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Clear()
                        If _idCiudad > 0 Then .Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                        If _idBodega > 0 Then .Add("@idBodega", SqlDbType.Int).Value = _idBodega
                        If _listaBodegas IsNot Nothing AndAlso _listaBodegas.Count > 0 Then _
                        .Add("@listaBodega", SqlDbType.VarChar).Value = Join(_listaBodegas.ToArray, ",")
                        If _centro IsNot Nothing AndAlso _centro <> "" Then .Add("@centro", SqlDbType.VarChar).Value = _centro
                        If _almacen IsNot Nothing AndAlso _almacen <> "" Then .Add("@almacen", SqlDbType.VarChar).Value = _almacen
                        If _material IsNot Nothing AndAlso _material <> "" And _material <> "0" Then .Add("@material", SqlDbType.VarChar).Value = _material
                        If _idEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        If Not serializada Then .Add("@CantidadRegistros", SqlDbType.Int).Direction = ParameterDirection.Output
                    End With
                    If serializada Then

                        .TiempoEsperaComando = 0
                        _resultado = .GenerarArchivoExcel("ConsultaInventarioSateliteSerializado", NombreArchivo, CommandType.StoredProcedure, NombrePlantilla, "INVENTARIO SERIALIZADDO", 4)
                        'dtDatos = .ejecutarDataTable("ConsultaInventarioSateliteSerializado", CommandType.StoredProcedure)
                    Else
                        .TiempoEsperaComando = 0
                        dtDatos = .ejecutarDataTable("ConsultaInventarioSatelite", CommandType.StoredProcedure)
                        Integer.TryParse(.SqlParametros("@CantidadRegistros").Value.ToString(), cantidadRegistros)
                    End If

                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Using

        Return dtDatos
    End Function

    Public Function ConsultarInventarioProductoFinancieros(ByVal serializada As Boolean, ByRef cantidadRegistros As Int32) As DataTable
        Dim dtDatos As New DataTable

        Using dbManager = New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Clear()
                        If _idCiudad > 0 Then .Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                        If _idBodega > 0 Then .Add("@idBodega", SqlDbType.Int).Value = _idBodega
                        If _listaBodegas IsNot Nothing AndAlso _listaBodegas.Count > 0 Then _
                        .Add("@listaBodega", SqlDbType.VarChar).Value = Join(_listaBodegas.ToArray, ",")
                        If _centro IsNot Nothing AndAlso _centro <> "" Then .Add("@centro", SqlDbType.VarChar).Value = _centro
                        If _almacen IsNot Nothing AndAlso _almacen <> "" Then .Add("@almacen", SqlDbType.VarChar).Value = _almacen
                        If _material IsNot Nothing AndAlso _material <> "" And _material <> "0" Then .Add("@material", SqlDbType.VarChar).Value = _material
                        If _idEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        If Not serializada Then .Add("@CantidadRegistros", SqlDbType.Int).Direction = ParameterDirection.Output
                    End With
                    If serializada Then

                        .TiempoEsperaComando = 0
                        _resultado = .GenerarArchivoExcel("ConsultaInventarioSateliteServicioFinancieroSerializado", NombreArchivo, CommandType.StoredProcedure, NombrePlantilla, "INVENTARIO SERIALIZADDO", 4)
                        'dtDatos = .ejecutarDataTable("ConsultaInventarioSateliteSerializado", CommandType.StoredProcedure)
                    Else
                        dtDatos = .ejecutarDataTable("ConsultaInventarioSateliteServicioFinanciero", CommandType.StoredProcedure)
                        Integer.TryParse(.SqlParametros("@CantidadRegistros").Value.ToString(), cantidadRegistros)
                    End If

                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Using

        Return dtDatos
    End Function
#End Region

End Class
