Imports LMDataAccessLayer

Public Class NovedadSerialOrdenRecoleccion

#Region "Atributos"

    Private _idDetalleSerial As Integer
    Private _serial As String
    Private _idDevolucion As Integer
    Private _idOrden As Integer
    Private _idEstado As Integer
    Private _estado As String
    Private _observacion As String

#End Region


#Region "Propiedades"

    Public Property IdDetalleSerial As Integer
        Get
            Return _idDetalleSerial
        End Get
        Set(value As Integer)
            _idDetalleSerial = value
        End Set
    End Property

    Public Property Serial As String
        Get
            Return _serial
        End Get
        Set(value As String)
            _serial = value
        End Set
    End Property

    Public Property IdDevolucion As Integer
        Get
            Return _idDevolucion
        End Get
        Set(value As Integer)
            _idDevolucion = value
        End Set
    End Property

    Public Property IdOrden As Integer
        Get
            Return _idOrden
        End Get
        Set(value As Integer)
            _idOrden = value
        End Set
    End Property

    Public Property IdEstado As Integer
        Get
            Return _idEstado
        End Get
        Set(value As Integer)
            _idEstado = value
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

#End Region


#Region "Métodos Privados"


#End Region

#Region "Métodos Públicos"

    Public Function GuardarNovedad() As String

        Dim dbManager As New LMDataAccess
        Dim resultado As String

        With dbManager
            If _idDetalleSerial > 0 Then
                .SqlParametros.Add("@idSerial", SqlDbType.Int).Value = _idDetalleSerial
            End If
            If Not String.IsNullOrEmpty(_serial) Then
                .SqlParametros.Add("@serial", SqlDbType.VarChar).Value = _serial
            End If
            If _idEstado > 0 Then
                .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = _idEstado
            End If
            If Not String.IsNullOrEmpty(_observacion) Then
                .SqlParametros.Add("@observacion", SqlDbType.VarChar).Value = _observacion
            End If

            resultado = .EjecutarScalar("GuardarNovedadOrdenRecoleccionSerial", CommandType.StoredProcedure)
        End With

        Return resultado
    End Function

#End Region


End Class
