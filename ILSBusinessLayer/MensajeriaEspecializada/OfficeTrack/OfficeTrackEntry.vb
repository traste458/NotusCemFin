Imports LMDataAccessLayer

Namespace MensajeriaEspecializada.OfficeTrack

    Public Class OfficeTrackEntry


#Region "Campos"
        Private _IdEntry As Integer
        Private _EventType As Short
        Private _EntryType As Short
        Private _EntryDate As DateTime
        Private _Data As String
        Private _Form As String
        Private _Tasks As List(Of OfficeTrackTask)

        Private _Task As OfficeTrackTask

        Private _EntryLocationAdress As String
        Private _EmployeeNumber As String
#End Region

#Region "Propiedades"

        Public Property Task() As OfficeTrackTask
            Get
                Return _Task
            End Get
            Set(ByVal value As OfficeTrackTask)
                _Task = value
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

        Public Property EntryLocationAdress() As String
            Get
                Return _EntryLocationAdress
            End Get
            Set(ByVal value As String)
                _EntryLocationAdress = value
            End Set
        End Property

        Public Property Tasks() As List(Of OfficeTrackTask)
            Get
                Return _Tasks
            End Get
            Set(ByVal value As List(Of OfficeTrackTask))
                _Tasks = value
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


        Public Property EventType() As Short
            Get
                Return _EventType
            End Get
            Set(ByVal value As Short)
                _EventType = value
            End Set
        End Property

        Public Property EntryType() As Short
            Get
                Return _EntryType
            End Get
            Set(ByVal value As Short)
                _EntryType = value
            End Set
        End Property



        Public Property EntryDate() As DateTime
            Get
                Return _EntryDate
            End Get
            Set(ByVal value As DateTime)
                _EntryDate = value
            End Set
        End Property

        Public Property Data() As String
            Get
                Return _Data
            End Get
            Set(ByVal value As String)
                _Data = value
            End Set
        End Property

        Public Property Form() As String
            Get
                Return _Form
            End Get
            Set(ByVal value As String)
                _Form = value
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


        Public Function Registrar(ByVal cadenaConexion As String) As Short
            Dim resultado As Short = 0
            Dim dbManager As New LMDataAccess(cadenaConexion)
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@EventType", SqlDbType.SmallInt).Value = EventType
                        .Add("@EntryType", SqlDbType.SmallInt).Value = EntryType
                        .Add("@EntryDate", SqlDbType.DateTime).Value = EntryDate
                        .Add("@Form", SqlDbType.VarChar, Integer.MaxValue).Value = Form
                        .Add("@Data", SqlDbType.VarChar, Integer.MaxValue).Value = Data
                        .Add("@EntryLocationAdress", SqlDbType.VarChar, Integer.MaxValue).Value = EntryLocationAdress
                        .Add("@EmployeeNumber", SqlDbType.VarChar, Integer.MaxValue).Value = EmployeeNumber
                        .Add("@TaskNumber", SqlDbType.BigInt).Value = Me.Task.TaskNumber
                        .Add("@IdEntry", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With

                    .IniciarTransaccion()
                    .EjecutarNonQuery("CrearOfficeTrackEntry", CommandType.StoredProcedure) '"CrearProducto"'
                    resultado = CShort(.SqlParametros("@result").Value)

                    If resultado = 0 Then
                        _IdEntry = .SqlParametros("@IdEntry").Value
                        'For Each tarea As OfficeTrackTask In _Tasks

                        _Task.IdEntry = Me._IdEntry
                        _Task.RegistrarTarea(dbManager)
                        'Next
                        .ConfirmarTransaccion()
                    Else
                        .AbortarTransaccion()
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return resultado
        End Function





#End Region

    End Class

End Namespace

