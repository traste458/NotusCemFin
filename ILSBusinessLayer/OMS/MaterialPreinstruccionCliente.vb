Namespace OMS

    Public Class MaterialPreinstruccionCliente
        Private _material As String
        Private _idPreinstruccion As Integer
        Public Property Material()
            Get
                Return _material
            End Get
            Set(ByVal value)
                _material = value
            End Set
        End Property

        Public Property IdPreinstruccion()
            Get
                Return _idPreinstruccion
            End Get
            Set(ByVal value)
                _idPreinstruccion = value
            End Set
        End Property

        Public Sub Procesar(ByVal listaMateriales As String, Optional ByVal db As LMDataAccessLayer.LMDataAccess = Nothing)
            If db Is Nothing Then db = New LMDataAccessLayer.LMDataAccess
            db.SqlParametros.Clear()
            If listaMateriales <> "" AndAlso _idPreinstruccion > 0 Then
                db.agregarParametroSQL("@materiales", listaMateriales)
                db.agregarParametroSQL("@idPreinstruccion", _idPreinstruccion, SqlDbType.Int)
                db.ejecutarNonQuery("ProcesarMaterialPreinstruccionCliente", CommandType.StoredProcedure)
            Else
                Throw New Exception("Debe proporcionar los datos necesarios para guardar los materiales de la preinstrucción")
            End If

        End Sub

        Public Shared Function Obtener(ByVal idPreinstruccion As Integer) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idPreinstruccion", idPreinstruccion)
            Dim dt As DataTable = db.ejecutarDataTable("ObtenerMaterialPreinstruccionCliente", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Shared Function Obtener(ByVal materiales As String) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@materiales", materiales)
            Dim dt As DataTable = db.ejecutarDataTable("ObtenerMaterialPreinstruccionCliente", CommandType.StoredProcedure)
            Return dt
        End Function
    End Class

End Namespace
