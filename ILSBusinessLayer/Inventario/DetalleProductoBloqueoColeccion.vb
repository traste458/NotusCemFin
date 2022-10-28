Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Namespace Inventario

    Public Class DetalleProductoBloqueoColeccion
        Inherits CollectionBase

#Region "Atributos"

        Private _idBloqueo As List(Of Integer)
        Private _idProducto As List(Of Integer)

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdBloqueo() As List(Of Integer)
            Get
                Return _idBloqueo
            End Get
            Set(ByVal value As List(Of Integer))
                _idBloqueo = value
            End Set
        End Property

        Public Property IdProducto() As List(Of Integer)
            Get
                Return _idProducto
            End Get
            Set(ByVal value As List(Of Integer))
                _idProducto = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim objDetalleProductoBloqueo As Type = GetType(DetalleProductoBloqueo)
            Dim pInfo As PropertyInfo

            For Each pInfo In objDetalleProductoBloqueo.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As DetalleProductoBloqueo)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As DetalleProductoBloqueo)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As DetalleProductoBloqueoColeccion)
            For Each item As DetalleProductoBloqueo In rango
                item.Accion = Enumerados.AccionItem.Adicionar
            Next

            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As DetalleProductoBloqueo)
            With Me.InnerList
                For Each item As DetalleProductoBloqueo In Me.InnerList
                    If item.IdBloqueoDetalleProducto = valor.IdBloqueoDetalleProducto Then
                        item.Accion = Enumerados.AccionItem.Eliminar
                        Exit For
                    End If
                Next
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            If index > -1 Then
                CType(Me.InnerList(index), DetalleProductoBloqueo).Accion = Enumerados.AccionItem.Eliminar
            End If
        End Sub

        Public Function IndiceDe(ByVal idBloqueoDetalleProducto As Integer) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), DetalleProductoBloqueo)
                    If .IdBloqueoDetalleProducto = idBloqueoDetalleProducto Then
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
            Dim miRegistro As DetalleProductoBloqueo

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miRegistro = CType(Me.InnerList(index), DetalleProductoBloqueo)
                If miRegistro IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(DetalleProductoBloqueo).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miRegistro, Nothing)
                        End If
                    Next
                    dtAux.Rows.Add(drAux)
                End If
            Next

            Return dtAux
        End Function

        Public Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                'Filtros de la colección.
                Me.Clear()
                With dbManager
                    If Not _idBloqueo Is Nothing AndAlso _idBloqueo.Count > 0 Then _
                        .SqlParametros.Add("@listaIdBloqueo", SqlDbType.VarChar).Value = String.Join(",", _idBloqueo.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())

                    If Not _idProducto Is Nothing AndAlso _idProducto.Count > 0 Then _
                        .SqlParametros.Add("@listaIdProducto", SqlDbType.VarChar).Value = String.Join(",", _idProducto.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())

                    .ejecutarReader("ConsultaBloqueoInventarioDetalleProducto", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        Dim objItem As DetalleProductoBloqueo
                        While .Reader.Read
                            objItem = New DetalleProductoBloqueo()

                            objItem.IdBloqueoDetalleProducto = CInt(.Reader("idBloqueoDetalleProducto"))
                            objItem.IdBloqueo = CInt(.Reader("idBloqueo"))
                            objItem.IdProducto = .Reader("idProducto").ToString()
                            objItem.Material = .Reader("material").ToString()
                            objItem.Cantidad = CInt(.Reader("cantidad"))
                            objItem.Subproducto = .Reader("subproducto").ToString()

                            objItem.Registrado = True
                            objItem.Accion = Enumerados.AccionItem.Ninguna

                            Me.Adicionar(objItem)
                        End While
                    End If
                End With

            Catch ex As Exception
                Throw New Exception("Se generó error en [CargarDatos]", ex)
            End Try
            dbManager.Dispose()
        End Sub

#End Region

    End Class

End Namespace


