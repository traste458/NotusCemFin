Imports LMDataAccessLayer
Namespace Fulfillment
    Public Class PostProduccion

#Region "Atributos (campos)"
        Private _imei As String
        Private _iccid As String
        Private _material As String
        Private _subProducto As String
        Private _serial As String
        Private _sim As String
        Private _msisdn As String
        Private _fechaProduccion As String
        Private _telefonoSecuencia As String
        Private _unidades_caja As String
        Private _caja As String
        Private _orden As String

#End Region

#Region "Propiedades"

        Public Property imei() As String
            Get
                Return _imei
            End Get
            Set(ByVal value As String)
                _imei = value
            End Set
        End Property

        Public Property iccid() As String
            Get
                Return _iccid
            End Get
            Set(ByVal value As String)
                _iccid = value
            End Set
        End Property

        Public Property msisdn() As String
            Get
                Return _msisdn
            End Get
            Set(ByVal value As String)
                _msisdn = value
            End Set
        End Property

        Public Property material() As String
            Get
                Return _material
            End Get
            Set(ByVal value As String)
                _material = value
            End Set
        End Property

        Public Property subproducto() As String
            Get
                Return _subProducto
            End Get
            Set(ByVal value As String)
                _subProducto = value
            End Set
        End Property

        Public Property serial() As String
            Get
                Return _serial
            End Get
            Set(ByVal value As String)
                _serial = value
            End Set
        End Property

        Public Property sim() As String
            Get
                Return _sim
            End Get
            Set(ByVal value As String)
                _sim = value
            End Set
        End Property

        Public Property fechaProduccion() As String
            Get
                Return _fechaProduccion
            End Get
            Set(ByVal value As String)
                _fechaProduccion = value
            End Set
        End Property

        Public Property telefonoSecuencia() As String
            Get
                Return _telefonoSecuencia
            End Get
            Set(ByVal value As String)
                _telefonoSecuencia = value
            End Set
        End Property

        Public Property unidades_caja() As String
            Get
                Return _unidades_caja
            End Get
            Set(ByVal value As String)
                _unidades_caja = value
            End Set
        End Property

        Public Property caja() As String
            Get
                Return _caja
            End Get
            Set(ByVal value As String)
                _caja = value
            End Set
        End Property

        Public Property orden() As String
            Get
                Return _orden
            End Get
            Set(ByVal value As String)
                _orden = value
            End Set
        End Property

#End Region

#Region "Metodos Publicos"
        Public Function registrarTripleta(ByVal _imei As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim resultado As Integer
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@imei", SqlDbType.VarChar).Value = _imei
                    End With
                    resultado = .ejecutarScalar("ConfirmarTripletaFulfillment", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
            Return resultado
        End Function

        Public Sub consultarTripleta(ByVal _imei As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@imei", SqlDbType.VarChar).Value = _imei
                    End With
                    .ejecutarReader("ConsultarTripletaFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        _imei = .Reader("imei").ToString
                        _iccid = .Reader("iccid").ToString
                        _msisdn = .Reader("msisdn").ToString
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Function obtenerNombreArchivo(ByVal sProceso As String, ByVal procesoGenerico As String) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim dt As DataTable
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@nombreProceso", SqlDbType.VarChar).Value = sProceso
                        .Add("@nombreprocesoGenerico", SqlDbType.VarChar).Value = procesoGenerico
                    End With
                    dt = .ejecutarDataTable("ObtenerNombreArchivoImpresionFulfillment", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
            Return dt
        End Function

        Public Function obtenerMapeoDatosImpresion(ByVal sProceso As String, ByVal sProcesoGenerico As String) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim dt As New DataTable
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@nombreProceso", SqlDbType.VarChar).Value = sProceso
                        .Add("@nombreProcesoGenerico", SqlDbType.VarChar).Value = sProcesoGenerico
                    End With
                    dt = .ejecutarDataTable("ObtenerMapeoDatosImpresionFulfillment", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
            Return dt
        End Function

        Public Sub obtenerDatosImpresion(ByVal pSerial As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@serial", SqlDbType.VarChar).Value = pSerial
                    End With
                    .ejecutarReader("ObtenerDatosStickersPostproduccionFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        _material = .Reader("material").ToString
                        _subProducto = .Reader("subproducto").ToString
                        _serial = .Reader("serial").ToString
                        _sim = .Reader("sim").ToString
                        _msisdn = .Reader("msisdn").ToString
                        _fechaProduccion = .Reader("fechaProduccion").ToString
                        _telefonoSecuencia = .Reader("telefonoSecuencia").ToString
                        _unidades_caja = .Reader("unidades_caja").ToString
                        Long.TryParse(.Reader("caja").ToString, _caja)
                        _orden = .Reader("orden").ToString
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

#End Region

    End Class
End Namespace
