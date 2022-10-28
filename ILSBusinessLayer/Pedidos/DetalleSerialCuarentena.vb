Imports ILSBusinessLayer.Estructuras
Imports LMDataAccessLayer

Namespace Pedidos

    Public Class DetalleSerialCuarentena

#Region "Campos"

        Private _idDetalleSerial As Long
        Private _serial As String
        Private _idDetallePedido As Integer
        Private _infoSerialCuarentena As DataTable
        Private _idPedido As Integer

#End Region

#Region "Propiedades"

        Public Property IdDetalleSerial() As Long
            Get
                Return _idDetalleSerial
            End Get
            Set(ByVal value As Long)
                _idDetalleSerial = value
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

        Public Property IdDetallePedido() As Integer
            Get
                Return _idDetallePedido
            End Get
            Set(ByVal value As Integer)
                _idDetallePedido = value
            End Set
        End Property

        Public ReadOnly Property InfoSerialCuarentena() As DataTable
            Get
                If _infoSerialCuarentena Is Nothing Then CargarInformacion()
                Return _infoSerialCuarentena
            End Get
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idPedido As Integer)
            MyBase.New()
            _idPedido = idPedido
            CargarInformacion()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            Dim dbManager As New LMDataAccess
            If _idPedido > 0 Then

                Try
                    With dbManager
                        If _idPedido > 0 Then .SqlParametros.Add("@idPedido", SqlDbType.Int).Value = _idPedido
                        _infoSerialCuarentena = .ejecutarDataTable("ObtenerInfoSerialesCuarentena", CommandType.StoredProcedure)
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If

        End Sub

        Private Sub EstablecerParametros(ByRef db As LMDataAccess)
            With db.SqlParametros
                .Clear()
                If Not String.IsNullOrEmpty(_serial) Then .Add("@serial", SqlDbType.VarChar, 20).Value = _serial
                .Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
            End With
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
            Dim filtro As New FiltroDetalleCuarentena
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroDetalleCuarentena) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDatos As DataTable
            Try
                With dbManager
                    With .SqlParametros
                        If filtro.IdDetalleSerial > 0 Then .Add("@idDetalleSerial", SqlDbType.BigInt).Value = filtro.IdDetalleSerial
                        If filtro.Serial IsNot Nothing AndAlso filtro.Serial.Trim.Length > 0 Then .Add("@Serial", SqlDbType.VarChar, 20).Value = filtro.Serial
                        If filtro.IdDetallePedido > 0 Then .Add("@idDetallePedido", SqlDbType.Int).Value = filtro.IdDetallePedido
                        If filtro.IdPedido > 0 Then .Add("@idPedido", SqlDbType.Int).Value = filtro.IdPedido
                        If filtro.sinOTB <> Enumerados.EstadoBinario.NoEstablecido Then .Add("@sinOTB", SqlDbType.Bit).Value = filtro.sinOTB
                    End With
                    dtDatos = .ejecutarDataTable("ObtenerInfoDetalleCuarentena", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Shared Function ObtenerPorId(ByVal identificador As Integer) As DataTable
            Dim filtro As New FiltroDetalleCuarentena
            Dim dtDatos As New DataTable
            filtro.IdDetalleSerial = identificador
            dtDatos = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerInfoSerialesCuarentena(ByVal filtro As FiltroDetalleCuarentena) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDatos As DataTable
            Try
                With dbManager
                    With .SqlParametros
                        If filtro.Serial IsNot Nothing AndAlso filtro.Serial.Trim.Length > 0 Then .Add("@Serial", SqlDbType.VarChar, 20).Value = filtro.Serial
                        If filtro.IdDetallePedido > 0 Then .Add("@idDetallePedido", SqlDbType.Int).Value = filtro.IdDetallePedido
                        If filtro.IdPedido > 0 Then .Add("@idPedido", SqlDbType.Int).Value = filtro.IdPedido
                    End With
                    dtDatos = .ejecutarDataTable("ObtenerInfoSerialesCuarentena", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

#End Region

    End Class

End Namespace