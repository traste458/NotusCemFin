Imports System.Text
Imports LMDataAccessLayer

Namespace MensajeriaEspecializada

    ''' <summary>
    ''' Author: Beltrán, Diego
    ''' Create date: 04/08/2014
    ''' Description: Clase diseñada para cargar los datos de los diferentes clientes asociados al CEM
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ClienteCem

#Region "Atributos"

        Private _idClienteCem As Integer
        Private _nombre As String
        Private _estado As Boolean

        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        ''' <summary>
        ''' Define o establece el identificador del cliente asociado al CEM
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdClienteCem As Integer
            Get
                Return _idClienteCem
            End Get
            Set(value As Integer)
                _idClienteCem = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece el nombre del cliente asociado al CEM
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nombre As String
            Get
                Return _nombre
            End Get
            Set(value As String)
                _nombre = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece el estado del cliente asociado al CEM
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Estado As Boolean
            Get
                Return _estado
            End Get
            Set(value As Boolean)
                _estado = value
            End Set
        End Property

#End Region

#Region "Construtores"

        Public Sub New()
            MyBase.New()
        End Sub

        ''' <summary>
        ''' Constructor que sobrecarga la clase con los datos del idClienteCEM proporcionado
        ''' </summary>
        ''' <param name="idClienteCEM"> de tipo <see langword="Integer"/> que contiene la información correspondiente al identificador del cliente CEM. </param>
        ''' <remarks>
        ''' Su forma de instanciamiento se debe realizar de la siguiente manera:
        ''' Dim miClase As New  MensajeriaEspecializada.ClienteCem(idClienteCEM:= idClienteCEM)
        ''' </remarks>
        Public Sub New(ByVal idClienteCEM As Integer)
            MyBase.New()
            _idClienteCem = idClienteCEM
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        ''' <summary>
        ''' función que realiza la inicialización de la carga de los atributos de la clase, según los parametros establecidos
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@listaIdClienteCem", SqlDbType.VarChar, 30).Value = CStr(_idClienteCem)
                    .ejecutarReader("ObtenerInfoClienteCem", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            CargarResultadoConsulta(.Reader)
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

#Region "Métodos Públicos"

#End Region

#Region "Métodos Protegidos"

        ''' <summary>
        ''' Método encargado de sobrecargar los atributos de la clase 
        ''' </summary>
        ''' <param name="reader"> de tipo <see langword="Data.Common.DbDataReader"/> que contiene un objeto de tipo reader, 
        ''' para realizar la lectura y asignación de valores a los atributos de la clase</param>
        ''' <remarks></remarks>
        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Integer.TryParse(reader("idClienteCem"), _idClienteCem)
                    If Not IsDBNull(reader("nombre")) Then _nombre = CStr(reader("nombre"))
                    If Not IsDBNull(reader("estado")) Then Boolean.TryParse(reader("estado"), _estado)
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace