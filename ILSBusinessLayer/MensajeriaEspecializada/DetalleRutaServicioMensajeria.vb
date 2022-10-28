Imports ILSBusinessLayer
Imports LMDataAccessLayer

Namespace MensajeriaEspecializada

    Public Class DetalleRutaServicioMensajeria

#Region "Atributos"

        Private _idDetalle As Integer
        Private _idRuta As Integer
        Private _idServicio As Integer
        Private _idUsuarioLog As Integer
        Private _secuencia As Integer

#End Region

#Region "Propiedades"

        Public Property IdDetalle() As Integer
            Get
                Return _idDetalle
            End Get
            Set(ByVal value As Integer)
                _idDetalle = value
            End Set
        End Property

        Public Property IdRuta() As Integer
            Get
                Return _idRuta
            End Get
            Set(ByVal value As Integer)
                _idRuta = value
            End Set
        End Property

        Public Property IdServicio() As Integer
            Get
                Return _idServicio
            End Get
            Set(ByVal value As Integer)
                _idServicio = value
            End Set
        End Property

        Public Property IdUsuarioLog() As Integer
            Get
                Return _idUsuarioLog
            End Get
            Set(ByVal value As Integer)
                _idUsuarioLog = value
            End Set
        End Property

        Public Property Secuencia() As Integer
            Get
                Return _secuencia
            End Get
            Set(ByVal value As Integer)
                _secuencia = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idDetalle As Integer)
            MyBase.New()
            _idDetalle = idDetalle
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        If _idDetalle > 0 Then .SqlParametros.Add("@idDetalle", SqlDbType.Int).Value = _idDetalle
                        If _idRuta > 0 Then .SqlParametros.Add("@idRuta", SqlDbType.Int).Value = _idRuta
                        If _idServicio > 0 Then .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicio

                        .ejecutarReader("ObtenerDetalleRutaServicioMensajeria", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing And .Reader.HasRows Then
                            If .Reader.Read() Then
                                _idDetalle = CInt(.Reader("idDetalle").ToString())
                                _idRuta = CInt(.Reader("idRuta").ToString())
                                _idServicio = CInt(.Reader("idServicio").ToString())
                                _idUsuarioLog = CInt(.Reader("idUsuarioLog").ToString())
                                _secuencia = CInt(.Reader("secuencia").ToString())
                            End If
                            .Reader.Close()
                        End If

                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
        End Sub

#End Region

#Region "Funciones Públicas"

        Public Function ObtenerDatos() As DataTable
            Dim dtReturn As New DataTable

            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        If _idDetalle > 0 Then .SqlParametros.Add("@idDetalle", SqlDbType.Int).Value = _idDetalle
                        If _idRuta > 0 Then .SqlParametros.Add("@idRuta", SqlDbType.Int).Value = _idRuta
                        If _idServicio > 0 Then .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicio
                        .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = Enumerados.RutaMensajeria.Creada

                        dtReturn = .ejecutarDataTable("ObtenerDetalleRutaServicioMensajeria", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return dtReturn
        End Function

        Public Function ObtenerDatosEstado(ByVal pEstado As Enumerados.RutaMensajeria) As DataTable
            Dim dtReturn As New DataTable

            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        If _idDetalle > 0 Then .SqlParametros.Add("@idDetalle", SqlDbType.Int).Value = _idDetalle
                        If _idRuta > 0 Then .SqlParametros.Add("@idRuta", SqlDbType.Int).Value = _idRuta
                        If _idServicio > 0 Then .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicio
                        .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = pEstado

                        dtReturn = .EjecutarDataTable("ObtenerDetalleRutaServicioMensajeria", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return dtReturn
        End Function

        Public Function Desvincular(ByVal idUsuario As Integer) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim idResultado As Integer = -1

            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idRuta", SqlDbType.Int).Value = _idRuta
                        .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicio
                        .SqlParametros.Add("@idDetalle", SqlDbType.Int).Value = _idDetalle
                        .SqlParametros.Add("@secuencia", SqlDbType.Int).Value = _secuencia
                        .SqlParametros.Add("@idUsuarioLog", SqlDbType.Int).Value = idUsuario

                        .SqlParametros.Add("@return", SqlDbType.Int).Direction = ParameterDirection.Output

                        .iniciarTransaccion()
                        .ejecutarNonQuery("EliminarServicioRutaMensajeria", CommandType.StoredProcedure)

                        Integer.TryParse(.SqlParametros("@return").Value, idResultado)
                        If idResultado = 0 Then
                            .confirmarTransaccion()
                            resultado.EstablecerMensajeYValor(0, "Proceso exitoso.")
                        Else
                            .abortarTransaccion()
                            resultado.EstablecerMensajeYValor(1, "No se encontro el ítem para desvincular.")
                        End If
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return resultado
        End Function

        Public Function Adicionar(ByVal radicado As Long, ByVal idRuta As Integer, ByVal idUsuario As Integer) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim idResultado As Integer = -1

            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@radicado", SqlDbType.BigInt).Value = radicado
                        .SqlParametros.Add("@idRuta", SqlDbType.Int).Value = idRuta
                        .SqlParametros.Add("@idUsuarioLog", SqlDbType.Int).Value = idUsuario

                        .SqlParametros.Add("@return", SqlDbType.Int).Direction = ParameterDirection.Output

                        .iniciarTransaccion()
                        .ejecutarNonQuery("AdicionarServicioRutaMensajeria", CommandType.StoredProcedure)

                        Integer.TryParse(.SqlParametros("@return").Value, idResultado)
                        If idResultado = 0 Then
                            .confirmarTransaccion()
                            resultado.EstablecerMensajeYValor(0, "Proceso exitoso.")
                        Else
                            .abortarTransaccion()
                            Select Case idResultado
                                Case 1
                                    resultado.EstablecerMensajeYValor(1, "No se encontró el radicado en estado disponible para adicionarlo a la Ruta.")
                                Case 2
                                    resultado.EstablecerMensajeYValor(2, "El radicado se encuentra asignado a otra Ruta.")
                            End Select
                        End If
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using

            Return resultado
        End Function

        Public Function MoverSecuecia(ByVal idDetalle As Long, ByVal movimiento As Enumerados.MovimientoSecuencia) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim idResultado As Integer = -1

            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idDetalle", SqlDbType.BigInt).Value = idDetalle
                        .SqlParametros.Add("@movimiento", SqlDbType.Int).Value = movimiento

                        .SqlParametros.Add("@return", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                        .iniciarTransaccion()
                        .ejecutarNonQuery("MoverSecuenciaRutaServicioMensajeria", CommandType.StoredProcedure)

                        Integer.TryParse(.SqlParametros("@return").Value, idResultado)
                        If idResultado = 0 Then
                            .confirmarTransaccion()
                            resultado.EstablecerMensajeYValor(0, "Proceso exitoso.")
                        Else
                            .abortarTransaccion()
                            resultado.EstablecerMensajeYValor(1, "Imposible realizar operación.")
                        End If
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using

            Return resultado
        End Function

#End Region

    End Class

End Namespace

