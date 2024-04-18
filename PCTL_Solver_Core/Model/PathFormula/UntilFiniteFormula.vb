Namespace Core.Model.Formula

    Public Class UntilFiniteFormula
        Inherits UntilInfiniteFormula


        Public Sub New(firstFormula As StateFormula, lastFormula As StateFormula, hopCount As Integer)
            MyBase.New(firstFormula, lastFormula)
            Me._HopCount = hopCount
        End Sub

        Private _HopCount As Integer


        Public ReadOnly Property HopCount As Integer
            Get
                Return _HopCount
            End Get
        End Property
    End Class
End Namespace
