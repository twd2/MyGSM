Imports System.ComponentModel
Imports System.Reflection
Imports System.Text

Public Class SettingsObject

    Public Shared [Default] As New SettingsObject

    Public Shared Function FromStorage(s As Storage) As SettingsObject
        Dim so As New SettingsObject
        Dim T As Type = GetType(SettingsObject)
        Dim ps = T.GetProperties()
        For Each pi In ps
            Dim k = pi.Name
            Dim pt = pi.PropertyType
            If pt.Equals(GetType(Storage.StorageType)) Then
                pi.SetValue(so, s.IntegerSettings(k), Nothing)
            ElseIf pt.Equals(GetType(Integer)) Then
                pi.SetValue(so, s.IntegerSettings(k), Nothing)
            ElseIf pt.Equals(GetType(Boolean)) Then
                pi.SetValue(so, s.BooleanSettings(k), Nothing)
            Else
                pi.SetValue(so, s.Settings(k), Nothing)
            End If
        Next
        Return so
    End Function

    Public Sub SaveToStorage(s As Storage)
        Dim T As Type = GetType(SettingsObject)
        Dim ps = T.GetProperties()
        For Each pi In ps
            Dim k = pi.Name
            Dim pt = pi.PropertyType
            Dim v = pi.GetValue(Me, Nothing)
            If pt.Equals(GetType(Storage.StorageType)) Then
                s.IntegerSettings(k) = v
            ElseIf pt.Equals(GetType(Integer)) Then
                s.IntegerSettings(k) = v
            ElseIf pt.Equals(GetType(Boolean)) Then
                s.BooleanSettings(k) = v
            Else
                s.Settings(k) = v
            End If
        Next
    End Sub

    Public Function ToDictionary() As Dictionary(Of String, String)
        Dim dic As New Dictionary(Of String, String)
        Dim T As Type = GetType(SettingsObject)
        Dim ps = T.GetProperties()
        For Each pi In ps
            Dim k = pi.Name
            Dim pt = pi.PropertyType
            Dim v = pi.GetValue(Me, Nothing).ToString()
            If pt.Equals(GetType(Storage.StorageType)) Then
                v = DirectCast(pi.GetValue(Me, Nothing), Integer).ToString()
            End If
            dic.Add(k, v)
        Next
        Return dic
    End Function

    Private _Version As String = "1.0"
    <DisplayName("版本"), CategoryAttribute("常规"), DescriptionAttribute("设置版本号"), DefaultValue("1.0")> _
    Public Property Version() As String
        Get
            Return _Version
        End Get
        Set(ByVal value As String)
            _Version = value
        End Set
    End Property

    Private _SerialPort As Integer = 1
    <DisplayName("串口号"), CategoryAttribute("串口"), DescriptionAttribute("设置串口号"), DefaultValue(1)> _
    Public Property SerialPort() As Integer
        Get
            Return _SerialPort
        End Get
        Set(ByVal value As Integer)
            _SerialPort = value
        End Set
    End Property

    Private _BaudRate As Integer = 9600
    <DisplayName("波特率"), CategoryAttribute("串口"), DescriptionAttribute("设置波特率"), DefaultValue(9600)> _
    Public Property BaudRate() As Integer
        Get
            Return _BaudRate
        End Get
        Set(ByVal value As Integer)
            _BaudRate = value
        End Set
    End Property

    Private _StorageType As Storage.StorageType = Storage.StorageType.None
    <DisplayName("存储类型"), CategoryAttribute("存储"), DescriptionAttribute("设置存储类型，修改此项后需要重新启动程序"), DefaultValue(Storage.StorageType.None)> _
    Public Property StorageType() As Storage.StorageType
        Get
            Return _StorageType
        End Get
        Set(ByVal value As Storage.StorageType)
            _StorageType = value
        End Set
    End Property

    Private _StoragePath As String = ""
    <DisplayName("存储路径"), CategoryAttribute("存储"), DescriptionAttribute("设置存储路径，修改此项后需要重新启动程序"), DefaultValue("")> _
    Public Property StoragePath() As String
        Get
            Return _StoragePath
        End Get
        Set(ByVal value As String)
            _StoragePath = value
        End Set
    End Property

    Private _Signature As String = ""
    <DisplayName("短信息签名"), CategoryAttribute("短信息"), DescriptionAttribute("设置短信息签名"), DefaultValue("")> _
    Public Property Signature() As String
        Get
            Return _Signature
        End Get
        Set(ByVal value As String)
            _Signature = value
        End Set
    End Property

    Private _AutoATH As Boolean = True
    <DisplayName("自动挂断"), CategoryAttribute("语音"), DescriptionAttribute("设置是否自动挂断打来的电话并发送短消息提示"), DefaultValue(True)> _
    Public Property AutoATH() As Boolean
        Get
            Return _AutoATH
        End Get
        Set(ByVal value As Boolean)
            _AutoATH = value
        End Set
    End Property

End Class
