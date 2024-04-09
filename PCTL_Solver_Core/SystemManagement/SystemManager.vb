
Imports PCTL_Solver_Core.Core.Model

Namespace SystemManagement


    Public Class SystemManager
        Private _Networks As New List(Of Core.Model.Network)
        Public Sub New()

        End Sub
        Public Function CreateNetwork(name As String) As Core.Model.Network
            If _Networks.Any(Function(x) x.Name = name) Then
                Throw New Exception("Network already exists") 'TODO Create Exceptions
            End If
            Dim network = New Core.Model.Network(name)
            _Networks.Add(network)
            Return network
        End Function

        Public Sub CreateState(network As Core.Model.Network, stateName As String, initPr As String, labels As String)
            If Not IsNewStateInvalid(network, stateName, initPr) Then
                Dim myState = New State(stateName, Double.Parse(initPr))

                For Each lbl In labels.Replace(" ", "").Split(",")
                    Dim myLabel As Label
                    If lbl.StartsWith("!"c) Then
                        myLabel = Label.CreateLabel(lbl.Substring(1), True)
                    Else
                        myLabel = Label.CreateLabel(lbl, False)
                    End If
                    myState.AddLabel(myLabel)
                Next

                ' For Each br In branches.Replace(" ", "").Split(",")
                ' Dim brParams = br.Split("-")
                '
                '  Next
                network.AddState(myState)
            End If

        End Sub

        Public Sub CreateBranch(network As Core.Model.Network, stateName As String, branches As String)
            Dim myState = network.GetStates.Where(Function(x) x.Name = stateName).FirstOrDefault
            If myState Is Nothing Then
                Console.WriteLine(String.Format("State {0} doesn't exist", stateName))
                Return
            End If
            Dim totalPr = 0.0
            For Each br In branches.Split(",")
                Dim brParams = br.Split("-")
                Dim toStateName = brParams.ElementAt(0)
                Dim branchPr As Double
                If Not Double.TryParse(brParams.ElementAt(1), branchPr) Then
                    Console.WriteLine(String.Format("Please enter a correct propability for state {0}", toStateName))
                    Return
                End If
                totalPr += branchPr
                Dim toState = network.GetStates.Where(Function(x) x.Name = toStateName).FirstOrDefault
                If toState Is Nothing Then
                    Console.WriteLine(String.Format("State {0} doesn't exist", toStateName))
                    Return
                End If
                Dim myBranch = New Branch(String.Format("{0}->{1}", stateName, toStateName), branchPr, myState, toState)
                myState.AddBranch(myBranch)
                network.AddBranch(myBranch)
            Next
            If totalPr <> 1 Then
                Throw New Exception(String.Format("Total Propabilities exiting state {0} is not equal to 1", stateName))
            End If
        End Sub
        Public Function IsNewStateInvalid(network As Core.Model.Network, stateName As String, initPr As String) As Boolean
            Return network.GetStates.Any(Function(x) x.Name = stateName) OrElse Not Double.TryParse(initPr, 0)
        End Function
        Public Function ValidateInitPr(network As Core.Model.Network) As Boolean
            Return network.GetStates.Sum(Function(x) x.InitPr) = 1
        End Function
    End Class
End Namespace
