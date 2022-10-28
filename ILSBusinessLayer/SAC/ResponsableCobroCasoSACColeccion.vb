Imports LMDataAccessLayer
Imports System.Reflection
Imports ILSBusinessLayer.Enumerados

Namespace SAC

    Public Class ResponsableCobroCasoSACColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idResponsable As Short
        Private _idUnidadNegocio As Byte
        Private _nombre As String
        Private _activo As EstadoBinario

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _nombre = ""
            _activo = EstadoBinario.NoEstablecido
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As ResponsableCobroCasoSAC
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As ResponsableCobroCasoSAC)
                If value IsNot Nothing AndAlso value.Registrado Then
                    Me.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o sin datos a la colección.")
                End If
            End Set
        End Property

        Public Property IdResponsable() As Integer
            Get
                Return _idResponsable
            End Get
            Set(ByVal value As Integer)
                _idResponsable = value
            End Set
        End Property

        Public Property IdUnidadNegocio() As Byte
            Get
                Return _idUnidadNegocio
            End Get
            Set(ByVal value As Byte)
                _idUnidadNegocio = value
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

        Public Property Activo() As EstadoBinario
            Get
                Return _activo
            End Get
            Set(ByVal value As EstadoBinario)
                _activo = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim tipo As Type = GetType(ResponsableCobroCasoSAC)
            Dim pInfo As PropertyInfo

            For Each pInfo In tipo.GetProperties
                If pInfo.PropertyType.Namespace = "System" Then
                    With dtAux
                        .Columns.Add(pInfo.Name, pInfo.PropertyType)
                    End With
                End If
            Next

            Return dtAux
        End Function

#End Region

#Region "Métodos Públicos"

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As ResponsableCobroCasoSAC)
            If valor.Registrado Then
                Me.InnerList.Insert(posicion, valor)
            Else
                Throw New Exception("No se puede adicionar un objeto nulo o sin datos a la colección.")
            End If
        End Sub

        Public Sub Adicionar(ByVal valor As ResponsableCobroCasoSAC)
            If valor.Registrado Then
                Me.InnerList.Add(valor)
            Else
                Throw New Exception("No se puede adicionar un objeto nulo o sin datos a la colección.")
            End If
        End Sub

        Public Sub Remover(ByVal valor As ResponsableCobroCasoSAC)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function GenerarDataTable() As DataTable
            Dim dtAux As DataTable = CrearEstructuraDeTabla()
            Dim drAux As DataRow
            Dim respuesta As ResponsableCobroCasoSAC

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                respuesta = CType(Me.InnerList(index), ResponsableCobroCasoSAC)
                If respuesta IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(ResponsableCobroCasoSAC).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(respuesta, Nothing)
                        End If
                    Next
                    dtAux.Rows.Add(drAux)
                End If
            Next

            Return dtAux
        End Function

        Public Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try

                Dim idPerfil As Integer
                If System.Web.HttpContext.Current.Session("usxp009") IsNot Nothing Then _
                    Integer.TryParse(System.Web.HttpContext.Current.Session("usxp009").ToString(), idPerfil)
                Dim usuarioUnidad As New UsuarioPerfilUnidadNegocio(idPerfil)

                Me.Clear()

                With dbManager
                    Me._idUnidadNegocio = usuarioUnidad.IdUnidadNegocio
                    If Me._idResponsable > 0 Then .SqlParametros.Add("@idResponsable", SqlDbType.SmallInt).Value = Me._idResponsable
                    If Me._idUnidadNegocio > 0 Then .SqlParametros.Add("@idUnidadNegocio", SqlDbType.TinyInt).Value = Me._idUnidadNegocio
                    If Me._nombre IsNot Nothing AndAlso Me._nombre.Trim.Length > 0 Then _
                        .SqlParametros.Add("@nombre", SqlDbType.VarChar, 70).Value = Me._nombre
                    If Me._activo <> EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@activo", SqlDbType.Bit).Value = IIf(Me._activo = EstadoBinario.Activo, 1, 0)
                    .ejecutarReader("ConsultarResponsableCobroCasoSAC", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim elResponsable As ResponsableCobroCasoSAC
                        Dim idResponsable As Short
                        While .Reader.Read
                            elResponsable = New ResponsableCobroCasoSAC
                            Short.TryParse(.Reader("idResponsable").ToString, idResponsable)
                            Byte.TryParse(.Reader("idUnidadNegocio").ToString(), elResponsable.IdUnidadNegocio)
                            elResponsable.EstablecerIdentificador(idResponsable)
                            elResponsable.Nombre = .Reader("nombre").ToString
                            Boolean.TryParse(.Reader("activo").ToString, elResponsable.Activo)
                            elResponsable.MarcarComoRegistrado()
                            Me.InnerList.Add(elResponsable)
                        End While
                        .Reader.Close()
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

#End Region

#Region "Métodos Compartidos"

        Public Shared Function ObtenerTodosEnDataTable() As DataTable
            Dim dtAux As New DataTable
            Dim dbManager As New LMDataAccess

            Dim idPerfil As Integer

            Try
                If System.Web.HttpContext.Current.Session("usxp009") IsNot Nothing Then _
                    Integer.TryParse(System.Web.HttpContext.Current.Session("usxp009").ToString(), idPerfil)
                Dim usuarioUnidad As New UsuarioPerfilUnidadNegocio(idPerfil)
                dbManager.SqlParametros.Add("@idUnidadNegocio", SqlDbType.TinyInt).Value = usuarioUnidad.IdUnidadNegocio
                dtAux = dbManager.EjecutarDataTable("ConsultarResponsableCobroCasoSAC", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtAux
        End Function

#End Region

    End Class

End Namespace
