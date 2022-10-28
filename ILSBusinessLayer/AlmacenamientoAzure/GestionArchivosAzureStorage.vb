Imports System.IO
Imports System.Net
Imports System.Threading.Tasks
Imports System.Web
Imports Microsoft.Azure
Imports Microsoft.Azure.Storage
Imports Microsoft.Azure.Storage.Blob

Public Class GestionArchivosAzureStorage

    Public Property TipoContenido As String
    Public Property ArchivoStream As Stream
    Public Property RutaAzure As String

    Private configConexion As New Comunes.ConfigValues("CADENA_CONEXION_AZURE")
    Private contenedor As New Comunes.ConfigValues("CONTENEDOR_AZURE")
    Private raizRutaCarpeta As New Comunes.ConfigValues("RUTA_AZURE")
    Private contenedorAzure As String
    Private cadenaConexionAzure As String

    Public Sub New(ByVal key As Integer, nombreArchivo As String)
        MyBase.New()
        Me.cadenaConexionAzure = configConexion.ConfigKeyValue
        Me.contenedorAzure = contenedor.ConfigKeyValue
        Me.RutaAzure = raizRutaCarpeta.ConfigKeyValue & "/Archivos/Servicio" & key.ToString().PadLeft(8, "0") & "/" & nombreArchivo
    End Sub

    Public Sub New()
        MyBase.New()
        Me.cadenaConexionAzure = configConexion.ConfigKeyValue
        Me.contenedorAzure = contenedor.ConfigKeyValue
    End Sub


    Public Async Function AlmacenarArchivoAzureAsync() As Task

        Dim connectionString As String = cadenaConexionAzure
        Dim account As CloudStorageAccount = CloudStorageAccount.Parse(connectionString)
        Dim client As CloudBlobClient = account.CreateCloudBlobClient()
        Dim container As CloudBlobContainer = client.GetContainerReference(contenedorAzure)

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        If Await container.CreateIfNotExistsAsync() Then
            Await container.SetPermissionsAsync(New BlobContainerPermissions With {
                                          .PublicAccess = BlobContainerPublicAccessType.Blob
                                  })
        End If

        Dim cloudBlockBlob As CloudBlockBlob = container.GetBlockBlobReference(RutaAzure)
        cloudBlockBlob.Properties.ContentType = TipoContenido
        'Await cloudBlockBlob.UploadFromFileAsync(rutaFile) 'Para subir archivo desde ruta local
        Await cloudBlockBlob.UploadFromStreamAsync(ArchivoStream)
        ArchivoStream.Dispose()
    End Function

    Public Function CloudBlockArchivo(ByRef existBlobArchivo As Boolean) As CloudBlockBlob
        Dim storageConnection As String = cadenaConexionAzure
        Dim cloudStorageAccount As CloudStorageAccount = CloudStorageAccount.Parse(storageConnection)
        Dim blobClient As CloudBlobClient = cloudStorageAccount.CreateCloudBlobClient()

        Dim cloudBlobContainer As CloudBlobContainer = blobClient.GetContainerReference(contenedorAzure)
        Dim blockBlob As CloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(RutaAzure)

        existBlobArchivo = BlobExistsOnCloud(blobClient, contenedorAzure, RutaAzure)

        Return blockBlob
    End Function

    Private Function BlobExistsOnCloud(ByVal client As CloudBlobClient, ByVal containerName As String, ByVal key As String) As Boolean
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        Return client.GetContainerReference(containerName).GetBlockBlobReference(key).Exists()
    End Function

    Public Function EliminarArchivo(ruta As String) As Boolean
        Dim result As Boolean = False
        Dim storageConnection As String = cadenaConexionAzure
        Dim cloudStorageAccount As CloudStorageAccount = CloudStorageAccount.Parse(storageConnection)
        Dim blobClient As CloudBlobClient = cloudStorageAccount.CreateCloudBlobClient()

        Dim connectionString As String = cadenaConexionAzure
        Dim account As CloudStorageAccount = CloudStorageAccount.Parse(connectionString)
        Dim client As CloudBlobClient = account.CreateCloudBlobClient()
        Dim container As CloudBlobContainer = client.GetContainerReference(contenedorAzure)
        Try
            Dim existBlobArchivo As Boolean = BlobExistsOnCloud(blobClient, contenedorAzure, ruta)

            If existBlobArchivo Then
                Dim blos As CloudBlockBlob = container.GetBlockBlobReference(ruta)
                result = blos.DeleteIfExists()
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return result
    End Function

    Public Function DescargarArchivoPorRuta(ruta As String) As Boolean

        Dim memStream As New MemoryStream()
        Dim storageConnection As String = cadenaConexionAzure
        Dim cloudStorageAccount As CloudStorageAccount = CloudStorageAccount.Parse(storageConnection)
        Dim blobClient As CloudBlobClient = cloudStorageAccount.CreateCloudBlobClient()

        Dim connectionString As String = cadenaConexionAzure
        Dim account As CloudStorageAccount = CloudStorageAccount.Parse(connectionString)
        Dim client As CloudBlobClient = account.CreateCloudBlobClient()
        Dim container As CloudBlobContainer = client.GetContainerReference(contenedorAzure)
        Try
            Dim existBlobArchivo As Boolean = BlobExistsOnCloud(blobClient, contenedorAzure, ruta)

            If existBlobArchivo Then
                Dim cloudBlock As CloudBlockBlob = container.GetBlockBlobReference(ruta)

                cloudBlock.DownloadToStream(memStream)

                HttpContext.Current.Response.ContentType = cloudBlock.Properties.ContentType.ToString()
                HttpContext.Current.Response.AddHeader("Content-Disposition", "Attachment; filename=" & Path.GetFileName(ruta))
                HttpContext.Current.Response.AddHeader("Content-Length", cloudBlock.Properties.Length.ToString())
                HttpContext.Current.Response.BinaryWrite(memStream.ToArray())
                HttpContext.Current.Response.Flush()
                HttpContext.Current.Response.Close()

                Return True

            Else
                Return False
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function

End Class
