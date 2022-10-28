Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports System.IO

Public Class GestionNovedadProduccionColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idNovedad As Integer
    Private _idFacturaGuia As Integer
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

    Default Public Property Item(ByVal index As Integer) As GestionNovedadProduccion
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As GestionNovedadProduccion)
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

    Public Property IdFacturaGuia As Integer
        Get
            Return _idFacturaGuia
        End Get
        Set(value As Integer)
            _idFacturaGuia = value
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
        Dim mNovedadProduccion As Type = GetType(GestionNovedadProduccion)
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As GestionNovedadProduccion)
        Me.InnerList.Insert(posicion, valor)
        If Not _registrado Then _registrado = True
    End Sub

    Public Sub Add(ByVal valor As GestionNovedadProduccion)
        Me.InnerList.Add(valor)
        If Not _registrado Then _registrado = True
    End Sub

    Public Sub AdicionarRango(ByVal rango As GestionNovedadProduccionColeccion)
        Me.InnerList.AddRange(rango)
        If Not _registrado Then _registrado = True
    End Sub

    Public Sub Remover(ByVal valor As GestionNovedadProduccion)
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
            With CType(Me.InnerList(index), GestionNovedadProduccion)
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
        Dim miDetalle As GestionNovedadProduccion

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miDetalle = CType(Me.InnerList(index), GestionNovedadProduccion)
            If miDetalle IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(GestionNovedadProduccion).GetProperties
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
                If _idFacturaGuia > 0 Then .SqlParametros.Add("@idFacturaGuia", SqlDbType.Int).Value = _idFacturaGuia
                .ejecutarReader("ObtenerGestionDeNovedadDeProduccion", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    Dim obj As GestionNovedadProduccion
                    While .Reader.Read
                        obj = New GestionNovedadProduccion
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

    Public Shared Sub SerializeObject(ByVal filename As String, ByVal col As GestionNovedadProduccion, ByVal ExtraTypes() As System.Type)
        Dim s As New Xml.Serialization.XmlSerializer(GetType(GestionNovedadProduccion), ExtraTypes)
        Dim writer As New StreamWriter(filename)

        s.Serialize(writer, col)
        writer.Close()
    End Sub

    Public Shared Sub SerializeObject(ByVal filename As String, ByVal col As GestionNovedadProduccion)
        Dim s As New Xml.Serialization.XmlSerializer(GetType(GestionNovedadProduccion))
        Dim writer As New StreamWriter(filename)

        s.Serialize(writer, col)
        writer.Close()
    End Sub

    Public Shared Function DeserializeObject(ByVal filename As String, ByVal ExtraTypes() As System.Type) As GestionNovedadProduccion

        Dim fs As New IO.FileStream(filename, FileMode.Open)
        Dim w As New Xml.Serialization.XmlSerializer(GetType(GestionNovedadProduccion), ExtraTypes)
        Dim g As GestionNovedadProduccion = CType(w.Deserialize(fs), GestionNovedadProduccion)

        fs.Close()

        Return g
    End Function

    Public Shared Function DeserializeObject(ByVal filename As String) As GestionNovedadProduccion
        Dim fs As New IO.FileStream(filename, FileMode.Open)
        Dim w As New Xml.Serialization.XmlSerializer(GetType(GestionNovedadProduccion))
        Dim g As GestionNovedadProduccion = CType(w.Deserialize(fs), GestionNovedadProduccion)

        fs.Close()

        Return g
    End Function

#End Region

End Class
