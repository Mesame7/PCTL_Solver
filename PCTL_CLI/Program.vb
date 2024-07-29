Imports System

Imports System.Globalization
Imports System.IO
Imports System.Net.Mime.MediaTypeNames
Imports System.Text.RegularExpressions
Imports PCTL_Solver_Core
Imports PCTL_Solver_Core.Core.Model.Formula
Imports PCTL_Solver_Core.SystemManagement
Module Program
    Sub Main(args As String())
        Dim myCulture = CultureInfo.GetCultureInfo("en-US")
        Console.ForegroundColor = ConsoleColor.Yellow
        CultureInfo.DefaultThreadCurrentCulture = myCulture
        CultureInfo.DefaultThreadCurrentUICulture = myCulture

        Dim userInput As String
        Console.WriteLine("See All Commands with -help")
        Do
            Try
                Console.WriteLine("-----------------")
                Console.Write(">>Enter a command: ")
                userInput = Console.ReadLine()
                Dim inputArgs = GetInputArguments(userInput)
                Select Case inputArgs.FirstOrDefault.TrimStart("-").ToLower
                    Case "open"
                        OpenNetwork(inputArgs)
                    Case "new"
                        Console.WriteLine("You are creating a new network, Please enter the name: ")
                        userInput = Console.ReadLine()
                        Dim mynetwork = SystemManager.GetInstance().CreateNetwork(userInput)
                    Case "eval"
                        EvaluateFormulas(inputArgs)
                    Case "clear"
                        Reset()
                        Console.WriteLine("Saved networks cleared")
                    Case "time"
                        FormulaEvaluator.ShowTime = Not FormulaEvaluator.ShowTime
                        Console.WriteLine(If(FormulaEvaluator.ShowTime, "Time will show", "Time will not show"))
                    Case "value"
                        FormulaEvaluator.ShowValue = Not FormulaEvaluator.ShowValue
                        Console.WriteLine(If(FormulaEvaluator.ShowValue, "Value will show", "Value will not show"))
                    Case "dict"
                        FormulaEvaluator.AddToDict = Not FormulaEvaluator.AddToDict
                        Console.WriteLine(If(FormulaEvaluator.AddToDict, "Dict will be used", "Dict will not be used"))
                    Case "help"
                        Console.WriteLine(_Help)
                End Select
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
        Loop While userInput.ToLower() <> "exit"
        Console.WriteLine("Exiting the program. Press any key to close...")
        Console.ReadKey(True)
    End Sub
    Sub NetworkManager(network As Core.Model.Model)
        Dim userInput As String
        Console.Write(String.Format("You are managing now network : {0}", network.Name))
        Do
            userInput = Console.ReadLine()
            Select Case userInput.ToLower()
                Case "st"
                    Console.Write("You are creating a new state, Please enter the name: ")
                    userInput = Console.ReadLine()
                  '  Dim mynetwork = SystemManager.GetInstance().CreateNetwork(userInput)

                Case "br"

            End Select
        Loop While userInput.ToLower() <> "exit"

    End Sub

    Sub EvaluateFormulas(inputArgs As String())
        If inputArgs.Length > 1 Then
            SystemManager.GetInstance().EvaluateFormulaFromFile(inputArgs.ElementAt(1).Replace("""", ""))
        Else
            Console.WriteLine("Please specify a file to the network")
        End If

    End Sub
    Sub OpenNetwork(inputArgs As String())
        If inputArgs.Length > 1 Then
            SystemManager.GetInstance().ReadModelFromFile(inputArgs.ElementAt(1).Replace("""", ""))
        Else
            Console.WriteLine("Please specify a file to the network")
        End If
    End Sub

    Sub Reset()
        SystemManager.GetInstance.Reset()
    End Sub


    Function GetInputArguments(input As String) As String()
        Dim regexPattern As String = " (?=(?:[^""]*""[^""]*"")*[^""]*$)"
        Dim regex As New Regex(regexPattern)
        Return regex.Split(input)
    End Function

    Const _Help As String = "
--open command allows the user to load a model into the program by
specifying a .txt file containing the model.

How to execute: open PATH\TO\model.txt
---------------------------------------
---------------------------------------
--eval command enables the user to load and evaluate PCTL formulas
from a .txt file, subsequently displaying the results of the evaluation. It
should always be executed after executing a open command to have a model
for which the formulas can be evaluated.

How to execute: eval PATH\TO\formulas.txt
---------------------------------------
---------------------------------------
--clear command clears all loaded models and formulas, effectively resetting 
the program to its initial state. It can be called at any time with no
dependency on any other commands.
How to execute: clear
---------------------------------------
---------------------------------------
--time command can be used to toggle the display of execution time after
evaluating a formula. It can also be called at any time with no dependency
on any other commands.
How to execute: time
---------------------------------------
---------------------------------------
--value command can be used to toggle the display of the evaluated value
of the top-level ProbabilityFormula. This command can be invoked at any
time, independently of other commands.
How to execute: value
---------------------------------------
---------------------------------------
--round command can be used to set the number of digits to which final
values of the PathFormula will be rounded. This is since a Double value
in .NET has a precision between 15-17 bits as mentioned on the Microsoft
Learn page [6].
How to execute: round 6
---------------------------------------
---------------------------------------

--exit command to exit.
How to execute: exit
---------------------------------------
---------------------------------------
--help command displays descriptions of the previous commands along
with examples of how to use them
How to execute: help

"

End Module
