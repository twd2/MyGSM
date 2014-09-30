Public Class frmDialing

    Private Sub TryATD()
        If Not Globals.GSM.ATD(txtNumber.Text) Then
            MsgBox("失败", MsgBoxStyle.Critical, "")
        End If
    End Sub

    Private Sub txtCall_Click(sender As Object, e As EventArgs) Handles txtCall.Click
        TryATD()
    End Sub

    Private Sub ButtonNumber_Click(sender As Object, e As EventArgs) Handles b1.Click, b2.Click, b3.Click, bA.Click,
                                                                         b4.Click, b5.Click, b6.Click, bB.Click,
                                                                         b7.Click, b8.Click, b9.Click, bC.Click,
                                                                         bAsterisk.Click, b0.Click, bHashtag.Click, bD.Click
        If Not sender.GetType().Equals(GetType(Button)) Then
            Return
        End If
        Dim butt = DirectCast(sender, Button)
        Dim ch = butt.Text
        txtNumber.AppendText(ch)
        AddNumber(ch)
    End Sub

    Private Sub AddNumber(ch As String)
        If ch = "" Then
            Throw New Exception("Empty string")
        End If
        If Globals.GSM.isCalling Then
            Try
                Globals.GSM.DTMFTone(ch(0))
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub txtHangUp_Click(sender As Object, e As EventArgs) Handles txtHangUp.Click
        Try
            Globals.GSM.ATHAll()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtNumber_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNumber.KeyPress
        If e.KeyChar = vbCr Then
            TryATD()
        Else
            AddNumber(e.KeyChar)
        End If
    End Sub

    Private Sub txtNumber_TextChanged(sender As Object, e As EventArgs) Handles txtNumber.TextChanged

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Globals.GSM.ATA()
        Catch ex As Exception

        End Try
    End Sub
End Class