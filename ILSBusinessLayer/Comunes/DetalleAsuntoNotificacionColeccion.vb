Imports LMDataAccessLayer

Namespace Comunes

    Public Class DetalleAsuntoNotificacionColeccion
        Inherits CollectionBase

#Region "Propiedades"

        Public Property IdAsuntoNotificacion As Integer

        Private Property Cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idAsunto As Integer)
            Me.New()
            Me.IdAsuntoNotificacion = idAsunto
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Públicos"

        Default Public Property Item(ByVal index As Integer) As DetalleAsuntoNotificacion
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As DetalleAsuntoNotificacion)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As DetalleAsuntoNotificacion)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As DetalleAsuntoNotificacion)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As DetalleAsuntoNotificacion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub CargarDatos()
            Using dbManager As New LMDataAccess
                With dbManager
                    Try
                        .SqlParametros.Clear()
                        If _IdAsuntoNotificacion > 0 Then .SqlParametros.Add("@idAsuntoNotificacion", SqlDbType.Int).Value = _IdAsuntoNotificacion
                        .ejecutarReader("ObtenerDetalleAsuntoNotificacion", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            Dim objElemento As DetalleAsuntoNotificacion
                            While .Reader.Read
                                objElemento = New DetalleAsuntoNotificacion()
                                objElemento.CargarResultadoConsulta(.Reader)
                                Me.InnerList.Add(objElemento)
                            End While
                            Me.Cargado = True
                        End If
                    Catch ex As Exception
                        Throw ex
                    End Try
                End With
            End Using
        End Sub

#End Region

    End Class

End Namespace
