Imports LMDataAccessLayer
Imports System.Data.SqlClient
Imports ILSBusinessLayer.Estructuras

Namespace Productos
    Public Class Subproducto

#Region "Variables"
        Private _idSubproducto As String
        Private _material As String
        Private _subproducto As String
        Private _subproductoCliente As String
        Private _unidadesEmpaque As Integer
        Private _idProducto As Integer
        Private _idTipoInstruccion As Integer
        Private _tipoInstruccion As String
        Private _dtExistencias As New DataTable
        Private _codigoEan As String
#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal material As String)
            Me.New()
            Me.CargarDatos(material)
        End Sub

#End Region

#Region "Propiedades"

        Public Property SubproductoCliente() As String
            Get
                Return _subproductoCliente
            End Get
            Set(ByVal value As String)
                _subproductoCliente = value
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

        Public Property TipoInstruccion() As String
            Get
                Return _tipoInstruccion
            End Get
            Set(ByVal value As String)
                _tipoInstruccion = value
            End Set
        End Property

        Public Property IdTipoInstruccion() As Integer
            Get
                Return _idTipoInstruccion
            End Get
            Set(ByVal value As Integer)
                _idTipoInstruccion = value
            End Set
        End Property

        Public Property UnidadEmpaque() As Integer
            Get
                Return _unidadesEmpaque
            End Get
            Set(ByVal value As Integer)
                _unidadesEmpaque = value
            End Set
        End Property

        Public Property IdProducto() As Integer
            Get
                Return _idProducto
            End Get
            Set(ByVal value As Integer)
                _idProducto = value
            End Set
        End Property

        Public Property Subproducto() As String
            Get
                Return _subproducto
            End Get
            Set(ByVal value As String)
                _subproducto = value
            End Set
        End Property

        Public ReadOnly Property Existencias() As DataTable
            Get
                Return _dtExistencias
            End Get
        End Property

        Public Property CodigoEAN() As String
            Get
                Return _codigoEan
            End Get
            Set(ByVal value As String)
                _codigoEan = value
            End Set
        End Property
#End Region

