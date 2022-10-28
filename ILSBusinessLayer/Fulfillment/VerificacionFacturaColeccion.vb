Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports System.IO

Public Class VerificacionFacturaColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _factura As String
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal factura As String)
        Me.New()
        _factura = factura
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As VerificacionFactura
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As VerificacionFactura)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
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

    Public ReadOnly Property Registrado As Boolean
        Get
            Return _registrado
        End Get
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim mVerificacionFactura As Type = GetType(VerificacionFactura)
        Dim pInfo As PropertyInfo

        For Each pInfo In mVerificacionFactura.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As VerificacionFactura)
        Me.InnerList.Insert(posicion, valor)
        If Not _registrado Then _registrado = True
    End Sub

    Public Sub Add(ByVal valor As VerificacionFactura)
        Me.InnerList.Add(valor)
        If Not _registrado Then _registrado = True
    End Sub

    Public Sub AdicionarRango(ByVal rango As VerificacionFacturaColeccion)
        Me.InnerList.AddRange(rango)
        If Not _registrado Then _registrado = True
    End Sub

    Public Sub Remover(ByVal valor As VerificacionFactura)
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
            With CType(Me.InnerList(index), VerificacionFactura)
                If .Factura = identificador Then
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
        Dim miDetalle As VerificacionFactura

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miDetalle = CType(Me.InnerList(index), VerificacionFactura)
            If miDetalle IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(VerificacionFactura).GetProperties
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
                If _factura <> Nothing Then .SqlParametros.Add("@factura", SqlDbType.VarChar).Value = _factura
                .ejecutarReader("ObtenerInformacionVerificacionDeFacturas", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    Dim obj As VerificacionFactura
                    While .Reader.Read
                        obj = New VerificacionFactura
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

    Public Shared Sub SerializeObject(ByVal filename As String, ByVal col As VerificacionFactura, ByVal ExtraTypes() As System.Type)
        Dim s As New Xml.Serialization.XmlSerializer(GetType(VerificacionFactura), ExtraTypes)
        Dim writer As New StreamWriter(filename)

        s.Serialize(writer, col)
        writer.Close()
    End Sub

    Public Shared Sub SerializeObject(ByVal filename As String, ByVal col As VerificacionFactura)
        Dim s As New Xml.Serialization.XmlSerializer(GetType(VerificacionFactura))
        Dim writer As New StreamWriter(filename)

        s.Serialize(writer, col)
        writer.Close()
    End Sub

    Public Shared Function DeserializeObject(ByVal filename As String, ByVal ExtraTypes() As System.Type) As VerificacionFactura

        Dim fs As New IO.FileStream(filename, FileMode.Open)
        Dim w As New Xml.Serialization.XmlSerializer(GetType(VerificacionFactura), ExtraTypes)
        Dim g As VerificacionFactura = CType(w.Deserialize(fs), VerificacionFactura)

        fs.Close()

        Return g
    End Function

    Public Shared Function DeserializeObject(ByVal filename As String) As VerificacionFactura
        Dim fs As New IO.FileStream(filename, FileMode.Open)
        Dim w As New Xml.Serialization.XmlSerializer(GetType(VerificacionFactura))
        Dim g As VerificacionFactura = CType(w.Deserialize(fs), VerificacionFactura)

        fs.Close()

        Return g
    End Function

#End Region

End Class
