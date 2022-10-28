Imports LMDataAccessLayer

Namespace MensajeriaEspecializada.OfficeTrack

    Public Class OfficeTrackTask

#Region "Campos"

        Private _IdTask As Long
        Private _IdDetalle As Long
        Private _IdEntry As Integer
        Private _TaskNumber As Long
        Private _StatusID As Short
        Private _StatusName As String
        Private _TaskTypeCode As Integer
        Private _Duration As Short
        Private _Notes As String
        Private _Description As String
        Private _Categories As String
        Private _ContactName As String
        Private _EmployeeNumber As String
        Private _CustomerName As String
        Private _Phone1 As String
        Private _Phone2 As String
        Private _LocationAdressAs As Location
        Private _StartDate As DateTime
        Private _DueDate As DateTime = DateTime.MinValue
        Private _StartDateFromEpoch As DateTime
        Private _ExternalCreationDate As DateTime = DateTime.MinValue
        Private _FechaEntregaDevolucion As DateTime = DateTime.MinValue
        Private _IdNovedadDevolucion As Integer = Integer.MinValue
        Private _SendToOfficeTrack As Boolean
        Private _Observacion As String

#End Region

#Region "Propiedades"


        Public Property Observacion() As String
            Get
                Return _Observacion
            End Get
            Set(ByVal value As String)
                _Observacion = value
            End Set
        End Property


        Public Property IdNovedadDevolucion() As Integer
            Get
                Return _IdNovedadDevolucion
            End Get
            Set(ByVal value As Integer)
                _IdNovedadDevolucion = value
            End Set
        End Property


        Public Property SendToOfficeTrack() As Boolean
            Get
                Return _SendToOfficeTrack
            End Get
            Set(ByVal value As Boolean)
                _SendToOfficeTrack = value
            End Set
        End Property


        Public Property ExternalCreationDate() As DateTime
            Get
                Return _ExternalCreationDate
            End Get
            Set(ByVal value As DateTime)
                _ExternalCreationDate = value
            End Set
        End Property

        Public Property FechaEntregaDevolucion() As DateTime
            Get
                Return _FechaEntregaDevolucion
            End Get
            Set(ByVal value As DateTime)
                _FechaEntregaDevolucion = value
            End Set
        End Property



        Public Property DueDate() As DateTime
            Get
                Return _DueDate
            End Get
            Set(ByVal value As DateTime)
                _DueDate = value
            End Set
        End Property

        Public Property LocationAdress() As Location
            Get
                Return _LocationAdressAs
            End Get
            Set(ByVal value As Location)
                _LocationAdressAs = value
            End Set
        End Property

        Public Property EmployeeNumber() As String
            Get
                Return _EmployeeNumber
            End Get
            Set(ByVal value As String)
                _EmployeeNumber = value
            End Set
        End Property

        Public Property TaskTypeCode() As Integer
            Get
                Return _TaskTypeCode
            End Get
            Set(ByVal value As Integer)
                _TaskTypeCode = value
            End Set
        End Property

        Public Property IdTask() As Long
            Get
                Return _IdTask
            End Get
            Set(ByVal value As Long)
                _IdTask = value
            End Set
        End Property

        Public Property IdDetalle() As Long
            Get
                Return _IdDetalle
            End Get
            Set(ByVal value As Long)
                _IdDetalle = value
            End Set
        End Property

        Public Property Duration() As Short
            Get
                Return _Duration
            End Get
            Set(ByVal value As Short)
                _Duration = value
            End Set
        End Property

        Public Property IdEntry() As Integer
            Get
                Return _IdEntry
            End Get
            Set(ByVal value As Integer)
                _IdEntry = value
            End Set
        End Property

        Public Property TaskNumber() As String
            Get
                Return _TaskNumber
            End Get
            Set(ByVal value As String)
                _TaskNumber = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal value As String)
                _Description = value
            End Set
        End Property

        Public Property Status() As Short
            Get
                Return _StatusID
            End Get
            Set(ByVal value As Short)
                _StatusID = value
            End Set
        End Property

        Public Property StatusName() As String
            Get
                Return _StatusName
            End Get
            Set(ByVal value As String)
                _StatusName = value
            End Set
        End Property

        Public Property Notes() As String
            Get
                Return _Notes
            End Get
            Set(ByVal value As String)
                _Notes = value
            End Set
        End Property

        Public Property ContactName() As String
            Get
                Return _ContactName
            End Get
            Set(ByVal value As String)
                _ContactName = value
            End Set
        End Property

        Public Property Phone1() As String
            Get
                Return _Phone1
            End Get
            Set(ByVal value As String)
                _Phone1 = value
            End Set
        End Property

        Public Property Phone2() As String
            Get
                Return _Phone2
            End Get
            Set(ByVal value As String)
                _Phone2 = value
            End Set
        End Property

        Public Property CustomerName() As String
            Get
                Return _CustomerName
            End Get
            Set(ByVal value As String)
                _CustomerName = value
            End Set
        End Property

        Public Property Categories() As String
            Get
                Return _Categories
            End Get
            Set(ByVal value As String)
                _Categories = value
            End Set
        End Property

        Public Property StartDate() As DateTime
            Get
                Return _StartDate
            End Get
            Set(ByVal value As DateTime)
                _StartDate = value
            End Set
        End Property

        Public Property StartDateFromEpoch() As DateTime
            Get
                Return _StartDateFromEpoch
            End Get
            Set(ByVal value As DateTime)
                _StartDateFromEpoch = value
            End Set
        End Property
