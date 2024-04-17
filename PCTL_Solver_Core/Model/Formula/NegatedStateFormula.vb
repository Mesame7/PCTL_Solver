Namespace Core.Model.Formula

    Public Class NegatedStateFormula
        Inherits StateFormula

        Public Sub New(Evaluator As FormulaEvaluator, isNegated As Boolean, subFormula As StateFormula)
            MyBase.New(Evaluator)
            Me._IsNegated = isNegated
            Me._SubFormula = subFormula
        End Sub
        Private _IsNegated As Boolean
        Private _SubFormula As StateFormula

        Public ReadOnly Property IsNegated As Boolean
            Get
                Return _IsNegated
            End Get
        End Property

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
