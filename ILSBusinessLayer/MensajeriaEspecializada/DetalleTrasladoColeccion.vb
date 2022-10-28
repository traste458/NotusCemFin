Imports System.Reflection
Imports LMDataAccessLayer

Public Class DetalleTrasladoColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idDetalle As Integer
    Private _idTraslado As Integer
    Private _serial As String
    Private _fechaInicial As Date
    Private _fechaFinal As Date
    Private _fecha As DateTime
    Private _cargado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idTraslado As Integer)
        Me.New()
        _idTraslado = idTraslado
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As DetalleTraslado
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As DetalleTraslado)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public ReadOnly Property IdDetalle() As Long
        Get
            Return _idDetalle
        End Get
    End Property

    Public Property IdTraslado() As Long
        Get
            Return _idTraslado
        End Get
        Set(ByVal value As Long)
            _idTraslado = value
        End Set
    End Property

    Public Property Serial() As String
        Get
            Return _serial
        End Get
        Set(ByVal value As String)
            _serial = value
        End Set
    End Property

    Public Property Fecha() As DateTime
        Get
            Return _fecha
        End Get
        Set(ByVal value As DateTime)
            _fecha = value
        End Set
    End Property

    Public Property FechaInicial() As Date
        Get
            Return _fechaInicial
        End Get
        Set(ByVal value As Date)
            _fechaInicial = value
        End Set
    End Property

    Public Property FechaFinal() As Date
        Get
            Return _fechaFinal
        End Get
        Set(ByVal value As Date)
            _fechaFinal = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim objDetalleTraslado As Type = GetType(DetalleTraslado)
        Dim pInfo As PropertyInfo

        For Each pInfo In objDetalleTraslado.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As DetalleTraslado)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As DetalleTraslado)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As DetalleTrasladoColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As DetalleTraslado)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub

    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub

    Public Function IndiceDe(ByVal serial As String) As Integer
        Dim indice As Integer = -1
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), DetalleTraslado)
                If .Serial = serial Then
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
        Dim miDetalle As DetalleTraslado

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miDetalle = CType(Me.InnerList(index), DetalleTraslado)
            If miDetalle IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(DetalleTraslado).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
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
                If Me._idDetalle > 0 Then .SqlParametros.Add("@idDetalle", SqlDbType.Int).Value = Me._idDetalle
                If Me._idTraslado > 0 Then .SqlParametros.Add("@idTraslado", SqlDbType.Int).Value = Me._idTraslado
                If Me._serial > 0 Then .SqlParametros.Add("@serial", SqlDbType.BigInt).Value = Me._serial                
                .ejecutarReader("ObtenerDetalleTrasladoServicioMensajeria", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim elDetalle As DetalleTraslado
                    While .Reader.Read
                        elDetalle = New DetalleTraslado
                        elDetalle.CargarResultadoConsulta(.Reader)
                        _cargado = True
                        Me.InnerList.Add(elDetalle)
                    End While
                    .Reader.Close()
                End If
            End With
            _cargado = True
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

    Public Sub Registrar()
        Dim db As New LMDataAccess
        Try
            For Each detalle As DetalleTraslado In Me.List
                With db
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idTraslado", SqlDbType.Int).Value = detalle.IdTraslado
                    .SqlParametros.Add("@serial", SqlDbType.VarChar).Value = detalle.Serial
                    .ejecutarNonQuery("CrearDetalleTrasladoServicioMensajeria", CommandType.StoredProcedure)
                End With
            Next
        Catch ex As Exception
            Throw New Exception("Error al registrar el detalle del traslado. " & ex.Message)
        End Try
        db.Dispose()
    End Sub

    Public Sub Registrar(ByVal db As LMDataAccess, ByVal idTraslado As Long)
        Try
            For Each detalle As DetalleTraslado In Me.List
                With db
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idTraslado", SqlDbType.Int).Value = idTraslado
                    .SqlParametros.Add("@serial", SqlDbType.VarChar).Value = detalle.Serial
                    .ejecutarNonQuery("CrearDetalleTrasladoServicioMensajeria", CommandType.StoredProcedure)
                End With
            Next

        Catch ex As Exception
            Throw New Exception("Error al registrar el detalle del traslado. " & ex.Message)
        End Try
    End Sub

#End Region
End Class