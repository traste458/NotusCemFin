Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Enumerados
Imports LMDataAccessLayer

Namespace WMS

    Public Class Bodega

#Region "Campos"

        Private _idBodega As Short
        Private _nombre As String
        Private _codigo As String
        Private _direccion As String
        Private _telefono As String
        Private _idCiudad As Integer
        Private _ciudad As String
        Private _estado As Boolean
        Private _infoPosiciones As DataTable
        Private _infoOrdenBodegaje As DataTable
        Private _aceptaProductoEnReconocimiento As Boolean

#End Region

#Region "Propiedades"

        Public Property IdBodega() As Short
            Get
                Return _idBodega
            End Get
            Set(ByVal value As Short)
                _idBodega = value
            End Set
        End Property

        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        Public Property Codigo() As String
            Get
                Return _codigo
            End Get
            Set(ByVal value As String)
                _codigo = value
            End Set
        End Property

        Public Property Direccion() As String
            Get
                Return _direccion
            End Get
            Set(ByVal value As String)
                _direccion = value
            End Set
        End Property

        Public Property Telefono() As String
            Get
                Return _telefono
            End Get
            Set(ByVal value As String)
                _telefono = value
            End Set
        End Property

        Public Property IdCiudad() As Integer
            Get
                Return _idCiudad
            End Get
            Set(ByVal value As Integer)
                _idCiudad = value
            End Set
        End Property

        Protected Friend Property Ciudad() As String
            Get
                Return _ciudad
            End Get
            Set(value As String)
                _ciudad = value
            End Set
        End Property

        Public Property Estado As Boolean
            Get
                Return _estado
            End Get
            Set(value As Boolean)
                _estado = value
            End Set
        End Property

        Public Property Activa() As Boolean
            Get
                Return _estado
            End Get
            Set(ByVal value As Boolean)
                _estado = value
            End Set
        End Property

        Public ReadOnly Property DetallePosiciones() As DataTable
            Get
                If _infoPosiciones Is Nothing Then
                    Dim filtro As New FiltroPosicionBodega
                    filtro.IdBodega = _idBodega
                    _infoPosiciones = ObtenerDetallePosiciones(filtro)
                End If
                Return _infoPosiciones
            End Get
        End Property

        Public ReadOnly Property DetalleOrdenBodegaje() As DataTable
            Get
                Return _infoOrdenBodegaje
            End Get
        End Property

        Public Property AceptaProductoEnReconocimiento() As Boolean
            Get
                Return _aceptaProductoEnReconocimiento
            End Get
            Set(ByVal value As Boolean)
                _aceptaProductoEnReconocimiento = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idBodega As Integer)
            MyBase.New()
            _idBodega = idBodega
            CargarInformacion()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            If _idBodega > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = _idBodega
                        .ejecutarReader("ObtenerInfoBodega", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                _nombre = .Reader("nombre").ToString
                                _codigo = .Reader("codigo").ToString
                                _direccion = .Reader("direccion").ToString
                                _telefono = .Reader("telefono").ToString
                                Integer.TryParse(.Reader("idCiudad").ToString, _idCiudad)
                                _ciudad = .Reader("tecnologia").ToString
                                _estado = CBool(.Reader("estado"))
                                _aceptaProductoEnReconocimiento = CBool(.Reader("aceptaProdSinReconocimiento"))
                            End If
                            .Reader.Close()
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
        End Sub

        Private Sub CargarPosiciones()
            Dim filtro As New FiltroPosicionBodega
            filtro.IdBodega = _idBodega
            _infoPosiciones = PosicionBodega.ObtenerListado(filtro)
        End Sub

        Private Sub CargarOrdenesBodegaje()

        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As Short
            Dim resultado As Short = 0
            If _nombre.Trim.Length > 0 And _codigo.Trim.Length > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@nombre", SqlDbType.VarChar, 50).Value = _nombre.Trim.ToUpper
                            .Add("@codigo", SqlDbType.VarChar, 10).Value = _codigo.Trim.ToUpper
                            .Add("@direccion", SqlDbType.VarChar, 70).IsNullable = True
                            .Item("@direccion").Value = IIf(_direccion.Trim.Length > 0, _direccion.Trim, DBNull.Value)
                            .Add("@telefono", SqlDbType.VarChar, 50).IsNullable = True
                            .Item("@telefono").Value = IIf(_telefono.Trim.Length > 0, _telefono.Trim, DBNull.Value)
                            .Add("@idCiudad", SqlDbType.Int).IsNullable = True
                            .Item("@idCiudad").Value = IIf(_idCiudad <> 0, _idCiudad, DBNull.Value)
                            .Add("@aceptaProdSinReconocimiento", SqlDbType.Bit).Value = _aceptaProductoEnReconocimiento
                            .Add("@idBodega", SqlDbType.Int).Direction = ParameterDirection.Output
                            .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        End With
                        .ejecutarNonQuery("CrearBodega", CommandType.StoredProcedure)
                        resultado = CShort(.SqlParametros("@returnValue").Value)
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 4
            End If
            Return resultado
        End Function

        Public Function Actualizar() As Short
            Dim resultado As Short = 0
            If _idBodega > 0 AndAlso _nombre.Trim.Length > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@idBodega", SqlDbType.Int).Value = _idBodega
                            .Add("@nombre", SqlDbType.VarChar, 50).Value = _nombre.Trim.ToUpper
                            .Add("@direccion", SqlDbType.VarChar, 70).IsNullable = True
                            .Item("@direccion").Value = IIf(_direccion.Trim.Length > 0, _direccion.Trim, DBNull.Value)
                            .Add("@telefono", SqlDbType.VarChar, 50).IsNullable = True
                            .Item("@telefono").Value = IIf(_telefono.Trim.Length > 0, _telefono.Trim, DBNull.Value)
                            .Add("@idCiudad", SqlDbType.Int).IsNullable = True
                            .Item("@idCiudad").Value = IIf(_idCiudad <> 0, _idCiudad, DBNull.Value)
                            .Add("@estado", SqlDbType.Bit).Value = _estado
                            .Add("@aceptaProdSinReconocimiento", SqlDbType.Bit).Value = _aceptaProductoEnReconocimiento
                            .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        End With
                        .ejecutarNonQuery("ActualizarBodega", CommandType.StoredProcedure)
                        resultado = CShort(.SqlParametros("@returnValue").Value)
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 3
            End If
            Return resultado
        End Function

#End Region

#Region "Métodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroBodega
            filtro.Activa = EstadoBinario.Activo
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListadoBodegaTipo() As DataTable
            Dim filtro As New FiltroBodega
            filtro.Activa = EstadoBinario.Activo
            filtro.IdTipo = 1
            Dim dtDatos As DataTable = ObtenerListadoBodegaTipo(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroBodega) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDatos As DataTable
            Try
                With dbManager
                    With .SqlParametros
                        If filtro.IdBodega > 0 Then .Add("@idBodega", SqlDbType.Int).Value = filtro.IdBodega
                        If filtro.Nombre IsNot Nothing AndAlso filtro.Nombre.Trim.Length > 0 Then _
                            .Add("@nombre", SqlDbType.VarChar, 50).Value = filtro.Nombre.Trim
                        If filtro.Codigo IsNot Nothing AndAlso filtro.Codigo.Trim.Length > 0 Then _
                            .Add("@codigo", SqlDbType.VarChar, 10).Value = filtro.Codigo.Trim
                        If filtro.IdCiudad > 0 Then .Add("@idCiudad", SqlDbType.Int).Value = filtro.IdCiudad
                        If filtro.Activa > 0 Then .Add("@estado", SqlDbType.Bit).Value = IIf(filtro.Activa = 1, 1, 0)
                    End With
                    dtDatos = .ejecutarDataTable("ObtenerInfoBodega", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListadoBodegaTipo(ByVal filtro As FiltroBodega) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDatos As DataTable
            Try
                With dbManager
                    With .SqlParametros
                        If filtro.IdBodega > 0 Then .Add("@idBodega", SqlDbType.Int).Value = filtro.IdBodega
                        If filtro.Nombre IsNot Nothing AndAlso filtro.Nombre.Trim.Length > 0 Then _
                            .Add("@nombre", SqlDbType.VarChar, 50).Value = filtro.Nombre.Trim
                        If filtro.Codigo IsNot Nothing AndAlso filtro.Codigo.Trim.Length > 0 Then _
                            .Add("@codigo", SqlDbType.VarChar, 10).Value = filtro.Codigo.Trim
                        If filtro.IdCiudad > 0 Then .Add("@idCiudad", SqlDbType.Int).Value = filtro.IdCiudad
                        If filtro.Activa > 0 Then .Add("@estado", SqlDbType.Bit).Value = IIf(filtro.Activa = 1, 1, 0)
                        If filtro.IdTipo > 0 Then .Add("@IdTipo", SqlDbType.Int).Value = filtro.IdTipo
                    End With
                    dtDatos = .ejecutarDataTable("ObtenerInfoBodegaIdtipo", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Shared Function ObtenerPorId(ByVal identificador As Integer) As DataTable
            Dim filtro As New FiltroBodega
            Dim dtDatos As New DataTable
            filtro.IdBodega = identificador
            dtDatos = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Shared Function ObtenerDetallePosiciones(ByVal filtro As FiltroPosicionBodega) As DataTable
            Dim dtDatos As New DataTable
            dtDatos = PosicionBodega.ObtenerListado(filtro)
            Return dtDatos
        End Function

#End Region

    End Class

End Namespace


