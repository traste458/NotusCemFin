Imports LMDataAccessLayer

Namespace Comunes

    Public Class UsuarioNotificacionColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idAsuntoNotificacion As Integer

        Private Property Cargado As Boolean

#End Region

#Region "Propiedades"

        Public Property IdAsuntoNotificacion As Integer
            Get
                Return _idAsuntoNotificacion
            End Get
            Set(value As Integer)
                _idAsuntoNotificacion = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idAsuntoNotificacion As Integer)
            Me.New()
            Me._idAsuntoNotificacion = idAsuntoNotificacion
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Públicos"

        Default Public Property Item(ByVal index As Integer) As UsuarioNotificacion
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As UsuarioNotificacion)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As UsuarioNotificacion)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As UsuarioNotificacion)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As UsuarioNotificacion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub CargarDatos()
            Using dbManager As New LMDataAccess
                With dbManager
                    Try
                        .SqlParametros.Clear()
                        If _idAsuntoNotificacion > 0 Then .SqlParametros.Add("@idAsuntoNotificacion", SqlDbType.Int).Value = _idAsuntoNotificacion
                        .ejecutarReader("ObtenerInfoUsuarioNotificacion", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            Dim objElemento As UsuarioNotificacion
                            While .Reader.Read
                                objElemento = New UsuarioNotificacion()
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
