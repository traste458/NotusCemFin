Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports ILSBusinessLayer.MensajeriaEspecializada

Namespace WMS

    Public Class BodegaColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idBodega As Short
        Private _idUnidadNegocio As Short
        Private _idCampania As Integer
        Private _esAdministrable As Nullable(Of Boolean)
        Private _cargado As Boolean

        'Filtros externos
        Private _idSite As Integer

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As Bodega
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(value As Bodega)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property IdBodega As Short
            Get
                Return _idBodega
            End Get
            Set(value As Short)
                _idBodega = value
            End Set
        End Property

        Public Property IdUnidadNegocio As Short
            Get
                Return _idUnidadNegocio
            End Get
            Set(value As Short)
                _idUnidadNegocio = value
            End Set
        End Property

        Public Property IdSite As Integer
            Get
                Return _idSite
            End Get
            Set(value As Integer)
                _idSite = value
            End Set
        End Property

        Public Property IdCampania As Integer
            Get
                Return _idCampania
            End Get
            Set(value As Integer)
                _idCampania = value
            End Set
        End Property

        Public Property EsAdministrable As Nullable(Of Boolean)
            Get
                Return _esAdministrable
            End Get
            Set(value As Nullable(Of Boolean))
                _esAdministrable = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idUnidadNegocio As Short)
            MyBase.New()
            _idUnidadNegocio = idUnidadNegocio
            CargarDatos()
        End Sub

        Public Sub New(ByVal idSite As Integer)
            MyBase.New()
            _idSite = idSite
            CargarDatos()
        End Sub

        Public Sub New(ByVal idCampania As Long)
            MyBase.New()
            _idCampania = idCampania
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim miObj As Type = GetType(Bodega)
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As Bodega)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As Bodega)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As BodegaColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As Bodega)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function IndiceDe(ByVal idBodega As Short) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), Bodega)
                    If .IdBodega = idBodega Then
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
            Try
                Dim drAux As DataRow
                Dim miDetalle As Bodega

                For index As Integer = 0 To Me.InnerList.Count - 1
                    drAux = dtAux.NewRow
                    miDetalle = CType(Me.InnerList(index), Bodega)
                    If miDetalle IsNot Nothing Then
                        For Each pInfo As PropertyInfo In GetType(Bodega).GetProperties
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

        Private Sub CargarDatos()
            Using dbManager As New LMDataAccess
                With dbManager
                    Try
                        .SqlParametros.Clear()

                        If _idBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = _idBodega
                        If _idUnidadNegocio > 0 Then .SqlParametros.Add("@idUnidadNegocio", SqlDbType.Int).Value = _idUnidadNegocio
                        If _idSite > 0 Then .SqlParametros.Add("@idSite", SqlDbType.Int).Value = _idSite
                        If _idCampania > 0 Then .SqlParametros.Add("@idCampania", SqlDbType.Int).Value = _idCampania
                        If _esAdministrable IsNot Nothing Then .SqlParametros.Add("@esAdministrable", SqlDbType.Bit).Value = _esAdministrable

                        .ejecutarReader("ObtenerInfoBodega", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            Dim elDetalle As Bodega

                            While .Reader.Read
                                If .Reader.HasRows Then
                                    elDetalle = New Bodega
                                    Integer.TryParse(.Reader("idBodega"), elDetalle.IdBodega)
                                    elDetalle.Nombre = .Reader("nombre")
                                    elDetalle.Codigo = .Reader("codigo")
                                    If Not IsDBNull(.Reader("direccion")) Then elDetalle.Direccion = .Reader("direccion")
                                    If Not IsDBNull(.Reader("telefono")) Then elDetalle.Telefono = .Reader("telefono")
                                    If Not IsDBNull(.Reader("idCiudad")) Then elDetalle.IdCiudad = .Reader("idCiudad")
                                    If Not IsDBNull(.Reader("ciudad")) Then elDetalle.Ciudad = .Reader("ciudad")
                                    elDetalle.AceptaProductoEnReconocimiento = .Reader("aceptaProdSinReconocimiento")
                                    elDetalle.Estado = .Reader("estado")

                                    _cargado = True
                                    Me.InnerList.Add(elDetalle)
                                End If
                            End While
                            If Not .Reader.IsClosed Then .Reader.Close()
                        End If
                    Catch ex As Exception
                        Throw ex
                    End Try
                End With
            End Using
        End Sub

#End Region

    End Class

End Namespace

