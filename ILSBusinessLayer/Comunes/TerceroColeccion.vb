Imports LMDataAccessLayer
Imports System.Reflection

Public Class TerceroColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idTercero As Decimal
    Private _listaidArea As ArrayList
    Private _listaidCargo As ArrayList
    Private _listaidCiudad As ArrayList
    Private _listaidCliente As ArrayList
    Private _listaidPos As ArrayList
    Private _listaidSucursal As ArrayList
    Private _listaidBodega As ArrayList
    Private _listaidCentro_costo As ArrayList
    Private _listaidEmpresa_temporal As ArrayList
    Private _listaidCreador As ArrayList
    Private _listaidPerfil As ArrayList
    Private _listaidCac As ArrayList
    Private _listaidClasificacionHorario As ArrayList
    Private _cargado As Boolean

#End Region

#Region "Constructores"
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal IdTercero As Decimal)
        MyBase.New()
        _idTercero = IdTercero
        CargarDatos()
    End Sub
#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As Tercero
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As Tercero)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property
    Public Property IdTercero() As Decimal

        Get
            Return _idTercero
        End Get
        Set(ByVal value As Decimal)
            _idTercero = value
        End Set
    End Property

    Public Property Listaidarea() As ArrayList

        Get
            Return _listaidArea
        End Get
        Set(ByVal value As ArrayList)
            _listaidArea = value
        End Set
    End Property

    Public Property Listaidcargo() As ArrayList

        Get
            Return _listaidCargo
        End Get
        Set(ByVal value As ArrayList)
            _listaidCargo = value
        End Set
    End Property

    Public Property Listaidciudad() As ArrayList

        Get
            Return _listaidCiudad
        End Get
        Set(ByVal value As ArrayList)
            _listaidCiudad = value
        End Set
    End Property

    Public Property Listaidcliente() As ArrayList

        Get
            Return _listaidCliente
        End Get
        Set(ByVal value As ArrayList)
            _listaidCliente = value
        End Set
    End Property

    Public Property Listaidpos() As ArrayList

        Get
            Return _listaidPos
        End Get
        Set(ByVal value As ArrayList)
            _listaidPos = value
        End Set
    End Property

    Public Property Listaidsucursal() As ArrayList

        Get
            Return _listaidSucursal
        End Get
        Set(ByVal value As ArrayList)
            _listaidSucursal = value
        End Set
    End Property

    Public Property Listaidbodega() As ArrayList

        Get
            Return _listaidBodega
        End Get
        Set(ByVal value As ArrayList)
            _listaidBodega = value
        End Set
    End Property

    Public Property Listaidcentro_costo() As ArrayList

        Get
            Return _listaidCentro_costo
        End Get
        Set(ByVal value As ArrayList)
            _listaidCentro_costo = value
        End Set
    End Property

    Public Property Listaidempresa_temporal() As ArrayList

        Get
            Return _listaidEmpresa_temporal
        End Get
        Set(ByVal value As ArrayList)
            _listaidEmpresa_temporal = value
        End Set
    End Property

    Public Property Listaidcreador() As ArrayList

        Get
            Return _listaidCreador
        End Get
        Set(ByVal value As ArrayList)
            _listaidCreador = value
        End Set
    End Property

    Public Property Listaidperfil() As ArrayList
        Get
            If _listaidPerfil Is Nothing Then _listaidPerfil = New ArrayList
            Return _listaidPerfil
        End Get
        Set(ByVal value As ArrayList)
            _listaidPerfil = value
        End Set
    End Property

    Public Property Listaidcac() As ArrayList

        Get
            Return _listaidCac
        End Get
        Set(ByVal value As ArrayList)
            _listaidCac = value
        End Set
    End Property

    Public Property ListaidClasificacionHorario() As ArrayList

        Get
            Return _listaidClasificacionHorario
        End Get
        Set(ByVal value As ArrayList)
            _listaidClasificacionHorario = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim miTercero As Type = GetType(Tercero)
        Dim pInfo As PropertyInfo
        For Each pInfo In miTercero.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As Tercero)
        Me.InnerList.Insert(posicion, valor)
    End Sub


    Public Sub Adicionar(ByVal valor As Tercero)
        Me.InnerList.Add(valor)
    End Sub


    Public Sub AdicionarRango(ByVal rango As TerceroColeccion)
        Me.InnerList.AddRange(rango)
    End Sub


    Public Sub Remover(ByVal valor As Tercero)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub


    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub


    Public Function IndiceDe(ByVal idTercero As Integer) As Integer
        Dim indice As Integer = -1
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), Tercero)
                If .IdTercero = idTercero Then indice = index
                Exit For

            End With
        Next
        Return indice
    End Function

    Public Function ItemPorIdentificador(ByVal identificador As Integer) As Tercero
        Dim resultado As Tercero
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), Tercero)
                If .IdTercero = identificador Then
                    resultado = CType(Me.InnerList(index), Tercero)
                    Exit For
                End If
            End With
        Next
        Return resultado
    End Function
    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miTercero As Tercero
        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miTercero = CType(Me.InnerList(index), Tercero)
            If miTercero IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(Tercero).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(miTercero, Nothing)
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
                If Me._idTercero > 0 Then .SqlParametros.Add("@idTercero", SqlDbType.Decimal).Value = Me._idTercero
                If Me._listaidArea IsNot Nothing AndAlso Me._listaidArea.Count > 0 Then _
                .SqlParametros.Add("@listaidArea", SqlDbType.VarChar, 500).Value = Join(_listaidArea.ToArray, ",")
                If Me._listaidCargo IsNot Nothing AndAlso Me._listaidCargo.Count > 0 Then _
                .SqlParametros.Add("@listaidCargo", SqlDbType.VarChar, 500).Value = Join(_listaidCargo.ToArray, ",")
                If Me._listaidCiudad IsNot Nothing AndAlso Me._listaidCiudad.Count > 0 Then _
                .SqlParametros.Add("@listaidCiudad", SqlDbType.VarChar, 500).Value = Join(_listaidCiudad.ToArray, ",")
                If Me._listaidCliente IsNot Nothing AndAlso Me._listaidCliente.Count > 0 Then _
                .SqlParametros.Add("@listaidCliente", SqlDbType.VarChar, 500).Value = Join(_listaidCliente.ToArray, ",")
                If Me._listaidPos IsNot Nothing AndAlso Me._listaidPos.Count > 0 Then _
                .SqlParametros.Add("@listaidPos", SqlDbType.VarChar, 500).Value = Join(_listaidPos.ToArray, ",")
                If Me._listaidSucursal IsNot Nothing AndAlso Me._listaidSucursal.Count > 0 Then _
                .SqlParametros.Add("@listaidSucursal", SqlDbType.VarChar, 500).Value = Join(_listaidSucursal.ToArray, ",")
                If Me._listaidBodega IsNot Nothing AndAlso Me._listaidBodega.Count > 0 Then _
                .SqlParametros.Add("@listaidBodega", SqlDbType.VarChar, 500).Value = Join(_listaidBodega.ToArray, ",")
                If Me._listaidCentro_costo IsNot Nothing AndAlso Me._listaidCentro_costo.Count > 0 Then _
                .SqlParametros.Add("@listaidCentro_costo", SqlDbType.VarChar, 500).Value = Join(_listaidCentro_costo.ToArray, ",")
                If Me._listaidEmpresa_temporal IsNot Nothing AndAlso Me._listaidEmpresa_temporal.Count > 0 Then _
                .SqlParametros.Add("@listaidEmpresa_temporal", SqlDbType.VarChar, 500).Value = Join(_listaidEmpresa_temporal.ToArray, ",")
                If Me._listaidCreador IsNot Nothing AndAlso Me._listaidCreador.Count > 0 Then _
                .SqlParametros.Add("@listaidCreador", SqlDbType.VarChar, 500).Value = Join(_listaidCreador.ToArray, ",")
                If Me._listaidPerfil IsNot Nothing AndAlso Me._listaidPerfil.Count > 0 Then _
                .SqlParametros.Add("@listaidPerfil", SqlDbType.VarChar, 500).Value = Join(_listaidPerfil.ToArray, ",")
                If Me._listaidCac IsNot Nothing AndAlso Me._listaidCac.Count > 0 Then _
                .SqlParametros.Add("@listaidCac", SqlDbType.VarChar, 500).Value = Join(_listaidCac.ToArray, ",")
                If Me._listaidClasificacionHorario IsNot Nothing AndAlso Me._listaidClasificacionHorario.Count > 0 Then _
                .SqlParametros.Add("@listaidClasificacionHorario", SqlDbType.VarChar, 500).Value = Join(_listaidClasificacionHorario.ToArray, ",")

                .ejecutarReader("ObtenerTerceros", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    Dim elTercero As Tercero
                    While .Reader.Read
                        elTercero = New Tercero
                        elTercero.CargarResultadoConsulta(.Reader)
                        _cargado = True
                        Me.InnerList.Add(elTercero)
                    End While
                    .Reader.Close()
                End If
            End With
            _cargado = True
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

End Class
