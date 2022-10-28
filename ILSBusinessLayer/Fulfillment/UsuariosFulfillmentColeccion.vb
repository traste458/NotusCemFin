Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports System.IO

Public Class UsuariosFulfillmentColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _nombreUsuario As String
    Private _numeroCedula As Integer
    Private _estado As Boolean
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal Estado As Boolean)
        Me.New()
        _estado = Estado
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As UsuariosFulfillment
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As UsuariosFulfillment)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property NombreUsuario As String
        Get
            Return _nombreUsuario
        End Get
        Set(value As String)
            _nombreUsuario = value
        End Set
    End Property

    Public Property NumeroCedula As Integer
        Get
            Return _numeroCedula
        End Get
        Set(value As Integer)
            _numeroCedula = value
        End Set
    End Property

    Public Property Estado As Boolean
        Get
            Return _estado
        End Get
        Set(value As Boolean)
            _estado = value
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
        Dim mUsuariosFulfillment As Type = GetType(UsuariosFulfillment)
        Dim pInfo As PropertyInfo

        For Each pInfo In mUsuariosFulfillment.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As UsuariosFulfillment)
        Me.InnerList.Insert(posicion, valor)
        If Not _registrado Then _registrado = True
    End Sub

    Public Sub Add(ByVal valor As UsuariosFulfillment)
        Me.InnerList.Add(valor)
        If Not _registrado Then _registrado = True
    End Sub

    Public Sub AdicionarRango(ByVal rango As UsuariosFulfillmentColeccion)
        Me.InnerList.AddRange(rango)
        If Not _registrado Then _registrado = True
    End Sub

    Public Sub Remover(ByVal valor As UsuariosFulfillment)
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
            With CType(Me.InnerList(index), UsuariosFulfillment)
                If .IdUsuarioFulfillment = identificador Then
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
        Dim miDetalle As UsuariosFulfillment

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miDetalle = CType(Me.InnerList(index), UsuariosFulfillment)
            If miDetalle IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(UsuariosFulfillment).GetProperties
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
                If _nombreUsuario <> Nothing Then .SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = _nombreUsuario
                If _numeroCedula > 0 Then .SqlParametros.Add("@cedula", SqlDbType.Int).Value = _numeroCedula
                .SqlParametros.Add("@estado", SqlDbType.Bit).Value = _estado
                .ejecutarReader("ObtenerInformacionDeUsuariosDeFulfillment", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    Dim obj As UsuariosFulfillment
                    While .Reader.Read
                        obj = New UsuariosFulfillment
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

    Public Shared Sub SerializeObject(ByVal filename As String, ByVal col As UsuariosFulfillment, ByVal ExtraTypes() As System.Type)
        Dim s As New Xml.Serialization.XmlSerializer(GetType(UsuariosFulfillment), ExtraTypes)
        Dim writer As New StreamWriter(filename)

        s.Serialize(writer, col)
        writer.Close()
    End Sub

    Public Shared Sub SerializeObject(ByVal filename As String, ByVal col As UsuariosFulfillment)
        Dim s As New Xml.Serialization.XmlSerializer(GetType(UsuariosFulfillment))
        Dim writer As New StreamWriter(filename)

        s.Serialize(writer, col)
        writer.Close()
    End Sub

    Public Shared Function DeserializeObject(ByVal filename As String, ByVal ExtraTypes() As System.Type) As UsuariosFulfillment

        Dim fs As New IO.FileStream(filename, FileMode.Open)
        Dim w As New Xml.Serialization.XmlSerializer(GetType(UsuariosFulfillment), ExtraTypes)
        Dim g As UsuariosFulfillment = CType(w.Deserialize(fs), UsuariosFulfillment)

        fs.Close()

        Return g
    End Function

    Public Shared Function DeserializeObject(ByVal filename As String) As UsuariosFulfillment
        Dim fs As New IO.FileStream(filename, FileMode.Open)
        Dim w As New Xml.Serialization.XmlSerializer(GetType(UsuariosFulfillment))
        Dim g As UsuariosFulfillment = CType(w.Deserialize(fs), UsuariosFulfillment)

        fs.Close()

        Return g
    End Function

#End Region

End Class
