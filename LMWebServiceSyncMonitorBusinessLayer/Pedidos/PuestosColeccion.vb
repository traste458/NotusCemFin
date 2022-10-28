Imports System.Collections
Imports System.Reflection

Namespace Puestos

    Public Class PuestosColeccion
        Inherits CollectionBase

#Region "atributos"

        Private _listaCentros As ArrayList

#End Region

#Region "Propiedades"

        Public Property Item(ByVal index As Integer) As SAPPoolPedidos.ZmmIntVstel
            Get
                Return Me.List.Item(index)
            End Get
            Set(ByVal value As SAPPoolPedidos.ZmmIntVstel)
                Me.List.Item(index) = value
            End Set
        End Property

        Public Property ListaCentros() As ArrayList
            Get
                If _listaCentros Is Nothing Then _listaCentros = New ArrayList
                Return _listaCentros
            End Get
            Set(ByVal value As ArrayList)
                _listaCentros = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            CargarDatos()
        End Sub

        Public Sub New(ByVal centros As ArrayList)
            MyBase.New()
            _listaCentros = centros
            CargarDatos()
        End Sub
#End Region

#Region "Metodos Publicos"

        Public Sub CargarDatos()
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Try
                With dbManager
                    If _listaCentros IsNot Nothing AndAlso _listaCentros.Count > 0 Then _
                        .SqlParametros.Add("@listaCentros", SqlDbType.VarChar, 100).Value = Join(_listaCentros.ToArray, ",")
                    .ejecutarReader("ObtenerRegiones", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim puesto As SAPPoolPedidos.ZmmIntVstel
                        While .Reader.Read
                            puesto = New SAPPoolPedidos.ZmmIntVstel
                            puesto.vstel = .Reader("centro").ToString
                            Me.InnerList.Add(puesto)
                        End While
                        .Reader.Close()
                    End If
                End With
            Catch ex As Exception
                Throw New Exception("error al obtener lista de puestos(centros-region) " & ex.Message)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Public Sub Add(ByVal Valor As SAPPoolPedidos.ZmmIntVstel)
            Me.InnerList.Add(Valor)
        End Sub

        Public Sub Remove(ByVal index As Integer)
            If index <= Count - 1 OrElse index >= 0 Then
                Me.InnerList.RemoveAt(index)
            End If
        End Sub

        Public Function GenerarDataTable() As DataTable
            Dim dtAux As DataTable = CrearEstructuraDeTabla()
            Dim drAux As DataRow
            Dim miColeccion As PuestosColeccion

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miColeccion = CType(Me.InnerList(index), PuestosColeccion)
                If miColeccion IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(PuestosColeccion).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miColeccion, Nothing)
                        End If
                    Next
                    dtAux.Rows.Add(drAux)
                End If
            Next
            Return dtAux

        End Function

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim miColeccion As Type = GetType(PuestosColeccion)
            Dim pInfo As PropertyInfo

            For Each pInfo In miColeccion.GetProperties
                If pInfo.PropertyType.Namespace = "System" Then
                    With dtAux
                        .Columns.Add(pInfo.Name, pInfo.PropertyType)
                    End With
                End If
            Next
            Return dtAux
        End Function
#End Region

    End Class

    Public Class Region

#Region "Variables"

        Private _idRegion As Integer
        Private _centro As String
        Private _almacen As String
        Private _error As String

#End Region

#Region "Propiedades"
        Public Property Almacen() As String
            Get
                Return _almacen
            End Get
            Set(ByVal value As String)
                _almacen = value
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

        Public ReadOnly Property IdRegion() As Integer
            Get
                Return _idRegion
            End Get

        End Property

        Public ReadOnly Property InfoError() As String
            Get
                Return _error
            End Get
        End Property


#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idRegion As Integer)
            CargarDatos(idRegion)
        End Sub

        Public Sub New(ByVal puesto As String)
            CargarDatos(puesto)
        End Sub
#End Region

#Region "Metodos Privados"

        Private Overloads Sub CargarDatos(ByVal idRegion As Integer)
            Dim dm As New LMDataAccessLayer.LMDataAccess
            Try
                dm.SqlParametros.Add("@idRegion", SqlDbType.Int).Value = idRegion
                dm.ejecutarReader("ObtenerRegiones", CommandType.StoredProcedure)
                If dm.Reader.Read Then
                    _idRegion = idRegion
                    _centro = dm.Reader("centro").ToString()
                    _almacen = dm.Reader("almacen").ToString()
                End If
                Me._error = String.Empty
            Catch ex As Exception
                _error = "Error al cargar los datos. " & ex.Message
            Finally
                dm.Dispose()
            End Try
        End Sub

        Private Overloads Sub CargarDatos(ByVal centro As String)
            Dim dm As New LMDataAccessLayer.LMDataAccess
            Try
                dm.SqlParametros.Add("@centro", SqlDbType.VarChar, 10).Value = centro
                dm.ejecutarReader("ObtenerRegiones", CommandType.StoredProcedure)
                If dm.Reader.Read Then
                    _idRegion = IdRegion
                    _centro = dm.Reader("centro").ToString()
                    _almacen = dm.Reader("almacen").ToString()
                End If
                Me._error = String.Empty
            Catch ex As Exception
                _error = "Error al cargar los datos. " & ex.Message
            Finally
                If dm.Reader IsNot Nothing Then dm.Reader.Close()
                dm.Dispose()
            End Try
        End Sub

#End Region

#Region "Metodos"
        Public Shared Function ObtenerTodas() As DataTable
            Dim dm As New LMDataAccessLayer.LMDataAccess
            Return dm.ejecutarDataTable("ObtenerRegiones")
        End Function

        Public Shared Function ObtenerTodas(ByVal filtro As FiltroRegion) As DataTable
            Dim dm As New LMDataAccessLayer.LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .idRegion > 0 Then dm.SqlParametros.Add("@idRegion", SqlDbType.Int).Value = .idRegion
                If .centro <> String.Empty Then dm.SqlParametros.Add("@centro", SqlDbType.VarChar).Value = .centro
                If .almacen <> String.Empty Then dm.SqlParametros.Add("@almacen", SqlDbType.VarChar).Value = .almacen
            End With
            dtDatos = dm.ejecutarDataTable("ObtenerRegiones", CommandType.StoredProcedure)

            Return dtDatos
        End Function


#End Region

        Public Structure FiltroRegion
            Dim idRegion As Integer
            Dim centro As String
            Dim almacen As String
        End Structure
    End Class

End Namespace