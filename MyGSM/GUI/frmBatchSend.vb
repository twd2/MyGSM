Imports System.IO

Public Class frmBatchSend

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Using OFD As New OpenFileDialog
            If OFD.ShowDialog = DialogResult.OK Then
                txtList.Text = File.ReadAllText(OFD.FileName)
            End If
        End Using
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        For Each Line As String In Split(txtList.Text, vbCrLf)
            If Line = "" OrElse Line.First = "'" Then
                Continue For
            End If
            Dim Values = Globals.ParseString(Line)
            Dim SendTo = String.Format(txtSendTo.Text, Values)
            Dim Msg = String.Format(txtMsg.Text, Values)
            Globals.Storage.EnqueueSend(SendTo, Msg)
        Next
        MsgBox("已加入发送队列", 64 + 0, "提示")
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        For Each Person As ContactPerson In Globals.Storage.ContactsAll
            txtList.AppendText(Person.Number & "," & Person.Name & vbCrLf)
        Next
    End Sub

    Private Sub frmBatchSend_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class