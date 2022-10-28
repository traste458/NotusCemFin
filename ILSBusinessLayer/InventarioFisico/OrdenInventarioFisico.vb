Imports LMDataAccessLayer

Namespace InventarioFisico
    Public Class OrdenInventarioFisico

#Region "Atributos"

        Protected _usuario As String
        Protected _fechaCierre As Date
        Protected _estado As String
        Protected _auditor As String

#End Region

#Region "Propiedades"

        Public Property IdInventario As Long
        Public Property IdUsuario As Integer
        Public Property Linea As Integer
        Public Property FechaCreacion As Date
        Public Property IdEstado As Integer
        Public Property IdAuditor As Integer

        Public Property UltimoSerialLeido As String
        Public Property CantidadLeida As Long
        Public Property Material As String
        Public Property Referencia As String

        Public Property Registrado As Boolean

        Public ReadOnly Property Usuario As String
            Get
                Return _usuario
            End Get
        End Property

        Public ReadOnly Property FechaCierre As Date
            Get
                Return _fechaCierre
            End Get
        End Property

        Public ReadOnly Property Estado As String
            Get
                Return _estado
            End Get
        End Property

        Public ReadOnly Property Auditor As String
            Get
                Return _auditor
            End Get
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            Me._Registrado = False
        End Sub

        Public Sub New(ByVal idUsuario As Integer)
            Me.New()
            Me._IdUsuario = idUsuario
            CargarInformacion()
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Anular(ByVal codigoAuditor As String) As ResultadoProceso
            Dim resultado As New ResultadoProceso(-1, "No se ha evaluado el resultado del proceso")
            If Me._IdInventario > 0 AndAlso Me._IdUsuario > 0 AndAlso Not EsNuloOVacio(codigoAuditor) Then
                Using dbManager As New LMDataAccess
                    Try
                        With dbManager
                            .SqlParametros.Add("@idInventario", SqlDbType.BigInt).Value = Me._IdInventario
                            .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me._IdUsuario
                            .SqlParametros.Add("@codigoAuditor", SqlDbType.VarChar, 50).Value = codigoAuditor
                            .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                            .TiempoEsperaComando = 0
                            .IniciarTransaccion()
                            .EjecutarNonQuery("AnularOrdenInventarioFisico", CommandType.StoredProcedure)
                            If Integer.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                                resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                                If resultado.Valor = 0 Then
                                    .ConfirmarTransaccion()
                                Else
                                    .AbortarTransaccion()
                                End If
                            Else
                                resultado.EstablecerMensajeYValor(300, "Imposible evaluar el resultado de la anulación. La transacción ha sido abortada. Por favor intente nuevamente")
                                .AbortarTransaccion()
                            End If
                        End With
                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                        Throw ex
                    End Try
                End Using
            Else
                resultado.EstablecerMensajeYValor(200, "No se han proporcionado todos los valores requeridos para anular la orden. " & _
                                      "Id Inventario: " & Me._IdInventario.ToString & _
                                      "Id Usuario: " & Me._IdUsuario.ToString & _
                                      "Código Auditor: " & codigoAuditor.Trim)
            End If

            Return resultado
        End Function

        Public Function Cerrar(ByVal codigoAuditor As String) As ResultadoProceso
            Dim resultado As New ResultadoProceso(-1, "No se ha evaluado el resultado del proceso")
            If Me._IdInventario > 0 AndAlso Me._IdUsuario > 0 AndAlso Not EsNuloOVacio(codigoAuditor) Then
                Using dbManager As New LMDataAccess
                    Try
                        With dbManager
                            .SqlParametros.Add("@idInventario", SqlDbType.BigInt).Value = Me._IdInventario
                            .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me._IdUsuario
                            .SqlParametros.Add("@codigoAuditor", SqlDbType.VarChar, 50).Value = codigoAuditor
                            .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                            .TiempoEsperaComando = 0
                            .IniciarTransaccion()
                            .EjecutarNonQuery("CerrarOrdenInventario", CommandType.StoredProcedure)
                            If Integer.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                                resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                                If resultado.Valor = 0 Then
                                    .ConfirmarTransaccion()
                                Else
                                    .AbortarTransaccion()
                                End If
                            Else
                                resultado.EstablecerMensajeYValor(300, "Imposible evaluar el resultado del cierre de inventario. La transacción ha sido abortada. Por favor intente nuevamente")
                                .AbortarTransaccion()
                            End If
                        End With
                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                        Throw ex
                    End Try
                End Using
            Else
                resultado.EstablecerMensajeYValor(200, "No se han proporcionado todos los valores requeridos para cerrar la orden. " & _
                                      "Id Inventario: " & Me._IdInventario.ToString & _
                                      "Id Usuario: " & Me._IdUsuario.ToString & _
                                      "Código Auditor: " & codigoAuditor.Trim)
            End If

            Return resultado
        End Function

        Public Function RegistrarUnicoSerial(ByVal serial As String) As ResultadoProceso
            Dim resultado As New ResultadoProceso(-1, "No se ha evaluado el resultado del proceso")
            If Me.IdUsuario > 0 AndAlso Not EsNuloOVacio(serial) Then
                Using dbManager As New LMDataAccess
                    Try
                        With dbManager
                            .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me.IdUsuario
                            .SqlParametros.Add("@serial", SqlDbType.VarChar, 50).Value = serial
                            .SqlParametros.Add("@idInventario", SqlDbType.BigInt).Direction = ParameterDirection.InputOutput
                            .SqlParametros.Add("@cantidadLeida", SqlDbType.BigInt).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@ultimoSerial", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@material", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@referencia", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                            If Me._IdInventario > 0 Then .SqlParametros("@idInventario").Value = Me._IdInventario

                            .IniciarTransaccion()
                            .EjecutarNonQuery("LeerSerialSueltoEnInventario", CommandType.StoredProcedure)

                            If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                                resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString

                                If resultado.Valor = 0 Then
                                    .ConfirmarTransaccion()
                                    Long.TryParse(.SqlParametros("@idInventario").Value.ToString, Me._IdInventario)
                                    Me._UltimoSerialLeido = .SqlParametros("@ultimoSerial").Value.ToString
                                    Integer.TryParse(.SqlParametros("@cantidadLeida").Value.ToString, Me._CantidadLeida)
                                    Me._Material = .SqlParametros("@material").Value.ToString
                                    Me._Referencia = .SqlParametros("@referencia").Value.ToString
                                    Me._Registrado = True
                                Else
                                    .AbortarTransaccion()
                                End If
                            Else
                                .AbortarTransaccion()
                                resultado.EstablecerMensajeYValor(300, "Imposible evaluar el resultado de la lectura. Por favor intente nuevamente")
                            End If
                        End With
                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                        Throw ex
                    End Try
                End Using
            Else
                resultado.EstablecerMensajeYValor(200, "No se han proporcionado todos los valores requeridos para realizar la lectura. " & _
                                      "Id Inventario: " & Me._IdInventario.ToString & _
                                      "Serial: " & serial.Trim)
            End If

            Return resultado
        End Function

        Public Function RegistrarSerialSobrante(ByVal serial As String, ByVal material As String, ByVal idRegion As Short) As ResultadoProceso
            Dim resultado As New ResultadoProceso(-1, "No se ha evaluado el resultado del proceso")
            If Me.IdUsuario > 0 AndAlso Not EsNuloOVacio(serial) AndAlso Not EsNuloOVacio(material) AndAlso idRegion > 0 Then
                Using dbManager As New LMDataAccess
                    Try
                        With dbManager
                            .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me.IdUsuario
                            .SqlParametros.Add("@serial", SqlDbType.VarChar, 50).Value = serial
                            .SqlParametros.AddWithValue("@material", material).Direction = ParameterDirection.InputOutput
                            .SqlParametros.Add("@idRegion", SqlDbType.SmallInt).Value = idRegion
                            .SqlParametros.Add("@idInventario", SqlDbType.BigInt).Direction = ParameterDirection.InputOutput
                            .SqlParametros.Add("@cantidadLeida", SqlDbType.BigInt).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@ultimoSerial", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@referencia", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                            If Me._IdInventario > 0 Then .SqlParametros("@idInventario").Value = Me._IdInventario

                            .IniciarTransaccion()
                            .EjecutarNonQuery("LeerSerialSobranteEnInventario", CommandType.StoredProcedure)

                            If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                                resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString

                                If resultado.Valor = 0 Then
                                    .ConfirmarTransaccion()
                                    Long.TryParse(.SqlParametros("@idInventario").Value.ToString, Me._IdInventario)
                                    Me._UltimoSerialLeido = .SqlParametros("@ultimoSerial").Value.ToString
                                    Integer.TryParse(.SqlParametros("@cantidadLeida").Value.ToString, Me._CantidadLeida)
                                    Me._Material = .SqlParametros("@material").Value.ToString
                                    Me._Referencia = .SqlParametros("@referencia").Value.ToString
                                    Me._Registrado = True
                                Else
                                    .AbortarTransaccion()
                                End If
                            Else
                                .AbortarTransaccion()
                                resultado.EstablecerMensajeYValor(300, "Imposible evaluar el resultado del registro. Por favor intente nuevamente")
                            End If
                        End With
                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                        Throw ex
                    End Try
                End Using
            Else
                resultado.EstablecerMensajeYValor(200, "No se han proporcionado todos los valores requeridos para realizar la lectura. " & _
                                      "Id Inventario: " & Me._IdInventario.ToString & ", " & _
                                      "Serial: " & serial.Trim & ", " & _
                                      "Material: " & material.Trim & ", " & _
                                      "Id Region: " & idRegion.ToString)
            End If

            Return resultado
        End Function

        Public Function RegistrarOtb(ByVal idOtb As Long) As ResultadoProceso
            Dim resultado As New ResultadoProceso(-1, "No se ha evaluado el resultado del proceso")
            If Me.IdUsuario > 0 AndAlso idOtb > 0 Then
                Using dbManager As New LMDataAccess
                    Try
                        With dbManager
                            .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me.IdUsuario
                            .SqlParametros.Add("@idOtb", SqlDbType.BigInt).Value = idOtb
                            .SqlParametros.Add("@idInventario", SqlDbType.BigInt).Direction = ParameterDirection.InputOutput
                            .SqlParametros.Add("@cantidadLeida", SqlDbType.BigInt).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@ultimoSerial", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@material", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@referencia", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                            If Me._IdInventario > 0 Then .SqlParametros("@idInventario").Value = Me._IdInventario

                            .IniciarTransaccion()
                            .EjecutarNonQuery("LeerOtbSerializadaEnInventarioFisico", CommandType.StoredProcedure)

                            If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                                resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString

                                If resultado.Valor = 0 Then
                                    .ConfirmarTransaccion()
                                    Long.TryParse(.SqlParametros("@idInventario").Value.ToString, Me._IdInventario)
                                    Me._UltimoSerialLeido = .SqlParametros("@ultimoSerial").Value.ToString
                                    Integer.TryParse(.SqlParametros("@cantidadLeida").Value.ToString, Me._CantidadLeida)
                                    Me._Material = .SqlParametros("@material").Value.ToString
                                    Me._Referencia = .SqlParametros("@referencia").Value.ToString
                                    Me._Registrado = True
                                Else
                                    .AbortarTransaccion()
                                End If
                            Else
                                .AbortarTransaccion()
                                resultado.EstablecerMensajeYValor(300, "Imposible evaluar el resultado de la lectura. Por favor intente nuevamente")
                            End If
                        End With
                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                        Throw ex
                    End Try
                End Using
            Else
                resultado.EstablecerMensajeYValor(200, "No se han proporcionado todos los valores requeridos para realizar la lectura. " & _
                                      "Id Inventario: " & Me._IdInventario.ToString & ", " & _
                                      "Id OTB: " & idOtb.ToString)
            End If

            Return resultado
        End Function

        Public Function RegistrarRangoDeSeriales(ByVal serialInicial As String, ByVal serialFinal As String) As ResultadoProceso
            Dim resultado As New ResultadoProceso(-1, "No se ha evaluado el resultado del proceso")
            If Me.IdUsuario > 0 AndAlso Not EsNuloOVacio(serialInicial) AndAlso Not EsNuloOVacio(serialFinal) Then
                Using dbManager As New LMDataAccess
                    Try
                        With dbManager
                            .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me.IdUsuario
                            .SqlParametros.Add("@serialInicial", SqlDbType.VarChar, 50).Value = serialInicial.Trim
                            .SqlParametros.Add("@serialFinal", SqlDbType.VarChar, 50).Value = serialFinal.Trim
                            .SqlParametros.Add("@idInventario", SqlDbType.BigInt).Direction = ParameterDirection.InputOutput
                            .SqlParametros.Add("@cantidadLeida", SqlDbType.BigInt).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@ultimoSerial", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@material", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@referencia", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                            If Me._IdInventario > 0 Then .SqlParametros("@idInventario").Value = Me._IdInventario

                            .IniciarTransaccion()
                            .EjecutarNonQuery("LeerRangoDeSerialesEnInventarioFisico", CommandType.StoredProcedure)

                            If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                                resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString

                                If resultado.Valor = 0 Then
                                    .ConfirmarTransaccion()
                                    Long.TryParse(.SqlParametros("@idInventario").Value.ToString, Me._IdInventario)
                                    Me._UltimoSerialLeido = .SqlParametros("@ultimoSerial").Value.ToString
                                    Integer.TryParse(.SqlParametros("@cantidadLeida").Value.ToString, Me._CantidadLeida)
                                    Me._Material = .SqlParametros("@material").Value.ToString
                                    Me._Referencia = .SqlParametros("@referencia").Value.ToString
                                    Me._Registrado = True
                                Else
                                    .AbortarTransaccion()
                                End If
                            Else
                                .AbortarTransaccion()
                                resultado.EstablecerMensajeYValor(300, "Imposible evaluar el resultado de la lectura. Por favor intente nuevamente")
                            End If
                        End With
                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                        Throw ex
                    End Try
                End Using
            Else
                resultado.EstablecerMensajeYValor(200, "No se han proporcionado todos los valores requeridos para realizar la lectura. " & _
                                      "Id Inventario: " & Me._IdInventario.ToString & ", " & _
                                      "Serial Inicial: " & serialInicial & ", " & _
                                      "Serial Final: " & serialFinal)
            End If

            Return resultado
        End Function

        Public Function RegistrarDupla(ByVal esn As String, ByVal sim As String) As ResultadoProceso
            Dim resultado As New ResultadoProceso(-1, "No se ha evaluado el resultado del proceso")
            If Me.IdUsuario > 0 AndAlso Not EsNuloOVacio(esn) AndAlso Not EsNuloOVacio(sim) Then
                Using dbManager As New LMDataAccess
                    Try
                        With dbManager
                            .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me.IdUsuario
                            .SqlParametros.Add("@esn", SqlDbType.VarChar, 50).Value = esn.Trim
                            .SqlParametros.Add("@sim", SqlDbType.VarChar, 50).Value = sim.Trim
                            .SqlParametros.Add("@idInventario", SqlDbType.BigInt).Direction = ParameterDirection.InputOutput
                            .SqlParametros.Add("@cantidadLeida", SqlDbType.BigInt).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@ultimoSerial", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@material", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@referencia", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                            If Me._IdInventario > 0 Then .SqlParametros("@idInventario").Value = Me._IdInventario

                            .IniciarTransaccion()
                            .EjecutarNonQuery("LeerDuplaDeSerialesEnInventarioFisico", CommandType.StoredProcedure)

                            If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                                resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString

                                If resultado.Valor = 0 Then
                                    .ConfirmarTransaccion()
                                    Long.TryParse(.SqlParametros("@idInventario").Value.ToString, Me._IdInventario)
                                    Me._UltimoSerialLeido = .SqlParametros("@ultimoSerial").Value.ToString
                                    Integer.TryParse(.SqlParametros("@cantidadLeida").Value.ToString, Me._CantidadLeida)
                                    Me._Material = .SqlParametros("@material").Value.ToString
                                    Me._Referencia = .SqlParametros("@referencia").Value.ToString
                                    Me._Registrado = True
                                Else
                                    .AbortarTransaccion()
                                End If
                            Else
                                .AbortarTransaccion()
                                resultado.EstablecerMensajeYValor(300, "Imposible evaluar el resultado de la lectura. Por favor intente nuevamente")
                            End If
                        End With
                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                        Throw ex
                    End Try
                End Using
            Else
                resultado.EstablecerMensajeYValor(200, "No se han proporcionado todos los valores requeridos para realizar la lectura. " & _
                                      "Id Inventario: " & Me._IdInventario.ToString & ", " & _
                                      "ESN: " & esn & ", " & _
                                      "SIM: " & sim)
            End If

            Return resultado
        End Function

