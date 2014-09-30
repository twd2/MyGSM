Imports System.Text

Public Class PDU

    Public Shared Function ToPDU(sms As ShortMessage) As String
        Dim hw As New HexWriter()
        hw.Write("001100") 'magic number 1
        hw.Write(sms.RemoteAddress.Length)
        hw.Write(EncodeAddress(sms.RemoteAddress, sms.RemoteAddressType))
        hw.Write("000800") 'magic number 2
        Dim uc = EncodeUCS2(sms.Message)
        hw.Write(uc.Length)
        hw.Write(uc)
        Return hw.ToString()
    End Function

    Public Shared Function Parse(raw As String) As ShortMessage
        Dim hr As New HexReader(raw)
        Dim SMSCLength = hr.ReadByte()
        Dim SMSCType As ShortMessage.AddressType
        Dim SMSC = DecodeAddress(hr.ReadString(SMSCLength), SMSCType)
        Dim Unknown = hr.ReadByte()
        Dim RemoteAddressLength = hr.ReadByte()

        '转换成字节数
        If RemoteAddressLength Mod 2 = 1 Then
            RemoteAddressLength += 1
        End If
        RemoteAddressLength = RemoteAddressLength / 2 + 1
        Dim RemoteAddressType As ShortMessage.AddressType

        Dim RemoteAddress = DecodeAddress(hr.ReadString(RemoteAddressLength), RemoteAddressType)
        Dim PID = hr.ReadByte() 'HexToByte(raw.Substring(offset, 2))
        Dim DCS = hr.ReadByte() 'HexToByte(raw.Substring(offset, 2))
        Dim SendTime = DecodeTime(hr.ReadString(7))
        Dim UDL = hr.ReadByte() 'HexToByte(raw.Substring(offset, 2))

        If DCS = 0 Then '7bit时, UDL存的是真实字符的个数, 要转换为字节数
            UDL = Math.Ceiling(UDL * 7 / 8)
        End If

        Dim UD = hr.ReadByte(UDL) 'HexToBytes(raw.Substring(offset, UDL * 2))
        Dim UD_string = ""
        If DCS = 0 Then '7bit
            UD_string = Decode7bit(Math.Floor(UDL * 8 / 7), UD)
        ElseIf DCS = 8 Then 'UCS2
            UD_string = DecodeUCS2(UD)
        Else
            UD_string = HexHelper.ByteToHex(UD)
            Globals.dbg("Warning: Unknown DCS, using raw data")
        End If

        Dim sms As New ShortMessage
        sms.Message = UD_string
        sms.MessageCentre = SMSC
        sms.Time = SendTime
        sms.RemoteAddress = RemoteAddress
        sms.RemoteAddressType = RemoteAddressType
        Return sms
    End Function

    'Public Shared Sub test()
    '    Globals.dbg(Decode7bit(3, HexHelper.HexToBytes("B0986C46ABD96E38")))
    '    Globals.dbg(HexHelper.BytesToHex(Encode7bit("01234567")))
    '    Globals.dbg(DecodeUCS2(HexHelper.HexToBytes("60A8597D")))
    '    Dim t As ShortMessage.AddressType
    '    Globals.dbg(DecodeAddress("91683108100005F0", t))
    '    Globals.dbg(t.ToString())
    '    Globals.dbg(HexHelper.BytesToHex(EncodeAddress("+8613123456789", ShortMessage.AddressType.International)))
    '    Globals.dbg(DecodeTime("41907051830023"))
    '    Parse("0891683108100025F8040D91683123435413F100004190705183412303AE2011")
    '    Parse("0891683108100005F0040D91683123435413F10008419070518300230460A8597D")
    '    Parse("0891683108100045F9640C910156184510200004419070025014237E0605040B8423F04E06246170706C69636174696F6E2F766E642E7761702E6D6D732D6D65737361676500B487AF848C8298464F50503549583439683046008D9083687474703A2F2F3232312E3137392E3138352E3233312F464F505035495834396830460088048102FF5E890A803130363538303830008A808E0301646F")
    '    Dim sms As New ShortMessage
    '    sms.RemoteAddress = "+8613693550000"
    '    sms.Message = "您好"
    '    Globals.dbg(ToPDU(sms))
    'End Sub

    Public Shared Function DecodeTime(raw As String) As DateTime
        Dim year = Now.Year.ToString().Substring(0, 2) + raw(1) + raw(0)
        Dim month = raw(3) + raw(2)
        Dim day = raw(5) + raw(4)
        Dim hour = raw(7) + raw(6)
        Dim minute = raw(9) + raw(8)
        Dim second = raw(11) + raw(10)
        Dim offset = HexHelper.HexToByte(raw(13) + raw(12))
        Return New DateTime(year, month, day, hour, minute, second)
    End Function

    Public Shared Function EncodeAddress(addr As String, type As ShortMessage.AddressType) As Byte()
        addr = addr.TrimStart("+"c)
        If addr.Length Mod 2 = 1 Then
            addr += "F"
        End If
        Dim len = addr.Length / 2 + 1
        Dim raw(len - 1) As Byte
        raw(0) = type
        For i = 0 To addr.Length - 1 Step 2
            raw(1 + i / 2) = Convert.ToByte(addr(i), 16) + (Convert.ToByte(addr(i + 1), 16) << 4)
        Next
        Return raw
    End Function

    Public Shared Function DecodeAddress(raw As String, ByRef outType As ShortMessage.AddressType) As String
        Dim sb As New StringBuilder()

        For i = 0 To raw.Length - 1 Step 2
            Dim currblock = raw.Substring(i, 2)
            If i = 0 Then
                outType = Convert.ToInt32(currblock, 16)
                If outType = ShortMessage.AddressType.International Then
                    sb.Append("+")
                End If
            Else
                sb.Append(currblock(1))
                If currblock(0).ToString().ToUpper() <> "F" Then
                    sb.Append(currblock(0))
                End If
            End If
        Next

        Return sb.ToString()
    End Function

    Public Shared Function DecodeAddress(raw As Byte(), ByRef outType As ShortMessage.AddressType) As String
        Return DecodeAddress(HexHelper.ByteToHex(raw), outType)
    End Function


    Public Shared Function DecodeUCS2(raw As Byte()) As String
        Return Encoding.BigEndianUnicode.GetString(raw)
    End Function

    Public Shared Function EncodeUCS2(data As String) As Byte()
        Return Encoding.BigEndianUnicode.GetBytes(data)
    End Function

    Public Shared Function Encode7bit(data As String) As Byte()
        Dim raw = Encoding.Default.GetBytes(data)
        Dim rawlen = raw.Length

        Dim result(Math.Ceiling(rawlen * 7 / 8)) As Byte

        Dim lastData As Byte = 0 '上一字节残余的数据
        Dim i = 0, counter = 0

        '8个字节一组
        Do While i < rawlen
            Dim idInGroup = i Mod 8 '当前正在处理的组内字节的序号，范围是0-7

            If idInGroup = 0 Then
                '组内第一个字节，只是保存起来，待处理下一个字节时使用
                lastData = raw(i)
            Else
                '组内其它字节，将其右边部分与残余数据相加，得到一个目标编码字节
                result(counter) = raw(i) << (8 - idInGroup) Or lastData
                lastData = raw(i) >> idInGroup
                counter += 1
            End If

            i += 1
        Loop

        If i Mod 8 > 0 Then
            result(counter) = lastData
            counter += 1
        End If

        ReDim Preserve result(counter - 1)
        Return result
    End Function

    Public Shared Function Decode7bit(count As Integer, raw As Byte()) As String
        'Dim raw = HexToByte(data)
        Dim rawlen = raw.Length

        Dim result(Math.Ceiling(rawlen * 8 / 7)) As Byte

        Dim lastData As Byte = 0 '上一字节残余的数据
        Dim i = 0, counter = 0

        '7个字节一组
        Do While i < rawlen
            Dim idInGroup = i Mod 7 '当前正在处理的组内字节的序号，范围是0-6
            result(counter) = (raw(i) << idInGroup Or lastData) And &H7F
            lastData = raw(i) >> (7 - idInGroup)

            counter += 1
            i += 1

            '到了一组的最后一个字节
            If i Mod 7 = 0 Then
                '额外得到一个目标解码字节
                result(counter) = lastData
                lastData = 0

                counter += 1
            End If
        Loop
        ReDim Preserve result(counter - 1)
        Return Encoding.Default.GetString(result)
    End Function

End Class
