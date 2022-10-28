Imports LMDataAccessLayer

Namespace Comunes

    Public Class ArchivoAyudaColeccion
        Inherits CollectionBase

#Region "Filtros de Búsqueda"

        Public Property IdArchivo As Integer
        Public Property IdPerfil As Integer

        Private Property Cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.new()
        End Sub

#End Region

#Region "Métodos Públicos"

        Default Public Property Item(ByVal index As Integer) As ArchivoAyuda
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As ArchivoAyuda)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As ArchivoAyuda)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As ArchivoAyuda)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As ArchivoAyuda)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub CargarDatos()
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        If Me.IdArchivo > 0 Then .SqlParametros.Add("@idArchivo", SqlDbType.Int).Value = Me.IdArchivo
                        If Me.IdPerfil > 0 Then .SqlParametros.Add("@idPerfil", SqlDbType.Int).Value = Me.IdPerfil
                        .ejecutarReader("ObtenerArchivoAyuda", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            Dim objArchivoAyuda As ArchivoAyuda
                            While .Reader.Read
                                objArchivoAyuda = New ArchivoAyuda()
                                objArchivoAyuda.CargarResultadoConsulta(.Reader)
                                Me.InnerList.Add(objArchivoAyuda)
                            End While
                            Me.Cargado = True
                        End If
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
        End Sub

#End Region

    End Class

End Namespace
