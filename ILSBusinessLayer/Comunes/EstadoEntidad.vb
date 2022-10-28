Imports LMDataAccessLayer

Public Class EstadoEntidad

#Region "Atributos"
    Private _idEstado As Short
    Private _nombre As String
    Private _idEntidad As Short
    Private _entidad As String
#End Region

#Region "Propiedades"

    Public ReadOnly Property IdEstado() As Short
        Get
            Return _idEstado
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

    Public Property IdEntidad() As Short
        Get
            Return _idEntidad
        End Get
        Set(ByVal value As Short)
            _idEntidad = value
        End Set
    End Property

    Public ReadOnly Property Entidad()
        Get
            Return _entidad
        End Get
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _nombre = ""
        _entidad = ""
    End Sub

    Public Sub New(ByVal idEstado As Short, Optional ByVal idEntidad As Short = 0)
        Me.New()
        Me.CargarDatos(idEstado, idEntidad)
    End Sub

#End Region

#Region "Métodos privados"

    Private Sub CargarDatos(ByVal idEstado As Short, Optional ByVal idEntidad As Short = 0)
        Dim dbManager As New LMDataAccessLayer.LMDataAccess

        Try
            With dbManager
                If Me._idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.SmallInt).Value = Me._idEstado
                If Me._idEntidad > 0 Then .SqlParametros.Add("@idEntidad", SqlDbType.SmallInt).Value = Me._idEntidad
                .ejecutarReader("ConsultarEstadoEntidad", CommandType.StoredProcedure)
                If .Reader IsNot Nothing AndAlso .Reader.Read Then
                    Short.TryParse(.Reader("idEstado").ToString, _idEstado)
                    _nombre = .Reader("nombre").ToString
                    Short.TryParse(.Reader("idEntidad").ToString, _idEntidad)
                    _entidad = .Reader("entidad").ToString
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub EstablecerIdentificador(ByVal valor As Short)
        _idEstado = valor
    End Sub

    Protected Friend Sub EstablecerEntidad(ByVal valor As String)
        _entidad = valor
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso

        Return resultado
    End Function

    Public Function Actualizar() As ResultadoProceso
        Dim resultado As New ResultadoProceso

        Return resultado
    End Function

#End Region

End Class
