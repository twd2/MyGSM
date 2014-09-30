<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDebug
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

    '注意:  以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDebug))
        Me.txtDebug = New System.Windows.Forms.TextBox()
        Me.txtUserCommand = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'txtDebug
        '
        Me.txtDebug.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDebug.BackColor = System.Drawing.Color.Black
        Me.txtDebug.Font = New System.Drawing.Font("Lucida Console", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDebug.ForeColor = System.Drawing.Color.White
        Me.txtDebug.Location = New System.Drawing.Point(12, 12)
        Me.txtDebug.Multiline = True
        Me.txtDebug.Name = "txtDebug"
        Me.txtDebug.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDebug.Size = New System.Drawing.Size(584, 406)
        Me.txtDebug.TabIndex = 0
        Me.txtDebug.Text = "Hello, world!" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'txtUserCommand
        '
        Me.txtUserCommand.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUserCommand.BackColor = System.Drawing.Color.Black
        Me.txtUserCommand.Font = New System.Drawing.Font("Lucida Console", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUserCommand.ForeColor = System.Drawing.Color.White
        Me.txtUserCommand.Location = New System.Drawing.Point(12, 424)
        Me.txtUserCommand.Name = "txtUserCommand"
        Me.txtUserCommand.Size = New System.Drawing.Size(584, 27)
        Me.txtUserCommand.TabIndex = 1
        Me.txtUserCommand.Text = "AT"
        '
        'frmDebug
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(608, 461)
        Me.Controls.Add(Me.txtUserCommand)
        Me.Controls.Add(Me.txtDebug)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmDebug"
        Me.Text = "控制台"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtDebug As System.Windows.Forms.TextBox
    Friend WithEvents txtUserCommand As System.Windows.Forms.TextBox
End Class
