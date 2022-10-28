Imports LMDataAccesLayer
Imports ILSBusinessLayer.Inventario
Imports LMDataAccessLayer

Namespace MensajeriaEspecializada

    Public Class InventarioSatelitePrestamo
        Inherits InventarioBodegaSateliteColeccion

#Region "Atributos"

        Private _filtroRapido As String

#End Region

#Region "Propiedades"

        Public Property FiltroRapido() As String
            Get
                Return _filtroRapido
            End Get
            Set(ByVal value As String)
                _filtroRapido = value
            End Set
        End Property

#End Region

#Region "Métodos Públicos"

        Public Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                Me.Clear()
                With dbManager
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

                    .ejecutarReader("ConsultaItemBodegaSatelitePrestamo", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        Dim objItem As ItemBodegaSatelite
                        While .Reader.Read
                            objItem = New ItemBodegaSatelite()

                            objItem.IdRegistro = CDbl(.Reader("idRegistro"))
                            objItem.Serial = .Reader("serial").ToString()
                            objItem.IdProducto = CInt(.Reader("idProducto"))
                            objItem.NombreProducto = .Reader("nombreProducto").ToString()
                            Integer.TryParse(.Reader("idSubProducto").ToString(), objItem.IdSubProducto)
                            objItem.Referencia = .Reader("referencia").ToString()
                            objItem.Material = .Reader("material").ToString()
                            objItem.IdRegion = CShort(.Reader("idRegion"))
                            objItem.CodRegion = .Reader("codRegion").ToString()
                            objItem.NombreRegion = .Reader("nombreRegion").ToString()
                            objItem.IdEstado = CInt(.Reader("idEstado"))
                            objItem.NombreEstado = .Reader("nombreEstado").ToString()
                            If Not IsDBNull(.Reader("fechaRecepcion")) Then objItem.FechaRecepcion = CDate(.Reader("fechaRecepcion"))
                            objItem.Cargado = CBool(.Reader("cargado"))
                            objItem.Nacionalizado = CBool(.Reader("nacionalizado"))
                            objItem.Termosellado = CBool(.Reader("termosellado"))
                            objItem.IdBodega = CInt(.Reader("idBodega"))
                            objItem.NombreBodega = .Reader("nombreBodega").ToString()
                            Integer.TryParse(.Reader("idPosicion").ToString(), objItem.IdPosicion)
                            objItem.CodPosicion = .Reader("codPosicion").ToString()
                            If Not IsDBNull(.Reader("fechaAsignacionInventario")) Then objItem.FechaAsignacionInventario = CDate(.Reader("fechaAsignacionInventario"))
                            Short.TryParse(.Reader("idOrigenDespacho").ToString(), objItem.IdOrigenDespacho)
                            objItem.NombreOrigenDespacho = .Reader("nombreOrigenDespacho").ToString()
                            objItem.IdUsuarioModificacion = CInt(.Reader("idUsuarioModificacion"))
                            objItem.NombreUsuarioModificacion = .Reader("nombreUsuarioModificacion").ToString()
                            objItem.IdUnidadNegocio = CInt(.Reader("idUnidadNegocio"))
                            objItem.NombreUnidadNegocio = .Reader("nombreUnidadNegocio").ToString()

                            objItem.Registrado = True
                            objItem.Accion = Enumerados.AccionItem.Ninguna

                            Me.Adicionar(objItem)
                        End While
                        _cargado = Enumerados.EstadoBinario.Activo
                    End If
                End With
            Catch ex As Exception
                Throw New Exception("Se generó error en [CargarDatos]", ex)
            End Try
            dbManager.Dispose()
        End Sub

        Public Function ObtieneInventarioPrestamoAgrupado() As DataTable
            Dim dtDatos As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                Me.Clear()
                With dbManager
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

                    If Not String.IsNullOrEmpty(Me._filtroRapido) Then .SqlParametros.Add("@filtroRapido", SqlDbType.VarChar).Value = _filtroRapido

                    dtDatos = .ejecutarDataTable("ConsultaItemBodegaSatelitePrestamoAgrupado", CommandType.StoredProcedure)
                End With
            Catch ex As Exception
                Throw New Exception("Se generó error en [ObtieneInventarioPrestamo]", ex)
            End Try
            Return dtDatos
        End Function

#End Region

    End Class

End Namespace