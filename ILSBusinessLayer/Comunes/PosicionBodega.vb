Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Enumerados
Imports LMDataAccessLayer

Namespace WMS

    Public Class PosicionBodega

#Region "Campos"

        Private _idPosicion As Integer
        Private _idBodega As Short
        Private _codigo As String
        Private _idRegion As Short
        Private _region As String
        Private _permitirMezcla As Boolean
        Private _estante As Short
        Private _nivel As String
        Private _cubiculo As Short
        Private _numOtbs As Short
        Private _saldo As Integer
        Private _idClasificacion As Short
        Private _clasificacion As String
        Private _fechaCreacion As Date
        Private _idCreador As Integer
        Private _creador As String
        Private _detalleOrdenBodegaje As DataTable
        Private _detalleSeriales As DataTable

#End Region

#Region "Propiedades"

        Public Property IdPosicion() As Integer
            Get
                Return _idPosicion
            End Get
            Set(ByVal value As Integer)
                _idPosicion = value
            End Set
        End Property

        Public Property IdBodega() As Short
            Get
                Return _idBodega
            End Get
            Set(ByVal value As Short)
                _idBodega = value
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

        Public Property IdRegion() As Short
            Get
                Return _idRegion
            End Get
            Set(ByVal value As Short)
                _idRegion = value
            End Set
        End Property

        Public ReadOnly Property Region()
            Get
                Return _region
            End Get
        End Property

        Public Property PermitirMezcla() As Boolean
            Get
                Return _permitirMezcla
            End Get
            Set(ByVal value As Boolean)
                _permitirMezcla = value
            End Set
        End Property

        Public Property Estante() As Short
            Get
                Return _estante
            End Get
            Set(ByVal value As Short)
                _estante = value
            End Set
        End Property

        Public Property Nivel() As String
            Get
                Return _nivel
            End Get
            Set(ByVal value As String)
                _nivel = value
            End Set
        End Property

        Public Property Cubiculo() As Short
            Get
                Return _cubiculo
            End Get
            Set(ByVal value As Short)
                _cubiculo = value
            End Set
        End Property

        Public ReadOnly Property NumeroDeOTBs() As Short
            Get
                Return _numOtbs
            End Get
        End Property

        Public ReadOnly Property Saldo() As Integer
            Get
                Return _saldo
            End Get
        End Property

        Public Property IdClasificacion() As Short
            Get
                Return _idClasificacion
            End Get
            Set(ByVal value As Short)
                _idClasificacion = value
            End Set
        End Property

        Public ReadOnly Property Clasificacion() As String
            Get
                Return _clasificacion
            End Get
        End Property

        Public Property IdCreador() As Integer
            Get
                Return _idCreador
            End Get
            Set(ByVal value As Integer)
                _idCreador = value
            End Set
        End Property

        Public ReadOnly Property Creador() As String
            Get
                Return _creador
            End Get
        End Property

        Public ReadOnly Property FechaCreacion() As Date
            Get
                Return _fechaCreacion
            End Get
        End Property

        Public ReadOnly Property DetalleOrdenBodegaje() As DataTable
            Get
                Return _detalleOrdenBodegaje
            End Get
        End Property

        Public ReadOnly Property DetalleSeriales() As DataTable
            Get
                Return _detalleSeriales
            End Get
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idPosicion As Integer)
            MyBase.New()
            _idPosicion = idPosicion
            CargarInformacion()
        End Sub

        Public Sub New(ByVal posicion As String)
            MyBase.New()
            _codigo = posicion.Trim
            CargarInformacion()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            If _idPosicion > 0 OrElse (_codigo IsNot Nothing AndAlso _codigo.Trim.Length > 0) Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        If _idPosicion > 0 Then .SqlParametros.Add("@idPosicion", SqlDbType.Int).Value = _idPosicion
                        If _codigo IsNot Nothing AndAlso _codigo.Trim.Length > 0 Then _
                            .SqlParametros.Add("@codigo", SqlDbType.VarChar, 20).Value = _codigo
                        .ejecutarReader("ObtenerInfoPosicionBodega", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                Integer.TryParse(.Reader("idPosicion").ToString, _idPosicion)
                                Short.TryParse(.Reader("idBodega").ToString, _idBodega)
                                _codigo = .Reader("codigo").ToString
                                Short.TryParse(.Reader("idRegion").ToString, _idRegion)
                                _permitirMezcla = CBool(.Reader("permitirMezcla"))
                                Short.TryParse(.Reader("estante").ToString, _estante)
                                _nivel = .Reader("nivel").ToString
                                Short.TryParse(.Reader("cubiculo").ToString, _cubiculo)
                                Integer.TryParse(.Reader("numOtbs").ToString, _numOtbs)
                                Integer.TryParse(.Reader("saldo").ToString, _saldo)
                                Short.TryParse(.Reader("idClasificacion").ToString, _idClasificacion)
                                _clasificacion = .Reader("clasificacion").ToString
                                Date.TryParse(.Reader("fechaCreacion").ToString, _fechaCreacion)
                                Integer.TryParse(.Reader("idCreador").ToString, _idCreador)
                                _creador = .Reader("creador").ToString
                            End If
                            .Reader.Close()
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
        End Sub

        Private Sub CargarDetalleSeriales()

        End Sub

        Private Sub CargarOrdenesBodegaje()

        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As Short
            Dim resultado As Short = 0
            If _idBodega > 0 AndAlso _estante > 0 AndAlso _nivel IsNot Nothing AndAlso _nivel.Trim.Length > 0 AndAlso _cubiculo > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@idBodega", SqlDbType.Int).Value = _idBodega
                            .Add("@estante", SqlDbType.SmallInt).Value = _estante
                            .Add("@nivel", SqlDbType.VarChar, 3).Value = _nivel
                            .Add("@cubiculo", SqlDbType.SmallInt).Value = _cubiculo
                            .Add("@idClasificacion", SqlDbType.SmallInt).Value = _idClasificacion
                            .Add("@idCreador", SqlDbType.Int).Value = _idCreador
                            .Add("@idRegion", SqlDbType.SmallInt).IsNullable = True
                            .Item("@idRegion").Value = IIf(_idRegion > 0, _idRegion, DBNull.Value)
                            .Add("@permitirMezcla", SqlDbType.Bit).Value = _permitirMezcla
                            .Add("@idPosicion", SqlDbType.Int).Direction = ParameterDirection.Output
                            .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        End With
                        .ejecutarNonQuery("CrearPosicionBodega", CommandType.StoredProcedure)
                        resultado = CShort(.SqlParametros("@returnValue").Value)
                        If resultado = 0 Then Integer.TryParse(.SqlParametros("@idPosicion").Value.ToString, _idPosicion)
                        If _idPosicion > 0 Then CargarInformacion()
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 3
            End If
            Return resultado
        End Function

        Public Function Registrar(ByVal cubiculoFinal As Short, ByRef arrCodigo As ArrayList) As Short
            Dim resultado As Short = 0
            If _idBodega > 0 AndAlso _estante > 0 AndAlso _nivel IsNot Nothing AndAlso _nivel.Trim.Length > 0 _
                AndAlso _cubiculo > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@idBodega", SqlDbType.Int).Value = _idBodega
                            .Add("@estante", SqlDbType.SmallInt).Value = _estante
                            .Add("@nivel", SqlDbType.VarChar, 3).Value = _nivel
                            .Add("@cubiculoInicial", SqlDbType.SmallInt).Value = _cubiculo
                            .Add("@cubiculoFinal", SqlDbType.SmallInt).Value = cubiculoFinal
                            .Add("@idClasificacion", SqlDbType.SmallInt).Value = _idClasificacion
                            .Add("@idCreador", SqlDbType.Int).Value = _idCreador
                            .Add("@idRegion", SqlDbType.SmallInt).IsNullable = True
                            .Item("@idRegion").Value = IIf(_idRegion > 0, _idRegion, DBNull.Value)
                            .Add("@permitirMezcla", SqlDbType.Bit).Value = _permitirMezcla
                            .Add("@codigos", SqlDbType.VarChar, 8000).Direction = ParameterDirection.Output
                            .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        End With
                        .iniciarTransaccion()
                        .ejecutarNonQuery("CrearPosicionesDeBodega", CommandType.StoredProcedure)
                        resultado = CShort(.SqlParametros("@returnValue").Value)
                        If resultado = 0 Then arrCodigo.AddRange(.SqlParametros("@codigos").Value.ToString.Split(","))
                        .confirmarTransaccion()
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 3
            End If
            Return resultado
        End Function

        Public Function Actualizar() As Short
            'Dim resultado As Short = 0
            'If _idBodega > 0 AndAlso _nombre.Trim.Length > 0 Then
            '    Dim dbManager As New LMDataAccess
            '    Try
            '        With dbManager
            '            With .SqlParametros
            '                .Add("@idBodega", SqlDbType.Int).Value = _idBodega
            '                .Add("@nombre", SqlDbType.VarChar, 50).Value = _nombre.Trim.ToUpper
            '                .Add("@direccion", SqlDbType.VarChar, 70).IsNullable = True
            '                .Item("@direccion").Value = IIf(_direccion.Trim.Length > 0, _direccion.Trim, DBNull.Value)
            '                .Add("@telefono", SqlDbType.VarChar, 50).IsNullable = True
            '                .Item("@telefono").Value = IIf(_telefono.Trim.Length > 0, _telefono.Trim, DBNull.Value)
            '                .Add("@idCiudad", SqlDbType.Int).IsNullable = True
            '                .Item("@idCiudad").Value = IIf(_idCiudad <> 0, _idCiudad, DBNull.Value)
            '                .Add("@estado", SqlDbType.Bit).Value = _estado
            '                .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            '            End With
            '            .ejecutarNonQuery("ActualizarBodega", CommandType.StoredProcedure)
            '            resultado = CShort(.SqlParametros("@returnValue").Value)
            '        End With
            '    Finally
            '        If dbManager IsNot Nothing Then dbManager.Dispose()
            '    End Try
            'Else
            '    resultado = 3
            'End If
            'Return resultado
        End Function

#End Region

#Region "Métodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroPosicionBodega
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroPosicionBodega) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDatos As DataTable
            Try
                With dbManager

                    With .SqlParametros
                        If filtro.IdPosicion > 0 Then .Add("@idPosicion", SqlDbType.Int).Value = filtro.IdPosicion
                        If filtro.Codigo IsNot Nothing AndAlso filtro.Codigo.Trim.Length > 0 Then _
                            .Add("@codigo", SqlDbType.VarChar, 20).Value = filtro.Codigo.Trim
                        If filtro.IdBodega > 0 Then .Add("@idBodega", SqlDbType.Int).Value = filtro.IdBodega
                        If filtro.IdProducto > 0 Then .Add("@idProducto", SqlDbType.Int).Value = filtro.IdProducto
                        If filtro.Material IsNot Nothing AndAlso filtro.Material.Trim.Length > 0 Then _
                            .Add("@material", SqlDbType.VarChar, 10).Value = filtro.Material.Trim
                        If filtro.IdClasificacion > 0 Then .Add("@idClasificacion", SqlDbType.Int).Value = filtro.IdClasificacion
                        If filtro.IdRegion > 0 Then .Add("@idRegion", SqlDbType.SmallInt).Value = filtro.IdRegion
                        If filtro.FechaVencimientoInicial <> Date.MinValue And filtro.FechaVencimientoFinal <> Date.MinValue Then
                            .Add("@fVencimientoInicial", SqlDbType.SmallDateTime).Value = filtro.FechaVencimientoInicial
                            .Add("@fVencimientoFinal", SqlDbType.SmallDateTime).Value = filtro.FechaVencimientoFinal
                        End If
                        If filtro.FechaRecepcionInicial <> Date.MinValue And filtro.FechaRecepcionFinal <> Date.MinValue Then
                            .Add("@fRecepcionInicial", SqlDbType.SmallDateTime).Value = filtro.FechaRecepcionInicial
                            .Add("@fRecepcionFinal", SqlDbType.SmallDateTime).Value = filtro.FechaRecepcionFinal
                        End If
                        If filtro.BodegaActiva <> EstadoBinario.NoEstablecido Then
                            .Add("@bodegaActiva", SqlDbType.Bit).Value = IIf(filtro.BodegaActiva = 1, 1, 0)
                        End If
                    End With
                    dtDatos = .ejecutarDataTable("ObtenerInfoPosicionBodega", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Shared Function ObtenerPorId(ByVal identificador As Integer) As DataTable
            Dim filtro As New FiltroPosicionBodega
            Dim dtDatos As New DataTable
            filtro.IdBodega = identificador
            dtDatos = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Shared Function ExistePosicion(ByVal codigo As String) As Boolean
            Dim dbManager As New LMDataAccess
            Dim dtResultado As DataTable
            Dim resultado As Boolean = False
            Try
                With dbManager
                    .SqlParametros.Add("@listaCodigoPosicion", SqlDbType.VarChar, 8000).Value = codigo
                    dtResultado = .ejecutarDataTable("ValidarExistenciaDePosicionDeBodega", CommandType.StoredProcedure)
                End With
                If dtResultado IsNot Nothing AndAlso dtResultado.Rows.Count > 0 Then
                    Dim pk() As DataColumn = {dtResultado.Columns("codigoPosicion")}
                    dtResultado.PrimaryKey = pk
                    Dim drAux As DataRow = dtResultado.Rows.Find(codigo)
                    If drAux IsNot Nothing Then resultado = CBool(drAux("existePosicion").ToString)
                End If
            Catch ex As Exception
                Throw New Exception("Error al tratar de validar si la posición proporcionada existe. " & ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return resultado
        End Function

        Public Shared Function ValidarExistencia(ByVal arrCodigoPosicion As ArrayList) As DataTable
            If arrCodigoPosicion IsNot Nothing AndAlso arrCodigoPosicion.Count > 0 Then
                Dim dbManager As New LMDataAccess
                Dim dtResultado As DataTable
                Try
                    With dbManager
                        .SqlParametros.Add("@listaCodigoPosicion", SqlDbType.VarChar, 8000).Value = Join(arrCodigoPosicion.ToArray, ",")
                        dtResultado = .ejecutarDataTable("ValidarExistenciaDePosicionDeBodega", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    Throw New Exception("Error al tratar de validar la existencia de una o más posiciones de bodega. " & ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
                Return dtResultado
            Else
                Throw New Exception("No se ha proporcionado ninguna posición de bodega para efectura la verificación de existencia. Por favor verifique")
            End If
        End Function

#End Region

    End Class

End Namespace
