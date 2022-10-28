Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports ILSBusinessLayer.Enumerados

Namespace InventarioFisico

    Public Class InfoInventarioSerialColeccion
        Inherits CollectionBase


#Region "Atributos (Filtros de Búsqueda)"

        Private _listaSerial As List(Of String)
        Private _cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _listaSerial = New List(Of String)
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As InfoInventarioSerial
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As InfoInventarioSerial)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property ListaSeriales As List(Of String)
            Get
                If _listaSerial Is Nothing Then _listaSerial = New List(Of String)
                Return _listaSerial
            End Get
            Set(value As List(Of String))
                _listaSerial = value
            End Set
        End Property
        
#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim obj As Type = GetType(InfoInventarioSerial)
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As InfoInventarioSerial)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As InfoInventarioSerial)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As DetalleMsisdnEnServicioMensajeriaColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As InfoInventarioSerial)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function IndiceDe(ByVal serial As String) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), InfoInventarioSerial)
                    If .Serial = serial Then
                        indice = index
                        Exit For
                    End If
                End With
            Next
            Return indice
        End Function

        Public Function GenerarDataTable() As DataTable
            If Not _cargado Then CargarDatos()
            Dim dtAux As DataTable = CrearEstructuraDeTabla()
            Dim drAux As DataRow
            Dim obj As InfoInventarioSerial

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                obj = CType(Me.InnerList(index), InfoInventarioSerial)
                If obj IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(InfoInventarioSerial).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(obj, Nothing)
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
                    If _listaSerial IsNot Nothing AndAlso _listaSerial.Count > 0 Then
                        Dim dt As New DataTable()
                        dt.Columns.Add("serial", GetType(String))
                        For Each s As String In _listaSerial
                            dt.Rows.Add(s)
                        Next
                        .SqlParametros.Add("@listaSerial", SqlDbType.Structured).Value = dt
                    End If

                    .ejecutarReader("ConsultarInfoInventarioFisicoSerial", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        Dim obj As InfoInventarioSerial
                        While .Reader.Read
                            obj = New InfoInventarioSerial
                            obj.CargarResultadoConsulta(.Reader)
                            Me.InnerList.Add(obj)
                        End While
                        .Reader.Close()
                    End If
                End With
                _cargado = True
            End Using
        End Sub
#End Region

    End Class
End Namespace