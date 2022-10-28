Imports ILSBusinessLayer.Estructuras

Namespace OMS

    Public Class EnvioNacionalizacionDetalle

#Region "Atributos"
        Private _idDetalleEnvio As Long
        Private _idEnvio As Long
        Private _idOrdenTrabajo As Long
#End Region

#Region "Constructores/Destructores"

        Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            _idDetalleEnvio = identificador
            CargarInformacion()
        End Sub

#End Region

#Region "Propiedades"

        Public ReadOnly Property IdDetalleEnvio() As Long
            Get
                Return _idDetalleEnvio
            End Get
        End Property

        Public Property IdEnvio() As Long
            Get
                Return _idEnvio
            End Get
            Set(ByVal value As Long)
                _idEnvio = value
            End Set
        End Property

        Public Property IdOrdenTrabajo() As Long
            Get
                Return _idOrdenTrabajo
            End Get
            Set(ByVal value As Long)
                _idOrdenTrabajo = value
            End Set
        End Property

#End Region

#Region "Metodos Publicos"

        Public Sub Crear()
            Dim db As New LMDataAccessLayer.LMDataAccess
            With db
                With .SqlParametros
                    .Add("@idEnvio", SqlDbType.BigInt).Value = _idEnvio
                    .Add("@idOrdenTrabajo", SqlDbType.BigInt).Value = _idOrdenTrabajo
                    .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                End With

                Try
                    Dim result As Integer = 0
                    .iniciarTransaccion()
                    .ejecutarNonQuery("CrearEnvioNacionalizacionDetalle", CommandType.StoredProcedure)
                    result = .SqlParametros("@result").Value
                    If result = 0 Then
                        _idDetalleEnvio = CLng(.SqlParametros("@identity").Value)
                        .confirmarTransaccion()
                    Else
                        Throw New Exception("Imposible registrar la información de la Orden de Trabajo relacionada con el envio a navionalizacion en la Base de Datos.")
                    End If
                Catch ex As Exception
                    If .estadoTransaccional Then .abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
        End Sub

        Public Shared Function ObtenerDetalleEnvio(ByVal filtro As FiltroDetalleEnvio) As DataTable
            Dim dt As New DataTable()
            Dim db As New LMDataAccessLayer.LMDataAccess

            Try
                With db
                    With .SqlParametros
                        If filtro.IdEnvio > 0 Then .Add("@idEnvio", SqlDbType.BigInt).Value = filtro.IdEnvio
                    End With

                    dt = .ejecutarDataTable("ObtenerDetalleEnvioAgrupado", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try

            Return dt
        End Function

#End Region

#Region "Metodos Privados"

        Private Sub CargarInformacion()

        End Sub

#End Region

#Region "Métodos Compartidos"

        'Public Overloads Shared Function ObtenerListado() As DataTable
        '    Dim filtro As New FiltroDetalleEnvio
        '    Dim dtDatos As DataTable = ObtenerListado(filtro)
        '    Return dtDatos
        'End Function

        'Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroDetalleEnvio) As DataTable
        '    Dim dt As New DataTable()
        '    Dim db As New LMDataAccessLayer.LMDataAccess
        '    With db
        '        With .SqlParametros
        '            .Add("@idEnvio", SqlDbType.BigInt).Value = filtro.IdDetalleEnvio
        '        End With
        '        Try
        '            dt = .ejecutarDataTable("ObtenerDetalleEnvio", CommandType.StoredProcedure)
        '        Catch ex As Exception
        '            Throw New Exception(ex.Message, ex)
        '        Finally
        '            db.Dispose()
        '        End Try
        '    End With
        '    Return dt
        'End Function

#End Region

    End Class
End Namespace