Imports System.IO
Imports System.Text
Imports System.Threading

Public Class frmMain

    Private lvwColumnSorter As ListViewColumnSorter

    Private Sub frmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Globals.GSM.ClosedCallback = Nothing
        If Globals.GSM IsNot Nothing Then
            Globals.GSM.Close()
        End If
    End Sub

    Dim sentInfo As New List(Of String)
    Private Sub OnRing()
        If Globals.SettingsStorage.BooleanSettings("AutoATH") Then
            If Globals.GSM.lastCall <> "" Then
                ShowMsg("有来电来自: " + Globals.GSM.lastCall + ", 已自动挂断", ToolTipIcon.Info)
                Globals.GSM.ATHWaiting()
                If sentInfo.FindIndex(Function(s As String)
                                          Return s = Globals.GSM.lastCall
                                      End Function) < 0 Then '还未发送过提示消息
                    Globals.Storage.EnqueueSend(Globals.GSM.lastCall, "本号码现在由计算机程序控制，不方便接听电话。")
                    sentInfo.Add(Globals.GSM.lastCall)
                    SendGetMsgAsync()
                End If
            Else
                ShowMsg("有未知号码来电, 已自动挂断", ToolTipIcon.Info)
                Globals.GSM.ATHWaiting()
            End If
        Else
            If Globals.GSM.lastCall <> "" Then
                ShowMsg("有来电来自: " + Globals.GSM.lastCall + ", 请注意处理", ToolTipIcon.Info)
            Else
                ShowMsg("有未知号码来电, 请注意处理", ToolTipIcon.Info)
            End If
        End If
    End Sub

    Private Sub OnNewSMS()
        ShowMsg("有新消息, 正在接收", ToolTipIcon.Info)
        SendGetMsg()
    End Sub
    Private Sub OnModemClosed()
        MsgBox("GSM模块连接已断开", MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly, "错误")
        Environment.Exit(1)
    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Globals.Init()

        MyPort.Text = "COM" & Globals.SettingsStorage.Settings("SerialPort")

        Globals.GSM.NewSMSCallback = AddressOf OnNewSMS
        Globals.GSM.RingCallback = AddressOf OnRing
        Globals.GSM.ClosedCallback = AddressOf OnModemClosed

        '创建一个ListView排序类的对象，并设置listView1的排序器
        lvwColumnSorter = New ListViewColumnSorter()
        MsgRevList.ListViewItemSorter = lvwColumnSorter

        SendGetMsgAsync()

        lvwColumnSorter.Order = SortOrder.Descending
        lvwColumnSorter.SortColumn = 1
        MsgRevList.Sort()

    End Sub

    Sub SendGetMsgAsync()
        Dim MyThread As New Thread(AddressOf SendGetMsg)
        MyThread.Start()
    End Sub

    Sub SendGetMsg()
        SyncLock Globals.GSM
            'Control.CheckForIllegalCrossThreadCalls = False
            'Timer1.Enabled = False
            Me.BeginInvoke(Sub()
                               处理队列DToolStripMenuItem.Enabled = False
                           End Sub)
            Try
                ReStatus("刷新")
                ReStatus("正在检测信号强度")
                Dim CSQ = Val(Globals.GSM.GetCSQ(0)) '检测信号强度
                Me.BeginInvoke(Sub()
                                   Signal.Value = IIf(CSQ < 31, CSQ, 0)
                               End Sub)
                ReStatus("正在发送")
                Me.BeginInvoke(Sub()
                                   SRProgress.Value = 0
                               End Sub)
                Dim totalCount = Globals.Storage.ReadySendCount
                Do While Globals.Storage.ReadySendCount > 0
                    Dim sms = Globals.Storage.DequeueSend()
                    Try
                        Dim Succeeded = Globals.GSM.SendMessage(sms.RemoteAddress, sms.Message)
                        Globals.Storage.AckSent(sms.UUID, Succeeded)
                    Catch ex As Exception
                        MsgBox(ex.ToString)
                    End Try
                    Dim finishedCount = totalCount - Globals.Storage.ReadySendCount
                    Me.BeginInvoke(Sub()
                                       SRProgress.Value = finishedCount / totalCount * 100
                                   End Sub)
                Loop
                'Dim FileNames = Directory.GetFiles(Utils.ReadyPath)
                'For i = 0 To FileNames.Length - 1
                '    Dim FileName = FileNames(i)
                '    Dim Success As Boolean = True
                '    Try
                '        Using Rd As New StreamReader(FileName)
                '            Dim Receiver = Rd.ReadLine
                '            Dim Data = Rd.ReadToEnd
                '            Success = m_WaveCom.SendMsg(Receiver, Data)
                '        End Using
                '    Catch ex As Exception
                '        MsgBox(ex.ToString)
                '    End Try
                '    File.WriteAllText(Path.Combine(Utils.SentPath, IIf(Success, "Success", "Failed") & "\" & Format(Now, "yyyyMMdd_hhmmss_ffff") & ".msg"),
                '                      File.ReadAllText(FileName)) 'Split(FileName, "\").Last
                '    File.Delete(FileName)
                '    Dim id = i
                '    Me.BeginInvoke(Sub()
                '                       SRProgress.Value = (id + 1) / (FileNames.Length) * 100
                '                   End Sub)
                'Next
                ReStatus("正在接收")
                Dim Msgs = Globals.GSM.ReadMessage(True)
                If Msgs.Count > 0 Then
                    Beep()
                    ShowMsg("收到新消息", ToolTipIcon.Info)
                End If
                For Each sms As ShortMessage In Msgs
                    Globals.Storage.InsertReceived(sms)
                Next
                ReStatus("完毕")
            Catch ex As Exception
                Globals.dbg(ex.ToString)
                ShowMsg(ex.Message, ToolTipIcon.Error)
            End Try
        End SyncLock
        RefreshReceivedList()
        'Timer1.Enabled = True
        Me.BeginInvoke(Sub()
                           处理队列DToolStripMenuItem.Enabled = True
                       End Sub)
    End Sub

    Sub ReStatus(ByVal Status As String)
        Me.BeginInvoke(Sub()
                           MyStatus.Text = Status
                       End Sub)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim MyThread As New Thread(AddressOf SendGetMsg)
        MyThread.Start()
    End Sub

    Sub ShowMsg(ByVal Msg As String, ByVal Ico As ToolTipIcon)
        Me.BeginInvoke(Sub()
                           MyIcon.BalloonTipIcon = Ico
                           MyIcon.BalloonTipText = Msg
                           MyIcon.ShowBalloonTip(100)
                       End Sub)
    End Sub

    Private Sub 退出ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 退出ToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub MyIcon_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyIcon.MouseDoubleClick
        Me.Show()
        WindowState = FormWindowState.Normal
    End Sub


    Private Sub frmMain_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged
        If WindowState = FormWindowState.Minimized Then
            Me.Hide()
            ShowMsg("已隐藏, 双击此图标以恢复", ToolTipIcon.Info)
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        frmSingleSend.Show()
    End Sub

    Private Sub MsgRevList_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles MsgRevList.ColumnClick
        ' 检查点击的列是不是现在的排序列.
        If e.Column = lvwColumnSorter.SortColumn Then
            ' 重新设置此列的排序方法.
            If lvwColumnSorter.Order = SortOrder.Ascending Then
                lvwColumnSorter.Order = SortOrder.Descending
            Else
                lvwColumnSorter.Order = SortOrder.Ascending
            End If
        Else
            ' 设置排序列，默认为正向排序
            lvwColumnSorter.SortColumn = e.Column
            lvwColumnSorter.Order = SortOrder.Ascending
        End If

        ' 用新的排序方法对ListView排序
        MsgRevList.Sort()
    End Sub

    Private Sub MsgList_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles MsgRevList.DoubleClick
        Try
            Dim sms = Globals.Storage.GetReceived(MsgRevList.SelectedItems(0).Text)
            Dim showmsg As New frmShowMessage
            showmsg.sms = sms
            showmsg.Show()
        Catch ex As Exception

        End Try
    End Sub

    Sub RefreshReceivedList()
        Try
            MsgRevList.BeginInvoke(Sub()
                                       MsgRevList.BeginUpdate()
                                       MsgRevList.Items.Clear()
                                       For Each sms In Globals.Storage.ListReceived()
                                           Dim item As New ListViewItem(sms.UUID)
                                           item.SubItems.Add(sms.Time) '时间
                                           item.SubItems.Add(Globals.Storage.ContactsToName(sms.RemoteAddressDisplay)) '发送人
                                           item.SubItems.Add(sms.Message)
                                           MsgRevList.Items.Add(item)
                                       Next
                                       'For Each FileName In Directory.GetFiles(Utils.RecvPath)
                                       '    Try
                                       '        Using Rd As New StreamReader(FileName)
                                       '            Dim item As New ListViewItem(Split(Split(FileName, "\").Last, ".").First)
                                       '            Rd.ReadLine() 'ID
                                       '            item.SubItems.Add(Rd.ReadLine) '时间
                                       '            Dim SenderNumber = Rd.ReadLine, SenderName = Contacts.GetNameByNumber(SenderNumber)

                                       '            item.SubItems.Add(IIf(SenderName <> "", SenderName, SenderNumber))
                                       '            item.SubItems.Add(Rd.ReadToEnd)
                                       '            MsgRevList.Items.Add(item)
                                       '        End Using
                                       '    Catch ex As Exception

                                       '    End Try
                                       'Next
                                       MsgRevList.EndUpdate()
                                   End Sub)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        RefreshReceivedList()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        frmBatchSend.Show()
    End Sub

    Private Sub 删除DToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 删除DToolStripMenuItem.Click
        Try
            Globals.Storage.DeleteReceived(MsgRevList.SelectedItems(0).Text)
            RefreshReceivedList()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub 回复RToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 回复RToolStripMenuItem.Click
        Try
            frmSingleSend.TextBox1.Text = Globals.Storage.GetReceived(MsgRevList.SelectedItems(0).Text).RemoteAddress 'File.ReadAllLines(Path.Combine(Utils.RecvPath, MsgRevList.SelectedItems(0).Text & ".msg"))(2)
            frmSingleSend.Show()
            frmSingleSend.TextBox2.Focus()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub 处理队列DToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 处理队列DToolStripMenuItem.Click
        Dim MyThread As New Thread(AddressOf SendGetMsg)
        MyThread.Start()
    End Sub

    Private Sub 批量发送BToolStripMenuItem_Click(sender As Object, e As EventArgs)
        frmBatchSend.Show()
    End Sub

    Private Sub 刷新列表RToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 刷新列表RToolStripMenuItem.Click
        RefreshReceivedList()
    End Sub

    Private Sub MenuStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub

    Private Sub 控制台CToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 控制台CToolStripMenuItem.Click
        Globals.frmdbg.Show()
    End Sub

    Private Sub 通讯录CToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles 通讯录CToolStripMenuItem1.Click
        frmContacts.Show()
    End Sub

    Private Sub 发送短消息SToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles 发送短消息SToolStripMenuItem1.Click
        frmSingleSend.Show()
    End Sub

    Private Sub 批量发送DToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 批量发送DToolStripMenuItem.Click
        frmBatchSend.Show()
    End Sub

    Private Sub 参数设置CToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 参数设置CToolStripMenuItem.Click
        frmSettings.Show()
    End Sub

    Private Sub 拨号DToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 拨号DToolStripMenuItem.Click
        frmDialing.Show()
    End Sub
End Class