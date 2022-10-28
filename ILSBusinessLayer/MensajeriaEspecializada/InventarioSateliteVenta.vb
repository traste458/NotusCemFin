Imports ILSBusinessLayer.Inventario
Imports LMDataAccessLayer

Namespace MensajeriaEspecializada

    Public Class InventarioSateliteVenta
        Inherits InventarioBodegaSateliteColeccion

#Region "Atributos"

        Private _idPlanVenta As List(Of Integer)
        Private _idCampania As List(Of Integer)
        Private _idTipoProducto As List(Of Integer)

#End Region

#Region "Propiedaes"

        Public Property IdPlanVenta As List(Of Integer)
            Get
                Return _idPlanVenta
            End Get
            Set(value As List(Of Integer))
                _idPlanVenta = value
            End Set
        End Property

        Public Property IdCampania As List(Of Integer)
            Get
                Return _idCampania
            End Get
            Set(value As List(Of Integer))
                _idCampania = value
            End Set
        End Property

        Public Property IdTipoProducto As List(Of Integer)
            Get
                Return _idTipoProducto
            End Get
            Set(value As List(Of Integer))
                _idTipoProducto = value
            End Set
        End Property

#End Region

#Region "Métodos Públicos"

        Public Function ObtieneInventarioVentaAgrupado()
            Dim dtDatos As DataTable
            Using dbManager As New LMDataAccess
                Try
                    Me.Clear()
                    With dbManager
                        .TiempoEsperaComando = 0
                        If Not _idRegistro Is Nothing AndAlso _idRegistro.Count > 0 Then _
                        .SqlParametros.Add("@listaIdRegistro", SqlDbType.VarChar).Value = String.Join(",", _idRegistro.Cast(Of String)().ToArray)

                        If Not _serial Is Nothing AndAlso _serial.Count > 0 Then _
                            .SqlParametros.Add("@listaSerial", SqlDbType.VarChar).Value = String.Join(",", _serial.ToArray)

                        If Not _idProducto Is Nothing AndAlso _idProducto.Count > 0 Then _
                            .SqlParametros.Add("@listaProducto", SqlDbType.VarChar).Value = String.Join(",", _idProducto.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())

                        If Not _material Is Nothing AndAlso _material.Count > 0 Then _
                            .SqlParametros.Add("@listaMaterial", SqlDbType.VarChar).Value = String.Join(",", _material.ToArray)

                        If Not _idRegion Is Nothing AndAlso _idRegion.Count > 0 Then _
                            .SqlParametros.Add("@listaRegion", SqlDbType.VarChar).Value = String.Join(",", _idRegion.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())

                        If Not _idEstado Is Nothing AndAlso _idEstado.Count > 0 Then _
                            .SqlParametros.Add("@listaEstado", SqlDbType.VarChar).Value = String.Join(",", _idEstado.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())

                        If Not _fechaRecepcionInicial.Equals(Date.MinValue) Then _
                            .SqlParametros.Add("@fechaRecepcionInicial", SqlDbType.DateTime).Value = _fechaRecepcionInicial

                        If Not _fechaRecepcionFinal.Equals(Date.MinValue) Then _
                            .SqlParametros.Add("@fechaRecepcionFinal", SqlDbType.DateTime).Value = _fechaRecepcionFinal

                        If _cargado <> Enumerados.EstadoBinario.NoEstablecido Then _
                            .SqlParametros.Add("@cargado", SqlDbType.Bit).Value = IIf(_cargado = Enumerados.EstadoBinario.Activo, 1, 0)

                        If _nacionalizado <> Enumerados.EstadoBinario.NoEstablecido Then _
                            .SqlParametros.Add("@nacionalizado", SqlDbType.Bit).Value = IIf(_nacionalizado = Enumerados.EstadoBinario.Activo, 1, 0)

                        If _termosellado <> Enumerados.EstadoBinario.NoEstablecido Then _
                            .SqlParametros.Add("@termosellado", SqlDbType.Bit).Value = IIf(_termosellado = Enumerados.EstadoBinario.Activo, 1, 0)

                        If Not _idBodega Is Nothing AndAlso _idBodega.Count > 0 Then _
                            .SqlParametros.Add("@listaBodega", SqlDbType.VarChar).Value = String.Join(",", _idBodega.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())

                        If Not _idPosicion Is Nothing AndAlso _idPosicion.Count > 0 Then _
                            .SqlParametros.Add("@listaPosicion", SqlDbType.VarChar).Value = String.Join(",", _idPosicion.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())

                        If Not _codPosicion Is Nothing AndAlso _codPosicion.Count > 0 Then _
                            .SqlParametros.Add("@listaCodPosicion", SqlDbType.VarChar).Value = String.Join(",", _codPosicion.ToArray)

                        If Not _fechaAsignacionInventarioInicial.Equals(Date.MinValue) Then _
                            .SqlParametros.Add("@fechaAsignacionInventarioInicial", SqlDbType.DateTime).Value = _fechaAsignacionInventarioInicial

                        If Not _fechaAsignacionInventarioFinal.Equals(Date.MinValue) Then _
                            .SqlParametros.Add("@fechaAsignacionInventarioFinal", SqlDbType.DateTime).Value = _fechaAsignacionInventarioFinal

                        If Not _idClienteExterno Is Nothing AndAlso _idClienteExterno.Count > 0 Then _
                            .SqlParametros.Add("@listaClienteExterno", SqlDbType.VarChar).Value = String.Join(",", _idClienteExterno.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())

                        If Not _idUnidadNegocio Is Nothing AndAlso _idUnidadNegocio.Count > 0 Then _
                            .SqlParametros.Add("@listaUnidadNegocio", SqlDbType.VarChar).Value = String.Join(",", _idUnidadNegocio.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())

                        If Not _idPlanVenta Is Nothing AndAlso _idPlanVenta.Count > 0 Then _
                            .SqlParametros.Add("@listaPlanVenta", SqlDbType.VarChar).Value = String.Join(",", _idPlanVenta.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())

                        If Not _idCampania Is Nothing AndAlso _idCampania.Count > 0 Then _
                            .SqlParametros.Add("@listaCompania", SqlDbType.VarChar).Value = String.Join(",", _idCampania.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())

                        If Not _idTipoProducto Is Nothing AndAlso _idTipoProducto.Count > 0 Then _
                            .SqlParametros.Add("@listaTipoProducto", SqlDbType.VarChar).Value = String.Join(",", _idTipoProducto.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())

                        dtDatos = .EjecutarDataTable("ConsultaItemBodegaSateliteVentaAgrupado", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return dtDatos
        End Function

#End Region

    End Class

End Namespace