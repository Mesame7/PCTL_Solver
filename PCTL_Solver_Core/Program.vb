Imports System

Module Program
    Sub Main(args As String())
        Dim userInput As String

        Console.WriteLine("Type 'exit' to quit.")
        Do
            Console.Write("Enter a command: ")
            userInput = Console.ReadLine()

            ' Process the user input here (e.g., execute specific commands based on input).
            ' For now, let's just display the entered value.
            Console.WriteLine($"You entered: {userInput}")

        Loop While userInput.ToLower() <> "exit"

        Console.WriteLine("Exiting the program. Press any key to close...")
        Console.ReadKey(True)
    End Sub
End Module
