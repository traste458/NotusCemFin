Imports LMDataAccessLayer
Imports System.Reflection
Imports ILSBusinessLayer.Enumerados

Namespace SAC

    Public Class OrigenRespuestaGestionCasoSACColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idOrigenRespuesta As Byte
        Private _idUnidadNegocio As Byte
        Private _descripcion As String
        Private _requiereArchivo As EstadoBinario
        Private _activo As EstadoBinario

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _descripcion = ""
            _requiereArchivo = EstadoBinario.NoEstablecido
            _activo = EstadoBinario.NoEstablecido
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As OrigenRespuestaGestionCasoSAC
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As OrigenRespuestaGestionCasoSAC)
                If value IsNot Nothing AndAlso value.Registrado Then
                    Me.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o sin datos a la colección.")
                End If
            End Set
        End Property

        Public Property IdOrigenRespuesta() As Byte
            Get
                Return _idOrigenRespuesta
            End Get
            Set(ByVal value As Byte)
                _idOrigenRespuesta = value
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

        Public Property Descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
        End Property

        Public Property RequiereArchivo() As EstadoBinario
            Get
                Return _requiereArchivo
            End Get
            Set(ByVal value As EstadoBinario)
                _requiereArchivo = value
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

#Region "Métodos Públicos"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim tipo As Type = GetType(OrigenRespuestaGestionCasoSAC)
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As OrigenRespuestaGestionCasoSAC)
            If valor.Registrado Then
                Me.InnerList.Insert(posicion, valor)
            Else
                Throw New Exception("No se puede adicionar un objeto nulo o sin datos a la colección.")
            End If
        End Sub

        Public Sub Adicionar(ByVal valor As OrigenRespuestaGestionCasoSAC)
            If valor.Registrado Then
                Me.InnerList.Add(valor)
            Else
                Throw New Exception("No se puede adicionar un objeto nulo o sin datos a la colección.")
            End If
        End Sub

        Public Sub Remover(ByVal valor As OrigenRespuestaGestionCasoSAC)
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
            Dim origen As OrigenRespuestaGestionCasoSAC

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                origen = CType(Me.InnerList(index), OrigenRespuestaGestionCasoSAC)
                If origen IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(OrigenRespuestaGestionCasoSAC).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(origen, Nothing)
                        End If
                    Next
                    dtAux.Rows.Add(drAux)
                End If
            Next

            Return dtAux
        End Function

        Public Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Dim idPerfil As Integer
            Try
                If System.Web.HttpContext.Current.Session("usxp009") IsNot Nothing Then _
                    Integer.TryParse(System.Web.HttpContext.Current.Session("usxp009").ToString(), idPerfil)
                Dim usuarioUnidad As New UsuarioPerfilUnidadNegocio(idPerfil)
                _idUnidadNegocio = usuarioUnidad.IdUnidadNegocio
                Me.Clear()
                With dbManager
                    If Me._idOrigenRespuesta > 0 Then _
                        .SqlParametros.Add("@idOrigen", SqlDbType.TinyInt).Value = Me._idOrigenRespuesta
                    If Me._idUnidadNegocio > 0 Then _
                        .SqlParametros.Add("@idUnidadNegocio", SqlDbType.TinyInt).Value = Me._idUnidadNegocio
                    If Me._descripcion IsNot Nothing AndAlso Me._descripcion.Trim.Length > 0 Then _
                        .SqlParametros.Add("@descripcion", SqlDbType.VarChar, 20).Value = Me._descripcion
                    If Me._requiereArchivo <> EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@requiereArchivo", SqlDbType.Bit).Value = IIf(Me._requiereArchivo = EstadoBinario.Activo, 1, 0)
                    If Me._activo <> EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@activo", SqlDbType.Bit).Value = IIf(Me._activo = EstadoBinario.Activo, 1, 0)
                    .ejecutarReader("ConsultarOrigenRespuestaGestionCasoSAC", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim origen As OrigenRespuestaGestionCasoSAC
                        While .Reader.Read
                            origen = New OrigenRespuestaGestionCasoSAC
                            Byte.TryParse(.Reader("idOrigenRespuesta").ToString, origen.IdOrigenRespuesta)
                            origen.Descripcion = .Reader("descripcion").ToString
                            Boolean.TryParse(.Reader("requiereArchivo").ToString, origen.RequiereArchivo)
                            Boolean.TryParse(.Reader("activo").ToString, origen.Activo)
                            origen.Registrado = True
                            Me.InnerList.Add(origen)
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
                dtAux = dbManager.EjecutarDataTable("ConsultarOrigenRespuestaGestionCasoSAC", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtAux
        End Function

#End Region

    End Class

End Namespace

