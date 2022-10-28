Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Enumerados
Imports ILSBusinessLayer.Comunes
Imports System.IO

Public Class UsuariosFulfillment

#Region "Atributos"

    Private _idUsuarioFulfillment As Integer
    Private _nombre As String
    Private _cedula As Long
    Private _estado As Boolean
    Private _nombreEstado As String
    Private _idUsuarioRegistra As Integer
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
    End Sub

    Public Sub New(ByVal identificador As Integer)
        _idUsuarioFulfillment = identificador
        CargarInformacion()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdUsuarioFulfillment As Integer
        Get
            Return _idUsuarioFulfillment
        End Get
        Set(ByVal value As Integer)
            _idUsuarioFulfillment = value
        End Set
    End Property

    Public Property Nombre As String
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
        End Set
    End Property

    Public Property Cedula As Long
        Get
            Return _cedula
        End Get
        Set(value As Long)
            _cedula = value
        End Set
    End Property

    Public Property Estado As Boolean
        Get
            Return _estado
        End Get
        Set(value As Boolean)
            _estado = value
        End Set
    End Property

    Public Property NombreEstado As String
        Get
            Return _nombreEstado
        End Get
        Set(value As String)
            _nombreEstado = value
        End Set
    End Property

    Public Property IdUsuarioRegistra As Integer
        Get
            Return _idUsuarioRegistra
        End Get
        Set(ByVal value As Integer)
            _idUsuarioRegistra = value
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

#Region "Métodos Privados"

    Private Sub CargarInformacion()
        If _idUsuarioFulfillment > 0 Then
            Using dbManager As New LMDataAccess
                With dbManager
                    If _idUsuarioFulfillment > 0 Then .SqlParametros.Add("@idUsuarioFulfillment", SqlDbType.Int).Value = _idUsuarioFulfillment
                    If _nombre > 0 Then .SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = _nombre
                    If _cedula <> Nothing Then .SqlParametros.Add("@cedula", SqlDbType.Int).Value = _cedula
                    .ejecutarReader("ObtenerInformacionDeUsuariosDeFulfillment", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            CargarValorDePropiedades(.Reader)
                        End If
                        .Reader.Close()
                    End If
                End With
            End Using
        End If
    End Sub

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarValorDePropiedades(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing AndAlso reader.HasRows Then
            Integer.TryParse(reader("idUsuarioFulfillment").ToString, _idUsuarioFulfillment)
            _nombre = reader("nombreUsuario").ToString
            Long.TryParse(reader("numeroCedula").ToString, _cedula)
            Boolean.TryParse(reader("estado").ToString, _estado)
            _nombreEstado = reader("nombreEstado")
            _registrado = True
        End If
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso(-1, "Registro no creado")
        If _idUsuarioFulfillment = 0 AndAlso Not EsNuloOVacio(_nombre) AndAlso Not EsNuloOVacio(_cedula) AndAlso Not EsNuloOVacio(_estado) _
            AndAlso Not EsNuloOVacio(_idUsuarioRegistra) Then
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = _nombre
                        .SqlParametros.Add("@cedula", SqlDbType.BigInt).Value = _cedula
                        .SqlParametros.Add("@estado", SqlDbType.Bit).Value = _estado
                        .SqlParametros.Add("@idUsuarioRegistro", SqlDbType.Int).Value = _idUsuarioRegistra
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 400).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .iniciarTransaccion()
                        .TiempoEsperaComando = 200
                        .ejecutarNonQuery("RegistrarUsuarioFulfillment", CommandType.StoredProcedure)
                        If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                            If resultado.Valor = 0 Then
                                If .estadoTransaccional Then .confirmarTransaccion()
                                resultado.EstablecerMensajeYValor(0, "El usuario fue registrado satisfactoriamente.")
                            Else
                                If .estadoTransaccional Then .abortarTransaccion()
                            End If
                        Else
                            If .estadoTransaccional Then .abortarTransaccion()
                            resultado.Mensaje = "No se pudo evaluar el resultado de registro arrojado por la base de  datos. Por favor intente nuevamente."
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                    resultado.EstablecerMensajeYValor(500, "Error al registrar el usuario: " & ex.Message)
                End Try
            End Using
        Else
            resultado.EstablecerMensajeYValor(300, "No se han proporcionado los valores de todos los parámetros obligatorios. Por favor verifique")
        End If

        Return resultado
    End Function

    Public Function RegistrarEdicion() As ResultadoProceso
        Dim resultado As New ResultadoProceso(-1, "Registro no creado")
        If _idUsuarioFulfillment > 0 AndAlso Not EsNuloOVacio(_nombre) AndAlso Not EsNuloOVacio(_cedula) AndAlso Not EsNuloOVacio(_estado) _
            AndAlso Not EsNuloOVacio(_idUsuarioRegistra) Then
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        If _idUsuarioFulfillment > 0 Then .SqlParametros.Add("@idUsuarioFulfillment", SqlDbType.Int).Value = _idUsuarioFulfillment
                        .SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = _nombre
                        .SqlParametros.Add("@cedula", SqlDbType.BigInt).Value = _cedula
                        .SqlParametros.Add("@estado", SqlDbType.Bit).Value = _estado
                        .SqlParametros.Add("@idUsuarioRegistro", SqlDbType.Int).Value = _idUsuarioRegistra
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 400).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .iniciarTransaccion()
                        .TiempoEsperaComando = 200
                        .ejecutarNonQuery("RegistrarEdicionUsuarioFulfillment", CommandType.StoredProcedure)
                        If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                            If resultado.Valor = 0 Then
                                If .estadoTransaccional Then .confirmarTransaccion()
                                resultado.EstablecerMensajeYValor(0, "El usuario fue editado satisfactoriamente.")
                            Else
                                If .estadoTransaccional Then .abortarTransaccion()
                            End If
                        Else
                            If .estadoTransaccional Then .abortarTransaccion()
                            resultado.Mensaje = "No se pudo evaluar el resultado de registro arrojado por la base de  datos. Por favor intente nuevamente."
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                    resultado.EstablecerMensajeYValor(500, "Error al registrar el usuario: " & ex.Message)
                End Try
            End Using
        Else
            resultado.EstablecerMensajeYValor(300, "No se han proporcionado los valores de todos los parámetros obligatorios. Por favor verifique")
        End If

        Return resultado
    End Function

#End Region

End Class
