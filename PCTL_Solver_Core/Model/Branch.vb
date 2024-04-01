Public Class Branch
    Inherits SystemObject

    Public Sub New(name As String, p As Double, fromState As State, toState As State)
        _Name = name
        _P = p
        _FromState = fromState
        _ToState = toState

    End Sub



    Private _P As Double

    Private _FromState As State
    Private _ToState As State
    Public Property P As Double
        Get
            Return _P
        End Get
        Set(value As Double)
            Throw New NotImplementedException("Validate First the value")
        End Set
    End Property



    Public ReadOnly Property FromState As State
        Get
            Return _FromState
        End Get
    End Property

    Public ReadOnly Property ToState As State
        Get
            Return _ToState
        End Get
    End Property

End Class
