Imports ILSBusinessLayer.Estructuras
Imports LMDataAccessLayer

Namespace OMS

    Public Class SerialMuestra

#Region "Atributos"

        Private _idSerialMuestra As Long
        Private _idOrden As Long
        Private _serial As String
        Private _idCreador As Long
        Private _fechaMuestra As Date
        Private _codigoOrden As String

#End Region

#Region "Constructores"

        Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            _idSerialMuestra = identificador
            CargarInformacion()
        End Sub

#End Region

#Region "Propiedades"

        Public ReadOnly Property IdSerialMuestra() As Long
            Get
                Return _idSerialMuestra
            End Get
        End Property

        Public Property IdOrden() As Long
            Get
                Return _idOrden
            End Get
            Set(ByVal value As Long)
                _idOrden = value
            End Set
        End Property

        Public Property IdCreador() As Long
            Get
                Return _idCreador
            End Get
            Set(ByVal value As Long)
                _idCreador = value
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

        Public Property CodigoOrden() As String
            Get
                Return _codigoOrden
            End Get
            Set(ByVal value As String)
                _codigoOrden = value
            End Set
        End Property

        Public Property FechaMuestra() As Date
            Get
                Return _fechaMuestra
            End Get
            Set(ByVal value As Date)
                _fechaMuestra = value
            End Set
        End Property

#End Region

#Region "Metodos Privados"

        Private Sub CargarInformacion()
            Dim db As New LMDataAccess
            With db
                With .SqlParametros
                    .Add("@idSerialMuestra", SqlDbType.BigInt).Value = _idSerialMuestra
                End With

                Try
                    .ejecutarReader("ObtenerSerialesMuestreo", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            _serial = .Reader("serial").ToString()
                            _codigoOrden = .Reader("orden").ToString()
                            _fechaMuestra = .Reader("hora").ToString()
                        End If
                        If Not .Reader.IsClosed Then .Reader.Close()
                    End If
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
        End Sub

#End Region

#Region "Metodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroSerialMuestra
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroSerialMuestra) As DataTable
            Dim dtDatos As New DataTable
            Dim db As New LMDataAccess
            With db
                With .SqlParametros
                    If filtro.IdSerialMuestra <> 0 Then .Add("@idSerialMuestra", SqlDbType.BigInt).Value = filtro.IdSerialMuestra
                    If filtro.IdFactura <> 0 Then .Add("@idFactura", SqlDbType.BigInt).Value = filtro.IdFactura
                    If filtro.IdGuia <> 0 Then .Add("@idGuia", SqlDbType.BigInt).Value = filtro.IdGuia
                End With
                Try
                    dtDatos = .ejecutarDataTable("ObtenerSerialesMuestreo", CommandType.StoredProcedure)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
            Return dtDatos
        End Function

        Public Shared Function ObtenerPorId(ByVal identificador As Long) As DataTable
            Dim filtro As New FiltroSerialMuestra
            filtro.IdSerialMuestra = identificador
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Shared Function ValidarExistenSerialesMuestreo(ByVal filtro As FiltroSerialMuestra) As Short
            Dim db As New LMDataAccess
            Dim result As Short
            With db
                If filtro.IdSerialMuestra <> 0 Then .SqlParametros.Add("@idSerialMuestra", SqlDbType.BigInt).Value = filtro.IdSerialMuestra
                If filtro.IdFactura <> 0 Then .SqlParametros.Add("@idFactura", SqlDbType.BigInt).Value = filtro.IdFactura
                If filtro.IdGuia <> 0 Then .SqlParametros.Add("@idGuia", SqlDbType.BigInt).Value = filtro.IdGuia
                .SqlParametros.Add("@result", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue

                Try
                    .ejecutarNonQuery("ValidarExistenSerialesMuestreo", CommandType.StoredProcedure)
                    Short.TryParse(.SqlParametros("@result").Value, result)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With

            Return result
        End Function

#End Region

    End Class

End Namespace