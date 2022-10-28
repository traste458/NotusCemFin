Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Enumerados
Imports LMDataAccessLayer

Namespace Localizacion

    Public Class Ciudad

#Region "Campos"

        Private _idCiudad As Integer
        Private _nombre As String
        Private _departamento As String
        Private _idPais As Short
        Private _pais As String
        Private _idRegion As Short
        Private _region As String
        Private _estado As Boolean

#End Region

#Region "Propiedades"

        Public Property IdCiudad() As Integer
            Get
                Return _idCiudad
            End Get
            Set(ByVal value As Integer)
                _idCiudad = value
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

        Public Property Departamento() As String
            Get
                Return _departamento
            End Get
            Set(ByVal value As String)
                _departamento = value
            End Set
        End Property

        Public Property IdPais() As Short
            Get
                Return _idPais
            End Get
            Set(ByVal value As Short)
                _idPais = value
            End Set
        End Property

        Public ReadOnly Property Pais() As String
            Get
                Return _pais
            End Get
        End Property

        Public Property IdRegion() As Short
            Get
                Return _idRegion
            End Get
            Set(ByVal value As Short)
                _idRegion = value
            End Set
        End Property


        Public ReadOnly Property Region() As String
            Get
                Return _region
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

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            _idCiudad = identificador
            CargarInformacion()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            If _idCiudad > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                        .ejecutarReader("ObtenerInfoCiudad", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing AndAlso .Reader.Read Then
                            _nombre = .Reader("nombre").ToString
                            _departamento = .Reader("departamento").ToString
                            Short.TryParse(.Reader("idPais").ToString, _idPais)
                            _pais = .Reader("pais").ToString
                            Short.TryParse(.Reader("idRegion").ToString, _idRegion)
                            _region = .Reader("region")
                            _estado = .Reader("estado")
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

        End Function

        Public Function Actualizar() As Short

        End Function

        Public Sub ObtenerCiudad(ByVal idCiudad As Integer)

            Dim db As New LMDataAccess
            db.agregarParametroSQL("@idCiudad", idCiudad)
            Dim dReader As SqlClient.SqlDataReader = db.ejecutarReader("SeleccionarCiudades", CommandType.StoredProcedure)
            If dReader.Read() Then
                Try
                    _idCiudad = dReader("idCiudad").ToString()
                    _idPais = dReader("idPais").ToString()
                    _nombre = dReader("nombre").ToString()
                    _region = dReader("nombreRegion").ToString()
                    _idRegion = dReader("idRegion").ToString()
                Finally
                    dReader.Close()
                    db.cerrarConexion()
                End Try
            End If
        End Sub

#End Region

#Region "Métodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroCiudad
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroCiudad) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            Try
                With filtro
                    If .IdCiudad > 0 Then db.SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = .IdCiudad
                    If .Nombre IsNot Nothing AndAlso .Nombre.Trim.Length > 0 Then _
                        db.SqlParametros.Add("@nombre", SqlDbType.VarChar, 100).Value = .Nombre.ToString
                    If .Departamento IsNot Nothing AndAlso .Departamento > 0 Then _
                        db.SqlParametros.Add("@departamento", SqlDbType.VarChar).Value = .Departamento.ToString
                    If .IdPais > 0 Then db.SqlParametros.Add("@idPais", SqlDbType.SmallInt).Value = .IdPais
                    If .IdRegion > 0 Then db.SqlParametros.Add("@idRegion", SqlDbType.SmallInt).Value = .IdRegion
                    If .Activo > 0 Then db.SqlParametros.Add("@estado", SqlDbType.Int).Value = .Activo
                    dtDatos = db.ejecutarDataTable("ObtenerInfoCiudad", CommandType.StoredProcedure)
                    Return dtDatos
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListadoAntiguo(ByVal estado As Integer, Optional ByVal nombre As String = "", Optional ByVal idCiudad As Integer = 0) As DataTable
            Dim db As New LMDataAccess
            Dim filtro As New FiltroCiudad
            Dim dtDatos As DataTable
            If idCiudad > 0 Then db.SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = idCiudad
            If nombre IsNot Nothing AndAlso nombre.Trim.Length > 0 Then _
                db.SqlParametros.Add("@nombre", SqlDbType.VarChar, 100).Value = nombre.ToString
            If estado > 0 Then db.SqlParametros.Add("@estado", SqlDbType.Int).Value = estado
            dtDatos = db.ejecutarDataTable("ObtenerInfoCiudades", CommandType.StoredProcedure)
            Return dtDatos

        End Function

        Public Shared Function ObtenerCiudadesPorPais(Optional ByVal idpais As Integer = 170)
            Dim db As New LMDataAccess
            db.agregarParametroSQL("@idPais", idpais)
            Return db.ejecutarDataTable("SeleccionarCiudades", CommandType.StoredProcedure)
        End Function
#End Region
    End Class

End Namespace


