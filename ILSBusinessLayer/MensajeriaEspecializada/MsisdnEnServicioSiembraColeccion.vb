Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Public Class MsisdnEnServicioSiembraColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Property _idRegistro As Integer
    Property _idRegistroSiembra As Integer
    Property _msisdn As Int64
    Property _tipo As Integer
    Property _idPlan As Integer
    Property _nombrePlan As String
    Property _fechaDevolucion As Date
    Property _material As String
    Property _Descripcionmaterial As String
    Property _idTipoSim As Integer
    Property _tipoSim As String
    Property _materialSim As String
    Property _Descripcionmaterialsim As String
    Property _idRegion As Integer
    Property _region As String
    Property _idPaquete As Integer
    Property _Paquete As String
    Property _idUsuario As Integer
    Property _resultado As Integer
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idUsuario As Long)
        Me.New()
        _idUsuario = idUsuario
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As MsisdnEnServicioSiembraColeccion
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As MsisdnEnServicioSiembraColeccion)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim msisdnEnServicioSiembra As Type = GetType(MsisdnEnServicioSiembraColeccion)
        Dim pInfo As PropertyInfo

        For Each pInfo In msisdnEnServicioSiembra.GetProperties
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
    Public Function GenerarDataTable() As DataTable
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miDetalle As MsisdnEnServicioSiembraColeccion

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miDetalle = CType(Me.InnerList(index), MsisdnEnServicioSiembraColeccion)
            If miDetalle IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(MsisdnEnServicioSiembraColeccion).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(miDetalle, Nothing)
                    End If
                Next
                dtAux.Rows.Add(drAux)
            End If
        Next
        Return dtAux
    End Function

    Public Function GenerarDataTableDesdeLista() As DataTable
        Dim dtAux As DataTable
         dtAux = GenerarDataTable()
        Return dtAux
    End Function

    Public Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            Me.Clear()
            With dbManager
                If Me._idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me._idUsuario
                .TiempoEsperaComando = 0
                .ejecutarReader("ObtenerInfoTransitoriaMinesSiembra", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim elDetalle As MsisdnEnServicioSiembraColeccion
                    While .Reader.Read
                        elDetalle = New MsisdnEnServicioSiembraColeccion
                        elDetalle.CargarResultadoConsulta(.Reader)
                        Me.InnerList.Add(elDetalle)
                    End While
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

    Public Sub ConsultarMsisdn()
        Dim dbManager As New LMDataAccess
        Try
            Me.Clear()
            With dbManager
                If Me._idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me._idUsuario
                If Me._msisdn > 0 Then .SqlParametros.Add("@msisdn", SqlDbType.Decimal).Value = Me._msisdn
                .TiempoEsperaComando = 0
                .ejecutarReader("ObtenerInfoTransitoriaMsisdnSiembra", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    While .Reader.Read
                        CargarResultadoConsulta(.Reader)
                     End While
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub
#End Region

    Public Sub EliminarRegistrosTransitorias()
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                End With
                .TiempoEsperaComando = 0
                .EjecutarDataTable("EliminarTransitoriaCrearserviciosiembra", CommandType.StoredProcedure)
           
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
    End Sub

    Public Sub EliminarMsisdn()
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .Add("@msisdn", SqlDbType.Decimal).Value = _msisdn
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                .TiempoEsperaComando = 0
                .EjecutarDataTable("EliminarTransitoriaMinesSiembra", CommandType.StoredProcedure)
                _resultado = CType(.SqlParametros("@resultado").Value.ToString, Integer)

            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
    End Sub

    Public Sub AdicionarMsisdn()
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    If Me._idUsuario > 0 Then .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    If Me._idRegistro > 0 Then .Add("@idRegistro", SqlDbType.Decimal).Value = _idRegistro
                    If Me._msisdn > 0 Then .Add("@msisdn", SqlDbType.Decimal).Value = _msisdn
                    .Add("@tipo", SqlDbType.Int).Value = _tipo
                    If Me._idPlan > 0 Then .Add("@idPlan", SqlDbType.Int).Value = _idPlan
                    .Add("@nombrePlan", SqlDbType.VarChar).Value = _nombrePlan
                    .Add("@fechaDevolucion", SqlDbType.Date).Value = _fechaDevolucion
                    .Add("@material", SqlDbType.VarChar).Value = _material
                    .Add("@Descripcionmaterial", SqlDbType.VarChar).Value = _Descripcionmaterial
                    If Me._idTipoSim > 0 Then .Add("@idTipoSim", SqlDbType.Int).Value = _idTipoSim
                    .Add("@tipoSim", SqlDbType.VarChar).Value = _tipoSim
                    .Add("@materialSim", SqlDbType.VarChar).Value = _materialSim
                    .Add("@Descripcionmaterialsim", SqlDbType.VarChar).Value = _Descripcionmaterialsim
                    If Me._idRegion > 0 Then .Add("@idRegion", SqlDbType.Int).Value = _idRegion
                    .Add("@region", SqlDbType.VarChar).Value = _region
                    If Me._idPaquete > 0 Then .Add("@idPaquete", SqlDbType.Int).Value = _idPaquete
                    .Add("@Paquete", SqlDbType.VarChar).Value = _Paquete
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("RegistrarTransitoriaMinesSiembra", CommandType.StoredProcedure)
                _resultado = CType(.SqlParametros("@resultado").Value.ToString, Integer)
                
            Catch ex As Exception
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
    End Sub

  
    Public Sub EditarMsisdn()
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    If Me._idUsuario > 0 Then .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    If Me._idRegistro > 0 Then .Add("@idRegistro", SqlDbType.Decimal).Value = _idRegistro
                    If Me._msisdn > 0 Then .Add("@msisdn", SqlDbType.Decimal).Value = _msisdn
                    .Add("@tipo", SqlDbType.Int).Value = _tipo
                    If Me._idPlan > 0 Then .Add("@idPlan", SqlDbType.Int).Value = _idPlan
                    .Add("@nombrePlan", SqlDbType.VarChar).Value = _nombrePlan
                    .Add("@fechaDevolucion", SqlDbType.Date).Value = _fechaDevolucion
                    .Add("@material", SqlDbType.VarChar).Value = _material
                    .Add("@Descripcionmaterial", SqlDbType.VarChar).Value = _Descripcionmaterial
                    If Me._idTipoSim > 0 Then .Add("@idTipoSim", SqlDbType.Int).Value = _idTipoSim
                    .Add("@tipoSim", SqlDbType.VarChar).Value = _tipoSim
                    .Add("@materialSim", SqlDbType.VarChar).Value = _materialSim
                    .Add("@Descripcionmaterialsim", SqlDbType.VarChar).Value = _Descripcionmaterialsim
                    If Me._idRegion > 0 Then .Add("@idRegion", SqlDbType.Int).Value = _idRegion
                    .Add("@region", SqlDbType.VarChar).Value = _region
                    If Me._idPaquete > 0 Then .Add("@idPaquete", SqlDbType.Int).Value = _idPaquete
                    .Add("@Paquete", SqlDbType.VarChar).Value = _Paquete
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("ActualizarTransitoriaMinesSiembra", CommandType.StoredProcedure)
                _resultado = CType(.SqlParametros("@resultado").Value.ToString, Integer)

            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
    End Sub

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                If Not IsDBNull(reader("idRegistro")) Then _idRegistro = CInt(reader("idRegistro"))
                If Not IsDBNull(reader("idRegistroSiembra")) Then _idRegistroSiembra = CInt(reader("idRegistroSiembra"))
                If Not IsDBNull(reader("msisdn")) Then _msisdn = Convert.ToInt64(reader("msisdn"))
                If Not IsDBNull(reader("tipo")) Then _tipo = CInt(reader("tipo"))
                If Not IsDBNull(reader("idPlan")) Then _idPlan = CInt(reader("idPlan"))
                If Not IsDBNull(reader("nombrePlan")) Then _nombrePlan = reader("nombrePlan").ToString()
                If Not IsDBNull(reader("fechaDevolucion")) Then _fechaDevolucion = CDate(reader("fechaDevolucion"))
                If Not IsDBNull(reader("material")) Then _material = reader("material").ToString
                If Not IsDBNull(reader("Descripcionmaterial")) Then _Descripcionmaterial = reader("Descripcionmaterial").ToString
                If Not IsDBNull(reader("idTipoSim")) Then _idTipoSim = CInt(reader("idTipoSim"))
                If Not IsDBNull(reader("tipoSim")) Then _tipoSim = reader("tipoSim").ToString
                If Not IsDBNull(reader("Descripcionmaterialsim")) Then _Descripcionmaterialsim = reader("Descripcionmaterialsim").ToString
                If Not IsDBNull(reader("idRegion")) Then _idRegion = CInt(reader("idRegion"))
                If Not IsDBNull(reader("region")) Then _region = reader("region").ToString
                If Not IsDBNull(reader("idPaquete")) Then _idPaquete = CInt(reader("idPaquete"))
                If Not IsDBNull(reader("Paquete")) Then _Paquete = reader("Paquete").ToString
                If Not IsDBNull(reader("idUsuario")) Then _idUsuario = CInt(reader("idUsuario"))
              
            End If
        End If

    End Sub

#End Region
End Class
