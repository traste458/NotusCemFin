Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Enumerados
Imports ILSBusinessLayer.Comunes
Imports System.IO

Public Class GestionNovedadProduccion

#Region "Atributos"

    Private _idNovedad As Integer
    Private _novedad As String
    Private _gestion As String
    Private _fechaGestion As Date
    Private _usuarioGestion As String
    Private _idUsuarioGestiono As Integer
    Private _conGestion As Integer
    Private _conImagen As Integer
    Private _conArchivo As Integer
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
    End Sub

    Public Sub New(ByVal identificador As Integer)
        _idNovedad = identificador
        CargarInformacion()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdNovedad As Integer
        Get
            Return _idNovedad
        End Get
        Set(ByVal value As Integer)
            _idNovedad = value
        End Set
    End Property

    Public Property Novedad As String
        Get
            Return _novedad
        End Get
        Set(value As String)
            _novedad = value
        End Set
    End Property

    Public Property Gestion As String
        Get
            Return _gestion
        End Get
        Set(value As String)
            _gestion = value
        End Set
    End Property

    Public Property FechaGestion As Date
        Get
            Return _fechaGestion
        End Get
        Set(value As Date)
            _fechaGestion = value
        End Set
    End Property

    Public Property UsuarioGestion As String
        Get
            Return _usuarioGestion
        End Get
        Set(value As String)
            _usuarioGestion = value
        End Set
    End Property

    Public Property IdUsuarioGestiono As Integer
        Get
            Return _idUsuarioGestiono
        End Get
        Set(value As Integer)
            _idUsuarioGestiono = value
        End Set
    End Property

    Public Property ConGestion As Integer
        Get
            Return _conGestion
        End Get
        Set(value As Integer)
            _conGestion = value
        End Set
    End Property

    Public Property ConImagen As Integer
        Get
            Return _conImagen
        End Get
        Set(value As Integer)
            _conImagen = value
        End Set
    End Property

    Public Property ConArchivo As Integer
        Get
            Return _conArchivo
        End Get
        Set(value As Integer)
            _conArchivo = value
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
        If _idNovedad > 0 Then
            Using dbManager As New LMDataAccess
                With dbManager
                    If _idNovedad > 0 Then .SqlParametros.Add("@idNovedad", SqlDbType.Int).Value = _idNovedad
                    .ejecutarReader("ObtenerGestionDeNovedadDeProduccion", CommandType.StoredProcedure)
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
            Integer.TryParse(reader("idNovedad").ToString, _idNovedad)
            _novedad = reader("novedad").ToString
            _gestion = reader("gestion").ToString
            Date.TryParse(reader("fechaGestion").ToString, _fechaGestion)
            Integer.TryParse(reader("usuarioGestion").ToString, _usuarioGestion)
            Integer.TryParse(reader("conGestion").ToString, _conGestion)
            Integer.TryParse(reader("conImagen").ToString, _conImagen)
            Integer.TryParse(reader("conArchivo").ToString, _conArchivo)
            _registrado = True
        End If
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso(-1, "Registro no creado")
        If _idNovedad > 0 AndAlso Not EsNuloOVacio(_gestion) AndAlso _idUsuarioGestiono > 0 Then
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idNovedad", SqlDbType.Int).Value = _idNovedad
                        .SqlParametros.Add("gestion", SqlDbType.VarChar, 2000).Value = _gestion.Trim
                        .SqlParametros.Add("idUsuario", SqlDbType.Int).Value = _idUsuarioGestiono
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 400).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .iniciarTransaccion()
                        .TiempoEsperaComando = 200
                        .ejecutarNonQuery("RegistrarGestionNovedadesProduccion", CommandType.StoredProcedure)
                        If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                            If resultado.Valor = 0 Then
                                If .estadoTransaccional Then .confirmarTransaccion()
                                resultado.EstablecerMensajeYValor(0, "La gestion fue registrada satisfactoriamente.")
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
                End Try
            End Using
        Else
            resultado.EstablecerMensajeYValor(300, "No se han proporcionado los valores de todos los parámetros obligatorios. Por favor verifique")
        End If
        Return resultado
    End Function

#End Region

End Class