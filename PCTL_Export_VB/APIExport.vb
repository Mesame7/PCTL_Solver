Imports System.Runtime.InteropServices
Imports PCTL_Solver_Core.SystemManagement
Imports RGiesecke.DllExport
Namespace APIExporter

    Public Class APIExport
        Public Shared Sub ReadNetwork(filePath As String)
            SystemManager.GetInstance().ReadModelFromFile(filePath)
        End Sub
        Public Shared Sub EvaluateFormula(filePath As String)
            SystemManager.GetInstance().EvaluateFormulaFromFile(filePath)
        End Sub
    End Class
End Namespace
