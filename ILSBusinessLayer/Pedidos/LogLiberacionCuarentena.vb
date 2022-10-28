Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras

Namespace Pedidos

    Public Class LogLiberacionCuarentena

#Region "Campos"

        Private _idLogLiberacionCuarentena As Long
        Private _serial As String
        Private _idPedido As Integer
        Private _idDetallePedido As Integer

#End Region

#Region "Propiedades"

        Public Property IdLogLiberacionCuarentena() As Long
            Get
                Return _idLogLiberacionCuarentena
            End Get
            Set(ByVal value As Long)
                _idLogLiberacionCuarentena = value
            End Set
        End Property

        Public Property Serial() As String
            Get
                Return _serial
            End Get
            Set(ByVal value As String)
                _serial = value
            End Set
        End Property

        Public Property IdPedido() As Integer
            Get
                Return _idPedido
            End Get
            Set(ByVal value As Integer)
                _idPedido = value
            End Set
        End Property

        Public Property IdDetallePedido() As Integer
            Get
                Return _idDetallePedido
            End Get
            Set(ByVal value As Integer)
                _idDetallePedido = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            _idPedido = identificador
            CargarInformacion()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            If _idPedido > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idPedido", SqlDbType.Int).Value = _idPedido
                        .ejecutarReader("ObtenerInfoLiberacionCuarentena", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                Long.TryParse(.Reader("idDetallePedido").ToString(), _idDetallePedido)
                                _serial = .Reader("serial").ToString()
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

        End Function

        Public Function Eliminar() As Short
        End Function

#End Region

#Region "Métodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroLiberacionCuarentena
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroLiberacionCuarentena) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDatos As DataTable
            Try
                With dbManager
                    With .SqlParametros
                        If filtro.IdLogLiberacion > 0 Then .Add("@idLogLiberacion", SqlDbType.BigInt).Value = filtro.IdLogLiberacion
                        If filtro.Serial IsNot Nothing AndAlso filtro.Serial.Trim.Length > 0 Then .Add("@Serial", SqlDbType.VarChar, 20).Value = filtro.Serial
                        If filtro.IdPedido > 0 Then .Add("@idPedido", SqlDbType.Int).Value = filtro.IdPedido
                        If filtro.IdDetallePedido > 0 Then .Add("@idDetallePedido", SqlDbType.Int).Value = filtro.IdDetallePedido
                    End With
                    dtDatos = .ejecutarDataTable("ObtenerInfoLiberacionCuarentena", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

#End Region

    End Class

End Namespace