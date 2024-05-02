﻿Namespace Core.Model

    Public Class State
        Inherits SystemObject
        Public Sub New(name As String, initPr As Double, index As Integer)
            Me.Name = name
            Me.InitPr = initPr
            Me._Index = index
        End Sub

        Private _InitPr As Double
        Private _Labels As New List(Of Label)

        Private _NextBranches As New List(Of Branch)
        Private _PreBranches As New List(Of Branch)
        Private _Index As Integer

        Public ReadOnly Property Index As Integer
            Get
                Return _Index
            End Get
        End Property

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
        Public Sub AddPreBranch(br As Branch)
            _PreBranches.Add(br)
        End Sub
        Public Function GetBranches() As List(Of Branch)
            Return _NextBranches
        End Function
        Public Function GetNextStates() As List(Of State)
            Return _NextBranches.Select(Function(x) x.ToState)
        End Function
        Public Function GetPreBranches() As List(Of Branch)
            Return _PreBranches
        End Function
        Public Function GetPreStates() As List(Of Branch)
            Return _PreBranches.Select(Function(x) x.FromState)
        End Function
        Public Sub AddLabel(lb As Label)
            _Labels.Add(lb)
        End Sub
        Public Function GetLabels() As List(Of Label)
            Return _Labels
        End Function
    End Class
End Namespace

