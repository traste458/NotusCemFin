Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Enumerados
Imports ILSBusinessLayer.Comunes
Imports LMDataAccessLayer
Imports System.Net.Mail
Imports System.Text
Imports System.IO

Namespace Productos

    Public Class Producto

#Region "Campos"

        Private _idProducto As Integer
        Private _nombre As String
        Private _codigo As String
        Private _idTecnologia As Integer
        Private _tecnologia As String
        Private _idFabricante As Integer
        Private _fabricante As String
        Private _estado As Boolean
        Private _idTipoProducto As Short
        Private _tipoProducto As String
        Private _idTipoUnidad As Short
        Private _unidadEmpaque As String
        Private _infoProveedor As DataTable
        Private _esSim As Boolean
        Private _aplicaTecnologia As Boolean
        Private _esSerializado As Boolean
        Private _idClasificacionInterna As Short
        Private _idClasificacionExterna As Short
        Private _clasificacionInterna As String
        Private _clasificacionExterna As String
        Private _tieneImagen As Boolean
        Private _listImagenes As List(Of ImagenProducto)
		Private _requiereConsecutivo As Boolean
        Private _registrado As Boolean
        Private _codigoHomologacion As String
        Private _descripProducto As String
        Private _claseProducto As String
        Private _strDestinoPP As String = String.Empty
        Private _strDestinoCC As String = String.Empty
        Private _productoPrincipal As String
#End Region

#Region "Propiedades"

        Public Property IdProducto() As Integer
            Get
                Return _idProducto
            End Get
            Set(ByVal value As Integer)
                _idProducto = value
            End Set
        End Property

        Public Property CodigoHomologacion() As String
            Get
                Return _codigoHomologacion
            End Get
            Set(ByVal value As String)
                _codigoHomologacion = value
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

        Public Property Codigo() As String
            Get
                Return _codigo
            End Get
            Set(ByVal value As String)
                _codigo = value
            End Set
        End Property

        Public Property IdTecnologia() As Integer
            Get
                Return _idTecnologia
            End Get
            Set(ByVal value As Integer)
                _idTecnologia = value
            End Set
        End Property

        Public Property Tecnologia() As String
            Get
                Return _tecnologia
            End Get
            Protected Friend Set(ByVal value As String)
                _tecnologia = value
            End Set
        End Property

        Public Property IdFabricante() As Integer
            Get
                Return _idFabricante
            End Get
            Set(ByVal value As Integer)
                _idFabricante = value
            End Set
        End Property

        Public Property Fabricante() As String
            Get
                Return _fabricante
            End Get
            Protected Friend Set(ByVal value As String)
                _fabricante = value
            End Set
        End Property

        Public Property Activo() As Boolean
            Get
                Return _estado
            End Get
            Set(ByVal value As Boolean)
                _estado = value
            End Set
        End Property

        Public Property IdTipoProducto() As Short
            Get
                Return _idTipoProducto
            End Get
            Set(ByVal value As Short)
                _idTipoProducto = value
            End Set
        End Property

        Public Property TipoProducto()
            Get
                Return _tipoProducto
            End Get
            Protected Friend Set(ByVal value)
                _tipoProducto = value
            End Set
        End Property

        Public ReadOnly Property InfoProveedor() As DataTable
            Get
                If _infoProveedor Is Nothing Then CargarListadoProveedor()
                Return _infoProveedor
            End Get
        End Property

        Public Property TieneImagen As Boolean
            Get
                Return _tieneImagen
            End Get
            Protected Friend Set(value As Boolean)
                _tieneImagen = value
            End Set
        End Property

        Public Property ListaImagenes As List(Of ImagenProducto)
            Get
                If IsNothing(_listImagenes) And _idProducto > 0 Then CargarImagenes()
                Return _listImagenes
            End Get
            Set(value As List(Of ImagenProducto))
                _listImagenes = value
            End Set
        End Property

        Public Property IdTipoUnidad() As Short
            Get
                Return _idTipoUnidad
            End Get
            Set(ByVal value As Short)
                _idTipoUnidad = value
            End Set
        End Property

        Public Property UnidadEmpaque() As String
            Get
                Return _unidadEmpaque
            End Get
            Protected Friend Set(ByVal value As String)
                _unidadEmpaque = value
            End Set
        End Property

        Public Property EsSim() As Boolean
            Get
                Return _esSim
            End Get
            Protected Friend Set(ByVal value As Boolean)
                _esSim = value
            End Set
        End Property

        Public Property AplicaTecnologia() As Boolean
            Get
                Return _aplicaTecnologia
            End Get
            Protected Friend Set(ByVal value As Boolean)
                _aplicaTecnologia = value
            End Set
        End Property

        Public Property EsSerializado() As Boolean
            Get
                Return _esSerializado
            End Get
            Protected Friend Set(ByVal value As Boolean)
                _esSerializado = value
            End Set
        End Property

        Public Property IdClasificacionInterna() As Short
            Get
                Return _idClasificacionInterna
            End Get
            Set(ByVal value As Short)
                _idClasificacionInterna = value
            End Set
        End Property

        Public Property ClasificacionInterna() As String
            Get
                Return _clasificacionInterna
            End Get
            Set(ByVal value As String)
                _clasificacionInterna = value
            End Set
        End Property

        Public Property IdClasificacionExterna() As Short
            Get
                Return _idClasificacionExterna
            End Get
            Set(ByVal value As Short)
                _idClasificacionExterna = value
            End Set
        End Property

        Public Property ClasificacionExterna() As String
            Get
                Return _clasificacionExterna
            End Get
            Protected Friend Set(ByVal value As String)
                _clasificacionExterna = value
            End Set
        End Property
        Public Property RequiereConsecutivo As Boolean
            Get

                Return _requiereConsecutivo
            End Get
            Set(value As Boolean)

                _requiereConsecutivo = value
            End Set
        End Property

        Public Property Registrado() As Boolean
            Get
                Return _registrado
            End Get
            Protected Friend Set(ByVal value As Boolean)
                _registrado = value
            End Set
        End Property

        Public Property DescripProducto() As String
            Get

                Return _descripProducto
            End Get
            Set(value As String)

                _descripProducto = value
            End Set
        End Property

        Public Property ClaseProducto() As String
            Get
                Return _claseProducto
            End Get
            Set(value As String)
                _claseProducto = value
            End Set
        End Property

        Public Property ProductoPrincipal() As String
            Get
                Return _productoPrincipal
            End Get
            Set(value As String)
                _productoPrincipal = value
            End Set
        End Property
