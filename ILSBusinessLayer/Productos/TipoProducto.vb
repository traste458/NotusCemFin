Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Enumerados
Imports LMDataAccessLayer

Namespace Productos

    Public Class TipoProducto

#Region "Campos"

        Private _idTipoProducto As Short
        Private _descripcion As String
        Private _instruccionable As Boolean
        Private _estado As Boolean
        Private _idTipoUnidad As Short
        Private _unidadEmpaque As String
        Private _aplicaTecnologia As Boolean
        Private _idModulo As Integer
        Private _tipoAplicativo As Short
        Private _pesado As Boolean
        Private _idTipoCargue As Integer
        Private _registrado As Boolean
        Private _expresionRegular As String

        Private _caracterPermitido As String
        Private _longitudPermitida As String
        Private _rangoPermitido As String

        Public Enum Tipo
            HANDSETS = 1
            SIM_CARDS = 2
            TARJETAS_PREPAGO = 3
            INSUMOS = 4
            MERCHANDISING = 5
            MATERIAL_POP_Y_PUBLICIDAD = 6
            ACCESORIOS = 7
            BONOS = 8
            TOKEN = 9
            PAPELERIA = 10
        End Enum

        Public Enum Tecnologia
            TDMA = 901
            GSM = 902
            CUATRO_G = 11042
	        DATAFONOS_LAPTOPS=904
	        TOKEN_LAPTOPS=905
            MERCHANDISING=11047
        End Enum


#End Region

