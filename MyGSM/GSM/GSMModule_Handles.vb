Imports System.Threading
Imports System.Text

Partial Public Class GSMModule

    Public lastCall As String = ""
    Private Sub CLIP_handle(data As String)
        Dim d = Globals.ParseString(data)
        OnCall(d(0))
    End Sub

    Private Sub CRING_handle(data As String)
        Dim d = Globals.ParseString(data)
        'ignore
    End Sub

    Private Sub CMTI_handle(data As String)
        Dim d = Globals.ParseString(data)
        Dim id = Integer.Parse(d(1))
        If NewSMSCallback IsNot Nothing Then
            NewSMSCallback.BeginInvoke(id, Nothing, Nothing)
        Else
            Globals.dbg("Warning: NewSMS not handled")
        End If
    End Sub

    Dim ModemClock As DateTime

    Private Sub CCLK_handle(data As String)
        ModemClock = DateTime.Parse(data.Replace("""", ""))
    End Sub

    Dim lstSMS As List(Of ShortMessage)

    Private Sub CMGL_handle(data As String)
        Dim d = Globals.ParseString(data)
        Dim id = Integer.Parse(d(0))
        Dim sms = PDU.Parse(_Serial.ReadLine())
        sms.IDForModule = id
        lstSMS.Add(sms)
    End Sub

    Private CSQ As String()

    Private Sub CSQ_handle(data As String)
        CSQ = Globals.ParseString(data)
    End Sub

    
End Class
