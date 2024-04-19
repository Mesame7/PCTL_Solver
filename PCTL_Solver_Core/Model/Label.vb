Namespace Core.Model
    Public Class Label

        Private Shared _Labels As New List(Of Label)
        Private _Name As String
        Private _IsNegated As Boolean
        Private Sub New(name As String)
            Me._Name = name
        End Sub

        Public Shared Function CreateLabel(name As String) As Label
            Dim existingLabel = _Labels.Where(Function(x) x._Name = name).FirstOrDefault
            If existingLabel IsNot Nothing Then
                Return existingLabel
            Else
                Dim newLabel = New Label(name)
                _Labels.Add(newLabel)
                Return newLabel
            End If
        End Function

        Public Overrides Function Equals(other As Object) As Boolean
            Return Me.Name = other.Name
        End Function

        Public ReadOnly Property Name As String
            Get
                Return _Name
            End Get
        End Property

        Protected Sub ResetLabels()
            _Labels = New List(Of Label)
        End Sub
    End Class

End Namespace