#Region "Propiedades"

        Public Property RangoPermitido() As String
            Get
                Return _rangoPermitido
            End Get
            Set(ByVal value As String)
                _rangoPermitido = value
            End Set
        End Property
        Public Property LongitudPermitida() As String
            Get
                Return _longitudPermitida
            End Get
            Set(ByVal value As String)
                _longitudPermitida = value
            End Set
        End Property

        Public Property CaracterPermitido() As String
            Get
                Return _caracterPermitido
            End Get
            Set(ByVal value As String)
                _caracterPermitido = value
            End Set
        End Property

        Public Property IdTipoProducto() As Short
            Get
                Return _idTipoProducto
            End Get
            Set(ByVal value As Short)
                _idTipoProducto = value
            End Set
        End Property

        Public Property Descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
        End Property

        Public Property Instruccionable() As Boolean
            Get
                Return _instruccionable
            End Get
            Set(ByVal value As Boolean)
                _instruccionable = value
            End Set
        End Property

        Public Property Activo() As Boolean
            Get
                Return _estado
            End Get
            Set(ByVal value As Boolean)
                _estado = value
            End Set
        End Property

        Public Property IdTipoUnidad() As Short
            Get
                Return _idTipoUnidad
            End Get
            Set(ByVal value As Short)
                _idTipoUnidad = value
            End Set
        End Property

        Public ReadOnly Property UnidadEmpaque() As String
            Get
                Return _unidadEmpaque
            End Get
        End Property

        Public Property AplicaTecnologia() As Boolean
            Get
                Return _aplicaTecnologia
            End Get
            Set(ByVal value As Boolean)
                _aplicaTecnologia = value
            End Set
        End Property

        Public Property IdModulo() As Integer
            Get
                Return _idModulo
            End Get
            Set(ByVal value As Integer)
                _idModulo = value
            End Set
        End Property

        Public Property TipoAplicativo() As Short
            Get
                Return _tipoAplicativo
            End Get
            Set(ByVal value As Short)
                _tipoAplicativo = value
            End Set
        End Property

        ''' <summary>
        ''' Define si el tipo de producto indicado debe ser pesado o no
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Pesado() As Boolean
            Get
                Return _pesado
            End Get
            Set(ByVal value As Boolean)
                _pesado = value
            End Set
        End Property

        Public Property IdTipoCargue() As Integer
            Get
                Return _idTipoCargue
            End Get
            Set(ByVal value As Integer)
                _idTipoCargue = value
            End Set
        End Property

        Public Property ExpresionValidacion() As String
            Get
                Return _expresionRegular
            End Get
            Set(ByVal value As String)
                _expresionRegular = value
            End Set
        End Property

        Public Property Registrado() As Boolean
            Get
                Return _registrado
            End Get
            Protected Friend Set(ByVal value As Boolean)
                _registrado = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            _idTipoProducto = identificador
            CargarInformacion()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            Dim dbManager As New LMDataAccess

            Try
                With dbManager
                    .SqlParametros.Add("@idTipoProducto", SqlDbType.BigInt).Value = _idTipoProducto
                    .ejecutarReader("ObtenerListadoTipoProducto", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing AndAlso .Reader.Read Then
                        CargarResultadoConsulta(.Reader)
                        .Reader.Close()
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As Short
            Dim resultado As Short
            If _idTipoProducto <> 0 And _descripcion.Trim.Length > 0 And _idTipoUnidad <> 0 Then
                Dim dbManager As New LMDataAccess

                Try
                    With dbManager
                        .SqlParametros.Add("@idTipoProducto", SqlDbType.SmallInt).Value = _idTipoProducto
                        .SqlParametros.Add("@descripcion", SqlDbType.VarChar, 50).Value = _descripcion
                        .SqlParametros.Add("@instruccionable", SqlDbType.Bit).Value = _instruccionable
                        .SqlParametros.Add("@idTipoUnidad", SqlDbType.SmallInt).IsNullable = True
                        .SqlParametros.Add("@tipoAplicativo", SqlDbType.SmallInt).IsNullable = True
                        .SqlParametros("@idTipoUnidad").Value = IIf(_idTipoUnidad <> 0, _idTipoUnidad, DBNull.Value)
                        .SqlParametros.Add("@aplicaTecnologia", SqlDbType.Bit).Value = _aplicaTecnologia
                        .SqlParametros("@tipoAplicativo").Value = IIf(_tipoAplicativo <> 0, _tipoAplicativo, DBNull.Value)
                        .SqlParametros.Add("@pesado", SqlDbType.Bit).Value = _pesado
                        .SqlParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("CrearTipoProducto", CommandType.StoredProcedure)
                        resultado = CShort(.SqlParametros("@returnValue").Value)
                        If resultado = 0 Then _idTipoProducto = CShort(.SqlParametros("@idTipoProducto").Value)
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 4
            End If
            Return resultado
        End Function

        Public Function Actualizar() As Short
            Dim resultado As Short
            If _idTipoProducto <> 0 And _descripcion.Trim.Length > 0 And _idTipoUnidad <> 0 Then
                Dim dbManager As New LMDataAccess

                Try
                    With dbManager
                        .SqlParametros.Add("@idTipoProducto", SqlDbType.SmallInt).Value = _idTipoProducto
                        .SqlParametros.Add("@descripcion", SqlDbType.VarChar, 50).Value = _descripcion
                        .SqlParametros.Add("@estado", SqlDbType.Bit).Value = _estado
                        .SqlParametros.Add("@instruccionable", SqlDbType.Bit).Value = _instruccionable
                        .SqlParametros.Add("@idTipoUnidad", SqlDbType.SmallInt).IsNullable = True
                        .SqlParametros.Add("@tipoAplicativo", SqlDbType.SmallInt).IsNullable = True
                        .SqlParametros.Add("@aplicaTecnologia", SqlDbType.Bit).Value = _aplicaTecnologia
                        .SqlParametros("@idTipoUnidad").Value = IIf(_idTipoUnidad <> 0, _idTipoUnidad, DBNull.Value)
                        .SqlParametros("@tipoAplicativo").Value = IIf(_tipoAplicativo <> 0, _tipoAplicativo, DBNull.Value)
                        .SqlParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("ActualizarTipoProducto", CommandType.StoredProcedure)
                        resultado = CShort(.SqlParametros("@returnValue").Value)
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 3
            End If
            Return resultado
        End Function

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing AndAlso reader.HasRows Then
                Integer.TryParse(reader("idTipoProducto").ToString, Me._idTipoProducto)
                _descripcion = reader("descripcion").ToString
                _instruccionable = CBool(reader("instruccionable").ToString)
                _estado = CBool(reader("estado").ToString)
                Short.TryParse(reader("idTipoUnidad").ToString, _idTipoUnidad)
                _unidadEmpaque = reader("unidadEmpaque").ToString
                _aplicaTecnologia = CBool(reader("aplicaTecnologia").ToString)
                Short.TryParse(reader("tipoAplicativo").ToString, _tipoAplicativo)
                _pesado = CBool(reader("pesado").ToString)
                Integer.TryParse(reader("idTipoCargue").ToString, _idTipoCargue)
                _expresionRegular = reader("expresionRegular").ToString
                _registrado = True
            End If

        End Sub

#End Region

#Region "Métodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroTipoProducto
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroTipoProducto) As DataTable
            Dim dtDatos As New DataTable
            Dim db As New LMDataAccess
            With filtro
                If .IdTipoProducto > 0 Then db.SqlParametros.Add("@idTipoProducto", SqlDbType.BigInt).Value = .IdTipoProducto
                If .Activo > 0 Then db.SqlParametros.Add("@estado", SqlDbType.Int).Value = IIf(.Activo = 1, 1, 0)
                If .Instruccionable > 0 Then db.SqlParametros.Add("@instruccionable", SqlDbType.Int).Value = IIf(.Instruccionable = 1, 1, 0)
                If .Descripcion IsNot Nothing AndAlso .Descripcion.Trim.Length > 0 Then db.SqlParametros.Add("@descripcion", SqlDbType.VarChar).Value = .Descripcion
                If .ExisteModulo > 0 Then db.SqlParametros.Add("@existeModulo", SqlDbType.Int).Value = IIf(.ExisteModulo = 1, 1, 0)
                If .IdModulo > 0 Then db.SqlParametros.Add("@idModulo", SqlDbType.Int).Value = .IdModulo
                If .tipoAplicativo > 0 Then db.SqlParametros.Add("@tipoAplicativo", SqlDbType.SmallInt).Value = .tipoAplicativo
                If .listaNoCargar IsNot Nothing AndAlso .listaNoCargar.Count Then db.SqlParametros.Add("@listaNoCargar", SqlDbType.VarChar).Value = Join(.listaNoCargar.ToArray, ",")
                If .Pesado > 0 Then db.SqlParametros.Add("@pesado", SqlDbType.Bit).Value = IIf(.Pesado = 1, 1, 0)
                If .EsSerialziado > 0 Then db.SqlParametros.Add("@serializado", SqlDbType.Int).Value = IIf(.EsSerialziado = 1, 1, 0)
                dtDatos = db.EjecutarDataTable("ObtenerListadoTipoProducto", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos
        End Function

        Public Shared Function ObtenerPorId(ByVal identificador As Short) As DataTable
            Dim dtDatos As DataTable
            Dim filtro As New FiltroTipoProducto
            filtro.IdTipoProducto = identificador
            dtDatos = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Sub CargarInfoConfiguracionLecturaSerial(ByVal idTipoProducto As Tipo, ByVal idTecnologia As Tecnologia)
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.AgregarParametroSQL("@idTipoProducto", idTipoProducto, SqlDbType.Int)
            db.AgregarParametroSQL("@idTecnologia", idTecnologia, SqlDbType.Int)
            Try
                db.ejecutarReader("ObtenerInfoConfiguracionLecturaSerial", CommandType.StoredProcedure)
                With db
                    If .Reader IsNot Nothing Then
                        If .Reader.Read() Then
                            _caracterPermitido = .Reader("caracterPermitido")
                            _longitudPermitida = .Reader("longitudPermitida")
                            _rangoPermitido = .Reader("rangoPermitido")
                        End If
                    End If
                End With
                If String.IsNullOrEmpty(_caracterPermitido) Or String.IsNullOrEmpty(_longitudPermitida) Then
                    Throw New Exception("No se encontró Configuración de Lectura para validar el serial. " & vbCrLf & _
                                         "Por favor contacte a IT Development ")
                End If
            Catch ex As Exception
            Finally
                db.Dispose()
            End Try
        End Sub
        Public Sub CargarInfoConfiguracionLecturaSerialMaterial(ByVal vMaterial As String)
            Dim expCaracteres As String
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@Material", vMaterial, SqlDbType.VarChar)

            Try
                db.ejecutarReader("ObtenerInfoConfiguracionLecturaSerialMaterial", CommandType.StoredProcedure)
                With db
                    If .Reader IsNot Nothing Then
                        If .Reader.Read() Then
                            _caracterPermitido = .Reader("caracterPermitido")
                            _longitudPermitida = .Reader("longitudPermitida")
                            _rangoPermitido = .Reader("rangoPermitido")
                        End If
                    End If
                End With
                If String.IsNullOrEmpty(_caracterPermitido) Or String.IsNullOrEmpty(_longitudPermitida) Then
                    Throw New Exception("No se encontró Configuración de Lectura para validar el serial. " & vbCrLf & _
                                         "Por favor contacte a IT Development ")
                End If
            Catch ex As Exception
            Finally
                db.Dispose()
            End Try
        End Sub

#End Region

    End Class

End Namespace


