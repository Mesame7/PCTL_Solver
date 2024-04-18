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

    End Class
End Namespace
