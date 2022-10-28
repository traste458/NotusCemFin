Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Public Class GestionServicioTecnicoColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idGestion As Integer
    Private _idDetalleSerial As Long

    Private _cargado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idDetalleSerial As Long)
        Me.New()
        _idDetalleSerial = idDetalleSerial
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Default Property Item(ByVal index As Integer) As GestionServicioTecnico
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As GestionServicioTecnico)
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

    Public Property IdDetalleSerial() As Long
        Get
            Return _idDetalleSerial
        End Get
        Set(ByVal value As Long)
            _idDetalleSerial = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim miGestionServicioTecnico As Type = GetType(GestionServicioTecnico)
        Dim pInfo As PropertyInfo

        For Each pInfo In miGestionServicioTecnico.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As GestionServicioTecnico)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As GestionServicioTecnico)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As GestionServicioTecnicoColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As GestionServicioTecnico)
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
            With CType(Me.InnerList(index), GestionServicioTecnico)
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
        Dim miGestion As GestionServicioTecnico

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miGestion = CType(Me.InnerList(index), GestionServicioTecnico)
            If miGestion IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(GestionServicioTecnico).GetProperties
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
                    If _idGestion > 0 Then .SqlParametros.Add("@idGestion", SqlDbType.Int).Value = _idGestion
                    If _idDetalleSerial > 0 Then .SqlParametros.Add("@idDetalleSerial", SqlDbType.BigInt).Value = _idDetalleSerial

                    .ejecutarReader("ObtenerGestionServicioTecnico", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim laGestion As GestionServicioTecnico

                        While .Reader.Read
                            laGestion = New GestionServicioTecnico
                            laGestion.IdGestion = CInt(.Reader("idGestion"))
                            laGestion.IdDetalleSerial = CLng(.Reader("idDetalleSerial"))
                            laGestion.Fecha = CDate(.Reader("fecha"))
                            Integer.TryParse(.Reader("idUsuario"), laGestion.IdUsuario)
                            If Not IsDBNull(.Reader("nombreUsuario")) Then laGestion.NombreUsuario = .Reader("nombreUsuario").ToString()
                            laGestion.Observacion = .Reader("observacion").ToString

                            Me.InnerList.Add(laGestion)
                        End While
                        If Not .Reader.IsClosed Then .Reader.Close()
                    End If
                End With
                _cargado = True
            Catch ex As Exception
                Throw ex
            End Try
        End Using
    End Sub

#End Region

End Class