#Region "Metodos"

        Public Sub CargarDatos(ByVal material As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim filtro As New FiltroSubproducto
            filtro.Estado = Enumerados.EstadoBinario.Activo
            filtro.Material = material
            Try
                With db
                    If filtro.Estado = Enumerados.EstadoBinario.NoEstablecido Then filtro.Estado = Enumerados.EstadoBinario.Activo
                    .SqlParametros.Add("@material", SqlDbType.VarChar, 20).Value = filtro.Material
                    .ejecutarReader("ObtenerListadoMateriales", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            _material = .Reader("material")
                            _subproducto = .Reader("Subproducto")
                            _idProducto = .Reader("idProducto")
                            _tipoInstruccion = .Reader("tipoInstruccion")
                            _idTipoInstruccion = .Reader("idTipoInstruccion")
                            _subproductoCliente = .Reader("subproductoCliente").ToString()
                            Integer.TryParse(.Reader("cantidad_empaque").ToString(), _unidadesEmpaque)
                            _codigoEan = .Reader("codigoEan").ToString()
                        End If
                    End If
                End With
            Finally
                db.Dispose()
            End Try
        End Sub

        Public Function ObtenerPorMaterial(ByVal material As String, Optional ByVal tipoPedido As Short = 0) As DataTable
            Dim dtDatos As DataTable
            Dim filtro As New FiltroSubproducto
            filtro.Estado = Enumerados.EstadoBinario.Activo
            filtro.Material = material
            dtDatos = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Function ObtenerPorReferencia(ByVal referencia As String) As DataTable
            Dim dtDatos As DataTable
            Dim filtro As New FiltroSubproducto
            filtro.Estado = Enumerados.EstadoBinario.Activo
            filtro.Subproducto = referencia
            dtDatos = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Function ObtenerPorRegion(ByVal idRegion As Integer) As DataTable
            Dim dtDatos As DataTable
            Dim filtro As New FiltroSubproducto
            filtro.Estado = Enumerados.EstadoBinario.Activo
            filtro.idRegion = idRegion
            dtDatos = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Shared Function ObtenerListado() As DataTable
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim dtDatos As DataTable
            Try
                With dbManager
                    dtDatos = .ejecutarDataTable("ObtenerListadoMateriales", CommandType.StoredProcedure)
                End With

                Return dtDatos
            Finally
                dbManager.Dispose()
            End Try
        End Function

        Public Shared Function ObtenerListadoCombo() As DataTable
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim dtDatos As DataTable
            Try
                With dbManager
                    dtDatos = .ejecutarDataTable("ObtenerListadoMaterialesCombo", CommandType.StoredProcedure)
                End With

                Return dtDatos
            Finally
                dbManager.Dispose()
            End Try
        End Function

        Public Shared Function ObtenerListadoComboMaterial(pMaterial As String) As DataTable
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim dtDatos As DataTable
            Try
                With dbManager
                    If (Not String.IsNullOrEmpty(pMaterial) AndAlso pMaterial.Length > 0) Then
                        .SqlParametros.Add("@material", SqlDbType.VarChar, 10).Value = pMaterial
                    End If
                    dtDatos = .ejecutarDataTable("ObtenerListadoMaterialesfiltroCombo", CommandType.StoredProcedure)
                End With

                Return dtDatos
            Finally
                dbManager.Dispose()
            End Try
        End Function

        Public Shared Function ObtenerListadoComboMaterialServicioFinancieros(pMaterial As String) As DataTable
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim dtDatos As DataTable
            Try
                With dbManager
                    If (Not String.IsNullOrEmpty(pMaterial) AndAlso pMaterial.Length > 0) Then
                        .SqlParametros.Add("@material", SqlDbType.VarChar, 10).Value = pMaterial
                    End If
                    dtDatos = .ejecutarDataTable("ObtenerListadoMaterialesfiltroServicioFinanciero", CommandType.StoredProcedure)
                End With

                Return dtDatos
            Finally
                dbManager.Dispose()
            End Try
        End Function
        Public Shared Function ObtenerListado(ByVal filtro As FiltroSubproducto) As DataTable
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim dtDatos As DataTable
            Try
                With dbManager
                    If filtro.Estado = Enumerados.EstadoBinario.NoEstablecido Then
                        filtro.Estado = Enumerados.EstadoBinario.Activo
                    End If
                    .SqlParametros.Add("@idEstado", SqlDbType.SmallInt).Value = filtro.Estado
                    If filtro.IdProducto <> 0 Then .SqlParametros.Add("@idProducto", SqlDbType.VarChar).Value = filtro.IdProducto
                    If Not filtro.Material Is Nothing Then .SqlParametros.Add("@material", SqlDbType.VarChar, 20).Value = filtro.Material
                    If Not filtro.Subproducto Is Nothing Then .SqlParametros.Add("@subproducto", SqlDbType.VarChar, 20).Value = filtro.Subproducto
                    '' If Not filtro.idRegion <> 0 Then .SqlParametros.Add("@idRegion", SqlDbType.Int).Value = filtro.idRegion
                    If filtro.IdtipoOrden <> 0 Then .agregarParametroSQL("@IdtipoOrden", filtro.IdtipoOrden, SqlDbType.Int)
                    If filtro.IdTipoInstruccion <> 0 Then .agregarParametroSQL("@IdTipoInstruccion", filtro.IdTipoInstruccion, SqlDbType.Int)
                    If filtro.IdTipoProducto > 0 Then .agregarParametroSQL("@idTipoProducto", filtro.IdTipoProducto, SqlDbType.Int)
                    If filtro.EsSerializado > 0 Then .SqlParametros.Add("@esSerializado", SqlDbType.Bit).Value = IIf(filtro.EsSerializado = 1, 1, 0)
                End With
                dtDatos = dbManager.ejecutarDataTable("ObtenerListadoMateriales", CommandType.StoredProcedure)
                Return dtDatos
            Finally
                dbManager.Dispose()
            End Try
        End Function
        Public Function ObtenerExistenciaMaterialDisponible(ByVal filtro As FiltroExistenciasMaterial) As Integer
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim iResultado As Integer
            Try
                With dbManager
                    .TiempoEsperaComando = 600
                    If filtro.material.ToString.Trim.Length > 0 Then .SqlParametros.Add("@material", SqlDbType.VarChar, 20).Value = filtro.material.Trim
                    If filtro.idRegion <> 0 Then .SqlParametros.Add("@idRegion", SqlDbType.Int).Value = filtro.idRegion
                    If filtro.idCliente <> 0 Then .SqlParametros.Add("@idCliente", SqlDbType.Int).Value = filtro.idCliente
                    If filtro.idBodega <> 0 Then .SqlParametros.Add("@idbodega", SqlDbType.Int).Value = filtro.idBodega
                    If filtro.idPedido <> 0 Then .SqlParametros.Add("@idPedido", SqlDbType.Int).Value = filtro.idPedido
                    If filtro.numeroPedido <> 0 Then .SqlParametros.Add("@numeroPedido", SqlDbType.BigInt).Value = filtro.numeroPedido
                    If filtro.idTipoPedido <> 0 Then .SqlParametros.Add("@idTipoPedido", SqlDbType.Int).Value = filtro.idTipoPedido
                    If filtro.cantidadSolicitada <> 0 Then .SqlParametros.Add("@cantidadSolicitada", SqlDbType.Int).Value = filtro.cantidadSolicitada
                    .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                    .ejecutarNonQuery("ObtenerExistenciaDisponibleMaterial", CommandType.StoredProcedure)
                    If Not Integer.TryParse(.SqlParametros("@resultado").Value.ToString, iResultado) Then
                        Throw New Exception("Imposible determinar la cantidad disponible. Por favor intente nuevamente.")
                    End If
                End With

                Return iResultado
            Finally
                dbManager.Dispose()
            End Try
        End Function

        Public Function ObtenerExistenciaMaterialDisponiblePorRegion(ByVal filtro As FiltroExistenciasMaterial) As DataTable
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim dtResultado As New DataTable
            Try
                With dbManager
                    .TiempoEsperaComando = 120000
                    If filtro.material.ToString.Trim.Length > 0 Then .SqlParametros.Add("@material", SqlDbType.VarChar, 20).Value = filtro.material.Trim
                    If filtro.idCliente <> 0 Then .SqlParametros.Add("@idCliente", SqlDbType.Int).Value = filtro.idCliente
                    If filtro.idBodega <> 0 Then .SqlParametros.Add("@idbodega", SqlDbType.Int).Value = filtro.idBodega
                    If filtro.idPedido <> 0 Then .SqlParametros.Add("@idPedido", SqlDbType.Int).Value = filtro.idPedido
                    If filtro.numeroPedido <> 0 Then .SqlParametros.Add("@idPedido", SqlDbType.BigInt).Value = filtro.numeroPedido
                    dtResultado = .ejecutarDataTable("ObtenerExistenciaDisponibleMaterialPorRegion", CommandType.StoredProcedure)
                End With

                Return dtResultado

            Finally
                dbManager.Dispose()
            End Try
        End Function

        Public Function ObtenerMaterialExcentoDeImpuesto(Optional ByVal material As String = "") As DataTable
            Dim dt As New DataTable
            Dim dm As New LMDataAccess
            Try
                If material.Trim.Length > 0 Then dm.agregarParametroSQL("@material", material, SqlDbType.VarChar, 20)
                dt = dm.ejecutarDataTable("ObtenerSubproductoExcentoDeImpuesto", CommandType.StoredProcedure)
            Catch ex As Exception
                Throw New Exception("Error al obetner información de material exento " & ex.Message)
            End Try
            Return dt
        End Function

        Public Function AdicionarMaterialExcentoDeImpuesto(ByVal material As String, ByVal idUsuario As Integer) As ResultadoProceso
            Dim dm As New LMDataAccess
            Dim resultado As New ResultadoProceso
            Try
                With dm
                    .agregarParametroSQL("@material", material, SqlDbType.VarChar, 20)
                    .agregarParametroSQL("@idUsuario", idUsuario, SqlDbType.Int)
                    .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .ejecutarNonQuery("RegistrarMaterialExcentoDeImpuesto", CommandType.StoredProcedure)
                    resultado.Valor = CInt(.SqlParametros("@resultado").Value)
                    If resultado.Valor = 0 Then
                        resultado.Mensaje = "Se adicionó correctamente el material " & material & " a la lista."
                    ElseIf resultado.Valor = 1 Then
                        resultado.Mensaje = "El material ya esta registrado como excento de impuesto."
                    ElseIf resultado.Valor = 2 Then
                        resultado.Mensaje = "Ocurrio un error inesperado al registrar material"
                    End If
                End With

            Catch ex As Exception
                resultado.EstablecerMensajeYValor(-1, ex.Message)
            End Try
            Return resultado
        End Function

        Public Function EliminarMaterialExcentoDeImpuesto(ByVal material As String, ByVal idUsuario As Integer) As ResultadoProceso
            Dim dm As New LMDataAccess
            Dim resultado As New ResultadoProceso
            Try
                With dm
                    .agregarParametroSQL("@listaMaterial", material, SqlDbType.VarChar, 8000)
                    .agregarParametroSQL("@idUsuario", idUsuario, SqlDbType.Int)
                    .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .ejecutarNonQuery("EliminarMaterialExcentoDeImpuesto", CommandType.StoredProcedure)
                    resultado.Valor = CInt(.SqlParametros("@resultado").Value)
                    If resultado.Valor = 0 Then
                        resultado.Mensaje = "Se eliminó correctamente el/los material(es) " & material & " de la lista."
                    ElseIf resultado.Valor = 1 Then
                        resultado.Mensaje = "Ocurrió un error inesperado al eliminar el/los material(es) de la lista."
                    End If
                End With

            Catch ex As Exception
                resultado.EstablecerMensajeYValor(-1, ex.Message)
            End Try
            Return resultado
        End Function
#End Region

    End Class
End Namespace

