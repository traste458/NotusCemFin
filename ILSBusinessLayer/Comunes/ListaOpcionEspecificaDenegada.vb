Imports LMDataAccessLayer
Imports System.Reflection

Namespace PermisoOpcion
    Public Class ListaOpcionEspecificaDenegada
        Inherits CollectionBase
#Region "Atributos (Filtros de Búsqueda)"

        Private _idOpcionEspecifica As Integer
        Private _idOpcionFuncional As Integer
        Private _idUsuario As Integer
        Private _identificadorDenegacion As Integer
        Private _arrListaOpcionEspecifica As ArrayList

        Private _cargado As Boolean
        Private _OpcionEspecifica As OpcionEspecificaDenegada

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _arrListaOpcionEspecifica = New ArrayList
        End Sub

        Public Sub New(ByVal idOpcion As Integer)
            Me.New()
            _idOpcionEspecifica = idOpcion
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As OpcionEspecificaDenegada
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As OpcionEspecificaDenegada)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property IdOpcionEspecifica() As Integer
            Get
                Return _idOpcionEspecifica
            End Get
            Set(ByVal value As Integer)
                _idOpcionEspecifica = value
            End Set
        End Property

        Public Property IdOpcionFuncional() As Integer
            Get
                Return _idOpcionFuncional
            End Get
            Set(ByVal value As Integer)
                _idOpcionFuncional = value
            End Set
        End Property

        Public Property ListadoOpcionEspecifica() As ArrayList
            Get
                If _arrListaOpcionEspecifica Is Nothing Then _arrListaOpcionEspecifica = New ArrayList
                Return _arrListaOpcionEspecifica
            End Get
            Set(ByVal value As ArrayList)
                _arrListaOpcionEspecifica = value
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

        Public Property IdentificadorDenegacion() As Integer
            Get
                Return _identificadorDenegacion
            End Get
            Set(ByVal value As Integer)
                _identificadorDenegacion = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim miOpcionEspecifica As Type = GetType(OpcionEspecificaDenegada)
            Dim pInfo As PropertyInfo

            For Each pInfo In miOpcionEspecifica.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As OpcionEspecificaDenegada)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As OpcionEspecificaDenegada)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As ListaOpcionEspecificaDenegada)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As OpcionEspecificaDenegada)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function IndiceDe(ByVal idOpcionEspecifica As Integer) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), OpcionEspecificaDenegada)
                    If .IdOpcionEspecifica = idOpcionEspecifica Then
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
            Dim miOpcionEspecifica As OpcionEspecificaDenegada

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miOpcionEspecifica = CType(Me.InnerList(index), OpcionEspecificaDenegada)
                If miOpcionEspecifica IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(OpcionEspecificaDenegada).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miOpcionEspecifica, Nothing)
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
                    EstablecerFiltros(dbManager)
                    .ejecutarReader("ObtenerOpcionEspecificaDenegada", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then

                        While .Reader.Read
                            _OpcionEspecifica = New OpcionEspecificaDenegada
                            _OpcionEspecifica.IdOpcionEspecifica = .Reader("idListadoOpcion")
                            _OpcionEspecifica.IdOpcionFuncional = .Reader("idOpcionFuncional")
                            _OpcionEspecifica.NombreOpcionFuncional = .Reader("nombreOpcionFuncional")
                            _OpcionEspecifica.NombreOpcionEspecifica = .Reader("nombreOpcionEspecifica")
                            _OpcionEspecifica.Activo = .Reader("activo")
                            _OpcionEspecifica.IdPerfil = .Reader("idPerfil")
                            _OpcionEspecifica.IdDenegacion = .Reader("idDenegacion")
                            _OpcionEspecifica.IdentificadorDenegacion = .Reader("identificador")
                            _OpcionEspecifica.Registrado = True
                            Me.InnerList.Add(_OpcionEspecifica)
                        End While
                        .Reader.Close()
                    End If
                End With
                _cargado = True
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Public Sub EstablecerFiltros(ByVal db As LMDataAccess)
            With db
                If Me._idOpcionFuncional > 0 Then _
                          .SqlParametros.Add("@idOpcionFuncional", SqlDbType.Int).Value = Me._idOpcionFuncional
                If Me._identificadorDenegacion > 0 Then _
                          .SqlParametros.Add("@identificador", SqlDbType.Int).Value = Me._identificadorDenegacion
                If Me._idOpcionEspecifica > 0 Then _
                          .SqlParametros.Add("@idListadoOpcion", SqlDbType.Int).Value = Me._idOpcionEspecifica
                If Me._arrListaOpcionEspecifica IsNot Nothing AndAlso Me._arrListaOpcionEspecifica.Count > 0 Then _
                    .SqlParametros.Add("@listaOpcionEspecifica", SqlDbType.VarChar, 1000).Value = Join(Me._arrListaOpcionEspecifica.ToArray, ",")
                If Me._idUsuario > 0 Then _
                    .SqlParametros.Add("@idUsuarioConsulta", SqlDbType.Int).Value = Me._idUsuario
            End With
        End Sub

#End Region

#Region "Métodos Compartidos"

        Public Shared Function ObtenerTodosEnDataTable() As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtAux As New DataTable

            Try
                dtAux = dbManager.ejecutarDataTable("ObtenerListaOpcionEspecificaDenegada", CommandType.StoredProcedure)

            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtAux
        End Function
#End Region

    End Class
End Namespace

