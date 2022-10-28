Imports ILSBusinessLayer
Imports LMDataAccessLayer

Namespace Inventario

    Public Class DetalleSerialBloqueo

#Region "Atributos (Campos)"

        Private _idBloqueoDetalleSerial As Integer
        Private _idBloqueoDetalleProducto As Integer
        Private _idBloqueo As Integer
        Private _serial As String

        'Atributos para indicar el estado del item
        Private _registrado As Boolean
        Private _accion As Enumerados.AccionItem

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal serial As String)
            Me.New()
            _serial = serial
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdBloqueoDetalleSerial() As Integer
            Get
                Return _idBloqueoDetalleSerial

            End Get
            Set(ByVal value As Integer)
                _idBloqueoDetalleSerial = value
            End Set
        End Property

        Public Property IdBloqueoDetalleProducto() As Integer
            Get
                Return _idBloqueoDetalleProducto
            End Get
            Set(ByVal value As Integer)
                _idBloqueoDetalleProducto = value
            End Set
        End Property

        Public Property IdBloqueo() As Integer
            Get
                Return _idBloqueo
            End Get
            Set(ByVal value As Integer)
                _idBloqueo = value
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
                    .SqlParametros.Add("@listaSerial", SqlDbType.VarChar).Value = _serial
                    .ejecutarReader("ConsultaBloqueoInventarioDetalleSerial", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        If .Reader.Read() Then

                            _idBloqueoDetalleSerial = CInt(.Reader("idBloqueoDetalleSerial"))
                            _idBloqueoDetalleProducto = CInt(.Reader("idBloqueoDetalleProducto"))
                            _idBloqueo = CInt(.Reader("idBloqueo"))
                            _serial = .Reader("serial").ToString()

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

