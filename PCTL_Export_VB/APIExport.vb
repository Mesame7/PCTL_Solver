Imports System.Runtime.InteropServices
Imports PCTL_Solver_Core.Core.Model.Formula
Imports PCTL_Solver_Core.SystemManagement
Imports RGiesecke.DllExport
Namespace APIExporter

    Public Class APIExport
        Public Shared Sub ReadNetwork(filePath As String)
            SystemManager.GetInstance().ReadModelFromFile(filePath)
        End Sub
        Public Shared Function EvaluateFormula(filePath As String) As Dictionary(Of String, Integer())
            SystemManager.GetInstance().EvaluateFormulaFromFile(filePath)
            Return FormulaEvaluator.GetEvaluationVector()
        End Function
        Public Shared Function GetStates() As String()
            Return SystemManager.GetInstance.GenerateStatesForPython()
        End Function
        Public Shared Function GetPMatrix() As Double()()
            Return SystemManager.GetInstance.GetPMatrixPython.Select(Function(row) row.ToArray()).ToArray()
        End Function
        Public Shared Function GetTimes() As Dictionary(Of String, Double)
            Return FormulaEvaluator._TimeDictionary
        End Function

        Public Shared Sub Reset()
            SystemManager.GetInstance.Reset()
        End Sub
    End Class
End Namespace
