Namespace Core.Model.Formula
    Public MustInherit Class StateFormula
        Private _Evaluator As FormulaEvaluator
        Public ReadOnly Property Evaluator As FormulaEvaluator
            Get
                Return _Evaluator
            End Get
        End Property
        Public Sub New()

        End Sub
        Public Sub New(evaluator As FormulaEvaluator)
            _Evaluator = evaluator
        End Sub

        'Public MustOverride Function Evaluate() As Boolean



    End Class
End Namespace
