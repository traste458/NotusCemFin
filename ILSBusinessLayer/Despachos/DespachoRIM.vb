Imports ILSBusinessLayer
Namespace Despachos
    Public Class DespachoRIM

#Region "atributos"
        Private _dtErrores As New DataTable
        Private _idCargue As Integer
        Private _idUsuario As Integer
        Private _fechaArchivo As Date
        Private _cntregistros As Integer
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
                Return _idCargue
            End Get
            Set(ByVal value As Integer)
                _idCargue = value
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
#End Region

#Region "Metodos"
        Public Function ExisteArchivo(ByVal anio As Integer, ByVal mes As Integer) As Boolean
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idAnio", anio, SqlDbType.Int)
            db.agregarParametroSQL("@idMes", mes, SqlDbType.Int)
            Return CBool(db.ejecutarScalar("ConsultarArchivoRim", CommandType.StoredProcedure))
            db.cerrarConexion()
        End Function

        Public Sub CargarArchivo(ByVal rutaFisica As String, ByVal opcion As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim adminArchivo As New AdministradorArchivos
            Dim dt As DataTable = adminArchivo.CargarDespachosRIM(rutaFisica)
            Try
                If Not adminArchivo.ContieneErrores Then
                    db.agregarParametroSQL("@idUsuario", _idUsuario, SqlDbType.Int)
                    db.agregarParametroSQL("@fechaArchivo", _fechaArchivo.ToString("yyy-MM-dd"), SqlDbType.Date)
                    db.agregarParametroSQL("@opcion", opcion, SqlDbType.VarChar)
                    db.SqlParametros.Add("@idCargue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    db.iniciarTransaccion()
                    db.ejecutarNonQuery("InsertarCargueDespachoRIM", CommandType.StoredProcedure)

                    _idCargue = db.SqlParametros("@idCargue").Value
                    db.SqlParametros.Clear()
                    Dim miColumna As New DataColumn("idCargue", GetType(Integer), _idCargue)
                    dt.Columns.Add(miColumna)
                    dt.Columns("idCargue").SetOrdinal(1)
                    dt.Columns.Add("fechaCargueRegistro", GetType(Date))

                    For a As Integer = 0 To dt.Rows.Count - 1
                        dt.Rows(a).Item("fechaCargueRegistro") = _fechaArchivo
                    Next

                    db.inicilizarBulkCopy()
                    db.BulkCopy.DestinationTableName = "DespachoRIM"
                    db.BulkCopy.WriteToServer(dt)
                    db.confirmarTransaccion()
                    _cntregistros = dt.Rows.Count
                End If
                _dtErrores = adminArchivo.ListaErrores
            Catch ex As Exception
                If db.estadoTransaccional Then db.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                db.Dispose()
            End Try
        End Sub

        Public Shared Function Consultar(ByVal filtro As FiltroDespachoRIM) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            With filtro
                If .idDespacho > 0 Then db.agregarParametroSQL("@idDespacho", .idDespacho, SqlDbType.Int)
                If .fechaCargueInicial > Date.MinValue Then
                    db.agregarParametroSQL("@fechaCargueInicial", .fechaCargueInicial, SqlDbType.Date)
                    db.agregarParametroSQL("@fechaCargueFinal", .fechaCargueFinal, SqlDbType.Date)
                End If
                If .fechaArchivoInicial > Date.MinValue Then
                    db.agregarParametroSQL("@fechaArchivoInicial", .fechaArchivoInicial, SqlDbType.Date)
                    db.agregarParametroSQL("@fechaArchivoFinal", .fechaArchivoFinal, SqlDbType.Date)
                End If
                Dim dt As DataTable = db.ejecutarDataTable("ConsultarDespachoRIM", CommandType.StoredProcedure)
                Return dt
            End With
        End Function

        Public Structure FiltroDespachoRIM
            Dim idDespacho As Integer
            Dim fechaCargueInicial As Date
            Dim fechaCargueFinal As Date
            Dim fechaArchivoInicial As Date
            Dim fechaArchivoFinal As Date
        End Structure

      
#End Region

    End Class
End Namespace

