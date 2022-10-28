Imports LMDataAccessLayer

Public Class DetalleMsisdnEnServicioMensajeriaTipoVentaColeccion
    Inherits DetalleMsisdnEnServicioMensajeriaColeccion

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idServicio As Integer)
        Me.New()
        _idServicioMensajeria = idServicio
        _hayCambioServicio = Enumerados.EstadoBinario.NoEstablecido
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Overloads Property Item(ByVal index As Integer) As DetalleMsisdnEnServicioMensajeriaTipoVenta
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As DetalleMsisdnEnServicioMensajeriaTipoVenta)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

#End Region

#Region "Métodos Públicos"

    Public Overloads Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            Me.Clear()
            With dbManager
                If Me._idServicioMensajeria > 0 Then .SqlParametros.Add("@idServicioMensajeria", SqlDbType.Int).Value = Me._idServicioMensajeria
                If Me._idRegistro > 0 Then .SqlParametros.Add("@idRegistro", SqlDbType.Int).Value = Me._idRegistro
                If Me._idTipoServicio > 0 Then .SqlParametros.Add("@idTipoServicio", SqlDbType.Int).Value = Me._idTipoServicio
                If Me._msisdn > 0 Then .SqlParametros.Add("@msisdn", SqlDbType.BigInt).Value = Me._msisdn
                If Me._hayCambioServicio <> Enumerados.EstadoBinario.NoEstablecido Then _
                    .SqlParametros.Add("@tieneCambioServicio", SqlDbType.Bit).Value = IIf(_hayCambioServicio = Enumerados.EstadoBinario.Activo, 1, 0)
                .ejecutarReader("ObtenerDetalleMsisdnEnServicioMensajeria", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim elDetalle As DetalleMsisdnEnServicioMensajeriaTipoVenta
                    While .Reader.Read
                        elDetalle = New DetalleMsisdnEnServicioMensajeriaTipoVenta
                        elDetalle.CargarResultadoConsulta(.Reader)
                        _cargado = True
                        Me.InnerList.Add(elDetalle)
                    End While
                    .Reader.Close()
                End If
            End With
            _cargado = True
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

End Class
