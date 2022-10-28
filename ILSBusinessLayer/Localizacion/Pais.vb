Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Enumerados
Imports LMDataAccessLayer

Namespace Localizacion

    Public Class Pais

#Region "Campos"

        Private _idPais As Short
        Private _nombre As String
        Private _codigoAlpha As String

#End Region

#Region "Propiedades"

        Public Property IdPais() As Short
            Get
                Return _idPais
            End Get
            Set(ByVal value As Short)
                _idPais = value
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

        Public Property CodigoAlpha() As Boolean
            Get
                Return _codigoAlpha
            End Get
            Set(ByVal value As Boolean)
                _codigoAlpha = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            _idPais = identificador
            CargarInformacion()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            If _idPais > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idPais", SqlDbType.Int).Value = _idPais
                        .ejecutarReader("ObtenerInfoPais", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                _nombre = .Reader("nombre").ToString
                                _codigoAlpha = .Reader("alpha").ToString
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
            Dim resultado As Short
            If _idPais > 0 AndAlso _nombre.Trim.Length > 0 AndAlso _codigoAlpha.Trim.Length > 0 Then
                Dim dbManager As New LMDataAccess

                Try
                    With dbManager
                        .SqlParametros.Add("@idPais", SqlDbType.SmallInt).Value = _idPais
                        .SqlParametros.Add("@nombre", SqlDbType.VarChar, 70).Value = _nombre
                        .SqlParametros.Add("@codigoAlpha", SqlDbType.VarChar, 5).Value = _codigoAlpha
                        .SqlParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("CrearPais", CommandType.StoredProcedure)
                        resultado = CShort(.SqlParametros("@returnValue").Value)
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 5
            End If
            Return resultado
        End Function

        Public Function Actualizar() As Short
            Dim resultado As Short
            If _idPais > 0 AndAlso _nombre.Trim.Length > 0 AndAlso _codigoAlpha.Trim.Length > 0 Then
                Dim dbManager As New LMDataAccess

                Try
                    With dbManager
                        .SqlParametros.Add("@idPais", SqlDbType.SmallInt).Value = _idPais
                        .SqlParametros.Add("@nombre", SqlDbType.VarChar, 70).Value = _nombre
                        .SqlParametros.Add("@codigoAlpha", SqlDbType.VarChar, 5).Value = _codigoAlpha
                        .SqlParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("ActualizarPais", CommandType.StoredProcedure)
                        resultado = CShort(.SqlParametros("@returnValue").Value)
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 4
            End If
            Return resultado
        End Function

#End Region

#Region "Métodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroPais
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroPais) As DataTable
            Dim dtDatos As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        If filtro.IdPais > 0 Then .Add("@idPais", SqlDbType.SmallInt).Value = filtro.IdPais
                        If filtro.Nombre IsNot Nothing AndAlso filtro.Nombre.Trim.Length > 0 Then _
                            .Add("@nombre", SqlDbType.VarChar, 70).Value = filtro.Nombre
                        If filtro.CodigoAlpha IsNot Nothing AndAlso filtro.CodigoAlpha.Trim.Length > 0 Then _
                            .Add("@codigoAlpha", SqlDbType.VarChar, 5).Value = filtro.CodigoAlpha
                    End With

                    dtDatos = .ejecutarDataTable("ObtenerInfoPais", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Shared Function ObtenerPorId(ByVal identificador As Integer) As DataTable
            Dim filtro As New FiltroPais
            Dim dtDatos As New DataTable
            filtro.IdPais = identificador
            dtDatos = ObtenerListado(filtro)
            Return dtDatos
        End Function

#End Region

    End Class

End Namespace


