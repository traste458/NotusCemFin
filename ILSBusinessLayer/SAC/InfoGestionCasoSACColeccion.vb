Imports LMDataAccessLayer
Imports System.Reflection

Namespace SAC

    Public Class InfoGestionCasoSACColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idGestion As Integer
        Private _idCaso As Integer
        Private _idCliente As Short
        Private _idGestionador As Short
        Private _fechaGestionInicial As Date
        Private _fechaGestionFinal As Date
        Private _fechaRegistroInicial As Date
        Private _fechaRegistroFinal As Date
        Private _idUsuarioRegistra As Integer
#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idCaso As Integer)
            MyBase.New()
            _idCaso = idCaso
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As InfoGestionCasoSAC
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As InfoGestionCasoSAC)
                If value IsNot Nothing Then
                    Me.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property IdGestion() As Integer
            Get
                Return _idGestion
            End Get
            Set(ByVal value As Integer)
                _idGestion = value
            End Set
        End Property

        Public Property IdCaso() As Integer
            Get
                Return _idCaso
            End Get
            Set(ByVal value As Integer)
                _idCaso = value
            End Set
        End Property

        Public Property IdCliente() As Short
            Get
                Return _idCliente
            End Get
            Set(ByVal value As Short)
                _idCliente = value
            End Set
        End Property

        Public Property IdGestionador() As Short
            Get
                Return _idGestionador
            End Get
            Set(ByVal value As Short)
                _idGestionador = value
            End Set
        End Property

        Public Property FechaGestionInicial() As Date
            Get
                Return _fechaGestionInicial
            End Get
            Set(ByVal value As Date)
                _fechaGestionInicial = value
            End Set
        End Property

        Public Property FechaGestionFinal() As Date
            Get
                Return _fechaGestionFinal
            End Get
            Set(ByVal value As Date)
                _fechaGestionFinal = value
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

        Public Property IdUsuarioRegistra() As Integer
            Get
                Return _idUsuarioRegistra
            End Get
            Set(ByVal value As Integer)
                _idUsuarioRegistra = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim tipo As Type = GetType(InfoGestionCasoSAC)
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As InfoGestionCasoSAC)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As InfoGestionCasoSAC)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As InfoGestionCasoSACColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As InfoGestionCasoSAC)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function ExisteIdGestion(ByVal idGestion As Integer) As Boolean
            Dim existe As Boolean = False
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), InfoGestionCasoSAC)
                    If .IdGestion = idGestion Then
                        existe = True
                        Exit For
                    End If
                End With
            Next
            Return existe
        End Function

        Public Function IndiceDe(ByVal idGestion As Integer) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), InfoGestionCasoSAC)
                    If .IdGestion = idGestion Then
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
            Dim infoGestion As InfoGestionCasoSAC
            Dim valor As Object

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                infoGestion = CType(Me.InnerList(index), InfoGestionCasoSAC)
                If infoGestion IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(InfoGestionCasoSAC).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            valor = pInfo.GetValue(infoGestion, Nothing)
                            If TypeOf valor Is Date Then
                                If CDate(valor) > Date.MinValue Then drAux(pInfo.Name) = valor
                            Else
                                drAux(pInfo.Name) = valor
                            End If
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

                    If Me._idGestion > 0 Then .SqlParametros.Add("@idGestion", SqlDbType.Int).Value = Me._idGestion
                    If Me._idCaso > 0 Then .SqlParametros.Add("@idCaso", SqlDbType.Int).Value = Me._idCaso
                    If Me._idCliente > 0 Then .SqlParametros.Add("@idCliente", SqlDbType.SmallInt).Value = Me._idCliente
                    If Me._idGestionador Then .SqlParametros.Add("@idGestionador", SqlDbType.Int).Value = Me._idGestionador
                    If Me._fechaGestionInicial > Date.MinValue OrElse Me._fechaGestionFinal > Date.MinValue Then
                        If Me._fechaGestionInicial > Date.MinValue And Me._fechaGestionFinal = Date.MinValue _
                            Then Me._fechaGestionFinal = Me._fechaGestionInicial
                        If Me._fechaGestionInicial = Date.MinValue And Me._fechaGestionFinal > Date.MinValue _
                            Then Me._fechaGestionInicial = Me._fechaGestionFinal

                        .SqlParametros.Add("@fechaGestionInicial", SqlDbType.SmallDateTime).Value = Me._fechaGestionInicial
                        .SqlParametros.Add("@fechaGestionFinal", SqlDbType.SmallDateTime).Value = Me._fechaGestionFinal
                    End If
                    If Me._fechaRegistroInicial > Date.MinValue OrElse Me._fechaRegistroFinal > Date.MinValue Then
                        If Me._fechaRegistroInicial > Date.MinValue And Me._fechaRegistroFinal = Date.MinValue _
                            Then Me._fechaRegistroFinal = Me._fechaRegistroInicial
                        If Me._fechaRegistroInicial = Date.MinValue And Me._fechaRegistroFinal > Date.MinValue _
                            Then Me._fechaRegistroInicial = Me._fechaRegistroFinal

                        .SqlParametros.Add("@fechaRegistroInicial", SqlDbType.SmallDateTime).Value = Me._fechaRegistroInicial
                        .SqlParametros.Add("@fechaRegistroFinal", SqlDbType.SmallDateTime).Value = Me._fechaRegistroFinal
                    End If
                    If Me._idUsuarioRegistra Then .SqlParametros.Add("@idUsuarioRegistra", SqlDbType.Int).Value = Me._idUsuarioRegistra

                    .ejecutarReader("ConsultarInfoGestionCasoSAC", CommandType.StoredProcedure)

                    Try
                        If .Reader IsNot Nothing Then
                            Dim infoGestion As InfoGestionCasoSAC
                            Dim idGestion As Int64
                            Dim fecha As Date
                                While .Reader.Read
                                    infoGestion = New InfoGestionCasoSAC
                                    Integer.TryParse(.Reader("idGestion").ToString, idGestion)
                                    infoGestion.EstablecerIdentificador(idGestion)
                                    Integer.TryParse(.Reader("idCaso").ToString, infoGestion.IdCaso)
                                    Short.TryParse(.Reader("idTipoGestion").ToString, infoGestion.IdTipoGestion)
                                    infoGestion.EstablecerTipoGestion(.Reader("tipoGestion").ToString)
                                    infoGestion.Descripcion = .Reader("descripcion").ToString
                                    Short.TryParse(.Reader("idCliente").ToString, infoGestion.IdCliente)
                                    infoGestion.EstablecerCliente(.Reader("cliente").ToString)
                                    Integer.TryParse(.Reader("idGestionador").ToString, infoGestion.IdGestionador)
                                    infoGestion.EstablecerGestionador(.Reader("gestionador").ToString)
                                    Date.TryParse(.Reader("fechaGestion").ToString, infoGestion.FechaDeGestion)
                                    Date.TryParse(.Reader("fechaRegistro").ToString, fecha)
                                    infoGestion.EstablecerFechaRegistro(fecha)
                                    Integer.TryParse(.Reader("idUsuarioRegistra").ToString, infoGestion.IdUsuarioRegistra)
                                    infoGestion.EstablecerUsuarioRegistra(.Reader("usuarioRegistra").ToString)
                                    infoGestion.MarcarComoRegistrado()

                                    Me.InnerList.Add(infoGestion)
                                End While
                                .Reader.Close()
                            End If
                    Catch ex As Exception
                        Throw New Exception("Error al tratar de cargar el listado de Gestiones del Caso registradas." & ex.Message)

                    End Try
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
                dtAux = dbManager.EjecutarDataTable("ConsultarInfoGestionCasoSAC", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtAux
        End Function

#End Region

    End Class

End Namespace

