Imports LMDataAccessLayer
Imports System.String
Imports System.Web

Namespace MensajeriaEspecializada

    Public Class PaqueteVenta

#Region "Atributos (Campos)"

        Private _idPaquete As Integer
        Private _nombre As String
        Private _activo As Nullable(Of Boolean)
        Private _observacion As String

#End Region

#Region "Propiedades"

        Public Property IdPaquete As Integer
            Get
                Return _idPaquete
            End Get
            Set(value As Integer)
                _idPaquete = value
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

        Public Property Activo As Boolean
            Get
                Return _activo
            End Get
            Set(value As Boolean)
                _activo = value
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

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idPaquete As Integer)
            MyBase.New()
            _idPaquete = idPaquete
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            With dbManager
                If _idPaquete > 0 Then .SqlParametros.Add("@idPaquete", SqlDbType.VarChar).Value = _idPaquete
                If Not String.IsNullOrEmpty(_nombre) Then .SqlParametros.Add("@nombrePaquete", SqlDbType.VarChar).Value = _nombre
                If _activo IsNot Nothing Then .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo

                .ejecutarReader("ObtienePaquetesVenta", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    If .Reader.Read Then
                        CargarResultadoConsulta(.Reader)
                    End If
                    .Reader.Close()
                End If
            End With
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@nombre", SqlDbType.VarChar, 2000).Value = _nombre
                        If Not String.IsNullOrEmpty(_observacion) Then .Add("@observacion", SqlDbType.VarChar, 2000).Value = _observacion
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .IniciarTransaccion()
                    .EjecutarNonQuery("RegistraPaqueteVenta", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        If resultado.Valor = 0 Then
                            .ConfirmarTransaccion()
                        Else
                            .AbortarTransaccion()
                        End If
                    Else
                        .AbortarTransaccion()
                        resultado.EstablecerMensajeYValor(400, "No se logró establecer la respuesta del servidor, por favor intentelo nuevamente. ")
                    End If

                End With
            Catch ex As Exception
                If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                resultado.EstablecerMensajeYValor(500, "Se generó un error al realizar el registro: " & ex.Message)
            End Try
            Return resultado
        End Function

        Public Function Actualizar(ByVal idUsuario As Integer) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idPaquete", SqlDbType.Int).Value = _idPaquete
                        .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        If Not String.IsNullOrEmpty(_nombre) Then .Add("@nombre", SqlDbType.VarChar, 2000).Value = _nombre
                        If _activo IsNot Nothing Then .Add("@activo", SqlDbType.Bit).Value = _activo
                        If Not String.IsNullOrEmpty(_observacion) Then .Add("observacion", SqlDbType.VarChar, 2000).Value = _observacion
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .IniciarTransaccion()
                    .EjecutarNonQuery("ActualizarPaqueteVenta", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        If resultado.Valor = 0 Then
                            .ConfirmarTransaccion()
                        Else
                            .AbortarTransaccion()
                        End If
                    Else
                        .AbortarTransaccion()
                        resultado.EstablecerMensajeYValor(400, "No se logró establecer la respuesta del servidor, por favor intentelo nuevamente. ")
                    End If
                End With
            Catch ex As Exception
                If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                resultado.EstablecerMensajeYValor(500, "Se generó un error al actualizar el registro: " & ex.Message)
            End Try
            Return resultado
        End Function

#End Region

#Region "Métodos Protegidos"

        Protected Friend Overridable Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Integer.TryParse(reader("idPaquete").ToString, _idPaquete)
                    _nombre = reader("nombrePaquete").ToString
                    _activo = reader("activo")
                    If Not IsDBNull(reader("observacion")) Then _observacion = reader("observacion").ToString
                End If
            End If

        End Sub

#End Region

    End Class

End Namespace
