Imports ILSBusinessLayer
Imports ILSBusinessLayer.Estructuras
Imports LMDataAccessLayer

Namespace OMS
    Public Class PreinstruccionCantidadDistribucion

#Region "Variables Privadas"

        Private _idCantidad As Integer
        Private _idPreinstruccion As Integer
        'Private _preinstruccion As PreinstruccionCliente
        Private _idRegion As Integer
        Private _region As Region
        Private _idTipoInstruccion As Short
        Private _tipoInstruccion As TipoInstruccion
        Private _cantidad As Integer
        Private _idUsuario As Integer
        Private _fechaRegistro As DateTime
        Private _error As String
        Private _dtErrores As DataTable
        Private _listaCantidades As List(Of PreinstruccionCantidadDistribucion)
        Private _idDetalleOrdenCompra As Integer
#End Region

#Region "Propiedades"

        Public Property ListaPorcentajes() As List(Of PreinstruccionCantidadDistribucion)
            Get
                Return _listaCantidades
            End Get
            Set(ByVal value As List(Of PreinstruccionCantidadDistribucion))
                _listaCantidades = value
            End Set
        End Property

        Public Property IdDetalleOrdenCompra() As Integer
            Get
                Return _idDetalleOrdenCompra
            End Get
            Set(ByVal value As Integer)
                _idDetalleOrdenCompra = value
            End Set
        End Property

        Public ReadOnly Property TotalInstruccionado() As Integer
            Get
                Dim auxContador As Decimal
                For Each obj As PreinstruccionCantidadDistribucion In _listaCantidades
                    auxContador += obj.Cantidad
                Next
                Return _idCantidad
            End Get
        End Property

        Public Property IdCantidad() As Integer
            Get
                Return _idCantidad
            End Get
            Set(ByVal value As Integer)
                _idCantidad = value
            End Set
        End Property

        Public Property IdPreinstruccion() As Integer
            Get
                Return _idPreinstruccion
            End Get
            Set(ByVal value As Integer)
                _idPreinstruccion = value
            End Set
        End Property

        Public Property IdRegion() As Integer
            Get
                Return _idRegion
            End Get
            Set(ByVal value As Integer)
                _idRegion = value
            End Set
        End Property

        Public ReadOnly Property RegionObj() As Region
            Get
                If Not _region Is Nothing Then
                    Return _region
                Else
                    If _idRegion > 0 Then
                        _region = New Region(_idRegion)
                        Return _region
                    Else
                        Return New Region()
                    End If
                End If
            End Get
        End Property

        Public Property IdTipoInstruccion() As Short
            Get
                Return _idTipoInstruccion
            End Get
            Set(ByVal value As Short)
                _idTipoInstruccion = value
            End Set
        End Property

        Public ReadOnly Property TipoInstruccionObj() As TipoInstruccion
            Get
                If Not _tipoInstruccion Is Nothing Then
                    Return _tipoInstruccion
                Else
                    If _idTipoInstruccion > 0 Then
                        _tipoInstruccion = New TipoInstruccion(_idTipoInstruccion)
                        Return _tipoInstruccion
                    Else
                        Return New TipoInstruccion()
                    End If
                End If
            End Get
        End Property

        Public Property Cantidad() As Integer
            Get
                Return _cantidad
            End Get
            Set(ByVal value As Integer)
                _cantidad = value
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

        Public ReadOnly Property FechaRegistro() As DateTime
            Get
                Return _fechaRegistro
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
            _listaCantidades = New List(Of PreinstruccionCantidadDistribucion)

        End Sub

        Public Sub New(ByVal idCantidad As Integer)
            CargarDatos(idCantidad)
        End Sub

#End Region

