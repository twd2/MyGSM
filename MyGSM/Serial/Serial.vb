Imports System.Text
Imports System.IO
Imports System.Threading
Imports System.Runtime.InteropServices

Public Class Serial
    Implements IDisposable

    Public WriteBufferSize = 64
    Public ReadBufferSize = 64

    Public Opened = False
    Public LineEnding As String = vbCrLf

    Private _hFile, _SerialName, _baudRate, _Timeout As Integer
    Private _LastDCB As Win32Native.DCB

    Sub New(ByVal COM As Integer, baudRate As Integer, ByVal Timeout As Integer)
        _SerialName = COM
        _baudRate = baudRate
        Open()
        Me.Timeout = Timeout
    End Sub

    Sub Open()
        _hFile = Win32Native.CreateFile("\\.\COM" & _SerialName.ToString, Win32Native.GENERIC_READ Or Win32Native.GENERIC_WRITE, 0, Nothing, Win32Native.OPEN_EXISTING, 0, Nothing)
        Win32Native.LastError()
        Dim MyDCB As New Win32Native.DCB
        Win32Native.GetCommState(_hFile, _LastDCB)
        Win32Native.BuildCommDCBA(String.Format("baud={0} parity=N data=8 stop=1", _baudRate), MyDCB)
        Win32Native.LastError()
        'MyDCB.DCBlength = System.Runtime.InteropServices.Marshal.SizeOf(MyDCB)
        Win32Native.SetCommState(_hFile, MyDCB)
        Win32Native.LastError()
        Opened = True
    End Sub

    ReadOnly Property hCom() As Integer
        Get
            Return _hFile
        End Get
    End Property

    Property Timeout() As Integer
        Get
            Return _Timeout
        End Get
        Set(ByVal value As Integer)
            _Timeout = value
            Dim CommTimeout As Win32Native.COMMTIMEOUTS
            CommTimeout.ReadIntervalTimeout = _Timeout
            CommTimeout.ReadTotalTimeoutConstant = _Timeout
            CommTimeout.ReadTotalTimeoutMultiplier = 1
            CommTimeout.WriteTotalTimeoutConstant = _Timeout
            CommTimeout.WriteTotalTimeoutMultiplier = 0
            Win32Native.SetCommTimeouts(_hFile, CommTimeout)
        End Set
    End Property

    Public Function Write(buff As Byte(), offset As Integer, count As Integer, Optional blockdelay As Integer = 0) As Integer

        Dim WrittenCount As Integer = 0, CurrCount As Integer = 0
        If blockdelay = 0 Then
            Do While WrittenCount < count
                SyncLock Me
                    Win32Native.SetLastError(0)
                    Win32Native.WriteFile(_hFile, buff(offset + WrittenCount), Math.Min(count - WrittenCount, WriteBufferSize), CurrCount, 0)
                    Win32Native.LastError()
                End SyncLock
                WrittenCount += CurrCount
            Loop
        Else
            Do While WrittenCount < count
                SyncLock Me
                    Win32Native.SetLastError(0)
                    Win32Native.WriteFile(_hFile, buff(offset + WrittenCount), Math.Min(count - WrittenCount, WriteBufferSize), CurrCount, 0)
                    Win32Native.LastError()
                End SyncLock
                WrittenCount += CurrCount
                Thread.Sleep(blockdelay)
            Loop
        End If
        Return WrittenCount

    End Function

    Public Function WriteLine(ByVal Data As String, Optional blockdelay As Integer = 0) As Integer
        Dim buff = Encoding.Default.GetBytes(Data & vbCr)
        Globals.dbg(">>>" & Data)
        Return Write(buff, 0, buff.Length, blockdelay) 'Win32Native.WriteFile(_hFile, Byts(0), Byts.Length, Ret, 0)
    End Function

    Public Function Read(buff As Byte(), offset As Integer, count As Integer) As Integer
        SyncLock Me
            Dim Ret As Integer
            Win32Native.SetLastError(0)
            Win32Native.ReadFile(_hFile, buff(offset), count, Ret, 0)
            Win32Native.LastError()
            Return Ret
        End SyncLock
    End Function

    Public Function ReadBlock() As String
        Dim buff(ReadBufferSize - 1) As Byte
        Dim byteread = Read(buff, 0, buff.Length)
        ReDim Preserve buff(byteread)
        Dim str = Encoding.Default.GetString(buff)
        Globals.dbg(str)
        Return str
    End Function

    Public Function ReadChar() As String
        Dim buff(0) As Byte
        Dim byteread = Read(buff, 0, buff.Length)
        If byteread > 0 Then
            Return Encoding.Default.GetString(buff)
        Else
            Return ""
            'Throw New Exception("An error occurred when reading")
        End If
    End Function

    Public Function ReadLine() As String
        Dim str = ""
        Do While (Not str.EndsWith(LineEnding)) AndAlso (Not str = ">")
            str += ReadChar()
            'Thread.Sleep(1)
        Loop
        If str.EndsWith(LineEnding) Then
            str = str.Substring(0, str.Length - LineEnding.Length)
        End If
        If str.Trim() <> "" Then
            Globals.dbg(str)
        End If
        Return str
    End Function

    Sub Close()
        Win32Native.SetCommState(_hFile, _LastDCB)
        Win32Native.CloseHandle(_hFile)
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
                _LastDCB = Nothing
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
