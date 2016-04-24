Imports System.Data.SQLite
Imports System.IO

Public Class SQLiteStorage
    Inherits Storage

    Private _conn As SQLiteConnection

    Private InitDBSQL As String = My.Resources.ddl
    Private _InitDBCommand As SQLiteCommand

    Const EnqueueSendSQL As String = "INSERT INTO [ReadySend] ([Time], [RemoteAddress], [RemoteAddressType], [Message]) VALUES (@Time, @RemoteAddress, @RemoteAddressType, @Message)"
    Private _EnqueueSendCommand As SQLiteCommand
    Const DequeueSendSQL As String = "SELECT [ID], [Time], [RemoteAddress], [RemoteAddressType], [Message] FROM [ReadySend] LIMIT 1"
    Private _DequeueSendCommand As SQLiteCommand
    Const GetReadySendByIDSQL As String = "SELECT [ID], [Time], [RemoteAddress], [RemoteAddressType], [Message] FROM [ReadySend] WHERE [ID]=@id"
    Private _GetReadySendByIDCommand As SQLiteCommand
    Const DeleteReadySendByIDSQL As String = "DELETE FROM [ReadySend] WHERE [ID]=@id"
    Private _DeleteReadySendByIDCommand As SQLiteCommand
    Const ReadySendCountSQL As String = "SELECT COUNT(*) FROM [ReadySend]"
    Private _ReadySendCountCommand As SQLiteCommand

    Const InsertSentSQL As String = "INSERT INTO [Sent] ([Time], [RemoteAddress], [RemoteAddressType], [Message], [Succeeded]) VALUES (@Time, @RemoteAddress, @RemoteAddressType, @Message, @Succeeded)"
    Private _InsertSentCommand As SQLiteCommand

    Const InsertReceivedSQL As String = "INSERT INTO [Received] ([Time], [RemoteAddress], [RemoteAddressType], [Message]) VALUES (@Time, @RemoteAddress, @RemoteAddressType, @Message)"
    Private _InsertReceivedCommand As SQLiteCommand
    Const GetReceivedByIDSQL As String = "SELECT [ID], [Time], [RemoteAddress], [RemoteAddressType], [Message] FROM [Received] WHERE [ID]=@id"
    Private _GetReceivedByIDCommand As SQLiteCommand
    Const DeleteReceivedByIDSQL As String = "DELETE FROM [Received] WHERE [ID]=@id"
    Private _DeleteReceivedByIDCommand As SQLiteCommand
    Const ListReceivedSQL As String = "SELECT [ID], [Time], [RemoteAddress], [RemoteAddressType], [Message] FROM [Received]"
    Private _ListReceivedCommand As SQLiteCommand

    Const ListContactsSQL As String = "SELECT [ID], [Name], [Number], [Remark] FROM [Contacts]"
    Private _ListContactsCommand As SQLiteCommand
    Const GetContactsByNameSQL As String = "SELECT [ID], [Name], [Number], [Remark] FROM [Contacts] WHERE [Name]=@Name"
    Private _GetContactsByNameCommand As SQLiteCommand
    Const SetContactsByNameSQL As String = "UPDATE [Contacts] SET [Number]=@Number, [Remark]=@Remark WHERE [Name]=@Name"
    Private _SetContactsByNameCommand As SQLiteCommand
    Const InsertContactsSQL As String = "INSERT INTO [Contacts] ([Name], [Number], [Remark]) VALUES (@Name, @Number, @Remark)"
    Private _InsertContactsCommand As SQLiteCommand
    Const DeleteContactsByNameSQL As String = "DELETE FROM [Contacts] WHERE [Name]=@Name"
    Private _DeleteContactsByNameCommand As SQLiteCommand

    Const ListSettingsSQL As String = "SELECT [Key], [Value] FROM [Settings]"
    Private _ListSettingsCommand As SQLiteCommand
    Const GetSettingsByKeySQL As String = "SELECT [Key], [Value] FROM [Settings] WHERE [Key]=@Key"
    Private _GetSettingsByKeyCommand As SQLiteCommand
    Const SetSettingsByKeySQL As String = "UPDATE [Settings] SET [Value]=@Value WHERE [Key]=@Key"
    Private _SetSettingsByKeyCommand As SQLiteCommand
    Const InsertSettingsSQL As String = "INSERT INTO [Settings] ([Key], [Value]) VALUES (@Key, @Value)"
    Private _InsertSettingsCommand As SQLiteCommand

    Private Sub initCommands()
        _InitDBCommand = New SQLiteCommand(InitDBSQL, _conn)

        _EnqueueSendCommand = New SQLiteCommand(EnqueueSendSQL, _conn)
        _DequeueSendCommand = New SQLiteCommand(DequeueSendSQL, _conn)
        _GetReadySendByIDCommand = New SQLiteCommand(GetReadySendByIDSQL, _conn)
        _DeleteReadySendByIDCommand = New SQLiteCommand(DeleteReadySendByIDSQL, _conn)
        _ReadySendCountCommand = New SQLiteCommand(ReadySendCountSQL, _conn)

        _InsertSentCommand = New SQLiteCommand(InsertSentSQL, _conn)

        _InsertReceivedCommand = New SQLiteCommand(InsertReceivedSQL, _conn)
        _GetReceivedByIDCommand = New SQLiteCommand(GetReceivedByIDSQL, _conn)
        _DeleteReceivedByIDCommand = New SQLiteCommand(DeleteReceivedByIDSQL, _conn)
        _ListReceivedCommand = New SQLiteCommand(ListReceivedSQL, _conn)

        _ListContactsCommand = New SQLiteCommand(ListContactsSQL, _conn)
        _GetContactsByNameCommand = New SQLiteCommand(GetContactsByNameSQL, _conn)
        _SetContactsByNameCommand = New SQLiteCommand(SetContactsByNameSQL, _conn)
        _InsertContactsCommand = New SQLiteCommand(InsertContactsSQL, _conn)
        _DeleteContactsByNameCommand = New SQLiteCommand(DeleteContactsByNameSQL, _conn)

        _ListSettingsCommand = New SQLiteCommand(ListSettingsSQL, _conn)
        _GetSettingsByKeyCommand = New SQLiteCommand(GetSettingsByKeySQL, _conn)
        _SetSettingsByKeyCommand = New SQLiteCommand(SetSettingsByKeySQL, _conn)
        _InsertSettingsCommand = New SQLiteCommand(InsertSettingsSQL, _conn)
    End Sub

    Public Sub New(dbfilename As String, Optional ByVal pwd As String = Nothing)
        Dim sb As New SQLiteConnectionStringBuilder
        sb.DataSource = dbfilename
        If pwd IsNot Nothing Then
            sb.Password = pwd
        End If
        _conn = New SQLiteConnection(sb.ToString)
        _conn.Open()
        initCommands()
        Using cmd As SQLiteCommand = _InitDBCommand.Clone()
            cmd.ExecuteNonQuery()
            'Utils.dbg("Built Database Structure")
        End Using
    End Sub

    Public Overrides Sub AckSent(smsid As String, Succeeded As Boolean)
        Dim sms = GetReadySend(smsid)
        DeleteReadySend(smsid)
        Using cmd As SQLiteCommand = _InsertSentCommand.Clone()
            AddParametersWithShortMessage(cmd, sms)
            cmd.Parameters.AddWithValue("Succeeded", Succeeded)
            cmd.ExecuteReader()
        End Using
    End Sub

    Public Sub DeleteReadySend(smsid As String)
        Using cmd As SQLiteCommand = _DeleteReadySendByIDCommand.Clone()
            cmd.Parameters.AddWithValue("ID", Int32.Parse(smsid))
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Overrides Sub DeleteReceived(smsid As String)
        Using cmd As SQLiteCommand = _DeleteReceivedByIDCommand.Clone()
            cmd.Parameters.AddWithValue("ID", Int32.Parse(smsid))
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Function ReaderToSMS(rd As SQLiteDataReader) As ShortMessage
        If rd.Read() Then
            Dim sms As New ShortMessage
            sms.IDForModule = rd.GetInt32(0)
            sms.UUID = rd.GetInt32(0)
            sms.Time = rd.GetDateTime(1)
            sms.RemoteAddress = rd.GetString(2)
            sms.RemoteAddressType = Convert.ToInt32(rd.GetString(3))
            sms.Message = rd.GetString(4)
            Return sms
        Else
            Return Nothing
        End If
    End Function

    Private Sub AddParametersWithShortMessage(cmd As SQLiteCommand, sms As ShortMessage)
        cmd.Parameters.AddWithValue("Time", Now)
        cmd.Parameters.AddWithValue("RemoteAddress", sms.RemoteAddress)
        cmd.Parameters.AddWithValue("RemoteAddressType", sms.RemoteAddressType)
        cmd.Parameters.AddWithValue("Message", sms.Message)
    End Sub

    Public Overrides Function DequeueSend() As ShortMessage
        Using cmd As SQLiteCommand = _DequeueSendCommand.Clone()
            Dim rd = cmd.ExecuteReader()
            Return ReaderToSMS(rd)
        End Using
    End Function

    Public Overloads Overrides Sub EnqueueSend(sms As ShortMessage)
        Using cmd As SQLiteCommand = _EnqueueSendCommand.Clone()
            AddParametersWithShortMessage(cmd, sms)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Overrides Sub ExportReceived(smsid As String, copyto As String)
        Dim sms = GetReceived(smsid)
        Using sw As New StreamWriter(copyto)
            sw.WriteLine(sms.IDForModule.ToString)
            sw.WriteLine(sms.Time.ToString)
            sw.WriteLine(sms.RemoteAddress)
            sw.WriteLine(DirectCast(sms.RemoteAddressType, Integer))
            sw.WriteLine(sms.Message)
            sw.Flush()
        End Using
    End Sub

    Public Overrides Function GetReadySend(smsid As String) As ShortMessage
        Using cmd As SQLiteCommand = _GetReadySendByIDCommand.Clone()
            cmd.Parameters.AddWithValue("ID", Int32.Parse(smsid))
            Dim rd = cmd.ExecuteReader()
            Return ReaderToSMS(rd)
        End Using
    End Function

    Public Overrides Function ReadySendCount() As Integer
        Using cmd As SQLiteCommand = _ReadySendCountCommand.Clone()
            Dim rd = cmd.ExecuteReader()
            rd.Read()
            Return rd.GetInt32(0)
        End Using
    End Function

    Public Overrides Function GetReceived(smsid As String) As ShortMessage
        Using cmd As SQLiteCommand = _GetReceivedByIDCommand.Clone()
            cmd.Parameters.AddWithValue("ID", Int32.Parse(smsid))
            Dim rd = cmd.ExecuteReader()
            Return ReaderToSMS(rd)
        End Using
    End Function

    Public Overrides Sub InsertReceived(sms As ShortMessage)
        Using cmd As SQLiteCommand = _InsertReceivedCommand.Clone()
            AddParametersWithShortMessage(cmd, sms)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Overrides Function ListReceived() As List(Of ShortMessage)
        Using cmd As SQLiteCommand = _ListReceivedCommand.Clone()
            Dim rd = cmd.ExecuteReader()
            Dim lastsms = ReaderToSMS(rd)
            Dim lstsms As New List(Of ShortMessage)
            Do While lastsms IsNot Nothing
                lstsms.Add(lastsms)
                lastsms = ReaderToSMS(rd)
            Loop
            Return lstsms
        End Using
    End Function

    Public Overrides Sub InitContacts()
        Using cmd As SQLiteCommand = _ListContactsCommand.Clone()
            Dim rd = cmd.ExecuteReader()
            _ContactsCache = New List(Of ContactPerson)
            Do While rd.Read()
                Dim cp As New ContactPerson
                cp.ID = rd.GetInt32(0)
                cp.Name = rd.GetString(1)
                cp.Number = rd.GetString(2)
                cp.Remark = rd.GetString(3)
                _ContactsCache.Add(cp)
            Loop
        End Using
    End Sub

    Protected Overrides Sub InternalDeleteContactsRecord(Name As String)
        Using cmd As SQLiteCommand = _DeleteContactsByNameCommand.Clone()
            cmd.Parameters.AddWithValue("Name", Name)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Protected Overrides Sub InternalInsertContactsRecord(cp As ContactPerson)
        Using cmd As SQLiteCommand = _InsertContactsCommand.Clone()
            cmd.Parameters.AddWithValue("Name", cp.Name)
            cmd.Parameters.AddWithValue("Number", cp.Number)
            cmd.Parameters.AddWithValue("Remark", cp.Remark)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Overrides Sub InitSettings()
        Dim ak(_SettingsCache.Keys.Count - 1) As String
        _SettingsCache.Keys.CopyTo(ak, 0)
        For Each k In ak
            Dim v = InternalGetSetting(k)
            If v IsNot Nothing Then
                _SettingsCache(k) = v
            Else
                '数据库中没有则插入默认值
                InternalInsertSetting(k, _SettingsCache(k))
            End If
        Next
        Using cmd As SQLiteCommand = _ListSettingsCommand.Clone()
            Dim rd = cmd.ExecuteReader()
            Do While rd.Read()
                Dim k = rd.GetString(0),
                    v = rd.GetString(1)
                If Not _SettingsCache.ContainsKey(k) Then
                    '数据库有额外的信息则加入
                    _SettingsCache.Add(k, v)
                End If
            Loop
        End Using
    End Sub

    Protected Overrides Function InternalGetSetting(key As String) As String
        Using cmd As SQLiteCommand = _GetSettingsByKeyCommand.Clone()
            cmd.Parameters.AddWithValue("Key", key)
            Dim rd = cmd.ExecuteReader()
            If rd.Read() Then
                Return rd.GetString(1)
            Else
                Return Nothing
            End If
        End Using
    End Function

    Protected Overrides Sub InternalInsertSetting(key As String, value As String)
        Using cmd As SQLiteCommand = _InsertSettingsCommand.Clone()
            cmd.Parameters.AddWithValue("Key", key)
            cmd.Parameters.AddWithValue("Value", value)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Protected Overrides Sub InternalSetSetting(key As String, value As String)
        Using cmd As SQLiteCommand = _SetSettingsByKeyCommand.Clone()
            cmd.Parameters.AddWithValue("Key", key)
            cmd.Parameters.AddWithValue("Value", value)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Overrides Function Type() As Storage.StorageType
        Return StorageType.SQLite
    End Function

    Protected Overrides Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' DO:  释放托管状态(托管对象)。
                _InitDBCommand.Dispose()

                _EnqueueSendCommand.Dispose()
                _DequeueSendCommand.Dispose()
                _GetReadySendByIDCommand.Dispose()
                _DeleteReadySendByIDCommand.Dispose()
                _ReadySendCountCommand.Dispose()

                _InsertSentCommand.Dispose()

                _InsertReceivedCommand.Dispose()
                _GetReceivedByIDCommand.Dispose()
                _DeleteReceivedByIDCommand.Dispose()
                _ListReceivedCommand.Dispose()

                _ListContactsCommand.Dispose()
                _GetContactsByNameCommand.Dispose()
                _SetContactsByNameCommand.Dispose()
                _InsertContactsCommand.Dispose()
                _DeleteContactsByNameCommand.Dispose()

                _ListSettingsCommand.Dispose()
                _GetSettingsByKeyCommand.Dispose()
                _SetSettingsByKeyCommand.Dispose()
                _InsertSettingsCommand.Dispose()

                _conn.Close()
            End If

            ' DO:  释放非托管资源(非托管对象)并重写下面的 Finalize()。
            ' DO:  将大型字段设置为 null。
        End If
        Me.disposedValue = True
    End Sub

End Class
