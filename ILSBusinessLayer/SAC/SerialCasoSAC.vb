Imports LMDataAccessLayer

Namespace SAC

    Public Class SerialCasoSAC

#Region "Atributos"

        Private _idSerial As Integer
        Private _idCaso As Integer
        Private _serial As String
        Private _idTipoSerial As Short
        Private _idPos As Integer
        Private _pos As String
        Private _idCoordinador As Integer
        Private _coordinador As String
        Private _idSupervisor As Integer
        Private _supervisor As String
        Private _fechaRegistro As Date
        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public ReadOnly Property IdSerial() As Integer
            Get
                Return _idSerial
            End Get
        End Property

        Public Property IdCaso() As Integer
            Get
                Return _idCaso
            End Get
            Set(ByVal value As Integer)
                _idCaso = value
            End Set
        End Property

        Public Property Serial() As String
            Get
                Return _serial
            End Get
            Set(ByVal value As String)
                _serial = value
            End Set
        End Property

        Public Property IdTipoSerial() As Short
            Get
                Return _idTipoSerial
            End Get
            Set(ByVal value As Short)
                _idTipoSerial = value
            End Set
        End Property

        Public ReadOnly Property IdPos() As Integer
            Get
                Return _idPos
            End Get
        End Property

        Public ReadOnly Property PDV() As String
            Get
                Return _pos
            End Get
        End Property

        Public ReadOnly Property IdCoordinador() As Integer
            Get
                Return _idCoordinador
            End Get
        End Property

        Public ReadOnly Property Coordinador() As String
            Get
                Return _coordinador
            End Get
        End Property

        Public ReadOnly Property IdSupervisor() As Integer
            Get
                Return _idSupervisor
            End Get
        End Property

        Public ReadOnly Property Supervisor() As String
            Get
                Return _supervisor
            End Get
        End Property

        Public ReadOnly Property FechaRegistro() As Date
            Get
                Return _fechaRegistro
            End Get
        End Property

        Public ReadOnly Property Registrado() As Boolean
            Get
                Return _registrado
            End Get
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _serial = ""
            _pos = ""
            _coordinador = ""
            _supervisor = ""
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            CargarDatos(identificador)
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos(ByVal identificador As Integer)
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idSerial", SqlDbType.Int).Value = identificador
                    .ejecutarReader("ConsultarSerialCasoSAC", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idSerial").ToString, _idSerial)
                            Integer.TryParse(.Reader("idCaso").ToString, _idCaso)
                            _serial = .Reader("serial").ToString
                            Short.TryParse(.Reader("idTipoSerial"), _idTipoSerial)
                            Integer.TryParse(.Reader("idPos").ToString, _idPos)
                            _pos = .Reader("pos").ToString
                            Integer.TryParse(.Reader("idCoordinador").ToString, _idCoordinador)
                            _coordinador = .Reader("coordinador").ToString
                            Integer.TryParse(.Reader("idSupervisor").ToString, _idSupervisor)
                            _supervisor = .Reader("supervisor").ToString
                            Date.TryParse(.Reader("fechaRegistro").ToString, _fechaRegistro)
                            _registrado = True
                        End If
                        .Reader.Close()
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub EstablecerIdentificador(ByVal identificador As Short)
            _idSerial = identificador
        End Sub

        Protected Friend Sub EstablecerIdPos(ByVal valor As Integer)
            _idPos = valor
        End Sub

        Protected Friend Sub EstablecerPDV(ByVal valor As String)
            _pos = valor
        End Sub

        Protected Friend Sub EstablecerIdCoordinador(ByVal valor As Integer)
            _idCoordinador = valor
        End Sub

        Protected Friend Sub EstablecerCoordinador(ByVal valor As String)
            _coordinador = valor
        End Sub

        Protected Friend Sub EstablecerIdSupervisor(ByVal valor As Integer)
            _idSupervisor = valor
        End Sub

        Protected Friend Sub EstablecerSupervisor(ByVal valor As String)
            _supervisor = valor
        End Sub

        Protected Friend Sub EstablecerFechaRegistro(ByVal valor As Date)
            _fechaRegistro = valor
        End Sub

        Protected Friend Sub MarcarComoRegistrado()
            _registrado = True
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso

            Return resultado
        End Function

        Public Function Actualizar() As ResultadoProceso
            Dim resultado As New ResultadoProceso

            Return resultado
        End Function

        Public Function EsValidoParaAsigancionACaso() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If Me._serial.Trim.Length > 0 Then
                Dim dbManager As New LMDataAccess

                Try
                    With dbManager
                        .SqlParametros.Add("@serial", SqlDbType.VarChar, 20).Value = Me._serial
                        .SqlParametros.Add("@return", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .ejecutarReader("ValidarSerialParaAsignacionACaso", CommandType.StoredProcedure)
                        Dim returnValue As Short = CShort(.SqlParametros("@return").Value)
                        '***Se retira validación de existencia del serial por solicitud del usuario***'
                        'If returnValue = 0 Or returnValue = 2 Then 
                        If returnValue = 2 Then resultado.Mensaje = "El serial se encuentra asociado a un caso que no ha sido cerrado."
                        If .Reader IsNot Nothing AndAlso .Reader.Read Then
                            Integer.TryParse(.Reader("idPos").ToString, Me._idPos)
                            Me._pos = .Reader("pos").ToString
                            Integer.TryParse(.Reader("idCoordinador").ToString, Me._idCoordinador)
                            Me._coordinador = .Reader("coordinador").ToString
                            Integer.TryParse(.Reader("idSupervisor").ToString, Me._idSupervisor)
                            Me._supervisor = .Reader("supervisor").ToString
                            Short.TryParse(.Reader("idTipoSerial").ToString, Me._idTipoSerial)
                            .Reader.Close()
                        Else
                            resultado.Valor = 5
                            resultado.Mensaje = "Imposible recuperar la información del serial consultado. por favor intente nuevamente."
                        End If
                        'Else
                        '    resultado.Valor = 1
                        '    resultado.Mensaje = "El serial especificado no existe en la BD."
                        'End If
                    End With
                Catch ex As Exception
                    resultado.Valor = 3
                    resultado.Mensaje = "Ocurrió un error inesperado al tratar de consultar serial. " & ex.Message
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado.Valor = 4
                resultado.Mensaje = "No se ha establecido el serial que se desea asignar al Caso"
            End If
            Return resultado
        End Function

#End Region

    End Class

End Namespace

