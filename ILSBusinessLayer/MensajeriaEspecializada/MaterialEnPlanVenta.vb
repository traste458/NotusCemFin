Imports ILSBusinessLayer
Imports LMDataAccessLayer

Namespace MensajeriaEspecializada

    Public Class MaterialEnPlanVenta

#Region "Atributos (Campos)"

        Private _idRegistro As Integer
        Private _idPlan As Integer
        Private _nombrePlan As String
        Private _material As String
        Private _descripcionMaterial As String
        Private _precioVentaEquipo As Double
        Private _ivaEquipo As Double

        Private _registrado As Boolean
#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idRegistro As Integer)
            Me.New()
            _idRegistro = idRegistro
            CargarDatos()
        End Sub

        Public Sub New(ByVal idPlan As Integer, material As String)
            Me.New()
            _idPlan = idPlan
            _material = material
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdRegistro As Integer
            Get
                Return _idRegistro
            End Get
            Set(value As Integer)
                _idRegistro = value
            End Set
        End Property

        Public Property IdPlan As Integer
            Get
                Return _idPlan
            End Get
            Set(value As Integer)
                _idPlan = value
            End Set
        End Property

        Public Property NombrePlan As String
            Get
                Return _nombrePlan
            End Get
            Protected Friend Set(value As String)
                _nombrePlan = value
            End Set
        End Property

        Public Property Material As String
            Get
                Return _material
            End Get
            Set(value As String)
                _material = value
            End Set
        End Property

        Public Property DescripcionMaterial As String
            Get
                Return _descripcionMaterial
            End Get
            Protected Friend Set(value As String)
                _descripcionMaterial = value
            End Set
        End Property

        Public Property PrecioVentaEquipo As Double
            Get
                Return _precioVentaEquipo
            End Get
            Set(value As Double)
                _precioVentaEquipo = value
            End Set
        End Property

        Public Property IvaEquipo As Double
            Get
                Return _ivaEquipo
            End Get
            Set(value As Double)
                _ivaEquipo = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            If Not String.IsNullOrEmpty(_material) OrElse _idPlan > 0 OrElse _idRegistro > 0 Then
                Using dbManager As New LMDataAccess
                    Try
                        With dbManager
                            If Not String.IsNullOrEmpty(_material) Then .SqlParametros.Add("@material", SqlDbType.VarChar).Value = _material
                            If _idPlan > 0 Then .SqlParametros.Add("@idPlan", SqlDbType.Int).Value = _idPlan
                            If _idRegistro > 0 Then .SqlParametros.Add("@idRegistro", SqlDbType.Int).Value = _idRegistro

                            .ejecutarReader("ObtenerDetalleMaterialEnPlanVenta", CommandType.StoredProcedure)
                            If .Reader IsNot Nothing Then
                                If .Reader.Read() Then CargarResultadoConsulta(.Reader)
                                If Not .Reader.IsClosed Then .Reader.Close()
                            End If
                        End With
                    Catch ex As Exception
                        Throw ex
                    End Try
                End Using
            End If
        End Sub

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Integer.TryParse(reader("idRegistro").ToString, _idRegistro)
                    Integer.TryParse(reader("idPlan").ToString, _idPlan)
                    _nombrePlan = reader("nombrePlan").ToString
                    _material = reader("material").ToString
                    _descripcionMaterial = reader("descripcionMaterial").ToString
                    Double.TryParse(reader("precioVentaEquipo").ToString, _precioVentaEquipo)
                    Double.TryParse(reader("ivaEquipo").ToString, _ivaEquipo)

                    _registrado = True
                End If
            End If
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Adicionar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Using dbManager As New LMDataAccess
                Try
                    If _idPlan > 0 And Not String.IsNullOrEmpty(_material) Then
                        With dbManager
                            .SqlParametros.Add("@idPlan", SqlDbType.Int).Value = _idPlan
                            .SqlParametros.Add("@material", SqlDbType.VarChar).Value = _material
                            .SqlParametros.Add("@precioVentaEquipo", SqlDbType.Money).Value = _precioVentaEquipo
                            .SqlParametros.Add("@ivaEquipo", SqlDbType.Money).Value = _ivaEquipo

                            .SqlParametros.Add("@result", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                            .ejecutarNonQuery("RegistrarMaterialEnPlanVenta", CommandType.StoredProcedure)
                            Dim respuesta As Integer = .SqlParametros("@result").Value
                            If respuesta = 0 Then
                                resultado.EstablecerMensajeYValor(respuesta, "Material agregados satisfactoriamente al plan.")
                            Else
                                Select Case respuesta
                                    Case 1 : resultado.EstablecerMensajeYValor(respuesta, "El plan no se encuentra registrado en el sistema")
                                    Case 2 : resultado.EstablecerMensajeYValor(respuesta, "El material no se encuentra registrado en el sistema")
                                    Case 3 : resultado.EstablecerMensajeYValor(respuesta, "El material ya se encuentra asociados al plan de venta")
                                End Select
                            End If
                        End With
                    Else
                        resultado.EstablecerMensajeYValor(100, "No se han establecido los valores mínimos para vincular el material.")
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return resultado
        End Function

        Public Function Eliminar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If _idRegistro > 0 Or _idPlan > 0 Then
                Using dbManager As New LMDataAccess
                    Try
                        With dbManager
                            .SqlParametros.Add("@idRegistro", SqlDbType.BigInt).Value = _idRegistro
                            .SqlParametros.Add("@idPlan", SqlDbType.Int).Value = _idPlan
                            .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                            .iniciarTransaccion()
                            .ejecutarNonQuery("EliminarMaterialDePlanVenta", CommandType.StoredProcedure)
                            If Integer.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                                If resultado.Valor = 0 Then
                                    .confirmarTransaccion()
                                Else
                                    Select Case resultado.Valor
                                        Case 1
                                            resultado.Mensaje = "No se ha proporcionado información correspondiente al Material a desvincular"
                                        Case Else
                                            resultado.Mensaje = "Ocurrió un error inesperado al tratar de desvincular el Material"
                                    End Select
                                    .abortarTransaccion()
                                End If
                            Else
                                Throw New Exception("Imposible evaluar la respuesta del servidor.")
                            End If
                        End With
                    Catch ex As Exception
                        Throw ex
                    End Try
                End Using
            Else
                resultado.EstablecerMensajeYValor(10, "No se han proporcionado los datos mínimos para reconocer el registro a eliminar.")
            End If
            Return resultado
        End Function

#End Region

    End Class

End Namespace

