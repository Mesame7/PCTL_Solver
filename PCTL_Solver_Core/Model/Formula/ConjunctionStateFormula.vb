Namespace Core.Model.Formula

    Public Class ConjunctionStateFormula
        Inherits StateFormula

        Private _SubStates As New List(Of StateFormula)

        Public Sub AddSubState(StateFormula As StateFormula)
            _SubStates.Add(StateFormula)
        End Sub
        'Public Overrides Function Evaluate() As Boolean
        '    For Each subState In _SubStates
        '        If Not subState Then Return False
        '    Next
        '    Return True
        'End Function

    End Class
End Namespace
