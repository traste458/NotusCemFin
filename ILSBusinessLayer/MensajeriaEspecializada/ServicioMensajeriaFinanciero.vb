Imports ILSBusinessLayer.MensajeriaEspecializada
Imports LMDataAccessLayer
Imports System.Text

Namespace MensajeriaEspecializada

    Public Class ServicioMensajeriaFinanciero
        Inherits ServicioMensajeria
        Implements IServicioMensajeria

#Region "Atributos"

        Private _idCampania As Integer
        Private _listProductos As List(Of Integer)
        Private _listTipoServicio As List(Of String)
        Private _listCupoProducto As List(Of String)

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

        Public Property ListProductos As List(Of Integer)
            Get
                If _listProductos Is Nothing Then _listProductos = New List(Of Integer)
                Return _listProductos
            End Get
            Set(value As List(Of Integer))
                _listProductos = value
            End Set
        End Property

        Public Property ListTipoServicio As List(Of String)
            Get
                If _listTipoServicio Is Nothing Then _listTipoServicio = New List(Of String)
                Return _listTipoServicio
            End Get
            Set(value As List(Of String))
                _listTipoServicio = value
            End Set
        End Property


        Public Property ListCupoProducto As List(Of String)
            Get
                If _listCupoProducto Is Nothing Then _listCupoProducto = New List(Of String)
                Return _listCupoProducto
            End Get
            Set(value As List(Of String))
                _listCupoProducto = value
            End Set
        End Property

        Public Property NombresCompleto As Object
        Public Property PrimerApellido As Object
        Public Property SegundoApellido As Object
        Public Property CodigoEstrategiaComercial As Object
        Public Property Sexo As Object
        Public Property Celular As Object
        Public Property TelefonoAdicional As Object
        Public Property CodigoAgenteVendedor As Object
        Public Property Correo As Object

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region "Métodos Privados"

#End Region

