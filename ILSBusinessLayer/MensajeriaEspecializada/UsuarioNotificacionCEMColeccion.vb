Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Namespace MensajeriaEspecializada

    Public Class UsuarioNotificacionCEMColeccion
        Inherits CollectionBase

#Region "Filtros de Búsqueda"

        Private _idUsuarioNotificacion As ArrayList
        Private _idAsuntoNotificacion As ArrayList
        Private _idBodega As ArrayList
        Private _email As String
        Private _destinatarioNotificacion As String
        Private _estado As ArrayList
        Private _fechaInicio As Date
        Private _fechaFin As Date
        Private _cargado As Boolean

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As UsuarioNotificacionCEM
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(value As UsuarioNotificacionCEM)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property IdUsuarioNotificacion As ArrayList
            Get
                If _idUsuarioNotificacion Is Nothing Then _idUsuarioNotificacion = New ArrayList
                Return _idUsuarioNotificacion
            End Get
            Set(value As ArrayList)
                _idUsuarioNotificacion = value
            End Set
        End Property

        Public Property Email As String
            Get
                Return _email
            End Get
            Set(value As String)
                _email = value
            End Set
        End Property

        Public Property IdAsuntoNotificacion As ArrayList
            Get
                If _idAsuntoNotificacion Is Nothing Then _idAsuntoNotificacion = New ArrayList
                Return _idAsuntoNotificacion
            End Get
            Set(value As ArrayList)
                _idAsuntoNotificacion = value
            End Set
        End Property

        Public Property IdBodega As ArrayList
            Get
                If _idBodega Is Nothing Then _idBodega = New ArrayList
                Return _idBodega
            End Get
            Set(value As ArrayList)
                _idBodega = value
            End Set
        End Property

        Public Property DestinatarioNotificacion As String
            Get
                Return _destinatarioNotificacion
            End Get
            Set(value As String)
                _destinatarioNotificacion = value
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
            Dim objUsuarioNotificacion As Type = GetType(UsuarioNotificacionCEM)
            Dim pInfo As PropertyInfo

            For Each pInfo In objUsuarioNotificacion.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As UsuarioNotificacionCEM)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As UsuarioNotificacionCEM)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As UsuarioNotificacionCEM)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Function GenerarDataTable() As DataTable
            If Not _cargado Then CargarDatos()
            Dim dtAux As DataTable = CrearEstructuraDeTabla()
            Dim drAux As DataRow
            Dim miRegistro As UsuarioNotificacionCEM

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miRegistro = CType(Me.InnerList(index), UsuarioNotificacionCEM)
                If miRegistro IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(UsuarioNotificacionCEM).GetProperties
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
                If _idUsuarioNotificacion IsNot Nothing AndAlso _idUsuarioNotificacion.Count > 0 Then _
                            .SqlParametros.Add("@idUsuarioNotificacion", SqlDbType.VarChar).Value = Join(_idUsuarioNotificacion.ToArray(), ",")
                If _idAsuntoNotificacion IsNot Nothing AndAlso _idAsuntoNotificacion.Count > 0 Then _
                            .SqlParametros.Add("@idAsuntoNotificacion", SqlDbType.VarChar).Value = Join(_idAsuntoNotificacion.ToArray(), ",")
                If _idBodega IsNot Nothing AndAlso _idBodega.Count > 0 Then _
                            .SqlParametros.Add("@idBodega", SqlDbType.VarChar).Value = Join(_idBodega.ToArray(), ",")
                If Not String.IsNullOrEmpty(_destinatarioNotificacion) Then _
                    .SqlParametros.Add("@destinatarioNotificacion", SqlDbType.VarChar, 500).Value = _destinatarioNotificacion
                If _estado IsNot Nothing AndAlso _estado.Count > 0 Then _
                    .SqlParametros.Add("@estado", SqlDbType.VarChar).Value = Join(_estado.ToArray(), ",")
                If Not String.IsNullOrEmpty(_email) Then .SqlParametros.Add("@email", SqlDbType.VarChar, 250).Value = _email
                If _fechaInicio > Date.MinValue Then .SqlParametros.Add("@fechaInicio", SqlDbType.DateTime).Value = _fechaInicio
                If _fechaFin > Date.MinValue Then .SqlParametros.Add("@fechaFinal", SqlDbType.DateTime).Value = _fechaFin

                .ejecutarReader("ObtenerUsuarioNotificacion", CommandType.StoredProcedure)
                If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                    Dim objUsuarioNotificacion As UsuarioNotificacionCEM
                    While .Reader.Read
                        objUsuarioNotificacion = New UsuarioNotificacionCEM()
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