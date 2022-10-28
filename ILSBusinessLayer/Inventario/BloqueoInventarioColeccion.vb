Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Namespace Inventario

    Public Class BloqueoInventarioColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de búsqueda)"

        Private _idBloqueo As Integer
        Private _idUsuario As Integer
        Private _idEstado As Integer

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdBloqueo() As Integer
            Get
                Return _idBloqueo
            End Get
            Set(ByVal value As Integer)
                _idBloqueo = value
            End Set
        End Property

        Public Property IdUsuario() As Integer
            Get
                Return _idUsuario
            End Get
            Set(ByVal value As Integer)
                _idUsuario = value
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

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim objBloqueoInventario As Type = GetType(BloqueoInventario)
            Dim pInfo As PropertyInfo

            For Each pInfo In objBloqueoInventario.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As BloqueoInventario)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As BloqueoInventario)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As BloqueoInventarioColeccion)
            For Each item As BloqueoInventario In rango
                item.Accion = Enumerados.AccionItem.Adicionar
            Next

            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As BloqueoInventario)
            With Me.InnerList
                For Each item As BloqueoInventario In Me.InnerList
                    If item.IdBloqueo = valor.IdBloqueo Then
                        item.Accion = Enumerados.AccionItem.Eliminar
                        Exit For
                    End If
                Next
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            If index > -1 Then
                CType(Me.InnerList(index), BloqueoInventario).Accion = Enumerados.AccionItem.Eliminar
            End If
        End Sub

        Public Function IndiceDe(ByVal idBloqueo As Integer) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), BloqueoInventario)
                    If .IdBloqueo = idBloqueo Then
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
            Dim miRegistro As BloqueoInventario

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miRegistro = CType(Me.InnerList(index), BloqueoInventario)
                If miRegistro IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(BloqueoInventario).GetProperties
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
                    If _idBloqueo > 0 Then .SqlParametros.Add("@idBloqueo", SqlDbType.Int).Value = _idBloqueo
                    If _idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    If _idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = _idEstado

                    .ejecutarReader("ObtieneBloqueoInventario", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        Dim objItem As BloqueoInventario
                        While .Reader.Read
                            objItem = New BloqueoInventario()

                            objItem.IdBloqueo = CInt(.Reader("idBloqueo"))
                            objItem.IdBodega = CInt(.Reader("idBodega").ToString())
                            objItem.FechaRegistro = CDate(.Reader("fechaRegistro"))
                            objItem.IdUsuario = CInt(.Reader("idUsuario").ToString())
                            objItem.IdEstado = CInt(.Reader("idEstado").ToString())
                            objItem.FechaInicio = CDate(.Reader("fechaInicio"))
                            If Not IsDBNull(.Reader("fechaFin")) Then objItem.FechaFin = CDate(.Reader("fechaFin"))
                            objItem.IdUnidadNegocio = CShort(.Reader("idUnidadNegocio").ToString())
                            If Not IsDBNull(.Reader("idDestinatario")) Then objItem.IdDestinatario = CInt(.Reader("idDestinatario"))
                            objItem.IdTipoBloqueo = CShort(.Reader("idTipoBloqueo").ToString())
                            objItem.Observacion=.Reader("observacion").ToString()

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
