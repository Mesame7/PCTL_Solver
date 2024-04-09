﻿Namespace Core.Model

    Public Class State
        Inherits SystemObject
        Public Sub New(name As String, initPr As Double)
            Me.Name = name
            Me.InitPr = initPr
        End Sub

        Private _InitPr As Double
        Private _Labels As New List(Of Label)

        Private _NextBranches As New List(Of Branch)



        Public Property InitPr As Double
            Get
                Return _InitPr
            End Get
            Set(value As Double)
                _InitPr = value
            End Set
        End Property
        Public Sub AddBranch(br As Branch)
            _NextBranches.Add(br)
        End Sub
        Public Function GetBranches() As List(Of Branch)
            Return _NextBranches
        End Function
        Public Sub AddLabel(lb As Label)
            _Labels.Add(lb)
        End Sub
        Public Function GetLabels() As List(Of Label)
            Return _Labels
        End Function
    End Class
End Namespace