#End Region

#Region "Contructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()


        End Sub


#End Region

#Region "Metodos Publicos"



        Public Function RegistrarTarea(dbManager As LMDataAccess) As Short
            Dim respuesta As Short
            If dbManager IsNot Nothing Then
                With dbManager
                    .SqlParametros.Clear()

                    .SqlParametros.Add("@idDetalle", SqlDbType.BigInt).Value = IdDetalle
                    If _IdEntry <= 0 Then
                        .SqlParametros.Add("@IdEntry", SqlDbType.Int).Value = DBNull.Value
                    Else
                        .SqlParametros.Add("@IdEntry", SqlDbType.Int).Value = IdEntry
                    End If

                    .SqlParametros.Add("@TaskNumber", SqlDbType.BigInt).Value = _TaskNumber
                    .SqlParametros.Add("@StatusID", SqlDbType.SmallInt).Value = _StatusID
                    .SqlParametros.Add("@TaskTypeCode", SqlDbType.Int).Value = _TaskTypeCode
                    .SqlParametros.Add("@Duration", SqlDbType.SmallInt).Value = Duration
                    .SqlParametros.Add("@Notes", SqlDbType.VarChar).Value = _Notes
                    .SqlParametros.Add("@Description", SqlDbType.VarChar).Value = Description
                    .SqlParametros.Add("@Categories", SqlDbType.VarChar).Value = _Categories
                    .SqlParametros.Add("@ContactName", SqlDbType.VarChar).Value = _ContactName
                    .SqlParametros.Add("@EmployeeNumber", SqlDbType.VarChar).Value = _EmployeeNumber
                    .SqlParametros.Add("@CustomerName", SqlDbType.VarChar).Value = _CustomerName
                    .SqlParametros.Add("@Phone1", SqlDbType.VarChar).Value = _Phone1
                    .SqlParametros.Add("@Phone2", SqlDbType.VarChar).Value = _Phone2
                    .SqlParametros.Add("@LocationAdress", SqlDbType.VarChar).Value = LocationAdress.Adress
                    .SqlParametros.Add("@StartDate", SqlDbType.DateTime).Value = _StartDate
                    If (_DueDate <> DateTime.MinValue) Then
                        .SqlParametros.Add("@DueDate", SqlDbType.DateTime).Value = _DueDate
                    End If
                    '.SqlParametros.Add("@StartDateFromEpoch", SqlDbType.DateTime).Value = _StartDateFromEpoch
                    If (ExternalCreationDate <> DateTime.MinValue) Then
                        .SqlParametros.Add("@ExternalCreationDate", SqlDbType.DateTime).Value = ExternalCreationDate
                    End If
                    If (FechaEntregaDevolucion <> DateTime.MinValue) Then
                        .SqlParametros.Add("@FechaEntregaDevolucion", SqlDbType.DateTime).Value = FechaEntregaDevolucion
                    End If
                    If (IdNovedadDevolucion <> Integer.MinValue) Then
                        .SqlParametros.Add("@IdNovedadDevolucion", SqlDbType.Int).Value = IdNovedadDevolucion
                    End If
                    If (_Observacion <> String.Empty) Then
                        .SqlParametros.Add("@observacion", SqlDbType.VarChar).Value = _Observacion
                    End If
                    .SqlParametros.Add("@SendToOfficeTrack", SqlDbType.Bit).Value = _SendToOfficeTrack
                    .SqlParametros.Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .EjecutarNonQuery("CrearOfficeTrackTask", CommandType.StoredProcedure)
                    respuesta = CShort(.SqlParametros("@result").Value)
                    'If respuesta = 0 Then _codigo = .SqlParametros("@TaskNumber").Value
                End With
            End If
            Return respuesta
        End Function

        Public Function ObtenerTareaServicio(ByVal IdServicioMensajeria As Integer)
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@IdServicio", SqlDbType.Int).Value = IdServicioMensajeria
                    .ejecutarReader("ConsultarOfficeTrackTaskUltimaTareaServicioMensajeria", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            If .Reader IsNot Nothing Then
                                If .Reader.HasRows Then
                                    Long.TryParse(.Reader("IdTask"), _IdTask)
                                    Long.TryParse(.Reader("IdDetalle"), _IdDetalle)
                                    'If Not IsDBNull(.Reader("IdEntry")) Then
                                    If Not IsDBNull(.Reader("IdEntry")) Then IdEntry = Integer.Parse(.Reader("IdEntry"))
                                    If Not IsDBNull(.Reader("TaskNumber")) Then TaskNumber = .Reader("TaskNumber")
                                    If Not IsDBNull(.Reader("StatusID")) Then Status = Short.Parse(.Reader("StatusID"))
                                    If Not IsDBNull(.Reader("StatusName")) Then StatusName = .Reader("StatusName")
                                    If Not IsDBNull(.Reader("TaskTypeCode")) Then TaskTypeCode = Integer.Parse(.Reader("TaskTypeCode"))
                                    If Not IsDBNull(.Reader("Duration")) Then Duration = Short.Parse(.Reader("Duration"))
                                    If Not IsDBNull(.Reader("Notes")) Then Notes = .Reader("Notes")
                                    If Not IsDBNull(.Reader("Description")) Then Description = .Reader("Description")
                                    'If Not IsDBNull(.Reader("Categories")) Then Categories = .Reader("Categories")
                                    If Not IsDBNull(.Reader("ContactName")) Then ContactName = .Reader("ContactName")
                                    If Not IsDBNull(.Reader("EmployeeNumber")) Then EmployeeNumber = .Reader("EmployeeNumber")
                                    If Not IsDBNull(.Reader("CustomerName")) Then CustomerName = .Reader("CustomerName")
                                    If Not IsDBNull(.Reader("Phone1")) Then Phone1 = .Reader("Phone1")
                                    If Not IsDBNull(.Reader("Phone2")) Then Phone2 = .Reader("Phone2")
                                    'If Not IsDBNull(.Reader("LocationAdress")) Then LocationAdress.Adress = .Reader("LocationAdress")
                                    If Not IsDBNull(.Reader("StartDate")) Then StartDate = DateTime.Parse(.Reader("StartDate"))
                                    If Not IsDBNull(.Reader("DueDate")) Then DueDate = DateTime.Parse(.Reader("DueDate"))
                                    'If Not IsDBNull(.Reader("StartDateFromEpoch")) Then StartDateFromEpoch = .Reader("StartDateFromEpoch")
                                    If Not IsDBNull(.Reader("ExternalCreationDate")) Then ExternalCreationDate = DateTime.Parse(.Reader("ExternalCreationDate"))
                                    If Not IsDBNull(.Reader("SendToOfficeTrack")) Then SendToOfficeTrack = .Reader("SendToOfficeTrack")
                                    'End If
                                End If
                                '_registrado = True
                            End If
                            .Reader.Close()
                        End If
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

        End Function

        Public Function ObtenerhistorialTareaServicio(ByVal IdServicioMensajeria As Integer) As DataTable
            Dim dthistorial As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@IdServicio", SqlDbType.Int).Value = IdServicioMensajeria
                    dthistorial = .EjecutarDataTable("ConsultarOfficeTrackTaskServicioMensajeriaHistorial", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dthistorial
        End Function

#End Region

    End Class


    Public Class Location

        Private _Adress As String

        Public Property Adress() As String
            Get
                Return _Adress
            End Get
            Set(ByVal value As String)
                _Adress = value
            End Set
        End Property



    End Class

End Namespace
