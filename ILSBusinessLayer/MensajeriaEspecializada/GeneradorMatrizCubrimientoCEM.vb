Imports LMDataAccessLayer
Namespace CEMService

    Public Class GeneradorMatrizCubrimientoCEM

#Region "Atributos"

        Private _matriz As List(Of CiudadCubrimientoCEM)
        Private _cargado As Boolean
#End Region

#Region "Constructores"

        Public Sub New()

        End Sub

#End Region

#Region "Propiedades"

        Public ReadOnly Property Matriz As List(Of CiudadCubrimientoCEM)
            Get
                If _matriz Is Nothing OrElse Not _cargado Then CargarDatos()
                Return _matriz
            End Get
        End Property

#End Region

#Region "Métodos Públicos"
        'TODO: Implementar este método que obtiene la información de la matriz de cubrimiento
        Public Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Dim resultado As New ResultadoProceso
            Try
                dbManager.TiempoEsperaComando = 0
                _matriz = PoblarLista(dbManager.ejecutarReader("ObtenerMatrizCubrimiento", CommandType.StoredProcedure))
                _cargado = True
            Catch ex As Exception
                resultado.Valor = "1"
                resultado.Mensaje = "Error consultando matriz de ciudades : " & ex.Message
                _cargado = False
            End Try
            Return
        End Sub

        Public Function PoblarLista(reader As SqlClient.SqlDataReader) As List(Of CiudadCubrimientoCEM)
            Dim lst As New List(Of CiudadCubrimientoCEM)
            If reader.HasRows Then
                Do While reader.Read
                    Dim entidad As New CiudadCubrimientoCEM()
                    entidad.CodigoCiudad = reader.Item("idCiudad")
                    entidad.NombreCiudad = reader.Item("nombre")
                    entidad.Departamento = reader.Item("departamento")
                    lst.Add(entidad)
                Loop
            End If
            reader.Close()
            Return lst
        End Function
#End Region

    End Class

End Namespace