Namespace Core.Model.Formula

    Public Class ProbabilityFormula
        Inherits StateFormula
        Private _PathFormula As PathFormula
        Private _P As Double
        Public Sub New(p As Double, path As PathFormula)

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
    End Class

End Namespace
