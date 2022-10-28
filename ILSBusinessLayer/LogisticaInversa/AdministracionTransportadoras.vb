Imports LMDataAccessLayer
Imports ILSBusinessLayer
Namespace LogisticaInversa
    Public Class AdministracionTransportadoras

#Region "atributos"
#End Region

#Region "Eventos"

        Public Shared Function ObtenerTransportadoras(ByVal idTransportadora As Integer) As DataTable
            Dim db As New LMDataAccess
            Dim dt As DataTable
            Try
                If idTransportadora > 0 Then
                    db.agregarParametroSQL("@idTransportadora", idTransportadora, SqlDbType.Int)
                End If
                db.agregarParametroSQL("@estado", 1, SqlDbType.Int)
                dt = db.ejecutarDataTable("SeleccionarTransportadoras", CommandType.StoredProcedure)
            Finally
                If db IsNot Nothing Then db = Nothing
            End Try
            Return dt
        End Function

        Public Shared Function cantidadTransportadoras() As Integer
            Dim db As New LMDataAccess
            Dim cantidad As Integer
            Try
                cantidad = db.ejecutarScalar("CantidadTransportadoras", CommandType.StoredProcedure)
            Finally
                If db IsNot Nothing Then db = Nothing
            End Try
            Return cantidad
        End Function

        Public Shared Function ObtenerTipoProducto() As DataTable
            Dim db As New LMDataAccess
            Dim dt As DataTable
            Try
                dt = db.ejecutarDataTable("ObtenerTipoProductos", CommandType.StoredProcedure)
            Finally
                If db IsNot Nothing Then db = Nothing
            End Try
            Return dt
        End Function

        Public Shared Function ObtenerTipoServicio(ByVal pIdTransportadora As Integer) As DataTable
            Dim db As New LMDataAccess
            Dim dt As DataTable
            Try
                db.SqlParametros.Clear()
                If pIdTransportadora > 0 Then db.agregarParametroSQL("@idTransportadora", pIdTransportadora, SqlDbType.Int)
                dt = db.ejecutarDataTable("ObtenerTipoServicioTransportadoras", CommandType.StoredProcedure)
            Finally
                If db IsNot Nothing Then db = Nothing
            End Try
            Return dt
        End Function

        Public Shared Function ObtenerTipoTarifa(ByVal pIdTransportadora As Integer, ByVal pOrigen As String) As DataTable
            Dim db As New LMDataAccess
            Dim dt As DataTable
            Try
                db.agregarParametroSQL("@idTransportadora", pIdTransportadora, SqlDbType.Int)
                db.agregarParametroSQL("@origen", pOrigen, SqlDbType.VarChar)
                dt = db.ejecutarDataTable("ObtenerTipoTarifaTransporte", CommandType.StoredProcedure)
            Finally
                If db IsNot Nothing Then db = Nothing
            End Try
            Return dt
        End Function

        Public Shared Function ObtenerCanales(ByVal pIdTransportadora As Integer, ByVal pOrigen As String) As DataTable
            Dim db As New LMDataAccess
            Dim dt As DataTable
            Try
                dt = db.ejecutarDataTable("ObtenerCanalesDistribucionTransporte", CommandType.StoredProcedure)
            Finally
                If db IsNot Nothing Then db = Nothing
            End Try
            Return dt
        End Function

        Public Shared Sub grabarTransportadora(ByVal dtTransportadora As DataTable, ByVal dtTarifa As DataTable, ByVal dtCombo As DataTable, ByVal dtRango As DataTable)
            Dim db As New LMDataAccess
            Dim numTransportadora As Integer = 0
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idTransp", SqlDbType.Int).Value = dtTransportadora.Rows(0).Item("idTrans")
                        .Add("@transportadora", SqlDbType.VarChar).Value = dtTransportadora.Rows(0).Item("transportadora")
                        .Add("@estado", SqlDbType.Int).Value = dtTransportadora.Rows(0).Item("estado")
                        .Add("@maneja_pos", SqlDbType.VarChar).Value = dtTransportadora.Rows(0).Item("maneja_pos")
                        .Add("@usaGuia", SqlDbType.Int).Value = dtTransportadora.Rows(0).Item("usaGuia")
                        .Add("@usaPrecinto", SqlDbType.Int).Value = dtTransportadora.Rows(0).Item("usaPrecinto")
                        .Add("@aplicaLogisticaInversa", SqlDbType.Int).Value = dtTransportadora.Rows(0).Item("aplicaLogisticaInversa")
                        .Add("@cargaPorImportacion", SqlDbType.Int).Value = dtTransportadora.Rows(0).Item("cargaPorImportacion")
                        .Add("@aplicaDespachoNacional", SqlDbType.Int).Value = dtTransportadora.Rows(0).Item("aplicaDespachoNacional")
                        .Add("@aplicaTipoProducto", SqlDbType.Int).Value = dtTransportadora.Rows(0).Item("aplicaTipoProducto")
                    End With
                    '.ejecutarNonQuery("AdicionarTransportadora", CommandType.StoredProcedure)
                    numTransportadora = .ejecutarScalar("AdicionarTransportadora", CommandType.StoredProcedure)

                    If dtTarifa IsNot Nothing Then
                        With .SqlParametros
                            .Clear()
                            .Add("@idTipoServicio", SqlDbType.Int).Value = dtTarifa.Rows(0).Item("idTipoServicio")
                            .Add("@idTransportadora", SqlDbType.Int).Value = numTransportadora
                            .Add("@valordeManejo", SqlDbType.Float).Value = dtTarifa.Rows(0).Item("valordeManejo")
                            .Add("@tarifaMinima", SqlDbType.Int).Value = dtTarifa.Rows(0).Item("tarifaMinima")
                            .Add("@idTipoProducto", SqlDbType.Int).Value = dtTarifa.Rows(0).Item("idTipoProducto")
                            .Add("@idTipoTarifa", SqlDbType.Int).Value = dtTarifa.Rows(0).Item("idTipoTarifa")
                            .Add("@idUsuario", SqlDbType.Int).Value = dtTarifa.Rows(0).Item("idUsuario")
                        End With
                        .ejecutarNonQuery("AdicionarTarifaTransportadora", CommandType.StoredProcedure)
                    End If
                    If dtCombo IsNot Nothing Then
                        Dim i As Integer
                        i = 0
                        For i = 0 To dtCombo.Rows.Count - 1
                            With .SqlParametros
                                .Clear()
                                .Add("@idTipoTarifa", SqlDbType.Int).Value = dtCombo.Rows(i).Item("idTipoTarifa")
                                .Add("@idTipoProducto", SqlDbType.Int).Value = dtCombo.Rows(i).Item("idTipoProducto")
                                .Add("@idUsuario", SqlDbType.Int).Value = dtCombo.Rows(i).Item("idUsuario")
                                .Add("@idTransportadora", SqlDbType.Int).Value = numTransportadora
                                .Add("@idGrupoTransportadora", SqlDbType.Int).Value = dtCombo.Rows(i).Item("idGrupoTransportadora")
                            End With
                            .ejecutarNonQuery("AdicionarCombosTransportadora", CommandType.StoredProcedure)
                        Next
                    End If
                    If dtRango IsNot Nothing Then
                        With .SqlParametros
                            .Clear()
                            .Add("@idTipoTarifa", SqlDbType.Int).Value = dtRango.Rows(0).Item("idTipoTarifa")
                            .Add("@valorInicial", SqlDbType.Int).Value = CInt(dtRango.Rows(0).Item("valorInicial"))
                            .Add("@valorFinal", SqlDbType.Int).Value = CInt(dtRango.Rows(0).Item("valorFinal"))
                            .Add("@idUsuario", SqlDbType.Int).Value = dtRango.Rows(0).Item("idUsuario")
                            .Add("@idTransportadora", SqlDbType.Int).Value = dtRango.Rows(0).Item("idTransportadora")
                        End With
                        .ejecutarNonQuery("AdicionarRangosTransportadora", CommandType.StoredProcedure)
                    End If
                End With
            Finally
                If db IsNot Nothing Then db = Nothing
            End Try
        End Sub

        Public Shared Sub ActualizarDatosTransportadora(ByVal dtDatos As DataTable, ByVal pOrigen As String)
            Dim db As New LMDataAccess
            Try
                If pOrigen = "Transportadora" Then
                    With db
                        With .SqlParametros
                            .Clear()
                            .Add("@idTransp", SqlDbType.Int).Value = dtDatos.Rows(0).Item("idTrans")
                            .Add("@transportadora", SqlDbType.VarChar).Value = dtDatos.Rows(0).Item("transportadora")
                            .Add("@estado", SqlDbType.Int).Value = dtDatos.Rows(0).Item("estado")
                            .Add("@maneja_pos", SqlDbType.VarChar).Value = dtDatos.Rows(0).Item("maneja_pos")
                            .Add("@usaGuia", SqlDbType.Int).Value = dtDatos.Rows(0).Item("usaGuia")
                            .Add("@usaPrecinto", SqlDbType.Int).Value = dtDatos.Rows(0).Item("usaPrecinto")
                            .Add("@aplicaLogisticaInversa", SqlDbType.Int).Value = dtDatos.Rows(0).Item("aplicaLogisticaInversa")
                            .Add("@cargaPorImportacion", SqlDbType.Int).Value = dtDatos.Rows(0).Item("cargaPorImportacion")
                            .Add("@aplicaDespachoNacional", SqlDbType.Int).Value = dtDatos.Rows(0).Item("aplicaDespachoNacional")
                            .Add("@aplicaTipoProducto", SqlDbType.Int).Value = dtDatos.Rows(0).Item("aplicaTipoProducto")
                        End With
                        .ejecutarNonQuery("ActualizarTransportadora", CommandType.StoredProcedure)
                    End With
                ElseIf pOrigen = "Tarifa" Then
                    With db
                        With .SqlParametros
                            .Clear()
                            If dtDatos.Rows(0).Item("accion") <> "borrar" Then
                                .Add("@idTarifa", SqlDbType.Int).Value = dtDatos.Rows(0).Item("idTarifa")
                                .Add("@idTipoServicio", SqlDbType.Int).Value = dtDatos.Rows(0).Item("idTipoServicio")
                                .Add("@idTransportadora", SqlDbType.Int).Value = dtDatos.Rows(0).Item("idTransportadora")
                                .Add("@valordeManejo", SqlDbType.Float).Value = dtDatos.Rows(0).Item("valordeManejo")
                                .Add("@tarifaMinima", SqlDbType.Int).Value = dtDatos.Rows(0).Item("tarifaMinima")
                                .Add("@idTipoProducto", SqlDbType.Int).Value = dtDatos.Rows(0).Item("idTipoProducto")
                                .Add("@idTipoTarifa", SqlDbType.Int).Value = dtDatos.Rows(0).Item("idTipoTarifa")
                                .Add("@idCanal", SqlDbType.Int).Value = dtDatos.Rows(0).Item("idCanal")
                                .Add("@idUsuario", SqlDbType.Int).Value = dtDatos.Rows(0).Item("idUsuario")
                                .Add("@accion", SqlDbType.VarChar).Value = dtDatos.Rows(0).Item("accion")
                            Else
                                .Add("@idTarifa", SqlDbType.Int).Value = dtDatos.Rows(0).Item("idTarifa")
                                .Add("@accion", SqlDbType.VarChar).Value = dtDatos.Rows(0).Item("accion")
                            End If
                        End With
                        .ejecutarNonQuery("ActualizarTarifasTransportadora", CommandType.StoredProcedure)
                    End With
                ElseIf pOrigen = "Combo" Then
                    If dtDatos.Rows(0).Item("accion") = "insertar" Then
                        Dim i As Integer
                        i = 0
                        For i = 0 To dtDatos.Rows.Count - 1
                            With db
                                With .SqlParametros
                                    .Clear()
                                    .Add("@idTipoTarifa", SqlDbType.Int).Value = dtDatos.Rows(i).Item("idTipoTarifa")
                                    .Add("@idTipoProducto", SqlDbType.Int).Value = dtDatos.Rows(i).Item("idTipoProducto")
                                    .Add("@idUsuario", SqlDbType.Int).Value = dtDatos.Rows(i).Item("idUsuario")
                                    .Add("@idTransportadora", SqlDbType.Int).Value = dtDatos.Rows(i).Item("idTransportadora")
                                    .Add("@idGrupoTransportadora", SqlDbType.Int).Value = dtDatos.Rows(i).Item("idGrupoTransportadora")
                                    .Add("@accion", SqlDbType.VarChar).Value = dtDatos.Rows(i).Item("accion")
                                End With
                                .ejecutarNonQuery("ActualizarCombosTransportadora", CommandType.StoredProcedure)
                            End With
                        Next
                    ElseIf dtDatos.Rows(0).Item("accion") = "actualizar" Then
                        With db
                            With .SqlParametros
                                .Clear()
                                .Add("@idTransportadora", SqlDbType.Int).Value = dtDatos.Rows(0).Item("idTransportadora")
                                .Add("@idGrupoTransportadora", SqlDbType.Int).Value = dtDatos.Rows(0).Item("idGrupoTransportadora")
                                .Add("@accion", SqlDbType.VarChar).Value = "borrar"
                            End With
                            .ejecutarNonQuery("ActualizarCombosTransportadora", CommandType.StoredProcedure)
                        End With
                        Dim i As Integer
                        i = 0
                        For i = 0 To dtDatos.Rows.Count - 1
                            With db
                                With .SqlParametros
                                    .Clear()
                                    .Add("@idTipoTarifa", SqlDbType.Int).Value = dtDatos.Rows(i).Item("idTipoTarifa")
                                    .Add("@idTipoProducto", SqlDbType.Int).Value = dtDatos.Rows(i).Item("idTipoProducto")
                                    .Add("@idUsuario", SqlDbType.Int).Value = dtDatos.Rows(i).Item("idUsuario")
                                    .Add("@idTransportadora", SqlDbType.Int).Value = dtDatos.Rows(i).Item("idTransportadora")
                                    .Add("@idGrupoTransportadora", SqlDbType.Int).Value = dtDatos.Rows(i).Item("idGrupoTransportadora")
                                    .Add("@accion", SqlDbType.VarChar).Value = "insertar"
                                End With
                                .ejecutarNonQuery("ActualizarCombosTransportadora", CommandType.StoredProcedure)
                            End With
                        Next
                    ElseIf dtDatos.Rows(0).Item("accion") = "borrar" Then
                        With db
                            With .SqlParametros
                                .Clear()
                                .Add("@idTransportadora", SqlDbType.Int).Value = dtDatos.Rows(0).Item("idTransportadora")
                                .Add("@idGrupoTransportadora", SqlDbType.Int).Value = dtDatos.Rows(0).Item("idGrupoTransportadora")
                                .Add("@accion", SqlDbType.VarChar).Value = dtDatos.Rows(0).Item("accion")
                            End With
                            .ejecutarNonQuery("ActualizarCombosTransportadora", CommandType.StoredProcedure)
                        End With
                    End If
                ElseIf pOrigen = "Rangos" Then
                    If dtDatos.Rows(0).Item("accion") = "insertar" Then
                        With db
                            With .SqlParametros
                                .Clear()
                                .Add("@idTipoTarifa", SqlDbType.Int).Value = dtDatos.Rows(0).Item("idTipoTarifa")
                                .Add("@valorInicial", SqlDbType.Int).Value = CInt(dtDatos.Rows(0).Item("valorInicial"))
                                .Add("@valorFinal", SqlDbType.Int).Value = CInt(dtDatos.Rows(0).Item("valorFinal"))
                                .Add("@idUsuario", SqlDbType.Int).Value = dtDatos.Rows(0).Item("idUsuario")
                                .Add("@idTransportadora", SqlDbType.Int).Value = dtDatos.Rows(0).Item("idTransportadora")
                                .Add("@accion", SqlDbType.VarChar).Value = dtDatos.Rows(0).Item("accion")
                            End With
                            .ejecutarNonQuery("ActualizarRangosTransportadora", CommandType.StoredProcedure)
                        End With
                    ElseIf dtDatos.Rows(0).Item("accion") = "actualizar" Then
                        With db
                            With .SqlParametros
                                .Clear()
                                .Add("@idTipoTarifa", SqlDbType.Int).Value = dtDatos.Rows(0).Item("idTipoTarifa")
                                .Add("@valorInicial", SqlDbType.Int).Value = CInt(dtDatos.Rows(0).Item("valorInicial"))
                                .Add("@valorFinal", SqlDbType.Int).Value = CInt(dtDatos.Rows(0).Item("valorFinal"))
                                .Add("@idUsuario", SqlDbType.Int).Value = dtDatos.Rows(0).Item("idUsuario")
                                .Add("@IdRango", SqlDbType.Int).Value = dtDatos.Rows(0).Item("idRango")
                                .Add("@accion", SqlDbType.VarChar).Value = dtDatos.Rows(0).Item("accion")
                            End With
                            .ejecutarNonQuery("ActualizarRangosTransportadora", CommandType.StoredProcedure)
                        End With
                    ElseIf dtDatos.Rows(0).Item("accion") = "borrar" Then
                        With db
                            With .SqlParametros
                                .Clear()
                                .Add("@IdRango", SqlDbType.Int).Value = dtDatos.Rows(0).Item("idRango")
                                .Add("@accion", SqlDbType.VarChar).Value = dtDatos.Rows(0).Item("accion")
                            End With
                            .ejecutarNonQuery("ActualizarRangosTransportadora", CommandType.StoredProcedure)
                        End With
                    End If
                ElseIf pOrigen = "TipoTarifa" Then
                    If dtDatos.Rows(0).Item("accion") = "insertar" Then
                        With db
                            With .SqlParametros
                                .Clear()
                                .Add("@descripcion", SqlDbType.VarChar, 50).Value = dtDatos.Rows(0).Item("descripcion")
                                .Add("@idUsuario", SqlDbType.Int).Value = dtDatos.Rows(0).Item("idUsuario")
                                .Add("@accion", SqlDbType.VarChar).Value = dtDatos.Rows(0).Item("accion")
                            End With
                            .ejecutarNonQuery("ActualizarTipoTarifa", CommandType.StoredProcedure)
                        End With
                    ElseIf dtDatos.Rows(0).Item("accion") = "actualizar" Then
                        With db
                            With .SqlParametros
                                .Clear()
                                .Add("@idTipoTarifa", SqlDbType.Int).Value = dtDatos.Rows(0).Item("idTipoTarifa")
                                .Add("@descripcion", SqlDbType.VarChar, 50).Value = dtDatos.Rows(0).Item("descripcion")
                                .Add("@idUsuario", SqlDbType.Int).Value = dtDatos.Rows(0).Item("idUsuario")
                                .Add("@accion", SqlDbType.VarChar).Value = dtDatos.Rows(0).Item("accion")
                            End With
                            .ejecutarNonQuery("ActualizarTipoTarifa", CommandType.StoredProcedure)
                        End With
                    ElseIf dtDatos.Rows(0).Item("accion") = "borrar" Then
                        With db
                            With .SqlParametros
                                .Clear()
                                .Add("@IdTipoTarifa", SqlDbType.Int).Value = dtDatos.Rows(0).Item("idTipoTarifa")
                                .Add("@accion", SqlDbType.VarChar).Value = dtDatos.Rows(0).Item("accion")
                            End With
                            .ejecutarNonQuery("ActualizarTipoTarifa", CommandType.StoredProcedure)
                        End With
                    End If
                End If

            Finally
                If db IsNot Nothing Then db = Nothing
            End Try
        End Sub

        Public Shared Function ObtenerPropiedadesTransportadora(ByVal pIdTransportadora As Integer)

            Dim db As New LMDataAccess
            Dim dt As DataTable
            Try
                db.SqlParametros.Clear()
                db.SqlParametros.Add("@idTransportadora", SqlDbType.Int).Value = pIdTransportadora
                dt = db.ejecutarDataTable("ObtenerPropiedadesTransporte", CommandType.StoredProcedure)
            Finally
                If db IsNot Nothing Then db = Nothing
            End Try
            Return dt

        End Function

        Public Shared Function ObtenerTarifaTransportadora(ByVal pIdTransportadora As Integer, ByVal pIdTarifa As Integer)

            Dim db As New LMDataAccess
            Dim dt As DataTable
            Try
                db.SqlParametros.Clear()
                If pIdTransportadora <> 0 Then db.SqlParametros.Add("@idTransportadora", SqlDbType.Int).Value = pIdTransportadora
                If pIdTarifa <> 0 Then db.SqlParametros.Add("@idTarifa", SqlDbType.Int).Value = pIdTarifa
                dt = db.ejecutarDataTable("ObtenerTarifasTransportadora", CommandType.StoredProcedure)
            Finally
                If db IsNot Nothing Then db = Nothing
            End Try
            Return dt

        End Function

        Public Shared Function ObtenerCombosTransportadora(ByVal pIdTransportadora As Integer, ByVal pIdGrupoCombo As Integer, ByVal comboTransportadora As Integer)

            Dim db As New LMDataAccess
            Dim dt As DataTable
            Try
                db.SqlParametros.Clear()
                If pIdTransportadora > 0 Then db.SqlParametros.Add("@idTransportadora", SqlDbType.Int).Value = pIdTransportadora
                If pIdGrupoCombo > 0 Then db.SqlParametros.Add("@idGrupoTransportadora", SqlDbType.Int).Value = pIdGrupoCombo
                If comboTransportadora > 0 Then db.SqlParametros.Add("@combosTransportadora", SqlDbType.Int).Value = comboTransportadora
                dt = db.ejecutarDataTable("ObtenerCombosTransporte", CommandType.StoredProcedure)
            Finally
                If db IsNot Nothing Then db = Nothing
            End Try
            Return dt

        End Function

        Public Shared Function ObtenerGrupoComboMaximo(ByVal pIdTransportadora As Integer)

            Dim db As New LMDataAccess
            Dim dt As DataTable
            Try
                db.SqlParametros.Clear()
                If pIdTransportadora > 0 Then db.SqlParametros.Add("@idTransportadora", SqlDbType.Int).Value = pIdTransportadora
                dt = db.ejecutarDataTable("ObtenerMaximogrupoTransporte", CommandType.StoredProcedure)
            Finally
                If db IsNot Nothing Then db = Nothing
            End Try
            Return dt

        End Function

        Public Shared Function ObtenerRangosTransportadora(ByVal pIdTransportadora As Integer, ByVal pIdTipoTarifa As String)

            Dim db As New LMDataAccess
            Dim dt As DataTable
            Try
                db.SqlParametros.Clear()
                If pIdTransportadora > 0 Then db.SqlParametros.Add("@idTransportadora", SqlDbType.Int).Value = pIdTransportadora
                If pIdTipoTarifa > 0 Then db.SqlParametros.Add("@idTipoTarifa", SqlDbType.Int).Value = pIdTipoTarifa
                dt = db.ejecutarDataTable("ObtenerRangosTransporte", CommandType.StoredProcedure)
            Finally
                If db IsNot Nothing Then db = Nothing
            End Try
            Return dt

        End Function


        Public Shared Function ObtenerComboTarifa(ByVal pIdTransportadora As Integer, ByVal pIdTipoTarifa As String)

            Dim db As New LMDataAccess
            Dim dt As DataTable
            Try
                db.SqlParametros.Clear()
                db.SqlParametros.Add("@idTransportadora", SqlDbType.Int).Value = pIdTransportadora
                db.SqlParametros.Add("@idTipoTarifa", SqlDbType.Int).Value = pIdTipoTarifa
                dt = db.ejecutarDataTable("ObtenerComboTarifa", CommandType.StoredProcedure)
            Finally
                If db IsNot Nothing Then db = Nothing
            End Try
            Return dt

        End Function

        Public Shared Function ObtenerRangoTarifa(ByVal pIdTransportadora As Integer, ByVal pIdTipoTarifa As String)

            Dim db As New LMDataAccess
            Dim dt As DataTable
            Try
                db.SqlParametros.Clear()
                db.SqlParametros.Add("@idTransportadora", SqlDbType.Int).Value = pIdTransportadora
                db.SqlParametros.Add("@idTipoTarifa", SqlDbType.Int).Value = pIdTipoTarifa
                dt = db.ejecutarDataTable("ObtenerRangoTarifa", CommandType.StoredProcedure)
            Finally
                If db IsNot Nothing Then db = Nothing
            End Try
            Return dt

        End Function

#End Region

    End Class
End Namespace