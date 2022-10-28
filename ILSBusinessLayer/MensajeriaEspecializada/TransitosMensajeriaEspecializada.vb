Imports System.Text
Imports LMDataAccessLayer

Namespace MensajeriaEspecializada

    ''' <summary>
    ''' Author: Beltrán, Diego
    ''' Create date: 31/07/2014
    ''' Description: Clase diseñada para cargar los servicios de mensajeria especializada que se asignan a tránsito y se reciben en las diferentes bodegas CEM
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TransitosMensajeriaEspecializada

#Region "Atributos"

        Private _idServicio As Long
        Private _numeroRadicado As Long
        Private _fecha As DateTime
        Private _idEstado As Enumerados.EstadoServicio
        Private _estado As String
        Private _nombre As String
        Private _nombreAutorizado As String
        Private _identicacion As String
        Private _idCiudad As Integer
        Private _ciudad As String
        Private _barrio As String
        Private _direccion As String
        Private _telefono As String
        Private _fechaVencimientoReserva As DateTime
        Private _fechaTransito As DateTime
        Private _guia As String
        Private _idTransportadora As Integer
        Private _transportadora As String

        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        ''' <summary>
        ''' Define o establece el identificador del servicio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdServicio As Long
            Get
                Return _idServicio
            End Get
            Set(value As Long)
                _idServicio = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece el número del radicado con el que se registro el servicio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroRadicado As Long
            Get
                Return _numeroRadicado
            End Get
            Set(value As Long)
                _numeroRadicado = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece la fecha de registro del servicio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Fecha As DateTime
            Get
                Return _fecha
            End Get
            Set(value As DateTime)
                _fecha = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece el identificador del estado del servicio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdEstado As Enumerados.EstadoServicio
            Get
                Return _idEstado
            End Get
            Set(value As Enumerados.EstadoServicio)
                _idEstado = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece el nombre del estado del servicio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Estado As String
            Get
                Return _estado
            End Get
            Set(value As String)
                _estado = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establecce el nombre del cliente que se registró en el servicio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nombre As String
            Get
                Return _nombre
            End Get
            Set(value As String)
                _nombre = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece el nombre de la persona autorizada a recibir el servicio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NombreAutorizado As String
            Get
                Return _nombreAutorizado
            End Get
            Set(value As String)
                _nombreAutorizado = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece el número de identificación del titular del servicio (Cliente)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Identicacion As String
            Get
                Return _identicacion
            End Get
            Set(value As String)
                _identicacion = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece el identificador de la ciudad destino del servicio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdCiudad As Integer
            Get
                Return _idCiudad
            End Get
            Set(value As Integer)
                _idCiudad = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece el nombre de la ciudad destino del servicio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Ciudad As String
            Get
                Return _ciudad
            End Get
            Set(value As String)
                _ciudad = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece el nombre del barrio de entrega del servicio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Barrio As String
            Get
                Return _barrio
            End Get
            Set(value As String)
                _barrio = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece la dirección de entrega del servicio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Direccion As String
            Get
                Return _direccion
            End Get
            Set(value As String)
                _direccion = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece el teléfono de contacto del titular del servicio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Telefono As String
            Get
                Return _telefono
            End Get
            Set(value As String)
                _telefono = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece la fecha máxima de entrega del servicio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FechaVencimientoReserva As DateTime
            Get
                Return _fechaVencimientoReserva
            End Get
            Set(value As DateTime)
                _fechaVencimientoReserva = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece la fecha en la que se generó el tránsito del servicio hacia su destino
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FechaTransito As DateTime
            Get
                Return _fechaTransito
            End Get
            Set(value As DateTime)
                _fechaTransito = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece el número de guía asignado para la salida a tránsito del servicio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Guia As String
            Get
                Return _guia
            End Get
            Set(value As String)
                _guia = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece el identificador de la transportadora con el que se realiza el tránsito del servicio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdTransportadora As Integer
            Get
                Return _idTransportadora
            End Get
            Set(value As Integer)
                _idTransportadora = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece el nombre de la transportadora con el que se realiza el tránsito del servicio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Transportadora As String
            Get
                Return _transportadora
            End Get
            Set(value As String)
                _transportadora = value
            End Set
        End Property


