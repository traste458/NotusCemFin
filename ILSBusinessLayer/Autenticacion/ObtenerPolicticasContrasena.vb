Imports LMDataAccessLayer

Public Class ObtenerPolicticasContrasena
#Region "Atributos"
    Private _numMayusculas As Integer
    Private _numMinusculas As Integer
    Private _numNumeros As Integer
    Private _numEspeciales As Integer
    Private _minLongitud As Integer
    Private _validacionUltimasContrasena As Integer

#End Region

#Region "Propiedades"
    Public Property NumMayusculas() As Integer
        Get
            Return _numMayusculas
        End Get
        Set(ByVal value As Integer)
            _numMayusculas = value
        End Set
    End Property

    Public Property ValidacionUltimasContrasena() As Integer
        Get
            Return _validacionUltimasContrasena
        End Get
        Set(ByVal value As Integer)
            _validacionUltimasContrasena = value
        End Set
    End Property

    Public Property NumMinusculas() As Integer
        Get
            Return _numMinusculas
        End Get
        Set(ByVal value As Integer)
            _numMinusculas = value
        End Set
    End Property

    Public Property NumNumeros() As Integer
        Get
            Return _numNumeros
        End Get
        Set(ByVal value As Integer)
            _numNumeros = value
        End Set
    End Property

    Public Property NumEspeciales() As Integer
        Get
            Return _numEspeciales
        End Get
        Set(ByVal value As Integer)
            _numEspeciales = value
        End Set
    End Property

    Public Property MinLongitud() As Integer
        Get
            Return _minLongitud
        End Get
        Set(ByVal value As Integer)
            _minLongitud = value
        End Set
    End Property
#End Region

#Region "Metodos Publicos"
    Public Function ObtenerRestriccionesContrasena() As DataTable
        Dim dbManager As New LMDataAccess
        Dim dtDatos As New DataTable
        Try
            dtDatos = dbManager.EjecutarDataTable("SP_ObtenerPoliticasContrasena", CommandType.StoredProcedure)
            Return dtDatos
        Catch ex As Exception

        End Try
        Return dtDatos
    End Function

    Public Function ValidarCantrasenaUltimosIngresos(idUsuario As Integer, pwd As String) As Byte
        Dim dbManager As New LMDataAccess
        Dim resultado As Byte = 1
        Try
            With dbManager
                .SqlParametros.Add("@idUsuario", SqlDbType.Int, 10).Value = idUsuario
                .SqlParametros.Add("@pwd", SqlDbType.VarChar, 50).Value = pwd
                .SqlParametros.Add("@resultado", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                .EjecutarNonQuery("SP_ValidarContrasenaNoIgualAnteriores", CommandType.StoredProcedure)
                If Not IsDBNull(.SqlParametros("@resultado").Value) Then
                    resultado = CByte(.SqlParametros("@resultado").Value.ToString)
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return resultado
    End Function

#End Region

End Class
