Namespace Core.Model.Formula

    Public Class ProbabilityFormula
        Inherits StateFormula
        Private _PathFormula As PathFormula
        Private _P As Double
        Private _FormulaEvaluator As FormulaEvaluator
        Public Sub New(p As Double, path As PathFormula, formulaEvaluator As FormulaEvaluator)
            Me._P = p
            Me._PathFormula = path
            Me._FormulaEvaluator = formulaEvaluator
        End Sub
        Public ReadOnly Property PathFormula As PathFormula
            Get
                Return _PathFormula
            End Get
        End Property
        Public ReadOnly Property P As Double
            Get
                Return _P
            End Get
        End Property

        Public Overrides Function Evaluate(state As State) As Boolean
            Return Me._FormulaEvaluator.EvaluateProbStateFormula(state, Me)
        End Function
    End Class

End Namespace
