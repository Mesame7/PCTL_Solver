Imports System
Imports PCTL_Solver_Core.SystemManagement

Module Program
    Sub Main(args As String())
        Dim userInput As String
        Dim sysManager As SystemManager = New SystemManager()
        Console.WriteLine("Type 'exit' to quit.")
        Do
            Console.Write("Enter a command: ")
            userInput = Console.ReadLine()
            Select Case userInput.ToLower()
                Case "new"
                    Console.Write("You are creating a new network, Please enter the name: ")
                    userInput = Console.ReadLine()
                    Dim mynetwork = sysManager.CreateNetwork(userInput)

                Case "my"

            End Select
            ' Process the user input here (e.g., execute specific commands based on input).
            ' For now, let's just display the entered value.
            Console.WriteLine($"You entered: {userInput}")

        Loop While userInput.ToLower() <> "exit"

        Console.WriteLine("Exiting the program. Press any key to close...")
        Console.ReadKey(True)
    End Sub
    Sub NetworkManager(network As Core.Model.System)
        Dim userInput As String
        Console.Write(String.Format("You are managing now network : {0}", network.Name))
        Do
            userInput = Console.ReadLine()
            Select Case userInput.ToLower()
                Case "st"
                    Console.Write("You are creating a new state, Please enter the name: ")
                    userInput = Console.ReadLine()
                    Dim mynetwork = sysManager.CreateNetwork(userInput)

                Case "br"

            End Select
        Loop While userInput.ToLower() <> "exit"

    End Sub

End Module
