Public Class frmSettings

    Private Sub frmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PropertyGrid1.SelectedObject = SettingsObject.FromStorage(Globals.SettingsStorage)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        PropertyGrid1.SelectedObject = SettingsObject.FromStorage(Globals.SettingsStorage)
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim so = DirectCast(PropertyGrid1.SelectedObject, SettingsObject)
        so.SaveToStorage(Globals.SettingsStorage)
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub PropertyGrid1_Click(sender As Object, e As EventArgs) Handles PropertyGrid1.Click

    End Sub
End Class