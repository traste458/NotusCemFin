Imports LMDataAccessLayer
Imports ILSBusinessLayer
Imports System.IO
Namespace Facturacion
    Public Class CargueArchivosfacturacion

#Region "atributos"
        Private _dtErrores As New DataTable
        Private _idMaestro As Integer
        Private _idUsuario As Integer
        Private _fechaArchivo As Date
        Private _cntregistros As Integer
        Private _tipoArchivoCarga As Integer
        Private _nombreArchivo As String
        Private _peso As Double
        Private _mes As Integer
        Private _año As Integer
#End Region

#Region "Propiedades"
        Public ReadOnly Property ListaErrores() As DataTable
            Get
                Return _dtErrores
            End Get
        End Property

        Public ReadOnly Property ContieneErrores() As Boolean
            Get
                If _dtErrores.Rows.Count > 0 Then
                    Return True
                Else : Return False
                End If
            End Get
        End Property

        Public Property IdCargue() As Integer
            Get
                Return _idMaestro
            End Get
            Set(ByVal value As Integer)
                _idMaestro = value
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

        Public Property FechaArchivo() As Date
            Get
                Return _fechaArchivo
            End Get
            Set(ByVal value As Date)
                _fechaArchivo = value
            End Set
        End Property

        Public Property cntRegistros() As Integer
            Get
                Return _cntregistros
            End Get
            Set(ByVal value As Integer)
                _cntregistros = value
            End Set
        End Property

        Public Property tipoArchivo() As Integer
            Get
                Return _tipoArchivoCarga
            End Get
            Set(ByVal value As Integer)
                _tipoArchivoCarga = value
            End Set
        End Property

        Public Property peso() As String
            Get
                Return _peso
            End Get
            Set(ByVal value As String)
                _peso = value
            End Set
        End Property

        Public Property mes() As Integer
            Get
                Return _mes
            End Get
            Set(ByVal value As Integer)
                _mes = value
            End Set
        End Property

        Public Property año() As Integer
            Get
                Return _año
            End Get
            Set(ByVal value As Integer)
                _año = value
            End Set
        End Property

        Public Property nombreArchivo() As String
            Get
                Return _nombreArchivo
            End Get
            Set(ByVal value As String)
                _nombreArchivo = value
            End Set
        End Property

#End Region

