Imports LMDataAccessLayer

Namespace MensajeriaEspecializada

    Public Class DetalleDespachoServicioMensajeriaColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros)"

        Public Property IdDetalle As Long
        Public Property IdRuta As Long

        Private Property Cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idRuta As Long)
            Me.New()
            Me.IdRuta = idRuta
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Públicos"

        Default Public Property Item(ByVal index As Integer) As DetalleDespachoServicioMensajeria
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As DetalleDespachoServicioMensajeria)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As DetalleDespachoServicioMensajeria)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As DetalleDespachoServicioMensajeria)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As DetalleDespachoServicioMensajeriaColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As DetalleDespachoServicioMensajeria)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function IndiceDe(ByVal idDetalle As Long) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), DetalleDespachoServicioMensajeria)
                    If .IdDetalle = idDetalle Then
                        indice = index
                        Exit For
                    End If
                End With
            Next
            Return indice
        End Function

        Public Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                Me.Clear()
                With dbManager
                    If Me.IdDetalle > 0 Then .SqlParametros.Add("@idDetalle", SqlDbType.BigInt).Value = Me.IdDetalle
                    If Me.IdRuta > 0 Then .SqlParametros.Add("@idRuta", SqlDbType.BigInt).Value = Me.IdRuta
                    
                    .ejecutarReader("ObtenerInfoDetalleDespachoServicio", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim elDetalle As DetalleDespachoServicioMensajeria

                        While .Reader.Read
                            elDetalle = New DetalleDespachoServicioMensajeria
                            elDetalle.CargarResultadoConsulta(.Reader)
                            Me.InnerList.Add(elDetalle)
                        End While
                        If Not .Reader.IsClosed Then .Reader.Close()
                    End If
                End With
                Me.Cargado = True
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

#End Region

    End Class

End Namespace
