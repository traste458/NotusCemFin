Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports System.IO

Public Class DetalleHistoricoServicioTecnicoColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"
    Private _serial As String
    Private _listaSeriales As ArrayList
#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As DetalleHistoricoServicioTecnico
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As DetalleHistoricoServicioTecnico)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property Serial As String
        Get
            Return _serial
        End Get
        Set(value As String)
            _serial = value
        End Set
    End Property

    Public Property ListaSeriales As ArrayList
        Get
            If _listaSeriales Is Nothing Then _listaSeriales = New ArrayList
            Return _listaSeriales
        End Get
        Set(value As ArrayList)
            _listaSeriales = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim obj As Type = GetType(DetalleHistoricoServicioTecnico)
        Dim pInfo As PropertyInfo

        For Each pInfo In obj.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As DetalleHistoricoServicioTecnico)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Add(ByVal valor As DetalleHistoricoServicioTecnico)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As DetalleHistoricoServicioTecnicoColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As DetalleHistoricoServicioTecnico)
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
            With CType(Me.InnerList(index), DetalleHistoricoServicioTecnico)
                If .Serial = identificador Then
                    indice = index
                    Exit For
                End If
            End With
        Next
        Return indice
    End Function

    Public Function GenerarDataTable() As DataTable
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim infoObj As DetalleHistoricoServicioTecnico

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            infoObj = CType(Me.InnerList(index), DetalleHistoricoServicioTecnico)
            If infoObj IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(DetalleHistoricoServicioTecnico).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(infoObj, Nothing)
                    End If
                Next
                dtAux.Rows.Add(drAux)
            End If
        Next

        Return dtAux
    End Function

    Public Sub CargarDatos()
        Using dbManager As New LMDataAccess
            Me.Clear()
            With dbManager
                Me.InnerList.Clear()
                Dim _seriales As String = ""
                For i As Integer = 0 To _listaSeriales.Count - 1
                    If _seriales.Trim.Length = 0 Then
                        _seriales = _listaSeriales(i).item(0)
                    Else
                        _seriales = _seriales & "," & _listaSeriales(i).item(0)
                    End If
                Next
                If _listaSeriales IsNot Nothing AndAlso _listaSeriales.Count > 0 Then _
                                .SqlParametros.Add("@listaSerial", SqlDbType.VarChar).Value = _seriales
                .ejecutarReader("ObtenerDetalleHistoricoServicioTecnico", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    Dim infoObj As DetalleHistoricoServicioTecnico
                    While .Reader.Read
                        infoObj = New DetalleHistoricoServicioTecnico
                        infoObj.AsignarValorAPropiedades(.Reader)
                        Me.InnerList.Add(infoObj)
                    End While
                    .Reader.Close()
                End If
            End With
        End Using
    End Sub

#End Region

#Region "Métodos Compartidos"

    Public Shared Sub SerializeObject(ByVal filename As String, ByVal col As DetalleHistoricoServicioTecnicoColeccion, ByVal ExtraTypes() As System.Type)
        Dim s As New Xml.Serialization.XmlSerializer(GetType(DetalleHistoricoServicioTecnicoColeccion), ExtraTypes)
        Dim writer As New StreamWriter(filename)

        s.Serialize(writer, col)
        writer.Close()
    End Sub

    Public Shared Sub SerializeObject(ByVal filename As String, ByVal col As DetalleHistoricoServicioTecnicoColeccion)
        Dim s As New Xml.Serialization.XmlSerializer(GetType(DetalleHistoricoServicioTecnicoColeccion))
        Dim writer As New StreamWriter(filename)

        s.Serialize(writer, col)
        writer.Close()
    End Sub

    Public Shared Function DeserializeObject(ByVal filename As String, ByVal ExtraTypes() As System.Type) As DetalleHistoricoServicioTecnicoColeccion

        Dim fs As New IO.FileStream(filename, FileMode.Open)
        Dim w As New Xml.Serialization.XmlSerializer(GetType(DetalleHistoricoServicioTecnicoColeccion), ExtraTypes)
        Dim g As DetalleHistoricoServicioTecnicoColeccion = CType(w.Deserialize(fs), DetalleHistoricoServicioTecnicoColeccion)

        fs.Close()

        Return g
    End Function

    Public Shared Function DeserializeObject(ByVal filename As String) As DetalleHistoricoServicioTecnicoColeccion
        Dim fs As New IO.FileStream(filename, FileMode.Open)
        Dim w As New Xml.Serialization.XmlSerializer(GetType(DetalleHistoricoServicioTecnicoColeccion))
        Dim g As DetalleHistoricoServicioTecnicoColeccion = CType(w.Deserialize(fs), DetalleHistoricoServicioTecnicoColeccion)

        fs.Close()

        Return g
    End Function

#End Region

End Class

