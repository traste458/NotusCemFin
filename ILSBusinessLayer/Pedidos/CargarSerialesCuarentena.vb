Imports LMDataAccessLayer

Namespace Pedidos

    Public Class CargarSerialesCuarentena

#Region "Campos"

        Private _dtInfoError As DataTable
        Private _dtInfoCarga As DataTable
        Private _idUsuario As Integer
        Private _esLiberacion As Boolean

#End Region

#Region "Propiedades"

        Public Property InfoError() As DataTable
            Get
                Return _dtInfoError
            End Get
            Set(ByVal value As DataTable)
                _dtInfoError = value
            End Set
        End Property

        Public Property InfoCarga() As DataTable
            Get
                Return _dtInfoCarga
            End Get
            Set(ByVal value As DataTable)
                _dtInfoCarga = value
            End Set
        End Property

        Public Property IdUsuario() As Integer
            Get
                Return _idUsuario
            End Get
            Set(ByVal value As Integer)
                _idUsuario = value
            End Set
        End Property

        Public Property EsLiberacion() As Boolean
            Get
                Return _esLiberacion
            End Get
            Set(ByVal value As Boolean)
                _esLiberacion = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            Inicializar()
        End Sub

        Public Sub New(ByVal dtDatosCarga As DataTable)
            MyBase.New()
            Inicializar()
            _dtInfoCarga = dtDatosCarga
        End Sub

        Public Sub New(ByVal idUsuario As Integer)
            MyBase.New()
            Inicializar()
            _idUsuario = idUsuario
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub Inicializar()
            _esLiberacion = False
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As Short

        End Function

        Public Function BorrarSerialesCargaTemporal() As Short
            Dim dbManager As New LMDataAccess
            Dim resultado As Short
            Dim _spNombre As String = "BorrarCargaSerialesCuarentenaTemporal"
            If _idUsuario > 0 Then
                If _esLiberacion Then
                    _spNombre = "BorrarLiberarSerialesCuarentenaTemporal"
                End If

                Try
                    With dbManager
                        .iniciarTransaccion()
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                        .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                        .ejecutarNonQuery(_spNombre, CommandType.StoredProcedure)

                        Short.TryParse(.SqlParametros("@returnValue").Value, resultado)
                        If resultado = 0 Then
                            .confirmarTransaccion()
                        Else
                            If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception("Error al tratar de borrar los datos cargados temporalmente en BD." & ex.Message)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 3
            End If

            Return resultado
        End Function

        Public Function CargarDatosParaValidacion() As Short
            Dim dbManager As New LMDataAccess
            Dim resultado As Short
            If _dtInfoCarga IsNot Nothing AndAlso _dtInfoCarga.Rows.Count > 0 Then
                Try
                    With dbManager
                        .iniciarTransaccion()
                        .inicilizarBulkCopy()
                        With .BulkCopy

                            If _esLiberacion Then
                                .DestinationTableName = "LiberarSerialesCuarentenaTemporal"
                            Else
                                .DestinationTableName = "CargaSerialesCuarentenaTemporal"
                            End If
                            .ColumnMappings.Add("serial", "serial")
                            .ColumnMappings.Add("numlinea", "numLinea")
                            .ColumnMappings.Add("idUsuario", "idUsuario")
                            .WriteToServer(_dtInfoCarga)
                        End With
                        .confirmarTransaccion()
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception("Error al tratar de cargar temporalmente los datos a la BD para realizar validaciones complementarias." & ex.Message)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 3
            End If

            Return resultado
        End Function

        Public Sub BuscarErroresDeIntegridad(ByRef dtError As DataTable)
            Dim dbManager As New LMDataAccess
            Dim _spNombre As String = "BuscarErroresEnSerialesCuarentena"
            If _esLiberacion Then
                _spNombre = "BuscarErroresLiberacionCuarentena"
            End If
            Try
                With dbManager
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .llenarDataTable(dtError, _spNombre, CommandType.StoredProcedure)
                End With
            Catch ex As Exception
                Throw New Exception("Error al tratar de validar la existencia de registro con error. " & ex.Message)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

#End Region

#Region "Metodo Compartido"

        Public Overloads Shared Function ObtenerSerialesCuarentenaTemporal(ByVal idUsuario As Integer) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDatos As DataTable
            If idUsuario > 0 Then
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        End With
                        dtDatos = .ejecutarDataTable("ObtenerSerialesCuarentenaTemporal", CommandType.StoredProcedure)
                        Dim pk() As DataColumn = {dtDatos.Columns("serial")}
                        dtDatos.PrimaryKey = pk

                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If

            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerSerialesLiberacionTemporal(ByVal idUsuario As Integer) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDatos As DataTable
            If idUsuario > 0 Then
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        End With
                        dtDatos = .ejecutarDataTable("ObtenerSerialesLiberacionTemporal", CommandType.StoredProcedure)
                        Dim pk() As DataColumn = {dtDatos.Columns("serial")}
                        dtDatos.PrimaryKey = pk

                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If

            Return dtDatos
        End Function

#End Region

    End Class

End Namespace