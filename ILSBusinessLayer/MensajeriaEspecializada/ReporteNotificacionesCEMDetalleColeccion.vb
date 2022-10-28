Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.IO
Imports System.Reflection

Namespace MensajeriaEspecializada

    Public Class ReporteNotificacionesCEMDetalleColeccion
        Inherits CollectionBase

#Region "Filtros de Búsqueda"

        Private _numeroRadicado As ArrayList
        Private _cargado As Boolean

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As ReporteNotificacionesCEMDetalle
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(value As ReporteNotificacionesCEMDetalle)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property NumeroRadicado As ArrayList
            Get
                If _numeroRadicado Is Nothing Then _numeroRadicado = New ArrayList
                Return _numeroRadicado
            End Get
            Set(value As ArrayList)
                _numeroRadicado = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim objUsuario As Type = GetType(ReporteNotificacionesCEMDetalle)
            Dim pInfo As PropertyInfo

            For Each pInfo In objUsuario.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As ReporteNotificacionesCEMDetalle)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As ReporteNotificacionesCEMDetalle)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As ReporteNotificacionesCEMDetalle)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Function GenerarDataTable() As DataTable
            If Not _cargado Then CargarDatos()
            Dim dtAux As DataTable = CrearEstructuraDeTabla()
            Dim drAux As DataRow
            Dim miRegistro As ReporteNotificacionesCEMDetalle

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miRegistro = CType(Me.InnerList(index), ReporteNotificacionesCEMDetalle)
                If miRegistro IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(ReporteNotificacionesCEMDetalle).GetProperties
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

            If _cargado Then Me.InnerList.Clear()
            With dbManager
                If _numeroRadicado IsNot Nothing AndAlso _numeroRadicado.Count > 0 Then _
                            .SqlParametros.Add("@numeroRadicado", SqlDbType.VarChar).Value = Join(_numeroRadicado.ToArray(), ",")
                
                .ejecutarReader("ReporteNotificacionesCEMDetalle", CommandType.StoredProcedure)
                If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                    Dim objUsuarioNotificacion As ReporteNotificacionesCEMDetalle
                    While .Reader.Read
                        objUsuarioNotificacion = New ReporteNotificacionesCEMDetalle()
                        objUsuarioNotificacion.CargarResultadoConsulta(.Reader)
                        Me.InnerList.Add(objUsuarioNotificacion)
                    End While
                    _cargado = True
                End If
            End With
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Sub

#End Region

    End Class

End Namespace