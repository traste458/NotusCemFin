Imports LMDataAccessLayer

Namespace Fulfillment

    Public Class GeneradorDuplaPreactivada

#Region "Atributos"

        Private _serial As String
        Private _msisdn As String
        Private _codigoEan As String
        Private _referencia As String
        Private _material As String
        Private _fechaGeneracion As Date
        Private _idUsuarioGenera As Integer
        Private _usuarioGenera As String
        Private _registrado As Boolean

#End Region

#Region "Contructores"

        Public Sub New()
            _serial = ""
            _msisdn = ""
        End Sub

        Public Sub New(ByVal serial As String)
            Me.New()
            Me._serial = serial
            CargarInformacion()
        End Sub

#End Region

#Region "Propiedades"

        Public Property Serial As String
            Get
                Return _serial
            End Get
            Set(ByVal value As String)
                _serial = value
            End Set
        End Property

        Public Property Msisdn As String
            Get
                Return _msisdn
            End Get
            Protected Friend Set(ByVal value As String)
                _msisdn = value
            End Set
        End Property

        Public Property CodigoEan As String
            Get
                Return _codigoEan
            End Get
            Protected Friend Set(value As String)
                _codigoEan = value
            End Set
        End Property

        Public Property Referencia As String
            Get
                Return _referencia
            End Get
            Protected Friend Set(value As String)
                _referencia = value
            End Set
        End Property

        Public Property Material As String
            Get
                Return _material
            End Get
            Protected Friend Set(value As String)
                _material = value
            End Set
        End Property

        Public Property FechaGeneracion As Date
            Get
                Return _fechaGeneracion
            End Get
            Set(value As Date)
                _fechaGeneracion = value
            End Set
        End Property

        Public Property IdUsuarioGenera As Integer
            Get
                Return _idUsuarioGenera
            End Get
            Set(value As Integer)
                _idUsuarioGenera = value
            End Set
        End Property

        Public Property UsuarioGenera As String
            Get
                Return _usuarioGenera
            End Get
            Protected Friend Set(value As String)
                _usuarioGenera = value
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
            If Not EsNuloOVacio(Me._msisdn) OrElse Not EsNuloOVacio(Me._serial) Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        If Not EsNuloOVacio(Me._msisdn) Then .SqlParametros.Add("@msisdn", SqlDbType.VarChar, 30).Value = Me._msisdn.Trim
                        If Not EsNuloOVacio(Me._serial) Then .SqlParametros.Add("@serial", SqlDbType.VarChar, 50).Value = Me._serial.Trim

                        .ejecutarReader("ObtenerInfoDuplaPreactivacionSerial", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                AsignarValorAPropiedades(.Reader)
                                _registrado = True
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
                    Me._serial = reader("serial").ToString
                    Me._msisdn = reader("msisdn").ToString
                    Me._codigoEan = reader("codigoEan").ToString
                    Me._referencia = reader("referencia").ToString
                    Me._material = reader("material").ToString
                    Date.TryParse(reader("fechaGeneracion").ToString, Me._fechaGeneracion)
                    Integer.TryParse(reader("idUsuarioGenera").ToString, Me._idUsuarioGenera)
                    Me._usuarioGenera = reader("usuarioGenera").ToString
                    Me._registrado = True
                End If
            End If
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function MarcarRegistroComoGenerado() As ResultadoProceso
            Dim resultado As New ResultadoProceso(200, "Proceso no exitoso. Por favor intente nuevamente")
            If Me._idUsuarioGenera > 0 AndAlso Not EsNuloOVacio(Me._serial) Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@serial", SqlDbType.VarChar, 50).Value = Me._serial.Trim.ToUpper
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me._idUsuarioGenera
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("MarcarDuplaSerialPreactivadoComoGenerada", CommandType.StoredProcedure)
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
                resultado.EstablecerMensajeYValor(300, "No se han establecido todos los datos requeridos para generar la dupla. Por favor verifique")
            End If

            Return resultado
        End Function

#End Region

    End Class

End Namespace
