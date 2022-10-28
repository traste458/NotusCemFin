Imports ILSBusinessLayer
Imports ILSBusinessLayer.Inventario
Imports System.Reflection
Imports System.Collections.Generic

Partial Public Class PruebaInventario
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'PruebaItemBodegaPrincipal()
        'PruebaInventarioBodegaPrincipalColeccion()
        'PruebaItemBodegaSatelite()
        'PruebaInventarioBodegaSateliteColeccion()

        PruebaBloqueoInventario()
    End Sub

    Public Sub PruebaItemBodegaPrincipal()
        Dim itemPrincipal As New ItemBodegaPrincipal()

        For Each mi As MemberInfo In GetType(ItemBodegaPrincipal).GetMembers()
            If mi.MemberType = MemberTypes.Property Then
                Dim pi As PropertyInfo = TryCast(mi, PropertyInfo)

                If pi IsNot Nothing Then
                    Response.Write("<b>" + pi.Name + "</b>: " + TryCast(pi.GetValue(itemPrincipal, Nothing), String) + "<br>")
                End If
            End If
        Next

    End Sub

    Public Sub PruebaInventarioBodegaPrincipalColeccion()
        Dim coleccion As New InventarioBodegaPrincipalColeccion

        'Filtros
        'coleccion.Serial = New List(Of String)(New String() {"352010046373984"})

        'coleccion.IdProducto = New List(Of Integer)(New Integer() {213, 421})

        'coleccion.FechaRecepcionInicial = New Date(2011, 12, 16)
        'coleccion.FechaRecepcionFinal = New Date(2011, 5, 18)

        coleccion.IdRegistro = New List(Of Long)(New Long() {3060})

        'Consulta
        coleccion.CargarDatos()

        ''Adicion
        'coleccion.Adicionar(New ItemBodegaPrincipal With {.Serial = "110932002486311", _
        '                                                  .IdProducto = 213, _
        '                                                  .IdSubProducto = 2491, _
        '                                                  .IdRegion = 1, _
        '                                                  .IdEstado = 1, _
        '                                                  .FechaRecepcion = Now(), _
        '                                                  .Cargado = False, _
        '                                                  .Nacionalizado = False, _
        '                                                  .Termosellado = False, _
        '                                                  .IdBodega = 4, _
        '                                                  .FechaAsignacionInventario = Now(), _
        '                                                  .IdUsuarioModificacion = 1, _
        '                                                  .Accion = Enumerados.AccionItem.Adicionar})

        'Eliminar
        'coleccion.RemoverDe(coleccion.IndiceDe(2013))
        coleccion.Remover(New ItemBodegaPrincipal(3060))

        ''Agregar colección
        'Dim objColeccionAdd As New InventarioBodegaPrincipalColeccion()
        'objColeccionAdd.Adicionar(New ItemBodegaPrincipal With {.Serial = "810932002486099", _
        '                                                  .IdProducto = 213, _
        '                                                  .IdSubProducto = 2491, _
        '                                                  .IdRegion = 1, _
        '                                                  .IdEstado = 1, _
        '                                                  .FechaRecepcion = Date.Now(), _
        '                                                  .Cargado = False, _
        '                                                  .Nacionalizado = False, _
        '                                                  .Termosellado = False, _
        '                                                  .IdBodega = 4, _
        '                                                  .FechaAsignacionInventario = Now(), _
        '                                                  .IdUsuarioModificacion = 1, _
        '                                                  .Accion = Enumerados.AccionItem.Adicionar})
        'objColeccionAdd.Adicionar(New ItemBodegaPrincipal With {.Serial = "910932002486199", _
        '                                                  .IdProducto = 213, _
        '                                                  .IdSubProducto = 2491, _
        '                                                  .IdRegion = 1, _
        '                                                  .IdEstado = 1, _
        '                                                  .FechaRecepcion = Now, _
        '                                                  .Cargado = False, _
        '                                                  .Nacionalizado = False, _
        '                                                  .Termosellado = False, _
        '                                                  .IdBodega = 4, _
        '                                                  .FechaAsignacionInventario = Now, _
        '                                                  .IdUsuarioModificacion = 1, _
        '                                                  .Accion = Enumerados.AccionItem.Adicionar})

        'coleccion.AdicionarRango(objColeccionAdd)

        ''Actualizar
        'For Each i As ItemBodegaPrincipal In coleccion
        '    i.Accion = Enumerados.AccionItem.Actualizar
        '    i.FechaRecepcion = i.FechaRecepcion.AddMinutes(60)
        '    i.IdBodega = 1
        'Next


        coleccion.AplicarCambios(1)

        Dim count As Integer = 0
        For Each myItem As ItemBodegaPrincipal In coleccion
            count = count + 1
            'Response.Write("Id: " + count.ToString() + "- Serial: " + myItem.Serial + "- Registrado: " + myItem.Registrado.ToString() + "<br>")
            itemsBulletedList.Items.Add("Id: " + count.ToString() + "- Serial: " + myItem.Serial + "- Registrado: " + myItem.Registrado.ToString())
        Next


        itemsGridView.DataSource = coleccion.GenerarDataTable()
        itemsGridView.DataBind()


    End Sub


    Public Sub PruebaItemBodegaSatelite()
        Dim itemSatelite As New ItemBodegaSatelite("359359006655671")

        For Each mi As MemberInfo In GetType(ItemBodegaSatelite).GetMembers()
            If mi.MemberType = MemberTypes.Property Then
                Dim pi As PropertyInfo = TryCast(mi, PropertyInfo)

                If pi IsNot Nothing Then
                    Response.Write("<b>" + pi.Name + "</b>: " + TryCast(pi.GetValue(itemSatelite, Nothing), String) + "<br>")
                End If
            End If
        Next

    End Sub

    Public Sub PruebaInventarioBodegaSateliteColeccion()
        Dim coleccion As New InventarioBodegaSateliteColeccion

        'Filtros
        coleccion.Serial = New List(Of String)(New String() {"571010004080342"})

        'coleccion.IdProducto = New List(Of Integer)(New Integer() {319, 148, 58})

        'coleccion.FechaRecepcionInicial = New Date(2011, 12, 19)
        'coleccion.FechaRecepcionFinal = New Date(2011, 5, 18)

        'Consulta
        coleccion.CargarDatos()

        'coleccion.Adicionar(New ItemBodegaSatelite With {.Serial = "110932002486308", _
        '                                                  .IdProducto = 213, _
        '                                                  .IdSubProducto = 2491, _
        '                                                  .IdRegion = 1, _
        '                                                  .IdEstado = 1, _
        '                                                  .FechaRecepcion = Now(), _
        '                                                  .Cargado = False, _
        '                                                  .Nacionalizado = False, _
        '                                                  .Termosellado = False, _
        '                                                  .IdBodega = 4, _
        '                                                  .FechaAsignacionInventario = Now(), _
        '                                                  .IdUsuarioModificacion = 1, _
        '                                                  .Accion = Enumerados.AccionItem.Adicionar})

        'coleccion.RemoverDe(coleccion.IndiceDe(1006))
        'coleccion.Remover(New ItemBodegaSatelite(1005))

        'Agreagar colección
        'Dim objColeccionAdd As New InventarioBodegaSateliteColeccion()
        'objColeccionAdd.Adicionar(New ItemBodegaSatelite With {.Serial = "810932002486308", _
        '                                                  .IdProducto = 213, _
        '                                                  .IdSubProducto = 2491, _
        '                                                  .IdRegion = 1, _
        '                                                  .IdEstado = 1, _
        '                                                  .FechaRecepcion = Date.Now(), _
        '                                                  .Cargado = False, _
        '                                                  .Nacionalizado = False, _
        '                                                  .Termosellado = False, _
        '                                                  .IdBodega = 4, _
        '                                                  .FechaAsignacionInventario = Now(), _
        '                                                  .IdUsuarioModificacion = 1, _
        '                                                  .Accion = Enumerados.AccionItem.Adicionar})
        'objColeccionAdd.Adicionar(New ItemBodegaSatelite With {.Serial = "910932002486308", _
        '                                                  .IdProducto = 213, _
        '                                                  .IdSubProducto = 2491, _
        '                                                  .IdRegion = 1, _
        '                                                  .IdEstado = 1, _
        '                                                  .FechaRecepcion = Now, _
        '                                                  .Cargado = False, _
        '                                                  .Nacionalizado = False, _
        '                                                  .Termosellado = False, _
        '                                                  .IdBodega = 4, _
        '                                                  .FechaAsignacionInventario = Now, _
        '                                                  .IdUsuarioModificacion = 1, _
        '                                                  .Accion = Enumerados.AccionItem.Adicionar})

        'coleccion.AdicionarRango(objColeccionAdd)

        'Actualizar
        For Each i As ItemBodegaSatelite In coleccion
            i.Accion = Enumerados.AccionItem.Actualizar
            i.FechaRecepcion = i.FechaRecepcion.AddMinutes(60)
            i.IdBodega = 1
        Next

        coleccion.AplicarCambios(1)

        Dim count As Integer = 0
        For Each myItem As ItemBodegaSatelite In coleccion
            count = count + 1
            'Response.Write("Id: " + count.ToString() + "- Serial: " + myItem.Serial + "- Registrado: " + myItem.Registrado.ToString() + "<br>")
            itemsBulletedList.Items.Add("Id: " + count.ToString() + "- Serial: " + myItem.Serial + "- Registrado: " + myItem.Registrado.ToString())
        Next


        itemsGridView.DataSource = coleccion.GenerarDataTable()
        itemsGridView.DataBind()


    End Sub


    Public Sub PruebaBloqueoInventario()
        'Dim objBloqueoInventario As New BloqueoInventario(40, Nothing, 1, 97, Now(), Nothing, 1, 1, 1, "Prueba: " & CStr(New Random().NextDouble()))
        Dim objBloqueoInventario As New BloqueoInventario()

        ''Agregar bloqueo producto
        'Dim detalleProducto As New DetalleProductoBloqueo()
        'detalleProducto.IdProducto = 213 '658
        'detalleProducto.Material = "5085" '"12206"
        'detalleProducto.Cantidad = 1
        'objBloqueoInventario.ProductoBloqueoColeccion.Adicionar(detalleProducto)

        ''Agregar bloqueo serial
        'Dim detalleSerial As New DetalleSerialBloqueo
        'detalleSerial.Serial = "351689054878330"
        'objBloqueoInventario.SerialBloqueoColeccion.Adicionar(detalleSerial)

        'objBloqueoInventario.SerialBloqueoColeccion.Adicionar(New DetalleSerialBloqueo("352687041724871"))
        'objBloqueoInventario.SerialBloqueoColeccion.Adicionar(New DetalleSerialBloqueo("352687041724921"))
        'objBloqueoInventario.SerialBloqueoColeccion.Adicionar(New DetalleSerialBloqueo("352687041725449"))
        'objBloqueoInventario.SerialBloqueoColeccion.Adicionar(New DetalleSerialBloqueo("352687041725464"))

        'Desbloquea un serial
        Dim objDetalleSerial As New DetalleSerialBloqueoColeccion()
        objDetalleSerial.Adicionar(New DetalleSerialBloqueo("57101000704133452"))


        'Dim resultado As ResultadoProceso = objBloqueoInventario.Registrar()
        Dim resultado As ResultadoProceso = objBloqueoInventario.DesbloquearSerial(objDetalleSerial)
        Response.Write("Se generó el bloqueo: " & resultado.Mensaje)

    End Sub
End Class