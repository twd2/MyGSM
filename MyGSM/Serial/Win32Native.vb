Imports System.Runtime.InteropServices

Public Class Win32Native
    Public Structure COMMTIMEOUTS
        Dim ReadIntervalTimeout As Long
        Dim ReadTotalTimeoutMultiplier As Long
        Dim ReadTotalTimeoutConstant As Long
        Dim WriteTotalTimeoutMultiplier As Long
        Dim WriteTotalTimeoutConstant As Long
    End Structure

    Public Const GENERIC_ALL = &H10000000
    Public Const GENERIC_READ As Integer = &H80000000
    Public Const GENERIC_WRITE As Integer = &H40000000
    Public Const OPEN_EXISTING = 3

    <DllImport("kernel32")>
    Public Shared Function CloseHandle(ByVal hObject As Integer) As Integer

    End Function

    <DllImport("kernel32")>
    Public Shared Function CreateFile(ByVal lpFileName As String, ByVal dwDesiredAccess As Integer, ByVal dwShareMode As Integer, ByVal lpSecurityAttributes As Integer, ByVal dwCreationDisposition As Integer, ByVal dwFlagsAndAttributes As Integer, ByVal hTemplateFile As Integer) As Integer

    End Function

    <DllImport("kernel32")>
    Public Shared Function GetLastError() As Integer

    End Function

    <DllImport("kernel32")>
    Public Shared Function SetLastError(errno As Integer) As Integer

    End Function

    <DllImport("kernel32")>
    Public Shared Function ReadFile(ByVal hFile As Integer, ByRef lpBuffer As Byte, ByVal nNumberOfBytesToRead As Integer, ByRef lpNumberOfBytesRead As Integer, ByVal lpOverlapped As Integer) As Integer

    End Function

    <DllImport("kernel32")>
    Public Shared Function WriteFile(ByVal hFile As Integer, ByRef lpBuffer As Byte, ByVal nNumberOfBytesToWrite As Integer, ByRef lpNumberOfBytesWritten As Integer, ByVal lpOverlapped As Integer) As Integer 'OVERLAPPED

    End Function

    <DllImport("kernel32")>
    Public Shared Function GetFileSize(ByVal hFile As Integer, ByRef lpFileSizeHigh As Integer) As Integer

    End Function

    <DllImport("kernel32")>
    Public Shared Function SetCommTimeouts(ByVal hFile As Integer, ByRef lpCommTimeouts As COMMTIMEOUTS) As Integer

    End Function

    <DllImport("kernel32")>
    Public Shared Function SetCommState(ByVal hCommDev As Integer, ByRef lpDCB As DCB) As Integer

    End Function

    <DllImport("kernel32")>
    Public Shared Function GetCommState(ByVal nCid As Integer, ByRef lpDCB As DCB) As Integer

    End Function

    <DllImport("kernel32")>
    Public Shared Function BuildCommDCBA(ByVal lpDef As String, ByRef lpDCB As DCB) As Long

    End Function
    Public Structure DCB
        Dim DCBlength As Integer
        Dim BaudRate As Integer
        Dim fBinary As Integer
        Dim fParity As Integer
        Dim fOutxCtsFlow As Integer
        Dim fOutxDsrFlow As Integer
        Dim fDtrControl As Integer
        Dim fDsrSensitivity As Integer
        Dim fTXContinueOnXoff As Integer
        Dim fOutX As Integer
        Dim fInX As Integer
        Dim fErrorChar As Integer
        Dim fNull As Integer
        Dim fRtsControl As Integer
        Dim fAbortOnError As Integer
        Dim fDummy2 As Integer
        Dim wReserved As Integer
        Dim XonLim As Integer
        Dim XoffLim As Integer
        Dim ByteSize As Byte
        Dim Parity As Byte
        Dim StopBits As Byte
        Dim XonChar As Byte
        Dim XoffChar As Byte
        Dim ErrorChar As Byte
        Dim EofChar As Byte
        Dim EvtChar As Byte
    End Structure

    Public Shared Sub LastError()
        Dim errno = Win32Native.GetLastError()
        If errno = 0 Then
            Return
        End If
        For Each item As String In Split(My.Resources.Win32Errors, vbCrLf)
            Dim Line = Split(item, " ")
            If Val(Line(0)) = errno Then
                Throw New Exception(Line(1))
            End If
        Next
    End Sub
End Class
