Imports LMDataAccessLayer
Imports System.Reflection

Public Class TransportadoraColeccion
    Inherits CollectionBase

#Region "Atributos"

    Private _idTransportadora As Integer
    Private _nombre As String
    Private _estado As Boolean = True
    Private _usaGuia As Boolean
    Private _usaPrecintos As Boolean
    Private _aplicaLogisticaInversa As Boolean
    Private _cargaPorImportacion As Integer
    Private _manejaPOS As String
    Private _aplicaDespachoNacional As Boolean
    Private _registrado As Boolean

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As Transportadora
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As Transportadora)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdTransportadora() As Integer
        Get
            Return _idTransportadora
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idTransportadora = value
        End Set
    End Property

    Public Property Nombre() As String
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
        End Set
    End Property

    Public Property Estado() As Boolean
        Get
            Return _estado
        End Get
        Set(ByVal value As Boolean)
            _estado = value
        End Set
    End Property

    Public Property UsaGuia() As Boolean
        Get
            Return _usaGuia
        End Get
        Set(ByVal value As Boolean)
            _usaGuia = value
        End Set
    End Property

    Public Property UsaPrecintos() As Boolean
        Get
            Return _usaPrecintos
        End Get
        Set(ByVal value As Boolean)
            _usaPrecintos = value
        End Set
    End Property

    Public Property AplicaLogisticaInversa() As Boolean
        Get
            Return _aplicaLogisticaInversa
        End Get
        Set(ByVal value As Boolean)
            _aplicaLogisticaInversa = value
        End Set
    End Property

    Public Property CargaPorImportacion() As Integer
        Get
            Return _cargaPorImportacion
        End Get
        Set(ByVal value As Integer)
            _cargaPorImportacion = value
        End Set
    End Property

    Public Property AplicaDespachoNacional() As Boolean
        Get
            Return _aplicaDespachoNacional
        End Get
        Set(ByVal value As Boolean)
            _aplicaDespachoNacional = value
        End Set
    End Property

    Public Property Registrado() As Boolean
        Get
            Return _registrado
        End Get
        Protected Friend Set(ByVal value As Boolean)
            _registrado = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim miObj As Type = GetType(Transportadora)
        Dim pInfo As PropertyInfo

        For Each pInfo In miObj.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As Transportadora)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As Transportadora)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As Transportadora)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As Transportadora)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub

    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub

    Public Function IndiceDe(ByVal idEstado As Short) As Integer
        Dim indice As Integer = -1
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), Transportadora)
                If .Estado = idEstado Then
                    indice = index
                    Exit For
                End If
            End With
        Next
        Return indice
    End Function

    Public Function GenerarDataTable() As DataTable
        If Not _registrado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Try
            Dim drAux As DataRow
            Dim miDetalle As Transportadora

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miDetalle = CType(Me.InnerList(index), Transportadora)
                If miDetalle IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(Transportadora).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miDetalle, Nothing)
                        ElseIf pInfo.PropertyType.Namespace = "ILSBusinessLayer.Enumerados" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miDetalle, Nothing)
                        End If
                    Next
                    dtAux.Rows.Add(drAux)
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
        Return dtAux
    End Function

    Public Sub CargarDatos()
        Using dbManager As New LMDataAccess
            Try
                Me.Clear()
                With dbManager
                    .SqlParametros.Add("@estado", SqlDbType.Int).Value = Me._estado
                    If _idTransportadora > 0 Then .SqlParametros.Add("@idTransportadora", SqlDbType.Int).Value = _idTransportadora
                    .ejecutarReader("SeleccionarTransportadoras", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim objDetalle As Transportadora
                        While .Reader.Read
                            objDetalle = New Transportadora
                            objDetalle.IdTransportadora = CInt(.Reader("idTransportadora").ToString)
                            objDetalle.Nombre = .Reader("transportadora").ToString
                            objDetalle.Estado = CInt(.Reader("estado").ToString)
                            objDetalle.UsaGuia = CBool(.Reader("usaGuia").ToString)
                            objDetalle.UsaPrecintos = CBool(.Reader("usaPrecinto").ToString)
                            objDetalle.AplicaLogisticaInversa = CBool(.Reader("aplicaLogisticaInversa").ToString)
                            objDetalle.CargaPorImportacion = CInt(.Reader("cargaPorImportacion").ToString)

                            Me.InnerList.Add(objDetalle)
                        End While
                        .Reader.Close()
                    End If
                End With
                _registrado = True
            Catch ex As Exception
                Throw ex
            End Try
        End Using
    End Sub

#End Region

End Class
