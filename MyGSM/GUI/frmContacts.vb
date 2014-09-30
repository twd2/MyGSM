Public Class frmContacts

    Private Sub frmContacts_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        RefreshList()
    End Sub

    Sub RefreshList()
        ContactsList.Items.Clear()
        For Each cp As ContactPerson In Globals.Storage.ContactsAll
            Dim item As New ListViewItem(cp.Name)
            item.SubItems.Add(cp.Number)
            item.SubItems.Add(cp.Remark)
            ContactsList.Items.Add(item)
        Next
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Globals.Storage.InsertContactsRecord(txtName.Text, txtNumber.Text, txtRemark.Text)
        RefreshList()
    End Sub

    Private Sub 发送短信SToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 发送短信SToolStripMenuItem.Click

    End Sub

    Private Sub 删除DToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 删除DToolStripMenuItem.Click
        Try
            Globals.Storage.DeleteContactsRecordAt(ContactsList.SelectedItems(0).Index)
            RefreshList()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub 呼叫CToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 呼叫CToolStripMenuItem.Click
        Try
            Dim num = ContactsList.SelectedItems(0).SubItems(1).Text
          
        Catch ex As Exception

        End Try
    End Sub
End Class