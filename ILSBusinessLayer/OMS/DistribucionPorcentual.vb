Namespace OMS
    Public Class DistribucionPorcentual

#Region "Variables Privadas"

        Private _idRegion As Integer
        Private _region As Region
        Private _porcentaje As Decimal
        Private _fechaAsignacion As DateTime
        Private _idUsuario As Integer
        Private _error As String

#End Region

#Region "Propiedades"

        Public ReadOnly Property IdRegion() As Integer
            Get
                Return _idRegion
            End Get
        End Property

        Public ReadOnly Property Region() As Region
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

        Public ReadOnly Property FechaAsignacion() As DateTime
            Get
                Return _fechaAsignacion
            End Get
        End Property

        Public Property IdUsuario() As Integer
            Get
                Return _idUsuario
            End Get
            Set(ByVal value As Integer)
                _idUsuario = value
            End Set
        End Property

        Public ReadOnly Property InfoError() As String
            Get
                Return _error
            End Get
        End Property

#End Region

#Region "Estructuras"

        Structure ValorConfiguracion
            Dim idRegion As Integer
            Dim porcentaje As Decimal
        End Structure

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region "Metodos Publicos"

        Public Sub Actualizar(ByVal valores As List(Of ValorConfiguracion))
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                db.iniciarTransaccion()                                
                db.SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                db.SqlParametros.Add("@idRegion", SqlDbType.Int)
                db.SqlParametros.Add("@porcentaje", SqlDbType.Decimal)
                For Each valor As ValorConfiguracion In valores                    
                    db.SqlParametros("@idRegion").Value = valor.idRegion
                    db.SqlParametros("@porcentaje").Value = valor.porcentaje
                    db.ejecutarNonQuery("ActualizarDistribucionPorcentual", CommandType.StoredProcedure)
                Next
                db.confirmarTransaccion()
            Catch ex As Exception
                db.abortarTransaccion()
                db.Dispose()
                Me._error = ex.Message
                Throw New Exception(_error)
            End Try
        End Sub

        Public Sub Registrar()
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                db.iniciarTransaccion()
                db.SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                db.SqlParametros.Add("@idRegion", SqlDbType.Int).Value = _idRegion
                db.SqlParametros.Add("@porcentaje", SqlDbType.Int).Value = _porcentaje
                db.ejecutarNonQuery("RegistrarDistribucionPorcentual", CommandType.StoredProcedure)
                db.confirmarTransaccion()
            Catch ex As Exception
                db.abortarTransaccion()
                db.Dispose()
                Me._error = ex.Message
            End Try
        End Sub

#End Region

#Region "Metodos Compartidos"

        Public Shared Function Obtener() As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim dt As DataTable = db.ejecutarDataTable("ObtenerDistribucionPorcentual", CommandType.StoredProcedure)
            Return dt
        End Function

#End Region

    End Class
End Namespace

