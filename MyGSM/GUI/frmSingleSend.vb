Imports System.IO

Public Class frmSingleSend

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        'TextBox2.Text = Mid(TextBox2.Text, 1, 70)
        Label3.Text = "字数统计: " & TextBox2.TextLength.ToString & " 字"
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Globals.Storage.EnqueueSend(TextBox1.Text, TextBox2.Text)
        If MsgBox("已加入发送队列, 是否立即处理队列?", MsgBoxStyle.Information Or MsgBoxStyle.OkCancel, "提示") = MsgBoxResult.Ok Then
            frmMain.SendGetMsgAsync()
        End If
        Me.Close()
    End Sub

    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label3.Click

    End Sub
End Class