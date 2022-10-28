Imports LMDataAccessLayer
Imports ILSBusinessLayer


Public Class CargueSAP

    Private _idOrdenRecepcion As Integer

    Public Property IdOrdenRecepcion() As Integer
        Get
            Return _idOrdenRecepcion
        End Get
        Set(ByVal value As Integer)
            _idOrdenRecepcion = value
        End Set
    End Property

#Region "constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idOrdenRecepcion As Long)
        Me.New()
        _idOrdenRecepcion = idOrdenRecepcion
    End Sub
#End Region

#Region "Metodos Publicos"

    Public Function RegistrarCargueSAP(ByVal material() As SAPContabilizacionEntrada.ZmmLgMateriales, ByVal idUsuario As Integer, ByVal numeroDocumento As String) As Integer
        Dim db As New LMDataAccess
        Dim idLog As Integer = 0
        Try
            If _idOrdenRecepcion > 0 Then
                With db
                    With .SqlParametros
                        .Add("@idOrdenRecepcion", SqlDbType.Int).Value = _idOrdenRecepcion
                        .Add("@numeroDocumento", SqlDbType.VarChar).Value = numeroDocumento
                        .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .Add("@cantidad", SqlDbType.Int).Value = material.Length
                        .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    Dim result As Integer = 0
                    .TiempoEsperaComando = 1200
                    .IniciarTransaccion()
                    .EjecutarNonQuery("RegistrarCantidadesDeCargarSAPOrdenRecepcion", CommandType.StoredProcedure)
                    result = .SqlParametros("@result").Value
                    If result = 0 Then
                        idLog = CLng(.SqlParametros("@identity").Value)
                        For Each cargueSAP As SAPContabilizacionEntrada.ZmmLgMateriales In material
                            .SqlParametros.Clear()
                            With .SqlParametros
                                .Add("@idLog", SqlDbType.Int).Value = idLog
                                .Add("@almacen", SqlDbType.VarChar).Value = cargueSAP.almacen
                                .Add("@centro", SqlDbType.VarChar).Value = cargueSAP.centro
                                .Add("@material", SqlDbType.VarChar).Value = CInt(cargueSAP.material)
                                .Add("@cantidad", SqlDbType.Int).Value = cargueSAP.cantidad
                                .Add("@posDocumento", SqlDbType.Int).Value = cargueSAP.posDocumento
                                .Add("@unidad", SqlDbType.VarChar).Value = cargueSAP.unidad
                            End With
                            .EjecutarNonQuery("RegistrarDetalleCargaSAPOrdenRecepcion", CommandType.StoredProcedure)
                        Next
                    Else
                        Throw New Exception("Imposible registrar la información de la Orden en la Base de Datos.")
                    End If
                    If result = 0 Then .ConfirmarTransaccion()
                End With
            End If
            Return idLog
        Catch ex As Exception
            If db.EstadoTransaccional Then db.AbortarTransaccion()
            Throw New Exception(ex.Message)
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
    End Function

    Public Function RegistrarCargueSerializadoSAP(ByVal material() As SAPContabilizacionEntrada.ZmmLgMateriales, ByVal idUsuario As Integer, ByVal numeroDocumento As String, Optional ByVal tipoCaso As Integer = 2) As Integer
        Dim db As New LMDataAccess
        Dim idLog As Integer = 0
        Try
            If _idOrdenRecepcion > 0 Then
                With db
                    With .SqlParametros
                        .Add("@idOrdenRecepcion", SqlDbType.Int).Value = _idOrdenRecepcion
                        .Add("@numeroDocumento", SqlDbType.VarChar).Value = numeroDocumento
                        .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .Add("@cantidad", SqlDbType.Int).Value = material.Length
                        .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    Dim result As Integer = 0
                    .TiempoEsperaComando = 1200
                    .IniciarTransaccion()
                    .EjecutarNonQuery("RegistrarCantidadesDeCargarSAPOrdenRecepcion", CommandType.StoredProcedure)
                    result = .SqlParametros("@result").Value
                    If result = 0 Then
                        idLog = CLng(.SqlParametros("@identity").Value)
                        For Each cargueSAP As SAPContabilizacionEntrada.ZmmLgMateriales In material
                            .SqlParametros.Clear()
                            With .SqlParametros
                                .Add("@idCargue", SqlDbType.Int).Value = idLog
                                .Add("@material", SqlDbType.VarChar).Value = CInt(cargueSAP.material)
                                .Add("@serial", SqlDbType.VarChar).Value = cargueSAP.lote
                                .Add("@idOrdenRecepcion", SqlDbType.VarChar).Value = _idOrdenRecepcion
                            End With
                            If tipoCaso = 2 Then
                                .EjecutarNonQuery("ActualizarCargueSAPTarjetasPrepago", CommandType.StoredProcedure)
                            ElseIf tipoCaso = 4 Then
                                .EjecutarNonQuery("ActualizarInfoCargueProductoSAP", CommandType.StoredProcedure)
                            End If
                        Next

                        For Each cargueSAP As SAPContabilizacionEntrada.ZmmLgMateriales In material
                            .SqlParametros.Clear()
                            With .SqlParametros
                                .Add("@idLog", SqlDbType.Int).Value = idLog
                                .Add("@almacen", SqlDbType.VarChar).Value = cargueSAP.almacen
                                .Add("@centro", SqlDbType.VarChar).Value = cargueSAP.centro
                                .Add("@material", SqlDbType.VarChar).Value = CInt(cargueSAP.material)
                                .Add("@cantidad", SqlDbType.Int).Value = cargueSAP.cantidad
                                .Add("@posDocumento", SqlDbType.Int).Value = cargueSAP.posDocumento
                                .Add("@unidad", SqlDbType.VarChar).Value = cargueSAP.unidad
                            End With
                            .EjecutarNonQuery("RegistrarDetalleCargaSAPOrdenRecepcion", CommandType.StoredProcedure)
                        Next
                    Else
                        Throw New Exception("Imposible registrar la información de la Orden en la Base de Datos.")
                    End If
                    If result = 0 Then .ConfirmarTransaccion()
                End With
            End If
            Return idLog
        Catch ex As Exception
            If db.EstadoTransaccional Then db.AbortarTransaccion()
            Throw New Exception(ex.Message)
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
    End Function

    Public Function RegistrarCargueSerializadoSAPToken(ByVal material() As SAPContabilizacionEntrada.ZmmLgMateriales, ByVal seriales() As SAPContabilizacionEntrada.ZmmLgSerialnumber, ByVal idUsuario As Integer, ByVal numeroDocumento As String) As Boolean
        Dim db As New LMDataAccess
        Dim retorno As Boolean = True
        Dim idLog As Integer = 0
        Try
            With db
                With .SqlParametros
                    .Add("@idOrdenRecepcion", SqlDbType.Int).Value = _idOrdenRecepcion
                    .Add("@numeroDocumento", SqlDbType.VarChar).Value = numeroDocumento
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .Add("@cantidad", SqlDbType.Int).Value = seriales.Length
                    .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                Dim resultado As Integer = 0
                .TiempoEsperaComando = 1200
                .EjecutarNonQuery("RegistrarCantidadesDeCargarSAPOrdenRecepcion", CommandType.StoredProcedure)
                resultado = .SqlParametros("@resultado").Value
                If resultado = 0 Then
                    idLog = CLng(.SqlParametros("@identity").Value)
                    .IniciarTransaccion()
                    For Each cargueSAP As SAPContabilizacionEntrada.ZmmLgSerialnumber In seriales
                        .SqlParametros.Clear()
                        With .SqlParametros
                            .Add("@serial", SqlDbType.VarChar).Value = cargueSAP.noSerie
                            .Add("@numeroDocumento", SqlDbType.VarChar).Value = numeroDocumento
                            .Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        End With
                        .EjecutarNonQuery("ActualizarInfoCargueProductosSerial", CommandType.StoredProcedure)
                        Dim result As Integer = 0
                        result = .SqlParametros("@result").Value
                        If result <> 0 Then retorno = False
                    Next

                    For Each cargueSAP As SAPContabilizacionEntrada.ZmmLgMateriales In material
                        .SqlParametros.Clear()
                        With .SqlParametros
                            .Add("@idLog", SqlDbType.Int).Value = idLog
                            .Add("@almacen", SqlDbType.VarChar).Value = cargueSAP.almacen
                            .Add("@centro", SqlDbType.VarChar).Value = cargueSAP.centro
                            .Add("@material", SqlDbType.VarChar).Value = CInt(cargueSAP.material)
                            .Add("@cantidad", SqlDbType.Int).Value = cargueSAP.cantidad
                            .Add("@posDocumento", SqlDbType.Int).Value = cargueSAP.posDocumento
                            .Add("@unidad", SqlDbType.VarChar).Value = cargueSAP.unidad
                        End With
                        .EjecutarNonQuery("RegistrarDetalleCargaSAPOrdenRecepcion", CommandType.StoredProcedure)
                    Next

                    If retorno Then
                        .ConfirmarTransaccion()
                    Else
                        .AbortarTransaccion()
                    End If
                End If
            End With

            Return retorno
        Catch ex As Exception
            If db.EstadoTransaccional Then db.AbortarTransaccion()
            Throw New Exception(ex.Message)
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
    End Function

    Public Function RegistrarCargueSerializadoSAPBonos(ByVal seriales() As SAPContabilizacionEntrada.ZmmLgSerialnumber, ByVal idUsuario As Integer, ByVal numeroDocumento As String) As Boolean
        Dim db As New LMDataAccess
        Dim idLog As Integer = 0
        Try
            If _idOrdenRecepcion > 0 Then
                With db
                    With .SqlParametros
                        .Add("@idOrdenRecepcion", SqlDbType.Int).Value = _idOrdenRecepcion
                        .Add("@numeroDocumento", SqlDbType.VarChar).Value = numeroDocumento
                        .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .Add("@cantidad", SqlDbType.Int).Value = seriales.Length
                        .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    Dim result As Integer = 0
                    .TiempoEsperaComando = 1200
                    .IniciarTransaccion()
                    .EjecutarNonQuery("RegistrarCantidadesDeCargarSAPOrdenRecepcion", CommandType.StoredProcedure)
                    result = .SqlParametros("@result").Value
                    If result = 0 Then
                        idLog = CLng(.SqlParametros("@identity").Value)
                        For Each cargueSAP As SAPContabilizacionEntrada.ZmmLgSerialnumber In seriales
                            .SqlParametros.Clear()
                            With .SqlParametros
                                .Add("@idCargue", SqlDbType.Int).Value = idLog
                                .Add("@material", SqlDbType.VarChar).Value = CInt(cargueSAP.material)
                                .Add("@serial", SqlDbType.VarChar).Value = cargueSAP.noSerie
                                .Add("@idOrdenRecepcion", SqlDbType.VarChar).Value = _idOrdenRecepcion
                            End With

                            .EjecutarNonQuery("ActualizarInfoCargueProductoSAP", CommandType.StoredProcedure)
                        Next
                    Else
                        Throw New Exception("Imposible registrar la información de la Orden en la Base de Datos.")
                    End If
                    If result = 0 Then .ConfirmarTransaccion()
                End With
            End If
            Return idLog
        Catch ex As Exception
            If db.EstadoTransaccional Then db.AbortarTransaccion()
            Throw New Exception(ex.Message)
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
    End Function

#End Region

End Class
