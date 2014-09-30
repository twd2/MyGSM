Imports System.Text

Public Class HexWriter

    Private _sb As New StringBuilder

    Public Sub New()

    End Sub

    Public Sub Write(int As Integer)
        Write({CByte(int)})
    End Sub

    Public Sub Write(b As Byte)
        Write({b})
    End Sub

    Public Sub Write(b As Byte())
        _sb.Append(HexHelper.ByteToHex(b))
    End Sub

    Public Sub Write(s As String)
        _sb.Append(s)
    End Sub

    Public Overrides Function ToString() As String
        Return _sb.ToString()
    End Function

End Class
