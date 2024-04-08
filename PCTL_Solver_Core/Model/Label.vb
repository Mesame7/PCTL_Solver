Namespace Core.Model
    Public Class Label

        Private Shared _Labels As New List(Of Label)
        Private _Name As String
        Private _IsNegated As Boolean
        Private Sub New(name As String, isNegated As Boolean)
            Me._Name = name
            Me._IsNegated = isNegated
        End Sub

        Public Function CreateLabel(name As String, isNegated As Boolean) As Label
            Dim existingLabel = _Labels.Where(Function(x) x._Name = name AndAlso x._IsNegated = isNegated).FirstOrDefault
            If existingLabel IsNot Nothing Then
                Return existingLabel
            Else
                Dim newLabel = New Label(name, isNegated)
                _Labels.Add(newLabel)
                Return newLabel
            End If
        End Function

        Public Overrides Function Equals(other As Object) As Boolean
            Return Me.IsNegated = other.IsNegated AndAlso Me.Name = other.Name
        End Function

        Public ReadOnly Property Name As String
            Get
                Return _Name
            End Get
        End Property

        Public ReadOnly Property IsNegated As Boolean
            Get
                Return _IsNegated
            End Get
        End Property

        Protected Sub ResetLabels()
            _Labels = New List(Of Label)
        End Sub
    End Class

End Namespace
