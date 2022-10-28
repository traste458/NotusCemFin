Imports System.Reflection
Imports LMDataAccessLayer

Namespace Comunes

    Public Class ConfiguracionLecturaSerialColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idConfiguracion As Integer
        Private _listaProducto As List(Of Integer)
        Private _listaTipoProducto As List(Of Integer)

        Private _cargado As Boolean

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As ConfiguracionLecturaSerial
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As ConfiguracionLecturaSerial)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property IdConfiguracion() As Integer
            Get
                Return _idConfiguracion
            End Get
            Set(ByVal value As Integer)
                _idConfiguracion = value
            End Set
        End Property

        Public Property ListaProducto As List(Of Integer)
            Get
                Return _listaProducto
            End Get
            Set(value As List(Of Integer))
                _listaProducto = value
            End Set
        End Property

        Public Property ListaTipoProducto As List(Of Integer)
            Get
                If _listaTipoProducto Is Nothing Then _listaTipoProducto = New List(Of Integer)
                Return _listaTipoProducto
            End Get
            Set(value As List(Of Integer))
                _listaTipoProducto = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idConfiguracion As Integer)
            Me.New()
            _idConfiguracion = idConfiguracion
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim miEstado As Type = GetType(ConfiguracionLecturaSerial)
            Dim pInfo As PropertyInfo

            For Each pInfo In miEstado.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As ConfiguracionLecturaSerial)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As ConfiguracionLecturaSerial)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As ConfiguracionLecturaSerial)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As ConfiguracionLecturaSerial)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function IndiceDe(ByVal idConfiguracion As Integer) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), ConfiguracionLecturaSerial)
                    If .IdConfiguracion = idConfiguracion Then
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
            Dim miEstado As ConfiguracionLecturaSerial

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miEstado = CType(Me.InnerList(index), ConfiguracionLecturaSerial)
                If miEstado IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(ConfiguracionLecturaSerial).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miEstado, Nothing)
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
                    Me.InnerList.Clear()
                    With dbManager
                        If _idConfiguracion > 0 Then .SqlParametros.Add("@idConfiguracion", SqlDbType.Int).Value = Me._idConfiguracion
                        If _listaProducto IsNot Nothing AndAlso _listaProducto.Count > 0 Then _
                            .SqlParametros.Add("@listaProducto", SqlDbType.VarChar).Value = String.Join(",", _listaProducto.ConvertAll(Of String)(Function(x) x.ToString).ToArray)
                        .ejecutarReader("ObtenerInfoConfiguracionLecturaSerial", CommandType.StoredProcedure)

                        If .Reader IsNot Nothing Then
                            Dim oConfiguracion As ConfiguracionLecturaSerial
                            While .Reader.Read
                                oConfiguracion = New ConfiguracionLecturaSerial
                                oConfiguracion.AsignarValoresConsulta(.Reader)
                                
                                Me.InnerList.Add(oConfiguracion)
                            End While
                            .Reader.Close()
                        End If
                    End With
                    _cargado = True
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
        End Sub

        Public Function ObtenerExpresionRegular() As String
            Dim strExpresion As String = String.Empty
            Using dbManager As New LMDataAccess
                With dbManager
                    If _listaProducto IsNot Nothing AndAlso _listaProducto.Count > 0 Then _
                            .SqlParametros.Add("@listaProducto", SqlDbType.VarChar).Value = String.Join(",", _listaProducto.ConvertAll(Of String)(Function(x) x.ToString).ToArray)
                    If _listaTipoProducto IsNot Nothing AndAlso _listaTipoProducto.Count > 0 Then _
                            .SqlParametros.Add("@listaTipoProducto", SqlDbType.VarChar).Value = String.Join(",", _listaTipoProducto.ConvertAll(Of String)(Function(x) x.ToString).ToArray)
                    strExpresion = .EjecutarScalar("ObtenerExpresionEvaluarLecturaSerial", CommandType.StoredProcedure).ToString
                End With
            End Using
            Return strExpresion
        End Function

#End Region

#Region "Métodos Compartidos"

        Public Shared Function ObtenerTodosEnDataTable() As DataTable
            Dim dtAux As New DataTable
            Dim dbManager As New LMDataAccess

            Try
                dtAux = dbManager.ejecutarDataTable("ObtenerEstadosGenericos", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtAux
        End Function

#End Region

    End Class

End Namespace
