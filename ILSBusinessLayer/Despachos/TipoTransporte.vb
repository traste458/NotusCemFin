Namespace Despachos
    Public Class TipoTransporte

#Region "Atributos"
        Private _idTipoTransporte As Integer
        Private _nombre As String
        Private _estado As Boolean
#End Region

#Region "Propiedades"
        Public ReadOnly Property IdTipoTransporte() As Integer
            Get
                Return _idTipoTransporte
            End Get
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

#Region "Constructores"
        Public Sub New()
            _idTipoTransporte = 0
            _nombre = ""
        End Sub

        Public Sub New(ByVal idTipoTransporte As Integer)
            Me.New()
            Me.SeleccionarPorID(idTipoTransporte)
        End Sub
#End Region

#Region "Metodos Amigos"
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="activos"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ListadoTipos(Optional ByVal activos As Boolean = True) As DataTable
            Dim resultado As New DataTable
            Dim adminBD As New LMDataAccessLayer.LMDataAccess

            If activos Then
                adminBD.agregarParametroSQL("@estado", 1, SqlDbType.Int)
            End If

            Try
                resultado = adminBD.ejecutarDataTable("SeleccionarTiposTransporte", CommandType.StoredProcedure)
            Catch ex As Exception
                Throw New Exception("Error al tratar de cargar listado de tipos de Transporte: " & ex.Message)
            Finally
                adminBD.Dispose()
            End Try

            Return resultado
        End Function
#End Region

#Region "Métodos Privados"
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idTipoTransporte"></param>
        ''' <remarks></remarks>
        Private Sub SeleccionarPorID(ByVal idTipoTransporte As Integer)
            Dim adminBD As New LMDataAccessLayer.LMDataAccess

            adminBD.agregarParametroSQL("@idTipo", idTipoTransporte, SqlDbType.Int)
            Try
                adminBD.ejecutarReader("SeleccionarTiposTransporte", CommandType.StoredProcedure)
                While adminBD.Reader.Read()

                    Me._idTipoTransporte = adminBD.Reader("idTipo")
                    Me._nombre = adminBD.Reader("nombre")
                    Me._estado = adminBD.Reader("estado")

                End While
            Catch ex As Exception
                Throw New Exception("Imposible obtener tipo de transporte con ID especificado")
            Finally
                adminBD.Dispose()
            End Try
        End Sub
#End Region

    End Class
End Namespace
