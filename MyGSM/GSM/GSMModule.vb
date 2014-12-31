Imports System.Threading

Partial Public Class GSMModule
    Implements IDisposable

    Enum ResponseType
        OK
        CONNECT
        RING
        NOCARRIER
        [ERROR]
        NODIALTONE
        BUSY
        NOANSWER
        PROCEEDING
    End Enum

    Const Default_Timeout = 10000
    Const SMS_Timeout = 60000
    Const CtrlZ = Chr(26)

    Public isCalling As Boolean = False
    Public _Serial As Serial
    Public _isRunning As Boolean = False
    Private _Thread As Thread = Nothing
    Public RingCallback As Action = Nothing
    Public NewSMSCallback As Action(Of Integer) = Nothing
    Public ClosedCallback As Action = Nothing
    Private ResponseEvent As New ManualResetEvent(False)
    Private MoreDataEvent As New ManualResetEvent(False)
    Private lastResponse As ResponseType
    Public ATCommands As New Dictionary(Of String, Action(Of String))
    Private lastUnknownString As String = ""
    Public ModuleName As String = ""

    Sub New(ByVal Comm As Integer, Optional BaudRate As Integer = 9600)
        initCommands()
        _Serial = New Serial(Comm, BaudRate, 10)
        _isRunning = True

        _Thread = New Thread(AddressOf ReadThreadEntryPoint)
        _Thread.Start()

        Try
            initModule()
        Catch ex As Exception
            Try
                _Serial.Close()
            Catch ex1 As Exception

            End Try
            Throw
        End Try

    End Sub

    Private Sub initModule()
        SendCommand("AT")
        SendCommand("AT+CGMI")
        ModuleName = lastUnknownString
        InitConfig()
        ATHAll()
    End Sub

    Private Sub InitConfig()
        Globals.dbg("AT+CLIP=1 " + TryCommand("AT+CLIP=1").ToString) '来电显示
    End Sub

    Private Sub initCommands()
        ATCommands.Add("CSQ", AddressOf CSQ_handle)
        ATCommands.Add("CMGL", AddressOf CMGL_handle)
        ATCommands.Add("CMTI", AddressOf CMTI_handle)
        ATCommands.Add("CCLK", AddressOf CCLK_handle)
        ATCommands.Add("CLIP", AddressOf CLIP_handle)
        ATCommands.Add("CRING", AddressOf CRING_handle)
    End Sub

    Private Sub SendCommand(cmd As String, Optional timeout As Integer = Default_Timeout, Optional blockdelay As Integer = 0)
        SyncLock Me
            ResponseEvent.Reset()
            _Serial.WriteLine(cmd, blockdelay)
            AssertOK(timeout)
        End SyncLock
    End Sub

    Private Function TryCommand(cmd As String, Optional timeout As Integer = Default_Timeout, Optional blockdelay As Integer = 0)
        SyncLock Me
            ResponseEvent.Reset()
            _Serial.WriteLine(cmd, blockdelay)
            Return GetResponse(timeout)
        End SyncLock
    End Function


    Private Sub ReadThreadEntryPoint()
        Try
            Do While _isRunning
                Dim line = ""
                line = _Serial.ReadLine()
                If line.Trim() = "" Then
                    Continue Do
                End If
                Select Case line(0)
                    Case "+"c 'AT command
                        Dim cmddata = line.Split({":"c}, 2)
                        cmddata(0) = cmddata(0).TrimStart("+"c)
                        If ATCommands.ContainsKey(cmddata(0)) Then
                            ATCommands(cmddata(0))(cmddata(1).Trim())
                        Else
                            Globals.dbg("Warning: " + cmddata(0) + " not handled")
                        End If
                    Case "O"c 'OK
                        If line = "OK" Then
                            lastResponse = ResponseType.OK
                            ResponseEvent.Set()
                        End If
                    Case "C"c 'CONNECT
                        If line = "CONNECT" Then
                            lastResponse = ResponseType.CONNECT
                            ResponseEvent.Set()
                        End If
                    Case "R"c 'RING
                        If line = "RING" Then
                            OnCall("")
                        End If
                    Case "E"c 'ERROR
                        If line = "ERROR" Then
                            lastResponse = ResponseType.ERROR
                            ResponseEvent.Set()
                        End If
                    Case "B"c 'BUSY
                        If line = "BUSY" Then
                            lastResponse = ResponseType.BUSY
                            isCalling = False
                            ResponseEvent.Set()
                        End If
                    Case "N"c 'NO CARRIER, NO DIALTONE, NO ANSWER
                        isCalling = False
                        If line = "NO CARRIER" Then
                            lastResponse = ResponseType.NOCARRIER
                            ResponseEvent.Set()
                        ElseIf line = "NO DIALTONE" Then
                            lastResponse = ResponseType.NODIALTONE
                            ResponseEvent.Set()
                        ElseIf line = "NO ANSWER" Then
                            lastResponse = ResponseType.NOANSWER
                            ResponseEvent.Set()
                        End If
                    Case "P"c 'PROCEEDING
                        If line = "PROCEEDING" Then
                            lastResponse = ResponseType.PROCEEDING
                            ResponseEvent.Set()
                        End If
                    Case ">"c 'need more data
                        MoreDataEvent.Set()
                    Case Else
                        lastUnknownString = line
                End Select
            Loop
        Catch ex As Exception
            'Globals.dbg(ex.ToString())
        End Try
        _isRunning = False
        If ClosedCallback IsNot Nothing Then
            ClosedCallback.BeginInvoke(Nothing, Nothing)
        End If
    End Sub

    Private Sub AssertResponse(res As ResponseType, Optional timeout As Integer = Default_Timeout)
        If GetResponse(timeout) <> res Then
            Throw New Exception("Modem " + lastResponse.ToString())
        End If
    End Sub

    Private Sub AssertOK(Optional timeout As Integer = Default_Timeout)
        AssertResponse(ResponseType.OK, timeout)
    End Sub

    Private Function GetResponse(Optional timeout As Integer = Default_Timeout) As ResponseType
        If Not ResponseEvent.WaitOne(timeout) Then
            Throw New Exception("Modem Timeout")
        End If
        Return lastResponse
    End Function

    Public Property ModemDate() As DateTime
        Get
            SendCommand("AT+CCLK?")
            Return ModemClock
            'Return ParseDate(Split(_Serial.Read, vbCrLf)(1).Substring(7)) 'DateTime.Parse(Split(Replace(Split(m_Comm.Read(), vbCrLf)(1).Substring(7), """", ""), "+")(0))
            'Dim ret = Replace(Split(m_Comm.Read(), vbCrLf)(1).Substring(7), """", "").Split({"/", ",", ":"}, StringSplitOptions.RemoveEmptyEntries)
            'Dim dt As New DateTime(Val(ret(0)), Val(ret(1)), Val(ret(2)), Val(ret(3)), Val(ret(4)), Val(ret(5)))
            'Return dt
        End Get
        Set(ByVal value As DateTime)
            Dim dt = value.ToUniversalTime().ToString("yy/MM/dd,HH:mm:ss")
            SendCommand("AT+CCLK=""" & dt & "+00""")
        End Set
    End Property

    Public Function GetCSQ() As String()
        SendCommand("AT+CSQ")
        Return CSQ
    End Function

    Public Function SendMessage(ByVal sms As ShortMessage) As Boolean
        SendCommand("AT")
        SendCommand("AT+CSCS=""GSM""")
        SendCommand("AT+CSMP=17,167,0,8")
        SendCommand("AT+CMGF=0")

        Dim pdudata = PDU.ToPDU(sms) ' "00"

        MoreDataEvent.Reset()
        _Serial.WriteLine("AT+CMGS=" + (pdudata.Length / 2 - 1).ToString())
        MoreDataEvent.WaitOne()
        Try
            SendCommand(pdudata + CtrlZ, SMS_Timeout, 100) '发送短信的超时时间要长一些, 每发64B需要等100ms
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Public Function SendMessage(ByVal RemoteAddress As String, ByVal Message As String) As Boolean
        Dim sms As New ShortMessage
        sms.RemoteAddress = RemoteAddress
        sms.Message = Message
        Return SendMessage(sms)
    End Function

    Public Function ReadMsg() As List(Of ShortMessage)
        Return ReadMessage(False)
    End Function

    Public Function ReadMessage(ByVal DeleteRead As Boolean) As List(Of ShortMessage)
        SendCommand("AT")
        lstSMS = New List(Of ShortMessage)
        SendCommand("AT+CMGF=0")
        SendCommand("AT+CMGL=4")
        Dim mysms = lstSMS
        lstSMS = New List(Of ShortMessage)
        If DeleteRead Then
            For Each sms In mysms
                DeleteMsg(sms.IDForModule)
            Next
        End If
        Return mysms
    End Function

    Public Sub DeleteMsg(ByVal ID As Integer)
        SendCommand("AT+CMGD=" & ID)
    End Sub

    Public Function ATD(number As String, Optional isData As Boolean = False) As Boolean
        isCalling = True
        SyncLock Me
            ResponseEvent.Reset()
            _Serial.WriteLine("ATD" + number + IIf(isData, "", ";"))
            Dim res = GetResponse(Default_Timeout)
            If res <> ResponseType.OK Then
                isCalling = False
                Return False
            End If
        End SyncLock
        Return True
    End Function

    Public Sub ATHAll()
        SendCommand("ATH")
        isCalling = False
    End Sub

    Public Sub ATHWaiting()
        SendCommand("ATH5")
        isCalling = False
    End Sub

    Public Function TryATH()
        Try
            ATHWaiting()
            isCalling = False
            Return True
        Catch ex As Exception
            Try
                ATHAll()
                isCalling = False
                Return True
            Catch ex1 As Exception

            End Try
        End Try
        Return False
    End Function

    Public Sub ATA()
        SendCommand("ATA")
        isCalling = True
    End Sub

    Public Sub DTMFTone(ch As Char, Optional time As Byte = 1)
        SendCommand("AT+VTD=" + time.ToString())
        SendCommand("AT+VTS=""" + ch + """")
    End Sub

    Private Sub OnCall(number As String)
        lastCall = number
        If isCalling Then
            Globals.dbg("Warning: Is calling, refusing new call")
            Dim t As New Thread(Sub()
                                    'Cannot ATHAll because it's calling.
                                    Try
                                        ATHWaiting()
                                    Catch ex As Exception

                                    End Try
                                End Sub)
            t.Start()
        Else
            If RingCallback IsNot Nothing Then
                RingCallback.BeginInvoke(Nothing, Nothing)
            Else
                Globals.dbg("Warning: RING not handled, refusing")
                Dim t As New Thread(Sub()
                                       TryATH()
                                    End Sub)
                t.Start()
            End If
        End If
      
    End Sub

    Public Sub Close()
        Try
            ATHAll()
        Catch ex As Exception

        End Try
        _Serial.Close()
        _isRunning = False
        If _Thread IsNot Nothing Then
            _Thread.Join()
        End If
        _Thread = Nothing
        ResponseEvent.Close()
        MoreDataEvent.Close()
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 检测冗余的调用

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                Try
                    Close()
                Catch ex As Exception

                End Try
                '释放托管状态(托管对象)。
            End If

            '释放非托管资源(非托管对象)并重写下面的 Finalize()。
            '将大型字段设置为 null。
        End If
        Me.disposedValue = True
    End Sub

    '仅当上面的 Dispose(ByVal disposing As Boolean)具有释放非托管资源的代码时重写 Finalize()。
    'Protected Overrides Sub Finalize()
    '    ' 不要更改此代码。    请将清理代码放入上面的 Dispose(ByVal disposing As Boolean)中。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' Visual Basic 添加此代码是为了正确实现可处置模式。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' 不要更改此代码。    请将清理代码放入上面的 Dispose (disposing As Boolean)中。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
