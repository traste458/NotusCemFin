
Namespace Entities
    Public Class TrazabilidadRequest
        Public Property Consulta As String
        Public Property NumeroReferencia As String
        Public Property TipoReferencia As Integer
        Public Property IdServicio As Integer
        Public Property Fecha As String
        Public Property idDoc As Integer
    End Class
    Public Class TrazabilidadResponse
        Public Property Valor As Integer
        Public Property Mensaje As String
        Public Property Data As New TrazabilidadData


        Public Class TrazabilidadData
            Public Property dtDatos As DataTable
            Public Property dsDatos As DataSet
            Public Property base64File As String
            Public Sub New()
                dtDatos = New DataTable
                dsDatos = New DataSet
            End Sub
        End Class

    End Class


End Namespace

