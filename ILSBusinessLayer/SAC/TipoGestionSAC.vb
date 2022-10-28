﻿Imports LMDataAccessLayer

Namespace SAC

    Public Class TipoGestionSAC

#Region "Atributos"

        Private _idTipo As Short
        Private _idUnidadNegocio As Byte
        Private _descripcion As String
        Private _activo As Boolean
        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public ReadOnly Property IdTipo() As Short
            Get
                Return _idTipo
            End Get
        End Property

        Public Property IdUnidadNegocio() As Byte
            Get
                Return _idUnidadNegocio
            End Get
            Set(ByVal value As Byte)
                _idUnidadNegocio = value
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

        Public Property Activo() As Boolean
            Get
                Return _activo
            End Get
            Set(ByVal value As Boolean)
                _activo = value
            End Set
        End Property

        Public ReadOnly Property Registrado() As Boolean
            Get
                Return _registrado
            End Get
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _descripcion = ""
        End Sub

        Public Sub New(ByVal identificador As Short)
            MyBase.New()
            CargarDatos(identificador)
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos(ByVal identificador As Short)
            Dim dbManager As New LMDataAccess
            Dim idPerfil As Integer
            Try
                If System.Web.HttpContext.Current.Session("usxp009") IsNot Nothing Then _
                    Integer.TryParse(System.Web.HttpContext.Current.Session("usxp009").ToString(), idPerfil)
                Dim usuarioUnidad As New UsuarioPerfilUnidadNegocio(idPerfil)
                With dbManager
                    .SqlParametros.Add("@idUnidadNegocio", SqlDbType.TinyInt).Value = usuarioUnidad.IdUnidadNegocio
                    .SqlParametros.Add("@listaIdTipo", SqlDbType.VarChar, 2000).Value = identificador.ToString
                    .ejecutarReader("ConsultarTipoDeGestion", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Short.TryParse(.Reader("idTipoGestion").ToString, _idTipo)
                            Byte.TryParse(.Reader("idUnidadNegocio").ToString(), _idUnidadNegocio)
                            _descripcion = .Reader("descripcion").ToString
                            Boolean.TryParse(.Reader("activo").ToString, _activo)
                            _registrado = True
                        End If
                        .Reader.Close()
                    End If

                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub EstablecerIdentificador(ByVal identificador As Short)
            _idTipo = identificador
        End Sub

        Protected Friend Sub MarcarComoRegistrado()
            _registrado = True
        End Sub

#End Region

#Region "Métodos Públicos"

        Private Sub Registrar()

        End Sub

        Private Sub Actualizar()

        End Sub

#End Region

    End Class

End Namespace
