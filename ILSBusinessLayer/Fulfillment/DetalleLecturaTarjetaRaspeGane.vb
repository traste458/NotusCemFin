Imports LMDataAccessLayer

Namespace Fulfillment

    Public Class DetalleLecturaTarjetaRaspeGane

#Region "Atributos"

        Private _idDetalle As Long
        Private _idRango As Integer
        Private _imei As String
        Private _consecutivo As Long
        Private _cerrarRango As Boolean
        Private _registrado As Boolean

#End Region

#Region "Contructores"

        Public Sub New()
        End Sub

        Public Sub New(ByVal consecutivo As Long)
            Me.New()
            _consecutivo = consecutivo
            CargarInformacion()
        End Sub

        Public Sub New(ByVal imei As String)
            Me.New()
            Me._imei = imei
            CargarInformacion()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdDetalle As Long
            Get
                Return _idDetalle
            End Get
            Set(ByVal value As Long)
                _idDetalle = value
            End Set
        End Property

        Public Property IdRango As Integer
            Get
                Return _idRango
            End Get
            Set(ByVal value As Integer)
                _idRango = value
            End Set
        End Property

        Public Property Imei As String
            Get
                Return _imei
            End Get
            Set(ByVal value As String)
                _imei = value
            End Set
        End Property

        Public Property Consecutivo As Long
            Get
                Return _consecutivo
            End Get
            Set(ByVal value As Long)
                _consecutivo = value
            End Set
        End Property

        Public ReadOnly Property CerrarRango As Boolean
            Get
                Return _cerrarRango
            End Get
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
            If Me._consecutivo > 0 OrElse Not HerramientasFuncionales.EsNuloOVacio(Me._imei) Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        If Me._consecutivo > 0 Then .SqlParametros.Add("@consecutivo", SqlDbType.BigInt).Value = Me._consecutivo
                        If Not HerramientasFuncionales.EsNuloOVacio(Me._imei) Then _
                            .SqlParametros.Add("@imei", SqlDbType.VarChar, 50).Value = Me._imei.Trim

                        .ejecutarReader("ObtenerInfoDetalleLecturaTarjetaRaspeGane", CommandType.StoredProcedure)
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
                    Long.TryParse(reader("idDetalle").ToString, Me._idDetalle)
                    Integer.TryParse(reader("idRango").ToString, Me._idRango)
                    Me._imei = reader("imei").ToString
                    Long.TryParse(reader("consecutivo").ToString, Me._consecutivo)
                    Me._registrado = True
                End If
            End If
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso(200, "Proceso no exitoso. Por favor intente nuevamente")
            If Not HerramientasFuncionales.EsNuloOVacio(Me._imei) AndAlso Me._consecutivo > 0 AndAlso Me._idRango > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idRango", SqlDbType.Int).Value = Me._idRango
                        .SqlParametros.Add("@imei", SqlDbType.VarChar, 50).Value = Me._imei.Trim
                        .SqlParametros.Add("@consecutivo", SqlDbType.BigInt).Value = Me._consecutivo
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@cerrarRango", SqlDbType.Bit).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("RegistrarDetalleLecturaTarjetaRaspeGane", CommandType.StoredProcedure)
                        If Short.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                            If Not EsNuloOVacio(.SqlParametros("@cerrarRango").Value) Then _cerrarRango = CBool(.SqlParametros("@cerrarRango").Value.ToString)
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