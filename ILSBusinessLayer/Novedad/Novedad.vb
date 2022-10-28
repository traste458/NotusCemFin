Imports ILSBusinessLayer.Estructuras
Imports LMDataAccessLayer
Namespace Novedad
    Public Class Novedad
#Region "variables"
        Private _idNovedad As Integer
        Private _idTipoNovedad As Integer
        Private _descripcion As String
        Private _estado As Boolean
#End Region

#Region "propiedades"

        Public ReadOnly Property IdNovedad() As Integer
            Get
                Return _idNovedad
            End Get
        End Property

        Public Property IdTipoNovedad() As Integer
            Get
                Return _idTipoNovedad
            End Get
            Set(ByVal value As Integer)
                _idTipoNovedad = value
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

        Public Property Estado() As Boolean
            Get
                Return _estado
            End Get
            Set(ByVal value As Boolean)
                _estado = value
            End Set
        End Property
#End Region

#Region "constructores"

        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(ByVal idNovedad As Integer)
            Me.New()
            Me.CargarDatos(idNovedad)
            _idNovedad = idNovedad
        End Sub

#End Region

#Region "metodos privados"

        Private Sub CargarDatos(ByVal idNovedad As Integer)
            Dim db As New LMDataAccess
            db.SqlParametros.Add("@idNovedad", SqlDbType.Int).Value = idNovedad
            Try
                db.ejecutarReader("ObtenerNovedadILS", CommandType.StoredProcedure)
                If db.Reader.Read Then
                    _idTipoNovedad = db.Reader("idTipoNovedad")
                    _descripcion = db.Reader("descripcion").ToString
                    _estado = db.Reader("estado")
                End If
            Catch ex As Exception
            Finally
                db.Dispose()
            End Try
        End Sub

#End Region

#Region "metodos publicos"

        Public Function Crear() As Boolean
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim retorno As Boolean
            With db
                With .SqlParametros
                    .Add("@idTipoNovedad", SqlDbType.Int).Value = _idTipoNovedad
                    .Add("@descripcion", SqlDbType.VarChar).Value = _descripcion
                    .Add("@estado", SqlDbType.Bit).Value = _estado                    
                    .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                End With

                Try
                    Dim result As Integer = 0
                    .iniciarTransaccion()
                    .ejecutarNonQuery("CrearNovedadILS", CommandType.StoredProcedure)
                    result = .SqlParametros("@result").Value
                    If result = 0 Then
                        _idNovedad = CLng(.SqlParametros("@identity").Value)
                        .confirmarTransaccion()
                        retorno = True
                    Else
                        Throw New Exception("Imposible registrar la información de la Novedad.")
                    End If

                Catch ex As Exception
                    If .estadoTransaccional Then .abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    .cerrarConexion()
                    .Dispose()
                End Try
            End With
            Return retorno
        End Function

        Public Sub Actualizar()
            If IdNovedad <> 0 Then
                Dim db As New LMDataAccessLayer.LMDataAccess

                Try
                    db.iniciarTransaccion()
                    With db.SqlParametros
                        .Add("@idNovedad", SqlDbType.BigInt).Value = _idNovedad
                        .Add("@idTipoNovedad", SqlDbType.Int).Value = _idTipoNovedad
                        .Add("@descripcion", SqlDbType.VarChar).Value = _descripcion
                        .Add("@estado", SqlDbType.Bit).Value = _estado
                    End With
                    db.ejecutarNonQuery("ActualizarNovedadILS", CommandType.StoredProcedure)
                    db.confirmarTransaccion()
                Catch ex As Exception
                    If db.estadoTransaccional Then db.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    db.cerrarConexion()
                End Try
            Else
                Throw New DuplicateNameException("La Novedad aún no ha sido registrada en la Base de Datos.")
            End If
        End Sub

#End Region

#Region "metodos compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroNovedadILS
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroNovedadILS) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdNovedad > 0 Then db.SqlParametros.Add("@idNovedad", SqlDbType.Int).Value = .IdNovedad
                If .IdTipoNovedad > 0 Then db.SqlParametros.Add("@idTipoNovedad", SqlDbType.Int).Value = .IdTipoNovedad
                If .Descripcion IsNot Nothing AndAlso .Descripcion.Trim.Length > 0 Then _
                    db.SqlParametros.Add("@descripcion", SqlDbType.VarChar).Value = .Descripcion
                If .Estado <> False Then db.SqlParametros.Add("@estado", SqlDbType.Bit).Value = .Estado
                dtDatos = db.ejecutarDataTable("ObtenerNovedadILS", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos

        End Function

#End Region

    End Class
End Namespace

