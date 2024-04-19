Imports PCTL_Solver_Core.Core.Model.Formula

Namespace Core.Model


    Public Class Network

        Private _Name As String
        Private _States As New List(Of State)
        Private _Branches As New List(Of Branch)
        Private _StateFormulas As New List(Of StateFormula)
        Private _PMatrix As Double(,)
        Private _Evaluator As FormulaEvaluator
        Public ReadOnly Property PMatrix As Double(,)
            Get
                Return _PMatrix
            End Get
        End Property
        Public ReadOnly Property Evaluator As FormulaEvaluator
            Get
                Return _Evaluator
            End Get
        End Property
        Public Property Name As String
            Get
                Return _Name
            End Get
            Set(value As String)
                _Name = value
            End Set
        End Property
        Public Sub New(name As String)
            Me._Name = name
            Me._Evaluator = New FormulaEvaluator(Me)
        End Sub
        Public Sub AddBranch(br As Branch)
            _Branches.Add(br)
        End Sub
        Public Function GetBranches() As List(Of Branch)
            Return _Branches
        End Function

        Public Sub AddState(st As State)
            _States.Add(st)
        End Sub

        Public Sub AddStateFormula(formula As StateFormula)
            _StateFormulas.Add(formula)
        End Sub

        Public Function GetStates() As List(Of State)
            Return _States
        End Function
        Public Function GetState(name As String) As State
            Return _States.Where(Function(x) x.Name = name).FirstOrDefault
        End Function
        Public Sub GeneratePMatrix()
            Dim matrix(_States.Count - 1, _States.Count - 1) As Double
            _States.OrderBy(Of Integer)(Function(x) x.Index)
            For Each st In _States
                For Each br In st.GetBranches
                    matrix(st.Index, br.ToState.Index) = br.P
                Next
            Next
            _PMatrix = matrix
        End Sub

        Public Function EvaluateStateFormula(state As State, formula As StateFormula) As Boolean
            Return _Evaluator.EvaluateStateFormula(state, formula)
        End Function

    End Class
End Namespace
