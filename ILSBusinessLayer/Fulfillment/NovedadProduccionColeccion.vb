Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports System.IO

Public Class NovedadProduccionColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idNovedad As Integer
    Private _idFacturaGuia As Long
    Private _factura As String
    Private _guia As String
    Private _idOrdenRecepcion As Long
    Private _ordenCompra As String
    Private _idProducto As Long
    Private _idSubproducto As Long
    Private _contentType As String
    Private _descripcion As String
    Private _fechaRegistro As Date
    Private _idUsuarioRegistra As Integer
    Private _usuarioRegistra As String
    Private _fechaSolucion As Date
    Private _idUsuarioSoluciona As Integer
    Private _usuarioSoluciona As String
    Private _fechaFacturaInicial As Date
    Private _fechaFacturaFinal As Date
    Private _fechaNovedadInicial As Date
    Private _fechaNovedadFinal As Date
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idNovedad As Integer)
        Me.New()
        _idNovedad = idNovedad
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As NovedadProduccion
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As NovedadProduccion)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdNovedad As Integer
        Get
            Return _idNovedad
        End Get
        Set(ByVal value As Integer)
            _idNovedad = value
        End Set
    End Property

    Public Property IdFacturaGuia As Long
        Get
            Return _idFacturaGuia
        End Get
        Set(ByVal value As Long)
            _idFacturaGuia = value
        End Set
    End Property

    Public Property Factura As String
        Get
            Return _factura
        End Get
        Set(value As String)
            _factura = value
        End Set
    End Property

    Public Property Guia As String
        Get
            Return _guia
        End Get
        Set(value As String)
            _guia = value
        End Set
    End Property

    Public Property IdOrdenRecepcion As Long
        Get
            Return _idOrdenRecepcion
        End Get
        Set(ByVal value As Long)
            _idOrdenRecepcion = value
        End Set
    End Property

    Public Property OrdenCompra As String
        Get
            Return _ordenCompra
        End Get
        Set(value As String)
            _ordenCompra = value
        End Set
    End Property

    Public Property IdProducto As Long
        Get
            Return _idProducto
        End Get
        Set(value As Long)
            _idProducto = value
        End Set
    End Property

    Public Property IdSubproducto As Long
        Get
            Return _idSubproducto
        End Get
        Set(value As Long)
            _idSubproducto = value
        End Set
    End Property

    Public Property ContentType As String
        Get
            Return _contentType
        End Get
        Set(value As String)
            _contentType = value
        End Set
    End Property

    Public Property Descripcion As String
        Get
            Return _descripcion
        End Get
        Set(ByVal value As String)
            _descripcion = value
        End Set
    End Property

    Public Property FechaRegistro As Date
        Get
            Return _fechaRegistro
        End Get
        Set(ByVal value As Date)
            _fechaRegistro = value
        End Set
    End Property

    Public Property IdUsuarioRegistra As Integer
        Get
            Return _idUsuarioRegistra
        End Get
        Set(ByVal value As Integer)
            _idUsuarioRegistra = value
        End Set
    End Property

    Public Property UsuarioRegistra As String
        Get
            Return _usuarioRegistra
        End Get
        Set(value As String)
            _usuarioRegistra = value
        End Set
    End Property

    Public Property FechaSolucion As Date
        Get
            Return _fechaSolucion
        End Get
        Set(ByVal value As Date)
            _fechaSolucion = value
        End Set
    End Property

    Public Property UsuarioSoluciona As String
        Get
            Return _usuarioSoluciona
        End Get
        Set(value As String)
            _usuarioSoluciona = value
        End Set
    End Property

    Public Property IdUsuarioSoluciona As Integer
        Get
            Return _idUsuarioSoluciona
        End Get
        Set(ByVal value As Integer)
            _idUsuarioSoluciona = value
        End Set
    End Property

    Public Property FechaFacturaInicial As Date
        Get
            Return _fechaFacturaInicial
        End Get
        Set(value As Date)
            _fechaFacturaFinal = value
        End Set
    End Property

    Public Property FechaFacturaFinal As Date
        Get
            Return _fechaFacturaFinal
        End Get
        Set(value As Date)
            _fechaFacturaFinal = value
        End Set
    End Property

    Public Property FechaNovedadInicial As Date
        Get
            Return _fechaNovedadInicial
        End Get
        Set(value As Date)
            _fechaNovedadInicial = value
        End Set
    End Property

    Public Property FechaNovedadFinal As Date
        Get
            Return _fechaNovedadFinal
        End Get
        Set(value As Date)
            _fechaNovedadFinal = value
        End Set
    End Property

    Public ReadOnly Property Registrado As Boolean
        Get
            Return _registrado
        End Get
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim mNovedadProduccion As Type = GetType(NovedadProduccion)
        Dim pInfo As PropertyInfo

        For Each pInfo In mNovedadProduccion.GetProperties
            If pInfo.PropertyType.Namespace = "System" Then
                With dtAux
                    .Columns.Add(pInfo.Name, pInfo.PropertyType)
                End With
            End If
        Next

        Return dtAux
    End Function

