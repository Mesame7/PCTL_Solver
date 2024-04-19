﻿Imports System.Numerics

Namespace Core.Model.Formula

    Public Class FormulaEvaluator

        Private _MyNetwork As Network
        Public Sub New(network As Network)
            Me._MyNetwork = network
        End Sub

        Public Function EvaluateStateFormula(state As State, formula As StateFormula) As Boolean
            Return formula.Evaluate(state)
        End Function


        Public Function EvaluateProbStateFormula(state As State, stFormula As ProbabilityFormula) As Boolean

            Select Case stFormula.PathFormula.GetType

                Case GetType(NextFormula)
                    Return EvaluateNextFormula(state, stFormula.PathFormula)(state.Index, 0) > stFormula.P
                Case GetType(UntilFiniteFormula)

                Case GetType(UntilInfiniteFormula)


            End Select
        End Function


        Private Function EvaluateNextFormula(state As State, xFormula As NextFormula) As Double(,)
            Dim bitVector = FindSAT(xFormula.StateFormula)
            Dim nextSAT = MultiplyMatrices(_MyNetwork.PMatrix, bitVector)
            Return nextSAT
        End Function

        Public Function FindSAT(stF As StateFormula) As Double(,)
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

    End Class
End Namespace
