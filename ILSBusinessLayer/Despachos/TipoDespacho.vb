Namespace Despachos
    Public Class TipoDespacho

#Region "Atributos"
        Private _idTipoDespacho As Integer
        Private _nombre As String
        Private _estado As Boolean
        Private _idEntidad As Integer
        Private _url As String
#End Region

#Region "Propiedades"
        Public ReadOnly Property IdTipoDespacho() As Integer
            Get
                Return _idTipoDespacho
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

        Public Property idEntidad() As Integer
            Get
                Return _idEntidad
            End Get
            Set(ByVal value As Integer)
                _idEntidad = value
            End Set
        End Property

        Public Property URL() As String
            Get
                Return _url
            End Get
            Set(ByVal value As String)
                _url = value
            End Set
        End Property

#End Region

#Region "Constructores"
        Public Sub New()
            _idTipoDespacho = 0
            _nombre = ""
            URL = ""
        End Sub

        Public Sub New(ByVal idTipoDespacho As Integer)
            Me.New()
            Me.SeleccionarPorID(idTipoDespacho)
        End Sub

#End Region

#Region "Metodos Amigos"
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="activos"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function ListadoTipos(Optional ByVal activos As Boolean = True) As DataTable
            Dim resultado As New DataTable
            Dim adminBD As New LMDataAccessLayer.LMDataAccess

            If activos Then
                adminBD.agregarParametroSQL("@estado", 1, SqlDbType.Int)
            End If

            Try
                resultado = adminBD.ejecutarDataTable("SeleccionarTiposDespacho", CommandType.StoredProcedure)
            Catch ex As Exception
                Throw New Exception("Error al tratar de cargar listado de tipos de despacho: " & ex.Message)
            Finally
                adminBD.Dispose()
            End Try

            Return resultado
        End Function

        Private Overloads Shared Function ObtenerListado(ByVal filtro As Estructuras.FiltroTipoDespacho) As DataTable
            Dim adminDB As New LMDataAccessLayer.LMDataAccess
            Dim dtDatos As New DataTable
            Try
                With adminDB
                    .agregarParametroSQL("@idEntidad", filtro.idEntidad, SqlDbType.Int)
                    If filtro.estado <> Enumerados.EstadoBinario.NoEstablecido Then .agregarParametroSQL("@estado", filtro.estado, SqlDbType.Bit)
                    dtDatos = .ejecutarDataTable("SeleccionarTipoDespacho", CommandType.StoredProcedure)
                End With

            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
            Return dtDatos
        End Function

        Public Overloads Shared Function ListadoTipoArticulos(Optional ByVal activos As Boolean = True) As DataTable
            Dim resultado As New DataTable
            Dim adminBD As New LMDataAccessLayer.LMDataAccess

            If activos Then
                adminBD.agregarParametroSQL("@estado", 1, SqlDbType.Int)
            End If

            Try
                resultado = adminBD.ejecutarDataTable("SeleccionarTipoArticulos", CommandType.StoredProcedure)
            Catch ex As Exception
                Throw New Exception("Error al tratar de cargar listado de tipos de despacho: " & ex.Message)
            Finally
                adminBD.Dispose()
            End Try

            Return resultado
        End Function


        Public Shared Function ObtenerPorIdEntidad(ByVal idEntidad As Integer) As DataTable
            Dim dtDatos As New DataTable
            Dim filtro As New Estructuras.FiltroTipoDespacho
            Try
                filtro.idEntidad = idEntidad
                filtro.estado = Enumerados.EstadoBinario.Activo
                dtDatos = ObtenerListado(filtro)
            Catch ex As Exception
                Throw New Exception("Error al tratar de cargar listado de tipos de despacho: " & ex.Message)
            End Try
            Return dtDatos
        End Function

#End Region

#Region "Métodos Privados"
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idTipoDespacho"></param>
        ''' <remarks></remarks>
        Private Sub SeleccionarPorID(ByVal idTipoDespacho As Integer)
            Dim adminBD As New LMDataAccessLayer.LMDataAccess

            adminBD.agregarParametroSQL("@identificador", idTipoDespacho, SqlDbType.Int)
            Try
                adminBD.ejecutarReader("SeleccionarTipoDespacho", CommandType.StoredProcedure)
                While adminBD.Reader.Read()

                    Me._idTipoDespacho = adminBD.Reader("idTipoDespacho")
                    Me._nombre = adminBD.Reader("nombre")
                    Me._estado = adminBD.Reader("estado")
                    Me._idEntidad = adminBD.Reader("idEntidad")
                    Me.URL = adminBD.Reader("url")

                End While
            Catch ex As Exception
                Throw New Exception("Imposible obtener tipo de despacho con ID especificado")
            Finally
                adminBD.Dispose()
            End Try
        End Sub

        
#End Region

    End Class
End Namespace
