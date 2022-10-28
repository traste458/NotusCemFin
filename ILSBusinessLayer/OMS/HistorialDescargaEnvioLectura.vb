Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras

Namespace OMS

    Public Class HistorialDescargaEnvioLectura

#Region "Campos"

        Private _idHistorialDescargaEnvioLectura As Long
        Private _idOrdenEnvioLectura As Long
        Private _idUsuario As Long
        Private _creador As String
        Private _fechaDescarga As Date

#End Region

#Region "Propiedades"

        Public Property IdHistorialDescargaEnvioLectura() As Long
            Get
                Return _idHistorialDescargaEnvioLectura
            End Get
            Set(ByVal value As Long)
                _idHistorialDescargaEnvioLectura = value
            End Set
        End Property

        Public Property IdOrdenEnvioLectura() As Long
            Get
                Return _idOrdenEnvioLectura
            End Get
            Set(ByVal value As Long)
                _idOrdenEnvioLectura = value
            End Set
        End Property

        Public Property IdUsuario() As Long
            Get
                Return _idUsuario
            End Get
            Set(ByVal value As Long)
                _idUsuario = value
            End Set
        End Property

        Public Property Creador() As String
            Get
                Return _creador
            End Get
            Set(ByVal value As String)
                _creador = value
            End Set
        End Property

        Public Property FechaDescarga() As Date
            Get
                Return _fechaDescarga
            End Get
            Set(ByVal value As Date)
                _fechaDescarga = value
            End Set
        End Property

#End Region

#Region "Contructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            _idHistorialDescargaEnvioLectura = identificador
            CargarInformacion()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            If _idHistorialDescargaEnvioLectura <> 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idHistorialDescargaEnvioLectura", SqlDbType.BigInt).Value = _idHistorialDescargaEnvioLectura
                        .ejecutarReader("ObtenerHistorialDescargaEnvioLectura", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                Long.TryParse(.Reader("idOrdenEnvioLectura").ToString, _idOrdenEnvioLectura)
                                Creador = .Reader("Creador").ToString()
                                Date.TryParse(.Reader("fechaDescarga").ToString, _fechaDescarga)
                            End If
                            .Reader.Close()
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
        End Sub

#End Region

#Region "Métodos Publicos"

        Public Function Registrar() As Short
            Dim resultado As Short = 0

            If _idOrdenEnvioLectura > 0 And _idUsuario > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@idOrdenEnvioLectura", SqlDbType.BigInt).Value = _idOrdenEnvioLectura
                            .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                            .Add("@identity", SqlDbType.Int).Direction = ParameterDirection.Output
                            .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        End With
                        .iniciarTransaccion()
                        .ejecutarNonQuery("CrearHistorialDescargaEnvioLectura", CommandType.StoredProcedure)
                        resultado = CShort(.SqlParametros("@returnValue").Value)
                        If resultado = 0 Then
                            _idHistorialDescargaEnvioLectura = CLng(.SqlParametros("@identity").Value)
                            .confirmarTransaccion()
                        Else
                            If .estadoTransaccional Then .abortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 2
            End If

            Return resultado

        End Function

#End Region


#Region "Métodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroDescargaEnvioLectura
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroDescargaEnvioLectura) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDatos As New DataTable()
            Try
                With dbManager
                    With .SqlParametros
                        If filtro.IdHistorialDescargaEnvioLectura > 0 Then .Add("@idHistorialDescargaEnvioLectura", SqlDbType.BigInt).Value = filtro.IdHistorialDescargaEnvioLectura
                        If filtro.IdOrdenEnvioLectura > 0 Then .Add("@idOrdenEnvioLectura", SqlDbType.BigInt).Value = filtro.IdOrdenEnvioLectura
                        If filtro.IdUsuario > 0 Then .Add("@idUsuario", SqlDbType.Int).Value = filtro.IdUsuario
                    End With
                    dtDatos = .ejecutarDataTable("ObtenerHistorialDescargaEnvioLectura", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtDatos
        End Function

#End Region

    End Class

End Namespace