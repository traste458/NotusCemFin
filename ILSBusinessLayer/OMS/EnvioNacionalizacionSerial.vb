Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras

Namespace OMS

    Public Class EnvioNacionalizacionSerial

#Region "Atributos"

        Private _idEnvioSerial As Long
        Private _idDetalleEnvio As Long
        Private _idEnvio As Long
        Private _idOrden As Long
        Private _serial As String
        Private _numeroNacionalizacion As Long
        Private _pedido As Long
        Private _entrega As Long
        Private _contabilizacion As Long
        Private _material As String
        Private _centro As String
        Private _cantidadCargue As Integer
        Private _cantidadRegistrosCargue As Long

#End Region

#Region "Constructores"

        Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            _idEnvioSerial = identificador
            CargarInformacion()
        End Sub
#End Region

#Region "Propiedades"
        Public Property IdEnvioSerial() As Long
            Get
                Return _idEnvioSerial
            End Get
            Set(ByVal value As Long)
                _idEnvioSerial = value
            End Set
        End Property

        Public Property IdDetalleEnvio() As Long
            Get
                Return _idDetalleEnvio
            End Get
            Set(ByVal value As Long)
                _idDetalleEnvio = value
            End Set
        End Property

        Public Property Serial() As String
            Get
                Return _serial
            End Get
            Set(ByVal value As String)
                _serial = value
            End Set
        End Property

        Public Property NumeroNacionalizacion() As Long
            Get
                Return _numeroNacionalizacion
            End Get
            Set(ByVal value As Long)
                _numeroNacionalizacion = value
            End Set
        End Property

        Public Property IdOrden() As Long
            Get
                Return _idOrden
            End Get
            Set(ByVal value As Long)
                _idOrden = value
            End Set
        End Property

        Public Property IdEnvio() As Long
            Get
                Return _idEnvio
            End Get
            Set(ByVal value As Long)
                _idEnvio = value
            End Set
        End Property

        Public Property Pedido() As Long
            Get
                Return _pedido
            End Get
            Set(ByVal value As Long)
                _pedido = value
            End Set
        End Property

        Public Property Entrega() As Long
            Get
                Return _entrega
            End Get
            Set(ByVal value As Long)
                _entrega = value
            End Set
        End Property

        Public Property Contabilizacion() As Long
            Get
                Return _contabilizacion
            End Get
            Set(ByVal value As Long)
                _contabilizacion = value
            End Set
        End Property

        Public Property Material() As String
            Get
                Return _material
            End Get
            Set(ByVal value As String)
                _material = value
            End Set
        End Property

        Public Property Centro() As String
            Get
                Return _centro
            End Get
            Set(ByVal value As String)
                _centro = value
            End Set
        End Property

        Public Property CantidadCargue() As String
            Get
                Return _cantidadCargue
            End Get
            Set(ByVal value As String)
                _cantidadCargue = value
            End Set
        End Property

        Public Property CantidadRegistrosCargue() As String
            Get
                Return _cantidadRegistrosCargue
            End Get
            Set(ByVal value As String)
                _cantidadRegistrosCargue = value
            End Set
        End Property

#End Region

