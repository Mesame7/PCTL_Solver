
Imports System.Text.RegularExpressions
Imports PCTL_Solver_Core.Core.Model
Imports PCTL_Solver_Core.Core.Model.Formula

Namespace SystemManagement


    Public Class SystemManager
        Private _Networks As New List(Of Core.Model.Network)
        Private _ActiveNetwork As Core.Model.Network
        Private _Evaluator As FormulaEvaluator

        Public Sub New()

        End Sub
        Public Function CreateNetwork(name As String) As Core.Model.Network
            If _Networks.Any(Function(x) x.Name = name) Then
                Throw New Exception("Network already exists") 'TODO Create Exceptions
            End If
            Dim network = New Core.Model.Network(name)
            _Networks.Add(network)
            _ActiveNetwork = network
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


        Public Function CreateStateFormula(f As String) As StateFormula
            f = f.Trim()
            If Regex.IsMatch(f, "^\s*\(.*\)\s*$") Then
                Dim conj = f.Substring(1, f.Length - 2).Trim
                If conj.StartsWith("!"c) Then
                    Return New NegatedStateFormula(_Evaluator, CreateStateFormula(conj.Substring(1)))
                ElseIf conj.StartsWith("P>") Then
                    Dim paramsTuple = GetPAndPathFormulaFromFormula(conj)
                    Return New ProbabilityFormula(paramsTuple.Item1, CreatePathFormula(paramsTuple.Item2))
                ElseIf Not conj.Contains("(") AndAlso Not conj.Contains("^") Then
                    If conj.ToLower = "true" OrElse conj.ToLower = "false" Then
                        Return New BooleanFormula(Boolean.Parse(conj))
                    Else
                        Return New LabelFormula(conj)
                    End If
                End If
                Dim conjArray = SplitFormulaIgnoringBrackets(conj)
                Dim conjFormula = New ConjunctionStateFormula()
                For Each subFormula In conjArray
                    conjFormula.AddSubState(CreateStateFormula(subFormula))
                Next
                Return conjFormula
            Else
                Console.Out.WriteLine("Please make sure you follow the rules for writing a formula")
            End If
            Return Nothing

        End Function
        Public Function CreatePathFormula(f As String) As PathFormula
            If Regex.IsMatch(f, "^\s*\[.*\]\s*$") Then

            Else
                Throw New Exception($"Issue with {f}")
            End If
        End Function
        Private Function GetPAndPathFormulaFromFormula(f As String) As Tuple(Of Double, String)
            f = f.Substring(2)
            Dim doubleString As String = ""
            For Each c In f
                If c <> "[" Then
                    doubleString += c
                End If
            Next
            Dim pathForm = f.Substring(doubleString.Length)
            If Regex.IsMatch(pathForm, "^\s*\[.*\]\s*$") Then
                Return New Tuple(Of Double, String)(Double.Parse(doubleString), pathForm)
            Else
                Throw New Exception($"Issue with Formula {f}")
            End If
        End Function
        Function SplitFormulaIgnoringBrackets(input As String) As String()
            Dim regexPattern As String = "\^(?![^\(]*\))"
            Dim regex As New Regex(regexPattern)
            Return regex.Split(input)
        End Function
    End Class
End Namespace
