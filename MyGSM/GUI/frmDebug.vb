Public Class frmDebug

    Private Sub txtDebug_TextChanged(sender As Object, e As EventArgs) Handles txtDebug.TextChanged

    End Sub

    Private Sub frmDebug_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True
        Me.Hide()
    End Sub

    Private Sub frmDebug_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub txtUserCommand_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtUserCommand.KeyPress
        If e.KeyChar = vbCr Then
            Globals.GSM._Serial.WriteLine(txtUserCommand.Text)
            txtUserCommand.Text = ""
        End If
    End Sub

    Private Sub txtUserCommand_TextChanged(sender As Object, e As EventArgs) Handles txtUserCommand.TextChanged
      
    End Sub
End Class