#End Region

#Region "Métodos Públicos"

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As NovedadProduccion)
        Me.InnerList.Insert(posicion, valor)
        If Not _registrado Then _registrado = True
    End Sub

    Public Sub Add(ByVal valor As NovedadProduccion)
        Me.InnerList.Add(valor)
        If Not _registrado Then _registrado = True
    End Sub

    Public Sub AdicionarRango(ByVal rango As NovedadProduccionColeccion)
        Me.InnerList.AddRange(rango)
        If Not _registrado Then _registrado = True
    End Sub

    Public Sub Remover(ByVal valor As NovedadProduccion)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub

    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub

    Public Function IndiceDe(ByVal identificador As Integer) As Integer
        Dim indice As Integer = -1
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), NovedadProduccion)
                If .IdNovedad = identificador Then
                    indice = index
                    Exit For
                End If
            End With
        Next
        Return indice
    End Function

    Public Function GenerarDataTable() As DataTable
        If Not _registrado AndAlso Me.InnerList.Count = 0 Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miDetalle As NovedadProduccion

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miDetalle = CType(Me.InnerList(index), NovedadProduccion)
            If miDetalle IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(NovedadProduccion).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(miDetalle, Nothing)
                    End If
                Next
                dtAux.Rows.Add(drAux)
            End If
        Next

        Return dtAux
    End Function

    Public Sub CargarDatos()
        Using dbManager As New LMDataAccess
            With dbManager
                If _idNovedad > 0 Then .SqlParametros.Add("@idNovedad", SqlDbType.Int).Value = _idNovedad
                If _idOrdenRecepcion > 0 Then .SqlParametros.Add("@idOrdenRecepcion", SqlDbType.Int).Value = _idOrdenRecepcion
                If _guia <> Nothing Then .SqlParametros.Add("@guia", SqlDbType.VarChar).Value = _guia
                If _factura <> Nothing Then .SqlParametros.Add("@factura", SqlDbType.VarChar).Value = _factura
                If _idProducto > 0 Then .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = _idProducto
                If IdSubproducto > 0 Then .SqlParametros.Add("@idSubproducto", SqlDbType.Int).Value = _idSubproducto
                If _fechaFacturaInicial <> Date.MinValue Then .SqlParametros.Add("@fechaInicialFactura", SqlDbType.Date).Value = _fechaFacturaInicial
                If _fechaFacturaFinal <> Date.MinValue Then .SqlParametros.Add("@fechaFinalFactura", SqlDbType.Date).Value = _fechaFacturaFinal
                If _fechaNovedadInicial <> Date.MinValue Then .SqlParametros.Add("@fechaInicialNovedad", SqlDbType.Date).Value = _fechaNovedadInicial
                If _fechaNovedadFinal <> Date.MinValue Then .SqlParametros.Add("@fechaFinalNovedad", SqlDbType.Date).Value = _fechaNovedadFinal
                .ejecutarReader("ObtenerInformacionDeNovedadDeProduccion", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    Dim obj As NovedadProduccion
                    While .Reader.Read
                        obj = New NovedadProduccion
                        obj.CargarValorDePropiedades(.Reader)
                        Me.InnerList.Add(obj)
                    End While
                    .Reader.Close()
                End If
            End With
            _registrado = True
        End Using
    End Sub

#End Region

#Region "Métodos Compartidos"

    Public Shared Sub SerializeObject(ByVal filename As String, ByVal col As NovedadProduccion, ByVal ExtraTypes() As System.Type)
        Dim s As New Xml.Serialization.XmlSerializer(GetType(NovedadProduccion), ExtraTypes)
        Dim writer As New StreamWriter(filename)

        s.Serialize(writer, col)
        writer.Close()
    End Sub

    Public Shared Sub SerializeObject(ByVal filename As String, ByVal col As NovedadProduccion)
        Dim s As New Xml.Serialization.XmlSerializer(GetType(NovedadProduccion))
        Dim writer As New StreamWriter(filename)

        s.Serialize(writer, col)
        writer.Close()
    End Sub

    Public Shared Function DeserializeObject(ByVal filename As String, ByVal ExtraTypes() As System.Type) As NovedadProduccion

        Dim fs As New IO.FileStream(filename, FileMode.Open)
        Dim w As New Xml.Serialization.XmlSerializer(GetType(NovedadProduccion), ExtraTypes)
        Dim g As NovedadProduccion = CType(w.Deserialize(fs), NovedadProduccion)

        fs.Close()

        Return g
    End Function

    Public Shared Function DeserializeObject(ByVal filename As String) As NovedadProduccion
        Dim fs As New IO.FileStream(filename, FileMode.Open)
        Dim w As New Xml.Serialization.XmlSerializer(GetType(NovedadProduccion))
        Dim g As NovedadProduccion = CType(w.Deserialize(fs), NovedadProduccion)

        fs.Close()

        Return g
    End Function

#End Region

End Class
