Imports ILSBusinessLayer.Estructuras
Imports LMDataAccessLayer

Namespace OMS

    Public Class OrdenCombo

#Region "variables"
        Private _idOrdenCombo As Long
        Private _idMaterial1 As String
        Private _idMaterial2 As String
        Private _idLinea As Integer
        Private _idUsuario As Long
        Private _cantidad As Integer
        Private _cantidadLeida As Integer
        Private _idEstado As Integer

        Public Enum EstadoOrden
            Abierta = 52
            Parcial = 53
            Finalizada = 54
            Anulada = 55
        End Enum
#End Region

#Region "propiedades"

        Public ReadOnly Property idOrdenCombo() As Long
            Get
                Return _idOrdenCombo
            End Get
        End Property

        Public Property IdMaterial1() As String
            Get
                Return _idMaterial1
            End Get
            Set(ByVal value As String)
                _idMaterial1 = value
            End Set
        End Property

        Public Property IdMaterial2() As String
            Get
                Return _idMaterial2
            End Get
            Set(ByVal value As String)
                _idMaterial2 = value
            End Set
        End Property

        Public Property IdLinea() As Integer
            Get
                Return _idLinea
            End Get
            Set(ByVal value As Integer)
                _idLinea = value
            End Set
        End Property

        Public Property IdUsuario() As Long
            Get
                Return _idUsuario
            End Get
            Set(ByVal value As Long)
                _idUsuario = value
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

        Public Property CantidadLeida() As Integer
            Get
                Return _cantidadLeida
            End Get
            Set(ByVal value As Integer)
                _cantidadLeida = value
            End Set
        End Property

        Public Property IdEstado() As Integer
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Integer)
                _idEstado = value
            End Set
        End Property

        Public ReadOnly Property Estado() As String
            Get
                Return _idEstado.ToString
            End Get
        End Property
#End Region

#Region "constructores"
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(ByVal idOrdenCombo As Long)
            Me.New()
            Me.CargarDatos(idOrdenCombo)
            _idOrdenCombo = idOrdenCombo
        End Sub
#End Region

#Region "metodos privados"

        Private Sub CargarDatos(ByVal idOrdenCombo As Long)
            Dim db As New LMDataAccess
            db.SqlParametros.Add("@idOrdenCombo", SqlDbType.BigInt).Value = idOrdenCombo
            Try
                db.ejecutarReader("ObtenerOrdenCombo", CommandType.StoredProcedure)
                If db.Reader.Read Then
                    _idOrdenCombo = db.Reader("idOrdenCombo")
                    _idMaterial1 = db.Reader("idMaterial1")
                    _idMaterial2 = db.Reader("idMaterial2")
                    _idLinea = db.Reader("idLinea")
                    _idUsuario = db.Reader("idUsuario")
                    _cantidad = db.Reader("cantidad")
                    _cantidadLeida = db.Reader("cantidadLeida")
                    _idEstado = db.Reader("idEstado")
                End If
            Catch ex As Exception
            Finally
                If Not db.Reader.IsClosed Then db.Reader.Close()
                db.Dispose()
            End Try
        End Sub

#End Region

