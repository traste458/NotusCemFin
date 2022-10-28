Imports LMDataAccessLayer
Imports ILSBusinessLayer.MensajeriaEspecializada

Public Class GerenciaCliente

#Region "Atributos"

    Private _idGerencia As Integer
    Private _nombre As String
    Private _activo As Boolean
    Private _idTerceroGerente As Integer
    Private _listCoordinadores As List(Of Integer)

    Private _registrado As Boolean

#End Region

#Region "Propiedades"

    Public Property IdGerencia As Integer
        Get
            Return _idGerencia
        End Get
        Set(value As Integer)
            _idGerencia = value
        End Set
    End Property

    Public Property Nombre As String
        Get
            Return _nombre
        End Get
        Set(value As String)
            _nombre = value
        End Set
    End Property

    Public Property Activo As Boolean
        Get
            Return _activo
        End Get
        Set(value As Boolean)
            _activo = value
        End Set
    End Property

    Public Property IdTerceroGerente As Integer
        Get
            Return _idTerceroGerente
        End Get
        Set(value As Integer)
            _idTerceroGerente = value
        End Set
    End Property

    Public Property ListaCoordinadores As List(Of Integer)
        Get
            Return _listCoordinadores
        End Get
        Set(value As List(Of Integer))
            _listCoordinadores = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idGerencia As Integer)
        _idGerencia = idGerencia
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idGerencia > 0 Then .SqlParametros.Add("@idGerencia", SqlDbType.Int).Value = CInt(_idGerencia)
                    .ejecutarReader("ObtenerGerenciaCliente", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            CargarResultadoConsulta(.Reader)
                            _registrado = True
                        End If
                        .Reader.Close()
                    End If
                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Using
    End Sub

    Private Function GeneraDataCoordinador(ByVal esRegistro As Boolean) As DataTable
        Dim dtDatos As New DataTable
        Try
            With dtDatos
                .Columns.Add(New DataColumn("IdGerencia", GetType(Integer)))
                .Columns.Add(New DataColumn("IdTercero", GetType(Integer)))
                .Columns.Add(New DataColumn("IdTerceroPadre", GetType(Integer)))
            End With

            If esRegistro Then
                'Se adiciona el Gerente
                Dim drGerente As DataRow = dtDatos.NewRow()
                drGerente("IdGerencia") = IdGerencia
                drGerente("IdTercero") = _idTerceroGerente
                dtDatos.Rows.Add(drGerente)
            End If
            

            For Each idTercero As Integer In _listCoordinadores
                Dim dr As DataRow = dtDatos.NewRow()
                dr("IdGerencia") = IdGerencia
                dr("IdTercero") = idTercero
                dr("IdTerceroPadre") = _idTerceroGerente
                dtDatos.Rows.Add(dr)
            Next

            dtDatos.AcceptChanges()
        Catch ex As Exception
            Throw ex
        End Try
        Return dtDatos
    End Function

#End Region

#Region "Métodos Públicos"

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idGerencia", SqlDbType.Int).Direction = ParameterDirection.Output
                    .SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = _nombre
                    .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo
                    .SqlParametros.Add("@return", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                    .iniciarTransaccion()
                    .ejecutarNonQuery("RegistrarGerenciaCliente", CommandType.StoredProcedure)

                    resultado.Valor = .SqlParametros("@return").Value
                    If resultado.Valor = 0 Then
                        _idGerencia = .SqlParametros("@idGerencia").Value

                        If _listCoordinadores.Count > 0 Then
                            .inicilizarBulkCopy()
                            With .BulkCopy
                                .ColumnMappings.Add("IdGerencia", "idGerencia")
                                .ColumnMappings.Add("IdTercero", "idPersona")
                                .ColumnMappings.Add("IdTerceroPadre", "idPersonaPadre")
                                .DestinationTableName = "PersonalEnGerencia"
                                .WriteToServer(GeneraDataCoordinador(True))
                            End With
                        End If

                        .confirmarTransaccion()
                        resultado.Mensaje = "Se registro satisfactoriamente la Gerencia."
                    Else
                        .abortarTransaccion()
                        resultado.Mensaje = "Se generó un error inesperado al registrar la Gerencia [" & resultado.Valor & "]"
                    End If
                End With
            Catch ex As Exception
                If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw ex
            End Try
        End Using
        Return resultado
    End Function

    Public Function Actualizar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idGerencia", SqlDbType.Int).Value = _idGerencia
                    .SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = _nombre
                    .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo
                    .SqlParametros.Add("@return", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                    .iniciarTransaccion()
                    .ejecutarNonQuery("ActualizarGerenciaCliente", CommandType.StoredProcedure)

                    resultado.Valor = .SqlParametros("@return").Value
                    If resultado.Valor = 0 Then
                        _idGerencia = .SqlParametros("@idGerencia").Value

                        If _listCoordinadores.Count > 0 Then
                            .inicilizarBulkCopy()
                            With .BulkCopy
                                .ColumnMappings.Add("IdGerencia", "idGerencia")
                                .ColumnMappings.Add("IdTercero", "idPersona")
                                .ColumnMappings.Add("IdTerceroPadre", "idPersonaPadre")
                                .DestinationTableName = "PersonalEnGerencia"
                                .WriteToServer(GeneraDataCoordinador(False))
                            End With
                        End If

                        .confirmarTransaccion()
                        resultado.Mensaje = "Se actualizó satisfactoriamente la Gerencia."
                    Else
                        .abortarTransaccion()
                        resultado.Mensaje = "Se generó un error inesperado al actualizar la Gerencia [" & resultado.Valor & "]"
                    End If
                End With
            Catch ex As Exception
                If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw ex
            End Try
        End Using
        Return resultado
    End Function

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idGerencia"), _idGerencia)
                _nombre = reader("nombre")
                If Not String.IsNullOrEmpty(reader("IdTerceroGerente").ToString) Then
                    Integer.TryParse(reader("IdTerceroGerente"), _idTerceroGerente)
                End If
                _activo = CBool(reader("activo"))
            End If
        End If
    End Sub

#End Region

End Class
