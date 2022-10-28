Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Enumerados
Imports LMDataAccessLayer

Public Class Tecnologia

#Region "Campos"

    Private _idTecnologia As Integer
    Private _codigo As String
    Private _descripcion As String
    Private _estado As Boolean
#End Region

#Region "Propiedades"

    Public Property IdTecnologia() As Short
        Get
            Return _idTecnologia
        End Get
        Set(ByVal value As Short)
            _idTecnologia = value
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

    Public Property Codigo() As String
        Get
            Return _codigo
        End Get
        Set(ByVal value As String)
            _codigo = value
        End Set
    End Property

    Public Property Activo() As Boolean
        Get
            Return _estado
        End Get
        Set(ByVal value As Boolean)
            _estado = value
        End Set
    End Property
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal identificador As Integer)
        MyBase.New()
        _idTecnologia = identificador
        CargarInformacion()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarInformacion()
        Dim dbManager As New LMDataAccess

        Try
            With dbManager
                .SqlParametros.Add("@idTecnologia", SqlDbType.BigInt).Value = _idTecnologia
                .ejecutarReader("ObtenerInfoTecnologia", CommandType.StoredProcedure)
                If .Reader IsNot Nothing AndAlso .Reader.Read Then
                    _descripcion = .Reader("descripcion").ToString
                    _codigo = .Reader("codigo").ToString
                    _estado = CBool(.Reader("estado"))
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function Registrar() As Short
        ''Dim resultado As Short
        ''If _idTecnologia <> 0 And _descripcion.Trim.Length > 0 And _idTipoUnidad <> 0 Then
        ''    Dim dbManager As New LMDataAccess

        ''    Try
        ''        With dbManager
        ''            .SqlParametros.Add("@idTipoProducto", SqlDbType.SmallInt).Value = _idTecnologia
        ''            .SqlParametros.Add("@descripcion", SqlDbType.VarChar, 50).Value = _descripcion
        ''            .SqlParametros.Add("@instruccionable", SqlDbType.Bit).Value = _instruccionable
        ''            .SqlParametros.Add("@idTipoUnidad", SqlDbType.SmallInt).IsNullable = True
        ''            .SqlParametros("@idTipoUnidad").Value = IIf(_idTipoUnidad <> 0, _idTipoUnidad, DBNull.Value)
        ''            .SqlParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
        ''            .ejecutarNonQuery("CrearTipoProducto", CommandType.StoredProcedure)
        ''            resultado = CShort(.SqlParametros("@returnValue").Value)
        ''            If resultado = 0 Then _idTecnologia = CShort(.SqlParametros("@idTipoProducto").Value)
        ''        End With
        ''    Finally
        ''        If dbManager IsNot Nothing Then dbManager.Dispose()
        ''    End Try
        ''Else
        ''    resultado = 4
        ''End If
        ''Return resultado
    End Function

    Public Function Actualizar() As Short
        ''Dim resultado As Short
        ''If _idTecnologia <> 0 And _descripcion.Trim.Length > 0 And _idTipoUnidad <> 0 Then
        ''    Dim dbManager As New LMDataAccess

        ''    Try
        ''        With dbManager
        ''            .SqlParametros.Add("@idTipoProducto", SqlDbType.SmallInt).Value = _idTecnologia
        ''            .SqlParametros.Add("@descripcion", SqlDbType.VarChar, 50).Value = _descripcion
        ''            .SqlParametros.Add("@estado", SqlDbType.Bit).Value = _estado
        ''            .SqlParametros.Add("@instruccionable", SqlDbType.Bit).Value = _instruccionable
        ''            .SqlParametros.Add("@idTipoUnidad", SqlDbType.SmallInt).IsNullable = True
        ''            .SqlParametros("@idTipoUnidad").Value = IIf(_idTipoUnidad <> 0, _idTipoUnidad, DBNull.Value)
        ''            .SqlParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
        ''            .ejecutarNonQuery("ActualizarTipoProducto", CommandType.StoredProcedure)
        ''            resultado = CShort(.SqlParametros("@returnValue").Value)
        ''        End With
        ''    Finally
        ''        If dbManager IsNot Nothing Then dbManager.Dispose()
        ''    End Try
        ''Else
        ''    resultado = 3
        ''End If
        ''Return resultado
    End Function

#End Region

#Region "Métodos Compartidos"

    Public Overloads Shared Function ObtenerListado() As DataTable
        Dim filtro As New FiltroTecnologia
        Dim dtDatos As DataTable = ObtenerListado(filtro)
        Return dtDatos
    End Function

    Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroTecnologia) As DataTable
        Dim dtDatos As New DataTable
        Dim db As New LMDataAccess
        With filtro
            If .IdTecnologia > 0 Then db.SqlParametros.Add("@idTecnologia", SqlDbType.BigInt).Value = .IdTecnologia
            If .Activo > 0 Then db.SqlParametros.Add("@estado", SqlDbType.Int).Value = IIf(.Activo = 1, 1, 0)
            If .Descripcion IsNot Nothing AndAlso .Descripcion.Trim.Length > 0 Then _
                db.SqlParametros.Add("@descripcion", SqlDbType.VarChar).Value = .Descripcion
            If .Codigo IsNot Nothing AndAlso .Codigo.Trim.Length > 0 Then _
                db.SqlParametros.Add("@codigo", SqlDbType.Int).Value = .Codigo.Trim
            dtDatos = db.ejecutarDataTable("ObtenerInfoTecnologia", CommandType.StoredProcedure)
            Return dtDatos
        End With
        Return dtDatos
    End Function

    Public Shared Function ObtenerPorId(ByVal identificador As Short) As DataTable
        Dim dtDatos As DataTable
        Dim filtro As New FiltroTecnologia
        filtro.IdTecnologia = identificador
        dtDatos = ObtenerListado(filtro)
        Return dtDatos
    End Function

#End Region

End Class
