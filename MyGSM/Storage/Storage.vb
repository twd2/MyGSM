Public MustInherit Class Storage
    Implements IDisposable

    Public Enum StorageType
        None
        FileSystem
        SQLite
    End Enum

    Protected _ContactsCache As List(Of ContactPerson) = Nothing

    Private _SettingInited As Boolean = False
    Protected _SettingsCache = SettingsObject.Default.ToDictionary()

    Public Sub New()

    End Sub

    Private Sub CheckContactsCache()
        If _ContactsCache Is Nothing Then
            InitContacts()
        End If
    End Sub

    Public ReadOnly Property ContactsAll As List(Of ContactPerson)
        Get
            CheckContactsCache()
            Return _ContactsCache
        End Get
    End Property

    Public ReadOnly Property ContactsItem(i As Integer) As ContactPerson
        Get
            Return ContactsAll(i)
        End Get
    End Property

    Public Function ContactsByName(Name As String) As ContactPerson
        Return ContactsAll.Find(Function(p As ContactPerson)
                                    Return p.Name = Name
                                End Function)
    End Function

    Public Function ContactsByNumber(Number As String) As ContactPerson
        Return ContactsAll.Find(Function(p As ContactPerson)
                                    Return p.Number = Number
                                End Function)
    End Function

    Public Sub InsertContactsRecord(cp As ContactPerson)
        ContactsAll.Add(cp)
        InternalInsertContactsRecord(cp)
    End Sub

    Public Sub InsertContactsRecord(Name As String, Number As String, Optional Remark As String = "")
        Dim cp As New ContactPerson
        cp.Name = Name
        cp.Number = Number
        cp.Remark = Remark
        InsertContactsRecord(cp)
    End Sub

    Public Sub DeleteContactsRecord(Name As String)
        Dim deleted = False
        ContactsAll.RemoveAll(Function(p As ContactPerson)
                                  If deleted Then '只删除一个
                                      Return False
                                  End If
                                  If p.Name = Name Then
                                      deleted = True
                                      Return True
                                  End If
                                  Return False
                              End Function)
        InternalDeleteContactsRecord(Name)
    End Sub

    Public Sub DeleteContactsRecordAt(index As Integer)
        Dim cp = ContactsAll(index)
        ContactsAll.RemoveAt(index)
        InternalDeleteContactsRecord(cp.Name)
    End Sub

    Public Function ContactsToName(Number As String) As String
        Dim cp = ContactsByNumber(Number)
        If cp Is Nothing Then
            Return Number
        Else
            Return cp.Name
        End If
    End Function

    Protected MustOverride Sub InternalInsertContactsRecord(cp As ContactPerson)

    Protected MustOverride Sub InternalDeleteContactsRecord(Name As String)

    'Protected MustOverride Sub InternalUpdateContactsRecord(cp As ContactsPerson)

    Protected MustOverride Function InternalGetSetting(key As String) As String

    Protected MustOverride Sub InternalSetSetting(key As String, value As String)

    Protected MustOverride Sub InternalInsertSetting(key As String, value As String)

    Public MustOverride Sub InitSettings()

    Public Property Settings(key As String) As String
        Get
            If Not _SettingInited Then
                InitSettings()
            End If
            Return _SettingsCache(key)
        End Get
        Set(value As String)
            If Not _SettingInited Then
                InitSettings()
            End If
            If _SettingsCache.ContainsKey(key) Then
                _SettingsCache(key) = value
                InternalSetSetting(key, value)
            Else
                _SettingsCache.Add(key, value)
                InternalInsertSetting(key, value)
            End If
        End Set
    End Property

    Public Property BooleanSettings(key As String) As Boolean
        Get
            Dim i = False
            If Boolean.TryParse(Settings(key), i) Then
                Return i
            Else
                Return False
            End If
        End Get
        Set(value As Boolean)
            Settings(key) = value.ToString()
        End Set
    End Property

    Public Property IntegerSettings(key As String) As Integer
        Get
            Dim i = 0
            If Integer.TryParse(Settings(key), i) Then
                Return i
            Else
                Return 0
            End If
        End Get
        Set(value As Integer)
            Settings(key) = value.ToString()
        End Set
    End Property

    Public Property StringSettings(key As String) As String
        Get
            Return Settings(key)
        End Get
        Set(value As String)
            Settings(key) = value
        End Set
    End Property

    Public MustOverride Sub InitContacts()

    'Public MustOverride Sub SaveContacts()

    Public MustOverride Sub ExportReceived(smsid As String, copyto As String)

    Public MustOverride Sub DeleteReceived(smsid As String)

    Public MustOverride Sub InsertReceived(sms As ShortMessage)

    Public MustOverride Function GetReceived(smsid As String) As ShortMessage

    Public MustOverride Function ListReceived() As List(Of ShortMessage)

    Public Sub EnqueueSend(RemoteAddress As String, Message As String)
        Dim sms As New ShortMessage
        sms.RemoteAddress = RemoteAddress
        sms.Message = Message
        EnqueueSend(sms)
    End Sub

    Public MustOverride Sub EnqueueSend(sms As ShortMessage)

    Public MustOverride Function ReadySendCount() As Integer

    Public MustOverride Function DequeueSend() As ShortMessage

    Public MustOverride Function GetReadySend(smsid As String) As ShortMessage

    Public MustOverride Sub AckSent(smsid As String, Succeeded As Boolean)

    Public MustOverride Function Type() As StorageType

#Region "IDisposable Support"
    Protected disposedValue As Boolean ' 检测冗余的调用

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' DO:  释放托管状态(托管对象)。
            End If

            ' DO:  释放非托管资源(非托管对象)并重写下面的 Finalize()。
            ' DO:  将大型字段设置为 null。
        End If
        Me.disposedValue = True
    End Sub

    ' DO:  仅当上面的 Dispose(ByVal disposing As Boolean)具有释放非托管资源的代码时重写 Finalize()。
    'Protected Overrides Sub Finalize()
    '    ' 不要更改此代码。    请将清理代码放入上面的 Dispose(ByVal disposing As Boolean)中。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' Visual Basic 添加此代码是为了正确实现可处置模式。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' 不要更改此代码。    请将清理代码放入上面的 Dispose (disposing As Boolean)中。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
