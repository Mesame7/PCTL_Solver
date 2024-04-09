Namespace Core.Model


    Public Class Network

        Private _Name As String
        Private _States As New List(Of State)
        Private _Branches As New List(Of Branch)


        Public Property Name As String
            Get
                Return _Name
            End Get
            Set(value As String)
                _Name = value
            End Set
        End Property
        Public Sub New(name As String)
            _Name = name
        End Sub
        Public Sub AddBranch(br As Branch)
            _Branches.Add(br)
        End Sub
        Public Function GetBranches() As List(Of Branch)
            Return _Branches
        End Function

        Public Sub AddState(st As State)
            _States.Add(st)
        End Sub
        Public Function GetStates() As List(Of State)
            Return _States
        End Function

    End Class
End Namespace
