Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer

Namespace OMS
    Public Class PreinstruccionPorcentajeSubdistribucion

#Region "Variables privadas"

        Private _idCantidad As Integer
        Private _idPreinstruccion As Integer
        'Private _preinstruccion As Preinstruccion
        Private _idSubdistribucion As Short
        Private _subdistribucion As SubdistribucionInstruccion
        Private _porcentaje As Decimal
        Private _cantidad As Integer
        Private _idUsuario As Integer
        Private _fechaRegistro As DateTime
        Private _error As String
        Private _dtErrores As DataTable
        Private _listaPorcentajes As List(Of PreinstruccionPorcentajeSubdistribucion)
        Private _idDetalleOrdenCompra As Integer
        Private _validarPorcentaje As Boolean
#End Region

#Region "Propiedades"

        Public Property ValidarPorcentaje() As Boolean
            Get
                Return _validarPorcentaje
            End Get
            Set(ByVal value As Boolean)
                _validarPorcentaje = value
            End Set
        End Property

        Public Property ListaPorcentajes() As List(Of PreinstruccionPorcentajeSubdistribucion)
            Get
                Return _listaPorcentajes
            End Get
            Set(ByVal value As List(Of PreinstruccionPorcentajeSubdistribucion))
                _listaPorcentajes = value
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

        'Pendiente crear propiedad Preinstruccion

        Public Property IdSubdistribucion() As Short
            Get
                Return _idSubdistribucion
            End Get
            Set(ByVal value As Short)
                _idSubdistribucion = value
            End Set
        End Property

        Public ReadOnly Property Subdistribucion() As SubdistribucionInstruccion
            Get
                If Not _subdistribucion Is Nothing Then
                    Return _subdistribucion
                Else
                    If _idSubdistribucion > 0 Then
                        _subdistribucion = New SubdistribucionInstruccion(_idSubdistribucion)
                        Return _subdistribucion
                    Else
                        Return New SubdistribucionInstruccion()
                    End If
                End If
            End Get
        End Property

        Public Property Porcentaje() As Decimal
            Get
                Return _porcentaje
            End Get
            Set(ByVal value As Decimal)
                _porcentaje = value
            End Set
        End Property

        Public Property Cantidad() As Integer
            Get
                Return _cantidad
            End Get
            Set(ByVal value As Integer)
                _cantidad = value
            End Set
        End Property

        Public Property idUsuario() As Integer
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

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _listaPorcentajes = New List(Of PreinstruccionPorcentajeSubdistribucion)

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
                db.ejecutarReader("ObtenerPreInsPorcentajeSubdistribucion", CommandType.StoredProcedure)
                If db.Reader IsNot Nothing AndAlso db.Reader.Read Then
                    _idCantidad = CInt(db.Reader("idCantidad"))
                    _idPreinstruccion = CInt(db.Reader("idPreinstruccion"))
                    _idSubdistribucion = CShort(db.Reader("idSubdistribucion"))
                    _porcentaje = CDec(db.Reader("porcentaje"))
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
            If _idPreinstruccion > 0 AndAlso _idUsuario > 0 AndAlso _idUsuario > 0 Then
                With db
                    With .SqlParametros
                        .Add("@idPreinstruccion", SqlDbType.Int).Value = _idPreinstruccion
                        .Add("@idSubdistribucion", SqlDbType.SmallInt).Value = _idSubdistribucion
                        .Add("@porcentaje", SqlDbType.Decimal).Value = _porcentaje
                        .Add("@cantidad", SqlDbType.Int).Value = _cantidad
                        .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                        .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                    End With
                    Try
                        Dim result As Integer
                        .ejecutarNonQuery("CrearPreInsPorcentajeSubdistribucion", CommandType.StoredProcedure)
                        result = .SqlParametros("@result").Value
                        If result = 0 Then
                            _idCantidad = CInt(.SqlParametros("@identity").Value)
                            CargarDatos(_idCantidad)
                            retorno = True
                        End If
                    Catch ex As Exception
                        Me._error = "Error al crear la preinstrucción de porcentaje de subdistribución. " & ex.Message
                        Throw New Exception(_error)
                    End Try
                End With
            Else
                Me._error = "Los datos para crear la preinstrucción de porcentaje de subdistribución no estan completos"
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
                            .Add("@idSubdistribucion", SqlDbType.SmallInt).Value = _idSubdistribucion
                            .Add("@porcentaje", SqlDbType.Decimal).Value = _porcentaje
                            .Add("@cantidad", SqlDbType.Int).Value = _cantidad
                            .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                            .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                        End With
                        Dim result As Integer

                        .ejecutarNonQuery("ActualizarPreInsPorcentajeSubdistribucion", CommandType.StoredProcedure)
                        result = .SqlParametros("@result").Value
                        If result = 0 Then
                            CargarDatos(_idSubdistribucion)
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
            If Me.ValidarDatos() Then
                For Each obj As PreinstruccionPorcentajeSubdistribucion In _listaPorcentajes
                    obj.IdPreinstruccion = _idPreinstruccion
                    obj.IdDetalleOrdenCompra = _idDetalleOrdenCompra
                    If obj.IdCantidad = 0 Then
                        obj.Crear(db)
                    Else
                        obj.Actualizar(db)
                    End If
                Next
            End If

        End Sub

        Private Function ValidarDatos() As Boolean
            Dim flag As Boolean = True
            If _validarPorcentaje Then
                For Each obj As PreinstruccionPorcentajeSubdistribucion In _listaPorcentajes
                    If obj._porcentaje > 100 Then
                        Recibos.DetalleOrdenCompra.RegistrarError(_dtErrores, _idDetalleOrdenCompra, "El porcentaje de subDistribución no debe sobrepasar el 100%")
                        flag = False
                    End If
                Next
            End If
            Return flag
        End Function
#End Region

#Region "Metodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroPreInsPorcentajeSubdistribucion
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroPreInsPorcentajeSubdistribucion) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdCantidad > 0 Then db.SqlParametros.Add("@idCantidad", SqlDbType.Int).Value = .IdCantidad
                If .IdPreinstruccion > 0 Then db.SqlParametros.Add("@idPreinstruccion", SqlDbType.Int).Value = .IdPreinstruccion
                If .IdSubdistribucion > 0 Then db.SqlParametros.Add("@idSubdistribucion", SqlDbType.SmallInt).Value = .IdSubdistribucion
                If .Porcentaje > 0 Then db.SqlParametros.Add("@porcentaje", SqlDbType.Decimal).Value = .Porcentaje
                If .Cantidad > 0 Then db.SqlParametros.Add("@cantidad", SqlDbType.Int).Value = .Cantidad
                If .IdUsuario > 0 Then db.SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = .IdUsuario
                If .FechaInicial <> Date.MinValue Then db.SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = .FechaInicial
                If .FechaFinal <> Date.MinValue Then db.SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = .FechaFinal

                dtDatos = db.ejecutarDataTable("ObtenerPreInsPorcentajeSubdistribucion", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos
        End Function

#End Region
        
    End Class
End Namespace