#Region "metodos publicos"

        Public Function Crear() As Boolean
            Dim dbManager As New LMDataAccessLayer.LMDataAccess            
            Dim retorno As Boolean = False
            Dim resultado As Short
            With dbManager
                With .SqlParametros
                    .Add("@idMaterial1", SqlDbType.VarChar).Value = _idMaterial1
                    .Add("@idMaterial2", SqlDbType.VarChar).Value = _idMaterial2
                    .Add("@idLinea", SqlDbType.Int).Value = _idLinea
                    .Add("@idUsuario", SqlDbType.BigInt).Value = _idUsuario
                    .Add("@cantidad", SqlDbType.Int).Value = _cantidad
                    .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                    .Add("@cantidadLeida", SqlDbType.Int).Value = _cantidadLeida                    
                    .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                End With
                Try
                    .iniciarTransaccion()
                    .ejecutarNonQuery("CrearOrdenCombo", CommandType.StoredProcedure)
                    Short.TryParse(.SqlParametros("@result").Value.ToString, resultado)
                    Long.TryParse(.SqlParametros("@identity").Value.ToString, _idOrdenCombo)
                    .confirmarTransaccion()
                    retorno = IIf(resultado = 0, True, False)
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso .estadoTransaccional Then .abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                    Return False
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End With
            Return retorno
        End Function

        Public Function Actualizar() As Boolean
            Dim resultado As Integer
            Dim retorno As Boolean = False
            If _idOrdenCombo > 0 Then
                Dim db As New LMDataAccessLayer.LMDataAccess

                Try
                    With db
                        With .SqlParametros
                            .Add("@idOrdenCombo", SqlDbType.BigInt).Value = _idOrdenCombo
                            .Add("@idMaterial1", SqlDbType.VarChar).Value = _idMaterial1
                            .Add("@idMaterial2", SqlDbType.VarChar).Value = _idMaterial2
                            .Add("@idLinea", SqlDbType.Int).Value = _idLinea
                            .Add("@idUsuario", SqlDbType.BigInt).Value = _idUsuario
                            .Add("@cantidad", SqlDbType.Int).Value = _cantidad
                            .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                            .Add("@cantidadLeida", SqlDbType.Int).Value = _cantidadLeida
                            .Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        End With
                        .iniciarTransaccion()
                        .ejecutarNonQuery("ActualizarOrdenCombo", CommandType.StoredProcedure)
                        Short.TryParse(.SqlParametros("@returnValue").Value.ToString, resultado)                       
                        If resultado = 0 Then
                            .confirmarTransaccion()                            
                        End If

                    End With
                Catch ex As Exception
                    If db.estadoTransaccional Then db.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    db.cerrarConexion()
                End Try
            Else
                Throw New DuplicateNameException("La Orden Combo aún no ha sido registrada en la Base de Datos.")
            End If
            Return Not resultado
        End Function

#End Region

#Region "métodos compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroOrdenCombo
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroOrdenCombo) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdOrdenCombo > 0 Then db.SqlParametros.Add("@idOrdenCombo", SqlDbType.BigInt).Value = .IdOrdenCombo
                If .IdMaterial1 > 0 Then db.SqlParametros.Add("@idMaterial1", SqlDbType.VarChar).Value = .IdMaterial1
                If .IdMaterial2 > 0 Then db.SqlParametros.Add("@idMaterial2", SqlDbType.VarChar).Value = .IdMaterial2
                If .IdLinea > 0 Then db.SqlParametros.Add("@idLinea", SqlDbType.Int).Value = .IdLinea
                If .IdUsuario > 0 Then db.SqlParametros.Add("@idUsuario", SqlDbType.BigInt).Value = .IdUsuario
                If .Cantidad > 0 Then db.SqlParametros.Add("@cantidad", SqlDbType.Int).Value = .Cantidad
                If .IdEstado > 0 Then db.SqlParametros.Add("@idEstado", SqlDbType.Int).Value = .IdEstado
                If .CantidadLeida > 0 Then db.SqlParametros.Add("@cantidadLeida", SqlDbType.Int).Value = .CantidadLeida
                If .FechaInicial <> Date.MinValue Then db.SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = .FechaInicial
                If .FechaFinal <> Date.MinValue Then db.SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = .FechaFinal
                If .ListaEstado IsNot Nothing AndAlso .ListaEstado.Count Then db.SqlParametros.Add("@listaEstado", SqlDbType.VarChar).Value = Join(.ListaEstado.ToArray, ",")
                dtDatos = db.ejecutarDataTable("ObtenerOrdenCombo", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos

        End Function

#End Region

    End Class
End Namespace

