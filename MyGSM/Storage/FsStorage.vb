Imports System.IO

Public Class FileSystemStorage
    Inherits Storage

    Public RootPath As String
    Public RecvPath As String
    Public ReadyPath As String
    Public SentPath As String
    Public SentSucceededPath As String
    Public SentFailedPath As String
    Public ContactsPath As String

    Public Sub New(Root As String)
        RootPath = Root
        init()
    End Sub

    Public Sub init()
        RecvPath = Path.Combine(RootPath, "Received")
        ReadyPath = Path.Combine(RootPath, "ReadySend")
        SentPath = Path.Combine(RootPath, "Sent")
        SentSucceededPath = Path.Combine(SentPath, "Succeeded")
        SentFailedPath = Path.Combine(SentPath, "Failed")
        ContactsPath = Path.Combine(RootPath, "Contacts")
        MakeSurePath(RootPath)
        MakeSurePath(RecvPath)
        MakeSurePath(ReadyPath)
        MakeSurePath(SentPath)
        MakeSurePath(SentSucceededPath)
        MakeSurePath(SentFailedPath)
    End Sub

    Private Sub MakeSurePath(p As String)
        If Not Directory.Exists(p) Then
            Directory.CreateDirectory(p)
        End If
    End Sub

    Public Overrides Sub ExportReceived(smsid As String, copyto As String)
        File.Copy(Path.Combine(RecvPath, smsid), copyto)
    End Sub

    Public Overrides Sub DeleteReceived(smsid As String)
        File.Delete(Path.Combine(RecvPath, smsid))
    End Sub

    Public Overrides Sub InsertReceived(sms As ShortMessage)
        Using sw As New StreamWriter(NewReceivedFilename())
            sw.WriteLine(sms.IDForModule.ToString)
            sw.WriteLine(sms.Time.ToString)
            sw.WriteLine(sms.RemoteAddress)
            sw.WriteLine(DirectCast(sms.RemoteAddressType, Integer))
            sw.WriteLine(sms.Message)
            sw.Flush()
        End Using
    End Sub

    Public Overrides Function GetReceived(smsid As String) As ShortMessage
        Dim sms As New ShortMessage
        sms.UUID = smsid
        Using Rd As New StreamReader(Path.Combine(RecvPath, smsid))
            sms.IDForModule = Integer.Parse(Rd.ReadLine())
            sms.Time = DateTime.Parse(Rd.ReadLine())
            sms.RemoteAddress = Rd.ReadLine()
            sms.RemoteAddressType = Integer.Parse(Rd.ReadLine())
            sms.Message = Rd.ReadToEnd()
        End Using
        Return sms
    End Function

    Public Overrides Function ListReceived() As List(Of ShortMessage)
        Dim lstRecv As New List(Of ShortMessage)
        For Each FileName In Directory.GetFiles(RecvPath)
            Try
                Dim sms As New ShortMessage

                Dim fi As New FileInfo(FileName)
                lstRecv.Add(GetReceived(fi.Name))

            Catch ex As Exception

            End Try
        Next
        Return lstRecv
    End Function

    Public Function NewReceivedFilename() As String
        Dim Rand As New Random
        Dim Ret = Path.Combine(RecvPath, Format(Now, "yyyyMMdd_hhmmss_") & Rand.Next(100000) & ".msg")
        Do While File.Exists(Ret)
            Ret = Path.Combine(RecvPath, Format(Now, "yyyyMMdd_hhmmss_") & Rand.Next(100000) & ".msg")
        Loop
        Return Ret
    End Function

    Public Function NewSendFilename() As String
        Dim Rand As New Random
        Dim Ret = Path.Combine(ReadyPath, Format(Now, "yyyyMMdd_hhmmss_") & Rand.Next(100000) & ".msg")
        Do While File.Exists(Ret)
            Ret = Path.Combine(ReadyPath, Format(Now, "yyyyMMdd_hhmmss_") & Rand.Next(100000) & ".msg")
        Loop
        Return Ret
    End Function

    Public Overrides Sub EnqueueSend(sms As ShortMessage)
        If sms.Message.Length > 65 Then
            Dim Part = Globals.SplitByLength(sms.Message, 60)
            For i = 0 To Part.Length - 1
                Dim NewMessage = "(" & (i + 1).ToString & "/" & Part.Length.ToString & ")" & Part(i)
                File.WriteAllText(NewSendFilename(),
                                            sms.RemoteAddress & vbCrLf & DirectCast(sms.RemoteAddressType, Integer) & vbCrLf & NewMessage)
            Next
        Else
            File.WriteAllText(NewSendFilename(), sms.RemoteAddress & vbCrLf & DirectCast(sms.RemoteAddressType, Integer) & vbCrLf & sms.Message)
        End If
    End Sub

    Public Overrides Function ReadySendCount() As Integer
        Dim FileNames = Directory.GetFiles(ReadyPath)
        Return FileNames.Count
    End Function

    'Private _lastFilename As String = ""
    Public Overrides Function DequeueSend() As ShortMessage
        Dim FileNames = Directory.GetFiles(ReadyPath)
        If FileNames.Count = 0 Then
            Return Nothing
        End If
        Dim fi As New FileInfo(FileNames(0))
        '_lastFilename = fi.Name
        Return GetReadySend(fi.Name)
    End Function

    Public Overrides Function GetReadySend(smsid As String) As ShortMessage
        Dim sms As New ShortMessage
        sms.UUID = smsid
        Dim fullname = Path.Combine(ReadyPath, smsid)
        Using Rd As New StreamReader(fullname)
            sms.RemoteAddress = Rd.ReadLine()
            sms.RemoteAddressType = Integer.Parse(Rd.ReadLine())
            sms.Message = Rd.ReadToEnd
        End Using
        Return sms
    End Function

    Public Overrides Sub AckSent(smsid As String, Succeeded As Boolean)
        If Succeeded Then
            File.Move(Path.Combine(ReadyPath, smsid), Path.Combine(SentSucceededPath, smsid))
        Else
            File.Move(Path.Combine(ReadyPath, smsid), Path.Combine(SentFailedPath, smsid))
        End If
    End Sub

    Public Overrides Sub InitContacts()
        _ContactsCache = New List(Of ContactPerson)
        If Not File.Exists(ContactsPath) Then
            File.WriteAllText(ContactsPath, "")
        End If
        For Each Line As String In File.ReadAllLines(ContactsPath)
            If Line = "" OrElse Line.First = "'" Then
                Continue For
            End If
            Dim item = Split(Line, " ")
            Dim cp As New ContactPerson
            cp.Name = item(0)
            cp.Number = item(1)
            _ContactsCache.Add(cp)
        Next
    End Sub

    Public Overrides Sub InitSettings()
        'nop
    End Sub

    Protected Overrides Sub InternalDeleteContactsRecord(Name As String)
        SaveContacts()
    End Sub

    Protected Overrides Sub InternalInsertContactsRecord(cp As ContactPerson)
        SaveContacts()
    End Sub

    Protected Overrides Function InternalGetSetting(key As String) As String
        Return ""
    End Function

    Protected Overrides Sub InternalInsertSetting(key As String, value As String)
        'nop
    End Sub

    Protected Overrides Sub InternalSetSetting(key As String, value As String)
        'nop
    End Sub

    Private Sub SaveContacts()
        Using Wt As New StreamWriter(RootPath, "Contacts")
            For Each Person As ContactPerson In _ContactsCache
                Wt.WriteLine(Person.Name & " " & Person.Number)
            Next
        End Using
    End Sub

    Public Overrides Function Type() As Storage.StorageType
        Return StorageType.FileSystem
    End Function

End Class
