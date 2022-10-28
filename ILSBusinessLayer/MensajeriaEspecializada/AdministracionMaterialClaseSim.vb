Imports LMDataAccessLayer
Public Class AdministracionMaterialClaseSim

#Region "Filtros de Búsqueda"

    Private _material As String
    Private _referencia As String
    Private _TipoMaterial As Integer
    Private _idUsuario As Integer
    Private _idClase As Integer
#End Region


#Region "Propiedades"

    Public Property Material As String
        Get
            Return _material
        End Get
        Set(value As String)
            _material = value
        End Set
    End Property

    Public Property Referencia As String
        Get
            Return _referencia
        End Get
        Set(value As String)
            _referencia = value
        End Set
    End Property
    Public Property TipoMaterial As Integer
        Get
            Return _TipoMaterial
        End Get
        Set(value As Integer)
            _TipoMaterial = value
        End Set
    End Property
    Public Property IdUsuario As Integer
        Get
            Return _idUsuario
        End Get
        Set(value As Integer)
            _idUsuario = value
        End Set
    End Property

    Public Property IdClase As Integer
        Get
            Return _idClase
        End Get
        Set(value As Integer)
            _idClase = value
        End Set
    End Property
#End Region
#Region "Métodos Públicos"

    Public Function ConsultarClaseSimMaterial() As DataTable
        Dim dt As New DataTable
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .Add("@Material", SqlDbType.VarChar).Value = _material.ToString()
                End With
                dt = .ejecutarDataTable("ConsultarMaterialClaseSim", CommandType.StoredProcedure)
                Return dt
            Catch ex As Exception
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
    End Function
    Public Function CargarMaterialesComboTipomaterial(Material As String, startIndex As Integer, endIndex As Integer) As DataTable
        Dim dbManager As New LMDataAccess
        Dim Dtmateriales As New DataTable
        With dbManager
            If Not String.IsNullOrEmpty(Material) Then .SqlParametros.Add("@material", SqlDbType.VarChar).Value = String.Format("%{0}%", Material)
            If (startIndex > 0) Then .SqlParametros.Add("@startIndex", SqlDbType.Int).Value = startIndex
            If (endIndex > 0) Then .SqlParametros.Add("@endIndex", SqlDbType.Int).Value = endIndex
            .TiempoEsperaComando = 0
            Dtmateriales = .ejecutarDataTable("ObtenerMaterialTipoMatrial", CommandType.StoredProcedure)
        End With
        If dbManager IsNot Nothing Then dbManager.Dispose()
        Return Dtmateriales
    End Function
    Public Function ObtieneClasesSIM() As DataTable
        Dim dtDatos As DataTable
        Using dbManager As New LMDataAccess
            dtDatos = dbManager.ejecutarDataTable("ObtieneClasesSIM", CommandType.StoredProcedure)
        End Using
        Return dtDatos
    End Function
    Public Function RegistrarClasesSIM() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        With dbManager
            Try

                .SqlParametros.Add("@Material", SqlDbType.VarChar).Value = _material
                .SqlParametros.Add("@idClase", SqlDbType.Int).Value = _idClase
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                .ejecutarReader("AsignarMaterialClaseSim", CommandType.StoredProcedure)
                If CInt(.SqlParametros("@resultado").Value) = 0 Then
                    resultado.EstablecerMensajeYValor(0, "La asignacion de Clase Sim Card correctamente ")
                Else
                    resultado.EstablecerMensajeYValor(1, "No se realizó la asignacion Clase Sim Card  por favor verificar.")
                End If
            Catch ex As Exception
                resultado.EstablecerMensajeYValor(1, "Se genero un error al realizar la Asignacion favor verificar." & ex.Message)
                Throw New Exception(ex.Message, ex)
            End Try
        End With
        dbManager.Dispose()
        Return resultado
    End Function
#End Region
End Class
