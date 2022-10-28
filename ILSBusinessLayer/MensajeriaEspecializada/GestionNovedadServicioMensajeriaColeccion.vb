Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Public Class GestionNovedadServicioMensajeriaColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idGestion As Integer
    Private _idNovedad As Integer

    Private _cargado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idNovedad As Integer)
        Me.New()
        _idNovedad = idNovedad
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As GestionNovedadServicioMensajeria
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As GestionNovedadServicioMensajeria)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
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

    Public Property IdNovedad() As Integer
        Get
            Return _idNovedad
        End Get
        Set(ByVal value As Integer)
            _idNovedad = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim miGestion As Type = GetType(GestionNovedadServicioMensajeria)
        Dim pInfo As PropertyInfo

        For Each pInfo In miGestion.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As GestionNovedadServicioMensajeria)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As GestionNovedadServicioMensajeria)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As GestionNovedadServicioMensajeria)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As GestionNovedadServicioMensajeria)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub

    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub

    Public Function IndiceDe(ByVal idGestion As Integer) As Integer
        Dim indice As Integer = -1
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), GestionNovedadServicioMensajeria)
                If .IdGestion = idGestion Then
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
        Dim miGestion As GestionNovedadServicioMensajeria

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miGestion = CType(Me.InnerList(index), GestionNovedadServicioMensajeria)
            If miGestion IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(GestionNovedadServicioMensajeria).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(miGestion, Nothing)
                    End If
                Next
                dtAux.Rows.Add(drAux)
            End If
        Next

        Return dtAux
    End Function

    Public Sub CargarDatos()
        Using dbManager As New LMDataAccess
            Try
                Me.Clear()
                With dbManager
                    If Me._idGestion > 0 Then .SqlParametros.Add("@idGestion", SqlDbType.Int).Value = Me._idGestion
                    If Me._idNovedad > 0 Then .SqlParametros.Add("@idNovedad", SqlDbType.Int).Value = Me._idNovedad

                    .ejecutarReader("ObtenerGestionNovedadServicioMensajeria", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim objGestionNovedad As GestionNovedadServicioMensajeria

                        While .Reader.Read
                            objGestionNovedad = New GestionNovedadServicioMensajeria()
                            Integer.TryParse(.Reader("idGestion").ToString, objGestionNovedad.IdGestion)
                            Integer.TryParse(.Reader("idNovedad").ToString, objGestionNovedad.IdNovedad)
                            objGestionNovedad.Observacion = .Reader("observacion").ToString
                            Integer.TryParse(.Reader("idUsuario").ToString, objGestionNovedad.IdUsuario)
                            objGestionNovedad.FechaRegistro = CDate(.Reader("fechaRegistro"))
                            _cargado = True
                            Me.InnerList.Add(objGestionNovedad)
                        End While
                        .Reader.Close()
                    End If
                End With
                _cargado = True
            Catch:End Try
        End Using
    End Sub

#End Region

End Class
