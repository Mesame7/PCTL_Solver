Namespace Core.Model
    Public MustInherit Class SystemObject
        Private _Name As String
        Public Property Name As String
            Get
                Return _Name
            End Get
            Set(value As String)
                _Name = value
            End Set
        End Property
    End Class
End Namespace
