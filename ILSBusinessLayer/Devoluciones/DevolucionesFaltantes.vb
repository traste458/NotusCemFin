Imports LMDataAccessLayer
Imports System

Public Class DevolucionesFaltantes

#Region "Atributos"

    Private _idDevolucion As Integer

#End Region

#Region "Propiedades"

    Public Property IdDevolucion As Integer
        Get
            Return _idDevolucion
        End Get
        Set(value As Integer)
            _idDevolucion = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "MétodosPublicos"

    Public Function VerficarFaltantesSerialesDevolucion() As DataTable
        Dim dbManager As New LMDataAccess
        Dim dtFaltantes As DataTable

        With dbManager
            If _idDevolucion > 0 Then
                .SqlParametros.Add("@idDevolucion", SqlDbType.Int).Value = _idDevolucion
            End If
            dtFaltantes = .ejecutarDataTable("ValidarSerialesFaltantesDevolucion", CommandType.StoredProcedure)
        End With

        Return dtFaltantes
    End Function

    Public Function VerificarDevolucionCreada(ByVal tipoCampo As Integer, ByVal remision As String) As ResultadoProceso
        Dim dbManager As New LMDataAccess
        Dim resultado As New ResultadoProceso

        With dbManager
            With .SqlParametros
                .Clear()
                .Add("@tipoCampo", SqlDbType.Int).Value = tipoCampo
                .Add("@remision", SqlDbType.VarChar).Value = remision
                .Add("@idResultado", SqlDbType.BigInt).Direction = ParameterDirection.Output
                .Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            End With

            Dim result As Short = 0

            .EjecutarNonQuery("VerificarDevolucionCreada", CommandType.StoredProcedure)
            Short.TryParse(.SqlParametros("@result").Value.ToString, result)

            If result = 1 Then
                resultado.EstablecerMensajeYValor(.SqlParametros("@idResultado").Value, "Verificacion correcta")
            Else
                resultado.EstablecerMensajeYValor("-501", "Imposible registrar la información de la Orden en la Base de Datos.")
            End If
        End With

        Return resultado
    End Function

#End Region

End Class
