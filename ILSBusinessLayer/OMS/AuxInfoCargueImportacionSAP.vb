Imports LMDataAccessLayer

Namespace OMS

    Public Class AuxInfoCargueImportacionSAP

#Region "Campos"

        Private _dtInfoCarga As DataTable
        Private _idOrden As Long
        Private _pedido As Long
        Private _idUsuario As Integer
        Private _cargado As Boolean = False

#End Region

#Region "Propiedades"

        Public Property InfoCarga() As DataTable
            Get
                Return _dtInfoCarga
            End Get
            Set(ByVal value As DataTable)
                _dtInfoCarga = value
            End Set
        End Property

        Public Property IdOrden() As Long
            Get
                Return _idOrden
            End Get
            Set(ByVal value As Long)
                _idOrden = value
            End Set
        End Property

        Public Property Pedido() As Long
            Get
                Return _pedido
            End Get
            Set(ByVal value As Long)
                _pedido = value
            End Set
        End Property

        Public Property IdUsuario() As Integer
            Get
                Return _idUsuario
            End Get
            Set(ByVal value As Integer)
                _idUsuario = value
            End Set
        End Property

        Public Property Cargado() As Boolean
            Get
                Return _cargado
            End Get
            Set(ByVal value As Boolean)
                _cargado = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal dtDatosCarga As DataTable)
            MyBase.New()
            _dtInfoCarga = dtDatosCarga
        End Sub

        'Public Sub New(ByVal idUsuario As Integer)
        '    MyBase.New()
        '    _idUsuario = idUsuario
        'End Sub

#End Region

#Region "Métodos Públicos"

        Public Function BorrarInformacion() As ResultadoProceso
            Dim dbManager As New LMDataAccess
            Dim rp As ResultadoProceso
            Dim resultado As Short
            If _idUsuario > 0 AndAlso _idOrden > 0 Then

                Try
                    With dbManager
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                        .SqlParametros.Add("@idOrden", SqlDbType.BigInt).Value = _idOrden
                        .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                        .ejecutarNonQuery("BorrarAuxInfoCargueImportacionSAP", CommandType.StoredProcedure)

                        rp.EstablecerMensajeYValor(.SqlParametros("@returnValue").Value, "Ejecucion Satisfactoria")
                        If rp.Valor <> 0 Then
                            Select Case rp.Valor
                                Case 1 : rp.EstablecerMensajeYValor(.SqlParametros("@returnValue").Value, "No se encontraron datos para eliminar.")
                            End Select
                        End If
                    End With
                Catch ex As Exception
                    Throw New Exception("Error al tratar de borrar los datos cargados temporalmente en BD." & ex.Message)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                rp.EstablecerMensajeYValor(1, "No fue posible eliminar la información de la tabla temporal de carga")
            End If

            Return rp
        End Function

        Public Function CargarInformacion() As ResultadoProceso
            Dim dbManager As New LMDataAccess
            Dim rp As New ResultadoProceso
            If _dtInfoCarga IsNot Nothing AndAlso _dtInfoCarga.Rows.Count > 0 Then
                Try
                    dbManager.TiempoEsperaComando = 1200

                    With dbManager
                        .iniciarTransaccion()
                        'rp = BorrarInfoTemporalCargue(dbManager)
                        'If rp.Valor = 0 Then
                        Using dtAux As DataTable = _dtInfoCarga.Copy
                            RegistrarInfoCargue(dtAux, dbManager)
                        End Using
                        'Else
                        'Throw New Exception(rp.Mensaje)
                        'End If

                        rp = ActualizarInformacionCargue(dbManager)
                        If rp.Valor = 0 Then
                            rp = BorrarInfoTemporalCargue(dbManager)
                            If rp.Valor = 0 Then .confirmarTransaccion()
                        Else
                            Throw New Exception(rp.Mensaje)
                        End If

                        If .estadoTransaccional Then .abortarTransaccion()
                    End With
                Catch ex As Exception
                    If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception("Error al tratar de actualizar la información de cargue de la orden " & _idOrden.ToString & "." & ex.Message)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                rp.EstablecerMensajeYValor(1, "No fue posible obtener la información de cargue para actualizarla.")
            End If

            Return rp
        End Function

#End Region

