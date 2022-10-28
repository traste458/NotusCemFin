Imports ILSBusinessLayer
Imports LMDataAccessLayer

Namespace MensajeriaEspecializada

    Public Class MatrizTipoClientePlanRecaudo
#Region "Atributos (Campos)"

        Private _idTipoCliente As Integer
        Private _tipoCliente As String
        Private _idPlan As Integer
        Private _nombrePlan As String
        Private _requiereCFM As Boolean
        Private _numeroCuotas As Integer
        Private _cargoFijoMensual As Double

        Private _registrado As Boolean
#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idTipoCliente As Integer, ByVal idPlan As Integer)
            Me.New()
            _idPlan = idPlan
            _idTipoCliente = idTipoCliente
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdTipoCliente As Integer
            Get
                Return _idTipoCliente
            End Get
            Set(value As Integer)
                _idTipoCliente = value
            End Set
        End Property

        Public Property TipoCliente As String
            Get
                Return _tipoCliente
            End Get
            Protected Friend Set(value As String)
                _tipoCliente = value
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

        Public Property RequiereCFM As Boolean
            Get
                Return _requiereCFM
            End Get
            Set(value As Boolean)
                _requiereCFM = value
            End Set
        End Property

        Public Property NumeroCuotas As Integer
            Get
                Return _numeroCuotas
            End Get
            Protected Friend Set(value As Integer)
                _numeroCuotas = value
            End Set
        End Property

        Public Property CargoFijoMensual As Double
            Get
                Return _cargoFijoMensual
            End Get
            Protected Friend Set(value As Double)
                _cargoFijoMensual = value
            End Set
        End Property
#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            If _idTipoCliente > 0 OrElse _idPlan > 0 Then
                Using dbManager As New LMDataAccess
                    Try
                        With dbManager
                            If _idTipoCliente > 0 Then .SqlParametros.Add("@idTipoCliente", SqlDbType.Int).Value = _idTipoCliente
                            If _idPlan > 0 Then .SqlParametros.Add("@idPlan", SqlDbType.Int).Value = _idPlan

                            .ejecutarReader("ObtenerDetalleMatrizTipoClientePlanRecaudo", CommandType.StoredProcedure)
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
                    Integer.TryParse(reader("idTipoCliente").ToString, _idTipoCliente)
                    _tipoCliente = reader("tipo_cliente").ToString
                    Integer.TryParse(reader("idPlan").ToString, _idPlan)
                    _nombrePlan = reader("nombrePlan").ToString
                    _requiereCFM = CBool(reader("requiereCFM").ToString)
                    Integer.TryParse(reader("numeroCuotas").ToString, _numeroCuotas)
                    Double.TryParse(reader("cargoFijoMensual").ToString, _cargoFijoMensual)

                    _registrado = True
                End If
            End If
        End Sub

#End Region

#Region "Métodos Públicos"

        'Public Function Adicionar() As ResultadoProceso
        '    Dim resultado As New ResultadoProceso
        '    Using dbManager As New LMDataAccess
        '        Try
        '            If _idPlan > 0 And Not String.IsNullOrEmpty(_material) Then
        '                With dbManager
        '                    .SqlParametros.Add("@idPlan", SqlDbType.Int).Value = _idPlan
        '                    .SqlParametros.Add("@material", SqlDbType.VarChar).Value = _material
        '                    .SqlParametros.Add("@precioVentaEquipo", SqlDbType.Money).Value = _precioVentaEquipo
        '                    .SqlParametros.Add("@ivaEquipo", SqlDbType.Money).Value = _ivaEquipo

        '                    .SqlParametros.Add("@result", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
        '                    .EjecutarNonQuery("RegistrarMaterialEnPlanVenta", CommandType.StoredProcedure)
        '                    Dim respuesta As Integer = .SqlParametros("@result").Value
        '                    If respuesta = 0 Then
        '                        resultado.EstablecerMensajeYValor(respuesta, "Material agregados satisfactoriamente al plan.")
        '                    Else
        '                        Select Case respuesta
        '                            Case 1 : resultado.EstablecerMensajeYValor(respuesta, "El plan no se encuentra registrado en el sistema")
        '                            Case 2 : resultado.EstablecerMensajeYValor(respuesta, "El material no se encuentra registrado en el sistema")
        '                            Case 3 : resultado.EstablecerMensajeYValor(respuesta, "El material ya se encuentra asociados al plan de venta")
        '                        End Select
        '                    End If
        '                End With
        '            Else
        '                resultado.EstablecerMensajeYValor(100, "No se han establecido los valores mínimos para vincular el material.")
        '            End If
        '        Catch ex As Exception
        '            Throw ex
        '        End Try
        '    End Using
        '    Return resultado
        'End Function

        'Public Function Eliminar() As ResultadoProceso
        '    Dim resultado As New ResultadoProceso
        '    If _idRegistro > 0 Or _idPlan > 0 Then
        '        Using dbManager As New LMDataAccess
        '            Try
        '                With dbManager
        '                    .SqlParametros.Add("@idRegistro", SqlDbType.BigInt).Value = _idRegistro
        '                    .SqlParametros.Add("@idPlan", SqlDbType.Int).Value = _idPlan
        '                    .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
        '                    .IniciarTransaccion()
        '                    .EjecutarNonQuery("EliminarMaterialDePlanVenta", CommandType.StoredProcedure)
        '                    If Integer.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
        '                        If resultado.Valor = 0 Then
        '                            .ConfirmarTransaccion()
        '                        Else
        '                            Select Case resultado.Valor
        '                                Case 1
        '                                    resultado.Mensaje = "No se ha proporcionado información correspondiente al Material a desvincular"
        '                                Case Else
        '                                    resultado.Mensaje = "Ocurrió un error inesperado al tratar de desvincular el Material"
        '                            End Select
        '                            .AbortarTransaccion()
        '                        End If
        '                    Else
        '                        Throw New Exception("Imposible evaluar la respuesta del servidor.")
        '                    End If
        '                End With
        '            Catch ex As Exception
        '                Throw ex
        '            End Try
        '        End Using
        '    Else
        '        resultado.EstablecerMensajeYValor(10, "No se han proporcionado los datos mínimos para reconocer el registro a eliminar.")
        '    End If
        '    Return resultado
        'End Function

#End Region

    End Class

End Namespace