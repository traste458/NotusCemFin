Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports System.IO

Public Class wsHistoricoServicioTecnicoColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"
    Private _registrado As Boolean
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim mCobroFabricante As Type = GetType(wsHistoricoServicioTecnico)
        Dim pInfo As PropertyInfo

        For Each pInfo In mCobroFabricante.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As wsHistoricoServicioTecnico)
        Me.InnerList.Insert(posicion, valor)
        If Not _registrado Then _registrado = True
    End Sub

    Public Sub Add(ByVal valor As wsHistoricoServicioTecnico)
        Me.InnerList.Add(valor)
        If Not _registrado Then _registrado = True
    End Sub

    Public Sub AdicionarRango(ByVal rango As wsHistoricoServicioTecnicoColeccion)
        Me.InnerList.AddRange(rango)
        If Not _registrado Then _registrado = True
    End Sub

    Public Sub Remover(ByVal valor As wsHistoricoServicioTecnico)
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
            With CType(Me.InnerList(index), wsHistoricoServicioTecnico)
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
        Dim miDetalle As wsHistoricoServicioTecnico

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miDetalle = CType(Me.InnerList(index), wsHistoricoServicioTecnico)
            If miDetalle IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(wsHistoricoServicioTecnico).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(miDetalle, Nothing)
                    End If
                Next
                dtAux.Rows.Add(drAux)
            End If
        Next
        Return dtAux
    End Function

#End Region

#Region "Métodos Compartidos"

    Public Shared Sub SerializeObject(ByVal filename As String, ByVal col As wsHistoricoServicioTecnico, ByVal ExtraTypes() As System.Type)
        Dim s As New Xml.Serialization.XmlSerializer(GetType(wsHistoricoServicioTecnico), ExtraTypes)
        Dim writer As New StreamWriter(filename)

        s.Serialize(writer, col)
        writer.Close()
    End Sub

    Public Shared Sub SerializeObject(ByVal filename As String, ByVal col As wsHistoricoServicioTecnico)
        Dim s As New Xml.Serialization.XmlSerializer(GetType(wsHistoricoServicioTecnico))
        Dim writer As New StreamWriter(filename)

        s.Serialize(writer, col)
        writer.Close()
    End Sub

    Public Shared Function DeserializeObject(ByVal filename As String, ByVal ExtraTypes() As System.Type) As wsHistoricoServicioTecnico

        Dim fs As New IO.FileStream(filename, FileMode.Open)
        Dim w As New Xml.Serialization.XmlSerializer(GetType(wsHistoricoServicioTecnico), ExtraTypes)
        Dim g As wsHistoricoServicioTecnico = CType(w.Deserialize(fs), wsHistoricoServicioTecnico)

        fs.Close()

        Return g
    End Function

    Public Shared Function DeserializeObject(ByVal filename As String) As wsHistoricoServicioTecnico
        Dim fs As New IO.FileStream(filename, FileMode.Open)
        Dim w As New Xml.Serialization.XmlSerializer(GetType(wsHistoricoServicioTecnico))
        Dim g As wsHistoricoServicioTecnico = CType(w.Deserialize(fs), wsHistoricoServicioTecnico)

        fs.Close()

        Return g
    End Function

#End Region

End Class