#Region "Metodos Privados"

        Private Sub RegistrarInfoCargue(ByVal dtInfoCargue As DataTable, ByVal dbManager As LMDataAccess)
            If dbManager IsNot Nothing Then
                Dim dcAux As DataColumn

                If Not dtInfoCargue.Columns.Contains("idUsuario") Then
                    dcAux = New DataColumn("idUsuario")
                    dcAux.DefaultValue = _idUsuario
                    dtInfoCargue.Columns.Add(dcAux)
                End If

                'If Not dtInfoCargue.Columns.Contains("cargado") Then
                '    dcAux = New DataColumn("cargado")
                '    dcAux.DataType = Type.GetType("System.Boolean")
                '    dcAux.DefaultValue = _cargado
                '    dtInfoCargue.Columns.Add(dcAux)
                'End If

                If Not dtInfoCargue.Columns.Contains("idOrden") Then
                    dcAux = New DataColumn("idOrden")
                    dcAux.DefaultValue = _idOrden
                    dtInfoCargue.Columns.Add(dcAux)
                End If

                If Not dtInfoCargue.Columns.Contains("pedido") Then
                    dcAux = New DataColumn("pedido")
                    dcAux.DefaultValue = _pedido
                    dtInfoCargue.Columns.Add(dcAux)
                End If

                With dbManager
                    .inicilizarBulkCopy()
                    With .BulkCopy
                        .BulkCopyTimeout = 1200
                        .DestinationTableName = "AuxInfoCargueImportacionSAP"
                        .ColumnMappings.Add("serial", "serial")
                        .ColumnMappings.Add("idOrden", "idOrden")
                        .ColumnMappings.Add("pedido", "pedidoCargue")
                        .ColumnMappings.Add("entrega", "entregaCargue")
                        .ColumnMappings.Add("contabilizacion", "contabilizacionCargue")
                        .ColumnMappings.Add("cambioMaterial", "cambioMaterialCargue")
                        '.ColumnMappings.Add("fechaCargue", "fechaCargue")
                        .ColumnMappings.Add("cargado", "cargado")
                        .ColumnMappings.Add("idUsuario", "idUsuario")
                        .WriteToServer(dtInfoCargue)
                    End With
                End With
            End If
        End Sub

        Private Function ActualizarInformacionCargue(ByVal dbManager As LMDataAccess) As ResultadoProceso
            Dim rp As New ResultadoProceso
            With dbManager
                .SqlParametros.Clear()
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                '.SqlParametros.Add("@cargado", SqlDbType.Bit).Value = Cargado
                .SqlParametros.Add("@idOrden", SqlDbType.BigInt).Value = _idOrden
                .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                '.TiempoEsperaComando = 600
                .ejecutarNonQuery("ActualizarInformacionCargue", CommandType.StoredProcedure)

                rp.EstablecerMensajeYValor(.SqlParametros("@returnValue").Value, "Ejecucion Satisfactoria")
                If rp.Valor <> 0 Then
                    Select Case rp.Valor
                        Case 1 : rp.EstablecerMensajeYValor(rp.Valor, "No se encontraron seriales en la tabla temporal relacionados con la orden. ")
                        Case 2 : rp.EstablecerMensajeYValor(rp.Valor, "No se encontraron seriales en la orden pendientes por cargar en la orden. ")
                        Case 3 : rp.EstablecerMensajeYValor(rp.Valor, "Error al actualizar la información de cargue en la orden de nacionalización. ")
                        Case 4 : rp.EstablecerMensajeYValor(rp.Valor, "No fue posible identificar el tipo de producto de la orden. ")
                        Case 5 : rp.EstablecerMensajeYValor(rp.Valor, "No fue posible actualizar el estado cargado en SAP de los seriales pertenecientes a la orden. ")
                        Case 6 : rp.EstablecerMensajeYValor(rp.Valor, "Error al actualizar el estado de la orden. ")
                    End Select
                End If
            End With

            Return rp
        End Function

        Private Function BorrarInfoTemporalCargue(ByVal dbManager As LMDataAccess) As ResultadoProceso
            Dim rp As New ResultadoProceso
            With dbManager
                .SqlParametros.Clear()
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                .SqlParametros.Add("@idOrden", SqlDbType.BigInt).Value = _idOrden
                .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                '.TiempoEsperaComando = 600
                .ejecutarNonQuery("BorrarAuxInfoCargueImportacionSAP", CommandType.StoredProcedure)

                rp.EstablecerMensajeYValor(.SqlParametros("@returnValue").Value, "Ejecucion Satisfactoria")
                If rp.Valor <> 0 Then
                    rp.EstablecerMensajeYValor(rp.Valor, "Error al eliminar la información de cargue de la tabla temporal")
                End If
            End With

            Return rp
        End Function

#End Region

    End Class

End Namespace