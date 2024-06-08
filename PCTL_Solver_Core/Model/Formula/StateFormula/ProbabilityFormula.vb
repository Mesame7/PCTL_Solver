Namespace Core.Model.Formula

    Public Class ProbabilityFormula
        Inherits StateFormula
        Private _PathFormula As PathFormula
        Private _PMin As Double
        Private _MinEqual As Boolean
        Private _MaxEqual As Boolean
        Private _PMax As Double = 1
        Private _FormulaEvaluator As FormulaEvaluator
        Public Sub New(pMin As Double, pMinEqual As Boolean, pMax As Double, pMaxEqual As Boolean, path As PathFormula, formulaEvaluator As FormulaEvaluator)
            If pMax > 1 OrElse pMax < 0 OrElse pMin > 1 OrElse pMin < 0 OrElse pMin > pMax Then
                Throw New Exception("Wrong P limits")
            End If
            Me._PMin = pMin
            Me._MinEqual = pMinEqual
            Me._PMax = pMax
            Me._MaxEqual = pMaxEqual
            Me._PathFormula = path
            Me._FormulaEvaluator = formulaEvaluator
        End Sub
        Public ReadOnly Property PathFormula As PathFormula
            Get
                Return _PathFormula
            End Get
        End Property

        Public Overrides Function Evaluate(state As State) As Boolean
            Dim evaluatedVal = Me._FormulaEvaluator.EvaluateProbStateFormula(state, Me)
            Select Case True
                Case _MaxEqual AndAlso evaluatedVal <= _PMax
                    Return _MinEqual AndAlso evaluatedVal >= _PMin OrElse Not _MinEqual AndAlso evaluatedVal > _PMin
                Case Not _MaxEqual AndAlso evaluatedVal < _PMax
                    Return _MinEqual AndAlso evaluatedVal >= _PMin OrElse Not _MinEqual AndAlso evaluatedVal > _PMin
                Case Else
                    Return False
            End Select

        End Function
    End Class

End Namespace
