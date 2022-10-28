Imports ILSBusinessLayer
Imports LMDataAccessLayer

Public Class DetalleMaterialServicioMensajeria

#Region "Atributos (Campos)"

    Private _idMaterialServicio As Integer
    Private _idServicio As Integer
    Private _idTipoServicio As Integer
    Private _material As String
    Private _descripcionMaterial As String
    Private _cantidad As Integer
    Private _cantidadLeida As Integer
    Private _cantidadCambio As Integer
    Private _cantidadDisponible As Integer
    Private _idUsuarioRegistra As Integer
    Private _fechaRegistro As Date
    Private _tieneDisponibilidad As Boolean
    Private _idProducto As Integer
    Private _idTipoProducto As Integer
    Private _fechaDevolucion As Date
    Private _esSerializado As Boolean
    Private _registrado As Boolean
    Private _productoFinNoSerializado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idMaterialServicio As Integer)
        MyBase.New()
        _idMaterialServicio = idMaterialServicio
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdMaterialServicio() As Integer
        Get
            Return _idMaterialServicio
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idMaterialServicio = value
        End Set
    End Property

    Public Property IdServicio() As Integer
        Get
            Return _idServicio
        End Get
        Set(ByVal value As Integer)
            _idServicio = value
        End Set
    End Property

    Public Property IdTipoServicio() As Integer
        Get
            Return _idTipoServicio
        End Get
        Set(ByVal value As Integer)
            _idTipoServicio = value
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

    Public Property DescripcionMaterial() As String
        Get
            Return _descripcionMaterial
        End Get
        Set(ByVal value As String)
            _descripcionMaterial = value
        End Set
    End Property

    Public Property Cantidad() As Integer
        Get
            Return _cantidad
        End Get
        Set(ByVal value As Integer)
            _cantidad = value
        End Set
    End Property

    Public Property CantidadLeida() As Integer
        Get
            Return _cantidadLeida
        End Get
        Protected Friend Set(ByVal value As Integer)
            _cantidadLeida = value
        End Set
    End Property

    Public Property CantidadCambio() As Integer
        Get
            Return _cantidadCambio
        End Get
        Protected Friend Set(ByVal value As Integer)
            _cantidadCambio = value
        End Set
    End Property

    Public Property CantidadDisponible() As Integer
        Get
            Return _cantidadDisponible
        End Get
        Protected Friend Set(ByVal value As Integer)
            _cantidadDisponible = value
        End Set
    End Property

    Public Property TieneDisponibilidad() As Boolean
        Get
            Return _tieneDisponibilidad
        End Get
        Set(ByVal value As Boolean)
            _tieneDisponibilidad = value
        End Set
    End Property

    Public Property IdUsuarioRegistra() As Integer
        Get
            Return _idUsuarioRegistra
        End Get
        Set(ByVal value As Integer)
            _idUsuarioRegistra = value
        End Set
    End Property

    Public Property FechaRegistro() As Date
        Get
            Return _fechaRegistro
        End Get
        Protected Friend Set(ByVal value As Date)
            _fechaRegistro = value
        End Set
    End Property

    Public Property IdProducto As Integer
        Get
            Return _idProducto
        End Get
        Set(value As Integer)
            _idProducto = value
        End Set
    End Property

    Public Property IdTipoProducto As Integer
        Get
            Return _idTipoProducto
        End Get
        Set(value As Integer)
            _idTipoProducto = value
        End Set
    End Property

    Public Property FechaDevolucion As Date
        Get
            Return _fechaDevolucion
        End Get
        Protected Friend Set(value As Date)
            _fechaDevolucion = value
        End Set
    End Property

    Public Property EsSerializado As Boolean
        Get
            Return _esSerializado
        End Get
        Set(value As Boolean)
            _esSerializado = value
        End Set
    End Property

    Public Property Registrado() As Boolean
        Get
            Return _registrado
        End Get
        Set(ByVal value As Boolean)
            _registrado = value
        End Set
    End Property

    Public Property ProductoFinNoSerializado As Boolean
        Get
            Return _productoFinNoSerializado
        End Get
        Set(ByVal value As Boolean)
            _productoFinNoSerializado = value
        End Set
    End Property


