Imports LMDataAccessLayer

Public Class MenusHandler

    Public Shared Function ObtenerMenusTopPorPerfil(idPerfil As Integer) As List(Of MenuTop)
        Dim listaMenu As New List(Of MenuTop)

        Using dbManager As New LMDataAccess
            With dbManager
                .SqlParametros.AddWithValue("@idPerfil", idPerfil)
                .ejecutarReader("ObtenerMenusTopPorPerfil", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    While (.Reader.Read)
                        Dim mnu As New MenuTop

                        mnu.IdMenu = Integer.Parse(.Reader("idMenu").ToString)
                        mnu.Menu = .Reader("menu").ToString
                        mnu.Posicion = Integer.Parse(.Reader("posicion").ToString)

                        listaMenu.Add(mnu)
                    End While
                End If
            End With
        End Using

        Return listaMenu
    End Function

    Public Shared Function ObtenerMenusBack(idMenuPadre As Integer) As DataTable
        Dim dt As DataTable

        Using dbManager As New LMDataAccess
            With dbManager
                .SqlParametros.AddWithValue("@idMenuPadre", idMenuPadre)
                dt = .EjecutarDataTable("ObtenerMenusBack", CommandType.StoredProcedure)
            End With
        End Using

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            dt.Columns.Add(New DataColumn("idMenuPadre", GetType(Integer)))
            Dim idPadre As Integer = 0

            For Each dr As DataRow In dt.Rows

                If CBool(dr("esPadre")) Then
                    idPadre = CInt(dr("idmenu").ToString)
                Else
                    dr("idMenuPadre") = idPadre
                End If
            Next
        End If

        Return dt
    End Function

    Public Shared Function ObtenerUrlMenu(idMenu As Integer, idUsuario As Integer) As String
        Dim url As String = ""

        Using dbManager As New LMDataAccess
            With dbManager
                .SqlParametros.AddWithValue("@idMenu", idMenu)
                .SqlParametros.AddWithValue("@idUsuario", idUsuario)
                .ejecutarReader("ObtenerUrlMenu", CommandType.StoredProcedure)
                If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                    .Reader.Read()
                    url = .Reader("url").ToString

                End If
            End With
        End Using

        Return url
    End Function

    Public Shared Function ObtenerPedidosUsuarioPerfil(idPerfil As Integer, idUsuario As Integer) As DataTable
        Dim dt As DataTable

        Using dbManager As New LMDataAccess
            With dbManager
                .SqlParametros.AddWithValue("@idPerfil", idPerfil)
                .SqlParametros.AddWithValue("@idUsuario", idUsuario)
                dt = .EjecutarDataTable("ObtenerPedidosPendientesPorAtender", CommandType.StoredProcedure)
            End With
        End Using

        Return dt
    End Function

    Public Shared Function ObtenerMenuGeneralDeUsuario(idUsuario As Integer) As List(Of MenuGeneral)
        Dim listaMenu As New List(Of MenuGeneral)

        Using dbManager As New LMDataAccess
            With dbManager
                .SqlParametros.AddWithValue("@idUsuario", idUsuario)
                .ejecutarReader("ObtenerListaMenusGeneral", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    While (.Reader.Read)
                        Dim mnu As New MenuGeneral

                        mnu.IdMenu = Integer.Parse(.Reader("idMenu").ToString)
                        If (Not Convert.IsDBNull(.Reader("idPadre"))) Then mnu.IdPadre = Integer.Parse(.Reader("idPadre").ToString)
                        mnu.Nombre = .Reader("nombre").ToString
                        mnu.Url = .Reader("url").ToString
                        If (Not Convert.IsDBNull(.Reader("posicion"))) Then mnu.Posicion = Integer.Parse(.Reader("posicion").ToString)

                        listaMenu.Add(mnu)
                    End While
                End If
            End With
        End Using

        Return listaMenu
    End Function

End Class
