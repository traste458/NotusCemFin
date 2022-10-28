Imports LMDataAccessLayer

Public Class DetalleTraslado
#Region "Variables"
    Private _idDetalle As Long
    Private _idTraslado As Long
    Private _serial As String    
    Private _fecha As DateTime
#End Region

#Region "Propiedades"
    Public ReadOnly Property IdDetalle() As Long
        Get
            Return _idDetalle
        End Get
    End Property

    Public Property IdTraslado() As Long
        Get
            Return _idTraslado
        End Get
        Set(ByVal value As Long)
            _idTraslado = value
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

    Public ReadOnly Property Fecha() As DateTime
        Get
            Return _fecha
        End Get
    End Property

#End Region

#Region "Constructores"

#End Region

#Region "Metodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Add("@idTraslado", SqlDbType.Int).Value = _idTraslado
                .ejecutarReader("ObtenerDetalleTrasladoServicioMensajeria", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    If .Reader.Read Then CargarResultadoConsulta(.Reader)
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

#Region "Metodos Publicos"

    Public Shared Function ValidoSerial(ByVal numeroRadicado As Long, ByVal serial As String, ByVal cantidadAdicionada As Integer, Optional ByRef mensaje As String = "") As Boolean
        Dim retorno As Boolean = True
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Add("@numeroRadicado", SqlDbType.BigInt).Value = numeroRadicado
                .SqlParametros.Add("@serial", SqlDbType.VarChar).Value = serial
                .SqlParametros.Add("@cantidadAdicionada", SqlDbType.Int).Value = cantidadAdicionada
                .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output
                .ejecutarReader("ValidarDetalleTrasladoServicioMensajeria", CommandType.StoredProcedure)
                mensaje = .SqlParametros("@mensaje").Value.ToString()
                If mensaje <> "" Then
                    retorno = False
                End If
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return retorno
    End Function

    Public Shared Function ObtenerInfoSerialBodegaSatelite(ByVal serial As String) As SerialBodegaSatelite
        Dim dbManager As New LMDataAccess
        Dim serialObj As New SerialBodegaSatelite()
        Try
            With dbManager
                .SqlParametros.Add("@serial", SqlDbType.VarChar).Value = serial
                .ejecutarReader("ObtenerInfoSerialBodegaSateliteCEM", CommandType.StoredProcedure)
                If .Reader.Read() Then
                    If .Reader IsNot Nothing Then
                        If .Reader.HasRows Then
                            serialObj.Serial = serial
                            serialObj.Material = .Reader("material").ToString()
                            Integer.TryParse(.Reader("idSubproducto").ToString(), serialObj.IdSubproducto)
                        End If
                        .Reader.Close()
                    End If
                End If
            End With
        Catch ex As Exception
            Throw New Exception("Error al obtener la información del serial. " & ex.Message)
        End Try
        Return serialObj
    End Function

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idDetalle").ToString, _idDetalle)
                Integer.TryParse(reader("idTraslado").ToString, _idTraslado)
                _serial = reader("serial").ToString
                DateTime.TryParse(reader("fecha").ToString(), _fecha)
            End If
        End If

    End Sub

#End Region

#Region "Estructuras"

    Public Structure SerialBodegaSatelite
        Public Serial As String
        Public Material As String
        Public IdSubproducto As Integer
    End Structure

#End Region

End Class
