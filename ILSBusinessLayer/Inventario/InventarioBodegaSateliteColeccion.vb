Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Namespace Inventario

    Public Class InventarioBodegaSateliteColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Protected Friend _idRegistro As List(Of Long)
        Protected Friend _serial As List(Of String)
        Protected Friend _idProducto As List(Of Integer)
        Protected Friend _material As List(Of String)
        Protected Friend _idRegion As List(Of Short)
        Protected Friend _idEstado As List(Of Short)
        Protected Friend _fechaRecepcionInicial As Date
        Protected Friend _fechaRecepcionFinal As Date
        Protected Friend _cargado As Enumerados.EstadoBinario
        Protected Friend _nacionalizado As Enumerados.EstadoBinario
        Protected Friend _termosellado As Enumerados.EstadoBinario
        Protected Friend _idBodega As List(Of Integer)
        Protected Friend _idPosicion As List(Of Integer)
        Protected Friend _codPosicion As List(Of String)
        Protected Friend _fechaAsignacionInventarioInicial As Date
        Protected Friend _fechaAsignacionInventarioFinal As Date
        Protected Friend _idClienteExterno As List(Of Integer)
        Protected Friend _idUnidadNegocio As List(Of Integer)
        Protected Friend _idClaseSim As List(Of Short)

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdRegistro() As List(Of Long)
            Get
                Return _idRegistro
            End Get
            Set(ByVal value As List(Of Long))
                _idRegistro = value
            End Set
        End Property

        Public Property Serial() As List(Of String)
            Get
                Return _serial
            End Get
            Set(ByVal value As List(Of String))
                _serial = value
            End Set
        End Property

        Public Property IdProducto() As List(Of Integer)
            Get
                Return _idProducto
            End Get
            Set(ByVal value As List(Of Integer))
                _idProducto = value
            End Set
        End Property

        Public Property Material() As List(Of String)
            Get
                Return _material
            End Get
            Set(ByVal value As List(Of String))
                _material = value
            End Set
        End Property

        Public Property IdRegion() As List(Of Short)
            Get
                Return _idRegion
            End Get
            Set(ByVal value As List(Of Short))
                _idRegion = value
            End Set
        End Property

        Public Property IdEstado() As List(Of Short)
            Get
                Return _idEstado
            End Get
            Set(ByVal value As List(Of Short))
                _idEstado = value
            End Set
        End Property

        Public Property FechaRecepcionInicial() As Date
            Get
                Return _fechaRecepcionInicial
            End Get
            Set(ByVal value As Date)
                _fechaRecepcionInicial = value
            End Set
        End Property

        Public Property FechaRecepcionFinal() As Date
            Get
                Return _fechaRecepcionFinal
            End Get
            Set(ByVal value As Date)
                _fechaRecepcionFinal = value
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

        Public Property Nacionalizado() As Boolean
            Get
                Return _nacionalizado
            End Get
            Set(ByVal value As Boolean)
                _nacionalizado = value
            End Set
        End Property

        Public Property Termosellado() As Boolean
            Get
                Return _termosellado
            End Get
            Set(ByVal value As Boolean)
                _termosellado = value
            End Set
        End Property

        Public Property IdBodega() As List(Of Integer)
            Get
                Return _idBodega
            End Get
            Set(ByVal value As List(Of Integer))
                _idBodega = value
            End Set
        End Property

        Public Property IdPosicion() As List(Of Integer)
            Get
                Return _idPosicion
            End Get
            Set(ByVal value As List(Of Integer))
                _idPosicion = value
            End Set
        End Property

        Public Property CodPosicion() As List(Of String)
            Get
                Return _codPosicion
            End Get
            Set(ByVal value As List(Of String))
                _codPosicion = value
            End Set
        End Property

        Public Property FechaAsignacionInventarioInicial() As Date
            Get
                Return _fechaAsignacionInventarioInicial
            End Get
            Set(ByVal value As Date)
                _fechaAsignacionInventarioInicial = value
            End Set
        End Property

        Public Property FechaAsignacionInventarioFinal() As Date
            Get
                Return _fechaAsignacionInventarioFinal
            End Get
            Set(ByVal value As Date)
                _fechaAsignacionInventarioFinal = value
            End Set
        End Property

        Public Property IdClienteExterno() As List(Of Integer)
            Get
                Return _idClienteExterno
            End Get
            Set(ByVal value As List(Of Integer))
                _idClienteExterno = value
            End Set
        End Property

        Public Property IdUnidadNegocio() As List(Of Integer)
            Get
                Return _idUnidadNegocio
            End Get
            Set(ByVal value As List(Of Integer))
                _idUnidadNegocio = value
            End Set
        End Property

        Public Property IdClaseSim As List(Of Short)
            Get
                Return _idClaseSim
            End Get
            Set(value As List(Of Short))
                _idClaseSim = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim objItemBodegaSatelite As Type = GetType(ItemBodegaSatelite)
            Dim pInfo As PropertyInfo

            For Each pInfo In objItemBodegaSatelite.GetProperties
                If pInfo.PropertyType.Namespace = "System" Then
                    With dtAux
                        .Columns.Add(pInfo.Name, pInfo.PropertyType)
                    End With
                End If
            Next

            Return dtAux
        End Function

