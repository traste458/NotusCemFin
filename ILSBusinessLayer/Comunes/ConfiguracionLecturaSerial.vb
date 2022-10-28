Imports LMDataAccessLayer

Namespace Comunes

    Public Class ConfiguracionLecturaSerial

#Region "Atributos"

        Private _idConfiguracion As Integer
        Private _idTipoProducto As Integer
        Private _tipoProducto As String
        Private _idTecnologia As Integer
        Private _tecnologia As String
        Private _caracterPermitido As String
        Private _longitud As String

#End Region

#Region "Propiedades"

        Public Property IdConfiguracion As Integer
            Get
                Return _idConfiguracion
            End Get
            Set(value As Integer)
                _idConfiguracion = value
            End Set
        End Property

        Public Property IdTipoProducto As Integer
            Get
                Return _idTipoProducto
            End Get
            Set(value As Integer)
                _idTipoProducto = value
            End Set
        End Property

        Public Property TipoProducto As String
            Get
                Return _tipoProducto
            End Get
            Set(value As String)
                _tipoProducto = value
            End Set
        End Property

        Public Property IdTecnologia As Integer
            Get
                Return _idTecnologia
            End Get
            Set(value As Integer)
                _idTecnologia = value
            End Set
        End Property

        Public Property Tecnologia As String
            Get
                Return _tecnologia
            End Get
            Set(value As String)
                _tecnologia = value
            End Set
        End Property

        Public Property CaracterPermitido As String
            Get
                Return _caracterPermitido
            End Get
            Set(value As String)
                _caracterPermitido = value
            End Set
        End Property

        Public Property Longitud As String
            Get
                Return _longitud
            End Get
            Set(value As String)
                _longitud = value
            End Set
        End Property

#End Region

#Region "Cosntructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idConfiguracion As Integer)
            MyBase.New()
            _idConfiguracion = idConfiguracion
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        If _idConfiguracion > 0 Then .SqlParametros.Add("@idConfiguracion", SqlDbType.SmallInt).Value = _idConfiguracion
                        .ejecutarReader("ObtenerInfoConfiguracionLecturaSerial", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                AsignarValoresConsulta(.Reader)
                            End If
                            .Reader.Close()
                        End If
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
        End Sub

#End Region

#Region "Métodos Compartidos"

        Protected Friend Sub AsignarValoresConsulta(reader As Common.DbDataReader)
            If reader IsNot Nothing Then
                Integer.TryParse(reader("idConfiguracion"), _idConfiguracion)
                Integer.TryParse(reader("idTipoProducto"), _idTipoProducto)
                _tipoProducto = reader("tipoProducto")
                Integer.TryParse(reader("idTecnologia"), _idTecnologia)
                _tecnologia = reader("tecnologia")
                _caracterPermitido = reader("caracterPermitido")
                _longitud = reader("longitudPermitida")
            End If
        End Sub

#End Region

    End Class

End Namespace
