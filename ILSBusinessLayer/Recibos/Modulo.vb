Namespace Recibos
    Public Class Modulo
#Region "variables"
        Private _idModulo As Long
        Private _nombre As String
        Private _estado As Boolean
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
        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
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
        Public Sub New(ByVal idModulo As Long)

        End Sub
#End Region

#Region "metodos"
        Public Sub Crear()

        End Sub
        Public Sub Obtener()

        End Sub
        Public Sub Eliminar()

        End Sub
#End Region

    End Class
End Namespace

