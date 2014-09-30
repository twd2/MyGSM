Public Class HexReader

    Const CharPerHex = 2

    Public Postion As Integer = 0
    Private _data As String = ""

    Public Sub New(data As String)
        If data.Length Mod 2 = 1 Then
            Throw New Exception("Wrong string")
        End If
        Me._data = data
    End Sub

    Public Function ReadString() As String
        Return ReadString(1)
    End Function

    Public Function ReadString(blockcount As Integer) As String
        Dim charcount = blockcount * CharPerHex
        If Postion + charcount > _data.Length Then
            Throw New Exception("Cannot read")
        End If
        Dim rdata = _data.Substring(Postion, charcount)
        Postion += charcount
        Return rdata
    End Function

    Public Function ReadByte() As Byte
        Return ReadByte(1)(0)
    End Function

    Public Function ReadByte(blockcount As Integer) As Byte()
        Return HexHelper.HexToBytes(ReadString(blockcount))
    End Function


End Class
