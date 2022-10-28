
Namespace Despachos
    Public Class DocumentoSAPDespacho
        Private _idRegistro As Integer
        Private _idDespacho As Long
        Private _tipoDocumento As GeneradorDocumentosSAP.tipoDoc
        Private _codigoDocumento As String

        Public Property IdRegistro() As Integer
            Get
                Return _idRegistro
            End Get
            Set(ByVal value As Integer)
                _idRegistro = value
            End Set
        End Property

        Public Property IdDespacho() As Integer
            Get
                Return _idDespacho
            End Get
            Set(ByVal value As Integer)
                _idDespacho = value
            End Set
        End Property

        Public Property TipoDocumento() As Integer
            Get
                Return _tipoDocumento
            End Get
            Set(ByVal value As Integer)
                _tipoDocumento = value
            End Set
        End Property

        Public Property CodigoDocumento() As Integer
            Get
                Return _codigoDocumento
            End Get
            Set(ByVal value As Integer)
                _tipoDocumento = value
            End Set
        End Property

        Public Function Guardar(ByVal documentos As List(Of ResultadoProceso)) As Boolean
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim resultado As Boolean = False
            Try
                db.agregarParametroSQL("@idDespacho", _idDespacho, SqlDbType.BigInt)
                db.SqlParametros.Add("@tipoDocumento", SqlDbType.VarChar, 5)
                db.SqlParametros.Add("@codigoDocumento", SqlDbType.VarChar)
                For Each valorRegistro As ResultadoProceso In documentos
                    db.SqlParametros("@tipoDocumento").Value = valorRegistro.Mensaje
                    db.SqlParametros("@codigoDocumento").Value = valorRegistro.Valor
                    resultado = db.ejecutarNonQuery("RegistrarDocumentoSAPDespacho", CommandType.StoredProcedure)
                Next

            Catch ex As Exception
                Throw New Exception(ex.Message)
                If db.Reader IsNot Nothing AndAlso Not db.Reader.IsClosed Then db.Reader.Close()
            Finally
                If db IsNot Nothing Then db.Dispose()

            End Try
            Return resultado
        End Function


        Public Shared Function ObtenerDocumentos(ByVal idDespacho As Long) As List(Of ResultadoProceso)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim listaDocs As New List(Of ResultadoProceso)
            Dim resultado As ResultadoProceso
            Dim dtDocumentos As New DataTable
            db.agregarParametroSQL("@idDespacho", idDespacho, SqlDbType.BigInt)

            Try
                dtDocumentos = db.ejecutarDataTable("ObtenerDocumentoSAPDespacho", CommandType.StoredProcedure)

                If dtDocumentos IsNot Nothing AndAlso dtDocumentos.Rows.Count > 0 Then
                    For Each dr As DataRow In dtDocumentos.Rows
                        With listaDocs
                            resultado = New ResultadoProceso
                            resultado.Valor = dr.Item("codigoDocumento")
                            resultado.Mensaje = dr.Item("tipoDocumento")
                            listaDocs.Add(resultado)
                        End With
                    Next
                    
                End If
                If db.Reader IsNot Nothing AndAlso Not db.Reader.IsClosed Then db.Reader.Close()

            Catch ex As Exception
                'Throw New Exception(ex.Message)
                If db.Reader IsNot Nothing AndAlso Not db.Reader.IsClosed Then db.Reader.Close()
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try

            Return listaDocs

        End Function
    End Class
End Namespace
