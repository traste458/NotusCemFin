Imports LMDataAccessLayer

Namespace MensajeriaEspecializada

    Public Class PersonalEnGerenciaColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idGerencia As Integer
        Private _idPersonaPadre As Integer
        Private _tipoPersona As Enumerados.TipoPersonaSiembra

        Private _cargado As Boolean

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As PersonalEnGerencia
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As PersonalEnGerencia)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property IdGerencia As Integer
            Get
                Return _idGerencia
            End Get
            Set(value As Integer)
                _idGerencia = value
            End Set
        End Property

        Public Property IdPersonaPadre As Integer
            Get
                Return _idPersonaPadre
            End Get
            Set(value As Integer)
                _idPersonaPadre = value
            End Set
        End Property

        Public Property TipoPersona As Enumerados.TipoPersonaSiembra
            Get
                Return _tipoPersona
            End Get
            Set(value As Enumerados.TipoPersonaSiembra)
                _tipoPersona = value
            End Set
        End Property
        
#End Region

#Region "Métodos Públicos"

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As PersonalEnGerencia)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As PersonalEnGerencia)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As PersonalEnGerenciaColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As PersonalEnGerencia)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function IndiceDe(ByVal idPersonaGerencia As Integer) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), PersonalEnGerencia)
                    If .IdPersonaGerencia = idPersonaGerencia Then
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
                    If Me._idGerencia > 0 Then .SqlParametros.Add("@idGerencia", SqlDbType.Int).Value = _idGerencia
                    If Me._idPersonaPadre > 0 Then .SqlParametros.Add("@idPersonaPadre", SqlDbType.Int).Value = _idPersonaPadre
                    If Me._tipoPersona > 0 Then .SqlParametros.Add("@tipoPersona", SqlDbType.Int).Value = _tipoPersona

                    .ejecutarReader("ObtienePersonalEnGerencia", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        Dim elDetalle As PersonalEnGerencia

                        While .Reader.Read
                            elDetalle = New PersonalEnGerencia
                            elDetalle.CargarResultadoConsulta(.Reader)
                            _cargado = True
                            Me.InnerList.Add(elDetalle)
                        End While
                        .Reader.Close()
                    End If
                End With
                _cargado = True
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

#End Region

    End Class

End Namespace
