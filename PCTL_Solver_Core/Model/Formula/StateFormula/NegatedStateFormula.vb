Namespace Core.Model.Formula

    Public Class NegatedStateFormula
        Inherits StateFormula

        Public Sub New(subFormula As StateFormula)
            Me._SubFormula = subFormula
        End Sub
        Private _SubFormula As StateFormula

        Public ReadOnly Property SubFormula As StateFormula
            Get
                Return _SubFormula
            End Get
        End Property
        'Public Overrides Function Evaluate(st As State) As Boolean
        '    Return _SubFormula.Evaluate(st) Xor _IsNegated
        'End Function

    End Class
End Namespace
