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
        Console.WriteLine("Type 'exit' to quit.")
        Do
            Try
                Console.Write("Enter a command: ")
                userInput = Console.ReadLine()
                Dim inputArgs = GetInputArguments(userInput)
                Select Case inputArgs.FirstOrDefault.ToLower
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

End Module