#Region "Métodos Públicos"

        Public Function RegistrarServicioWS() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@fechaAgenda", SqlDbType.Date).Value = _fechaAgenda
                        .Add("@idJornada", SqlDbType.Int).Value = _idJornada
                        .Add("@idEmpresa", SqlDbType.Int).Value = _idEmpresa
                        .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        .Add("@nombre", SqlDbType.VarChar, 1000).Value = _nombreCliente
                        .Add("@identificacion", SqlDbType.VarChar, 20).Value = _identificacionCliente
                        .Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                        .Add("@direccion", SqlDbType.VarChar, 1000).Value = _direccion
                        .Add("@telefono", SqlDbType.VarChar, 20).Value = _telefonoContacto
                        .Add("@idCampania", SqlDbType.Int).Value = IdCampania
                        .Add("@actividadLaboral", SqlDbType.VarChar).Value = _actividadLaboral
                        .Add("@codOficinaCliente", SqlDbType.VarChar).Value = _codOficinaCliene
                        .Add("@observacion", SqlDbType.VarChar).Value = _observacion

                        .Add("@nombresCompleto", SqlDbType.VarChar).Value = NombresCompleto
                        .Add("@primerApellido", SqlDbType.VarChar).Value = PrimerApellido
                        .Add("@segundoApellido", SqlDbType.VarChar).Value = SegundoApellido
                        .Add("@codigoEstrategia", SqlDbType.VarChar).Value = CodigoEstrategiaComercial
                        .Add("@sexo", SqlDbType.VarChar).Value = Sexo
                        .Add("@celular", SqlDbType.VarChar).Value = Celular
                        .Add("@telefonoAdicional", SqlDbType.VarChar).Value = TelefonoAdicional
                        .Add("@codAgenteVendedor", SqlDbType.VarChar).Value = CodigoAgenteVendedor
                        .Add("@correo", SqlDbType.VarChar).Value = Correo

                        If _listProductos IsNot Nothing AndAlso _listProductos.Count > 0 Then _
                            .Add("@listProductos", SqlDbType.VarChar).Value = String.Join(",", _listProductos.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                        If _listTipoServicio IsNot Nothing AndAlso _listTipoServicio.Count > 0 Then _
                            .Add("@listTipoServicio", SqlDbType.VarChar, 2000).Value = String.Join(",", _listTipoServicio.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                        If _listCupoProducto IsNot Nothing AndAlso _listCupoProducto.Count > 0 Then _
                            .Add("@listCupoProducto", SqlDbType.VarChar, 2000).Value = String.Join(",", _listCupoProducto.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@idServicio", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .IniciarTransaccion()
                    .EjecutarNonQuery("RegistrarServicioWS", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        If resultado.Valor = 0 Then
                            .ConfirmarTransaccion()
                            _idServicioMensajeria = .SqlParametros("@idServicio").Value
                        Else
                            .AbortarTransaccion()
                        End If
                    Else
                        .AbortarTransaccion()
                        resultado.EstablecerMensajeYValor(400, "No se pudo establecer la respuesta del servidor, por favor intentelo nuevamente.")
                    End If
                End With
            Catch ex As Exception
                If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                resultado.EstablecerMensajeYValor(400, "Se presento un error al generar el registro: " & ex.Message)
            End Try
            Return resultado
        End Function

        Public Function ActualizarServicioWS() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idServicioMensajeria", SqlDbType.BigInt).Value = _idServicioMensajeria
                        If _fechaAgenda > Date.MinValue Then .Add("@fechaAgenda", SqlDbType.Date).Value = _fechaAgenda
                        If _idJornada > 0 Then .Add("@idJornada", SqlDbType.Int).Value = _idJornada
                        If _idEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        If Not String.IsNullOrEmpty(_nombreCliente) Then .Add("@nombre", SqlDbType.VarChar, 1000).Value = _nombreCliente
                        If Not String.IsNullOrEmpty(_identificacionCliente) Then .Add("@identificacion", SqlDbType.VarChar, 20).Value = _identificacionCliente
                        If _idCiudad > 0 Then .Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                        If Not String.IsNullOrEmpty(_direccion) Then .Add("@direccion", SqlDbType.VarChar, 1000).Value = _direccion
                        If Not String.IsNullOrEmpty(_telefonoContacto) Then .Add("@telefono", SqlDbType.VarChar, 20).Value = _telefonoContacto
                        If _idCampania > 0 Then .Add("@idCampania", SqlDbType.Int).Value = IdCampania
                        If _idTipoServicio > 0 Then .Add("@idTipoServicio", SqlDbType.Int).Value = _idTipoServicio
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .IniciarTransaccion()
                    .EjecutarNonQuery("ActualizarServicioWS", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        If resultado.Valor = 0 Then
                            .ConfirmarTransaccion()
                        Else
                            .AbortarTransaccion()
                        End If
                    Else
                        .AbortarTransaccion()
                        resultado.EstablecerMensajeYValor(400, "No se pudo establecer la respuesta del servidor, por favor intentelo nuevamente.")
                    End If
                End With
            Catch ex As Exception
                If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                resultado.EstablecerMensajeYValor(400, "Se presento un error al generar el registro: " & ex.Message)
            End Try
            Return resultado
        End Function

        Public Function ActualizarServicioWSEmergia() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idServicioMensajeria", SqlDbType.BigInt).Value = _idServicioMensajeria
                        If _fechaAgenda > Date.MinValue Then .Add("@fechaAgenda", SqlDbType.Date).Value = _fechaAgenda
                        If _idJornada > 0 Then .Add("@idJornada", SqlDbType.Int).Value = _idJornada
                        If Not String.IsNullOrEmpty(_telefonoContacto) Then .Add("@telefono", SqlDbType.VarChar, 20).Value = _telefonoContacto
                        If Not String.IsNullOrEmpty(_telefonoFijo) Then .Add("@telefonoFijo", SqlDbType.VarChar, 20).Value = _telefonoFijo
                        If _idCiudad > 0 Then .Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                        If _observacion <> "" Then .Add("@observacion", SqlDbType.VarChar, (500)).Value = _observacion
                        If _direccion <> "" Then .Add("@direccion", SqlDbType.VarChar, (500)).Value = _direccion
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .IniciarTransaccion()
                    .EjecutarNonQuery("ActualizarServicioWSEmergia", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        If resultado.Valor = 0 Then
                            .ConfirmarTransaccion()
                        Else
                            .AbortarTransaccion()
                        End If
                    Else
                        .AbortarTransaccion()
                        resultado.EstablecerMensajeYValor(400, "No se pudo establecer la respuesta del servidor, por favor intentelo nuevamente.")
                    End If
                End With
            Catch ex As Exception
                If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                resultado.EstablecerMensajeYValor(400, "Se presento un error al generar el registro: " & ex.Message)
            End Try
            Return resultado
        End Function


        Public Function AgregarReferenciaWS() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idServicioMensajeria", SqlDbType.BigInt).Value = _idServicioMensajeria
                        If _listProductos IsNot Nothing AndAlso _listProductos.Count > 0 Then _
                            .Add("@listProductos", SqlDbType.VarChar).Value = String.Join(",", _listProductos.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                        If _listTipoServicio IsNot Nothing AndAlso _listTipoServicio.Count > 0 Then _
                            .Add("@listTipoServicio", SqlDbType.VarChar, 2000).Value = String.Join(",", _listTipoServicio.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                        If _listCupoProducto IsNot Nothing AndAlso _listCupoProducto.Count > 0 Then _
                            .Add("@listCupoProducto", SqlDbType.VarChar, 2000).Value = String.Join(",", _listCupoProducto.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .IniciarTransaccion()
                    .EjecutarNonQuery("AgregarReferenciaWS", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        .ConfirmarTransaccion()
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                    Else
                        .AbortarTransaccion()
                        resultado.EstablecerMensajeYValor(400, "No se pudo establecer la respuesta del servidor, por favor intentelo nuevamente.")
                    End If
                End With
            Catch ex As Exception
                If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                resultado.EstablecerMensajeYValor(400, "Se presento un error al generar el registro: " & ex.Message)
            End Try
            Return resultado
        End Function

        Public Function EliminarReferenciaWS() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idServicioMensajeria", SqlDbType.BigInt).Value = _idServicioMensajeria
                        If _listProductos IsNot Nothing AndAlso _listProductos.Count > 0 Then _
                            .Add("@listProductos", SqlDbType.VarChar).Value = String.Join(",", _listProductos.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                        If _listTipoServicio IsNot Nothing AndAlso _listTipoServicio.Count > 0 Then _
                            .Add("@listTipoServicio", SqlDbType.VarChar, 2000).Value = String.Join(",", _listTipoServicio.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .IniciarTransaccion()
                    .EjecutarNonQuery("EliminarReferenciaWS", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        .ConfirmarTransaccion()
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                    Else
                        .AbortarTransaccion()
                        resultado.EstablecerMensajeYValor(400, "No se pudo establecer la respuesta del servidor, por favor intentelo nuevamente.")
                    End If
                End With
            Catch ex As Exception
                If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                resultado.EstablecerMensajeYValor(400, "Se presento un error al generar el registro: " & ex.Message)
            End Try
            Return resultado
        End Function

        Public Function ValidarServicio() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idCampania", SqlDbType.Int).Value = IdCampania
                        If _idCiudad > 0 Then .Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                        If _listProductos IsNot Nothing AndAlso _listProductos.Count > 0 Then _
                            .Add("@listProductos", SqlDbType.VarChar).Value = String.Join(",", _listProductos.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .IniciarTransaccion()
                    .EjecutarNonQuery("ValidarServicio", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        If resultado.Valor = 0 Then
                            .ConfirmarTransaccion()
                        Else
                            .AbortarTransaccion()
                        End If
                    Else
                        .AbortarTransaccion()
                        resultado.EstablecerMensajeYValor(400, "No se pudo establecer la respuesta del servidor, por favor intentelo nuevamente.")
                    End If
                End With
            Catch ex As Exception
                If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                resultado.EstablecerMensajeYValor(400, "Se presento un error al generar el registro: " & ex.Message)
            End Try
            Return resultado
        End Function

#End Region

    End Class

End Namespace