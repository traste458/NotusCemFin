Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Namespace MensajeriaEspecializada

    Public Class MatrizTipoClientePlanRecaudoColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idTipoCliente As Integer
        Private _idPlan As Integer

        Private _cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idTipoCliente As Integer, ByVal idPlan As Integer)
            Me.New()
            _idTipoCliente = idTipoCliente
            _idPlan = idPlan
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As MatrizTipoClientePlanRecaudo
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As MatrizTipoClientePlanRecaudo)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
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

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim miMatrizTipoClientePlanRecaudo As Type = GetType(MatrizTipoClientePlanRecaudo)
            Dim pInfo As PropertyInfo

            For Each pInfo In miMatrizTipoClientePlanRecaudo.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As MatrizTipoClientePlanRecaudo)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As MatrizTipoClientePlanRecaudo)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As MatrizTipoClientePlanRecaudoColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As MatrizTipoClientePlanRecaudo)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function IndiceDe(ByVal idTipoCliente As Integer, idPlan As Integer) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), MatrizTipoClientePlanRecaudo)
                    If .IdTipoCliente = idTipoCliente And .IdPlan = idPlan Then
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
            Dim miDetalle As MatrizTipoClientePlanRecaudo

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miDetalle = CType(Me.InnerList(index), MatrizTipoClientePlanRecaudo)
                If miDetalle IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(MatrizTipoClientePlanRecaudo).GetProperties
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
                    If _idTipoCliente > 0 Then .SqlParametros.Add("@idTipoCliente", SqlDbType.Int).Value = _idTipoCliente
                    If _idPlan > 0 Then .SqlParametros.Add("@idPlan", SqlDbType.Int).Value = _idPlan

                    .ejecutarReader("ObtenerDetalleMatrizTipoClientePlanRecaudo", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim elDetalle As MatrizTipoClientePlanRecaudo

                        While .Reader.Read
                            elDetalle = New MatrizTipoClientePlanRecaudo
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

