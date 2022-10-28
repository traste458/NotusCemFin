Imports LMDataAccessLayer
Imports ILSBusinessLayer
Imports ILSBusinessLayer.Estructuras


Namespace OMS
    Public Class PreinstruccionPorcentajeRegion

#Region "Variables Privadas"

        Private _idPorcentaje As Integer
        Private _idPreinstruccion As Integer
        Private _idDetalleOrdenCompra As Integer
        Private _idRegion As Integer
        Private _region As Region
        Private _porcentaje As Decimal
        Private _idUsuario As Integer
        Private _fechaRegistro As DateTime
        Private _error As String
        Private _listaPorcentajes As List(Of PreinstruccionPorcentajeRegion)
        Private _dtErrores As DataTable
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

        Public Property ListaPorcentajes() As List(Of PreinstruccionPorcentajeRegion)
            Get
                Return _listaPorcentajes
            End Get
            Set(ByVal value As List(Of PreinstruccionPorcentajeRegion))
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

        Public Property IdPorcentaje() As Integer
            Get
                Return _idPorcentaje
            End Get
            Set(ByVal value As Integer)
                _idPorcentaje = value
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

        'Pendiente la propiedad de preinstruccion cliente

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

        Public Property Porcentaje() As Decimal
            Get
                Return _porcentaje
            End Get
            Set(ByVal value As Decimal)
                _porcentaje = value
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
            _listaPorcentajes = New List(Of PreinstruccionPorcentajeRegion)
        End Sub

        Public Sub New(ByVal idPorcentaje As Integer)
            CargarDatos(idPorcentaje)
        End Sub

#End Region

#Region "Metodos Privados"

        Private Sub CargarDatos(ByVal idPorcentaje As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idPorcentaje", idPorcentaje, SqlDbType.Int)
            Try
                db.ejecutarReader("ObtenerPreInsPorcentajeRegion", CommandType.StoredProcedure)
                If db.Reader IsNot Nothing AndAlso db.Reader.Read Then
                    _idPorcentaje = CInt(db.Reader("idPorcentaje"))
                    _idPreinstruccion = CInt(db.Reader("idPreinstruccion"))
                    _idRegion = CInt(db.Reader("idRegion"))
                    _porcentaje = CDec(db.Reader("porcentaje"))
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

        Public Function Crear(ByVal db As LMDataAccess)
            If db Is Nothing Then db = New LMDataAccess
            db.SqlParametros.Clear()
            Dim retorno As Boolean
            If Me._idPreinstruccion > 0 AndAlso _idRegion > 0 AndAlso _idUsuario > 0 Then
                With db
                    With .SqlParametros
                        .Add("@idPreinstruccion", SqlDbType.Int).Value = _idPreinstruccion
                        .Add("@idRegion", SqlDbType.Int).Value = _idRegion
                        .Add("@porcentaje", SqlDbType.Int).Value = _porcentaje
                        .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                        .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                    End With

                    Try
                        Dim result As Integer
                        .ejecutarNonQuery("CrearPreInsPorcentajeRegion", CommandType.StoredProcedure)
                        result = .SqlParametros("@result").Value
                        If result = 0 Then
                            _idPorcentaje = CInt(.SqlParametros("@identity").Value)
                            CargarDatos(_idPorcentaje)
                            retorno = True
                        End If
                    Catch ex As Exception
                        Me._error = "Error al crear la preinstrucción de porcentaje de región. " & ex.Message
                        Throw New Exception(_error)
                    End Try
                End With
            Else
                Me._error = "Los datos para crear la preinstrucción de porcentaje de región no estan completos"
                Throw New Exception(_error)
            End If
            Return retorno
        End Function

        Public Function Actualizar(ByVal db As LMDataAccess)
            If db Is Nothing Then db = New LMDataAccess
            db.SqlParametros.Clear()
            Dim retorno As Boolean = False
            If _idPorcentaje > 0 Then
                Try
                    With db

                        With .SqlParametros
                            .Add("@idPorcentaje", SqlDbType.Int).Value = _idPorcentaje
                            .Add("@idPreinstruccion", SqlDbType.Int).Value = _idPreinstruccion
                            .Add("@idRegion", SqlDbType.Int).Value = _idRegion
                            .Add("@porcentaje", SqlDbType.Decimal).Value = _porcentaje
                            .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                            .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                        End With
                        Dim result As Integer

                        .ejecutarNonQuery("ActualizarPreInsPorcentajeRegion", CommandType.StoredProcedure)
                        result = .SqlParametros("@result").Value
                        If result = 0 Then
                            CargarDatos(_idPorcentaje)
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
                For Each obj As PreinstruccionPorcentajeRegion In _listaPorcentajes
                    obj.IdPreinstruccion = _idPreinstruccion
                    obj.IdDetalleOrdenCompra = _idDetalleOrdenCompra
                    If obj.IdPorcentaje = 0 Then
                        obj.Crear(db)
                    Else
                        obj.Actualizar(db)
                    End If
                Next
            End If

        End Sub

        Private Function ValidarDatos() As Boolean
            Dim auxContador As Decimal
            Dim flag As Boolean = True
            If _validarPorcentaje Then
                For Each obj As PreinstruccionPorcentajeRegion In _listaPorcentajes
                    auxContador += obj.Porcentaje
                Next

                If auxContador <> 100 Then
                    Recibos.DetalleOrdenCompra.RegistrarError(_dtErrores, _idDetalleOrdenCompra, "La sumatoria del porcentaje de regiones debe dar como resultado 100%")
                    flag = False
                End If
            End If
            Return flag
        End Function
#End Region

#Region "Metodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroPreInsPorcentajeRegion
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroPreInsPorcentajeRegion) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdPorcentaje > 0 Then db.SqlParametros.Add("@idPorcentaje", SqlDbType.Int).Value = .IdPorcentaje
                If .IdPreinstruccion > 0 Then db.SqlParametros.Add("@idPreinstruccion", SqlDbType.Int).Value = .IdPreinstruccion
                If .IdRegion > 0 Then db.SqlParametros.Add("@idRegion", SqlDbType.Int).Value = .IdRegion
                If .Porcentaje > 0 Then db.SqlParametros.Add("@porcentaje", SqlDbType.Decimal).Value = .Porcentaje                
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