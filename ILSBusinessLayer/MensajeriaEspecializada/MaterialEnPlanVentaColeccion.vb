Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Namespace MensajeriaEspecializada

    Public Class MaterialEnPlanVentaColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idRegistro As Integer
        Private _idPlan As Integer
        Private _material As String

        Private _cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idRegistro As Integer)
            Me.New()
            _idRegistro = idRegistro
            CargarDatos()
        End Sub

        Public Sub New(ByVal material As String, ByVal idPlan As Integer)
            Me.New()
            _material = material
            _idPlan = idPlan
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As MaterialEnPlanVenta
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As MaterialEnPlanVenta)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property IdRegistro As Integer
            Get
                Return _idRegistro
            End Get
            Set(value As Integer)
                _idRegistro = value
            End Set
        End Property

        Public Property IdPlan As Integer
            Get
                Return _idPlan
            End Get
            Set(value As Integer)
                _idPlan = value
            End Set
        End Property

        Public Property Material As String
            Get
                Return _material
            End Get
            Set(value As String)
                _material = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim miMaterialEnPlanVenta As Type = GetType(MaterialEnPlanVenta)
            Dim pInfo As PropertyInfo

            For Each pInfo In miMaterialEnPlanVenta.GetProperties
                If pInfo.PropertyType.Namespace = "System" Then
                    With dtAux
                        .Columns.Add(pInfo.Name, pInfo.PropertyType)
                    End With
                ElseIf pInfo.PropertyType.Namespace = "ILSBusinessLayer.Enumerados" Then
                    With dtAux
                        .Columns.Add(pInfo.Name, GetType(Boolean))
                    End With
                End If
            Next
            Return dtAux
        End Function

#End Region

#Region "Métodos Públicos"

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As MaterialEnPlanVenta)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As MaterialEnPlanVenta)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As MaterialEnPlanVentaColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As MaterialEnPlanVenta)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function IndiceDe(ByVal material As String, idPlan As Integer) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), MaterialEnPlanVenta)
                    If .Material = material And .IdPlan = idPlan Then
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
            Dim miDetalle As MaterialEnPlanVenta

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miDetalle = CType(Me.InnerList(index), MaterialEnPlanVenta)
                If miDetalle IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(MaterialEnPlanVenta).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miDetalle, Nothing)
                        ElseIf pInfo.PropertyType.Namespace = "ILSBusinessLayer.Enumerados" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miDetalle, Nothing)
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
                Me.Clear()
                With dbManager
                    If Me._idRegistro > 0 Then .SqlParametros.Add("@idRegistro", SqlDbType.Int).Value = Me._idRegistro
                    If Me._idPlan > 0 Then .SqlParametros.Add("@idPlan", SqlDbType.Int).Value = Me._idPlan
                    If Not String.IsNullOrEmpty(Me._material) Then .SqlParametros.Add("@material", SqlDbType.VarChar).Value = Me._material

                    .ejecutarReader("ObtenerDetalleMaterialEnPlanVenta", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim elDetalle As MaterialEnPlanVenta

                        While .Reader.Read
                            elDetalle = New MaterialEnPlanVenta
                            elDetalle.CargarResultadoConsulta(.Reader)
                            Me.InnerList.Add(elDetalle)
                        End While
                        If Not .Reader.IsClosed Then .Reader.Close()
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

