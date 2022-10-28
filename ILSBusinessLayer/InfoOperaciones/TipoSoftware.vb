Imports LMDataAccessLayer
Imports ILSBusinessLayer
Imports ILSBusinessLayer.Enumerados
Imports System.IO
Imports System.Web
Public Class TipoSoftware

#Region "Atributos (Campos)"

#End Region

#Region "Propiedades"

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function ObtenerDatos() As DataTable
        Dim dtDatos As New DataTable
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                .SqlParametros.Clear()
                    dtDatos = .ejecutarDataTable("ConsultaTipoSoftware", CommandType.StoredProcedure)
                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return dtDatos
    End Function

#End Region

#Region "Métodos Compartidos"

#End Region

End Class
