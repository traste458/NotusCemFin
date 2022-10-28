Imports LMDataAccessLayer
Imports System.String

Namespace MensajeriaEspecializada

    Public Class Documento

#Region "Atributos (Campos)"

        Private _idDocumento As Short
        Private _nombre As String
        Private _observacion As String
        Private _activo As Nullable(Of Boolean)
        Private _recibo As Nullable(Of Boolean)
        Private _entrega As Nullable(Of Boolean)

        Private _listUnidadesNegocio As List(Of Integer)
        Private _listTipoServicio As List(Of Integer)

#End Region

#Region "Propiedades"

        Public Property IdDocumento As Short
            Get
                Return _idDocumento
            End Get
            Set(value As Short)
                _idDocumento = value
            End Set
        End Property

        Public Property Nombre As String
            Get
                Return _nombre
            End Get
            Set(value As String)
                _nombre = value
            End Set
        End Property

        Public Property Observacion As String
            Get
                Return _observacion
            End Get
            Set(value As String)
                _observacion = value
            End Set
        End Property

        Public Property Activo As Boolean
            Get
                Return _activo
            End Get
            Set(value As Boolean)
                _activo = value
            End Set
        End Property

        Public Property Recibo As Boolean
            Get
                Return _recibo
            End Get
            Set(value As Boolean)
                _recibo = value
            End Set
        End Property

        Public Property Entrega As Boolean
            Get
                Return _entrega
            End Get
            Set(value As Boolean)
                _entrega = value
            End Set
        End Property

        Public Property ListaUnidadesNegocio As List(Of Integer)
            Get
                Return _listUnidadesNegocio
            End Get
            Set(value As List(Of Integer))
                _listUnidadesNegocio = value
            End Set
        End Property

        Public Property ListaTipoServicio As List(Of Integer)
            Get
                Return _listTipoServicio
            End Get
            Set(value As List(Of Integer))
                _listTipoServicio = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idDocumento As Short)
            MyBase.New()
            _idDocumento = idDocumento
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        If _idDocumento > 0 Then .SqlParametros.Add("@idDocumento", SqlDbType.SmallInt).Value = _idDocumento
                        If Not String.IsNullOrEmpty(_nombre) Then .SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = _nombre
                        If _activo IsNot Nothing Then .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo

                        .ejecutarReader("ObtieneDocumentos", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                Integer.TryParse(.Reader("idDocumento").ToString, _idDocumento)
                                _nombre = .Reader("nombre").ToString
                                _observacion = .Reader("observacion").ToString
                                _activo = .Reader("activo")
                                _recibo = .Reader("recibo")
                                _entrega = .Reader("entrega")
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

#Region "Métodos Públicos"

        Public Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Using dbManager As New LMDataAccess
                Try
                    If Not String.IsNullOrEmpty(_nombre) Then
                        With dbManager
                            .SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = _nombre
                            If Not String.IsNullOrEmpty(_observacion) Then .SqlParametros.Add("@observacion", SqlDbType.VarChar).Value = _observacion
                            .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo
                            .SqlParametros.Add("@recibo", SqlDbType.Bit).Value = _recibo
                            .SqlParametros.Add("@entrega", SqlDbType.Bit).Value = _entrega

                            If _listUnidadesNegocio IsNot Nothing AndAlso _listUnidadesNegocio.Count > 0 Then _
                            .SqlParametros.Add("@listaUnidadNegocio", SqlDbType.VarChar).Value = Join(",", _listUnidadesNegocio.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                            If _listTipoServicio IsNot Nothing AndAlso _listTipoServicio.Count > 0 Then _
                                .SqlParametros.Add("@listaTipoServicio", SqlDbType.VarChar).Value = Join(",", _listTipoServicio.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())

                            .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                            .iniciarTransaccion()
                            .ejecutarNonQuery("RegistrarDocumento", CommandType.StoredProcedure)

                            Dim respuesta As Integer = .SqlParametros("@respuesta").Value
                            If respuesta = 0 Then
                                .confirmarTransaccion()
                            Else
                                Select Case respuesta
                                    Case 1 : resultado.EstablecerMensajeYValor(respuesta, "El nombre del documento ya se encuentra registrado")
                                End Select
                                .abortarTransaccion()
                            End If
                        End With
                    Else
                        resultado.EstablecerMensajeYValor(100, "No se proporcionaron los datos suficientes para realizar el registro.")
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return resultado
        End Function

        Public Function Actualizar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Using dbManager As New LMDataAccess
                Try
                    If _idDocumento > 0 And Not String.IsNullOrEmpty(_nombre) Then
                        With dbManager
                            .SqlParametros.Add("@idDocumento", SqlDbType.SmallInt).Value = _idDocumento
                            .SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = _nombre
                            If Not String.IsNullOrEmpty(_observacion) Then .SqlParametros.Add("@observacion", SqlDbType.VarChar).Value = _observacion
                            .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo
                            .SqlParametros.Add("@recibo", SqlDbType.Bit).Value = _recibo
                            .SqlParametros.Add("@entrega", SqlDbType.Bit).Value = _entrega

                            If _listUnidadesNegocio IsNot Nothing AndAlso _listUnidadesNegocio.Count > 0 Then _
                            .SqlParametros.Add("@listaUnidadNegocio", SqlDbType.VarChar).Value = Join(",", _listUnidadesNegocio.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())
                            If _listTipoServicio IsNot Nothing AndAlso _listTipoServicio.Count > 0 Then _
                                .SqlParametros.Add("@listaTipoServicio", SqlDbType.VarChar).Value = Join(",", _listTipoServicio.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())

                            .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                            .iniciarTransaccion()
                            .ejecutarNonQuery("ActualizarDocumento", CommandType.StoredProcedure)

                            Dim respuesta As Integer = .SqlParametros("@respuesta").Value
                            If respuesta = 0 Then
                                .confirmarTransaccion()
                            Else
                                .abortarTransaccion()
                            End If
                        End With
                    Else
                        resultado.EstablecerMensajeYValor(100, "No se proporcionaron los datos suficientes para actualizar el registro.")
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return resultado
        End Function

#End Region

    End Class

End Namespace
