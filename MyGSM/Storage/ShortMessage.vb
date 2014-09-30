Public Class ShortMessage

    Enum AddressType
        None
        Local = &HA1
        International = &H91
        Unknown = &H81
    End Enum

    Public IDForModule As Integer = 0
    Public MessageCentre As String = ""
    Private _RemoteAddress As String = ""
    Private _RemoteAddressType As ShortMessage.AddressType
    Public Time As Date = Now
    Public Message As String = ""
    Public UUID As String = ""

    Public Property RemoteAddressType As ShortMessage.AddressType
        Get
            If _RemoteAddressType = ShortMessage.AddressType.None Then
                If _RemoteAddress.StartsWith("+") Then
                    _RemoteAddressType = ShortMessage.AddressType.International
                Else
                    _RemoteAddressType = ShortMessage.AddressType.Unknown
                End If
            End If
            Return _RemoteAddressType
        End Get
        Set(value As ShortMessage.AddressType)
            _RemoteAddressType = value
        End Set
    End Property

    Public Property RemoteAddress As String
        Get
            Return _RemoteAddress.TrimStart("+"c)
        End Get
        Set(value As String)
            _RemoteAddress = value
        End Set
    End Property

    Public ReadOnly Property RemoteAddressDisplay As String
        Get
            If _RemoteAddressType = ShortMessage.AddressType.International Then
                Return "+" + RemoteAddress
            Else
                Return RemoteAddress
            End If
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return String.Format("#{0} {1}: {2}", IDForModule, RemoteAddress, Message)
    End Function

End Class
