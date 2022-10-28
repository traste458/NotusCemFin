Imports LMDataAccessLayer

Public Class CargueArchivosCEM

#Region "atributos"

#End Region

#Region "Propiedades"

#End Region

#Region "Metodos"

    Public Sub CargarArchivo(ByVal rutaFisica As String, ByVal Aprobado As Boolean)
        'Dim adminArchivo As New AdministradorArchivos
        'adminArchivo.TipoArchivo = _tipoArchivoCarga
        'Dim ds As DataSet = adminArchivo.cargarArchivosFacturacion(rutaFisica, _tipoArchivoCarga)
        'If Aprobado = False Then
        '    If Not adminArchivo.ContieneErrores Then
        '        InsertarRegistrosBds(ds)
        '    Else
        '        _dtErrores = adminArchivo.ListaErrores
        '        If Not ds.Tables("dtRegistros") Is Nothing Then
        '            _cntregistros = ds.Tables("dtRegistros").Rows.Count
        '        End If
        '    End If
        'Else
        '    InsertarRegistrosBds(ds)
        'End If
    End Sub

#End Region

End Class
