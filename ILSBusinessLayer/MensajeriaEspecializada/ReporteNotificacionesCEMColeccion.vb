Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.IO
Imports System.Reflection

Namespace MensajeriaEspecializada

    Public Class ReporteNotificacionesCEMColeccion
        Inherits CollectionBase

#Region "Filtros de Búsqueda"

        Private _numeroRadicado As ArrayList
        Private _ciudad As ArrayList
        Private _bodega As ArrayList
        Private _tipoNotificacion As ArrayList
        Private _estado As ArrayList
        Private _fechaInicio As Date
        Private _fechaFin As Date
        Private _cargado As Boolean

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As ReporteNotificacionesCEM
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(value As ReporteNotificacionesCEM)
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

        Public Property Ciudad As ArrayList
            Get
                If _ciudad Is Nothing Then _ciudad = New ArrayList
                Return _ciudad
            End Get
            Set(value As ArrayList)
                _ciudad = value
            End Set
        End Property

        Public Property Bodega As ArrayList
            Get
                If _bodega Is Nothing Then _bodega = New ArrayList
                Return _bodega
            End Get
            Set(value As ArrayList)
                _bodega = value
            End Set
        End Property

        Public Property TipoNotificacion As ArrayList
            Get
                If _tipoNotificacion Is Nothing Then _tipoNotificacion = New ArrayList
                Return _tipoNotificacion
            End Get
            Set(value As ArrayList)
                _tipoNotificacion = value
            End Set
        End Property

        Public Property Estado As ArrayList
            Get
                If _estado Is Nothing Then _estado = New ArrayList
                Return _estado
            End Get
            Set(value As ArrayList)
                _estado = value
            End Set
        End Property

        Public Property FechaInicio As Date
            Get
                Return _fechaInicio
            End Get
            Set(value As Date)
                _fechaInicio = value
            End Set
        End Property

        Public Property FechaFin As Date
            Get
                Return _fechaFin
            End Get
            Set(value As Date)
                _fechaFin = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim objUsuario As Type = GetType(ReporteNotificacionesCEM)
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As ReporteNotificacionesCEM)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As ReporteNotificacionesCEM)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As ReporteNotificacionesCEM)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Function GenerarDataTable() As DataTable
            If Not _cargado Then CargarDatos()
            Dim dtAux As DataTable = CrearEstructuraDeTabla()
            Dim drAux As DataRow
            Dim miRegistro As ReporteNotificacionesCEM

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miRegistro = CType(Me.InnerList(index), ReporteNotificacionesCEM)
                If miRegistro IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(ReporteNotificacionesCEM).GetProperties
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
                If _ciudad IsNot Nothing AndAlso _ciudad.Count > 0 Then _
                            .SqlParametros.Add("@ciudad", SqlDbType.VarChar).Value = Join(_ciudad.ToArray(), ",")
                If _bodega IsNot Nothing AndAlso _bodega.Count > 0 Then _
                            .SqlParametros.Add("@bodega", SqlDbType.VarChar).Value = Join(_bodega.ToArray(), ",")
                If _estado IsNot Nothing AndAlso _estado.Count > 0 Then _
                    .SqlParametros.Add("@estado", SqlDbType.VarChar).Value = Join(_estado.ToArray(), ",")
                If _fechaInicio > Date.MinValue Then .SqlParametros.Add("@fechaInicio", SqlDbType.DateTime).Value = _fechaInicio
                If _fechaFin > Date.MinValue Then .SqlParametros.Add("@fechaFinal", SqlDbType.DateTime).Value = _fechaFin

                .ejecutarReader("ReporteNotificacionesCEM", CommandType.StoredProcedure)
                If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                    Dim objUsuarioNotificacion As ReporteNotificacionesCEM
                    While .Reader.Read
                        objUsuarioNotificacion = New ReporteNotificacionesCEM()
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