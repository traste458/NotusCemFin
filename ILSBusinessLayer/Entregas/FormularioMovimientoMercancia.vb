Namespace ProcesoEntregas
    Public Class FormularioMovimientoMercancia

        Private _numeroFormulario As String
        Private _idUsuarioRegistro As Integer
        Private _idFabricante As Integer
        Private _cantidad As Integer
        Private _cantidadActual As Integer

        Public Property NumeroFormulario() As String
            Get
                Return _numeroFormulario
            End Get
            Set(ByVal value As String)
                _numeroFormulario = value
            End Set
        End Property

        Public Property IdUsuarioRegistro() As Integer
            Get
                Return _idUsuarioRegistro
            End Get
            Set(ByVal value As Integer)
                _idUsuarioRegistro = value
            End Set
        End Property

        Public Property IdFabricante() As Integer
            Get
                Return _idFabricante
            End Get
            Set(ByVal value As Integer)
                _idFabricante = value
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

        Public Property CantidadActual() As Integer
            Get
                Return _cantidadActual
            End Get
            Set(ByVal value As Integer)
                _cantidadActual = value
            End Set
        End Property

        Public Sub New()
            _numeroFormulario = ""
            _idUsuarioRegistro = 0
            _idFabricante = 0
            _cantidad = 0
            _cantidadActual = 0
        End Sub

        Public Sub RegistrarFormulario()
            Dim adminBD As New LMDataAccessLayer.LMDataAccess

            Try
                adminBD.iniciarTransaccion()
                adminBD.agregarParametroSQL("@numeroFormulario", Me._numeroFormulario)
                adminBD.agregarParametroSQL("@idUsuarioRegistro", Me._idUsuarioRegistro)
                adminBD.ejecutarNonQuery("CrearFormularioMovimientoMercancia", CommandType.StoredProcedure)
                adminBD.SqlParametros.Clear()
                adminBD.agregarParametroSQL("@numeroFormulario", Me._numeroFormulario)
                adminBD.agregarParametroSQL("@idFabricante", Me._idFabricante)
                adminBD.agregarParametroSQL("@cantidad", Me._cantidad)
                adminBD.agregarParametroSQL("@cantidadActual", Me._cantidadActual)
                adminBD.ejecutarNonQuery("CrearDetalleFormularioMovimientoMercancia", CommandType.StoredProcedure)
                adminBD.confirmarTransaccion()
            Catch ex As Exception
                adminBD.abortarTransaccion()
                Throw New Exception("Ocurrió un error registrando los datos del formulario: " & ex.Message)
            End Try
        End Sub

    End Class
End Namespace
