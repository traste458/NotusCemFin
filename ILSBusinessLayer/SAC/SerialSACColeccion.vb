Imports LMDataAccessLayer
Imports ARBusinessLayer.Enumerados

Namespace SAC

    Public Class SerialSACColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _serial As ArrayList
        Private _idCaso As ArrayList
        Private _idPos As ArrayList
        Private _idCoordinador As ArrayList
        Private _idSupervisor As ArrayList
        Private _idTipoSerial As Short
        Private _fechaRegistroInicial As Date
        Private _fechaRegistroFinal As Date
#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idCaso As Integer)
            MyBase.New()
            _idCaso = New ArrayList
            _idCaso.Add(idCaso)
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As SerialCasoSAC
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As SerialCasoSAC)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public ReadOnly Property Serial() As ArrayList
            Get
                If _serial Is Nothing Then _serial = New ArrayList
                Return _serial
            End Get
        End Property

        Public ReadOnly Property IdCaso() As ArrayList
            Get
                If _idCaso Is Nothing Then _idCaso = New ArrayList
                Return _idCaso
            End Get
        End Property

        Public ReadOnly Property IdPos() As ArrayList
            Get
                If _idPos Is Nothing Then _idPos = New ArrayList
                Return _idPos
            End Get
        End Property

        Public ReadOnly Property IdCoordinador() As ArrayList
            Get
                If _idCoordinador Is Nothing Then _idCoordinador = New ArrayList
                Return _idCoordinador
            End Get
        End Property

        Public ReadOnly Property IdSupervisor() As ArrayList
            Get
                If _idSupervisor Is Nothing Then _idSupervisor = New ArrayList
                Return _idSupervisor
            End Get
        End Property

        Public Property IdTipoSerial() As Short
            Get
                Return _idTipoSerial
            End Get
            Set(ByVal value As Short)
                _idTipoSerial = value
            End Set

        End Property

        Public Property FechaRegistroInicial() As Date
            Get
                Return _fechaRegistroInicial
            End Get
            Set(ByVal value As Date)
                _fechaRegistroInicial = value
            End Set
        End Property

        Public Property FechaRegistroFinal() As Date
            Get
                Return _fechaRegistroFinal
            End Get
            Set(ByVal value As Date)
                _fechaRegistroFinal = value
            End Set
        End Property

#End Region

