Imports LMDataAccessLayer

Namespace Comunes
    Public Class UsuarioAuditorInventario

#Region "Atributos"

        Private _idUsuario As Integer
        Private _nombre As String
        Private _identificacion As String
        Private _cargo As String
        Private _clave As String
        Private _registrado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()

        End Sub

        Public Sub New(ByVal identificador As Integer)
            _idUsuario = identificador
            CargarInformacion()

        End Sub
#End Region

#Region "Propiedades"

        Public Property IdUsuario As Integer
            Get
                Return _idUsuario
            End Get
            Set(ByVal value As Integer)
                _idUsuario = value
            End Set
        End Property

        Public Property Nombre As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        Public Property Identificacion As String
            Get
                Return _identificacion
            End Get
            Set(ByVal value As String)
                _identificacion = value
            End Set
        End Property

        Public Property Cargo As String
            Get
                Return _cargo
            End Get
            Set(ByVal value As String)
                _cargo = value
            End Set
        End Property

        Public Property Clave As String
            Get
                Return _clave
            End Get
            Set(ByVal value As String)
                _clave = value
            End Set
        End Property

        Public Property Registrado As Boolean
            Get
                Return _registrado
            End Get
            Protected Friend Set(value As Boolean)
                _registrado = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            Using dbManager As New LMDataAccess
                With dbManager
                    If _idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me._idUsuario
                    If Not EsNuloOVacio(_clave) Then .SqlParametros.Add("@clave", SqlDbType.VarChar, 50).Value = Me._clave.Trim
                    .ejecutarReader("ObtenerUsuarioAuditorInventario", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then CargarResultadoConsulta(.Reader)
                        .Reader.Close()
                    End If
                End With
            End Using
        End Sub

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing AndAlso reader.HasRows Then
                Integer.TryParse(reader("idusuario").ToString, _idUsuario)
                _nombre = reader("nombre").ToString
                _identificacion = reader("identificacion").ToString
                _cargo = reader("cargo").ToString
                _clave = reader("clave").ToString
                _registrado = True
            End If

        End Sub
#End Region

    End Class
End Namespace