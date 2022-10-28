Imports LMDataAccessLayer

Namespace Comunes


    Public Class PermisosPerfil

        Private _idPerfil As Integer
        Private _configuracion As String
        Private _valoresConfigurados As ArrayList

        Public Property IdPerfil() As Integer
            Get
                Return _idPerfil
            End Get
            Set(ByVal value As Integer)
                _idPerfil = value
            End Set
        End Property

        Public Property Configuracion() As String
            Get
                Return _configuracion
            End Get
            Set(ByVal value As String)
                _configuracion = value
            End Set
        End Property


        Public ReadOnly Property ValoresConfigurados() As ArrayList
            Get
                Return _valoresConfigurados
            End Get
        End Property

        Public Sub New(ByVal configuracion As String)
            MyBase.New()
            _idPerfil = 0
            _configuracion = String.Empty
            _valoresConfigurados = New ArrayList
            CargarInformacion(configuracion)
        End Sub

        Private Overloads Sub CargarInformacion(ByVal configuracion As String)
            Dim dm As New LMDataAccess
            With dm
                .SqlParametros.Add("@configKeyName", SqlDbType.VarChar, 50).Value = configuracion
                .ejecutarReader("ObtenerConfiguracion", CommandType.StoredProcedure)
                If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                    If .Reader.Read Then
                        _valoresConfigurados.AddRange(Split(.Reader.Item("configKeyValue"), ","))
                        _configuracion = .Reader.Item("configKeyName")
                    End If
                End If
            End With
        End Sub

        Public Function IdPerfilTieneValor(ByVal idPerfil As Integer) As Boolean
            Dim tieneValor As Boolean = False
            For index As Integer = 0 To _valoresConfigurados.Count - 1
                If idPerfil = _valoresConfigurados.Item(index) Then
                    tieneValor = True
                    Exit For
                End If
            Next
            Return tieneValor
        End Function

    End Class

End Namespace