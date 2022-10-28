Imports ILSBusinessLayer
Imports LMDataAccessLayer

Public Class DetalleImeiPrestamoServicioMensajeria
    Inherits DetalleSerialServicioMensajeria

#Region "Atributos"

    Private _numeroRadicado As Long
    Private _serialPrestamo As String

#End Region

#Region "Constructores"

    Public Sub New(ByVal numeroRadicado As Long, ByVal serialReparacion As String, ByVal serialPrestamo As String)
        MyBase.New()
        _numeroRadicado = numeroRadicado
        _serial = serialReparacion
        _serialPrestamo = serialPrestamo
    End Sub

#End Region

#Region "Propiedades"

    Public Property NumeroRdicado() As Long
        Get
            Return _numeroRadicado
        End Get
        Set(ByVal value As Long)
            _numeroRadicado = value
        End Set
    End Property

#End Region

#Region "Métodos Publicos"

    Public Function AsignarSerialPrestamo(ByVal idUsuario As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@numRadicado", SqlDbType.BigInt).Value = _numeroRadicado
                    .SqlParametros.Add("@serialReparacion", SqlDbType.VarChar).Value = _serial
                    .SqlParametros.Add("@serialPrestamo", SqlDbType.VarChar).Value = _serialPrestamo
                    .SqlParametros.Add("@idUsuario", SqlDbType.BigInt).Value = idUsuario

                    .iniciarTransaccion()
                    .ejecutarReader("AsignarSerialPrestamo", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                        If .Reader.Read() Then
                            resultado.Valor = CInt(.Reader.Item("valor"))
                            resultado.Mensaje = CStr(.Reader.Item("mensaje"))

                            .Reader.Close()
                            If resultado.Valor = 0 Then
                                .confirmarTransaccion()
                            Else
                                .abortarTransaccion()
                            End If
                        End If
                    Else
                        Throw New Exception("Imposible evaluar la respuesta del servidor.")
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw ex
            End Try
        End Using
        Return resultado
    End Function

#End Region

End Class