#Region "Metodos Publicos"

        Public Sub Crear()
            Dim db As New LMDataAccessLayer.LMDataAccess
            With db
                With .SqlParametros
                    .Add("@idDetalleEnvio", SqlDbType.BigInt).Value = _idDetalleEnvio
                    .Add("@serial", SqlDbType.VarChar, 20).Value = _serial
                    .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                End With

                Try
                    Dim result As Integer = 0
                    .iniciarTransaccion()
                    .ejecutarNonQuery("CrearEnvioNacionalizacionSerial", CommandType.StoredProcedure)
                    result = .SqlParametros("@result").Value
                    If result = 0 Then
                        _idEnvioSerial = CLng(.SqlParametros("@identity").Value)
                        .confirmarTransaccion()
                    Else
                        Throw New Exception("Imposible registrar la información del serial relacionado con el detalle de la nacionalizacion en la Base de Datos.")
                    End If
                Catch ex As Exception
                    If .estadoTransaccional Then .abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
        End Sub

        Public Function ActualizarSerialesEntrega() As Short
            Dim resultado As Short = 0

            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        '.Add("@idEnvio", SqlDbType.BigInt).Value = _idEnvio
                        .Add("@idOrden", SqlDbType.BigInt).Value = _idOrden
                        .Add("@pedido", SqlDbType.BigInt).Value = _pedido
                        .Add("@entrega", SqlDbType.BigInt).Value = _entrega
                        .Add("@cantidadCargue", SqlDbType.Int).Value = _cantidadCargue
                        .Add("@material", SqlDbType.VarChar, 20).Value = _material
                        .Add("@centro", SqlDbType.VarChar, 10).Value = _centro
                        .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With

                    .iniciarTransaccion()
                    .ejecutarNonQuery("ActualizarSerialesEntrega", CommandType.StoredProcedure)

                    Short.TryParse(.SqlParametros("@returnValue").Value.ToString(), resultado)

                    If resultado = 0 Then
                        .confirmarTransaccion()
                    Else
                        If .estadoTransaccional Then .abortarTransaccion()
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return resultado
        End Function

        Public Function ActualizarSerialesContabilizacion() As Short
            Dim resultado As Short = 0

            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idOrden", SqlDbType.BigInt).Value = _idOrden
                        .Add("@contabilizacion", SqlDbType.BigInt).Value = _contabilizacion
                        .Add("@entrega", SqlDbType.BigInt).Value = _entrega
                        .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .TiempoEsperaComando = 1200
                    .iniciarTransaccion()
                    .ejecutarNonQuery("ActualizarSerialesContabilizacion", CommandType.StoredProcedure)

                    Short.TryParse(.SqlParametros("@returnValue").Value.ToString(), resultado)

                    If resultado = 0 Then .confirmarTransaccion()

                    If .estadoTransaccional Then .abortarTransaccion()
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return resultado
        End Function

        Public Function ActualizarSerialesEnvioEnTablaBase() As ResultadoProceso
            Dim rp As New ResultadoProceso(0, "Ejecución Satisfactoria")

            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Clear()
                        .Add("@idOrden", SqlDbType.BigInt).Value = IdOrden
                        .Add("@idEnvioSerial", SqlDbType.BigInt).Value = IdEnvioSerial
                        .Add("@idEnvioSerialMax", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .TiempoEsperaComando = 1200
                    .iniciarTransaccion()
                    .ejecutarNonQuery("ActualizarSerialesEnvioEnTablaBase", CommandType.StoredProcedure)

                    rp.EstablecerMensajeYValor(.SqlParametros("@returnValue").Value.ToString(), " No fue posible actualizar los seriales del la orden " & _idOrden.ToString & " en la tabla base")

                    If rp.Valor = 0 Then
                        If Long.TryParse(.SqlParametros("@idEnvioSerialMax").Value.ToString(), IdEnvioSerial) Then
                            .confirmarTransaccion()
                        Else
                            rp.EstablecerMensajeYValor(5, "No fue posible obtener el ultimo serial del bloque actualizado.")
                        End If
                    Else
                        Select Case rp.Valor
                            Case 1 : rp.EstablecerMensajeYValor(1, "No se encontraron seriales para actualizar en bloque")
                            Case 2 : rp.EstablecerMensajeYValor(2, "Error al actualizar el estado de la orden")
                        End Select
                    End If

                    If .estadoTransaccional Then .abortarTransaccion()
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return rp
        End Function

        Public Function ObtenerCantidadOrdenNacionalizacion() As ResultadoProceso
            Dim rp As New ResultadoProceso

            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idOrden", SqlDbType.BigInt).Value = IdOrden
                        .Add("@cantidadRegistrosCargue", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .TiempoEsperaComando = 1200
                    .iniciarTransaccion()
                    .ejecutarNonQuery("ObtenerCantidadOrdenNacionalizacion", CommandType.StoredProcedure)

                    rp.EstablecerMensajeYValor(CInt(.SqlParametros("@returnValue").Value.ToString()), "ResultadoEjecucion")

                    If rp.Valor = 0 Then
                        CantidadRegistrosCargue = CLng(.SqlParametros("@cantidadRegistrosCargue").Value.ToString())
                        .confirmarTransaccion()
                    Else
                        Select Case rp.Valor
                            Case 1 : rp.EstablecerMensajeYValor(rp.Valor, "No se encontraron registros relacionados a la orden " & IdOrden)
                        End Select
                    End If

                    If .estadoTransaccional Then .abortarTransaccion()
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return rp
        End Function

#End Region

#Region "Metodos Privados"

        Private Sub CargarInformacion()
        End Sub

#End Region

#Region "Métodos Compartidos"

        Public Overloads Shared Function ObtenerSerialPorDetalleEnvio(ByVal idDetalleEnvio As Long) As DataTable
            Dim dt As New DataTable()
            Dim db As New LMDataAccess
            With db
                With .SqlParametros
                    .Add("@idDetalleEnvio", SqlDbType.BigInt).Value = idDetalleEnvio
                End With
                Try
                    dt = .ejecutarDataTable("ObtenerSerialPorDetalleEnvio", CommandType.StoredProcedure)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    db.Dispose()
                End Try
            End With
            Return dt
        End Function

        Public Shared Function ObtenerSerialesOrden(ByVal filtro As FiltroEnvioNacionalizacionSerial, ByRef resultado As Short) As DataTable
            Dim db As New LMDataAccess
            Dim dt As New DataTable()

            Try
                With db
                    With .SqlParametros
                        '.Add("@IdEnvio", SqlDbType.BigInt).Value = filtro.IdEnvio
                        .Add("@IdOrden", SqlDbType.BigInt).Value = filtro.IdOrden
                        .Add("@entrega", SqlDbType.BigInt).Value = filtro.Entrega
                        .Add("@pedido", SqlDbType.BigInt).Value = filtro.Pedido
                        .Add("@material", SqlDbType.VarChar, 20).Value = filtro.Material
                        .Add("@centro", SqlDbType.VarChar, 10).Value = filtro.Centro
                        .Add("@cantidad", SqlDbType.Int).Value = filtro.Cantidad
                        .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With

                    .iniciarTransaccion()
                    'dt = .ejecutarDataTable("ObtenerSerialesEntrega", CommandType.StoredProcedure)
                    dt = .ejecutarDataTable("ObtenerSerialesOrden", CommandType.StoredProcedure)

                    Short.TryParse(.SqlParametros("@returnValue").Value.ToString(), resultado)

                    If resultado = 0 Then
                        .confirmarTransaccion()
                    Else
                        If .estadoTransaccional Then .abortarTransaccion()
                    End If
                End With
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                db.Dispose()
            End Try

            Return dt
        End Function

        Public Overloads Shared Function ObtenerListadoSerialesOrden() As DataTable
            Dim filtro As New FiltroEnvioNacionalizacionSerial
            Dim dtDatos As DataTable = ObtenerListadoSerialesOrden(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListadoSerialesOrden(ByVal filtro As FiltroEnvioNacionalizacionSerial) As DataTable
            Dim db As New LMDataAccess
            Dim dt As New DataTable()

            Try
                With db
                    With .SqlParametros
                        .Add("@IdOrden", SqlDbType.BigInt).Value = filtro.IdOrden
                        .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With

                    dt = .ejecutarDataTable("ObtenerListadoSerialesOrden", CommandType.StoredProcedure)
                End With
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                db.Dispose()
            End Try

            Return dt
        End Function

#End Region

    End Class

End Namespace