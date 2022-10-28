Imports ILSBusinessLayer
Imports LMDataAccessLayer

Namespace Productos

    Public Class Material

#Region "Atributos (Campos)"
        Private _codigoOriginal As String
        Private _material As String
        Private _referencia As String
        Private _referenciaCliente As String
        Private _cantidadEmpaque As Integer
        Private _idProductoPadre As Integer
        Private _productoPadre As String
        Private _listaRegiones As String
        Private _idTecnologia As Short
        Private _tecnologia As String
        Private _idTipoOrden As Short
        Private _tipoOrden As String
        Private _codigoEan As String
        Private _esSim As Boolean
        Private _leerSim As Boolean
        Private _idEstado As Short
        Private _estado As String
        Private _tipoMaterial As String
        Private _idTipoProducto As Short
        Private _tipoProducto As String
        Private _unidadEmpaque As String
        Private _codigoEmpaque As String
        Private _esSerializado As Boolean
        Private _asignarMin As Boolean
        Private _idTipoEtiqueta As Short
        Private _tipoEtiqueta As String
        Private _color As String
        Private _registrado As Boolean
        Private _principal As Integer
        Private _idRegionOriginal As Integer
        Private _regionOriginal As String
#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _codigoOriginal = ""
            _material = ""
            _referencia = ""
            _referenciaCliente = ""
            _productoPadre = ""
            _tecnologia = ""
            _tipoOrden = ""
            _codigoEan = ""
            _estado = ""
            _tipoMaterial = ""
            _tipoProducto = ""
            _unidadEmpaque = ""
            _tipoEtiqueta = ""
			_color = ""
            _registrado = False
        End Sub

        Public Sub New(ByVal material As String)
            Me.New()
            CargarDatos(material.Trim)
        End Sub

#End Region

