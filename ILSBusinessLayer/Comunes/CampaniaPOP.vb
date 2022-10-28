Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.IO
Imports ILSBusinessLayer.Enumerados

Public Class CampaniaPOP

#Region "Atributos (campos)"

    Private _idCampania As Integer
    Private _nombre As String
    Private _fechaRegistro As Date
    Private _fechaVigencia As Date
    Private _activo As EstadoBinario
    Private _estado As String
    Private _idUsuarioRegistra As Integer
    Private _usuarioRegistra As String
    Private _descripcion As String
    Private _justificacion As String
    Private _flag As String

    Private _registrado As Boolean

#End Region

#Region "Propiedades"

    Public Property IdCampania As Integer
        Get
            Return _idCampania
        End Get
        Set(value As Integer)
            _idCampania = value
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

    Public Property FechaRegistro As Date
        Get
            Return _fechaRegistro
        End Get
        Set(value As Date)
            _fechaRegistro = value
        End Set
    End Property

    Public Property FechaVigencia As Date
        Get
            Return _fechaVigencia
        End Get
        Set(value As Date)
            _fechaVigencia = value
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

    Public Property Estado As String
        Get
            Return _estado
        End Get
        Set(value As String)
            _estado = value
        End Set
    End Property

    Public Property IdUsuarioRegistra As Integer
        Get
            Return _idUsuarioRegistra
        End Get
        Set(value As Integer)
            _idUsuarioRegistra = value
        End Set
    End Property

    Public Property UsuarioRegistra As String
        Get
            Return _usuarioRegistra
        End Get
        Set(value As String)
            _usuarioRegistra = value
        End Set
    End Property

    Public Property Descripcion As String
        Get
            Return _descripcion
        End Get
        Set(value As String)
            _descripcion = value
        End Set
    End Property

    Public Property Justificacion As String
        Get
            Return _justificacion
        End Get
        Set(value As String)
            _justificacion = value
        End Set
    End Property

    Public Property Flag As String
        Get
            Return _flag
        End Get
        Set(value As String)
            _flag = value
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

    Public Sub New(ByVal idCampania As Integer)
        MyBase.New()
        _idCampania = idCampania
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Add("@idListaCampania", SqlDbType.Int).Value = CStr(_idCampania)
                .ejecutarReader("ObtenerCampaniasPOP", CommandType.StoredProcedure)

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


    Public Shared Function ObtenerCiudadesPorPais(Optional ByVal idpais As Integer = 170)
        Dim db As New LMDataAccess
        db.agregarParametroSQL("@idPais", idpais)
        Return db.ejecutarDataTable("SeleccionarCiudades", CommandType.StoredProcedure)
    End Function

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If _idUsuarioRegistra > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@nombre", SqlDbType.VarChar, 255).Value = _nombre
                        .Add("@descripcion", SqlDbType.VarChar, 450).Value = _descripcion
                        .Add("@fechaVigencia", SqlDbType.DateTime).Value = _fechaVigencia
                        .Add("@idUsuarioRegistra", SqlDbType.Int).Value = _idUsuarioRegistra
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .Add("@mensaje", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output
                        .Add("@idCampania", SqlDbType.Int).Direction = ParameterDirection.Output
                    End With
                    .iniciarTransaccion()
                    .ejecutarNonQuery("RegistrarCampaniaPOP", CommandType.StoredProcedure)
                    If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        .confirmarTransaccion()
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        resultado.Valor = .SqlParametros("@resultado").Value
                        _idCampania = .SqlParametros("@idCampania").Value
                    Else
                        .abortarTransaccion()
                        resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta del servidor. Por favor intente nuevamente.")
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message)
            End Try
        Else
            resultado.EstablecerMensajeYValor(10, "No se han proporcionado todos los datos requeridos para realizar el registro. ")
        End If
        Return resultado
    End Function

    Public Function Actualizar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If _idUsuarioRegistra > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idUsuarioRegistra", SqlDbType.Int).Value = _idUsuarioRegistra
                        .Add("@idCampania", SqlDbType.BigInt).Value = _idCampania
                        .Add("@activo", SqlDbType.Bit).Value = _activo
                        If Not String.IsNullOrEmpty(_nombre) Then .Add("@nombre", SqlDbType.VarChar, 250).Value = _nombre
                        If _fechaVigencia > Date.MinValue Then .Add("@fechaVigencia", SqlDbType.DateTime).Value = _fechaVigencia
                        If Not String.IsNullOrEmpty(_descripcion) Then .Add("@descripcion", SqlDbType.VarChar, 450).Value = _descripcion
                        If Not String.IsNullOrEmpty(_justificacion) Then .Add("@justificacion", SqlDbType.VarChar, 450).Value = _justificacion
                        If Not String.IsNullOrEmpty(_flag) Then .Add("@flag", SqlDbType.VarChar, 20).Value = _flag
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .Add("@mensaje", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output
                    End With
                    .iniciarTransaccion()
                    .ejecutarNonQuery("ActualizarCampaniaPOP", CommandType.StoredProcedure)
                    If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        .confirmarTransaccion()
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        resultado.Valor = .SqlParametros("@resultado").Value
                    Else
                        .abortarTransaccion()
                        resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta del servidor. Por favor intente nuevamente.")
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message)
            End Try
        Else
            resultado.EstablecerMensajeYValor(10, "No se han proporcionado todos los datos requeridos para realizar el registro. ")
        End If
        Return resultado
    End Function

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idCampania"), _idCampania)
                If Not IsDBNull(reader("nombre")) Then _nombre = (reader("nombre").ToString)
                If Not IsDBNull(reader("fechaRegistro")) Then _fechaRegistro = CDate(reader("fechaRegistro"))
                If Not IsDBNull(reader("fechaVigencia")) Then _fechaVigencia = CDate(reader("fechaVigencia"))
                Boolean.TryParse(reader("activo"), _activo)
                If Not IsDBNull(reader("estado")) Then _estado = (reader("estado").ToString)
                Integer.TryParse(reader("idUsuarioRegistra"), _idUsuarioRegistra)
                If Not IsDBNull(reader("usuarioRegistra")) Then _usuarioRegistra = (reader("usuarioRegistra").ToString)
                If Not IsDBNull(reader("descripcion")) Then _descripcion = (reader("descripcion").ToString)
                If Not IsDBNull(reader("justificacion")) Then _justificacion = (reader("justificacion").ToString)
            End If
        End If
    End Sub

#End Region

End Class
