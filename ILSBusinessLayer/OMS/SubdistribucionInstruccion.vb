Imports LMDataAccessLayer
Imports ILSBusinessLayer
Imports ILSBusinessLayer.Estructuras

Namespace OMS
    Public Class SubdistribucionInstruccion

#Region "Variables privadas"

        Private _idSubdistribucion As Short
        Private _idRegionEquivalente As Integer
        Private _regionEquivalente As Region
        Private _codigo As String
        Private _nombre As String
        Private _activo As Boolean
        Private _idRegionPadre As Integer
        Private _regionPadre As Region
        Private _idTipoInstruccionPadre As Short
        Private _tipoInstruccionPadre As TipoInstruccion
        Private _error As String

#End Region

#Region "Propiedades"

        Public Property IdSubdistribucion() As Short
            Get
                Return _idSubdistribucion
            End Get
            Set(ByVal value As Short)
                _idSubdistribucion = value
            End Set
        End Property

        Public Property IdRegionEquivalente() As Integer
            Get
                Return _idRegionEquivalente
            End Get
            Set(ByVal value As Integer)
                _idRegionEquivalente = value
            End Set
        End Property

        Public ReadOnly Property RegionEquivalente() As Region
            Get
                If _regionEquivalente Is Nothing Then
                    If _idRegionEquivalente > 0 Then
                        Return New Region(_idRegionEquivalente)
                    End If
                    Return New Region()
                Else
                    Return _regionEquivalente
                End If
            End Get
        End Property

        Public Property Codigo() As Integer
            Get
                Return _codigo
            End Get
            Set(ByVal value As Integer)
                _codigo = value
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

        Public Property Activo() As Boolean
            Get
                Return _activo
            End Get
            Set(ByVal value As Boolean)
                _activo = value
            End Set
        End Property

        Public Property IdRegionPadre() As Integer
            Get
                Return _idRegionPadre
            End Get
            Set(ByVal value As Integer)
                _idRegionPadre = value
            End Set
        End Property

        Public ReadOnly Property RegionPadre() As Region
            Get
                If _regionPadre Is Nothing Then
                    If _idRegionPadre > 0 Then
                        Return New Region(_idRegionPadre)
                    End If
                    Return New Region()
                Else
                    Return _regionPadre
                End If
            End Get
        End Property

        Public Property IdTipoInstruccionPadre() As Integer
            Get
                Return _idTipoInstruccionPadre
            End Get
            Set(ByVal value As Integer)
                _idTipoInstruccionPadre = value
            End Set
        End Property

        Public ReadOnly Property TipoInstruccionPadre() As TipoInstruccion
            Get
                If _tipoInstruccionPadre Is Nothing Then
                    If _idTipoInstruccionPadre > 0 Then
                        Return New TipoInstruccion(_idTipoInstruccionPadre)
                    End If
                    Return New TipoInstruccion()
                Else
                    Return _tipoInstruccionPadre
                End If
            End Get
        End Property

        Public ReadOnly Property InfoError() As String
            Get
                Return _error
            End Get
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idSubdistribucion As Integer)
            CargarDatos(idSubdistribucion)
        End Sub

#End Region

