Imports LMDataAccessLayer

Namespace Recibos
    Public Class ModuloTipoProducto
#Region "variables"
        Private _idModulo As Long
        Private _idTipoProducto As Integer
        Private _url As String
        Private _modulo As Modulo
        Private _tipoProducto As Productos.TipoProducto
#End Region

#Region "propiedades"
        Public Property IdModulo() As Long
            Get
                Return _idModulo
            End Get
            Set(ByVal value As Long)
                _idModulo = value
            End Set
        End Property
        Public Property IdTipoProducto() As Integer
            Get
                Return _idTipoProducto
            End Get
            Set(ByVal value As Integer)
                _idTipoProducto = value
            End Set
        End Property
        Public Property Url() As String
            Get
                Return _url
            End Get
            Set(ByVal value As String)
                _url = value
            End Set
        End Property
        Public Property Modulo() As Modulo
            Get
                Return _modulo
            End Get
            Set(ByVal value As Modulo)
                _modulo = value
            End Set
        End Property
        Public Property TipoProducto() As Productos.TipoProducto
            Get
                Return _tipoProducto
            End Get
            Set(ByVal value As Productos.TipoProducto)
                _tipoProducto = value
            End Set
        End Property
#End Region

#Region "constructores"
        Public Sub New(ByVal idModulo As Long, ByVal idTipoProducto As Integer)
            _idModulo = idModulo
            _idTipoProducto = idTipoProducto
            _modulo = New Modulo(idModulo)
            _tipoProducto = New Productos.TipoProducto(idTipoProducto)
        End Sub
#End Region

#Region "metodos"
        Public Sub Crear()

        End Sub

        Public Shared Function Obtener(ByVal idModulo As Long, ByVal idTipoProducto As Integer) As ArrayList
            Dim db As New LMDataAccess
            Dim ar As ArrayList
            Dim dt As New DataTable            

            Try
                db.agregarParametroSQL("@idModulo", idModulo, SqlDbType.Int)
                db.agregarParametroSQL("@idTipoProducto", idTipoProducto, SqlDbType.Int)
                dt = db.ejecutarDataTable("ObtenerModuloTipoProducto", CommandType.StoredProcedure)
                If dt.Rows.Count > 0 Then                    
                    ar = New ArrayList(dt.Rows(0).ItemArray)
                Else
                    ar = New ArrayList
                End If

            Catch ex As Exception
                Throw New Exception("Error al tratar de cargar el tipo de producto " & ex.Message)
            End Try
            Return ar
        End Function

#End Region

    End Class
End Namespace

