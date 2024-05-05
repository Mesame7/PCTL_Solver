Imports System.IO
Imports NUnit.Framework
Imports PCTL_Solver_Core
Imports PCTL_Solver_Core.Core.Model.Formula
Imports PCTL_Solver_Core.SystemManagement
Namespace MC_Tests

    Public Class Tests

        <SetUp>
        Public Sub Setup()
        End Sub



        <Test>
        Public Sub TestCraps2()
            Dim SysManager = New SystemManager
            Dim currentFolderPath As String = TestContext.CurrentContext.TestDirectory
            Dim subFolderPath = Path.Combine(currentFolderPath, "files")
            SysManager.ReadNetworkFromFile(subFolderPath + "\craps\craps.txt")
            Dim outVal = SysManager.EvaluateFormulaFromFile(subFolderPath + "\craps\formulas.txt")
            Assert.True(outVal)
            Assert.True(FormulaEvaluator.LastOutValue = 0.2608024691)
        End Sub




        <Test>
        Public Sub TestSimple()
            Dim SysManager = New SystemManager
            Dim currentFolderPath As String = TestContext.CurrentContext.TestDirectory
            Dim subFolderPath = Path.Combine(currentFolderPath, "files")
            SysManager.ReadNetworkFromFile(subFolderPath + "\Koenig\states.txt")
            Dim outVal = SysManager.EvaluateFormulaFromFile(subFolderPath + "\Koenig\formulas.txt")
            Assert.True(outVal)
            Assert.True(FormulaEvaluator.LastOutValue = 1)
        End Sub

    End Class

End Namespace