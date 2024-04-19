Namespace Core.Model.Formula

    Public Class LabelFormula
        Inherits StateFormula
        Private _Label As String
        Public Sub New(label As String)
            _Label = label
        End Sub
        Public ReadOnly Property Label As String
            Get
                Return _Label
            End Get
        End Property

        Public Overrides Function Evaluate(state As State) As Boolean
            Return state.GetLabels.Any(Function(x) x.Name = Me._Label)
        End Function
    End Class
End Namespace
