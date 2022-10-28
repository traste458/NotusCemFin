Imports System.IO
Imports System.Text
Imports System.Net.Mail
Imports System.Net.Mime
Imports LMDataAccessLayer
Imports ILSBusinessLayer
Imports ILSBusinessLayer.Comunes
Imports ILSBusinessLayer.Productos
Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Recibos
Imports System.Collections.Generic

Namespace Comunes

    Public Class NotificadorGeneralEventos

#Region "Atributos"

        Private _inicioMensaje As String
        Private _finMensaje As String
        Private _titulo As String
        Private _asunto As String
        Private _mensaje As String
        Private _firmaMensaje As String
        Private _mailRespuesta As String
        Private _usuarioRespuesta As String
        Private _tipoNotificacion As Comunes.AsuntoNotificacion.Tipo
        Private _dtImagenes As DataTable
        Private _dtDatos As DataTable
        Private _adjuntosURL As New ArrayList
        Private _destinatariosPrincipal As String
        Private _destinatariosCopia As String

#End Region

#Region "Propiedades"

        Public Property InicioMensaje As String
            Get
                Return _inicioMensaje
            End Get
            Set(value As String)
                _inicioMensaje = value
            End Set
        End Property

        Public Property FinMensaje As String
            Get
                Return _finMensaje
            End Get
            Set(value As String)
                _finMensaje = value
            End Set
        End Property

        Public Property Titulo As String
            Get
                Return _titulo
            End Get
            Set(value As String)
                _titulo = value
            End Set
        End Property

        Public Property Asunto As String
            Get
                Return _asunto
            End Get
            Set(value As String)
                _asunto = value
            End Set
        End Property

        Public Property MailRespuesta As String
            Get
                Return _mailRespuesta
            End Get
            Set(value As String)
                _mailRespuesta = value
            End Set
        End Property

        Public Property UsuarioRespuesta As String
            Get
                Return _usuarioRespuesta
            End Get
            Set(value As String)
                _usuarioRespuesta = value
            End Set
        End Property

        Public Property FirmaMensaje As String
            Get
                Return _firmaMensaje
            End Get
            Set(value As String)
                _firmaMensaje = value
            End Set
        End Property

        Public Property TipoNotificacion As Comunes.AsuntoNotificacion.Tipo
            Get
                Return _tipoNotificacion
            End Get
            Set(value As Comunes.AsuntoNotificacion.Tipo)
                _tipoNotificacion = value
            End Set
        End Property

        Public Property dtImagenes() As DataTable
            Get
                Return _dtImagenes
            End Get
            Set(value As DataTable)
                _dtImagenes = value
            End Set
        End Property

        Public Property dtDatos() As DataTable
            Get
                Return _dtDatos
            End Get
            Set(value As DataTable)
                _dtDatos = value
            End Set
        End Property

        Public Property Mensaje() As String
            Get
                Return _mensaje
            End Get
            Set(value As String)
                _mensaje = value
            End Set
        End Property

        Public Property AdjuntosURL() As ArrayList
            Get
                Return _adjuntosURL
            End Get
            Set(ByVal value As ArrayList)
                _adjuntosURL = value
            End Set
        End Property

        Public Property DestinatarioPrincipal As String
            Get
                Return _destinatariosPrincipal
            End Get
            Set(value As String)
                _destinatariosPrincipal = value
            End Set
        End Property

        Public Property DestinatarioCopia As String
            Get
                Return _destinatariosCopia
            End Get
            Set(value As String)
                _destinatariosCopia = value
            End Set
        End Property

#End Region

