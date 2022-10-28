Imports LMDataAccessLayer

Namespace InventarioFisico
    Public Class InfoInventarioSerial

#Region "Atributos"

        Private _serial As String
        Private _material As String
        Private _descripcion As String
        Private _centro As String
        Private _almacen As String
        Private _idInventario As String
        Private _linea As String
        Private _fechaLectura As DateTime
        Private _registrado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
        End Sub

        Public Sub New(serial As String)
            _serial = serial
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Public Property Serial As String
            Get
                Return _serial
            End Get
            Set(value As String)
                _serial = value
            End Set
        End Property

        Public Property Material As String
            Get
                Return _material
            End Get
            Set(value As String)
                _material = value
            End Set
        End Property

        Public Property Descripcion As String
            Get
                Return _descripcion
            End Get
            Set(value As String)
                _descripcion = value
            End Set
        End Property

        Public Property Centro As String
            Get
                Return _almacen
            End Get
            Set(value As String)
                _almacen = value
            End Set
        End Property

        Public Property Almacen As String
            Get
                Return _almacen
            End Get
            Set(value As String)
                _almacen = value
            End Set
        End Property

        Public Property IdInventario As String
            Get
                Return _idInventario
            End Get
            Set(value As String)
                _idInventario = value
            End Set
        End Property

        Public Property Linea As String
            Get
                Return _linea
            End Get
            Set(value As String)
                _linea = value
            End Set
        End Property

        Public Property FechaLectura As DateTime
            Get
                Return _fechaLectura
            End Get
            Set(value As DateTime)
                _fechaLectura = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()

            If Not EsNuloOVacio(Me._serial) Then
                Using dbManager As New LMDataAccess
                    With dbManager
                        .SqlParametros.Add("@serial", SqlDbType.VarChar, 50).Value = Me._serial
                        .ejecutarReader("ObtenerListadoTipoProducto", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing AndAlso .Reader.Read Then
                            CargarResultadoConsulta(.Reader)
                            .Reader.Close()
                        End If
                    End With
                End Using
            End If
        End Sub

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing AndAlso reader.HasRows Then
                Me._serial = reader("serial").ToString
                Me._material = reader("material").ToString
                Me._descripcion = reader("descripcion").ToString
                Me._centro = reader("centro").ToString
                Me._almacen = reader("almacen").ToString
                Long.TryParse(reader("idInventario").ToString, Me._idInventario)
                Me._linea = reader("linea").ToString
                If Not IsDBNull(reader("fechaLectura")) Then Date.TryParse(reader("fechaLectura").ToString, Me._fechaLectura)
                _registrado = True
            End If

        End Sub

#End Region

    End Class
End Namespace