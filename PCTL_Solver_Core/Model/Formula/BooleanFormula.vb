Namespace Core.Model.Formula

    Public Class BooleanFormula
        Inherits StateFormula
        Public Sub New(Evaluator As FormulaEvaluator, value As Boolean)
            MyBase.New(Evaluator)
            Me._BoolValue = value
        End Sub

        Private _BoolValue As Boolean
        Public ReadOnly Property BoolValue As Boolean
            Get
                Return _BoolValue
            End Get
        End Property
        'Public Overrides Function Evaluate(st As State) As Boolean
        '    Return BoolValue
        'End Function
    End Class

End Namespace