#Region "Métodos Públicos"

        Public Function NotificacionEvento(Optional ByVal mensaje As String = "", Optional mensajeDetalle As String = "", Optional idBodega As Integer = 0, _
                                           Optional ByVal usuarioUnicoNotificacion As String = "") As ResultadoProceso
            Dim Notificacion As New AdministradorCorreo
            Dim DestinosPP As New MailAddressCollection
            Dim DestinosCC As New MailAddressCollection
            Dim respuestaEnvio As New ResultadoProceso
            Dim sbContenido As New StringBuilder

            Try
                With sbContenido
                    .Append(_inicioMensaje)
                    If mensaje <> "" Then
                        .Append("<br/> " & mensaje)
                    End If
                    .Append("<br/> " & _finMensaje)
                    If mensajeDetalle <> "" Then
                        .Append("<br/> " & mensajeDetalle)
                    End If
                End With

                With Notificacion
                    CargarDestinatarios(DestinosPP, DestinosCC, idBodega, usuarioUnicoNotificacion)
                    .Titulo = _titulo
                    .Asunto = _asunto
                    .TextoMensaje = sbContenido.ToString
                    .FirmaMensaje = _firmaMensaje
                    .Receptor = DestinosPP
                    .Copia = DestinosCC
                    If _adjuntosURL IsNot Nothing Then
                        .RutaAttachment = _adjuntosURL
                    End If
                    If Not String.IsNullOrEmpty(_mailRespuesta) Or Not String.IsNullOrEmpty(_usuarioRespuesta) Then _
                        .EstablecerCuentaRespuesta(_mailRespuesta, _usuarioRespuesta)
                    If Not .EnviarMail() Then
                        respuestaEnvio.EstablecerMensajeYValor(1, "Ocurrió un error inesperado y no fué posible enviar la notificación")
                    End If
                End With
            Finally
            End Try
            Return respuestaEnvio
        End Function

       Public Function NotificacionEventoImagen() As ResultadoProceso
            Dim Notificacion As New AdministradorCorreo
            Dim DestinosPP As New MailAddressCollection
            Dim DestinosCC As New MailAddressCollection
            Dim respuestaEnvio As New ResultadoProceso
            Dim sbContenido As New StringBuilder
            Dim mensaje As String = String.Empty
            Dim vistaAlterna As AlternateView = Nothing
            CrearMensaje(mensaje, vistaAlterna)
            Try
                With Notificacion
                    CargarDestinatariosNotificacion(_tipoNotificacion, DestinosPP, DestinosCC)
                    .Titulo = _titulo
                    .Asunto = _asunto
                    .TextoMensaje = sbContenido.ToString
                    .FirmaMensaje = "Logytech Mobile S.A.S <br />"
                    If (DestinosPP.Count > 0) Then
                        .Receptor = DestinosPP
                    End If
                    If (DestinosCC.Count > 0) Then
                        .Copia = DestinosCC
                    End If

                    .VistaAlternativa.Add(vistaAlterna)
                    If (DestinosCC.Count > 0 Or DestinosPP.Count > 0) Then
                        If Not .EnviarMail() Then
                            respuestaEnvio.EstablecerMensajeYValor(1, "Ocurrió un error inesperado y no fué posible enviar la notificación")
                        End If
                    Else
                        respuestaEnvio.EstablecerMensajeYValor(9, "No existe destinatario de correo configurado para enviar la notificación")
                    End If
                    
                End With
            Finally
            End Try
            Return respuestaEnvio
        End Function

        Public Function NotificacionEventoAdjunto() As ResultadoProceso
            Dim Notificacion As New AdministradorCorreo
            Dim DestinosPP As New MailAddressCollection
            Dim DestinosCC As New MailAddressCollection
            Dim respuestaEnvio As New ResultadoProceso
            Dim sbContenido As New StringBuilder
            Dim vistaAlterna As AlternateView = Nothing
            Try
                With sbContenido
                    .Append(_mensaje)
                End With

                With Notificacion
                    If _destinatariosPrincipal.Trim.Length <> 0 Or _destinatariosCopia.Trim.Length <> 0 Then
                        If _destinatariosPrincipal.Trim.Length > 0 Then DestinosPP.Add(_destinatariosPrincipal)
                        If _destinatariosCopia.Trim.Length > 0 Then DestinosCC.Add(_destinatariosCopia)
                    Else
                        CargarDestinatariosNotificacion(_tipoNotificacion, DestinosPP, DestinosCC)
                    End If
                    .Titulo = _titulo
                    .Asunto = _asunto
                    .TextoMensaje = sbContenido.ToString
                    .FirmaMensaje = "Logytech Mobile S.A.S <br />"
                    .Receptor = DestinosPP
                    .Copia = DestinosCC
                    '.VistaAlternativa.Add(vistaAlterna)
                    .RutaAttachment = _adjuntosURL
                    If Not .EnviarMail() Then
                        respuestaEnvio.EstablecerMensajeYValor(1, "Ocurrió un error inesperado y no fué posible enviar la notificación")
                    End If
                End With
            Finally
            End Try
            Return respuestaEnvio
        End Function

