Imports LMDataAccessLayer

Namespace Recibos
    Public Class Incoterm

#Region "variables"
        Private _idIncoterm As Integer
        Private _termino As String
        Private _descripcion As String
        Private _estado As Boolean
#End Region

#Region "propiedades"
        Public ReadOnly Property IdIncoterm() As Integer
            Get
                Return _idIncoterm
            End Get
        End Property
        Public Property Termino() As String
            Get
                Return _termino
            End Get
            Set(ByVal value As String)
                _termino = value
            End Set
        End Property
        Public Property Descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
        End Property
        Public Property Estado() As Boolean
            Get
                Return _estado
            End Get
            Set(ByVal value As Boolean)
                _estado = value
            End Set
        End Property
#End Region

#Region "constructores"

#End Region

#Region "metodos"
        Public Sub Crear()

        End Sub
        Public Overloads Shared Function Obtener() As DataTable
            Dim db As New LMDataAccess
            Dim dt As New DataTable
            Try
                dt = db.ejecutarDataTable("ObtenerIncoterm", CommandType.StoredProcedure)
            Catch ex As Exception
                Throw New Exception("Error al tratar de obtener los datos " & ex.Message)
            End Try
            Return dt
        End Function
        Public Overloads Shared Function Obtener(ByVal estado As Integer) As DataTable
            Dim db As New LMDataAccess
            Dim dt As New DataTable
            Try
                db.agregarParametroSQL("@estado", estado, SqlDbType.Int)
                dt = db.ejecutarDataTable("ObtenerIncoterm", CommandType.StoredProcedure)
            Catch ex As Exception
                Throw New Exception("Error al tratar de obtener los datos " & ex.Message)
            End Try
            Return dt
        End Function
        Public Sub Eliminar()

        End Sub
#End Region

    End Class
End Namespace

