Imports LMDataAccessLayer

Namespace Comunes

    Public Class InfoEstadoRestriccionCEM

#Region "Atributos"

        Private _idTipoServicio As Integer
        Private _idProcesoActual As Short
        Private _idProcesoSiguiente As Short
        Private _idEstadoActual As Integer
        Private _idEstadoSiguiente As Integer

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            CargarInformacion()
        End Sub

        Public Sub New(ByVal idTipoServicio As Integer, ByVal idProcesoActual As Short, idProcesoSiguiente As Short, ByVal idEstadoActual As Integer)
            MyBase.New()
            _idTipoServicio = idTipoServicio
            _idProcesoActual = idProcesoActual
            _idProcesoSiguiente = idProcesoSiguiente
            _idEstadoActual = _idEstadoActual
            CargarInformacion()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdProcesoActual As Short
            Get
                Return _idProcesoActual
            End Get
            Set(value As Short)
                _idProcesoActual = value
            End Set
        End Property

        Public Property IdProcesoSiguiente As Short
            Get
                Return _idProcesoSiguiente
            End Get
            Set(value As Short)
                _idProcesoSiguiente = value
            End Set
        End Property

        Public Property IdTipoServicio As Integer
            Get
                Return _idTipoServicio
            End Get
            Set(value As Integer)
                _idTipoServicio = value
            End Set
        End Property

        Public Property IdEstadoActual As Integer
            Get
                Return _idEstadoActual
            End Get
            Set(value As Integer)
                _idEstadoActual = value
            End Set
        End Property

        Public Property IdEstadoSiguiente As Integer
            Get
                Return _idEstadoSiguiente
            End Get
            Set(value As Integer)
                _idEstadoSiguiente = value
            End Set
        End Property

#End Region

#Region "Métodos"

        Public Sub CargarInformacion()
            Try
                Using dbManager As New LMDataAccess
                    With dbManager
                        .SqlParametros.Add("@idTipoServicio", SqlDbType.Int).Value = _idTipoServicio
                        If _idProcesoActual > 0 Then .SqlParametros.Add("@idProcesoActual", SqlDbType.SmallInt).Value = _idProcesoActual
                        If _idProcesoSiguiente > 0 Then .SqlParametros.Add("@idProcesoSiguiente", SqlDbType.SmallInt).Value = _idProcesoSiguiente
                        If _idEstadoActual > 0 Then .SqlParametros.Add("@idEstadoActual", SqlDbType.Int).Value = _idEstadoActual

                        .ejecutarReader("InfoEstadoRestriccionCEM", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                            .Reader.Read()
                            _idTipoServicio = CInt(.Reader("idTipoServicio"))
                            _idProcesoActual = CShort(.Reader("idProcesoActual"))
                            _idProcesoSiguiente = CShort(.Reader("idProcesoSiguiente"))
                            If Not IsDBNull(.Reader("idEstadoActual")) Then _idEstadoActual = CInt(.Reader("idEstadoActual"))
                            _idEstadoSiguiente = CInt(.Reader("idEstadoSiguiente"))
                        End If
                    End With
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

#End Region

    End Class

End Namespace