#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Add("@idMaterialServicio", SqlDbType.Int).Value = _idMaterialServicio
                .ejecutarReader("ObtenerDetalleMaterialServicioMensajeria", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    If .Reader.Read Then CargarResultadoConsulta(.Reader)
                    If Not .Reader.IsClosed Then .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                If Not IsDBNull(reader("idMaterialServicio")) Then Integer.TryParse(reader("idMaterialServicio").ToString, _idMaterialServicio)
                Integer.TryParse(reader("idTipoServicio").ToString, _idTipoServicio)
                _material = reader("material").ToString
                _descripcionMaterial = reader("descripcionMaterial").ToString
                Integer.TryParse(reader("cantidad"), _cantidad)
                Integer.TryParse(reader("cantidadLeida"), _cantidadLeida)
                Integer.TryParse(reader("cantidadCambio"), _cantidadCambio)
                Integer.TryParse(reader("cantidadDisponible"), _cantidadDisponible)
                Integer.TryParse(reader("idUsuarioRegistra"), _idUsuarioRegistra)
                _tieneDisponibilidad = CBool(reader("tieneDisponibilidad"))
                _fechaRegistro = CDate(reader("fechaRegistro"))
                Integer.TryParse(reader("idProducto"), _idProducto)
                Integer.TryParse(reader("idTipoProducto"), _idTipoProducto)
                If Not IsDBNull(reader("fechaDevolucion")) Then _fechaDevolucion = CDate(reader("fechaDevolucion"))
                If Not IsDBNull(reader("esSerializado")) Then _esSerializado = CBool(reader("esSerializado"))
                If Not IsDBNull(reader("productoFinNoSerializado")) Then _productoFinNoSerializado = CBool(reader("productoFinNoSerializado"))
                _registrado = True
            End If
        End If

    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function Adicionar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If _idServicio > 0 AndAlso Not String.IsNullOrEmpty(_material) AndAlso _idUsuarioRegistra > 0 AndAlso _cantidad > 0 Then
            Dim dbManager As New LMDataAccess
            With dbManager
                Try
                    .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicio
                    If _idTipoServicio > 0 Then .SqlParametros.Add("@idTipoServicio", SqlDbType.Int).Value = _idTipoServicio
                    .SqlParametros.Add("@material", SqlDbType.VarChar).Value = _material
                    .SqlParametros.Add("@cantidad", SqlDbType.Int).Value = _cantidad
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuarioRegistra
                    .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                    .iniciarTransaccion()
                    .ejecutarNonQuery("AdicionarReferenciaServicioMensajeria", CommandType.StoredProcedure)
                    If Not IsDBNull(.SqlParametros("@resultado").Value) Then
                        resultado.Valor = CInt(.SqlParametros("@resultado").Value)
                        If resultado.Valor = 0 Then
                            .confirmarTransaccion()
                        Else
                            If resultado.Valor = 1 Then
                                resultado.Mensaje = "El registro proporcionado ya existe. Por favor verifique"
                            Else
                                resultado.Mensaje = "Ocurrió un error inesperado al tratar de registrar referencia. Por favor intente nuevamente"
                            End If
                            .abortarTransaccion()
                        End If
                    Else
                        Throw New Exception("Imposible evaluar la respuesta del servidor. Por favor intente nuevamente")
                    End If
                Catch ex As Exception
                    .abortarTransaccion()
                    Throw ex
                End Try
            End With
            dbManager.Dispose()
        Else
            resultado.EstablecerMensajeYValor(10, "No se han proporcionado los parámetros mínimos para realizar la actualización.")
        End If

        Return resultado
    End Function

    Public Function Modificar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If _idUsuarioRegistra > 0 AndAlso _idMaterialServicio > 0 Then
            'Using dbManager As New LMDataAccess
            Dim dbManager As New LMDataAccess
            With dbManager
                Try
                    .SqlParametros.Add("@idMaterialServicio", SqlDbType.Int).Value = _idMaterialServicio
                    .SqlParametros.Add("@material", SqlDbType.VarChar).Value = _material
                    .SqlParametros.Add("@cantidad", SqlDbType.Int).Value = _cantidad
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuarioRegistra
                    .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                    .iniciarTransaccion()
                    .ejecutarNonQuery("ModificarReferenciaServicioMensajeria", CommandType.StoredProcedure)
                    If Not IsDBNull(.SqlParametros("@resultado").Value) Then
                        resultado.Valor = CInt(.SqlParametros("@resultado").Value)
                        If resultado.Valor = 0 Then
                            .confirmarTransaccion()
                        Else
                            If resultado.Valor = 1 Then
                                resultado.Mensaje = "El registro proporcionado no existe. Por favor verifique"
                            Else
                                resultado.Mensaje = "Ocurrió un error inesperado al tratar de actualizar registro. Por favor intente nuevamente"
                            End If
                            .abortarTransaccion()
                        End If
                    Else
                        Throw New Exception("Imposible evaluar la respuesta del servidor. Por favor intente nuevamente")
                    End If
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso .estadoTransaccional Then .abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                End Try
            End With
            dbManager.Dispose()
            'End Using
        Else
            resultado.EstablecerMensajeYValor(10, "No se han proporcionado los parámetros mínimos para realizar la actualización.")
        End If
        Return resultado
    End Function

    Public Function Eliminar(ByVal idUsuario As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If _idMaterialServicio > 0 AndAlso idUsuario > 0 Then
            Dim dbManager As New LMDataAccess
            With dbManager
                Try
                    .SqlParametros.Add("@idMaterialServicio", SqlDbType.Int).Value = _idMaterialServicio
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                    .iniciarTransaccion()
                    .ejecutarNonQuery("EliminarReferenciaServicioMensajeria", CommandType.StoredProcedure)
                    If Not IsDBNull(.SqlParametros("@resultado").Value) Then
                        resultado.Valor = CInt(.SqlParametros("@resultado").Value)
                        If resultado.Valor = 0 Then
                            resultado.Mensaje = "Eliminación de referecia exitosa."
                            .ConfirmarTransaccion()
                        Else
                            If resultado.Valor = 1 Then
                                resultado.Mensaje = "El registro proporcionado no existe. Por favor verifique"
                            Else
                                resultado.Mensaje = "Ocurrió un error inesperado al tratar de eliminar registro. Por favor intente nuevamente"
                            End If
                            .AbortarTransaccion()
                        End If
                    Else
                        Throw New Exception("Imposible evaluar la respuesta del servidor. Por favor intente nuevamente")
                    End If
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso .estadoTransaccional Then .abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                End Try
            End With
            dbManager.Dispose()
        Else
            resultado.EstablecerMensajeYValor(10, "No se han proporcionado todos los datos requeridos para realizar la eliminación.")
        End If
        Return resultado
    End Function

#End Region

End Class
