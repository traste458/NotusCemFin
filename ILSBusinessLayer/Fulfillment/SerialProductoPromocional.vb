Imports LMDataAccessLayer

Namespace Fulfillment
    Public Class SerialProductoPromocional

#Region "Atributos"

        Private _imei As String
        Private _serialProductoPromocional As String
        Private _fechaRegistro As Date
        Private _idUsuarioRegistra As Integer
        Private _usuarioRegistra As String
        Private _registrado As Boolean

#End Region

#Region "Contructores"

        Public Sub New()
            _imei = ""
            _serialProductoPromocional = ""
        End Sub

        Public Sub New(ByVal serial As String)
            Me.New()
            Me._serialProductoPromocional = serial
            CargarInformacion()
        End Sub

#End Region

#Region "Propiedades"

        Public Property Imei As String
            Get
                Return _imei
            End Get
            Set(ByVal value As String)
                _imei = value
            End Set
        End Property

        Public Property SerialProductoPromocional As String
            Get
                Return _serialProductoPromocional
            End Get
            Set(ByVal value As String)
                _serialProductoPromocional = value
            End Set
        End Property

        Public Property FechaRegistro As Date
            Get
                Return _fechaRegistro
            End Get
            Set(value As Date)
                _fechaRegistro = value
            End Set
        End Property

        Public Property IdUsuarioRegistra As Integer
            Get
                Return _idUsuarioRegistra
            End Get
            Set(value As Integer)
                _idUsuarioRegistra = value
            End Set
        End Property

        Public Property UsuarioRegistra As String
            Get
                Return _usuarioRegistra
            End Get
            Protected Friend Set(value As String)
                _usuarioRegistra = value
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

#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            If Not EsNuloOVacio(Me._serialProductoPromocional) OrElse Not EsNuloOVacio(Me._imei) Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        If Not EsNuloOVacio(Me._serialProductoPromocional) Then _
                            .SqlParametros.Add("@serialProductoPromocional", SqlDbType.VarChar, 30).Value = Me._serialProductoPromocional.Trim.ToUpper
                        If Not EsNuloOVacio(Me._imei) Then .SqlParametros.Add("@imei", SqlDbType.VarChar, 50).Value = Me._imei.Trim

                        .ejecutarReader("ObtenerInfoSerialProductoPromocional", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                AsignarValorAPropiedades(.Reader)
                            End If
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
        End Sub

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub AsignarValorAPropiedades(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Me._imei = reader("imei").ToString
                    Me._serialProductoPromocional = reader("serialProductoPromocional").ToString
                    Date.TryParse(reader("fechaRegistro"), Me._fechaRegistro)
                    Integer.TryParse("idUsuarioRegistra", Me._idUsuarioRegistra)
                    Me._usuarioRegistra = reader("usuarioRegistra").ToString
                    Me._registrado = True
                End If
            End If
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso(200, "Proceso no exitoso. Por favor intente nuevamente")
            If Me._idUsuarioRegistra > 0 AndAlso Not EsNuloOVacio(Me._imei) AndAlso Not EsNuloOVacio(Me._serialProductoPromocional) Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@serialProductoPromocional", SqlDbType.VarChar, 50).Value = Me._serialProductoPromocional.Trim.ToUpper
                        .SqlParametros.Add("@imei", SqlDbType.VarChar, 50).Value = Me._imei.Trim
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me._idUsuarioRegistra
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("RegistrarSerialProductoPromocional", CommandType.StoredProcedure)
                        If Short.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                            resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                        Else
                            resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta emitida por el servidor. Por favor intente nuevamente")
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado.EstablecerMensajeYValor(300, "No se han establecido todos los datos requeridos para realizar el registro. Por favor verifique")
            End If

            Return resultado
        End Function

#End Region

    End Class

End Namespace