#End Region

#Region "Construtores"

        Public Sub New()
            MyBase.New()
        End Sub

        ''' <summary>
        ''' Constructor que sobrecarga la clase con los datos del idServicio proporcionado
        ''' </summary>
        ''' <param name="idServicio"> de tipo <see langword="long"/> que contiene la información correspondiente al identificador del servicio. </param>
        ''' <remarks>
        ''' Su forma de instanciamiento se debe realizar de la siguiente manera:
        ''' Dim miClase As New  MensajeriaEspecializada.TransitosMensajeriaEspecializada(idServicio:= idServicio)
        ''' </remarks>
        Public Sub New(ByVal idServicio As Long)
            MyBase.New()
            _idServicio = idServicio
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        ''' <summary>
        ''' función que realiza la inicialización de la carga de los atributos de la clase, según los parametros establecidos
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@listaIdServicio", SqlDbType.VarChar, 30).Value = CStr(_idServicio)
                    .ejecutarReader("ObtenerInfoTransitosMensajeriaEspecializada", CommandType.StoredProcedure)

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

#End Region

#Region "Métodos Protegidos"

        ''' <summary>
        ''' Método encargado de sobrecargar los atributos de la clase 
        ''' </summary>
        ''' <param name="reader"> de tipo <see langword="Data.Common.DbDataReader"/> que contiene un objeto de tipo reader, 
        ''' para realizar la lectura y asignación de valores a los atributos de la clase</param>
        ''' <remarks></remarks>
        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Long.TryParse(reader("idServicioMensajeria"), _idServicio)
                    Long.TryParse(reader("numeroRadicado"), _numeroRadicado)
                    Date.TryParse(reader("fecha"), _fecha)
                    Integer.TryParse(reader("idEstado"), _idEstado)
                    If Not String.IsNullOrEmpty(reader("estado")) Then _estado = CStr(reader("estado"))
                    If Not String.IsNullOrEmpty(reader("nombre")) Then _nombre = CStr(reader("nombre"))
                    If Not String.IsNullOrEmpty(reader("nombreAutorizado")) Then _nombreAutorizado = CStr(reader("nombreAutorizado"))
                    If Not String.IsNullOrEmpty(reader("identicacion")) Then _identicacion = CStr(reader("identicacion"))
                    If Not String.IsNullOrEmpty(reader("idCiudad")) Then Integer.TryParse(reader("idCiudad"), _idCiudad)
                    If Not String.IsNullOrEmpty(reader("ciudad")) Then _ciudad = CStr(reader("ciudad"))
                    If Not String.IsNullOrEmpty(reader("barrio")) Then _barrio = CStr(reader("barrio"))
                    If Not String.IsNullOrEmpty(reader("direccion")) Then _direccion = CStr(reader("direccion"))
                    If Not String.IsNullOrEmpty(reader("telefono")) Then _telefono = CStr(reader("telefono"))
                    If Not String.IsNullOrEmpty(reader("fechaVencimientoReserva")) Then Date.TryParse(reader("fechaVencimientoReserva"), _fechaVencimientoReserva)
                    If Not String.IsNullOrEmpty(reader("fechaTransito")) Then Date.TryParse(reader("fechaTransito"), _fechaTransito)
                    If Not String.IsNullOrEmpty(reader("numeroGuia")) Then _guia = CStr(reader("numeroGuia"))
                    If Not String.IsNullOrEmpty(reader("idTransportadora")) Then Integer.TryParse(reader("idTransportadora"), _idTransportadora)
                    If Not String.IsNullOrEmpty(reader("transportadora")) Then _transportadora = CStr(reader("transportadora"))
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace


