Namespace Core.Model.Formula

    Public Class NextFormula
        Inherits PathFormula
        Public Sub New(st As StateFormula)
            _StateFormula = st
        End Sub

        Private _StateFormula As StateFormula
        Public ReadOnly Property StateFormula As StateFormula
            Get
                Return _StateFormula
            End Get
        End Property
    End Class
End Namespace
