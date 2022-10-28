Imports LMDataAccessLayer

Namespace Declaracion

    Public Class Declaracion

#Region "Atributos"

        Private _declaracion As String
        Private _factura As String
        Private _dtArchivo As DataTable
        Private _codigoAceptacion As String
        Private _idUsuario As Integer
        Private _guia As String
        Private _conSoporte As Boolean
        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public Property Declaracion As String
            Get
                Return _declaracion
            End Get
            Set(value As String)
                _declaracion = value
            End Set
        End Property

        Public Property Factura As String
            Get
                Return _factura
            End Get
            Set(value As String)
                _factura = value
            End Set
        End Property

        Public Property DtArchivo As DataTable
            Get
                Return _dtArchivo
            End Get
            Set(value As DataTable)
                _dtArchivo = value
            End Set
        End Property

        Public Property CodigoAceptacion As String
            Get
                Return _codigoAceptacion
            End Get
            Set(value As String)
                _codigoAceptacion = value
            End Set
        End Property

        Public Property IdUsuario As Integer
            Get
                Return _idUsuario
            End Get
            Set(value As Integer)
                _idUsuario = value
            End Set
        End Property

        Public Property Guia As String
            Get
                Return _guia
            End Get
            Set(value As String)
                _guia = value
            End Set
        End Property

        Public Property ConSoporte As String
            Get
                Return _conSoporte
            End Get
            Set(value As String)
                _conSoporte = value
            End Set
        End Property

        Public Property Registrado As Boolean
            Get
                Return _registrado
            End Get
            Set(value As Boolean)
                _registrado = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal declaracion As String)
            MyBase.New()
            _declaracion = declaracion
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _declaracion > 0 Then .SqlParametros.Add("@declaracion", SqlDbType.VarChar).Value = _declaracion
                    .ejecutarReader("ObtenerInfoDeclaracionSoporte", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            CargarResultadoConsulta(.Reader)
                            _registrado = True
                        End If
                        .Reader.Close()
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

        End Sub
#End Region

#Region "Métodos Públicos"

        Public Function ConsultaSerialesDeclaracion() As DataTable
            Dim dt As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Clear()
                        .Add("@declaracion", SqlDbType.VarChar).Value = _declaracion
                    End With
                    dt = .ejecutarDataTable("ObtenerSerialesDeclaracion", CommandType.StoredProcedure)
                End With
            Catch ex As Exception
                If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dt
        End Function

        Public Function RegistrarSoporte() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With dbManager
                        .SqlParametros.Clear()
                        .SqlParametros.Add("@declaracion", SqlDbType.VarChar).Value = _declaracion
                        .SqlParametros.Add("@factura", SqlDbType.VarChar).Value = _factura
                        .SqlParametros.Add("@codigoAceptacion", SqlDbType.VarChar).Value = _codigoAceptacion
                        .SqlParametros.Add("@nombreArchivo", SqlDbType.VarChar).Value = _dtArchivo.Rows(0).Item("NombreOriginal") '_nombreArchivo
                        .SqlParametros.Add("@datosBinarios", SqlDbType.VarBinary).Value = _dtArchivo.Rows(0).Item("DatosBinarios") '_datosBinarios
                        .SqlParametros.Add("@contentType", SqlDbType.VarChar).Value = _dtArchivo.Rows(0).Item("contentType") '_contentType
                        .SqlParametros.Add("@idTipoSoporte", SqlDbType.Int).Value = _dtArchivo.Rows(0).Item("IdTipoSoporte") '_idTipoSoporte
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                        .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output

                        .iniciarTransaccion()
                        .ejecutarNonQuery("RegistrarSoporteDeclaracion", CommandType.StoredProcedure)

                        If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                            resultado.Valor = .SqlParametros("@resultado").Value
                            resultado.Mensaje = .SqlParametros("@mensaje").Value
                            If resultado.Valor = 0 Then
                                .confirmarTransaccion()
                            Else
                                .abortarTransaccion()
                            End If
                        Else
                            .abortarTransaccion()
                            resultado.EstablecerMensajeYValor(400, "No se logró establecer respuesta del servidor, por favor intentelo nuevamente.")
                        End If

                    End With
                End With
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    _declaracion = (reader("declaracion").ToString)
                    _factura = (reader("factura").ToString)
                    _guia = (reader("guia").ToString)
                    _conSoporte = CBool(reader("conSoporte"))
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace
