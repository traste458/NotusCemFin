Imports LMDataAccessLayer

Public Class Reprocesos

    Inherits FulfillmentBase

#Region "Atributos"

    Dim _idTipoProducto As Integer
    Private _iccidInicial As String
    Private _iccidFinal As String
    Private _idOrdenBodegaje As Integer

#End Region

#Region "Propiedades"

    Public Property IdTipoProducto As Integer
        Get
            Return _idTipoProducto
        End Get
        Set(value As Integer)
            _idTipoProducto = value
        End Set
    End Property

    Public Property IccidInicial As String
        Get
            Return _iccidInicial
        End Get
        Set(value As String)
            _iccidInicial = value
        End Set
    End Property

    Public Property IccidFinal As String
        Get
            Return _iccidFinal
        End Get
        Set(value As String)
            _iccidFinal = value
        End Set
    End Property

    Public Property IdOrdenBodegaje As Integer
        Get
            Return _idOrdenBodegaje
        End Get
        Set(value As Integer)
            _idOrdenBodegaje = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.new()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Function ValidarSerial() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim db As LMDataAccessLayer.LMDataAccess
        Try
            db = New LMDataAccess
            With db
                With .SqlParametros
                    .Clear()
                    .Add("@idTipoProducto", SqlDbType.BigInt).Value = _idTipoProducto
                    .Add("@idOrdenReproceso", SqlDbType.BigInt).Value = _idOrden
                    .Add("@serial", SqlDbType.VarChar, 20).Value = _serial
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@mensaje", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output
                End With
                .ejecutarNonQuery("ValidarSerialEnReproceso", CommandType.StoredProcedure)
                If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                    resultado.Valor = .SqlParametros("@resultado").Value
                Else
                    resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta del servidor. Por favor intente nuevamente.")
                End If
            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
        Return resultado
    End Function

#End Region

#Region "Métodos Públicos"

    Public Overrides Function RegistrarSeriales() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim db As LMDataAccessLayer.LMDataAccess
        Try
            db = New LMDataAccess
            With db
                With .SqlParametros
                    .Clear()
                    .Add("@ingresaPorDevolucion", SqlDbType.Bit).Value = _ingresaPorDevolucion
                    .Add("@serial", SqlDbType.VarChar, 50).Value = _serial.Trim.ToUpper
                    .Add("@idProducto", SqlDbType.Int).Value = _idProducto
                    .Add("@caja", SqlDbType.BigInt).Value = _caja
                    .Add("@estiba", SqlDbType.BigInt).Value = _estiba
                    .Add("@idOrden", SqlDbType.BigInt).Value = _idOrden
                    .Add("@secuenciaEnOrden", SqlDbType.Int).Value = _ordenSecuencia
                    .Add("@linea", SqlDbType.SmallInt).Value = _linea
                    .Add("@sim", SqlDbType.VarChar, 50).Value = _iccid
                    .Add("@pin", SqlDbType.VarChar, 20).Value = _pin
                    .Add("@idTipoProducto", SqlDbType.Int).Value = _idTipoProducto
                    .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@mensaje", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output
                    If _noConformidad IsNot Nothing AndAlso _noConformidad.Count > 0 Then _
                        .Add("@listaNoConformidades", SqlDbType.VarChar).Value = Join(_noConformidad.ToArray(), ",")
                End With
                .ejecutarNonQuery("RegistrarReproceso", CommandType.StoredProcedure)
                If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                Else
                    resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta del servidor. Por favor intente nuevamente.")
                End If
            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try

        Return resultado
    End Function

    Public Overrides Function EsSerialValido() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        resultado = MyBase.EsSerialValido()
        If resultado.Valor = 0 Then
            resultado = ValidarSerial()
        End If

        Return resultado
    End Function

    Public Function ValidacionesReprocesos() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim db As LMDataAccessLayer.LMDataAccess
        Try
            db = New LMDataAccess
            With db
                With .SqlParametros
                    .Clear()
                    .Add("@idTipoProducto", SqlDbType.BigInt).Value = _idTipoProducto
                    .Add("@idOrden", SqlDbType.BigInt).Value = _idOrden
                    .Add("@serial", SqlDbType.VarChar, 20).Value = _serial
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@ingresaPorDevolucion", SqlDbType.Int).Direction = ParameterDirection.Output
                    .Add("@mensaje", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output
                End With
                .ejecutarNonQuery("ValidacionesEnReprocesos", CommandType.StoredProcedure)
                If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                    resultado.Valor = .SqlParametros("@resultado").Value
                    Boolean.TryParse(.SqlParametros("@ingresaPorDevolucion").Value.ToString, _ingresaPorDevolucion)
                Else
                    resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta del servidor. Por favor intente nuevamente.")
                End If
            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
        Return resultado
    End Function

    Public Function ValidarRangos() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim db As LMDataAccessLayer.LMDataAccess
        Try
            db = New LMDataAccess
            With db
                With .SqlParametros
                    .Clear()
                    .Add("@idTipoProducto", SqlDbType.BigInt).Value = _idTipoProducto
                    .Add("@idOrden", SqlDbType.BigInt).Value = _idOrden
                    .Add("@iccidInicial", SqlDbType.VarChar, 20).Value = _iccidInicial
                    .Add("@iccidFinal", SqlDbType.VarChar, 20).Value = _iccidFinal
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@mensaje", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output
                End With
                .ejecutarNonQuery("ValidarRangosLecturaReprocesos", CommandType.StoredProcedure)
                If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                    resultado.Valor = .SqlParametros("@resultado").Value
                Else
                    resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta del servidor. Por favor intente nuevamente.")
                End If
            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
        Return resultado
    End Function

    Public Function RegistrarReprocesosRangos() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim db As LMDataAccessLayer.LMDataAccess
        Try
            db = New LMDataAccess
            With db
                With .SqlParametros
                    .Add("@idTipoProducto", SqlDbType.BigInt).Value = _idTipoProducto
                    .Add("@idOrden", SqlDbType.BigInt).Value = _idOrden
                    .Add("@iccidInicial", SqlDbType.VarChar, 20).Value = _iccidInicial
                    .Add("@iccidFinal", SqlDbType.VarChar, 20).Value = _iccidFinal
                    .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .Add("@caja", SqlDbType.BigInt).Value = _caja
                    .Add("@estiba", SqlDbType.BigInt).Value = _estiba
                    .Add("@linea", SqlDbType.SmallInt).Value = _linea
                    .Add("@idProducto", SqlDbType.Int).Value = _idProducto
                    If _noConformidad IsNot Nothing AndAlso _noConformidad.Count > 0 Then _
                        .Add("@listaNoConformidades", SqlDbType.VarChar).Value = Join(_noConformidad.ToArray(), ",")
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@mensaje", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output
                End With
                .ejecutarNonQuery("RegistrarReprocesosRangoLectura", CommandType.StoredProcedure)
                If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                    resultado.Valor = .SqlParametros("@resultado").Value
                Else
                    resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta del servidor. Por favor intente nuevamente.")
                End If
            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try

        Return resultado
    End Function

    Public Function ValidarOtb() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@idTipoProducto", SqlDbType.Int).Value = _idTipoProducto
                    .Add("@idOrden", SqlDbType.Int).Value = _idOrden
                    .Add("@idOrdenBodegaje", SqlDbType.Int).Value = _idOrdenBodegaje
                    .Add("@mensaje", SqlDbType.VarChar, 2500).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .ejecutarNonQuery("ValidarRangoOtbReprocesos", CommandType.StoredProcedure)
                If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                    resultado.Valor = .SqlParametros("@resultado").Value
                Else
                    resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta del servidor. Por favor intente nuevamente.")
                End If
            End With
        Catch ex As Exception
            If dbManager IsNot Nothing Then dbManager.Dispose()
            Throw New Exception
        End Try
        Return resultado
    End Function

    Public Function RegistrarReprocesosOtb() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@idTipoProducto", SqlDbType.BigInt).Value = _idTipoProducto
                    .Add("@idOrden", SqlDbType.BigInt).Value = _idOrden
                    .Add("@idOrdenBodegaje", SqlDbType.Int).Value = _idOrdenBodegaje
                    .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .Add("@caja", SqlDbType.BigInt).Value = _caja
                    .Add("@estiba", SqlDbType.BigInt).Value = _estiba
                    .Add("@linea", SqlDbType.SmallInt).Value = _linea
                    .Add("@idProducto", SqlDbType.Int).Value = _idProducto
                    If _noConformidad IsNot Nothing AndAlso _noConformidad.Count > 0 Then _
                        .Add("@listaNoConformidades", SqlDbType.VarChar).Value = Join(_noConformidad.ToArray(), ",")
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@mensaje", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output
                End With
                .ejecutarNonQuery("RegistrarReprocesosRangoOtb", CommandType.StoredProcedure)
                If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                    resultado.Valor = .SqlParametros("@resultado").Value
                Else
                    resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta del servidor. Por favor intente nuevamente.")
                End If
            End With
        Catch ex As Exception
            If dbManager IsNot Nothing Then dbManager.Dispose()
            Throw New Exception
        End Try
        Return resultado
    End Function

#End Region

End Class
