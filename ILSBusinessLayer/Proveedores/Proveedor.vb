Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Enumerados
Imports LMDataAccessLayer

Public Class Proveedor

#Region "Campos"

    Private _idProveedor As Integer
    Private _nombre As String
    Private _direccion As String
    Private _telefono As String
    Private _idCiudad As Integer
    Private _ciudad As String
    Private _estado As Boolean
    Private _idCreador As Integer
    Private _creador As String
    Private _fechaCreacion As Date

#End Region

#Region "Propiedades"

    Public Property IdProveedor() As Integer
        Get
            Return _idProveedor
        End Get
        Set(ByVal value As Integer)
            _idProveedor = value
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

    Public Property Direccion() As String
        Get
            Return _direccion
        End Get
        Set(ByVal value As String)
            _direccion = value
        End Set
    End Property

    Public Property Telefono() As String
        Get
            Return _telefono
        End Get
        Set(ByVal value As String)
            _telefono = value
        End Set
    End Property

    Public Property IdCiudad() As Integer
        Get
            Return _idCiudad
        End Get
        Set(ByVal value As Integer)
            _idCiudad = value
        End Set
    End Property

    Public ReadOnly Property Ciudad() As String
        Get
            Return _ciudad
        End Get
    End Property

    Public Property Activo() As Boolean
        Get
            Return _estado
        End Get
        Set(ByVal value As Boolean)
            _estado = value
        End Set
    End Property

    Public Property IdCreador() As Integer
        Get
            Return _idCreador
        End Get
        Set(ByVal value As Integer)
            _idCreador = value
        End Set
    End Property

    Public ReadOnly Property Creador() As String
        Get
            Return _creador
        End Get
    End Property

    Public ReadOnly Property FechaCreacion() As Date
        Get
            Return _fechaCreacion
        End Get
    End Property
#End Region

#Region "Contructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal identificador As Integer)
        MyBase.New()
        _idProveedor = identificador
        CargarInformacion()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarInformacion()
        If _idProveedor <> 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idProveedor", SqlDbType.Int).Value = _idProveedor
                    .ejecutarReader("ObtenerInfoProveedor", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            _nombre = .Reader("nombre").ToString
                            _direccion = .Reader("direccion").ToString
                            _telefono = .Reader("telefono").ToString
                            Integer.TryParse(.Reader("idCiudad").ToString, _idCiudad)
                            _ciudad = .Reader("ciudad").ToString
                            _estado = CBool(.Reader("estado"))
                            Integer.TryParse(.Reader("idCreador").ToString, _idCreador)
                            _creador = .Reader("creador").ToString
                            Date.TryParse(.Reader("fechaCreacion").ToString, _fechaCreacion)
                        End If
                        .Reader.Close()
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End If
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function Registrar() As Short
        Dim resultado As Short = 0
        If _nombre.Trim.Length > 0 And _idCreador <> 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@nombre", SqlDbType.VarChar, 100).Value = _nombre.Trim.ToUpper
                        .Add("@direccion", SqlDbType.VarChar, 70).IsNullable = True
                        .Item("@direccion").Value = IIf(_direccion.Trim.Length > 0, _direccion.Trim, DBNull.Value)
                        .Add("@telefono", SqlDbType.VarChar, 50).IsNullable = True
                        .Item("@telefono").Value = IIf(_telefono.Trim.Length > 0, _telefono.Trim, DBNull.Value)
                        .Add("@idCiudad", SqlDbType.Int).IsNullable = True
                        .Item("@idCiudad").Value = IIf(_idCiudad <> 0, _idCiudad, DBNull.Value)
                        .Add("@idCreador", SqlDbType.Int).Value = _idCreador
                        .Add("@idProveedor", SqlDbType.Int).Direction = ParameterDirection.Output
                        .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .ejecutarNonQuery("CrearProveedor", CommandType.StoredProcedure)
                    resultado = CShort(.SqlParametros("@returnValue").Value)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        Else
            resultado = 3
        End If
        Return resultado
    End Function

    Public Function Actualizar() As Short
        Dim resultado As Short = 0
        If _idProveedor <> 0 And _nombre.Trim.Length > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idProveedor", SqlDbType.Int).Value = _idProveedor
                        .Add("@nombre", SqlDbType.VarChar, 100).Value = _nombre.Trim.ToUpper
                        .Add("@direccion", SqlDbType.VarChar, 70).IsNullable = True
                        .Item("@direccion").Value = IIf(_direccion.Trim.Length > 0, _direccion.Trim, DBNull.Value)
                        .Add("@telefono", SqlDbType.VarChar, 50).IsNullable = True
                        .Item("@telefono").Value = IIf(_telefono.Trim.Length > 0, _telefono.Trim, DBNull.Value)
                        .Add("@idCiudad", SqlDbType.Int).IsNullable = True
                        .Item("@idCiudad").Value = IIf(_idCiudad <> 0, _idCiudad, DBNull.Value)
                        .Add("@estado", SqlDbType.Bit).Value = _estado
                        .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .ejecutarNonQuery("ActualizarProveedor", CommandType.StoredProcedure)
                    resultado = CShort(.SqlParametros("@returnValue").Value)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        Else
            resultado = 3
        End If
        Return resultado
    End Function

#End Region

#Region "Métodos Compartidos"

    Public Overloads Shared Function ObtenerListado() As DataTable
        Dim filtro As New FiltroGeneral
        Dim dtDatos As DataTable = ObtenerListado(filtro)
        Return dtDatos
    End Function

    Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroGeneral, Optional ByVal idTipoProducto As Integer = 0) As DataTable
        Dim dbManager As New LMDataAccess
        Dim dtDatos As New DataTable
        Try
            With dbManager
                With .SqlParametros
                    If filtro.Identificador <> 0 Then .Add("@idProveedor", SqlDbType.Int).Value = filtro.Identificador
                    If filtro.Nombre IsNot Nothing AndAlso filtro.Nombre.Trim.Length > 0 Then .Add("@nombre", SqlDbType.VarChar, 100).Value = filtro.Nombre
                    If filtro.IdCiudad <> 0 Then .Add("@idCiudad", SqlDbType.Int).Value = filtro.IdCiudad
                    If filtro.Activo > 0 Then .Add("@estado", SqlDbType.Bit).Value = IIf(filtro.Activo = 1, 1, 0)
                    If idTipoProducto > 0 Then .Add("@idTipoProducto", SqlDbType.SmallInt).Value = idTipoProducto
                End With
                dtDatos = .ejecutarDataTable("ObtenerInfoProveedor", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

    Public Shared Function ObtenerPorId(ByVal identificador As Integer) As DataTable
        Dim filtro As New FiltroGeneral
        Dim dtDatos As New DataTable
        filtro.Identificador = identificador
        dtDatos = ObtenerListado(filtro)
        Return dtDatos
    End Function

#End Region


End Class



