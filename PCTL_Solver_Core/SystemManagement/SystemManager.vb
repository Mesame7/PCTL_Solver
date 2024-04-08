
Namespace SystemManagement


    Public Class SystemManager
        Private _Networks As New List(Of Core.Model.System)
        Public Sub New()

        End Sub
        Public Function CreateNetwork(name As String) As Core.Model.System
            If _Networks.Any(Function(x) x.Name = name) Then
                Throw New Exception("Network already exists") 'TODO Create Exceptions
            End If
            Dim network = New Core.Model.System(name)
            _Networks.Add(network)
            Return network
        End Function
    End Class
End Namespace
