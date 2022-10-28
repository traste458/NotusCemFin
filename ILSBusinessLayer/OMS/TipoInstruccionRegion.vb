Namespace OMS
    Public Class TipoInstruccionRegion

#Region "variables"
        Private _idTipoInstruccion As Integer
        Private _idRegion As Integer
#End Region

#Region "Propiedades"

        Public Property IdTipoInstruccion() As Integer
            Get
                Return _idTipoInstruccion
            End Get
            Set(ByVal value As Integer)
                _idTipoInstruccion = value
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

#End Region

#Region "Constructorres"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region "Metodos Compartidos"

        Public Shared Function ObtenerTodosCadena(Optional ByVal idTipoInstruccion As Integer = 0, Optional ByVal idRegion As Integer = 0) As String
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim retorno As String
            Try
                If idTipoInstruccion > 0 Then db.SqlParametros.Add("@idTipoInstruccion", SqlDbType.Int).Value = idTipoInstruccion
                If idRegion > 0 Then db.SqlParametros.Add("@idRegion", SqlDbType.Int).Value = idRegion
                db.ejecutarReader("ObtenerCadenaTipoInstruccionRegion", CommandType.StoredProcedure)
                If db.Reader IsNot Nothing AndAlso db.Reader.Read Then
                    retorno = db.Reader("tipoInsRegion")
                End If
            Finally
                If Not db.Reader.IsClosed Then db.Reader.Close()
                db.Dispose()                
            End Try
            Return retorno
        End Function

#End Region

    End Class
End Namespace
