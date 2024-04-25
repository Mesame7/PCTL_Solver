Namespace Core.Model.Formula

    Public Class ConjunctionStateFormula
        Inherits StateFormula

        Private _SubStateFormulas As New List(Of StateFormula)

        Public Sub AddSubState(StateFormula As StateFormula)
            _SubStateFormulas.Add(StateFormula)
        End Sub
        Public Overrides Function evaluate(state As State) As Boolean
            For Each subStateFormula In _SubStateFormulas
                If Not subStateFormula.Evaluate(state) Then Return False
            Next
            Return True
        End Function

    End Class
End Namespace
