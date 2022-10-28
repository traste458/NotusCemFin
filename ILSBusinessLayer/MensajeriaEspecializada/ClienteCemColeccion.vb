Imports LMDataAccessLayer
Imports System.Reflection

Namespace MensajeriaEspecializada

    ''' <summary>
    ''' Author: Beltrán, Diego
    ''' Create date: 04/08/2014
    ''' Description: Colección diseñada para cargar los diferenties clientes asociados al CEM
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ClienteCemColeccion
        Inherits CollectionBase

#Region "Filtros de Búsqueda"

        Private _listaIdCliente As List(Of Integer)
        Private _nombre As String
        Private _estado As Nullable(Of Boolean)

        Private _cargado As Boolean

#End Region

#Region "Propiedades"

        ''' <summary>
        ''' Propiedad por defecto que instancia la clase a la cual se asigna la colección
        ''' </summary>
        ''' <param name="index"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Default Public Property Item(ByVal index As Integer) As ClienteCem
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As ClienteCem)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Define o establece el listado de IdClientes que se desean consultar
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ListaIdCliente As List(Of Integer)
            Get
                If _listaIdCliente Is Nothing Then _listaIdCliente = New List(Of Integer)
                Return _listaIdCliente
            End Get
            Set(value As List(Of Integer))
                _listaIdCliente = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece el nombre del cliente "%Like%" que se desea consultar
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nombre As String
            Get
                Return _nombre
            End Get
            Set(value As String)
                _nombre = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece el estado del cliente que se desea consultar--activo =true, inactivo =false
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Estado As Nullable(Of Boolean)
            Get
                Return _estado
            End Get
            Set(value As Nullable(Of Boolean))
                _estado = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.new()
        End Sub

#End Region

#Region "Métodos Privados"

        ''' <summary>
        ''' Función que permite crear la estructura de la tabla, basada en las propiedades de la clase instanciada
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim objClienteCem As Type = GetType(ClienteCem)
            Dim pInfo As PropertyInfo

            For Each pInfo In objClienteCem.GetProperties
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

        ''' <summary>
        ''' Método que permite Insertar elementos a la colección
        ''' </summary>
        ''' <param name="posicion"></param>
        ''' <param name="valor"></param>
        ''' <remarks></remarks>
        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As ClienteCem)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        ''' <summary>
        ''' Método que permite Adicionar elementos a la colección
        ''' </summary>
        ''' <param name="valor"></param>
        ''' <remarks></remarks>
        Public Sub Adicionar(ByVal valor As ClienteCem)
            Me.InnerList.Add(valor)
        End Sub

        ''' <summary>
        ''' Método que permite adicionar un rango de elementos a la colección
        ''' </summary>
        ''' <param name="rango"></param>
        ''' <remarks></remarks>
        Public Sub AdicionarRango(ByVal rango As ClienteCem)
            Me.InnerList.AddRange(rango)
        End Sub

        ''' <summary>
        ''' Método que permite generar un elemento de tipo datatable
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GenerarDataTable() As DataTable
            If Not _cargado Then CargarDatos()
            Dim dtAux As DataTable = CrearEstructuraDeTabla()
            Dim drAux As DataRow
            Dim miRegistro As ClienteCem

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miRegistro = CType(Me.InnerList(index), ClienteCem)
                If miRegistro IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(ClienteCem).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miRegistro, Nothing)
                        End If
                    Next
                    dtAux.Rows.Add(drAux)
                End If
            Next

            Return dtAux
        End Function

        ''' <summary>
        ''' Método que permite cargar la colección con los datos obtenidos
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CargarDatos()
            Dim dbManager As New LMDataAccess

            If _cargado Then Me.InnerList.Clear()
            With dbManager
                With .SqlParametros
                    If _listaIdCliente IsNot Nothing AndAlso _listaIdCliente.Count > 0 Then _
                        .Add("@listaIdCliente", SqlDbType.VarChar).Value = String.Join(",", _listaIdCliente.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                    If Not String.IsNullOrEmpty(_nombre) Then .Add("@nombre", SqlDbType.VarChar, 2000).Value = _nombre
                    If _estado IsNot Nothing Then .Add("@estado", SqlDbType.Bit).Value = _estado
                End With

                .ejecutarReader("ObtenerInfoClienteCem", CommandType.StoredProcedure)
                If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                    Dim objClienteCEM As ClienteCem
                    While .Reader.Read
                        objClienteCEM = New ClienteCem()
                        objClienteCEM.CargarResultadoConsulta(.Reader)
                        Me.InnerList.Add(objClienteCEM)
                    End While
                    _cargado = True
                End If
            End With
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Sub

#End Region

    End Class

End Namespace