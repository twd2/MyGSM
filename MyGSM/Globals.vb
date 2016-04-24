Imports System.IO
Imports System.Text

Public Class Globals

    'Public Shared MainPath As String = Path.Combine(My.Application.Info.DirectoryPath, "SMS")
    Public Shared GSM As GSMModule
    Public Shared DatabasePath As String = "sms.db3"
    Public Shared SettingsStorage As Storage
    Public Shared Storage As Storage
    Public Shared frmdbg As frmDebug = frmDebug
    'Public Shared Contacts As New Contacts(MainPath)

    Public Shared Sub Init()
        'tests()

        frmDebug.Show()

        dbg("Loading")

        Dim args = My.Application.CommandLineArgs
        If args.Count >= 1 Then
            DatabasePath = args(0)
        End If

        dbg("DatabasePath: " + DatabasePath)

        InitStorage()

        dbg(My.Application.Info.AssemblyName + " " + SettingsStorage.Settings("Version"))

        PrintSerialPortNames()

        Do While Not InitGSM()
            If frmSettings.ShowDialog() <> DialogResult.OK Then
                Environment.Exit(1)
            End If
        Loop
    End Sub

    Private Shared Sub PrintSerialPortNames()
        dbg("Serial Ports: ")
        For Each n In My.Computer.Ports.SerialPortNames
            dbg(n)
        Next
    End Sub

    Private Shared Sub tests()
        'testDB()
        'Dim a = ParseString("1,2,3,""hello,\tworld"",""\r\n"",\r\n")
        'PDU.test()
    End Sub

    Private Shared Sub InitStorage()
        SettingsStorage = New SQLiteStorage(DatabasePath) 'New FsStorage(MainPath)
        Dim stype As Storage.StorageType = SettingsStorage.IntegerSettings("StorageType")
        Dim spath = SettingsStorage.Settings("StoragePath")
        '切换到新的存储
        If (stype = Storage.StorageType.None) OrElse (stype = Storage.StorageType.SQLite AndAlso spath.ToLower() = DatabasePath.ToLower()) Then
            '不需要更换Storage
            Storage = SettingsStorage
            dbg("Storage: Default")
        ElseIf stype = Storage.StorageType.SQLite Then
            Storage = New SQLiteStorage(spath)
            dbg("Storage: SQLite")
        ElseIf stype = Storage.StorageType.FileSystem Then
            Storage = New FileSystemStorage(spath)
            dbg("Storage: FileSystem")
        Else
            Throw New Exception("Unknown storage type")
        End If
    End Sub

    Public Shared Function InitGSM() As Boolean
        Try
            Dim commport = Globals.SettingsStorage.IntegerSettings("SerialPort")
            Dim baudrate = Globals.SettingsStorage.IntegerSettings("BaudRate")
            Globals.dbg(String.Format("Try openning COM{0} @ {1}bps", commport, baudrate))
            Application.DoEvents()
            Globals.GSM = New GSMModule(commport, baudrate)
        Catch ex As Exception
            Globals.dbg("打开串口失败: " & ex.Message)
            MsgBox("打开串口失败: " & ex.Message & vbCrLf & "请设置正确的串口号。", MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly, "错误")
            Try
                Globals.GSM.Close()
            Catch ex1 As Exception

            End Try
            Globals.GSM = Nothing
            Return False
            'Environment.Exit(1)
        End Try

        Try
            Globals.GSM.ModemDate() = Now '对时
            Globals.dbg(Globals.GSM.ModemDate().ToString)
        Catch ex As Exception
            Globals.dbg(ex.ToString())
        End Try
        Return True
    End Function

    Public Shared Sub testDB()
        Dim ss As New SQLiteStorage("sms.db3")
        ss.InsertContactsRecord("万呆", "+8612345678910")
        ss.InsertContactsRecord("10086", "10086")
        ss.Settings("a") = "b"
        Dim a = ss.Settings("SerialPort")
        Dim b = ss.ContactsByName("万呆")
        Dim c = ss.ContactsByNumber("+8612345678910")
        Dim d = ss.ContactsItem(0)
        'ss.EnqueueSend("123456", "您好")
        'Dim a = ss.DequeueSend
        'ss.AckSent(a.UUID, True)
        'ss.InsertReceived(New ShortMessage With {.Message = "hello, world", .RemoteAddress = "+8613000000000", .RemoteAddressType = ShortMessage.AddressType.International, .Time = Now})
        'Dim b = ss.ReadySendCount
        'Dim c = ss.ListReceived
        'ss.DeleteReceived(c(0).UUID)
        'Dim d = ss.ListReceived
    End Sub

    Public Shared Function SplitByLength(ByVal Source As String, ByVal Length As Integer) As String()
        Dim Count As Integer = Math.Ceiling(Source.Length / Length)
        Dim Ret(Count - 1) As String
        For i = 0 To Count - 1
            If i < Count - 1 Then
                Ret(i) = Source.Substring(0, Length)
                Source = Source.Substring(Length)
            Else
                Ret(i) = Source
            End If
        Next
        Return Ret
    End Function

    Public Shared Sub dbg(data As String)
        Console.WriteLine(data)
        Debug.Print(data)
        Try
            frmdbg.BeginInvoke(Sub()
                                   frmdbg.txtDebug.AppendText(data + vbCrLf)
                               End Sub)
        Catch ex As Exception

        End Try
    End Sub

    Public Shared Function ParseString(data As String) As String()
        Dim state As Integer = 0 '0: 普通, 1: 在引号中, 2: 在转义
        Dim stack As New Stack(Of Integer)
        Dim sb As New StringBuilder()
        Dim result As New List(Of String)
        For i = 0 To data.Length - 1
            Dim ch = data(i)
            Select Case state
                Case 0 '普通
                    If ch = "\"c Then
                        stack.Push(state) '开始转义
                        state = 2
                    ElseIf ch = """"c Then
                        stack.Push(state) '进入引号
                        state = 1
                    ElseIf ch = ","c Then
                        result.Add(sb.ToString()) '完成一段的解析, 放入结果中
                        sb = New StringBuilder()
                    Else
                        sb.Append(ch)
                    End If
                Case 1 '在引号中
                    If ch = "\"c Then
                        stack.Push(state) '开始转义
                        state = 2
                    ElseIf ch = """"c Then
                        state = stack.Pop() '出引号
                    Else
                        sb.Append(ch)
                    End If
                Case 2 '在转义
                    If ch = "\"c Then
                        sb.Append("\"c)
                    ElseIf ch = """"c Then
                        sb.Append(""""c)
                    ElseIf ch = "n"c Then
                        sb.Append(vbLf)
                    ElseIf ch = "t"c Then
                        sb.Append(vbTab)
                    ElseIf ch = "r"c Then
                        sb.Append(vbCr)
                    Else
                        sb.Append(ch)
                        'Throw New Exception("Unknown char")
                    End If
                    state = stack.Pop() '结束转义
                Case Else
                    Throw New Exception("Monster appeared")
            End Select
        Next
        result.Add(sb.ToString())
        Return result.ToArray()
    End Function

End Class