#Region "Métodos Públicos"

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As SerialCasoSAC)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As SerialCasoSAC)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As SerialSACColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As SerialCasoSAC)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function ExisteSerial(ByVal serial As String) As Boolean
            Dim existe As Boolean = False
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), SerialCasoSAC)
                    If .Serial.Trim = serial.Trim Then
                        existe = True
                        Exit For
                    End If
                End With
            Next
            Return existe
        End Function

        Public Function IndiceDe(ByVal serial As String) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), SerialCasoSAC)
                    If .Serial.Trim = serial.Trim Then
                        indice = index
                        Exit For
                    End If
                End With
            Next
            Return indice
        End Function

        Public Function GenerarDataTable() As DataTable
            Dim dtAux As New DataTable
            Dim drAux As DataRow
            Dim elSerial As SerialCasoSAC

            With dtAux
                .Columns.Add("idSerial", GetType(Integer))
                .Columns.Add("idCaso", GetType(Integer))
                .Columns.Add("serial", GetType(String))
                .Columns.Add("idTipoSerial", GetType(Short))
                .Columns.Add("idPos", GetType(Integer))
                .Columns.Add("pos", GetType(String))
                .Columns.Add("idCoordinador", GetType(Integer))
                .Columns.Add("coordinador", GetType(String))
                .Columns.Add("idSupervisor", GetType(Integer))
                .Columns.Add("supervisor", GetType(String))
                .Columns.Add("fechaRegistro", GetType(Date))
            End With

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                elSerial = CType(Me.InnerList(index), SerialCasoSAC)
                If elSerial IsNot Nothing Then
                    With elSerial
                        drAux("idSerial") = .IdSerial
                        drAux("idCaso") = .IdCaso
                        drAux("serial") = .Serial
                        drAux("idTipoSerial") = .IdTipoSerial
                        drAux("idPos") = .IdPos
                        drAux("pos") = .PDV
                        drAux("idCoordinador") = .IdCoordinador
                        drAux("coordinador") = .Coordinador
                        drAux("idSupervisor") = .IdSupervisor
                        drAux("supervisor") = .Supervisor
                        drAux("fechaRegistro") = .FechaRegistro
                        dtAux.Rows.Add(drAux)
                    End With

                End If
            Next

            Return dtAux
        End Function

        Public Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                Me.Clear()
                With dbManager
                    If Me._serial IsNot Nothing AndAlso Me._serial.Count > 0 Then _
                        .SqlParametros.Add("@listaSerial", SqlDbType.VarChar, 8000).Value = Join(Me._serial.ToArray, ",")
                    If Me._idCaso IsNot Nothing AndAlso Me._idCaso.Count > 0 Then _
                        .SqlParametros.Add("@listaIdCaso", SqlDbType.VarChar, 1000).Value = Join(Me._idCaso.ToArray, ",")
                    If Me._idPos IsNot Nothing AndAlso Me._idPos.Count > 0 Then _
                        .SqlParametros.Add("@listaIdPos", SqlDbType.VarChar, 8000).Value = Join(Me._idPos.ToArray, ",")
                    If Me._idCoordinador IsNot Nothing AndAlso Me._idCoordinador.Count > 0 Then _
                        .SqlParametros.Add("@listaIdCoordinador", SqlDbType.VarChar, 1000).Value = Join(Me._idCoordinador.ToArray, ",")
                    If Me._idSupervisor IsNot Nothing AndAlso Me._idSupervisor.Count > 0 Then _
                        .SqlParametros.Add("@listaIdSupervisor", SqlDbType.VarChar, 1000).Value = Join(Me._idSupervisor.ToArray, ",")
                    If Me._idTipoSerial > 0 Then .SqlParametros.Add("@idTipoSerial", SqlDbType.SmallInt).Value = Me._idTipoSerial
                    If Me._fechaRegistroInicial > Date.MinValue OrElse Me._fechaRegistroFinal > Date.MinValue Then
                        If Me._fechaRegistroInicial > Date.MinValue And Me._fechaRegistroFinal = Date.MinValue _
                            Then Me._fechaRegistroFinal = Me._fechaRegistroInicial
                        If Me._fechaRegistroInicial = Date.MinValue And Me._fechaRegistroFinal > Date.MinValue _
                            Then Me._fechaRegistroInicial = Me._fechaRegistroFinal

                        .SqlParametros.Add("@fechaRegistroInicial", SqlDbType.SmallDateTime).Value = Me._fechaRegistroInicial
                        .SqlParametros.Add("@fechaRegistroFinal", SqlDbType.SmallDateTime).Value = Me._fechaRegistroFinal
                    End If
                    .ejecutarReader("ConsultarSerialCasoSAC", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim elSerial As SerialCasoSAC
                        Dim idSerial As Integer
                        Dim idPos As Integer
                        Dim idCoordinador As Integer
                        Dim idSupervisor As Integer
                        Dim fechaRegistro As Date

                        While .Reader.Read
                            elSerial = New SerialCasoSAC
                            Integer.TryParse(.Reader("idSerial").ToString, idSerial)
                            elSerial.EstablecerIdentificador(idSerial)
                            Integer.TryParse(.Reader("idCaso").ToString, elSerial.IdCaso)
                            elSerial.Serial = .Reader("serial").ToString
                            Short.TryParse(.Reader("idTipoSerial"), elSerial.IdTipoSerial)
                            Integer.TryParse(.Reader("idPos").ToString, idPos)
                            elSerial.EstablecerIdPos(idPos)
                            elSerial.EstablecerPDV(.Reader("pos").ToString)
                            Integer.TryParse(.Reader("idCoordinador").ToString, idCoordinador)
                            elSerial.EstablecerIdCoordinador(idCoordinador)
                            elSerial.EstablecerCoordinador(.Reader("coordinador").ToString)
                            Integer.TryParse(.Reader("idSupervisor").ToString, idSupervisor)
                            elSerial.EstablecerIdSupervisor(idSupervisor)
                            elSerial.EstablecerSupervisor(.Reader("supervisor").ToString)
                            Date.TryParse(.Reader("fechaRegistro").ToString, fechaRegistro)
                            elSerial.EstablecerFechaRegistro(fechaRegistro)
                            elSerial.MarcarComoRegistrado()

                            Me.InnerList.Add(elSerial)
                        End While
                        .Reader.Close()
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

#End Region

#Region "Métodos Compartidos"

        Public Shared Function ObtenerTodosEnDataTable() As DataTable
            Dim dtAux As New DataTable
            Dim dbManager As New LMDataAccess

            Try
                dtAux = dbManager.EjecutarDataTable("ConsultarSerialCasoSAC", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtAux
        End Function

#End Region

    End Class

End Namespace