#Region "Metodos"

        Public Function ExisteArchivo(ByVal anio As Integer, ByVal mes As Integer, ByVal tipoArchivo As Integer) As Boolean
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idAnio", anio, SqlDbType.Int)
            db.agregarParametroSQL("@idMes", mes, SqlDbType.Int)
            db.agregarParametroSQL("@idTipoarchivo", tipoArchivo, SqlDbType.Int)

            Return CBool(db.ejecutarScalar("ConsultarArchivoFacturacion", CommandType.StoredProcedure))
            db.cerrarConexion()
        End Function

        Public Shared Function ObtenerListado(ByVal idTipoArchivo As String) As DataTable
            Dim db As New LMDataAccess
            Dim dt As DataTable
            db.agregarParametroSQL("@idTipoArchivo", idTipoArchivo, SqlDbType.Char)
            dt = db.ejecutarDataTable("ObtenerTipoArchivoFacturacion", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Function ExisteArchivoSoporte(ByVal nomArchivo As String) As String
            Dim returnValue As Boolean
            Dim rutaFisica As String = ""
            Dim extension As String = "." & nomArchivo.Split(".").GetValue(nomArchivo.Split(".").Length - 1)
            For i As Integer = 1 To 100000
                If rutaFisica.Trim = "" Then
                    rutaFisica = nomArchivo
                End If
                returnValue = File.Exists(rutaFisica)
                If returnValue = False Then
                    Exit For
                Else
                    rutaFisica = nomArchivo.Split(".").GetValue(0) & "_" & i & extension
                    returnValue = File.Exists(rutaFisica)
                End If
            Next
            Return rutaFisica
        End Function

        Public Sub CargarArchivo(ByVal rutaFisica As String, ByVal Aprobado As Boolean)
            Dim adminArchivo As New AdministradorArchivos
            adminArchivo.TipoArchivo = _tipoArchivoCarga
            Dim ds As DataSet = adminArchivo.cargarArchivosFacturacion(rutaFisica, _tipoArchivoCarga)
            If Aprobado = False Then
                If Not adminArchivo.ContieneErrores Then
                    InsertarRegistrosBds(ds)
                Else
                    _dtErrores = adminArchivo.ListaErrores
                    _cntregistros = ds.Tables("dtRegistros").Rows.Count
                End If
            Else
                InsertarRegistrosBds(ds)
            End If
        End Sub

        Private Sub InsertarRegistrosBds(ByRef ds As DataSet)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                db.agregarParametroSQL("@fechaInicial", DateSerial(_año, _mes, 1).ToString("yyy-MM-dd"), SqlDbType.Date)
                db.agregarParametroSQL("@fechaFinal", DateSerial(_año, _mes + 1, 0).ToString("yyy-MM-dd"), SqlDbType.Date)
                db.agregarParametroSQL("@idUsuario", _idUsuario, SqlDbType.Int)
                db.agregarParametroSQL("@idTipoArchivoCarga", _tipoArchivoCarga, SqlDbType.Int)
                db.SqlParametros.Add("@idMaestro", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                db.iniciarTransaccion()
                db.ejecutarNonQuery("InsertarCargueArchivoFacturacion", CommandType.StoredProcedure)
                _idMaestro = db.SqlParametros("@idMaestro").Value

                Dim miColumna As New DataColumn("idMaestro", GetType(Integer), _idMaestro)
                ds.Tables("dtRegistros").Columns.Add(miColumna)
                ds.Tables("dtRegistros").Columns("idmaestro").SetOrdinal(0)
                ds.Tables("dtRegistros").Columns.Add("idRegistro")
                ds.Tables("dtRegistros").Columns("idRegistro").SetOrdinal(0)
                ds.Tables("dtRegistros").Columns.Remove("correcto")
                db.inicilizarBulkCopy()
                db.BulkCopy.BulkCopyTimeout = 900
                If _tipoArchivoCarga = 7 Or _tipoArchivoCarga = 8 Then 'ActivosFijos ó material POP
                    db.BulkCopy.DestinationTableName = "DetalleArchivoDespachoProductoConsumo"
                ElseIf _tipoArchivoCarga = 9 Then 'Merchandising
                    db.BulkCopy.DestinationTableName = "DetalleArchivoDespachoMerchandising"
                End If
                db.BulkCopy.WriteToServer(ds.Tables("dtRegistros"))
                db.confirmarTransaccion()
                _cntregistros = ds.Tables("dtRegistros").Rows.Count
            Catch ex As Exception
                If db.estadoTransaccional Then db.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                db.Dispose()
            End Try
        End Sub

        Public Function CargueManualArchivosFacturacion()
            Dim resultado As Integer
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            With dbManager
                Try
                    .iniciarTransaccion()
                    .agregarParametroSQL("@tipoArchivo", tipoArchivo, SqlDbType.Int)
                    .agregarParametroSQL("@peso", peso, SqlDbType.VarChar)
                    .agregarParametroSQL("@fechaArchivo", FechaArchivo, SqlDbType.Date)
                    .agregarParametroSQL("@archivoSoporte", nombreArchivo, SqlDbType.Char)
                    .agregarParametroSQL("@idUsuario", IdUsuario, SqlDbType.Int)

                    dbManager.SqlParametros.Add("@idCargue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                    dbManager.ejecutarNonQuery("InsertarCargueArchivoFacturacionManual", CommandType.StoredProcedure)
                    resultado = dbManager.SqlParametros("@idCargue").Value

                    dbManager.confirmarTransaccion()

                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End With
            dbManager.Dispose()
            Return resultado
        End Function
#End Region

    End Class
End Namespace