#End Region

#Region "Métodos Privados"

        Private Sub CargarDestinatarios(ByVal destinoPP As MailAddressCollection, ByVal destinoCC As MailAddressCollection, Optional ByVal idBodega As Integer = 0, _
                                        Optional ByVal usuarioUnicoNotificacion As String = "")
            Dim ConfiguracionUsuario As New UsuarioNotificacion
            Dim filtro As New FiltroUsuarioNotificacion
            Dim dtDestinos As New DataTable
            Dim strDestinoPP As String = String.Empty
            Dim strDestinoCC As String = String.Empty

            If usuarioUnicoNotificacion = "" Then
                filtro.IdAsuntoNotificacion = _tipoNotificacion
                If idBodega <> 0 Then
                    filtro.IdBodega = idBodega
                End If
                filtro.Separador = ","
                Try
                    dtDestinos = UsuarioNotificacion.ObtenerDestinatarioNotificacion(filtro)
                    For Each fila As DataRow In dtDestinos.Rows
                        strDestinoPP += fila.Item("destinoPara")
                        strDestinoCC += fila.Item("destinoCopia")
                    Next

                    destinoPP.Add(strDestinoPP)
                    destinoCC.Add(strDestinoCC)

                Catch ex As Exception
                Finally
                    If dtDestinos IsNot Nothing Then dtDestinos.Rows.Clear()
                End Try
            Else
                strDestinoPP += (usuarioUnicoNotificacion)
                destinoPP.Add(strDestinoPP)
            End If

        End Sub

        Private Sub CrearMensaje(ByRef mensajeRespuesta As String, ByRef viewAltern As AlternateView)
            Dim mensaje As New StringBuilder()
            Dim listGeneralImagenes As New List(Of OrdenRecepcion.Imagen)
            Dim _notaMensaje = "Este mensaje fue generado automáticamente, si tiene alguna duda o inquietud respecto al mismo, por favor envía un correo electrónico al grupo <a href='mailto:itdevelopment@logytechmobile.com?subject=Inquietud Notificador'> IT Development </a>"
            Dim value As New ConfigValues("URL_IMAGEN_NOTIFICACION")
            Dim url As String = value.ConfigKeyValue
            Dim i As Integer = 0

            With mensaje
                .Append("<HTML>")
                .Append("	<HEAD>")
                .Append("		<LINK href='" & url & "include/styleBACK.css' type='text/css' rel='stylesheet'>")
                .Append("	</HEAD>")
                .Append("	<body class='cuerpo2'>")
                .Append("	<table width='100%' border='0' align='center' cellpadding='5' cellspacing='0' class='tabla'")
                .Append("		ID='Table1'>")
                .Append("		<tr>")
                .Append("			<td width='20%' ><img src='" & url & "images/logo_trans.png'>")
                .Append("			</td>")
                .Append("			<td align='center' bgcolor='#883485' width='80%'><font size='3' face='arial' color='white'><b>" & _titulo & "</b></font></td>")
                .Append("		</tr>")
                .Append("	</table>")
                .Append("	<br />")
                .Append("	<br />")
                If Now.Hour < 12 Then
                    .Append("	<font class='fuente'>Buenos Días, ")
                ElseIf Now.Hour > 18 Then
                    .Append("	<font class='fuente'>Buenas Noches, ")
                Else
                    .Append("	<font class='fuente'>Buenas Tardes, ")
                End If
                .Append("<br/><br/>" + _mensaje + "</br><br/>")
                .Append("<br/>Guia: " + dtDatos.Rows(0).Item("guia").ToString + "</br>")
                .Append("Factura: " + dtDatos.Rows(0).Item("factura").ToString + "</br>")
                .Append("Producto: " + dtDatos.Rows(0).Item("producto").ToString + " - " + dtDatos.Rows(0).Item("color").ToString + "</br>")
                .Append("Cantidad Aprox: " + dtDatos.Rows(0).Item("cantidadAprox").ToString + "</br>")
                .Append("Piezas: " + dtDatos.Rows(0).Item("piezas").ToString + "</br>")
                .Append("Peso Guía: " + dtDatos.Rows(0).Item("pesoGuia").ToString + "</br>")
                .Append("Peso Recibido: " + dtDatos.Rows(0).Item("pesoRecibido").ToString + "</br>")
                .Append("Diferencia: " + dtDatos.Rows(0).Item("diferencia").ToString + "</br>")
                .Append("Bodega Alma: " + dtDatos.Rows(0).Item("bodega").ToString + "</br>")
                .Append("Estado de los Pallets: " + dtDatos.Rows(0).Item("estado").ToString + "</br>")
                .Append("Orden Recepcion Notus: " + dtDatos.Rows(0).Item("ordenRecepcion").ToString + "</br></br>")
                .AppendLine("<br/><br/><table border='1' cellpadding='5' cellspacing='0' width=30% bordercolor='#f0f0f0'><tr bgcolor='#dddddd'><td colspan='3'><b>Imagenes de Soporte</td></tr>")
                Dim listImagenes As New List(Of OrdenRecepcion.Imagen)
                Dim objImagen As New OrdenRecepcion()
                With objImagen
                    .IdOrdenRecepcion = dtDatos.Rows(0).Item("ordenRecepcion").ToString
                    listImagenes = .ListaImagenes
                End With
                Dim x As Integer = 0
                For i = 0 To listImagenes.Count - 1
                    x = x + 1
                    If x = 1 Then
                        .AppendLine("<tr>")
                    End If
                    .AppendLine("<td width='100%'><img src=""cid:" + listImagenes(i).nombreImagen.ToString + """ alt='Imagen Recepcion' width='350' height='350'/></td>")
                    listGeneralImagenes.Add(listImagenes(i))
                    If x = 3 Then
                        .AppendLine("</tr>")
                        x = 0
                    End If
                Next
                .AppendLine("</table>")
                .Append("<br />	<font class='fuente'>Cordial Saludo,<br />")
                .Append("		<br><b>Logytech Mobile S.A.S <br /></b><br /><br />")
                .Append("</font><br /><br /><br /><br /><br />")
                If _notaMensaje <> "" Then
                    .Append("	<font class='fuente' size='1'><i>Nota: " & _notaMensaje & ".</i></font>")
                End If
                .Append("	</body>")
                .Append("</HTML>")
            End With

            Dim htmlBody As AlternateView = AlternateView.CreateAlternateViewFromString(mensaje.ToString, Nothing, MediaTypeNames.Text.Html)
            Dim lrImgProducto As LinkedResource
            For Each imgProd As OrdenRecepcion.Imagen In listGeneralImagenes
                lrImgProducto = New LinkedResource(New MemoryStream(imgProd.imagen))
                lrImgProducto.ContentId = imgProd.nombreImagen
                htmlBody.LinkedResources.Add(lrImgProducto)
            Next
            viewAltern = htmlBody
            mensajeRespuesta = mensaje.ToString()
        End Sub

        Private Sub CargarDestinatariosNotificacion(ByVal tipo As Comunes.AsuntoNotificacion.Tipo, ByVal destinoPP As MailAddressCollection, ByVal destinoCC As MailAddressCollection)
            Dim ConfiguracionUsuario As New UsuarioNotificacion
            Dim filtro As New FiltroUsuarioNotificacion
            Dim dtDestinos As New DataTable
            Dim strDestinoPP As String = String.Empty
            Dim strDestinoCC As String = String.Empty

            filtro.IdAsuntoNotificacion = tipo
            filtro.Separador = ","
            Try
                dtDestinos = UsuarioNotificacion.ObtenerDestinatarioNotificacion(filtro)
                For Each fila As DataRow In dtDestinos.Rows
                    strDestinoPP += fila.Item("destinoPara")
                    strDestinoCC += fila.Item("destinoCopia")
                Next

                If (strDestinoPP <> String.Empty And strDestinoPP <> "") Then
                    destinoPP.Add(strDestinoPP)
                End If
                If (strDestinoCC <> String.Empty And strDestinoCC <> "") Then
                    destinoCC.Add(strDestinoCC)
                End If



            Catch ex As Exception
            Finally
                If dtDestinos IsNot Nothing Then dtDestinos.Rows.Clear()
            End Try
        End Sub

#End Region

    End Class

End Namespace