#End Region

#Region "Contructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            _idProducto = identificador
            CargarInformacion()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            If _idProducto <> 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = _idProducto
                        .ejecutarReader("ObtenerInfoProducto", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                _nombre = .Reader("nombre").ToString
                                _codigo = .Reader("codigo").ToString
                                _codigoHomologacion = .Reader("codigoHomologacion").ToString
                                Integer.TryParse(.Reader("idTecnologia").ToString, _idTecnologia)
                                _tecnologia = .Reader("tecnologia").ToString
                                Short.TryParse(.Reader("idFabricante").ToString, _idFabricante)
                                _fabricante = .Reader("fabricante").ToString
                                _estado = CBool(.Reader("estado").ToString)
                                Short.TryParse(.Reader("idTipoProducto").ToString, _idTipoProducto)
                                _tipoProducto = .Reader("tipoProducto").ToString
                                Integer.TryParse(.Reader("idTipoUnidad").ToString, _idTipoUnidad)
                                _unidadEmpaque = .Reader("unidadEmpaque").ToString
                                _esSim = CBool(.Reader("esSim").ToString)
                                _aplicaTecnologia = CBool(.Reader("aplicaTecnologia").ToString)
                                _esSerializado = CBool(.Reader("esSerializado").ToString)
                                Integer.TryParse(.Reader("idClasificacionInterna").ToString, _idClasificacionInterna)
                                Integer.TryParse(.Reader("idClasificacionExterna").ToString, _idClasificacionExterna)
                                _clasificacionInterna = .Reader("clasificacionInterna").ToString
                                _clasificacionExterna = .Reader("clasificacionExterna").ToString
                                _tieneImagen = CBool(.Reader("tieneImagen"))
                                _requiereConsecutivo = CBool(.Reader("requiereConsecutivo").ToString)
                                _productoPrincipal = .Reader("productoPrincipal").ToString
                                _registrado = True
                            End If
                            .Reader.Close()
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
        End Sub

        Private Sub CargarListadoProveedor()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = _idProducto
                    _infoProveedor = .ejecutarDataTable("ObtenerProvedorDeProducto", CommandType.StoredProcedure)
                End With
                If _infoProveedor.PrimaryKey.Count = 0 Then
                    Dim pkColumn(0) As DataColumn
                    pkColumn(0) = _infoProveedor.Columns("idProveedor")
                    _infoProveedor.PrimaryKey = pkColumn
                End If
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Private Sub CargarImagenes()
            Using dbManager As New LMDataAccess
                _listImagenes = New List(Of ImagenProducto)
                Try
                    With dbManager
                        .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = _idProducto
                        .ejecutarReader("ObtenerImagenDeProducto", CommandType.StoredProcedure)

                        If .Reader IsNot Nothing Then
                            While .Reader.Read
                                Dim objImg As ImagenProducto
                                objImg.imagen = .Reader("imagen")
                                objImg.contenType = .Reader("contentType")
                                objImg.nombreImagen = .Reader("nombreImagen")
                                objImg.tamanio = .Reader("tamanio")

                                _listImagenes.Add(objImg)
                            End While
                        End If
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
        End Sub

        Private Function ObtenerProveedoresAdicionados() As DataTable
            Dim dtAux As DataTable = _infoProveedor.Clone
            For Each drAux As DataRow In _infoProveedor.Rows
                If drAux.RowState = DataRowState.Added Then dtAux.ImportRow(drAux)
            Next
            Return dtAux
        End Function

        Private Function ObtenerProveedoresEliminados() As DataTable
            Dim dtAux As DataTable = _infoProveedor.Clone
            For Each drAux As DataRow In _infoProveedor.Rows
                If drAux.RowState = DataRowState.Deleted Then dtAux.ImportRow(drAux)
            Next
            dtAux.RejectChanges()
            Return dtAux
        End Function

        Private Sub RegistrarProveedores(ByVal dtProveedor As DataTable, ByVal dbManager As LMDataAccess)
            If dbManager IsNot Nothing Then
                If Not dtProveedor.Columns.Contains("idProducto") Then
                    Dim dcAux As New DataColumn("idProducto")
                    dcAux.DefaultValue = _idProducto
                    dtProveedor.Columns.Add(dcAux)
                End If
                With dbManager
                    .inicilizarBulkCopy()
                    With .BulkCopy
                        .DestinationTableName = "ProductoProveedor"
                        .ColumnMappings.Add("idProducto", "idProducto")
                        .ColumnMappings.Add("idProveedor", "idProveedor")
                        .WriteToServer(dtProveedor)
                    End With
                End With
            End If
        End Sub

        Private Function RegistrarImagenes(ByVal dbManager As LMDataAccess) As Short
            Dim respuesta As Short
            If dbManager IsNot Nothing Then
                For Each img As ImagenProducto In _listImagenes
                    With dbManager
                        .SqlParametros.Clear()
                        .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = _idProducto
                        .SqlParametros.Add("@imagen", SqlDbType.VarBinary).Value = img.imagen
                        .SqlParametros.Add("@contentType", SqlDbType.VarChar).Value = img.contenType
                        .SqlParametros.Add("@nombreImagen", SqlDbType.VarChar).Value = img.nombreImagen
                        .SqlParametros.Add("@tamanio", SqlDbType.Int).Value = img.tamanio
                        .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                        .ejecutarNonQuery("RegistrarImagenProducto", CommandType.StoredProcedure)
                        respuesta = CShort(.SqlParametros("@returnValue").Value)
                    End With
                Next
            End If
            Return respuesta
        End Function

        Private Function RegistrarSubProductos(dbManager As LMDataAccess) As Short
            Dim respuesta As Short
            If dbManager IsNot Nothing Then
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = _idProducto
                    .SqlParametros.Add("@nombreReferencia", SqlDbType.VarChar).Value = _nombre
                    .SqlParametros.Add("@nombreSegunCliente", SqlDbType.VarChar).Value = _nombre
                    .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .SqlParametros.Add("@materialGenerado", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output

                    .ejecutarNonQuery("RegistrarSubproductoPOPPAP", CommandType.StoredProcedure)
                    respuesta = CShort(.SqlParametros("@returnValue").Value)
                    If respuesta = 0 Then _codigo = .SqlParametros("@materialGenerado").Value
                End With
            End If
            Return respuesta
        End Function

        Private Sub EliminarProveedores(ByVal dtProveedor As DataTable, ByVal dbManager As LMDataAccess)
            Dim arrAux As New ArrayList
            For Each drAux As DataRow In dtProveedor.Rows
                arrAux.Add(drAux("idProveedor"))
            Next
            With dbManager
                .SqlParametros.Clear()
                .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = _idProducto
                .SqlParametros.Add("@listaProveedores", SqlDbType.VarChar).Value = Join(arrAux.ToArray, ",")
                .ejecutarNonQuery("EliminarProveedorDeProducto", CommandType.StoredProcedure)
            End With
        End Sub

        Private Function GenerarEstructuraInfoProveedor() As DataTable
            Dim dtAux As New DataTable
            dtAux.Columns.Add("idProveedor", GetType(Integer))
            Dim pkColumn(0) As DataColumn
            pkColumn(0) = dtAux.Columns("idProveedor")
            dtAux.PrimaryKey = pkColumn
            Return dtAux
        End Function

        Public Shared Function ObtenerListadoCompletoDeImagenes() As DataTable
            Dim dt As New DataTable
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        dt = .ejecutarDataTable("ObtenerImagenDeProducto", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return dt
        End Function
#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As Short
            Dim resultado As Short = 0
            If _nombre.Trim.Length > 0 And _idFabricante > 0 And _idTipoProducto > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@nombre", SqlDbType.VarChar, 70).Value = _nombre.Trim
                            If _codigo.Trim.Length > 0 Then .Add("@codigo", SqlDbType.VarChar, 10).Value = _codigo.Trim.ToUpper
                            .Add("@idTecnologia", SqlDbType.Int).Value = _idTecnologia
                            .Add("@idFabricante", SqlDbType.Int).Value = _idFabricante
                            .Add("@idTipoProducto", SqlDbType.Int).Value = _idTipoProducto
                            .Add("@codigoHomologacion", SqlDbType.NVarChar).Value = _codigoHomologacion
                            .Add("@requiereConsecutivo", SqlDbType.Bit).Value = _requiereConsecutivo
                            If _idTipoUnidad <> 0 Then .Add("@idTipoUnidad", SqlDbType.Int).Value = _idTipoUnidad
                            .Add("@idProducto", SqlDbType.Int).Direction = ParameterDirection.Output
                            If _idClasificacionInterna > 0 Then .Add("@idClasificacionInterna", SqlDbType.SmallInt).Value = _idClasificacionInterna
                            If _idClasificacionExterna > 0 Then .Add("@idClasificacionExterna", SqlDbType.SmallInt).Value = _idClasificacionExterna
                            If Not EsNuloOVacio(_codigoHomologacion) Then .Add("@codigoHomologacion", SqlDbType.VarChar, 30).Value = _codigoHomologacion.Trim.ToUpper
                            .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        End With

                        .iniciarTransaccion()
                        .ejecutarNonQuery("CrearProducto", CommandType.StoredProcedure)
                        resultado = CShort(.SqlParametros("@returnValue").Value)

                        If resultado = 0 Then
                            _idProducto = CInt(.SqlParametros("@idProducto").Value)

                            If _infoProveedor Is Nothing Then CargarListadoProveedor()
                            If _infoProveedor.Rows.Count > 0 Then
                                Using dtAux As DataTable = _infoProveedor.Copy
                                    RegistrarProveedores(dtAux, dbManager)
                                End Using
                            End If

                            If _idTipoProducto = Enumerados.TipoProductoMaterial.MATERIA_POP_PUBLICIDAD _
                                Or _idTipoProducto = Enumerados.TipoProductoMaterial.PAPELERIA Then
                                resultado = RegistrarSubProductos(dbManager)
                            End If                      
							
							'Se registran las imágenes del producto
                            If _listImagenes IsNot Nothing AndAlso _listImagenes.Count > 0 And resultado = 0 Then
                                resultado = RegistrarImagenes(dbManager)
                            End If

                            If resultado = 0 Then
                                .confirmarTransaccion()
                            Else
                                .abortarTransaccion()
                            End If
                        Else
                            .abortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 3
            End If
            Return resultado
        End Function

        Public Function Actualizar() As Short
            Dim resultado As Short = 0
            If _idProducto <> 0 And _nombre.Trim.Length > 0 And _idFabricante <> 0 And _idTipoProducto <> 0 Then

                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@idProducto", SqlDbType.Int).Value = _idProducto
                            .Add("@nombre", SqlDbType.VarChar, 70).Value = _nombre
                            .Add("@idFabricante", SqlDbType.SmallInt).Value = _idFabricante
                            .Add("@idTecnologia", SqlDbType.Int).Value = _idTecnologia
                            .Add("@idTipoProducto", SqlDbType.SmallInt).Value = _idTipoProducto
                            If _idTipoUnidad <> 0 Then .Add("@idTipoUnidad", SqlDbType.SmallInt).IsNullable = True
                            .Add("@requiereConsecutivo", SqlDbType.Bit).Value = _requiereConsecutivo
                            .Item("@idTipoUnidad").Value = IIf(_idTipoUnidad <> 0, _idTipoUnidad, DBNull.Value)
                            If _idClasificacionInterna > 0 Then .Add("@idClasificacionInterna", SqlDbType.SmallInt).Value = _idClasificacionInterna
                            If _idClasificacionExterna > 0 Then .Add("@idClasificacionExterna", SqlDbType.SmallInt).Value = _idClasificacionExterna
                            If Not EsNuloOVacio(_codigoHomologacion) Then .Add("@codigoHomologacion", SqlDbType.VarChar, 30).Value = _codigoHomologacion.Trim.ToUpper
                            .Add("@estado", SqlDbType.Bit).Value = _estado
                            .Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        End With
                        .iniciarTransaccion()
                        .ejecutarNonQuery("ActualizarProducto", CommandType.StoredProcedure)
                        Short.TryParse(.SqlParametros("@returnValue").Value.ToString, resultado)
                        If resultado = 0 Then
                            'Se verifica si se adicionaron nuevos proveedores y de ser así se registran en la BD
                            Using dtAux As DataTable = ObtenerProveedoresAdicionados()
                                If dtAux.Rows.Count > 0 Then RegistrarProveedores(dtAux, dbManager)
                            End Using

                            'Se verifica si se eliminaron proveedores
                            Using dtAux As DataTable = ObtenerProveedoresEliminados()
                                If dtAux.Rows.Count > 0 Then EliminarProveedores(dtAux, dbManager)
                            End Using

                            'Se registran las imágenes del producto
                            If _listImagenes IsNot Nothing AndAlso _listImagenes.Count > 0 And resultado = 0 Then
                                resultado = RegistrarImagenes(dbManager)
                            End If
                        End If
                        If resultado = 0 Then .confirmarTransaccion()
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
                _infoProveedor.AcceptChanges()
            Else
                resultado = 3
            End If
            Return resultado
        End Function

        Public Function AdicionarProveedor(ByVal idProveedor As Integer) As Boolean
            Dim resultado As Boolean = False
            If _infoProveedor Is Nothing Then _infoProveedor = GenerarEstructuraInfoProveedor()
            If _infoProveedor.Rows.Find(idProveedor) Is Nothing Then
                Dim drAux As DataRow = _infoProveedor.NewRow
                drAux("idProveedor") = idProveedor
                _infoProveedor.Rows.Add(drAux)
            End If
            Return resultado
        End Function

        Public Function RemoverProveedor(ByVal idProveedor As Integer) As Short
            Dim resultado As Boolean = False
            If _infoProveedor IsNot Nothing Then
                Dim drAux As DataRow = _infoProveedor.Rows.Find(idProveedor)
                If drAux IsNot Nothing Then drAux.Delete()
            End If
            Return resultado
        End Function

        Public Sub AjustarInfoProveedor(ByVal dtProveedor As DataTable)
            Dim drProveedor As DataRow
            Dim idProveedor As Integer
            If _infoProveedor Is Nothing Then CargarListadoProveedor()
            'Se adicionan los nuevos Proveedores seleccionados
            For Each drAux As DataRow In dtProveedor.Rows
                idProveedor = CInt(drAux("idProveedor"))
                drProveedor = _infoProveedor.Rows.Find(idProveedor)
                If drProveedor Is Nothing Then Me.AdicionarProveedor(idProveedor)
            Next

            'Se elimina los Proveedores desasigandos
            For Each drAux As DataRow In _infoProveedor.Rows
                idProveedor = CInt(drAux("idProveedor"))
                drProveedor = dtProveedor.Rows.Find(idProveedor)
                If drProveedor Is Nothing Then Me.RemoverProveedor(idProveedor)
            Next
        End Sub

		Public Function NotificarProducto() As ResultadoProceso
            Dim respuestaEnvio As New ResultadoProceso
            Dim ctd As Integer = 0
            Try
                Dim Notificacion As New AdministradorCorreo
                Dim DestinosPP As New MailAddressCollection
                Dim DestinosCC As New MailAddressCollection
                Dim sbContenido As New StringBuilder
                With sbContenido
                    .Append("Se ha creado el producto:  <B>" & _claseProducto & "</B> con descripción  <B>" & _descripProducto.Trim.ToUpper & "</B>, código  <B>" & _codigo & "</B> y tipo de clasificacion  <B>" & _clasificacionInterna & "</B>")
                    .Append("<br/>el cual debe ser configurado con el nombre de Claro.")
                    .Append("<br/><br/>Por favor ingrese al sistema para configurar la información.&nbsp&nbsp&nbsp<B>http://www.logytechmobile.com/notusar/login.asp</B>")
                End With
                With Notificacion
                    CargarDestinatarios(AsuntoNotificacion.Tipo.NotificaciónNuevosProductos, DestinosPP, DestinosCC)
                    If _strDestinoPP.Trim <> "" Then
                        .Titulo = "Creación nuevo producto sin nombre claro"
                        .Asunto = "Notificación Creación nuevo producto sin nombre claro"
                        .TextoMensaje = sbContenido.ToString
                        .FirmaMensaje = "Logytech Mobile S.A.S <br />"
                        .Receptor = DestinosPP
                        .Copia = DestinosCC
                        If Not .EnviarMail() Then
                            respuestaEnvio.Valor = 1
                            respuestaEnvio.Mensaje = "Ocurrió un error inesperado y no fué posible enviar la notificación"
                        Else
                            respuestaEnvio.Valor = 0
                            respuestaEnvio.Mensaje = "Notificación enviada exitosamente"
                        End If
                    Else
                        respuestaEnvio.Valor = 2
                        respuestaEnvio.Mensaje = "No Existen destinatarios para enviar la notificación de creacion de nuevos productos"
                    End If
                End With
            Finally
            End Try
            Return respuestaEnvio
        End Function
		
        Public Function NotificarCreacionDeProducto() As ResultadoProceso
            Dim respuestaEnvio As New ResultadoProceso
			Dim ctd As Integer = 0
            Try
                Dim notificador As New AdministradorCorreo
                Dim strContenido As String
                Dim infoTipo As New TipoProducto(Me._idTipoProducto)
                strContenido = "Se ha creado el producto: <b>" & _nombre & "</b> con código: <b>" & _codigo & "</b> de tipo: <b>" & infoTipo.Descripcion & "</b>" & _
                    "<br/>al cual se le debe establecer el nombre manejado por el cliente externo." & _
                    "<br/><br/>Por favor ingrese al sistema para configurar la información.&nbsp&nbsp&nbsp<b>http://www.logytechmobile.com/notusar/login.asp</b>"
                With notificador
                    CargarDestinatarios(AsuntoNotificacion.Tipo.NotificaciónNuevosProductos, .Receptor, .Copia)
                    If (.Receptor IsNot Nothing AndAlso .Receptor.Count > 0) OrElse (.Copia IsNot Nothing AndAlso .Copia.Count > 0) Then
                        .Titulo = "Creación Nuevo Producto Sin Nombre Según Cliente Externo"
                        .Asunto = "Notificación Creación Nuevo Producto Sin Nombre Según Cliente Externo"
                        .TextoMensaje = strContenido
                        .FirmaMensaje = "Logytech Mobile S.A.S <br />"
                        If Not .EnviarMail() Then
                            respuestaEnvio.EstablecerMensajeYValor(1, "Ocurrió un error inesperado y no fué posible enviar la notificación")
                        Else
                            respuestaEnvio.EstablecerMensajeYValor(0, "Notificación Enviada Exitosamente")
                        End If
                        'Else
                        'respuestaEnvio.EstablecerMensajeYValor(2, "No Existen destinatarios para enviar la notificación de creacion de nuevos productos")
                    End If
                End With
            Catch ex As Exception
                respuestaEnvio.EstablecerMensajeYValor(1, "Ocurrió un error inesperado y no fué posible enviar la notificación de creación del producto")
            End Try
            Return respuestaEnvio
        End Function

        Private Sub CargarDestinatarios(ByVal tipo As Comunes.AsuntoNotificacion.Tipo, ByVal destinoPP As MailAddressCollection, ByVal destinoCC As MailAddressCollection)
            Dim configuracionUsuario As New UsuarioNotificacion
            Dim filtro As New FiltroUsuarioNotificacion
            Dim dtDestinos As DataTable

            With filtro
                .IdAsuntoNotificacion = tipo
                .Separador = ";"
            End With

            dtDestinos = UsuarioNotificacion.ObtenerDestinatarioNotificacion(filtro)
            If dtDestinos IsNot Nothing AndAlso dtDestinos.Rows.Count > 0 Then
                If destinoPP Is Nothing Then destinoPP = New MailAddressCollection
                If destinoCC Is Nothing Then destinoCC = New MailAddressCollection

                For Each fila As DataRow In dtDestinos.Rows
                    destinoPP.Add(fila.Item("destinoPara").ToString)
                    destinoCC.Add(fila.Item("destinoPara").ToString)
                Next
            End If
        End Sub
        Public Function ObtenerColoresProducto() As DataTable
            Dim dtResultado As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Clear()
                    If _idProducto > 0 Then .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = _idProducto
                    dtResultado = .ejecutarDataTable("ObtenerColoresProductos", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtResultado
        End Function
#End Region

#Region "Métodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroProducto
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroProducto) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDatos As DataTable
            Try
                With dbManager
                    With .SqlParametros
                        If filtro.IdProducto > 0 Then .Add("@idProducto", SqlDbType.Int).Value = filtro.IdProducto
                        If filtro.Nombre IsNot Nothing AndAlso filtro.Nombre.Trim.Length > 0 Then _
                            .Add("@nombre", SqlDbType.VarChar, 50).Value = filtro.Nombre
                        If filtro.Codigo IsNot Nothing AndAlso filtro.Codigo.Trim.Length > 0 Then _
                            .Add("@codigo", SqlDbType.VarChar, 10).Value = filtro.Codigo
                        If filtro.IdTecnologia > 0 Then .Add("@idTecnologia", SqlDbType.Int).Value = filtro.IdTecnologia
                        If filtro.IdFabricante > 0 Then .Add("@idFabricante", SqlDbType.SmallInt).Value = filtro.IdFabricante
                        If filtro.IdProveedor > 0 Then .Add("@idProveedor", SqlDbType.Int).Value = filtro.IdProveedor
                        If filtro.IdTipoProducto > 0 Then .Add("@idTipoProducto", SqlDbType.SmallInt).Value = filtro.IdTipoProducto
                        If filtro.Activo > 0 Then .Add("@estado", SqlDbType.Bit).Value = IIf(filtro.Activo = 1, 1, 0)
                        If filtro.SeparadorProveedor IsNot Nothing AndAlso filtro.SeparadorProveedor.Trim.Length > 0 Then _
                            .Add("@separadorProveedor", SqlDbType.VarChar, 4).Value = filtro.SeparadorProveedor.Trim
                    End With
                    dtDatos = .ejecutarDataTable("ObtenerInfoProducto", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerLista(ByVal filtro As FiltroListaProducto) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDatos As DataTable
            Try
                With dbManager
                    With .SqlParametros
                        If filtro.IdProducto > 0 Then .Add("@idProducto", SqlDbType.Int).Value = filtro.IdProducto
                        If filtro.IdTecnologia > 0 Then .Add("@idTecnologia", SqlDbType.Int).Value = filtro.IdTecnologia
                        If filtro.IdFabricante > 0 Then .Add("@idFabricante", SqlDbType.SmallInt).Value = filtro.IdFabricante
                        If filtro.IdProveedor > 0 Then .Add("@idProveedor", SqlDbType.Int).Value = filtro.IdProveedor
                        If filtro.IdTipoProducto > 0 Then .Add("@idTipoProducto", SqlDbType.SmallInt).Value = filtro.IdTipoProducto
                        If filtro.Activo > 0 Then .Add("@estado", SqlDbType.Bit).Value = IIf(filtro.Activo = 1, 1, 0)
                        If filtro.ListaIdTipoProducto IsNot Nothing AndAlso filtro.ListaIdTipoProducto.Count > 0 Then .Add("@listaIdTipoProducto", SqlDbType.VarChar).Value = Join(filtro.ListaIdTipoProducto.ToArray, ",")
                    End With
                    dtDatos = .ejecutarDataTable("ObtenerListaProducto", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Shared Function ObtenerPorId(ByVal identificador As Integer) As DataTable
            Dim filtro As New FiltroProducto
            Dim dtDatos As New DataTable
            filtro.IdProducto = identificador
            dtDatos = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ValoresMateriales(ByVal pMaterial As String) As DataTable
            Dim resultado As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Clear()
                        .Add("@material", SqlDbType.VarChar).Value = pMaterial
                    End With
                    resultado = .ejecutarDataTable("ObtenerValoresMaterial", CommandType.StoredProcedure)
                End With
            Catch ex As Exception
                Throw New Exception("Error al tratar de cargar los valores declarados por cada material: " & ex.Message)
            Finally
                dbManager.Dispose()
            End Try
            Return resultado
        End Function

        Public Function CargaValidacionTipoProducto(ByVal Material As String) As Producto

            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@Material", SqlDbType.BigInt).Value = Material

                    End With
                    .ejecutarReader("ObteneridTipoProductoLecturaSerialMaterial", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        IdTipoProducto = .Reader("idTipoProducto").ToString
                        IdTecnologia = .Reader("idTecnologia").ToString
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
            Return TipoProducto

        End Function

#End Region

#Region "Estructuras"

        Public Structure ImagenProducto
            Dim imagen As Byte()
            Dim contenType As String
            Dim nombreImagen As String
            Dim tamanio As Integer
            Dim producto As String
        End Structure

#End Region

    End Class

End Namespace


