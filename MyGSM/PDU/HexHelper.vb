Imports System.Text

Public Class HexHelper
    Public Shared Function HexToBytes(data As String) As Byte()
        Dim bin(data.Length / 2 - 1) As Byte
        For i = 0 To data.Length - 1 Step 2
            bin(i / 2) = HexToByte(data.Substring(i, 2))
        Next
        Return bin
    End Function

    Public Shared Function ByteToHex(data As Byte()) As String
        Dim sb As New StringBuilder()
        For i = 0 To data.Length - 1
            sb.Append(data(i).ToString("X2"))
        Next
        Return sb.ToString()
    End Function

    Public Shared Function HexToByte(data As String) As Byte
        Return Convert.ToByte(data, 16)
    End Function

    Public Shared Function ByteToHex(data As Byte) As String
        Return data.ToString("X2")
    End Function
End Class
