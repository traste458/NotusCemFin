Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Enumerados
Imports LMDataAccessLayer

Namespace Productos

    Public Class UnidadEmpaque

#Region "Campos"

        Private _idTipoUnidad As Short
        Private _descripcion As String
        Private _estado As Boolean

#End Region

#Region "Propiedades"

        Public Property IdTipoUnidad() As Short
            Get
                Return _idTipoUnidad
            End Get
            Set(ByVal value As Short)
                _idTipoUnidad = value
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
            _idTipoUnidad = identificador
            CargarInformacion()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            If _idTipoUnidad <> 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idTipoUnidad", SqlDbType.Int).Value = _idTipoUnidad
                        .ejecutarReader("ObtenerInfoUnidadEmpaque", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                _descripcion = .Reader("descripcion").ToString
                                _estado = CBool(.Reader("estado"))
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
            If _descripcion.Trim.Length > 0 Then
                Dim dbManager As New LMDataAccess

                Try
                    With dbManager
                        .SqlParametros.Add("@descripcion", SqlDbType.VarChar, 50).Value = _descripcion
                        .SqlParametros.Add("@idTipoUnidad", SqlDbType.SmallInt).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("CrearTipoUnidadEmpaque", CommandType.StoredProcedure)
                        resultado = CShort(.SqlParametros("@returnValue").Value)
                        If resultado = 0 Then _idTipoUnidad = CShort(.SqlParametros("@idTipoUnidad").Value)
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
            Dim resultado As Short
            If _idTipoUnidad <> 0 And _descripcion.Trim.Length > 0 Then
                Dim dbManager As New LMDataAccess

                Try
                    With dbManager
                        .SqlParametros.Add("@idTipoUnidad", SqlDbType.SmallInt).Value = _idTipoUnidad
                        .SqlParametros.Add("@descripcion", SqlDbType.VarChar, 50).Value = _descripcion
                        .SqlParametros.Add("@estado", SqlDbType.Bit).Value = _estado
                        .SqlParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("ActualizarTipoUnidadEmpaque", CommandType.StoredProcedure)
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
            Dim filtro As New FiltroUnidadEmpaque
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroUnidadEmpaque) As DataTable
            Dim dtDatos As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        If filtro.IdTipoUnidad > 0 Then .Add("@idTipoUnidad", SqlDbType.SmallInt).Value = filtro.IdTipoUnidad
                        If filtro.Activo > 0 Then .Add("@estado", SqlDbType.Int).Value = IIf(filtro.Activo = 1, 1, 0)
                        If filtro.Descripcion IsNot Nothing AndAlso filtro.Descripcion.Trim.Length > 0 Then _
                            .Add("@descripcion", SqlDbType.VarChar, 30).Value = filtro.Descripcion
                        If filtro.Material IsNot Nothing AndAlso filtro.Material.Trim.Length > 0 Then _
                            .Add("@material", SqlDbType.VarChar, 10).Value = filtro.Material
                    End With
                    
                    dtDatos = .ejecutarDataTable("ObtenerInfoUnidadEmpaque", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Shared Function ObtenerPorId(ByVal identificador As Integer) As DataTable
            Dim filtro As New FiltroUnidadEmpaque
            Dim dtDatos As New DataTable
            filtro.IdTipoUnidad = identificador
            dtDatos = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Shared Function ObtenerPorMaterial(ByVal material As String) As DataTable
            Dim filtro As New FiltroUnidadEmpaque
            Dim dtDatos As New DataTable
            filtro.Material = material
            dtDatos = ObtenerListado(filtro)
            Return dtDatos
        End Function

#End Region

    End Class

End Namespace


