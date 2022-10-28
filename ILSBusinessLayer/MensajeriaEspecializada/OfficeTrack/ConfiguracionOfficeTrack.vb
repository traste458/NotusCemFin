Imports LMDataAccessLayer
Imports System.Web.Services
Imports System.Web.Services.WebService
Imports ILSBusinessLayer.com.officetrack.latam

Namespace MensajeriaEspecializada.OfficeTrack

    Public Class ConfiguracionOfficeTrack

#Region "Campos"

        Private _IdConfiguracion As Integer
        Private _AplicationName As String
        Private _AplicationID As Integer
        Private _DataBaseConectionString As String

#End Region

#Region "Propiedades"

        Public Property AplicationName() As Integer
            Get
                Return _AplicationName
            End Get
            Set(ByVal value As Integer)
                _AplicationName = value
            End Set
        End Property

        Public Property IdConfiguracion() As String
            Get
                Return _IdConfiguracion
            End Get
            Set(ByVal value As String)
                _IdConfiguracion = value
            End Set
        End Property

        Public Property AplicationID() As Integer
            Get
                Return _AplicationID
            End Get
            Set(ByVal value As Integer)
                _AplicationID = value
            End Set
        End Property

        Public Property DataBaseConectionString() As String
            Get
                Return _DataBaseConectionString
            End Get
            Set(ByVal value As String)
                _DataBaseConectionString = value
            End Set
        End Property


#End Region

#Region "MetodosPublicos"

        Public Function CargarIdDetalleRutaRetrasmicion(ByVal idRuta As String, ByVal dbManager As LMDataAccess) As DataTable

            Dim sqlRead As SqlClient.SqlDataReader = Nothing
            Dim Detalle As DataTable = New DataTable()
            Try

                dbManager.AgregarParametroSQL("@idRuta", idRuta)
                sqlRead = dbManager.ejecutarReader("ConsultaDetalleRutaOfficeTrackRetransmicion", CommandType.StoredProcedure)
                Detalle.Load(sqlRead)

            Catch ex As Exception
                Throw ex
            Finally
                If sqlRead IsNot Nothing Then
                    sqlRead.Dispose()
                End If
            End Try

            Return Detalle
        End Function


        Public Sub CargarInformacion()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idDataBase", SqlDbType.Int).Value = _AplicationID
                    .ejecutarReader("ConsultaConfiguracionOfficetrack", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("ConfigurationID").ToString(), _IdConfiguracion)
                            _AplicationName = .Reader("AplicationName").ToString()
                            Integer.TryParse(.Reader("AplicationID").ToString(), _AplicationID)
                            _DataBaseConectionString = .Reader("DataBaseConectionString").ToString()
                        End If
                        .Reader.Close()
                    End If
                End With
            Catch ex As Exception
                Throw ex
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

        End Sub

        Public Sub CargarConfigOfficeTrack(ByVal _idDetalle As Integer, ByVal _UserName As String, ByVal _Password As String, _AplicationID As String, dbManager As LMDataAccess)


            Dim sqlRead As SqlClient.SqlDataReader = Nothing
            Dim Datos As String
            Try


                dbManager.AgregarParametroSQL("@IdDetalle", _idDetalle)
                dbManager.AgregarParametroSQL("@UserName", _UserName)
                dbManager.AgregarParametroSQL("@Password", _Password)
                dbManager.AgregarParametroSQL("@AplicationID", _AplicationID)
                dbManager.AgregarParametroSQL("@TaskTypeCode", 1)


                sqlRead = dbManager.ejecutarReader("ConsultaDatosOfficetrack", CommandType.StoredProcedure)
                If sqlRead.Read Then
                    Datos = sqlRead.Item(0)
                    Dim errorMessage As String = String.Empty
                    If Not NotificarAOfficeTrack(Datos, errorMessage) Then

                        Throw New Exception("No se logró notificar a Office Track de la creacion del servicio. " + errorMessage)
                    End If

                Else
                    Throw New Exception("No se encontró información de la tarea en la BD.")
                End If

            Catch ex As Exception
                Throw ex
            Finally
                If sqlRead IsNot Nothing Then
                    sqlRead.Dispose()
                End If
            End Try

        End Sub


        Public Sub CargarConfigOfficeTrack(ByVal _idDetalle As Integer, ByVal _UserName As String, ByVal _Password As String, _AplicationID As Short)
            Dim sqlRead As SqlClient.SqlDataReader = Nothing
            Dim Datos As String
            Using dbManager As New LMDataAccess
                Try

                    dbManager.AgregarParametroSQL("@IdDetalle", _idDetalle)
                    dbManager.AgregarParametroSQL("@UserName", _UserName)
                    dbManager.AgregarParametroSQL("@Password", _Password)
                    dbManager.AgregarParametroSQL("@AplicationID", _AplicationID)
                    dbManager.AgregarParametroSQL("@TaskTypeCode", 1)
                    dbManager.IniciarTransaccion()

                    sqlRead = dbManager.ejecutarReader("ConsultaDatosOfficetrack", CommandType.StoredProcedure)
                    If sqlRead.Read Then
                        Datos = sqlRead.Item(0)
                        Dim errorMessage As String = String.Empty
                        If Not NotificarAOfficeTrack(Datos, errorMessage) Then
                            Throw New Exception("No se logró notificar a Office Track de la creacion del servicio. " + errorMessage)
                        End If
                    Else
                        Throw New Exception("No se encontró información de la tarea en la BD.")
                    End If
                    dbManager.ConfirmarTransaccion()
                Catch ex As Exception
                    dbManager.AbortarTransaccion()
                    Throw ex
                Finally
                    If sqlRead IsNot Nothing Then
                        sqlRead.Dispose()
                    End If
                End Try
            End Using

        End Sub


        Public Function NotificarAOfficeTrack(ByVal xml As String, ByRef errorMessage As String) As Boolean

            Dim manager As New com.logytechmobile.apps.WsOfficeTrack
            'TaskManagement
            'Dim result As New CreateTaskReturnValues
            Dim result As String = String.Empty

            result = manager.NotificacionOfficeTrackSalida(xml)
            If result = "OK" Then
                Return True
            Else
                errorMessage = result
                Return False
            End If


        End Function

        Public Function CargarIdDetalleRuta(ByVal idRuta As String, ByVal dbManager As LMDataAccess) As DataTable

            Dim sqlRead As SqlClient.SqlDataReader = Nothing
            Dim Detalle As DataTable = New DataTable()
            Try

                dbManager.AgregarParametroSQL("@idRuta", idRuta)
                sqlRead = dbManager.ejecutarReader("ConsultaDetalleRutaOfficeTrack", CommandType.StoredProcedure)
                Detalle.Load(sqlRead)

            Catch ex As Exception
                Throw ex
            Finally
                If sqlRead IsNot Nothing Then
                    sqlRead.Dispose()
                End If
            End Try

            Return Detalle
        End Function


        Public Function CargarIdDetalleRuta(ByVal idRuta As String) As DataTable
            Dim dbManager As New LMDataAccess
            Dim sqlRead As SqlClient.SqlDataReader = Nothing
            Dim Detalle As DataTable = New DataTable()
            Try
                With dbManager
                    .AgregarParametroSQL("@idRuta", idRuta)
                    sqlRead = .ejecutarReader("ConsultaDetalleRutaOfficeTrack", CommandType.StoredProcedure)
                    Detalle.Load(sqlRead)

                End With
            Catch ex As Exception

            End Try

            Return Detalle
        End Function




#End Region




    End Class

End Namespace
