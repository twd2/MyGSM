<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.MyStatusBar = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.MyPort = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel3 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Signal = New System.Windows.Forms.ToolStripProgressBar()
        Me.ToolStripStatusLabel4 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.SRProgress = New System.Windows.Forms.ToolStripProgressBar()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.MyStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.MyIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.IconMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.退出ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MsgRevList = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.RevMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.回复RToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.删除DToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripStatusLabel5 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.处理队列DToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.刷新列表RToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.功能FToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.通讯录CToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.发送短消息SToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.批量发送DToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.拨号DToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.高级SToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.参数设置CToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.控制台CToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MyStatusBar.SuspendLayout()
        Me.IconMenu.SuspendLayout()
        Me.RevMenu.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MyStatusBar
        '
        Me.MyStatusBar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel2, Me.MyPort, Me.ToolStripStatusLabel3, Me.Signal, Me.ToolStripStatusLabel4, Me.SRProgress, Me.ToolStripStatusLabel1, Me.MyStatus})
        Me.MyStatusBar.Location = New System.Drawing.Point(0, 516)
        Me.MyStatusBar.Name = "MyStatusBar"
        Me.MyStatusBar.Padding = New System.Windows.Forms.Padding(1, 0, 16, 0)
        Me.MyStatusBar.Size = New System.Drawing.Size(864, 26)
        Me.MyStatusBar.TabIndex = 1
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(58, 21)
        Me.ToolStripStatusLabel2.Text = "端口号:"
        '
        'MyPort
        '
        Me.MyPort.Name = "MyPort"
        Me.MyPort.Size = New System.Drawing.Size(18, 21)
        Me.MyPort.Text = "0"
        '
        'ToolStripStatusLabel3
        '
        Me.ToolStripStatusLabel3.Name = "ToolStripStatusLabel3"
        Me.ToolStripStatusLabel3.Size = New System.Drawing.Size(73, 21)
        Me.ToolStripStatusLabel3.Text = "信号强度:"
        '
        'Signal
        '
        Me.Signal.Maximum = 30
        Me.Signal.Name = "Signal"
        Me.Signal.Size = New System.Drawing.Size(133, 20)
        '
        'ToolStripStatusLabel4
        '
        Me.ToolStripStatusLabel4.Name = "ToolStripStatusLabel4"
        Me.ToolStripStatusLabel4.Size = New System.Drawing.Size(73, 21)
        Me.ToolStripStatusLabel4.Text = "收发进度:"
        '
        'SRProgress
        '
        Me.SRProgress.Name = "SRProgress"
        Me.SRProgress.Size = New System.Drawing.Size(133, 20)
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(43, 21)
        Me.ToolStripStatusLabel1.Text = "状态:"
        '
        'MyStatus
        '
        Me.MyStatus.Name = "MyStatus"
        Me.MyStatus.Size = New System.Drawing.Size(39, 21)
        Me.MyStatus.Text = "就绪"
        '
        'MyIcon
        '
        Me.MyIcon.BalloonTipTitle = "短信管理面板"
        Me.MyIcon.ContextMenuStrip = Me.IconMenu
        Me.MyIcon.Icon = CType(resources.GetObject("MyIcon.Icon"), System.Drawing.Icon)
        Me.MyIcon.Text = "短信管理面板"
        Me.MyIcon.Visible = True
        '
        'IconMenu
        '
        Me.IconMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.退出ToolStripMenuItem})
        Me.IconMenu.Name = "IconMenu"
        Me.IconMenu.Size = New System.Drawing.Size(129, 28)
        '
        '退出ToolStripMenuItem
        '
        Me.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem"
        Me.退出ToolStripMenuItem.Size = New System.Drawing.Size(128, 24)
        Me.退出ToolStripMenuItem.Text = "退出(&X)"
        '
        'MsgRevList
        '
        Me.MsgRevList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MsgRevList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader3, Me.ColumnHeader2, Me.ColumnHeader4})
        Me.MsgRevList.ContextMenuStrip = Me.RevMenu
        Me.MsgRevList.FullRowSelect = True
        Me.MsgRevList.GridLines = True
        Me.MsgRevList.Location = New System.Drawing.Point(16, 32)
        Me.MsgRevList.Margin = New System.Windows.Forms.Padding(4)
        Me.MsgRevList.MultiSelect = False
        Me.MsgRevList.Name = "MsgRevList"
        Me.MsgRevList.Size = New System.Drawing.Size(831, 478)
        Me.MsgRevList.TabIndex = 3
        Me.MsgRevList.UseCompatibleStateImageBehavior = False
        Me.MsgRevList.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "ID"
        Me.ColumnHeader1.Width = 134
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "时间"
        Me.ColumnHeader3.Width = 140
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "来自"
        Me.ColumnHeader2.Width = 100
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "内容"
        Me.ColumnHeader4.Width = 146
        '
        'RevMenu
        '
        Me.RevMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.回复RToolStripMenuItem, Me.删除DToolStripMenuItem})
        Me.RevMenu.Name = "RevMenu"
        Me.RevMenu.Size = New System.Drawing.Size(130, 52)
        '
        '回复RToolStripMenuItem
        '
        Me.回复RToolStripMenuItem.Name = "回复RToolStripMenuItem"
        Me.回复RToolStripMenuItem.Size = New System.Drawing.Size(129, 24)
        Me.回复RToolStripMenuItem.Text = "回复(&R)"
        '
        '删除DToolStripMenuItem
        '
        Me.删除DToolStripMenuItem.Name = "删除DToolStripMenuItem"
        Me.删除DToolStripMenuItem.Size = New System.Drawing.Size(129, 24)
        Me.删除DToolStripMenuItem.Text = "删除(&D)"
        '
        'ToolStripStatusLabel5
        '
        Me.ToolStripStatusLabel5.Name = "ToolStripStatusLabel5"
        Me.ToolStripStatusLabel5.Size = New System.Drawing.Size(170, 20)
        Me.ToolStripStatusLabel5.Text = "ToolStripStatusLabel5"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.处理队列DToolStripMenuItem, Me.刷新列表RToolStripMenuItem, Me.功能FToolStripMenuItem, Me.高级SToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(864, 28)
        Me.MenuStrip1.TabIndex = 7
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        '处理队列DToolStripMenuItem
        '
        Me.处理队列DToolStripMenuItem.Name = "处理队列DToolStripMenuItem"
        Me.处理队列DToolStripMenuItem.Size = New System.Drawing.Size(102, 24)
        Me.处理队列DToolStripMenuItem.Text = "处理队列(&D)"
        '
        '刷新列表RToolStripMenuItem
        '
        Me.刷新列表RToolStripMenuItem.Name = "刷新列表RToolStripMenuItem"
        Me.刷新列表RToolStripMenuItem.Size = New System.Drawing.Size(101, 24)
        Me.刷新列表RToolStripMenuItem.Text = "刷新列表(&R)"
        '
        '功能FToolStripMenuItem
        '
        Me.功能FToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.通讯录CToolStripMenuItem1, Me.发送短消息SToolStripMenuItem1, Me.批量发送DToolStripMenuItem, Me.拨号DToolStripMenuItem})
        Me.功能FToolStripMenuItem.Name = "功能FToolStripMenuItem"
        Me.功能FToolStripMenuItem.Size = New System.Drawing.Size(69, 24)
        Me.功能FToolStripMenuItem.Text = "功能(&F)"
        '
        '通讯录CToolStripMenuItem1
        '
        Me.通讯录CToolStripMenuItem1.Name = "通讯录CToolStripMenuItem1"
        Me.通讯录CToolStripMenuItem1.Size = New System.Drawing.Size(172, 24)
        Me.通讯录CToolStripMenuItem1.Text = "通讯录(&C)"
        '
        '发送短消息SToolStripMenuItem1
        '
        Me.发送短消息SToolStripMenuItem1.Name = "发送短消息SToolStripMenuItem1"
        Me.发送短消息SToolStripMenuItem1.Size = New System.Drawing.Size(172, 24)
        Me.发送短消息SToolStripMenuItem1.Text = "发送短消息(&S)"
        '
        '批量发送DToolStripMenuItem
        '
        Me.批量发送DToolStripMenuItem.Name = "批量发送DToolStripMenuItem"
        Me.批量发送DToolStripMenuItem.Size = New System.Drawing.Size(172, 24)
        Me.批量发送DToolStripMenuItem.Text = "批量发送(&B)"
        '
        '拨号DToolStripMenuItem
        '
        Me.拨号DToolStripMenuItem.Name = "拨号DToolStripMenuItem"
        Me.拨号DToolStripMenuItem.Size = New System.Drawing.Size(172, 24)
        Me.拨号DToolStripMenuItem.Text = "拨号(&D)"
        '
        '高级SToolStripMenuItem
        '
        Me.高级SToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.参数设置CToolStripMenuItem, Me.控制台CToolStripMenuItem})
        Me.高级SToolStripMenuItem.Name = "高级SToolStripMenuItem"
        Me.高级SToolStripMenuItem.Size = New System.Drawing.Size(72, 24)
        Me.高级SToolStripMenuItem.Text = "高级(&A)"
        '
        '参数设置CToolStripMenuItem
        '
        Me.参数设置CToolStripMenuItem.Name = "参数设置CToolStripMenuItem"
        Me.参数设置CToolStripMenuItem.Size = New System.Drawing.Size(175, 24)
        Me.参数设置CToolStripMenuItem.Text = "参数设置(&S)"
        '
        '控制台CToolStripMenuItem
        '
        Me.控制台CToolStripMenuItem.Name = "控制台CToolStripMenuItem"
        Me.控制台CToolStripMenuItem.Size = New System.Drawing.Size(175, 24)
        Me.控制台CToolStripMenuItem.Text = "控制台(&C)"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(864, 542)
        Me.Controls.Add(Me.MsgRevList)
        Me.Controls.Add(Me.MyStatusBar)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(4, 2, 4, 2)
        Me.Name = "frmMain"
        Me.Text = "GSM模块控制器"
        Me.MyStatusBar.ResumeLayout(False)
        Me.MyStatusBar.PerformLayout()
        Me.IconMenu.ResumeLayout(False)
        Me.RevMenu.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MyStatusBar As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents MyStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents MyIcon As System.Windows.Forms.NotifyIcon
    Friend WithEvents IconMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents 退出ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripStatusLabel2 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents MyPort As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents MsgRevList As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents RevMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents 删除DToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 回复RToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripStatusLabel3 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents Signal As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents ToolStripStatusLabel4 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents SRProgress As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents ToolStripStatusLabel5 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents 处理队列DToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 刷新列表RToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 高级SToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 参数设置CToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 控制台CToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 功能FToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 通讯录CToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 发送短消息SToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 批量发送DToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 拨号DToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
