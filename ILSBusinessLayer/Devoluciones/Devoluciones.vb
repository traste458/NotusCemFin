Imports LMDataAccessLayer
Imports System.String
Imports ILSBusinessLayer.MensajeriaEspecializada
Imports System.Reflection

Public Class Devoluciones

#Region "Atributos"

    Protected Friend _idDevolucion As Integer
    Protected Friend _nombre As String
    Protected Friend _nombreCliente As String
    Protected Friend _fecha As Date
    Private _idCliente As Integer
    Private _Cliente As String
    Private _Observacion As String
    Private _Estado As String
    Private _idUsuario As Integer
    Private _FechaCargado As Date
    Private _idestado As Integer
    Private _idgrupoDevolucion As Integer
    Private _idposicion As Integer
    Private _guia As String
    Private FechaLeido As Date

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

    Public Property Nombre As String
        Get
            Return _nombre
        End Get
        Set(value As String)
            _nombre = value
        End Set
    End Property

    Public Property Observacion As String
        Get
            Return _Observacion
        End Get
        Set(value As String)
            _Observacion = value
        End Set
    End Property

    Public Property Estado As String
        Get
            Return _Estado
        End Get
        Set(value As String)
            _Estado = value
        End Set
    End Property
    Public Property IdEstado() As Integer
        Get
            Return _idestado
        End Get
        Set(ByVal value As Integer)
            _idestado = value
        End Set
    End Property
    Public Property Fecha As Date
        Get
            Return _fecha
        End Get
        Set(value As Date)
            _fecha = value
        End Set
    End Property


    Public Property IdCliente() As Integer
        Get
            Return _idCliente
        End Get
        Set(ByVal value As Integer)
            _idCliente = value
        End Set
    End Property
    Public Property IdgrupoDevolucion As Integer
        Get
            Return _idgrupoDevolucion
        End Get
        Set(value As Integer)
            _idgrupoDevolucion = value
        End Set
    End Property
    Public Property IdUsuario As Integer
        Get
            Return _idUsuario
        End Get
        Set(value As Integer)
            _idUsuario = value
        End Set
    End Property

    Public Property Cliente As String
        Get
            Return _Cliente
        End Get
        Set(value As String)
            _Cliente = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idDevolucion As Integer)
        MyBase.New()
        _idDevolucion = idDevolucion
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Public Sub CargarDatos()
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idDevolucion > 0 Then .SqlParametros.Add("@idDevolucion", SqlDbType.Int).Value = _idDevolucion
                    .ejecutarReader("ObtenerDevoluciones", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idDevolucion").ToString, _idDevolucion)
                            _nombre = .Reader("nombre").ToString
                            _fecha = .Reader("fecha")
                            Integer.TryParse(.Reader("idcliente").ToString, _idCliente)
                            Integer.TryParse(.Reader("idgrupo_devolucion").ToString, _idgrupoDevolucion)
                            Integer.TryParse(.Reader("idestado").ToString, _idestado)
                            _Cliente = .Reader("cliente")
                            _Estado = .Reader("Estado")
                        End If
                        .Reader.Close()
                    End If
                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Using
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Using dbManager As New LMDataAccess
            Try

            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return resultado
    End Function

    Public Function ActualizarGrupoDev() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Using dbManager As New LMDataAccess
            Try

                With dbManager
                    If _idDevolucion > 0 Then
                        .SqlParametros.Add("@idDevolucion", SqlDbType.Int).Value = _idDevolucion
                    End If
                    If _idgrupoDevolucion > 0 Then
                        .SqlParametros.Add("@idgrupo_devolucion", SqlDbType.Int).Value = _idgrupoDevolucion
                    End If
                    If _idUsuario > 0 Then
                        .SqlParametros.Add("@IdUsuario", SqlDbType.Int).Value = _idUsuario
                    End If
                    .SqlParametros.Add("@returnResultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                    .ejecutarNonQuery("ActualizarDevolucion", CommandType.StoredProcedure)

                    Dim respuesta As Integer = .SqlParametros("@returnResultado").Value
                    If respuesta = 0 Then
                        resultado.Valor = 0
                        resultado.Mensaje = "El registro se actualizo de forma correcta"
                    Else
                        resultado.Mensaje = "No fue posible registrar la actializacion"
                        resultado.Valor = 1

                    End If
                End With

            Catch ex As Exception
                resultado.Mensaje = "No fue posible registrar la actializacion" & ex.Message
                resultado.Valor = 1
                Throw ex
            End Try
        End Using
        Return resultado
    End Function

    Public Function EperturaDevolucion() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Using dbManager As New LMDataAccess
            Try

                With dbManager
                    If _idDevolucion > 0 Then
                        .SqlParametros.Add("@idDevolucion", SqlDbType.VarChar, 25).Value = _idDevolucion
                    End If
                    .SqlParametros.Add("@Observcion", SqlDbType.VarChar, 199).Value = _Observacion
                    If _idUsuario > 0 Then
                        .SqlParametros.Add("@IdUsuario", SqlDbType.Int).Value = _idUsuario
                    End If
                    .SqlParametros.Add("@returnResultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                    .ejecutarNonQuery("AperturaDevolucion", CommandType.StoredProcedure)

                    Dim respuesta As Integer = .SqlParametros("@returnResultado").Value
                    If respuesta = 0 Then
                        resultado.Valor = 0
                        resultado.Mensaje = "El registro se actualizo de forma correcta"
                    Else
                        resultado.Mensaje = "No fue posible registrar la actializacion"
                        resultado.Valor = 1

                    End If
                End With

            Catch ex As Exception
                resultado.Mensaje = "No fue posible registrar la actializacion" & ex.Message
                resultado.Valor = 1
                Throw ex
            End Try
        End Using
        Return resultado
    End Function


#End Region

End Class
