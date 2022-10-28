Imports LMDataAccessLayer

Namespace InventarioFisico

    Public Class InventarioFisicoBLL

#Region "Propiedades"

        Public Property IdInventario As Long
        Public Property IdUsuario As Integer
        Public Property Linea As Integer
        Public Property FechaCreacion As Date
        Public Property UltimoSerialLeido As String
        Public Property CantidadLeida As Long
        Public Property Material As String
        Public Property Referencia As String
        Public Property FechaCierre As Date
        Public Property IdEstado As Integer
        Public Property Auditor As String

        Public Property Registrado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            Me.Registrado = False
        End Sub

        Public Sub New(ByVal idUsuario As Integer)
            Me.New()
            Me.IdUsuario = idUsuario
            CargarOrdenPendiente()
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Registrar(serial As String) As ResultadoProceso
            Dim respuesta As New ResultadoProceso()
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me.IdUsuario
                        .SqlParametros.Add("@serial", SqlDbType.VarChar).Value = serial
                        .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .SqlParametros.Add("@cantInventario", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@ultimoSerial", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@material", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@referencia", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output

                        If Me._IdInventario > 0 Then
                            .SqlParametros.AddWithValue("@idInventario", Me._IdInventario).Direction = ParameterDirection.InputOutput
                        Else
                            .SqlParametros.Add("@idInventario", SqlDbType.BigInt).Direction = ParameterDirection.InputOutput
                        End If
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output

                        .iniciarTransaccion()
                        .ejecutarNonQuery("RegistrarSerialInventario", CommandType.StoredProcedure)

                        If Integer.TryParse(.SqlParametros("@respuesta").Value, respuesta.Valor) Then
                            respuesta.Mensaje = .SqlParametros("@mensaje").Value

                            If respuesta.Valor = 0 Then
                                Me.IdInventario = .SqlParametros("@idInventario").Value
                                Me.UltimoSerialLeido = .SqlParametros("@ultimoSerial").Value
                                Me.CantidadLeida = .SqlParametros("@cantInventario").Value
                                Me.Material = .SqlParametros("@material").Value
                                Me.Referencia = .SqlParametros("@referencia").Value

                                .confirmarTransaccion()
                                Me.Registrado = True
                            Else
                                .abortarTransaccion()
                            End If
                        Else
                            .abortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception("Método Registrar: " & ex.Message, ex.InnerException)
                End Try
            End Using
            Return respuesta
        End Function

        Function RegistrarRango(serialInicial As String, serialFinal As String) As ResultadoProceso
            Dim respuesta As New ResultadoProceso()
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me.IdUsuario
                        .SqlParametros.Add("@serialInicial", SqlDbType.VarChar).Value = serialInicial
                        .SqlParametros.Add("@serialFinal", SqlDbType.VarChar).Value = serialFinal
                        .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .SqlParametros.Add("@cantInventario", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@ultimoSerial", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@material", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@referencia", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output

                        If Me.IdInventario > 0 Then
                            .SqlParametros.AddWithValue("@idInventario", Me.IdInventario).Direction = ParameterDirection.InputOutput
                        Else
                            .SqlParametros.Add("@idInventario", SqlDbType.BigInt).Direction = ParameterDirection.InputOutput
                        End If
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output

                        .iniciarTransaccion()
                        .ejecutarNonQuery("RegistrarRangoSerialInventario", CommandType.StoredProcedure)

                        If Integer.TryParse(.SqlParametros("@respuesta").Value, respuesta.Valor) Then
                            respuesta.Mensaje = .SqlParametros("@mensaje").Value
                            If respuesta.Valor = 0 Then
                                Me.IdInventario = .SqlParametros("@idInventario").Value
                                Me.UltimoSerialLeido = .SqlParametros("@ultimoSerial").Value
                                Me.CantidadLeida = .SqlParametros("@cantInventario").Value
                                Me.Material = .SqlParametros("@material").Value
                                Me.Referencia = .SqlParametros("@referencia").Value

                                .confirmarTransaccion()
                                Me.Registrado = True
                            Else
                                .abortarTransaccion()
                            End If
                        Else
                            .abortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception("Método RegistrarRango: " & ex.Message, ex.InnerException)
                End Try
            End Using
            Return respuesta
        End Function

        Function RegistrarDupla(serial As String, sim As String) As ResultadoProceso
            Dim respuesta As New ResultadoProceso()
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me.IdUsuario
                        .SqlParametros.Add("@serial", SqlDbType.VarChar).Value = serial
                        .SqlParametros.Add("@sim", SqlDbType.VarChar).Value = sim
                        .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .SqlParametros.Add("@cantInventario", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@ultimoSerial", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@material", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@referencia", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output

                        If Me.IdInventario > 0 Then
                            .SqlParametros.AddWithValue("@idInventario", Me.IdInventario).Direction = ParameterDirection.InputOutput
                        Else
                            .SqlParametros.Add("@idInventario", SqlDbType.BigInt).Direction = ParameterDirection.InputOutput
                        End If
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output

                        .iniciarTransaccion()
                        .ejecutarNonQuery("RegistrarDuplaSerialInventario", CommandType.StoredProcedure)

                        If Integer.TryParse(.SqlParametros("@respuesta").Value, respuesta.Valor) Then
                            respuesta.Mensaje = .SqlParametros("@mensaje").Value
                            If respuesta.Valor = 0 Then
                                Me.IdInventario = .SqlParametros("@idInventario").Value
                                Me.UltimoSerialLeido = .SqlParametros("@ultimoSerial").Value
                                Me.CantidadLeida = .SqlParametros("@cantInventario").Value
                                Me.Material = .SqlParametros("@material").Value
                                Me.Referencia = .SqlParametros("@referencia").Value

                                .confirmarTransaccion()
                                Me.Registrado = True
                            Else
                                .abortarTransaccion()
                            End If
                        Else
                            .abortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception("Método RegistrarRango: " & ex.Message, ex.InnerException)
                End Try
            End Using
            Return respuesta
        End Function

        Function RegistrarOTB(idOTB As Long) As ResultadoProceso
            Dim respuesta As New ResultadoProceso()
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me.IdUsuario
                        .SqlParametros.Add("@idOTB", SqlDbType.BigInt).Value = idOTB
                        .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .SqlParametros.Add("@cantInventario", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@ultimoSerial", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@material", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@referencia", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output

                        If Me.IdInventario > 0 Then
                            .SqlParametros.AddWithValue("@idInventario", Me.IdInventario).Direction = ParameterDirection.InputOutput
                        Else
                            .SqlParametros.Add("@idInventario", SqlDbType.BigInt).Direction = ParameterDirection.InputOutput
                        End If
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output

                        .iniciarTransaccion()
                        .ejecutarNonQuery("RegistrarOTBInventario", CommandType.StoredProcedure)

                        If Integer.TryParse(.SqlParametros("@respuesta").Value, respuesta.Valor) Then
                            respuesta.Mensaje = .SqlParametros("@mensaje").Value
                            If respuesta.Valor = 0 Then
                                Me.IdInventario = .SqlParametros("@idInventario").Value
                                Me.UltimoSerialLeido = .SqlParametros("@ultimoSerial").Value
                                Me.CantidadLeida = .SqlParametros("@cantInventario").Value
                                Me.Material = .SqlParametros("@material").Value
                                Me.Referencia = .SqlParametros("@referencia").Value

                                .confirmarTransaccion()
                                Me.Registrado = True
                            Else
                                .abortarTransaccion()
                            End If
                        Else
                            .abortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception("Método RegistrarOTB: " & ex.Message, ex.InnerException)
                End Try
            End Using
            Return respuesta
        End Function

        Function RegistrarSerialConfirmado(serial As String, material As String, idRegion As Integer) As ResultadoProceso
            Dim respuesta As New ResultadoProceso()
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me.IdUsuario
                        .SqlParametros.Add("@serial", SqlDbType.VarChar).Value = serial
                        .SqlParametros.Add("@materialSerial", SqlDbType.VarChar).Value = material
                        .SqlParametros.Add("@idRegion", SqlDbType.Int).Value = idRegion
                        .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .SqlParametros.Add("@cantInventario", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@ultimoSerial", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@material", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@referencia", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output

                        If Me.IdInventario > 0 Then
                            .SqlParametros.AddWithValue("@idInventario", Me.IdInventario).Direction = ParameterDirection.InputOutput
                        Else
                            .SqlParametros.Add("@idInventario", SqlDbType.BigInt).Direction = ParameterDirection.InputOutput
                        End If
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output

                        .iniciarTransaccion()
                        .ejecutarNonQuery("RegistrarSerialNoExistenteInventario", CommandType.StoredProcedure)

                        If Integer.TryParse(.SqlParametros("@respuesta").Value, respuesta.Valor) Then
                            respuesta.Mensaje = .SqlParametros("@mensaje").Value

                            If respuesta.Valor = 0 Then
                                Me.IdInventario = .SqlParametros("@idInventario").Value
                                Me.UltimoSerialLeido = .SqlParametros("@ultimoSerial").Value
                                Me.CantidadLeida = .SqlParametros("@cantInventario").Value
                                Me.Material = .SqlParametros("@material").Value
                                Me.Referencia = .SqlParametros("@referencia").Value

                                .confirmarTransaccion()
                                Me.Registrado = True
                            Else
                                .abortarTransaccion()
                            End If
                        Else
                            .abortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception("Método RegistrarSerialConfirmado: " & ex.Message, ex.InnerException)
                End Try
            End Using
            Return respuesta
        End Function

        Public Function CerrarOrden(ByVal sAuditor As String) As ResultadoProceso
            Dim respuesta As New ResultadoProceso()
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idInventario", SqlDbType.BigInt).Value = Me.IdInventario
                        .SqlParametros.Add("@claveAuditor", SqlDbType.VarChar).Value = sAuditor
                        .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output

                        .iniciarTransaccion()
                        .ejecutarNonQuery("CerrarOrdenInventario", CommandType.StoredProcedure)

                        If Integer.TryParse(.SqlParametros("@respuesta").Value, respuesta.Valor) Then
                            respuesta.Mensaje = .SqlParametros("@mensaje").Value
                            If respuesta.Valor = 0 Then
                                .confirmarTransaccion()
                            Else
                                .abortarTransaccion()
                            End If
                        Else
                            .abortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception("Método CerrarOrden: " & ex.Message, ex.InnerException)
                End Try
            End Using
            Return respuesta
        End Function

        Public Function ObtieneAuditoresResponsables() As String
            Dim respuesta As String = String.Empty
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idInventario", SqlDbType.BigInt).Value = Me.IdInventario
                        respuesta = .ejecutarScalar("ObtenerInfoAuditoresInventario", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    Throw New Exception("Método ObtieneAuditoresResponsables: " & ex.Message, ex.InnerException)
                End Try
            End Using
            Return respuesta
        End Function

#End Region

#Region "Métodos Privados"

        Private Sub CargarOrdenPendiente()
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me.IdUsuario
                        .ejecutarReader("ObtenerOrdenInventarioFisicoPendiente", CommandType.StoredProcedure)

                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                Long.TryParse(.Reader("idInventario"), Me.IdInventario)
                                Me.UltimoSerialLeido = .Reader("ultimoSerial")
                                Integer.TryParse(.Reader("cantidad"), Me.CantidadLeida)
                                Me.Material = .Reader("material").ToString
                                Me.Referencia = .Reader("referencia").ToString

                                Me.Registrado = True
                            End If
                        End If
                    End With
                Catch ex As Exception
                    Throw New Exception("Método CargarOrdenPendiente: " & ex.Message, ex.InnerException)
                End Try
            End Using
        End Sub

#End Region

    End Class

End Namespace