#End Region

#Region "Métodos Públicos"

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As ItemBodegaSatelite)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As ItemBodegaSatelite)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As InventarioBodegaSateliteColeccion)
            For Each item As ItemBodegaSatelite In rango
                item.Accion = Enumerados.AccionItem.Adicionar
            Next

            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As ItemBodegaSatelite)
            With Me.InnerList
                For Each item As ItemBodegaSatelite In Me.InnerList
                    If item.IdRegistro = valor.IdRegistro Then
                        item.Accion = Enumerados.AccionItem.Eliminar
                        Exit For
                    End If
                Next
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            If index > -1 Then
                CType(Me.InnerList(index), ItemBodegaSatelite).Accion = Enumerados.AccionItem.Eliminar
            End If
        End Sub

        Public Function IndiceDe(ByVal registro As Long) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), ItemBodegaSatelite)
                    If .IdRegistro = registro Then
                        indice = index
                        Exit For
                    End If
                End With
            Next
            Return indice
        End Function

        Public Function GenerarDataTable() As DataTable
            If Not _cargado = Enumerados.EstadoBinario.Activo Then CargarDatos()
            Dim dtAux As DataTable = CrearEstructuraDeTabla()
            Dim drAux As DataRow
            Dim miRegistro As ItemBodegaSatelite

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miRegistro = CType(Me.InnerList(index), ItemBodegaSatelite)
                If miRegistro IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(ItemBodegaSatelite).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miRegistro, Nothing)
                        End If
                    Next
                    dtAux.Rows.Add(drAux)
                End If
            Next

            Return dtAux
        End Function

        Public Overridable Sub CargarDatos()

            Dim dbManager As New LMDataAccess
            Try
                'Filtros de la colección.
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


                    .ejecutarReader("ConsultaItemBodegaSatelite", CommandType.StoredProcedure)
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
                    End If
                End With

            Catch ex As Exception
                Throw New Exception("Se generó error en [CargarDatos]", ex)
            End Try
            dbManager.Dispose()
            
        End Sub

        Public Function AplicarCambios(ByVal idUsuario As Integer) As List(Of ResultadoProceso)
            Dim resultado As New List(Of ResultadoProceso)

            'Using dbManager As New LMDataAccess
            Dim dbManager As New LMDataAccess
            Try
                Dim itemsEliminarColeccion As New InventarioBodegaSateliteColeccion()
                Dim itemsEliminarLista As New List(Of Long)
                Dim itemsAgregarDataTable As DataTable = CrearEstructuraDeTabla()
                Dim itemsActualizarDataTable As DataTable = CrearEstructuraDeTabla()

                For Each itemBodega As ItemBodegaSatelite In Me.InnerList
                    'Items para eliminar
                    If itemBodega.Accion = Enumerados.AccionItem.Eliminar Then
                        itemsEliminarColeccion.Adicionar(itemBodega)
                        itemsEliminarLista.Add(itemBodega.IdRegistro)

                        'Items para agregar
                    ElseIf itemBodega.Accion = Enumerados.AccionItem.Adicionar Then
                        Dim filaInventario As DataRow = itemsAgregarDataTable.NewRow()

                        With filaInventario
                            .Item("serial") = itemBodega.Serial
                            .Item("idProducto") = itemBodega.IdProducto
                            If itemBodega.IdSubProducto > 0 Then .Item("idSubProducto") = itemBodega.IdSubProducto
                            If itemBodega.IdRegion > 0 Then .Item("idRegion") = itemBodega.IdRegion
                            .Item("idEstado") = itemBodega.IdEstado
                            .Item("fechaRecepcion") = itemBodega.FechaRecepcion
                            .Item("cargado") = itemBodega.Cargado
                            .Item("nacionalizado") = itemBodega.Nacionalizado
                            .Item("termosellado") = itemBodega.Termosellado
                            .Item("idBodega") = itemBodega.IdBodega
                            If itemBodega.IdPosicion > 0 Then .Item("idPosicion") = itemBodega.IdPosicion
                            .Item("fechaAsignacionInventario") = itemBodega.FechaAsignacionInventario
                            If itemBodega.IdAlmacenBodega > 0 Then .Item("IdAlmaceBodega") = itemBodega.IdAlmacenBodega
                            If itemBodega.IdOrigenDespacho > 0 Then .Item("idOrigenDespacho") = itemBodega.IdOrigenDespacho
                            .Item("idUsuarioModificacion") = idUsuario
                        End With
                        itemsAgregarDataTable.Rows.Add(filaInventario)

                        'Items para actualizar
                    ElseIf itemBodega.Accion = Enumerados.AccionItem.Actualizar Then
                        Dim filaInventario As DataRow = itemsActualizarDataTable.NewRow()

                        With filaInventario
                            .Item("idRegistro") = itemBodega.IdRegistro
                            .Item("serial") = itemBodega.Serial
                            If itemBodega.IdProducto > 0 Then .Item("idProducto") = itemBodega.IdProducto
                            If itemBodega.IdSubProducto > 0 Then .Item("idSubProducto") = itemBodega.IdSubProducto
                            If itemBodega.IdRegion > 0 Then .Item("idRegion") = itemBodega.IdRegion
                            If itemBodega.IdEstado > 0 Then .Item("idEstado") = itemBodega.IdEstado
                            If Not itemBodega.FechaRecepcion.Equals(Date.MinValue) Then .Item("fechaRecepcion") = itemBodega.FechaRecepcion
                            .Item("cargado") = itemBodega.Cargado
                            .Item("nacionalizado") = itemBodega.Nacionalizado
                            .Item("termosellado") = itemBodega.Termosellado
                            If itemBodega.IdBodega > 0 Then .Item("idBodega") = itemBodega.IdBodega
                            If itemBodega.IdPosicion > 0 Then .Item("idPosicion") = itemBodega.IdPosicion
                            If Not itemBodega.FechaAsignacionInventario.Equals(Date.MinValue) Then .Item("fechaAsignacionInventario") = itemBodega.FechaAsignacionInventario
                            If itemBodega.IdAlmacenBodega > 0 Then .Item("IdAlmaceBodega") = itemBodega.IdAlmacenBodega
                            If itemBodega.IdOrigenDespacho > 0 Then .Item("idServicioOrigen") = itemBodega.IdOrigenDespacho
                            .Item("idUsuarioModificacion") = idUsuario
                        End With
                        itemsActualizarDataTable.Rows.Add(filaInventario)

                    End If
                Next

                'Eliminación de Items
                If itemsEliminarColeccion.InnerList.Count > 0 Then
                    For Each itemEliminar In itemsEliminarColeccion
                        Remover(itemEliminar)
                    Next

                    With dbManager
                        .SqlParametros.Clear()
                        .SqlParametros.Add("@listaIdRegistro", SqlDbType.VarChar).Value = String.Join(",", itemsEliminarLista.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())

                        .iniciarTransaccion()
                        .ejecutarNonQuery("EliminaItemBodegaSatelite", CommandType.StoredProcedure)
                        .confirmarTransaccion()
                    End With
                End If


                'Adición de Items
                If itemsAgregarDataTable.Rows.Count > 0 Then
                    With dbManager
                        Dim idTransaccion As Long

                        .SqlParametros.Clear()
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .SqlParametros.Add("@idTransaccion", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .ejecutarNonQuery("GeneraTransaccionBodega", CommandType.StoredProcedure)

                        If Long.TryParse(.SqlParametros("@IdTransaccion").Value, idTransaccion) Then
                            .SqlParametros.Clear()
                            .iniciarTransaccion()
                            .inicilizarBulkCopy()

                            With .BulkCopy
                                itemsAgregarDataTable.Columns.Add(New DataColumn("idTransaccion", System.Type.GetType("System.Int64"), idTransaccion))

                                .DestinationTableName = "dbo.InventarioBodegaSatelite"
                                .ColumnMappings.Add("serial", "serial")
                                .ColumnMappings.Add("idProducto", "idProducto")
                                .ColumnMappings.Add("idSubProducto", "idSubProducto")
                                .ColumnMappings.Add("idRegion", "idRegion")
                                .ColumnMappings.Add("idEstado", "idEstado")
                                .ColumnMappings.Add("fechaRecepcion", "fechaRecepcion")
                                .ColumnMappings.Add("cargado", "cargado")
                                .ColumnMappings.Add("nacionalizado", "nacionalizado")
                                .ColumnMappings.Add("termosellado", "termosellado")
                                .ColumnMappings.Add("idBodega", "idBodega")
                                .ColumnMappings.Add("idPosicion", "idPosicion")
                                .ColumnMappings.Add("fechaAsignacionInventario", "fechaAsignacionInventario")
                                .ColumnMappings.Add("idAlmacenBodega", "idAlmacenBodega")
                                .ColumnMappings.Add("idServicioOrigen", "idServicioOrigen")
                                .ColumnMappings.Add("idUsuarioModificacion", "idUsuarioModificacion")
                                .WriteToServer(itemsAgregarDataTable)
                            End With
                            .SqlParametros.Add("@idTransaccion", SqlDbType.BigInt).Value = idTransaccion
                            .ejecutarReader("AgregaItemBodegaSatelite", CommandType.StoredProcedure)

                            If .Reader IsNot Nothing And .Reader.HasRows Then
                                While .Reader.Read()
                                    resultado.Add(New ResultadoProceso(CInt(.Reader.Item("idError")), CStr(.Reader.Item("descripcion"))))
                                End While
                                .Reader.Close()
                                .abortarTransaccion()
                            Else
                                If .Reader IsNot Nothing Then .Reader.Close()
                                .confirmarTransaccion()
                            End If
                        End If

                    End With
                End If

                'Actualización de Items
                If itemsActualizarDataTable.Rows.Count > 0 Then
                    With dbManager
                        Dim idTransaccion As Long

                        .SqlParametros.Clear()
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .SqlParametros.Add("@idTransaccion", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .ejecutarNonQuery("GeneraTransaccionBodega", CommandType.StoredProcedure)

                        If Long.TryParse(.SqlParametros("@IdTransaccion").Value, idTransaccion) Then
                            .SqlParametros.Clear()
                            .iniciarTransaccion()
                            .inicilizarBulkCopy()

                            With .BulkCopy
                                itemsActualizarDataTable.Columns.Add(New DataColumn("idTransaccion", System.Type.GetType("System.Int64"), idTransaccion))

                                .DestinationTableName = "dbo.DetalleTransaccionInventarioBodega"
                                .ColumnMappings.Add("idTransaccion", "idTransaccion")
                                .ColumnMappings.Add("idRegistro", "idRegistro")
                                .ColumnMappings.Add("Serial", "serial")
                                .ColumnMappings.Add("idProducto", "idProducto")
                                .ColumnMappings.Add("idSubProducto", "idSubProducto")
                                .ColumnMappings.Add("idRegion", "idRegion")
                                .ColumnMappings.Add("idEstado", "idEstado")
                                .ColumnMappings.Add("fechaRecepcion", "fechaRecepcion")
                                .ColumnMappings.Add("cargado", "cargado")
                                .ColumnMappings.Add("nacionalizado", "nacionalizado")
                                .ColumnMappings.Add("termosellado", "termosellado")
                                .ColumnMappings.Add("idBodega", "idBodega")
                                .ColumnMappings.Add("idPosicion", "idPosicion")
                                .ColumnMappings.Add("fechaAsignacionInventario", "fechaAsignacionInventario")
                                .ColumnMappings.Add("idAlmacenBodega", "idAlmacenBodega")
                                .ColumnMappings.Add("idOrigenDespacho", "idOrigen")
                                .ColumnMappings.Add("idUsuarioModificacion", "idUsuarioModificacion")
                                .WriteToServer(itemsActualizarDataTable)
                            End With
                            .SqlParametros.Add("@idTransaccion", SqlDbType.BigInt).Value = idTransaccion
                            .ejecutarReader("ActualizaItemBodegaSatelite", CommandType.StoredProcedure)

                            If .Reader IsNot Nothing And .Reader.HasRows Then
                                While .Reader.Read()
                                    resultado.Add(New ResultadoProceso(CInt(.Reader.Item("idError")), CStr(.Reader.Item("descripcion"))))
                                End While
                                .Reader.Close()
                                .abortarTransaccion()
                            Else
                                If .Reader IsNot Nothing Then .Reader.Close()
                                .confirmarTransaccion()
                            End If
                        End If
                    End With
                End If

            Catch ex As Exception
                If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception("Se generó error en [AplicarCambios]", ex)
            End Try
            dbManager.Dispose()
            'End Using
            Return resultado
        End Function

        Public Function ObtieneInventarioAgrupado() As DataTable
            Dim dtDatos As New DataTable
            Using dbManager As New LMDataAccess
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

                        If Not _idClaseSim Is Nothing AndAlso _idClaseSim.Count > 0 Then _
                            .SqlParametros.Add("@listaClaseSim", SqlDbType.VarChar).Value = String.Join(",", _idClaseSim.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())


                        dtDatos = .ejecutarDataTable("ConsultaItemBodegaSateliteAgrupado", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    Throw New Exception("Se generó error en [ObtieneInventarioAgrupado]: " & ex.Message, ex)
                End Try
            End Using
            Return dtDatos
        End Function

#End Region

#Region "Métodos Compartidos"

        Public Shared Function ObtenerTodosEnDataTable() As DataTable
            Dim dtAux As New DataTable

            Return dtAux
        End Function

#End Region

    End Class

End Namespace

