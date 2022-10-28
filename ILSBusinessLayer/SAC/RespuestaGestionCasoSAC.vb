Imports LMDataAccessLayer
Imports System.Web

Namespace SAC

    Public Class RespuestaGestionCasoSAC

#Region "Atributos"

        Private _idRespuesta As Integer
        Private _idGestion As Integer
        Private _idOrigenRespuesta As Byte
        Private _origenRespuesta As String
        Private _descripcion As String
        Private _requiereArchivo As Boolean
        Private _nombreArchivo As String
        Private _nombreArchivoConRuta As String
        Private _nombreArchivoOriginal As String
        Private _fechaRecepcion As Date
        Private _fechaRegistro As Date
        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public Property IdRespuesta() As Integer
            Get
                Return _idRespuesta
            End Get
            Set(ByVal value As Integer)
                _idRespuesta = value
            End Set
        End Property

        Public Property IdGestion() As Integer
            Get
                Return _idGestion
            End Get
            Set(ByVal value As Integer)
                _idGestion = value
            End Set
        End Property

        Public Property IdOrigenRespuesta() As Byte
            Get
                Return _idOrigenRespuesta
            End Get
            Set(ByVal value As Byte)
                _idOrigenRespuesta = value
            End Set
        End Property

        Public Property OrigenRespuesta() As String
            Get
                Return _origenRespuesta
            End Get
            Set(ByVal value As String)
                _origenRespuesta = value
            End Set
        End Property

        Public Property Descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
        End Property

        Public Property RequiereArchivo() As Boolean
            Get
                Return _requiereArchivo
            End Get
            Protected Friend Set(ByVal value As Boolean)
                _requiereArchivo = value
            End Set
        End Property

        Public Property NombreArchivo() As String
            Get
                Return _nombreArchivo
            End Get
            Set(ByVal value As String)
                _nombreArchivo = value
            End Set
        End Property

        Public Property NombreArchivoConRuta() As String
            Get
                Return _nombreArchivoConRuta
            End Get
            Set(ByVal value As String)
                _nombreArchivoConRuta = value
            End Set
        End Property

        Public Property NombreArchivoOriginal() As String
            Get
                Return _nombreArchivoOriginal
            End Get
            Set(ByVal value As String)
                _nombreArchivoOriginal = value
            End Set
        End Property

        Public Property FechaRecepcion() As Date
            Get
                Return _fechaRecepcion
            End Get
            Set(ByVal value As Date)
                _fechaRecepcion = value
            End Set
        End Property

        Public ReadOnly Property FechaRegistro() As Date
            Get
                Return _fechaRegistro
            End Get
        End Property

        Public ReadOnly Property Registrado() As Boolean
            Get
                Return _registrado
            End Get
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _descripcion = ""
            _nombreArchivo = ""
            _nombreArchivoConRuta = ""
            _nombreArchivoOriginal = ""
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            CargarDatos(identificador)
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos(ByVal identificador As Integer)
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idRespuesta", SqlDbType.Int).Value = identificador
                    .ejecutarReader("ConsultarRespuestaGestionCasoSAC", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idRespuesta").ToString, _idRespuesta)
                            Integer.TryParse(.Reader("idGestion").ToString, _idGestion)
                            Byte.TryParse(.Reader("idOrigenRespuesta").ToString, _idOrigenRespuesta)
                            _descripcion = .Reader("descripcion").ToString
                            _origenRespuesta = .Reader("origenRespuesta").ToString
                            Boolean.TryParse(.Reader("requiereArchivo").ToString, _requiereArchivo)
                            _nombreArchivo = .Reader("archivo").ToString
                            _nombreArchivoConRuta = HttpContext.Current.Server.MapPath(.Reader("archivoConRuta").ToString)
                            _nombreArchivoOriginal = .Reader("archivoOriginal").ToString
                            Date.TryParse(.Reader("fechaRecepcion").ToString, _fechaRecepcion)
                            Date.TryParse(.Reader("fechaRegistro").ToString, _fechaRegistro)
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

#Region "Métodos Protegidos"

        Protected Friend Sub EstablecerFechaRegistro(ByVal valor As Date)
            _fechaRegistro = valor
        End Sub

        Protected Friend Sub MarcarComoRegistrado()
            _registrado = True
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso

            Return resultado
        End Function

        Public Function Actualizar() As ResultadoProceso
            Dim resultado As New ResultadoProceso

            Return resultado
        End Function

#End Region

    End Class

End Namespace


