Imports LMDataAccessLayer

Namespace InventarioFisico

    Public Class Alerta

#Region "Atributos"

        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public Property IdAlerta As Long
        Public Property IdUsuario As Integer
        Public Property Bodega As String
        Public Property Linea As Integer
        Public Property IdEstado As Integer
        Public Property Descripcion As String
        Public Property FechaRegistro As Date
        Public Property FechaFinalizacion As Date
        Public Property Registrado As Boolean
            Get
                Return _registrado
            End Get
            Protected Friend Set(value As Boolean)
                _registrado = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idAlerta As Long)
            Me.New()
            Me.IdAlerta = idAlerta
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As ResultadoProceso
            Dim respuesta As New ResultadoProceso()
            If Me.IdUsuario > 0 AndAlso Me.Linea > 0 Then
                Using dbManager As New LMDataAccess
                    With dbManager
                        .SqlParametros.Clear()
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me.IdUsuario
                        .SqlParametros.Add("@linea", SqlDbType.Int).Value = Me.Linea
                        .SqlParametros.Add("@idAlerta", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                        .EjecutarNonQuery("RegistrarAlertaInventario", CommandType.StoredProcedure)
                        If (Integer.TryParse(.SqlParametros("@respuesta").Value, respuesta.Valor)) Then
                            respuesta.Mensaje = .SqlParametros("@mensaje").Value.ToString
                            Integer.TryParse(.SqlParametros("@idAlerta").Value.ToString, Me._IdAlerta)
                            If respuesta.Valor = 0 Then Descripcion = respuesta.Mensaje
                        Else
                            respuesta.EstablecerMensajeYValor(1, "No se logro obtener respuesta del servidor, por favor intente nuevamente.")
                        End If
                    End With
                End Using
            Else
                respuesta.EstablecerMensajeYValor(1, "No se establecieron todos los valores necesarios para realizar el registro, por favor verifique.")
            End If
            Return respuesta
        End Function

        Public Function Cerrar() As ResultadoProceso
            Dim respuesta As New ResultadoProceso()
            If Me._IdAlerta > 0 Then
                Using dbManager As New LMDataAccess
                    With dbManager
                        .SqlParametros.Clear()
                        .SqlParametros.Add("@idAlerta", SqlDbType.BigInt).Value = Me.IdAlerta
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .EjecutarNonQuery("CerrarAlertaInventario", CommandType.StoredProcedure)
                        If (Integer.TryParse(.SqlParametros("@respuesta").Value, respuesta.Valor)) Then
                            respuesta.Mensaje = .SqlParametros("@mensaje").Value
                        Else
                            respuesta.EstablecerMensajeYValor(1, "No se logro obtener respuesta del servidor, por favor intente nuevamente.")
                        End If
                    End With
                End Using
            Else
                respuesta.EstablecerMensajeYValor(1, "No se establecieron todos los valores necesarios para realizar el registro, por favor verifique.")
            End If
            Return respuesta
        End Function

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()

            If Me._IdAlerta > 0 Then
                Using dbManager As New LMDataAccess
                    With dbManager
                        .SqlParametros.Add("@idAlerta", SqlDbType.Int).Value = Me._IdAlerta
                        .ejecutarReader("ObtenerInforAlertaInventario", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing AndAlso .Reader.Read Then
                            CargarResultadoConsulta(.Reader)
                            .Reader.Close()
                        End If
                    End With
                End Using
            End If
        End Sub

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing AndAlso reader.HasRows Then
                'Descripcion As String
                'FechaRegistro As Date
                'FechaFinalizacion As Date
                Integer.TryParse(reader("idAlerta").ToString, Me._IdAlerta)
                Integer.TryParse(reader("idUsuario").ToString, Me._IdUsuario)
                Me._Bodega = reader("bodega").ToString
                Integer.TryParse(reader("linea").ToString, Me._Linea)
                Integer.TryParse(reader("idEstado").ToString, Me._IdEstado)
                Me._Descripcion = reader("descripcion").ToString
                If Not IsDBNull(reader("fechaRegistro")) Then Date.TryParse(reader("fechaRegistro").ToString, Me._FechaRegistro)
                If Not IsDBNull(reader("fechaFinalizacion")) Then Date.TryParse(reader("fechaFinalizacion").ToString, Me._FechaFinalizacion)
                _registrado = True
            End If

        End Sub

#End Region

        Public Enum EstadoAlerta
            Registrada = 233
            Finalizada = 234
        End Enum

    End Class

End Namespace
