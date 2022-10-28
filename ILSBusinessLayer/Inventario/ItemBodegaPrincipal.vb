﻿Imports ILSBusinessLayer
Imports LMDataAccessLayer

Namespace Inventario

    Public Class ItemBodegaPrincipal

#Region "Atributos (Campos)"

        Private _idRegistro As Long
        Private _serial As String
        Private _idProducto As Integer
        Private _nombreProducto As String
        Private _idSubProducto As Integer
        Private _referencia As String
        Private _material As String
        Private _idRegion As Short
        Private _codRegion As String
        Private _nombreRegion As String
        Private _idEstado As Integer
        Private _nombreEstado As String
        Private _fechaRecepcion As Date
        Private _cargado As Boolean
        Private _nacionalizado As Boolean
        Private _termosellado As Boolean
        Private _idBodega As Integer
        Private _nombreBodega As String
        Private _idPosicion As Integer
        Private _codPosicion As String
        Private _fechaAsignacionInventario As Date


        Private _idServicioOrigen As Short
        Private _nombreServicioOrigen As String
        Private _idUsuarioModificacion As Integer
        Private _nombreUsuarioModificacion As String

        'Atributos para indicar el estado del item
        Private _registrado As Boolean
        Private _accion As Enumerados.AccionItem

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()

            _idSubProducto = Nothing
            _referencia = String.Empty
            _idPosicion = Nothing
            _idServicioOrigen = Nothing

        End Sub

        Public Sub New(ByVal serial As String)
            Me.New()
            _serial = serial
            CargarDatos()
        End Sub

        Public Sub New(ByVal idRegistro As Long)
            Me.New()
            _idRegistro = idRegistro
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdRegistro() As Long
            Get
                Return _idRegistro
            End Get
            Set(ByVal value As Long)
                _idRegistro = value
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

        Public Property IdProducto() As Integer
            Get
                Return _idProducto
            End Get
            Set(ByVal value As Integer)
                _idProducto = value
            End Set
        End Property

        Public Property NombreProducto() As String
            Get
                Return _nombreProducto
            End Get
            Protected Friend Set(ByVal value As String)
                _nombreProducto = value
            End Set
        End Property

        Public Property IdSubProducto() As Integer
            Get
                Return _idSubProducto
            End Get
            Set(ByVal value As Integer)
                _idSubProducto = value
            End Set
        End Property

        Public Property Referencia() As String
            Get
                Return _referencia
            End Get
            Protected Friend Set(ByVal value As String)
                _referencia = value
            End Set
        End Property

        Public Property Material() As String
            Get
                Return _material
            End Get
            Protected Friend Set(ByVal value As String)
                _material = value
            End Set
        End Property

        Public Property IdRegion() As Short
            Get
                Return _idRegion
            End Get
            Set(ByVal value As Short)
                _idRegion = value
            End Set
        End Property

        Public Property CodRegion() As String
            Get
                Return _codRegion
            End Get
            Protected Friend Set(ByVal value As String)
                _codRegion = value
            End Set
        End Property

        Public Property NombreRegion() As String
            Get
                Return _nombreRegion
            End Get
            Protected Friend Set(ByVal value As String)
                _nombreRegion = value
            End Set
        End Property

        Public Property IdEstado() As Integer
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Integer)
                _idEstado = value
            End Set
        End Property

        Public Property NombreEstado() As String
            Get
                Return _nombreEstado
            End Get
            Protected Friend Set(ByVal value As String)
                _nombreEstado = value
            End Set
        End Property

        Public Property FechaRecepcion() As Date
            Get
                Return _fechaRecepcion
            End Get
            Set(ByVal value As Date)
                _fechaRecepcion = value
            End Set
        End Property

        Public Property Cargado() As Boolean
            Get
                Return _cargado
            End Get
            Set(ByVal value As Boolean)
                _cargado = value
            End Set
        End Property

        Public Property Nacionalizado() As Boolean
            Get
                Return _nacionalizado
            End Get
            Set(ByVal value As Boolean)
                _nacionalizado = value
            End Set
        End Property

        Public Property Termosellado() As Boolean
            Get
                Return _termosellado
            End Get
            Set(ByVal value As Boolean)
                _termosellado = value
            End Set
        End Property

        Public Property IdBodega() As Integer
            Get
                Return _idBodega
            End Get
            Set(ByVal value As Integer)
                _idBodega = value
            End Set
        End Property

        Public Property NombreBodega() As String
            Get
                Return _nombreBodega
            End Get
            Protected Friend Set(ByVal value As String)
                _nombreBodega = value
            End Set
        End Property

        Public Property IdPosicion() As Integer
            Get
                Return _idPosicion
            End Get
            Set(ByVal value As Integer)
                _idPosicion = value
            End Set
        End Property

        Public Property CodPosicion() As String
            Get
                Return _codPosicion
            End Get
            Protected Friend Set(ByVal value As String)
                _codPosicion = value
            End Set
        End Property

        Public Property FechaAsignacionInventario() As Date
            Get
                Return _fechaAsignacionInventario
            End Get
            Set(ByVal value As Date)
                _fechaAsignacionInventario = value
            End Set
        End Property

        Public Property IdServicioOrigen() As Short
            Get
                Return _idServicioOrigen
            End Get
            Set(ByVal value As Short)
                _idServicioOrigen = value
            End Set
        End Property

        Public Property NombreServicioOrigen()
            Get
                Return _nombreServicioOrigen
            End Get
            Protected Friend Set(ByVal value)
                _nombreServicioOrigen = value
            End Set
        End Property

        Public Property IdUsuarioModificacion() As Integer
            Get
                Return _idUsuarioModificacion
            End Get
            Set(ByVal value As Integer)
                _idUsuarioModificacion = value
            End Set
        End Property

        Public Property NombreUsuarioModificacion() As String
            Get
                Return _nombreUsuarioModificacion
            End Get
            Protected Friend Set(ByVal value As String)
                _nombreUsuarioModificacion = value
            End Set
        End Property


        Public Property Registrado() As Boolean
            Get
                Return _registrado
            End Get
            Set(ByVal value As Boolean)
                _registrado = value
            End Set
        End Property

        Public Property Accion() As Enumerados.AccionItem
            Get
                Return _accion
            End Get
            Set(ByVal value As Enumerados.AccionItem)
                _accion = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            'Using dbManager As New LMDataAccess
            Dim dbManager As New LMDataAccess

            Try
                With dbManager
                    If Not String.IsNullOrEmpty(_serial) Then .SqlParametros.Add("@listaSerial", SqlDbType.VarChar).Value = _serial
                    If _idRegistro > 0 Then .SqlParametros.Add("@listaIdRegistro", SqlDbType.VarChar, 8000).Value = _idRegistro.ToString
                    .ejecutarReader("ConsultaItemBodegaPrincipal", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        If .Reader.Read() Then
                            _idRegistro = CDbl(.Reader("idRegistro"))
                            _serial = .Reader("serial").ToString()
                            _idProducto = CInt(.Reader("idProducto"))
                            _nombreProducto = .Reader("nombreProducto").ToString()
                            Integer.TryParse(.Reader("idSubProducto"), _idSubProducto)
                            _referencia = .Reader("referencia").ToString()
                            _material = .Reader("material").ToString()
                            _idRegion = CShort(.Reader("idRegion"))
                            _codRegion = .Reader("codRegion").ToString()
                            _nombreRegion = .Reader("nombreRegion").ToString()
                            _idEstado = CInt(.Reader("idEstado"))
                            _nombreEstado = .Reader("nombreEstado").ToString()
                            _fechaRecepcion = CDate(.Reader("fechaRecepcion"))
                            _cargado = CBool(.Reader("cargado"))
                            _nacionalizado = CBool(.Reader("nacionalizado"))
                            _termosellado = CBool(.Reader("termosellado"))
                            _idBodega = CInt(.Reader("idBodega"))
                            _nombreBodega = .Reader("nombreBodega").ToString()
                            Integer.TryParse(.Reader("idPosicion").ToString(), _idPosicion)
                            _codPosicion = .Reader("codPosicion").ToString()
                            _fechaAsignacionInventario = IIf(.Reader("fechaAsignacionInventario") Is Nothing, Nothing, CDate(.Reader("fechaAsignacionInventario")))
                            Short.TryParse(.Reader("idServicioOrigen").ToString(), _idServicioOrigen)
                            _nombreServicioOrigen = .Reader("nombreServicioOrigen").ToString()
                            _idUsuarioModificacion = CInt(.Reader("idUsuarioModificacion"))
                            _nombreUsuarioModificacion = .Reader("nombreUsuarioModificacion").ToString()

                            _registrado = True
                        End If
                        .Reader.Close()
                    End If
                End With

            Catch ex As Exception
                Throw New Exception("Se generó un error en [CargarDatos]", ex)
            End Try
            dbManager.Dispose()
            'End Using
        End Sub

#End Region

    End Class

End Namespace
