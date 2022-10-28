Imports GemBox.Spreadsheet
Imports System.IO
Imports System.Web
Imports System.Text

Namespace Recibos
    Public Class DetalleSerialOrdenRecepcion
#Region "variables"
        Private _idOrdenRecepcion As Long
       
#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
           
        End Sub

#End Region

#Region "propiedades"

        Public Property IdOrdenRecepcion() As Long
            Get
                Return _idOrdenRecepcion
            End Get
            Set(ByVal value As Long)
                _idOrdenRecepcion = value
            End Set
        End Property

       
#End Region
#Region "metodos publicos"

       

        Public Function ObtenerSerialOrdenRecepcion(ByVal idOrdenRecepcion As Integer) As DataTable
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim dtDatosSerial As DataTable
            Try
                With dbManager
                    .agregarParametroSQL("@idOrdenRecepcion", idOrdenRecepcion, SqlDbType.Int)
                    .TiempoEsperaComando = 300
                End With
                dtDatosSerial = dbManager.ejecutarDataTable("ObtenerSerialOrdenRecepcion", CommandType.StoredProcedure)
            Catch ex As Exception
                Throw New Exception("Error al tratar de obtener datos. " & ex.Message)
            Finally
                dbManager.Dispose()
            End Try

            Return dtDatosSerial
        End Function

        

#End Region

#Region "Métodos Privados"
       
#End Region
    End Class
End Namespace