#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            Using dbManager As New LMDataAccess
                With dbManager
                    If Me._IdInventario > 0 Then
                        .SqlParametros.Add("@idInventario", SqlDbType.BigInt).Value = Me._IdInventario
                    ElseIf Me._IdUsuario > 0 Then
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me.IdUsuario
                        .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = EstadoOrdenInventario.Abierta
                    Else
                        Return
                    End If

                    .ejecutarReader("ObtenerInfoOrdenInventarioFisico", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Long.TryParse(.Reader("idInventario").ToString, Me._IdInventario)
                            Integer.TryParse(.Reader("idUsuario").ToString, Me._IdUsuario)
                            Me._usuario = .Reader("usuario").ToString
                            Integer.TryParse(.Reader("linea").ToString, Me._Linea)
                            Date.TryParse(.Reader("fechaCreacion").ToString, Me._FechaCreacion)
                            Integer.TryParse(.Reader("idEstado").ToString, Me._IdEstado)
                            Me._estado = .Reader("estado").ToString
                            Integer.TryParse(.Reader("idAuditor").ToString, Me._IdAuditor)
                            Me._auditor = .Reader("auditor").ToString
                            If Not IsDBNull(.Reader("fechaCierre")) Then _
                                Date.TryParse(.Reader("fechaCierre").ToString, Me._fechaCierre)
                            Integer.TryParse(.Reader("cantidad"), Me.CantidadLeida)
                            Me.UltimoSerialLeido = .Reader("ultimoSerial")
                            Me.Material = .Reader("material").ToString
                            Me.Referencia = .Reader("referencia").ToString

                            Me.Registrado = True
                        End If
                    End If
                End With
            End Using
        End Sub

#End Region

        Public Enum EstadoOrdenInventario
            Abierta = 62
            Cerrada = 63
            Anulada = 93
        End Enum

    End Class

End Namespace