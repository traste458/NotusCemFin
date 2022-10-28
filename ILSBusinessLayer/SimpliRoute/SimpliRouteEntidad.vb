Imports System.Web
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Data
Imports System.Text
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Serialization

Namespace SimpliRouteEntidad
    'Public Class SimpliRouteRequest


    '    Public Property list() As List(Of Body) = New List(Of Body)

    'End Class


    'Public Class SimpliRouteResponse2


    '    Public Property rest As Body = New Body

    'End Class


    Public Class SimpliRouteResponse
        Public Property title As Integer
        Public Property order As String
        Public Property address As String
        Public Property latitude As String
        Public Property longitude As String
        Public Property load As String
        Public Property load_2 As String
        Public Property load_3 As String
        Public Property Window_start As String
        Public Property Window_end As String
        Public Property duration As String
        Public Property contact_name As String
        Public Property contact_phone As String
        Public Property contact_email As String
        Public Property reference As String
        Public Property notes As String
        Public Property planned_date As String
        Public Property skills_required As String


    End Class

    Public Class Body
        Public Property id As String
        Public Property order As String
        Public Property tracking_id As String
        Public Property status As String
        Public Property title As String
        Public Property address As String
        Public Property latitude As String
        Public Property longitude As String
        Public Property load As String
        Public Property load_2 As String
        Public Property load_3 As String
        Public Property Window_start As String
        Public Property Window_end As String
        Public Property duration As String
        Public Property contact_name As String
        Public Property contact_phone As String
        Public Property contact_email As String
        Public Property reference As String
        Public Property notes As String
        Public Property planned_date As String
        Public Property Driver As String
        Public Property Vehicle As String
        Public Property skills_required As List(Of Integer)
        Public Property IdUsuario As Integer
    End Class

    Public Class ResponseUsuario
        Public Property id As String
        Public Property username As String
        Public Property name As String
        Public Property phone As String
        Public Property email As String
        Public Property is_owner As String
        Public Property is_admin As String
        Public Property is_driver As String
        Public Property old_id As String
        Public Property created As String
        Public Property modified As String
        Public Property last_login As String
    End Class

End Namespace
