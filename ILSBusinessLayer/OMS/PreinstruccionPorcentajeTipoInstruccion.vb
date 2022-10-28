Imports LMDataAccessLayer

Namespace OMS
    Public Class PreinstruccionPorcentajeTipoInstruccion

#Region "Variables privadas"
        Private _idPorcentaje As Integer
        Private _idPreinstruccion As Integer
        Private _idTipoInstruccion As Integer
        Private _porcentaje As Decimal
        Private _idUsuario As Integer
        Private _fechaRegistro As Date
        Private _dtErrores As DataTable
        Private _listaPorcentajes As List(Of PreinstruccionPorcentajeTipoInstruccion)
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

        Public Property ListaPorcentajes() As List(Of PreinstruccionPorcentajeTipoInstruccion)
            Get
                Return _listaPorcentajes
            End Get
            Set(ByVal value As List(Of PreinstruccionPorcentajeTipoInstruccion))
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

        Public Property FechaRegistro() As Date
            Get
                Return _fechaRegistro
            End Get
            Set(ByVal value As Date)
                _fechaRegistro = value
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

        Public Property Porcentaje() As Decimal
            Get
                Return _porcentaje
            End Get
            Set(ByVal value As Decimal)
                _porcentaje = value
            End Set
        End Property

        Public Property IdTipoInstruccion() As Integer
            Get
                Return _idTipoInstruccion
            End Get
            Set(ByVal value As Integer)
                _idTipoInstruccion = value
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

        Public Property IdPorcentaje() As Integer
            Get
                Return _idPorcentaje
            End Get
            Set(ByVal value As Integer)
                _idPorcentaje = value
            End Set
        End Property

#End Region

#Region "Metodos"
        Public Sub Crear(Optional ByVal db As LMDataAccessLayer.LMDataAccess = Nothing)
            If db Is Nothing Then db = New LMDataAccessLayer.LMDataAccess
            db.SqlParametros.Clear()
            If _idPreinstruccion > 0 And _idUsuario > 0 Then

                With db.SqlParametros
                    .Add("@idPorcentaje", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@idPreinstruccion", SqlDbType.Int).Value = _idPreinstruccion
                    .Add("@idTipoInstruccion", SqlDbType.Int).Value = _idTipoInstruccion
                    .Add("@porcentaje", SqlDbType.Decimal, 10).Value = _porcentaje
                    .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .Add("@identity", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                Try
                    db.ejecutarNonQuery("CrearPreInsPorcentajeTipoInstruccion", CommandType.StoredProcedure)
                    If db.SqlParametros("@identity") IsNot Nothing Then
                        _idPorcentaje = db.SqlParametros("@identity").Value
                    End If
                Catch ex As Exception
                    Throw New Exception("Error al tratar de asignar los porcentajes de tipo de instrucción. " & ex.Message)
                End Try
            End If
        End Sub

        Public Sub Actualizar(ByVal db As LMDataAccess)
            If db Is Nothing Then db = New LMDataAccess
            db.SqlParametros.Clear()
            Dim retorno As Boolean = False
            If _idPorcentaje > 0 Then

                Try
                    With db
                        With .SqlParametros
                            .Add("@idPorcentaje", SqlDbType.Int).Value = _idPorcentaje
                            .Add("@porcentaje", SqlDbType.Decimal, 10).Value = _porcentaje
                            .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                        End With
                    End With


                    db.ejecutarNonQuery("ActualizarPreInsPorcentajeTipoInstruccion", CommandType.StoredProcedure)

                Catch ex As Exception
                    Throw New Exception(ex.Message)
                End Try
            End If
        End Sub

        Friend Sub Procesar(ByVal db As LMDataAccess, ByVal dtErrores As DataTable)
            db.SqlParametros.Clear()
            _dtErrores = dtErrores
            If Me.ValidarDatos() Then
                For Each obj As PreinstruccionPorcentajeTipoInstruccion In _listaPorcentajes
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
                For Each obj As PreinstruccionPorcentajeTipoInstruccion In _listaPorcentajes
                    auxContador += obj.Porcentaje
                Next
                If auxContador <> 100 Then
                    Recibos.DetalleOrdenCompra.RegistrarError(_dtErrores, _idDetalleOrdenCompra, "La sumatoria del porcentaje de los tipo de instruccion debe dar como resultado 100%")
                    flag = False
                End If
            End If
            Return flag
        End Function
#End Region

       
        Public Sub New()
            MyBase.New()
            _listaPorcentajes = New List(Of PreinstruccionPorcentajeTipoInstruccion)

        End Sub
    End Class
End Namespace

