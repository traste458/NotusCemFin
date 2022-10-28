Imports LMDataAccessLayer
Imports System.Net.Mail
Imports System.Text

Public Class CreacionUsuariosMasiva
#Region "Atributos"

    Private _resultado As Int32
    Private _IdUsuario As Int32
#End Region

#Region "Propiedades"


    Public Property Resultado() As Int32
        Get
            Return _resultado
        End Get
        Set(ByVal value As Int32)
            _resultado = value
        End Set
    End Property
    Public Property IdUsuario() As Int32
        Get
            Return _IdUsuario
        End Get
        Set(ByVal value As Int32)
            _IdUsuario = value
        End Set
    End Property

#End Region

    Public Function CrearUsuarios(ByVal dtSeriales As DataTable) As DataTable
        Dim dt As New DataTable
        Dim dbManager As New LMDataAccess
        dtSeriales.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), IdUsuario))
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                End With
                .EjecutarNonQuery("EliminaRegistroTransitoriaCargueUsuarios", CommandType.StoredProcedure)
                .InicilizarBulkCopy()
                .TiempoEsperaComando = 0
                With .BulkCopy
                    .DestinationTableName = "TransitoriaCreacionDeUsuarios"
                    .ColumnMappings.Add("Fila", "Fila")
                    .ColumnMappings.Add("nombreApellido", "NombreApellido")
                    .ColumnMappings.Add("Identificacion", "Identificacion")
                    .ColumnMappings.Add("Cargo", "Cargo")
                    .ColumnMappings.Add("Ciudad", "Ciudad")
                    .ColumnMappings.Add("Usuario", "Usuario")
                    .ColumnMappings.Add("Correo", "Correo")
                    .ColumnMappings.Add("idUsuario", "idUsuario")
                    .WriteToServer(dtSeriales)
                End With
                .IniciarTransaccion()
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                dt = .EjecutarDataTable("ProcesarCargueUsuarios", CommandType.StoredProcedure)

                Dim resu As String = .SqlParametros("@resultado").Value.ToString
                _resultado = CType(.SqlParametros("@resultado").Value.ToString, Int32)

                If _resultado = 0 Then
                    .AbortarTransaccion()
                    _resultado = 0
                    Return dt
                Else
                    .ConfirmarTransaccion()
                    For Each row As DataRow In dtSeriales.Rows
                        Dim valor As String = CStr(row("Identificacion"))
                        notificarContrasena(valor)
                    Next
                    Return dt
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
    End Function

    Public Function notificarContrasena(ByVal identificacion As String) As Integer
        Dim contrasenaRandom As New CreacionContrasenaRandom
        Dim encriptarContrasena As New EncryptionLibrary
        Dim resultadoConsulta As New List(Of String)
        Dim contrRandom As String
        Dim recuperarContrasenaBD As New RecuperacionContrasena

        contrRandom = contrasenaRandom.CreacionContrasenaRandom()
        resultadoConsulta = recuperarContrasenaBD.AlmacenarRamdomContrasena(identificacion, contrRandom)


        If resultadoConsulta.Item(0) = "0" Then
            Return 2
        End If

        If resultadoConsulta.Item(0) <> "0" Then
            With recuperarContrasenaBD
                EnviarCorreoCreacionUsuarioContrasena(resultadoConsulta.Item(1), resultadoConsulta.Item(0), resultadoConsulta.Item(2))
            End With
            Return 1
        Else
            Return 0
        End If

    End Function

    Public Function EnviarCorreoCreacionUsuarioContrasena(ByVal destinatario As String, ByVal usuario As String, ByVal token As String)
        Dim direccionPara As New MailAddressCollection
        Dim resultadoEnviado As Boolean = False
        Dim sb As New StringBuilder
        Dim correo As New AdministradorCorreo
        correo.Receptor.Add(destinatario)
        Dim urlRecuperacion As Comunes.ConfigValues = New Comunes.ConfigValues("URL_RECUPERACION_CONTRASENA")
        Try
            With correo
                .Titulo = "Asignacion Contraseña Creacion De Usuario Nuevo"
                .Asunto = "Asignacion Contraseña Creacion De Usuario Nuevo"
                .Receptor = .Receptor
                .TextoMensaje = String.Concat("Hola: ", usuario, vbCrLf, ", Por favor ingresar al link de abajo para asignar la contraseña a su usuario: ", vbCrLf, "</br><a style='margin:10px 0 10px 0;color:#ffffff;font-weight:bold;display:inline-block;padding:6px 10px;font-size:16px;text-align:center;background-image:none;border:1px solid transparent;border-radius:10px;-moz-border-radius:10px;-webkit-border-radius:10px;-khtml-border-radius:10px; background-color:#836493;' href='" & urlRecuperacion.ConfigKeyValue.ToString & token & "'> Asignacion contraseña </a></br>", vbCrLf, "si no asigna contraseña no podra ingresar con el usuario creado al sistema")
                .FirmaMensaje = "Logytech Mobile S.A.S <br />PBX. 57(1) 4395237 Ext 174 - 135"
                resultadoEnviado = .EnviarMail()
            End With
        Finally
        End Try
        Return resultadoEnviado
    End Function

End Class
