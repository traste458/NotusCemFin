Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Namespace Inventario

    Public Class DetalleSerialBloqueoColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idBloqueo As List(Of Integer)
        Private _serial As List(Of String)

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

        Public Property Serial() As List(Of String)
            Get
                Return _serial
            End Get
            Set(ByVal value As List(Of String))
                _serial = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim objDetalleSerialBloqueo As Type = GetType(DetalleSerialBloqueo)
            Dim pInfo As PropertyInfo

            For Each pInfo In objDetalleSerialBloqueo.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As DetalleSerialBloqueo)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As DetalleSerialBloqueo)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As DetalleSerialBloqueoColeccion)
            For Each item As DetalleSerialBloqueo In rango
                item.Accion = Enumerados.AccionItem.Adicionar
            Next

            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As DetalleSerialBloqueo)
            With Me.InnerList
                For Each item As DetalleSerialBloqueo In Me.InnerList
                    If item.Serial = valor.Serial Then
                        item.Accion = Enumerados.AccionItem.Eliminar
                        Exit For
                    End If
                Next
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            If index > -1 Then
                CType(Me.InnerList(index), DetalleSerialBloqueo).Accion = Enumerados.AccionItem.Eliminar
            End If
        End Sub

        Public Function IndiceDe(ByVal serial As String) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), DetalleSerialBloqueo)
                    If .Serial = serial Then
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
            Dim miRegistro As DetalleSerialBloqueo

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miRegistro = CType(Me.InnerList(index), DetalleSerialBloqueo)
                If miRegistro IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(DetalleSerialBloqueo).GetProperties
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
            'Using dbManager As New LMDataAccess
            Dim dbManager As New LMDataAccess
            Try
                'Filtros de la colección.
                Me.Clear()
                With dbManager
                    If Not _idBloqueo Is Nothing AndAlso _idBloqueo.Count > 0 Then _
                        .SqlParametros.Add("@listaIdBloqueo", SqlDbType.VarChar).Value = String.Join(",", _idBloqueo.Cast(Of String)().ToArray)

                    If Not _serial Is Nothing AndAlso _serial.Count > 0 Then _
                        .SqlParametros.Add("@listaSerial", SqlDbType.VarChar).Value = String.Join(",", _serial.ToArray)

                    .ejecutarReader("ConsultaBloqueoInventarioDetalleSerial", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        Dim objItem As DetalleSerialBloqueo
                        While .Reader.Read
                            objItem = New DetalleSerialBloqueo()

                            objItem.IdBloqueoDetalleSerial = CInt(.Reader("idBloqueoDetalleSerial"))
                            objItem.IdBloqueoDetalleProducto = CInt(.Reader("ddBloqueoDetalleProducto"))
                            objItem.IdBloqueo = CInt(.Reader("idBloqueo"))
                            objItem.Serial = .Reader("serial").ToString()

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
            'End Using

        End Sub

#End Region

    End Class

End Namespace

