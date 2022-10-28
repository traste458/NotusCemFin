Imports LMDataAccessLayer

Namespace serviceClaroSamsung


    Public Class InfoMotorizado
#Region "Atributos (Propiedades)"

        Public Property Id As Decimal
        Private dbManager As New LMDataAccess

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region
#Region "Métodos Públicos"
        Public Function ConsultarMotorizado() As DataTable
            Dim dtDatos As New DataTable
            If dbManager IsNot Nothing Then dbManager = New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Clear()
                        If Id > 0 Then .Add("@Id", SqlDbType.Decimal).Value = Id
                    End With
                    dtDatos = .EjecutarDataTable("TraerInfoMotorizado", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtDatos
        End Function
#End Region


    End Class
End Namespace