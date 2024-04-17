Imports System
Imports System.IO
Imports System.Text.RegularExpressions
Imports PCTL_Solver_Core.SystemManagement

Module Program
    Dim sysManager As SystemManager = New SystemManager()
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
                            ReadNetworkFromFile(inputArgs.ElementAt(1).Replace("""", ""))
                        Else
                            Console.Write("Please specify a file to the network")
                        End If
                    Case "new"
                        Console.Write("You are creating a new network, Please enter the name: ")
                        userInput = Console.ReadLine()
                        Dim mynetwork = sysManager.CreateNetwork(userInput)

                    Case "eval"
                        If inputArgs.Length > 1 Then
                            EvaluatePathFormula(inputArgs.ElementAt(1))
                        Else
                            Console.Write("Please specify a file to the network")
                        End If

                End Select
                ' Process the user input here (e.g., execute specific commands based on input).
                ' For now, let's just display the entered value.
                Console.WriteLine($"You entered: {userInput}")
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
    Sub ReadNetworkFromFile(path As String)

        Dim networkLines As New List(Of String())
        Try
            Using reader As New StreamReader(path)
                While (Not reader.EndOfStream)
                    networkLines.Add(reader.ReadLine().Replace(" ", "").Split(":"c))
                End While
            End Using
        Catch ex As Exception
            Console.WriteLine("Error reading the file: " & ex.Message)
        End Try

        Dim myNet = sysManager.CreateNetwork("Sample")
        For Each parts In networkLines
            sysManager.CreateState(
                myNet,
                parts.ElementAt(0),
                parts.ElementAt(1),
                parts.ElementAt(2))
        Next
        If Not sysManager.ValidateInitPr(myNet) Then
            Console.WriteLine("Please make sure that the inital properties sum to 1")
            Return
        Else
            Console.WriteLine(String.Format("Network {0} was created with {1} states", myNet.Name, myNet.GetStates.Count))
        End If
        For Each parts In networkLines
            sysManager.CreateBranch(myNet, parts.ElementAt(0), parts.ElementAt(3))
        Next
        Dim Ahmed = 1
    End Sub
    Function SplitStringIgnoringQuotes(input As String) As String()
        Dim regexPattern As String = " (?=(?:[^""]*""[^""]*"")*[^""]*$)"
        Dim regex As New Regex(regexPattern)
        Return regex.Split(input)
    End Function
    Sub EvaluatePathFormula(f As String)
        If Regex.IsMatch(f, "^\s*\(.*\)\s*$") Then


        Else
            Console.Out.WriteLine("Please make sure you follow the rules for writing a formula")
        End If


    End Sub

End Module
