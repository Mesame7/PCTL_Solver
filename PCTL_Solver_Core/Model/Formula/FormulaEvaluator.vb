﻿Imports System.Numerics

Namespace Core.Model.Formula

    Public Class FormulaEvaluator

        Private _MyNetwork As Network
        Public Sub New(network As Network)
            Me._MyNetwork = network
        End Sub

        Public Function EvaluateStateFormula(state As State, formula As StateFormula) As Boolean
            Dim timer = Stopwatch.StartNew()
            Dim output = formula.Evaluate(state)
            Console.WriteLine($"Formula Evaluated in {timer.Elapsed}")
            Return output
        End Function


        Public Function EvaluateProbStateFormula(state As State, stFormula As ProbabilityFormula) As Double

            Select Case stFormula.PathFormula.GetType

                Case GetType(NextFormula)
                    Return EvaluateNextFormula(state, stFormula.PathFormula)(state.Index, 0)
                Case GetType(UntilFiniteFormula)
                    Return EvaluateUntillFinite(state, stFormula.PathFormula)(state.Index, 0)
                Case GetType(UntilInfiniteFormula)
                    Dim s0 = GetS0(stFormula.PathFormula)
                    Return 0
                Case Else
                    Return 0
            End Select
        End Function


        Private Function EvaluateNextFormula(state As State, xFormula As NextFormula) As Double(,)
            Dim bitVector = FindSATVector(xFormula.StateFormula)
            Dim nextSAT = MultiplyMatrices(_MyNetwork.PMatrix, bitVector)
            Return nextSAT
        End Function


        Private Function EvaluateUntillFinite(state As State, uFormula As UntilFiniteFormula) As Double(,)
            Dim conditionStates As List(Of State) = GetStatesFomSATVector(FindSATVector(uFormula.FirstFormula)) 'C
            conditionStates.Sort(Function(x, y) x.Index < y.Index)
            Dim aMat = CalculateABMatFromStates(conditionStates, conditionStates)
            Dim lastStates As List(Of State) = GetStatesFomSATVector(FindSATVector(uFormula.LastFormula)) 'B
            lastStates.Sort(Function(x, y) x.Index < y.Index)
            Dim bMat = CalculateABMatFromStates(conditionStates, lastStates)
            Dim xn_1 = New Double(conditionStates.Count - 1, lastStates.Count - 1) {}
            For i = 0 To uFormula.HopCount - 1
                xn_1 = SolveLFP_OneIter(aMat, xn_1, bMat)
            Next
            Return xn_1
        End Function

        Private Function SolveLFP_OneIter(A As Double(,), xn As Double(,), b As Double(,)) As Double(,)
            Return AddMatrices(MultiplyMatrices(A, xn), b)
        End Function


        Private Function GetStatesFomSATVector(bitV As Double(,)) As List(Of State)
            Dim outList As New List(Of State)
            For i = 0 To bitV.Length - 1
                If bitV(i, 0) <> 0 Then
                    Dim tempI = i
                    outList.Add(_MyNetwork.GetStates.Where(Function(x) x.Index = tempI).FirstOrDefault)
                End If
            Next
            Return outList
        End Function


        'Page 765
        Function GetS0(untilFormula As UntilInfiniteFormula) As List(Of State)
            Dim lastStates As List(Of State) = GetStatesFomSATVector(FindSATVector(untilFormula.LastFormula))
            Dim conditionStates As List(Of State) = GetStatesFomSATVector(FindSATVector(untilFormula.FirstFormula))
            Dim outList As New List(Of State)
            For Each state In lastStates
                Dim visitedStates As New List(Of State) From {state}
                GetReachableStates(state, conditionStates, visitedStates)
                outList = outList.Union(visitedStates).ToList
            Next
            Return _MyNetwork.GetStates.Where(Function(x) Not outList.Contains(x)).ToList
        End Function


        'Page 777
        Function GetS1(untilFormula As UntilInfiniteFormula) As List(Of State)
            Dim lastStates As List(Of State) = GetStatesFomSATVector(FindSATVector(untilFormula.LastFormula))
            Dim preB As New List(Of State)
            For Each st In preB

            Next

        End Function

        Private Sub GetReachableStates(state As State, preConditionStates As List(Of State), ByRef visitedStates As List(Of State))
            For Each br In state.GetPreBranches
                Dim preState = br.FromState
                If Not visitedStates.Contains(preState) Then
                    visitedStates.Add(preState)
                    If preConditionStates.Contains(preState) Then
                        GetReachableStates(preState, preConditionStates, visitedStates)
                    End If
                End If
            Next
        End Sub

        Public Function FindSATVector(stF As StateFormula) As Double(,)
            Dim outSAT(_MyNetwork.GetStates.Count - 1, 0) As Double
            For Each state In _MyNetwork.GetStates
                outSAT(state.Index, 0) = If(stF.Evaluate(state), 1, 0)
            Next
            Return outSAT
        End Function
        Private Function MultiplyMatrices(matrix1 As Double(,), matrix2 As Double(,)) As Double(,)
            Dim numRows1 As Integer = matrix1.GetLength(0)
            Dim numCols1 As Integer = matrix1.GetLength(1)
            Dim numRows2 As Integer = matrix2.GetLength(0)
            Dim numCols2 As Integer = matrix2.GetLength(1)

            ' Initialize the result matrix
            Dim resultMatrix As Double(,) = New Double(numRows1 - 1, numCols2 - 1) {}

            ' Perform matrix multiplication
            For i As Integer = 0 To numRows1 - 1
                For j As Integer = 0 To numCols2 - 1
                    Dim sum As Double = 0
                    For k As Integer = 0 To numCols1 - 1
                        sum += matrix1(i, k) * matrix2(k, j)
                    Next
                    resultMatrix(i, j) = sum
                Next
            Next
            Return resultMatrix
        End Function
        Private Function AddMatrices(matrix1 As Double(,), matrix2 As Double(,)) As Double(,)
            If matrix1.GetLength(0) <> matrix2.GetLength(0) OrElse matrix1.GetLength(1) <> matrix2.GetLength(1) Then
                Throw New Exception("Mat size mismatch")
            End If
            Dim numRows As Integer = matrix1.GetLength(0)
                Dim numCols As Integer = matrix1.GetLength(1)

                ' Initialize the result matrix
                Dim resultMatrix As Double(,) = New Double(numRows - 1, numCols - 1) {}

                ' Perform element-wise addition
                For i As Integer = 0 To numRows - 1
                    For j As Integer = 0 To numCols - 1
                        resultMatrix(i, j) = matrix1(i, j) + matrix2(i, j)
                    Next
                Next

                Return resultMatrix
        End Function

        'Page 768
        Private Function CalculateABMatFromStates(states As List(Of State), lastStates As List(Of State)) As Double(,)
            Dim resultMatrix As Double(,) = New Double(states.Count - 1, lastStates.Count - 1) {}
            For i = 0 To states.Count - 1
                For j = 0 To lastStates.Count - 1
                    Dim tempJ = j
                    Dim nextState = states.ElementAt(i).GetBranches.Where(Function(x) x.ToState.Equals(lastStates.ElementAt(tempJ))).FirstOrDefault
                    If nextState Is Nothing Then
                        resultMatrix(i, j) = 0
                    Else
                        resultMatrix(i, j) = nextState.P
                    End If
                Next
            Next
            Return resultMatrix
        End Function



    End Class
End Namespace
