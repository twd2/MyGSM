Public Class frmShowMessage

    Public sms As ShortMessage

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub frmShowMessage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtUUID.Text = sms.UUID
        txtMessage.Text = sms.Message
        txtReceiveTime.Text = sms.Time.ToString()
        txtSender.Text = sms.RemoteAddress
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Globals.Storage.DeleteReceived(sms.UUID)
        frmMain.RefreshReceivedList()
        Me.Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Using sfd As New SaveFileDialog
            sfd.Filter = "短消息(*.msg)|*.msg|文本文件(*.txt)|*.txt"
            sfd.Title = "另存为短消息"
            If sfd.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Globals.Storage.ExportReceived(sms.UUID, sfd.FileName)
                MsgBox("成功", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "另存为")
            End If
        End Using
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            frmSingleSend.TextBox1.Text = sms.RemoteAddress
            frmSingleSend.Show()
            frmSingleSend.TextBox2.Focus()
        Catch ex As Exception

        End Try
    End Sub
End Class