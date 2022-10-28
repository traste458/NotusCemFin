Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Namespace MensajeriaEspecializada

    Public Class DocumentoServicioMensajeriaColeccion
        Inherits CollectionBase

#Region "Filtros de Búsqueda"

        Private _idDocumento As Integer
        Private _idServicio As Integer
        Private _numRadicado As Long
        Private _Consulta As Integer = 0

        Private _cargado As Boolean

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As DocumentoServicioMensajeria
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As DocumentoServicioMensajeria)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property IdDocumento As Integer
            Get
                Return _idDocumento
            End Get
            Set(value As Integer)
                _idDocumento = value
            End Set
        End Property

        Public Property IdServicio As Integer
            Get
                Return _idServicio
            End Get
            Set(value As Integer)
                _idServicio = value
            End Set
        End Property

        Public Property Radicado As Long
            Get
                Return _numRadicado
            End Get
            Set(value As Long)
                _numRadicado = value
            End Set
        End Property

        Public Property Consulta As Integer
            Get
                Return _Consulta
            End Get
            Set(value As Integer)
                _Consulta = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim objElemento As Type = GetType(DocumentoServicioMensajeria)
            Dim pInfo As PropertyInfo

            For Each pInfo In objElemento.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As DocumentoServicioMensajeria)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As DocumentoServicioMensajeria)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As DocumentoServicioMensajeria)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Function GenerarDataTable() As DataTable
            If Not _cargado Then CargarDatos()
            Dim dtAux As DataTable = CrearEstructuraDeTabla()
            Dim drAux As DataRow
            Dim miRegistro As DocumentoServicioMensajeria

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miRegistro = CType(Me.InnerList(index), DocumentoServicioMensajeria)
                If miRegistro IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(DocumentoServicioMensajeria).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miRegistro, Nothing)
                        End If
                    Next
                    dtAux.Rows.Add(drAux)
                End If
            Next

            Return dtAux
        End Function

        Public Function CargarDocumentos()
            Dim dbManager As New LMDataAccess
            Dim dtDatos As DataTable

            With dbManager
                If IdDocumento > 0 Then .SqlParametros.Add("@idDocumento", SqlDbType.Int).Value = _idDocumento
                If IdServicio > 0 Then .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicio
                If Radicado > 0 Then .SqlParametros.Add("@numRadicado", SqlDbType.BigInt).Value = _numRadicado
                dtDatos = .EjecutarDataTable("ObtenerInfoDocumentoServicioMensajeria", CommandType.StoredProcedure)

                If dtDatos.Rows.Count > 0 Then
                    _cargado = True
                End If

            End With
            If dbManager IsNot Nothing Then dbManager.Dispose()
            Return dtDatos
        End Function
        Public Sub CargarDatos()
            Dim dbManager As New LMDataAccess

            If _cargado Then Me.InnerList.Clear()
            With dbManager
                If IdDocumento > 0 Then .SqlParametros.Add("@idDocumento", SqlDbType.Int).Value = _idDocumento
                If IdServicio > 0 Then .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicio
                If Radicado > 0 Then .SqlParametros.Add("@numRadicado", SqlDbType.BigInt).Value = _numRadicado
                .SqlParametros.Add("@consulta", SqlDbType.Int).Value = _Consulta

                .ejecutarReader("ObtenerInfoDocumentoServicioMensajeria", CommandType.StoredProcedure)

                If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                    Dim objDetalle As DocumentoServicioMensajeria
                    While .Reader.Read
                        objDetalle = New DocumentoServicioMensajeria()
                        objDetalle.CargarResultadoConsulta(.Reader)
                        Me.InnerList.Add(objDetalle)
                    End While
                    _cargado = True
                End If
            End With
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Sub

        Public Function CargarDocumentosRequeridos()
            Dim dbManager As New LMDataAccess
            Dim dtDatos As DataTable
            With dbManager
                .SqlParametros.Clear()
                If Radicado > 0 Then .SqlParametros.Add("@numRadicado", SqlDbType.BigInt).Value = _numRadicado
                dtDatos = .ejecutarDataTable("ObtenerDocumentosServicioMensajeriaLegalizacion", CommandType.StoredProcedure)
            End With
            Return dtDatos
        End Function

#End Region

    End Class

End Namespace

