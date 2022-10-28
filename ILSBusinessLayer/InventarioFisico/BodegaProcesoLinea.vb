Imports LMDataAccessLayer

Namespace InventarioFisico
    Public Class BodegaProcesoLinea

#Region "Atributos"

        Protected _registrado As Boolean
        Protected _bodega As String

#End Region

#Region "Constructores"

        Public Sub New()
        End Sub

        Public Sub New(linea As Integer)
            Me._Linea = linea
            CargarInformacion()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdBodega As Integer

        Public ReadOnly Property Bodega As String
            Get
                Return _bodega
            End Get
        End Property

        Public Property Linea As Integer

        Public ReadOnly Property Registrado As Boolean
            Get
                Return _registrado
            End Get
        End Property

#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            Using dbManager As New LMDataAccess
                With dbManager
                    If Me._Linea > 0 Then .SqlParametros.Add("@linea", SqlDbType.Int).Value = Me._Linea
                    .ejecutarReader("ObtenerInformacionBodegaProcesoLinea", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idBodega").ToString, Me._IdBodega)
                            Me._bodega = .Reader("bodega").ToString
                            Integer.TryParse(.Reader("linea").ToString, Me._Linea)
                            Me._registrado = True
                        End If
                    End If

                End With
            End Using
        End Sub

#End Region

    End Class
End Namespace