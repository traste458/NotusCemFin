Namespace OMS
    Public Class PreinstruccionClienteColeccion
        Inherits CollectionBase
#Region "Variables"

        Private _idPreinstruccion As Integer
        Private _idDetalleOrdenCompra As Integer
        Private _detalleOrdenCompra As Recibos.DetalleOrdenCompra
        Private _prioridad As Short
        Private _idEstado As Integer
        Private _cantidadInstruccionada As Integer
        Private _idUsuario As Integer
        Private _fechaRegistro As DateTime
        
#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As PreinstruccionCliente
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As PreinstruccionCliente)
                If value IsNot Nothing Then
                    Me.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public ReadOnly Property IdPreinstruccion() As Integer
            Get
                Return _idPreinstruccion
            End Get
        End Property

        Public Property IdDetalleOrdenCompra() As Integer
            Get
                Return _idDetalleOrdenCompra
            End Get
            Set(ByVal value As Integer)
                _idDetalleOrdenCompra = value
            End Set
        End Property

        Public ReadOnly Property DetalleOrdenCompra() As Recibos.DetalleOrdenCompra
            Get
                If Not _detalleOrdenCompra Is Nothing Then
                    Return _detalleOrdenCompra
                Else
                    If _idDetalleOrdenCompra > 0 Then
                        _detalleOrdenCompra = New Recibos.DetalleOrdenCompra(_idDetalleOrdenCompra)
                        Return _detalleOrdenCompra
                    Else
                        Return New Recibos.DetalleOrdenCompra()
                    End If
                End If
            End Get
        End Property

        Public Property Prioridad() As Short
            Get
                Return _prioridad
            End Get
            Set(ByVal value As Short)
                _prioridad = value
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

        Public Property CantidadInstruccionada() As Integer
            Get
                Return _cantidadInstruccionada
            End Get
            Set(ByVal value As Integer)
                _cantidadInstruccionada = value
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

        Public ReadOnly Property FechaRegistro() As DateTime
            Get
                Return _fechaRegistro
            End Get
        End Property

        Public ReadOnly Property TotalInstruccionado() As Integer
            Get
                Dim cantAux As Integer
                For Each preIns As PreinstruccionCliente In Me
                    cantAux += preIns.CantidadInstruccionada
                Next
                Return cantAux
            End Get
        End Property

#End Region

#Region "Métodos"
        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As PreinstruccionCliente)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As PreinstruccionCliente)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub Remover(ByVal valor As PreinstruccionCliente)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub Procesar(ByVal db As LMDataAccessLayer.LMDataAccess, ByVal dtErrores As DataTable)
            For Each preinstruccion As PreinstruccionCliente In Me
                With preinstruccion
                    If .IdPreinstruccion = 0 Then
                        .Crear(dtErrores, db)
                    Else
                        If .CambioInstruccion Then _
                            .Actualizar(dtErrores, db)
                    End If
                End With
            Next
        End Sub

#End Region

    End Class
End Namespace