#Region "Propiedades"

        Public Property CodigoOriginal() As String
            Get
                Return _codigoOriginal
            End Get
            Protected Friend Set(ByVal value As String)
                _codigoOriginal = value
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

        Public Property Referencia() As String
            Get
                Return _referencia
            End Get
            Set(ByVal value As String)
                _referencia = value
            End Set
        End Property

        Public Property ReferenciaCliente() As String
            Get
                Return _referenciaCliente
            End Get
            Set(ByVal value As String)
                _referenciaCliente = value
            End Set
        End Property

        Public Property CantidadEmpaque() As Integer
            Get
                Return _cantidadEmpaque
            End Get
            Set(ByVal value As Integer)
                _cantidadEmpaque = value
            End Set
        End Property

        Public Property IdProductoPadre() As Integer
            Get
                Return _idProductoPadre
            End Get
            Set(ByVal value As Integer)
                _idProductoPadre = value
            End Set
        End Property

        Public Property ProductoPadre() As String
            Get
                Return _productoPadre
            End Get
            Protected Friend Set(ByVal value As String)
                _productoPadre = value
            End Set
        End Property

        Public Property ListaRegiones() As String
            Get
                Return _listaRegiones
            End Get
            Protected Friend Set(ByVal value As String)
                _listaRegiones = value
            End Set
        End Property

        Public Property IdTecnologia() As Short
            Get
                Return _idTecnologia
            End Get
            Protected Friend Set(ByVal value As Short)
                _idTecnologia = value
            End Set
        End Property

        Public Property Tecnologia() As String
            Get
                Return _tecnologia
            End Get
            Protected Friend Set(ByVal value As String)
                _tecnologia = value
            End Set
        End Property

        Public Property IdTipoOrden() As Short
            Get
                Return _idTipoOrden
            End Get
            Set(ByVal value As Short)
                _idTipoOrden = value
            End Set
        End Property

        Public Property TipoOrden() As String
            Get
                Return _tipoOrden
            End Get
            Protected Friend Set(ByVal value As String)
                _tipoOrden = value
            End Set
        End Property

        Public Property CodigoEan() As String
            Get
                Return _codigoEan
            End Get
            Set(ByVal value As String)
                _codigoEan = value
            End Set
        End Property

        Public Property EsSim() As Boolean
            Get
                Return _esSim
            End Get
            Protected Friend Set(ByVal value As Boolean)
                _esSim = value
            End Set
        End Property

        Public Property LeerSim() As Boolean
            Get
                Return _leerSim
            End Get
            Set(ByVal value As Boolean)
                _leerSim = value
            End Set
        End Property

        Public Property IdEstado() As Short
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Short)
                _idEstado = value
            End Set
        End Property

        Public Property Estado() As String
            Get
                Return _estado
            End Get
            Protected Friend Set(ByVal value As String)
                _estado = value
            End Set
        End Property

        Public Property TipoMaterial() As String
            Get
                Return _tipoMaterial
            End Get
            Protected Friend Set(ByVal value As String)
                _tipoMaterial = value
            End Set
        End Property

        Public Property IdTipoProducto() As Short
            Get
                Return _idTipoProducto
            End Get
            Protected Friend Set(ByVal value As Short)
                _idTipoProducto = value
            End Set
        End Property

        Public Property TipoProducto() As String
            Get
                Return _tipoProducto
            End Get
            Protected Friend Set(ByVal value As String)
                _tipoProducto = value
            End Set
        End Property

        Public Property UnidadEmpaque() As String
            Get
                Return _unidadEmpaque
            End Get
            Protected Friend Set(ByVal value As String)
                _unidadEmpaque = value
            End Set
        End Property

        Public Property CodigoEmpaque() As String
            Get
                Return _codigoEmpaque
            End Get
            Set(ByVal value As String)
                _codigoEmpaque = value
            End Set
        End Property

        Public Property EsSerializado() As Boolean
            Get
                Return _esSerializado
            End Get
            Protected Friend Set(ByVal value As Boolean)
                _esSerializado = value
            End Set
        End Property

        Public Property AsignarMin() As Boolean
            Get
                Return _asignarMin
            End Get
            Set(ByVal value As Boolean)
                _asignarMin = value
            End Set
        End Property

        Public Property IdTipoEtiqueta() As Short
            Get
                Return _idTipoEtiqueta
            End Get
            Set(ByVal value As Short)
                _idTipoEtiqueta = value
            End Set
        End Property

        Public Property TipoEtiqueta() As String
            Get
                Return _tipoEtiqueta
            End Get
            Protected Friend Set(ByVal value As String)
                _tipoEtiqueta = value
            End Set
        End Property

        Public Property Color() As String
            Get
                Return _color
            End Get
            Set(value As String)
                _color = value
            End Set
        End Property
        Public Property Registrado() As Boolean
            Get
                Return _registrado
            End Get
            Protected Friend Set(ByVal value As Boolean)
                _registrado = value
            End Set
        End Property

        Public Property Principal() As Integer
            Get
                Return _principal
            End Get
            Set(value As Integer)
                _principal = value
            End Set
        End Property

        Public Property IdRegionOriginal As Integer
            Get
                Return _idRegionOriginal
            End Get
            Set(value As Integer)
                _idRegionOriginal = value
            End Set
        End Property

        Public Property RegionOriginal As String
            Get
                Return _regionOriginal
            End Get
            Set(value As String)
                _regionOriginal = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos(ByVal material As String)
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@material", SqlDbType.VarChar, 20).Value = material
                    .ejecutarReader("ConsultarListadoMateriales", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            _codigoOriginal = .Reader("material").ToString
                            _material = .Reader("material").ToString
                            _referencia = .Reader("referencia").ToString
                            _referenciaCliente = .Reader("referenciaCliente").ToString
                            Integer.TryParse(.Reader("cantidadEmpaque").ToString, _cantidadEmpaque)
                            Integer.TryParse(.Reader("idProductoPadre").ToString, _idProductoPadre)
                            _productoPadre = .Reader("productoPadre").ToString
                            Short.TryParse(.Reader("idTecnologia").ToString, _idTecnologia)
                            _tecnologia = .Reader("tecnologia").ToString
                            Short.TryParse(.Reader("idTipoOrden").ToString, _idTipoOrden)
                            _tipoOrden = .Reader("tipoOrden").ToString
                            _codigoEan = .Reader("codigoEan").ToString
                            _esSim = CBool(.Reader("esSim").ToString)
                            _leerSim = CBool(.Reader("leerSim").ToString)
                            Short.TryParse(.Reader("idEstado").ToString, _idEstado)
                            _estado = .Reader("estado").ToString
                            _tipoMaterial = .Reader("tipoMaterial").ToString
                            Short.TryParse(.Reader("idTipoProducto").ToString, _idTipoProducto)
                            _tipoProducto = .Reader("tipoProducto").ToString
                            _unidadEmpaque = .Reader("unidadEmpaque").ToString
                            _codigoEmpaque = .Reader("codigoEmpaque").ToString
                            _esSerializado = CBool(.Reader("esSerializado").ToString)
                            _asignarMin = CBool(.Reader("asignarMin").ToString)
                            _listaRegiones = .Reader("listadoRegiones").ToString
                            Short.TryParse(.Reader("idTipoEtiqueta").ToString, _idTipoEtiqueta)
                            _tipoEtiqueta = .Reader("tipoEtiqueta").ToString
                            _color = .Reader("color").ToString
                            If .Reader("principal") Then
                                _principal = 1
                            Else
                                _principal = 0
                            End If
                            'Integer.TryParse(.Reader("principal").ToString, _principal)
                            If Not IsDBNull(.Reader("idRegionOriginal")) Then Integer.TryParse(.Reader("idRegionOriginal").ToString, _idRegionOriginal)
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

#Region "Métodos Publicos"

        Public Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If (Not String.IsNullOrEmpty(Me._material)) AndAlso (Not String.IsNullOrEmpty(Me._referencia)) AndAlso _
                Me._idTipoOrden > 0 AndAlso Me._cantidadEmpaque > 0 AndAlso Me._idProductoPadre > 0 Then

                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@codMaterial", SqlDbType.VarChar, 20).Value = Me._material.Trim
                            .Add("@nombreReferencia", SqlDbType.VarChar, 250).Value = Me._referencia.Trim
                            If Not String.IsNullOrEmpty(_referenciaCliente) Then _
                                .Add("@nombreSegunCliente", SqlDbType.VarChar, 500).Value = Me._referenciaCliente
                            .Add("@idTipoMaterial", SqlDbType.SmallInt).Value = Me._idTipoOrden
                            .Add("@asignarMin", SqlDbType.Bit).Value = Me._asignarMin
                            .Add("@cantidadEmpaque", SqlDbType.Int).Value = Me._cantidadEmpaque
                            .Add("@idProducto", SqlDbType.Int).Value = Me._idProductoPadre
                            'If Me._idTecnologia > 0 Then .Add("@idTecnologia", SqlDbType.Int).Value = Me._idTecnologia
                            .Add("@leerSim", SqlDbType.Bit).Value = Me._leerSim
                            '.Add("@esSim", SqlDbType.Bit).Value = Me._esSim
                            If _idTipoEtiqueta <> 0 Then .Add("@idTipoEtiqueta", SqlDbType.SmallInt).Value = _idTipoEtiqueta
                            If Not String.IsNullOrEmpty(_color) Then .Add("@color", SqlDbType.VarChar).Value = Me._color
                            .Add("@principal", SqlDbType.SmallInt).Value = _principal
                            If Not String.IsNullOrEmpty(Me._codigoEan) Then .Add("@codigoEAN", SqlDbType.VarChar, 15).Value = Me._codigoEan.Trim
                            If _idRegionOriginal <> 0 Then .Add("@idRegionOriginal", SqlDbType.SmallInt).Value = _idRegionOriginal
                            .Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        End With
                        .iniciarTransaccion()
                        .ejecutarNonQuery("RegistrarSubproducto", CommandType.StoredProcedure)
                        If Long.TryParse(.SqlParametros("@returnValue").Value.ToString, resultado.Valor) Then
                            If resultado.Valor = 0 Then
                                If .estadoTransaccional Then .confirmarTransaccion()
                                Me.CargarDatos(Me._material)
                            Else
                                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                                Select Case resultado.Valor
                                    Case 1
                                        resultado.Mensaje = "Ya existe un Subproducto asociado al Material proporcionado. Por favor verifique."
                                    Case 2
                                        resultado.Mensaje = "Ya existe un Subproducto asociado al Nombre de Subproducto proporcionado. Por favor verifique."
                                    Case 3
                                        resultado.Mensaje = "Ya existe un Subproducto asociado al Nombre de Subproducto según Cliente proporcionado. Por favor verifique."
                                    Case Else
                                        resultado.Mensaje = "Ocurrió un error inesperado al tratar de registrar la información del Subproducto. Por favor intente nuevamente."
                                End Select
                            End If
                        Else
                            If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                            resultado.EstablecerMensajeYValor(12, "Error al tratar de registrar subproducto. " & _
                                "No se obtuvo respuestas por parte del servidor de Bases de Datos. Por favor intente nuevamente")
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado.EstablecerMensajeYValor(11, "No se han proporcionado todos los valores requeridos para poder realizar el registro")
            End If

            Return resultado
        End Function

        Public Function Actualizar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If Not String.IsNullOrEmpty(_codigoOriginal) Then
                If (Not String.IsNullOrEmpty(_referencia)) AndAlso _idTipoOrden > 0 AndAlso _
                   _cantidadEmpaque > 0 AndAlso _idProductoPadre > 0 AndAlso _idEstado > 0 Then
                    Dim dbManager As New LMDataAccess
                    Try
                        With dbManager
                            .SqlParametros.Add("@materialActual", SqlDbType.VarChar, 20).Value = _codigoOriginal.Trim
                            If (Not String.IsNullOrEmpty(_material)) AndAlso _material.Trim <> "0" Then _
                                .SqlParametros.Add("@materialNuevo", SqlDbType.VarChar, 20).Value = _material.Trim
                            If (Not String.IsNullOrEmpty(_referencia)) Then _
                                .SqlParametros.Add("@referencia", SqlDbType.VarChar, 250).Value = _referencia.Trim
                            If (Not String.IsNullOrEmpty(_referenciaCliente)) Then _
                                .SqlParametros.Add("@referenciaSegunCliente", SqlDbType.VarChar, 250).Value = _referenciaCliente.Trim
                            If _idTipoOrden > 0 Then .SqlParametros.Add("@idTipoMaterial", SqlDbType.SmallInt).Value = _idTipoOrden
                            .SqlParametros.Add("@asignarMin", SqlDbType.Bit).Value = _asignarMin
                            .SqlParametros.Add("@cantidadEmpaque", SqlDbType.Int).Value = Math.Max(_cantidadEmpaque, 1)
                            If (Not String.IsNullOrEmpty(_codigoEan)) Then _
                                .SqlParametros.Add("@codigoEAN", SqlDbType.VarChar, 15).Value = _codigoEan.Trim
                            If _idProductoPadre > 0 Then .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = _idProductoPadre
                            .SqlParametros.Add("@leerSim", SqlDbType.SmallInt).Value = IIf(_leerSim, 1, 0)
                            .SqlParametros.Add("@idEstado", SqlDbType.SmallInt).Value = _idEstado
                            If _idTipoEtiqueta <> 0 Then .SqlParametros.Add("@idTipoEtiqueta", SqlDbType.SmallInt).Value = _idTipoEtiqueta
                            If (Not String.IsNullOrEmpty(_color)) Then .SqlParametros.Add("@Color", SqlDbType.VarChar).Value = _color
                            If _idRegionOriginal > 0 Then .SqlParametros.Add("@idRegionOriginal", SqlDbType.SmallInt).Value = _idRegionOriginal
                            .SqlParametros.Add("@principal", SqlDbType.SmallInt).Value = _principal
                            .SqlParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                            .iniciarTransaccion()
                            .ejecutarNonQuery("ActualizarInfoMaterial", CommandType.StoredProcedure)
                            If Not IsDBNull(.SqlParametros("@returnValue").Value) Then
                                If Long.TryParse(.SqlParametros("@returnValue").Value.ToString, resultado.Valor) Then
                                    If resultado.Valor = 0 Then
                                        .confirmarTransaccion()
                                        resultado.Mensaje = "La información ha sido actualizada satisfactoriamente."
                                    Else
                                        If .estadoTransaccional Then .abortarTransaccion()
                                        Select Case resultado.Valor
                                            Case 1
                                                resultado.Mensaje = "No existe ninguna referencia con el código de material proporcionado. Por favor verifique"
                                            Case 2
                                                resultado.Mensaje = "Existe un material previamente registrado, con el código de material especificado. Por favor verifique"
                                            Case 3
                                                resultado.Mensaje = "Existe otro registro con el mismo nombre de referencia que se ha asignado a la referencia que se está intentando actualizar. Por favor verifique"
                                            Case 4
                                                resultado.Mensaje = "Existe otro registro con el mismo nombre de referencia según cliente que se ha asignado a la referencia que se está intentando actualizar. Por favor verifique"
                                            Case Else
                                                resultado.Mensaje = "Ha ocurrido un error inesperado. Por favor intente nuevamente"
                                        End Select
                                    End If
                                Else
                                    If .estadoTransaccional Then .abortarTransaccion()
                                    resultado.EstablecerMensajeYValor(8, "Imposible evaluar la respuesta de confirmación de la transacción. Por favor intente nuevamente")
                                End If
                            Else
                                If .estadoTransaccional Then .abortarTransaccion()
                                resultado.EstablecerMensajeYValor(8, "Imposible evaluar la respuesta de confirmación de la transacción. Por favor intente nuevamente")
                            End If
                        End With
                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                        Throw New Exception(ex.Message, ex)
                    Finally
                        If dbManager IsNot Nothing Then dbManager.Dispose()
                    End Try
                Else
                    resultado.EstablecerMensajeYValor(9, "No se han proporcionado todos los datos requeridos para realizar la actualización")
                End If
            Else
                resultado.EstablecerMensajeYValor(10, "No se ha proporcionado el código de la referencia a actualizar")
            End If

            Return resultado
        End Function

#End Region

    End Class

End Namespace