#Region "Metodos Privados"

        Private Sub CargarDatos(ByVal idCantidad As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idCantidad", idCantidad, SqlDbType.Int)
            Try
                db.ejecutarReader("ObtenerPreInsCantidadDistribucion", CommandType.StoredProcedure)
                If db.Reader IsNot Nothing AndAlso db.Reader.Read Then
                    _idCantidad = CInt(db.Reader("idCantidad"))
                    _idPreinstruccion = CInt(db.Reader("idPreinstruccion"))
                    _idRegion = CInt(db.Reader("idRegion"))
                    _idTipoInstruccion = CShort(db.Reader("idTipoInstruccion"))
                    _cantidad = CInt(db.Reader("cantidad"))
                    _idUsuario = CInt(db.Reader("idUsuario"))
                    _fechaRegistro = CDate(db.Reader("fechaRegistro"))
                End If
            Finally
                If Not db.Reader.IsClosed Then db.Reader.Close()
                db.Dispose()
            End Try
        End Sub

#End Region

#Region "Metodos Publicos"

        Public Function Crear(Optional ByVal db As LMDataAccess = Nothing)
            If db Is Nothing Then db = New LMDataAccess
            db.SqlParametros.Clear()
            Dim retorno As Boolean
            If _idPreinstruccion > 0 AndAlso _idRegion > 0 AndAlso _idTipoInstruccion > 0 AndAlso _idUsuario > 0 Then
                With db
                    With .SqlParametros
                        .Add("@idPreinstruccion", SqlDbType.Int).Value = _idPreinstruccion
                        .Add("@idRegion", SqlDbType.Int).Value = _idRegion
                        .Add("@idTipoInstruccion", SqlDbType.SmallInt).Value = _idTipoInstruccion
                        .Add("@cantidad", SqlDbType.Int).Value = _cantidad
                        .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                        .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                    End With
                    Try
                        Dim result As Integer
                        .ejecutarNonQuery("CrearPreInsCantidadDistribucion", CommandType.StoredProcedure)
                        result = .SqlParametros("@result").Value
                        If result = 0 Then
                            _idCantidad = CInt(.SqlParametros("@identity").Value)
                            CargarDatos(_idCantidad)
                            retorno = True
                        End If
                    Catch ex As Exception
                        Me._error = "Error al crear la preinstrucción de cantidad distribución. " & ex.Message
                        Throw New Exception(_error)
                    End Try
                End With
            Else
                Me._error = "Los datos para crear la preinstrucción de cantidad distribución no estan completos"
                Throw New Exception(_error)
            End If
            Return retorno
        End Function

        Public Function Actualizar(Optional ByVal db As LMDataAccess = Nothing)
            If db Is Nothing Then db = New LMDataAccess
            db.SqlParametros.Clear()
            Dim retorno As Boolean = False
            If _idCantidad > 0 Then
                Try
                    With db
                        With .SqlParametros
                            .Add("@idCantidad", SqlDbType.Int).Value = _idCantidad
                            .Add("@idPreinstruccion", SqlDbType.Int).Value = _idPreinstruccion
                            .Add("@idRegion", SqlDbType.Int).Value = _idRegion
                            .Add("@idTipoInstruccion", SqlDbType.SmallInt).Value = _idTipoInstruccion
                            .Add("@cantidad", SqlDbType.Int).Value = _cantidad
                            .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                            .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                        End With
                        Dim result As Integer
                        .ejecutarNonQuery("ActualizarPreInsCantidadDistribucion", CommandType.StoredProcedure)
                        result = .SqlParametros("@result").Value
                        If result = 0 Then
                            CargarDatos(_idCantidad)
                            retorno = True
                        End If
                    End With
                Catch ex As Exception
                    db.abortarTransaccion()
                    Throw New Exception(ex.Message)
                End Try
            End If
            Return retorno
        End Function

        Friend Sub Procesar(ByVal db As LMDataAccess, ByVal dtErrores As DataTable)
            db.SqlParametros.Clear()
            _dtErrores = dtErrores

            For Each obj As PreinstruccionCantidadDistribucion In _listaCantidades
                obj.IdPreinstruccion = _idPreinstruccion
                obj.IdDetalleOrdenCompra = _idDetalleOrdenCompra
                If obj.IdCantidad = 0 Then
                    obj.Crear(db)
                Else
                    obj.Actualizar(db)
                End If
            Next

        End Sub

       
#End Region

#Region "Metodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroPreInsCantidadDistribucion
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroPreInsCantidadDistribucion) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdCantidad > 0 Then db.SqlParametros.Add("@idCantidad", SqlDbType.Int).Value = .IdCantidad
                If .IdPreinstruccion > 0 Then db.SqlParametros.Add("@idPreinstruccion", SqlDbType.Int).Value = .IdPreinstruccion
                If .IdRegion > 0 Then db.SqlParametros.Add("@idRegion", SqlDbType.Int).Value = .IdRegion
                If .IdTipoInstruccion > 0 Then db.SqlParametros.Add("@idTipoInstruccion", SqlDbType.SmallInt).Value = .IdTipoInstruccion
                If .Cantidad > 0 Then db.SqlParametros.Add("@cantidad", SqlDbType.Decimal).Value = .Cantidad
                If .IdUsuario > 0 Then db.SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = .IdUsuario
                If .FechaInicial <> Date.MinValue Then db.SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = .FechaInicial
                If .FechaFinal <> Date.MinValue Then db.SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = .FechaFinal

                dtDatos = db.ejecutarDataTable("ObtenerPreInsPorcentajeRegion", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos
        End Function

#End Region
        
    End Class
End Namespace
