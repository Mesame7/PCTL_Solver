Namespace Core.Model.Formula


    Public Class UntilInfiniteFormula
        Inherits PathFormula

        Public Sub New(firstFormula As StateFormula, lastFormula As StateFormula)
            Me._FirstFormula = firstFormula
            Me._LastFormula = lastFormula

        End Sub

        Private _FirstFormula As StateFormula
        Private _LastFormula As StateFormula


        Public ReadOnly Property FirstFormula As StateFormula
            Get
                Return _FirstFormula
            End Get
        End Property

        Public ReadOnly Property LastFormula As StateFormula
            Get
                Return _LastFormula
            End Get
        End Property

    End Class

End Namespace