#Region "Metodos Privados"

        Private Sub CargarDatos(ByVal idSubinstruccion As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idTipoInstruccion", IdSubdistribucion, SqlDbType.Int)
            Try
                db.ejecutarReader("ObtenerSubdistribucionInstruccion", CommandType.StoredProcedure)
                If db.Reader IsNot Nothing AndAlso db.Reader.Read Then
                    _idSubdistribucion = CInt(db.Reader("idSubdistribucion"))
                    _idRegionEquivalente = CInt(db.Reader("idRegionEquivalente"))
                    _codigo = db.Reader("codigo").ToString()
                    _nombre = db.Reader("nombre").ToString()
                    _activo = CBool(db.Reader("activo"))
                    _idRegionPadre = CInt(db.Reader("idRegionPadre"))
                    _idTipoInstruccionPadre = CShort(db.Reader("idTipoInstruccionPadre"))
                End If
            Finally
                If Not db.Reader.IsClosed Then db.Reader.Close()
                db.Dispose()
            End Try
        End Sub

#End Region

#Region "Metodos Publicos"

        Public Function Crear()
            Dim db As New LMDataAccess
            Dim retorno As Boolean
            If _codigo <> String.Empty AndAlso _nombre <> String.Empty AndAlso _idRegionPadre > 0 AndAlso _idTipoInstruccionPadre > 0 Then
                With db
                    With .SqlParametros
                        .Add("@idRegionEquivalente", SqlDbType.Int).IsNullable = IIf(_idRegionEquivalente > 0, _idRegionEquivalente, DBNull.Value)
                        .Add("@codigo", SqlDbType.VarChar).Value = _codigo
                        .Add("@nombre", SqlDbType.VarChar).Value = _nombre
                        .Add("@idRegionPadre", SqlDbType.Int).Value = _idRegionPadre
                        .Add("@idTipoInstruccionPadre", SqlDbType.SmallInt).Value = _idTipoInstruccionPadre
                        .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                    End With

                    Try
                        Dim result As Integer
                        .iniciarTransaccion()
                        .ejecutarNonQuery("CrearSubdistribucionInstruccion", CommandType.StoredProcedure)
                        result = .SqlParametros("@result").Value
                        If result = 0 Then
                            _idSubdistribucion = CInt(.SqlParametros("@identity").Value)
                            CargarDatos(_idSubdistribucion)
                            .confirmarTransaccion()
                            retorno = True
                        End If
                    Catch ex As Exception
                        .abortarTransaccion()
                        Me._error = "Error al crear la subdistribución instrucción de orden de compra. " & ex.Message
                        Throw New Exception(_error)
                    Finally
                        db.Dispose()
                    End Try
                End With
            Else
                Me._error = "Los datos para crear la subdistribución instrucción no esta completos"
            End If
            Return retorno
        End Function

        Public Function Actualizar() As Boolean
            Dim retorno As Boolean = False
            If _idSubdistribucion > 0 Then
                Dim db As New LMDataAccess
                Try
                    With db
                        With .SqlParametros
                            .Add("@idSubdistribucion", SqlDbType.Int).Value = _idSubdistribucion
                            .Add("@idRegionEquivalente", SqlDbType.SmallInt).IsNullable = IIf(_idRegionEquivalente > 0, _idRegionEquivalente, DBNull.Value)
                            .Add("@codigo", SqlDbType.VarChar).Value = _codigo
                            .Add("@nombre", SqlDbType.VarChar).Value = _nombre
                            .Add("@activo", SqlDbType.Bit).Value = _activo
                            .Add("@idRegionPadre", SqlDbType.Int).Value = _idRegionPadre
                            .Add("@idTipoInstruccionPadre", SqlDbType.Int).Value = _idTipoInstruccionPadre
                            .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                        End With
                        Dim result As Integer
                        .iniciarTransaccion()
                        .ejecutarNonQuery("ActualizarInfoSubdistribucionInstruccion", CommandType.StoredProcedure)
                        result = .SqlParametros("@result").Value
                        If result = 0 Then
                            CargarDatos(_idSubdistribucion)
                            .confirmarTransaccion()
                            retorno = True
                        End If
                    End With
                Catch ex As Exception
                    db.abortarTransaccion()
                    Throw New Exception(ex.Message)
                Finally
                    If Not db.Reader.IsClosed Then db.Reader.Close()
                    db.Dispose()
                End Try
            End If
            Return retorno
        End Function

#End Region

#Region "Metodos Compartidos"
        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroSubdistribucionInstruccion
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroSubdistribucionInstruccion) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdSubdistribucion > 0 Then db.SqlParametros.Add("@idSubdistribucion", SqlDbType.Int).Value = .IdSubdistribucion
                If .IdRegionEquivalente > 0 Then db.SqlParametros.Add("@idRegionEquivalente", SqlDbType.SmallInt).Value = .IdRegionEquivalente
                If .Codigo <> String.Empty Then db.SqlParametros.Add("@codigo", SqlDbType.VarChar).Value = .Codigo
                If .Nombre <> String.Empty Then db.SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = .Nombre
                If .Activo = Enumerados.EstadoBinario.Activo Then db.SqlParametros.Add("@activo", SqlDbType.Bit).Value = .Activo
                If .IdRegionPadre > 0 Then db.SqlParametros.Add("@idRegionPadre", SqlDbType.Int).Value = .IdRegionPadre
                If .IdTipoInstruccionPadre > 0 Then db.SqlParametros.Add("@idTipoInstruccionPadre", SqlDbType.Int).Value = .IdTipoInstruccionPadre

                dtDatos = db.ejecutarDataTable("ObtenerSubdistribucionInstruccion", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos
        End Function

#End Region

    End Class
End Namespace
