Imports System
Imports System.IO
Imports System.Text.RegularExpressions
Imports PCTL_Solver_Core.Core.Model.Formula
Imports PCTL_Solver_Core.SystemManagement
Module Program
    Public SysManager As SystemManager = New SystemManager()
    Sub Main(args As String())
        Dim userInput As String

        Console.WriteLine("Type 'exit' to quit.")
        Do
            Try

                Console.Write("Enter a command: ")
                userInput = Console.ReadLine()
                Dim inputArgs = SplitStringIgnoringQuotes(userInput)
                Select Case inputArgs.FirstOrDefault
                    Case "open"
                        If inputArgs.Length > 1 Then
                            SysManager.ReadNetworkFromFile(inputArgs.ElementAt(1).Replace("""", ""))
                        Else
                            Console.Write("Please specify a file to the network")
                        End If
                    Case "new"
                        Console.Write("You are creating a new network, Please enter the name: ")
                        userInput = Console.ReadLine()
                        Dim mynetwork = SysManager.CreateNetwork(userInput)

                    Case "eval"
                        If inputArgs.Length > 1 Then
                            SysManager.EvaluateFormulaFromFile(inputArgs.ElementAt(1).Replace("""", ""))
                        Else
                            Console.Write("Please specify a file to the network")
                        End If

                End Select
                ' Process the user input here (e.g., execute specific commands based on input).
                ' For now, let's just display the entered value.
                'Console.WriteLine($"You entered: {userInput}")
            Catch ex As Exception
                Console.WriteLine(ex.Message)

            End Try
        Loop While userInput.ToLower() <> "exit"

        Console.WriteLine("Exiting the program. Press any key to close...")
        Console.ReadKey(True)
    End Sub
    Sub NetworkManager(network As Core.Model.Network)
        Dim userInput As String
        Console.Write(String.Format("You are managing now network : {0}", network.Name))
        Do
            userInput = Console.ReadLine()
            Select Case userInput.ToLower()
                Case "st"
                    Console.Write("You are creating a new state, Please enter the name: ")
                    userInput = Console.ReadLine()
                  '  Dim mynetwork = sysManager.CreateNetwork(userInput)

                Case "br"

            End Select
        Loop While userInput.ToLower() <> "exit"

    End Sub


    Function SplitStringIgnoringQuotes(input As String) As String()
        Dim regexPattern As String = " (?=(?:[^""]*""[^""]*"")*[^""]*$)"
        Dim regex As New Regex(regexPattern)
        Return regex.Split(input)
    End Function

End Module
