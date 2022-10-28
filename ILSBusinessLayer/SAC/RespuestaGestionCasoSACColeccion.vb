Imports LMDataAccessLayer
Imports ARBusinessLayer.Enumerados
Imports System.Reflection
Imports System.Web

Namespace SAC

    Public Class RespuestaGestionCasoSACColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idRespuesta As ArrayList
        Private _idGestion As ArrayList
        Private _idOrigenRespuesta As ArrayList
        Private _archivo As String
        Private _fechaRecepcionInicial As Date
        Private _fechaRecepcionFinal As Date
        Private _fechaRegistroInicial As Date
        Private _fechaRegistroFinal As Date
#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idGestion As Integer)
            MyBase.New()
            _idGestion = New ArrayList
            _idGestion.Add(idGestion)
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As RespuestaGestionCasoSAC
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As RespuestaGestionCasoSAC)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public ReadOnly Property IdRespuesta() As ArrayList
            Get
                If _idRespuesta Is Nothing Then _idRespuesta = New ArrayList
                Return _idRespuesta
            End Get
        End Property

        Public ReadOnly Property IdGestion() As ArrayList
            Get
                If _idGestion Is Nothing Then _idGestion = New ArrayList
                Return _idGestion
            End Get
        End Property

        Public ReadOnly Property IdOrigenRespuesta() As ArrayList
            Get
                If _idOrigenRespuesta Is Nothing Then _idOrigenRespuesta = New ArrayList
                Return _idOrigenRespuesta
            End Get
        End Property

        Public Property Archivo() As String
            Get
                Return _archivo
            End Get
            Set(ByVal value As String)
                _archivo = value
            End Set
        End Property

        Public Property FechaRecepcionInicial() As Date
            Get
                Return _fechaRecepcionInicial
            End Get
            Set(ByVal value As Date)
                _fechaRecepcionInicial = value
            End Set
        End Property

        Public Property FechaRecpcionFinal() As Date
            Get
                Return _fechaRecepcionFinal
            End Get
            Set(ByVal value As Date)
                _fechaRecepcionFinal = value
            End Set
        End Property

        Public Property FechaRegistroInicial() As Date
            Get
                Return _fechaRegistroInicial
            End Get
            Set(ByVal value As Date)
                _fechaRegistroInicial = value
            End Set
        End Property

        Public Property FechaRegistroFinal() As Date
            Get
                Return _fechaRegistroFinal
            End Get
            Set(ByVal value As Date)
                _fechaRegistroFinal = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim tipo As Type = GetType(RespuestaGestionCasoSAC)
            Dim pInfo As PropertyInfo

            For Each pInfo In tipo.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As RespuestaGestionCasoSAC)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As RespuestaGestionCasoSAC)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As RespuestaGestionCasoSACColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub AdicionarRango(ByVal rango As RespuestaGestionCasoSACColeccion, ByVal limpiarActual As Boolean)
            If limpiarActual Then Me.InnerList.Clear()
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As RespuestaGestionCasoSAC)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function ExisteArchivo(ByVal archivo As String) As Boolean
            Dim existe As Boolean = False
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), RespuestaGestionCasoSAC)
                    If .NombreArchivo.Trim = archivo.Trim Then
                        existe = True
                        Exit For
                    End If
                End With
            Next
            Return existe
        End Function

        Public Function IndiceDe(ByVal idRespuesta As Integer) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), RespuestaGestionCasoSAC)
                    If .IdRespuesta = idRespuesta Then
                        indice = index
                        Exit For
                    End If
                End With
            Next
            Return indice
        End Function

        Public Function IndiceDe(ByVal archivo As String) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), RespuestaGestionCasoSAC)
                    If .NombreArchivo.Trim = archivo.Trim Then
                        indice = index
                        Exit For
                    End If
                End With
            Next
            Return indice
        End Function

        Public Function GenerarDataTable() As DataTable
            Dim dtAux As DataTable = CrearEstructuraDeTabla()
            Dim drAux As DataRow
            Dim respuesta As RespuestaGestionCasoSAC

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                respuesta = CType(Me.InnerList(index), RespuestaGestionCasoSAC)
                If respuesta IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(RespuestaGestionCasoSAC).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(respuesta, Nothing)
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
                    If Me._idRespuesta IsNot Nothing AndAlso Me._idRespuesta.Count > 0 Then _
                        .SqlParametros.Add("@listaIdRespuesta", SqlDbType.VarChar, 1000).Value = Join(Me._idRespuesta.ToArray, ",")
                    If Me._idGestion IsNot Nothing AndAlso Me._idGestion.Count > 0 Then _
                        .SqlParametros.Add("@listaIdGestion", SqlDbType.VarChar, 1000).Value = Join(Me._idGestion.ToArray, ",")
                    If Me._idOrigenRespuesta IsNot Nothing AndAlso Me._idOrigenRespuesta.Count > 0 Then _
                        .SqlParametros.Add("@listaIdOrigenRespuesta", SqlDbType.VarChar, 100).Value = Join(Me._idOrigenRespuesta.ToArray, ",")
                    If Me._fechaRecepcionInicial > Date.MinValue OrElse Me._fechaRecepcionFinal > Date.MinValue Then
                        If Me._fechaRecepcionInicial > Date.MinValue And Me._fechaRecepcionFinal = Date.MinValue _
                            Then Me._fechaRecepcionFinal = Me._fechaRecepcionInicial
                        If Me._fechaRecepcionInicial = Date.MinValue And Me._fechaRecepcionFinal > Date.MinValue _
                            Then Me._fechaRecepcionInicial = Me._fechaRecepcionFinal

                        .SqlParametros.Add("@fechaRecepcionInicial", SqlDbType.SmallDateTime).Value = Me._fechaRecepcionInicial
                        .SqlParametros.Add("@fechaRecepcionFinal", SqlDbType.SmallDateTime).Value = Me._fechaRecepcionFinal
                    End If
                    If Me._fechaRegistroInicial > Date.MinValue OrElse Me._fechaRegistroFinal > Date.MinValue Then
                        If Me._fechaRegistroInicial > Date.MinValue And Me._fechaRegistroFinal = Date.MinValue _
                            Then Me._fechaRegistroFinal = Me._fechaRegistroInicial
                        If Me._fechaRegistroInicial = Date.MinValue And Me._fechaRegistroFinal > Date.MinValue _
                            Then Me._fechaRegistroInicial = Me._fechaRegistroFinal

                        .SqlParametros.Add("@fechaRegistroInicial", SqlDbType.SmallDateTime).Value = Me._fechaRegistroInicial
                        .SqlParametros.Add("@fechaRegistroFinal", SqlDbType.SmallDateTime).Value = Me._fechaRegistroFinal
                    End If
                    .ejecutarReader("ConsultarRespuestaGestionCasoSAC", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim respuesta As RespuestaGestionCasoSAC
                        Dim fecha As Date

                        While .Reader.Read
                            respuesta = New RespuestaGestionCasoSAC
                            Integer.TryParse(.Reader("idRespuesta").ToString, respuesta.IdRespuesta)
                            Integer.TryParse(.Reader("idGestion").ToString, respuesta.IdGestion)
                            Byte.TryParse(.Reader("idOrigenRespuesta").ToString, respuesta.IdOrigenRespuesta)
                            respuesta.Descripcion = .Reader("descripcion").ToString
                            respuesta.OrigenRespuesta = .Reader("origenRespuesta").ToString
                            Boolean.TryParse(.Reader("requiereArchivo").ToString, respuesta.RequiereArchivo)
                            respuesta.NombreArchivo = .Reader("archivo").ToString
                            respuesta.NombreArchivoConRuta = HttpContext.Current.Server.MapPath(.Reader("archivoConRuta").ToString)
                            respuesta.NombreArchivoOriginal = .Reader("archivoOriginal").ToString
                            Date.TryParse(.Reader("fechaRecepcion").ToString, respuesta.FechaRecepcion)
                            Date.TryParse(.Reader("fechaRegistro").ToString, fecha)
                            respuesta.EstablecerFechaRegistro(fecha)
                            respuesta.MarcarComoRegistrado()

                            Me.InnerList.Add(respuesta)
                        End While
                        .Reader.Close()
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

#End Region

#Region "Métodos Compartidos"

        Public Shared Function ObtenerTodosEnDataTable() As DataTable
            Dim dtAux As New DataTable
            Dim dbManager As New LMDataAccess

            Try
                dtAux = dbManager.ejecutarDataTable("ConsultarRespuestaGestionCasoSAC", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtAux
        End Function

#End Region

    End Class

End Namespace

