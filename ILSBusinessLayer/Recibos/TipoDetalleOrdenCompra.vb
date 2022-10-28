Imports LMDataAccessLayer
Namespace Recibos
    Public Class TipoDetalleOrdenCompra

#Region "variables"

        Private _idTipoDetalleOrdenCompra As Short
        Private _tipo As String
        Private _descripcion As String
        Private _error As String

#End Region

#Region "propiedades"

        Public ReadOnly Property IdTipoDetalleOrdenCompra() As Short
            Get
                Return _idTipoDetalleOrdenCompra
            End Get
        End Property

        Public Property Tipo() As String
            Get
                Return _tipo
            End Get
            Set(ByVal value As String)
                _tipo = value
            End Set
        End Property

        Public Property Descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
        End Property

        Public ReadOnly Property InfoError() As String
            Get
                Return _error
            End Get
        End Property

        Public Enum TipoDetalle
            Principal = 1
            Secundario = 2
        End Enum

#End Region

#Region "contructores"

        Public Sub New()
            MyBase.New()
            _error = String.Empty
        End Sub

        Public Sub New(ByVal idTipo As Short)
            Me.New()
            Me.CargarDatos(idTipo)
        End Sub

#End Region

#Region "metodos privados"

        Private Sub CargarDatos(ByVal idTipo As Integer)
            Dim db As New LMDataAccess
            Try
                db.SqlParametros.Add("@idTipo", SqlDbType.SmallInt).Value = idTipo
                db.ejecutarReader("ObtenerInfoTipoDetalleOrdenCompra", CommandType.StoredProcedure)
                If db.Reader.Read Then
                    _idTipoDetalleOrdenCompra = idTipo
                    _tipo = db.Reader("tipo").ToString()
                    _descripcion = db.Reader("descripcion").ToString()
                End If
            Catch ex As Exception
                _error = "Error al cargar los datos. " & ex.Message
            Finally
                If Not db.Reader.IsClosed Then db.Reader.Close()
                db.Dispose()
            End Try
        End Sub

#End Region

#Region "metodos publicos"

        Public Function Crear() As Boolean
            Dim db As New LMDataAccess
            Dim retorno As Boolean = False
            If _tipo <> String.Empty Then
                With db
                    With .SqlParametros
                        .Add("@tipo", SqlDbType.VarChar).Value = _tipo
                        .Add("@descripcion", SqlDbType.VarChar).Value = _descripcion
                        .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                    End With

                    Try
                        Dim result As Integer
                        .iniciarTransaccion()
                        .ejecutarNonQuery("CrearInfoTipoDetalleOrdenCompra", CommandType.StoredProcedure)
                        result = .SqlParametros("@result").Value
                        If result = 0 Then
                            _idTipoDetalleOrdenCompra = CShort(.SqlParametros("@identity").Value)
                            CargarDatos(_idTipoDetalleOrdenCompra)
                            .confirmarTransaccion()
                            retorno = True
                        End If
                    Catch ex As Exception
                        Throw New Exception(ex.Message)
                    Finally
                        If Not db.Reader.IsClosed Then db.Reader.Close()
                        db.Dispose()
                    End Try
                End With
            End If
            Return retorno
        End Function

        Public Function Actualizar() As Boolean
            Dim retorno As Boolean = False
            If _idTipoDetalleOrdenCompra > 0 Then
                Dim db As New LMDataAccess
                Try
                    With db
                        With .SqlParametros
                            .Add("@idTipo", SqlDbType.SmallInt).Value = _idTipoDetalleOrdenCompra
                            .Add("@tipo", SqlDbType.VarChar).Value = _tipo
                            .Add("@descripcion", SqlDbType.VarChar).Value = _descripcion
                            .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                        End With
                        Dim result As Integer
                        .iniciarTransaccion()
                        .ejecutarNonQuery("ActualizarInfoTipoDetalleOrdenCompra", CommandType.StoredProcedure)
                        result = .SqlParametros("@result").Value
                        If result = 0 Then                            
                            CargarDatos(_idTipoDetalleOrdenCompra)
                            .confirmarTransaccion()
                            retorno = True
                        End If
                    End With
                Catch ex As Exception
                    Throw New Exception(ex.Message)
                Finally
                    If Not db.Reader.IsClosed Then db.Reader.Close()
                    db.Dispose()
                End Try
            End If
            Return retorno
        End Function

#End Region

    End Class
End Namespace

