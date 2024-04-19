Namespace Core.Model.Formula

    Public Class BooleanFormula
        Inherits StateFormula
        Public Sub New(value As Boolean)
            Me._BoolValue = value
        End Sub

        Public Overrides Function Evaluate(state As State) As Boolean
            Return BoolValue
        End Function

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