Imports System.Numerics

Namespace Core.Model.Formula

    Public Class FormulaEvaluator
        Public Shared LastOutValue As Double 'Improve or remove
        Public Shared ShowTime As Boolean = True
        Public Shared ShowValue As Boolean = True
        Public Shared AddToDict As Boolean = False
        Public Shared _EvaluationDictionary As New Dictionary(Of String, Integer())
        Public Shared _TimeDictionary As New Dictionary(Of String, Double)
        Private Shared _EvalCounter As Integer = 0
        Private _MyNetwork As Model
        Public Sub New(network As Model)
            Me._MyNetwork = network
        End Sub

        Public Function EvaluateStateFormula(state As State, formula As StateFormula) As Boolean
            Dim timer = Stopwatch.StartNew()
            Dim output = formula.Evaluate(state)
            If ShowTime Then
                Console.WriteLine($"Formula Evaluated in {timer.Elapsed}")
                _TimeDictionary.Add($"{_MyNetwork.GetStates.Count}:{_EvalCounter}", timer.Elapsed.TotalSeconds)
            End If
            _EvalCounter += 1
            Return output
        End Function


        Public Function EvaluateProbStateFormula(state As State, stFormula As ProbabilityFormula) As Double
            Dim outVal As Double
            Select Case stFormula.PathFormula.GetType

                Case GetType(NextFormula)
                    outVal = EvaluateNextFormula(state, stFormula.PathFormula)(state.Index, 0)
                Case GetType(UntilFiniteFormula)
                    outVal = EvaluateUntillFinite(state, stFormula.PathFormula)
                Case GetType(UntilInfiniteFormula)
                    outVal = EvaluateUntillInfinite(state, stFormula.PathFormula)
                Case Else
                    outVal = -1
            End Select
            ' outVal = Math.Round(outVal, 10)
            LastOutValue = outVal
            If ShowValue Then
                Console.WriteLine($"P Formula evaluates to {outVal}")
            End If
            Return outVal
        End Function
        Private Function GetSATVectorFromStates(states As List(Of State)) As Integer()
            Dim outList(_MyNetwork.GetStates.Count - 1) As Integer
            For Each st In states
                outList(st.Index) = 1
            Next
            Return outList
        End Function
        Public Shared Function GetEvaluationVector() As Dictionary(Of String, Integer())
            Return _EvaluationDictionary
        End Function
        Private Function EvaluateNextFormula(state As State, xFormula As NextFormula) As Double(,)
            Dim bitVector = FindSATVector(xFormula.StateFormula)
            Dim nextSAT = MultiplyMatrices(_MyNetwork.PMatrix, bitVector)
            Return nextSAT
        End Function


        Private Function EvaluateUntillInfinite(state As State, uFormula As UntilInfiniteFormula) As Double
            Dim conditionStates As List(Of State) = GetStatesFomSATVector(FindSATVector(uFormula.FirstFormula)) 'C
            Dim lastStates As List(Of State) = GetStatesFomSATVector(FindSATVector(uFormula.LastFormula)) 'B
            Dim s0 = GetS0(uFormula)
            Dim s1 = GetS1Prism(uFormula, s0) 'TODO 
            If s1.Contains(state) Then
                Return 1
            ElseIf s0.Contains(state) Then
                Return 0
            End If

            Dim sOther = _MyNetwork.GetStates.Where(Function(x) Not s0.Contains(x) AndAlso Not s1.Contains(x)).ToList
            If AddToDict Then

                _EvaluationDictionary.Add("S0", GetSATVectorFromStates(s0))
                _EvaluationDictionary.Add("S1", GetSATVectorFromStates(s1))

                _EvaluationDictionary.Add("S_Unknown", GetSATVectorFromStates(s1))
            End If
            Dim steadyStateMat = GetSteadyStateMatrix(sOther)
            Dim out = SolveWithLU(steadyStateMat, GetConstantMatrix(s1))
            Return out(state.Index)
        End Function
        Private Function EvaluateUntillFinite(state As State, uFormula As UntilFiniteFormula) As Double
            Dim conditionStates As List(Of State) = GetStatesFomSATVector(FindSATVector(uFormula.FirstFormula)) 'C
            Dim lastStates As List(Of State) = GetStatesFomSATVector(FindSATVector(uFormula.LastFormula)) 'B
            Dim s0 = GetS0(uFormula)
            Dim s1 = GetS1(uFormula) 'TODO 
            If s1.Contains(state) Then
                Return 1
            ElseIf s0.Contains(state) Then
                Return 0
            End If
            Dim s_rest = conditionStates.Where(Function(x) Not s0.Contains(x) AndAlso Not lastStates.Contains(x)).ToList
            s_rest.Sort(Function(x, y) x.Index < y.Index)
            If AddToDict Then

                _EvaluationDictionary.Add("S0", GetSATVectorFromStates(s0))
                _EvaluationDictionary.Add("S1", GetSATVectorFromStates(s1))
                _EvaluationDictionary.Add("S_Unknown", GetSATVectorFromStates(s_rest))
            End If
            Dim aMat = CalculateABMatFromStates(s_rest, s_rest)
            lastStates.Sort(Function(x, y) x.Index < y.Index)
            Dim bMat = CalculateABMatFromStates(s_rest, lastStates)
            Dim xn_1 = New Double(s_rest.Count - 1, lastStates.Count - 1) {} 'TODO Sum them up
            For i = 0 To uFormula.HopCount - 1
                xn_1 = SolveLFP_OneIter(aMat, xn_1, bMat)
            Next
            Return xn_1(state.Index, 0)
        End Function

        Private Function SolveLFP_OneIter(A As Double(,), xn As Double(,), b As Double(,)) As Double(,)
            Return AddMatrices(MultiplyMatrices(A, xn), b)
        End Function


        Private Function GetStatesFomSATVector(bitV As Double(,)) As List(Of State)
            Dim outList As New List(Of State)
            For i = 0 To bitV.Length - 1
                If bitV(i, 0) <> 0 Then
                    Dim tempI = i
                    outList.Add(_MyNetwork.GetStates.Where(Function(x) x.Index = tempI).FirstOrDefault)
                End If
            Next
            Return outList
        End Function


        'Page 765
        Function GetS0(untilFormula As UntilInfiniteFormula) As List(Of State)
            Dim lastStates As List(Of State) = GetStatesFomSATVector(FindSATVector(untilFormula.LastFormula))
            Dim conditionStates As List(Of State) = GetStatesFomSATVector(FindSATVector(untilFormula.FirstFormula))
            Dim outList As New List(Of State)
            For Each state In lastStates
                Dim visitedStates As New List(Of State) From {state}
                GetReachableStates(state, conditionStates, visitedStates)
                outList = outList.Union(visitedStates).ToList
            Next
            Return _MyNetwork.GetStates.Where(Function(x) Not outList.Contains(x)).ToList
        End Function


        'Page 777
        Function GetS1(untilFormula As UntilInfiniteFormula) As List(Of State)
            Return GetStatesFomSATVector(FindSATVector(untilFormula.LastFormula))

            'Dim lastStates As List(Of State) = GetStatesFomSATVector(FindSATVector(untilFormula.LastFormula))
            'Dim preB = GetPreStar(lastStates)
            'Dim s_Slash_preB = _MyNetwork.GetStates.Where(Function(x) Not preB.Contains(x)).ToList
            'Dim pre_s_Slash_preB = GetPreStar(s_Slash_preB)
            'Dim out = _MyNetwork.GetStates().Where(Function(x) Not pre_s_Slash_preB.Contains(x)).ToList
            'Return out
        End Function

        Function GetS1Prism(untilFormula As UntilInfiniteFormula, s0 As List(Of State)) As List(Of State)
            Dim lastStates As List(Of State) = GetStatesFomSATVector(FindSATVector(untilFormula.LastFormula))
            Dim conditionStates As List(Of State) = GetStatesFomSATVector(FindSATVector(untilFormula.FirstFormula))
            Dim differenceStates = conditionStates.Where(Function(x) Not lastStates.Contains(x))
            Dim R As List(Of State) = s0

            Dim f = False
            While Not f
                Dim R_dash = R.Union(differenceStates.Where(Function(x) x.GetNextStates.Any(Function(y) R.Contains(y)))).ToList 'TODO assuming next branches always have p >0

                If Not (R_dash.Count <> R.Count OrElse R_dash.Any(Function(x) Not R.Contains(x)) OrElse R.Any(Function(x) Not R_dash.Contains(x))) Then
                    f = True
                End If
                R = R.Union(R_dash).ToList
            End While
            Dim output = _MyNetwork.GetStates.Where(Function(x) Not R.Contains(x)).ToList
            Return output
        End Function
        Private Function GetPreStar(states As List(Of State))
            Dim preB As New List(Of State)
            For Each st In states
                GetReachableStates(st, preB)
            Next
            Return preB
        End Function

        'Private Sub GetReachableStates(state As State, ByRef visitedStates As List(Of State))
        '    For Each preState In state.GetPreStates
        '        If Not visitedStates.Contains(preState) Then
        '            visitedStates.Add(preState)

        '            GetReachableStates(preState, visitedStates)
        '        End If
        '    Next
        'End Sub
        Private Sub GetReachableStates(state As State, ByRef visitedStates As List(Of State))
            Dim stack As New Stack(Of State)
            stack.Push(state)

            While stack.Count > 0
                Dim currentState As State = stack.Pop()

                For Each preState In currentState.GetPreStates()
                    If Not visitedStates.Contains(preState) Then
                        visitedStates.Add(preState)
                        stack.Push(preState)
                    End If
                Next
            End While
        End Sub
        Private Sub GetReachableStates(state As State, preConditionStates As List(Of State), ByRef visitedStates As List(Of State))
            'For Each preState In state.GetPreStates
            '    If Not visitedStates.Contains(preState) Then
            '        If preConditionStates.Contains(preState) Then
            '            visitedStates.Add(preState)
            '            GetReachableStates(preState, preConditionStates, visitedStates)
            '        End If
            '    End If
            'Next
            Dim stack As New Stack(Of State)
            stack.Push(state)

            While stack.Count > 0
                Dim currentState As State = stack.Pop()

                For Each preState In currentState.GetPreStates()
                    If Not visitedStates.Contains(preState) Then
                        If preConditionStates.Contains(preState) Then
                            visitedStates.Add(preState)
                            stack.Push(preState)
                        End If
                    End If
                Next
            End While
        End Sub

        Public Function FindSATVector(stF As StateFormula) As Double(,)
            Dim outSAT(_MyNetwork.GetStates.Count - 1, 0) As Double
            For Each state In _MyNetwork.GetStates
                outSAT(state.Index, 0) = If(stF.Evaluate(state), 1, 0)
            Next
            Return outSAT
        End Function

        Private Function GetConstantMatrix(s1 As List(Of State)) As Double()
            Dim numRows = _MyNetwork.GetStates.Count
            Dim out(numRows - 1) As Double
            For Each state In s1
                out(state.Index) = 1
            Next
            Return out
        End Function
        Private Function GetSteadyStateMatrix(sOther As List(Of State)) As Double(,)
            Dim numRows = _MyNetwork.GetStates.Count
            Dim out(numRows - 1, numRows - 1) As Double
            For i = 0 To numRows - 1
                out(i, i) = 1

            Next
            For Each s In sOther
                For Each nextBranch In s.GetBranches
                    out(s.Index, nextBranch.ToState.Index) -= nextBranch.P
                Next
            Next
            Return out
        End Function
        Private Function GetSteadyStateMatrix(cStates As List(Of State), lastStates As List(Of State)) As Double(,)
            Dim numRows = _MyNetwork.GetStates.Count
            Dim out(numRows - 1, numRows - 1) As Double
            Dim resultMatrix As Double(,) = New Double(numRows - 1, numRows - 1) {}

            For Each s In cStates.Concat(lastStates)
                out(s.Index, s.Index) = 1
                For Each nextBranch In s.GetBranches
                    out(s.Index, nextBranch.ToState.Index) = -nextBranch.P
                Next
            Next
            Return out
        End Function
        Private Function MultiplyMatrices(matrix1 As Double(,), matrix2 As Double(,)) As Double(,)
            Dim numRows1 = matrix1.GetLength(0)
            Dim numCols1 = matrix1.GetLength(1)
            Dim numRows2 = matrix2.GetLength(0)
            Dim numCols2 = matrix2.GetLength(1)
            ' Initialize the result matrix
            Dim resultMatrix As Double(,) = New Double(numRows1 - 1, numCols2 - 1) {}
            ' Perform matrix multiplication
            For i As Integer = 0 To numRows1 - 1
                For j As Integer = 0 To numCols2 - 1
                    Dim sum As Double = 0
                    For k As Integer = 0 To numCols1 - 1
                        sum += matrix1(i, k) * matrix2(k, j)
                    Next
                    resultMatrix(i, j) = sum
                Next
            Next
            Return resultMatrix
        End Function
        Private Function AddMatrices(matrix1 As Double(,), matrix2 As Double(,)) As Double(,)
            If matrix1.GetLength(0) <> matrix2.GetLength(0) OrElse matrix1.GetLength(1) <> matrix2.GetLength(1) Then
                Throw New Exception("Mat size mismatch")
            End If
            Dim numRows As Integer = matrix1.GetLength(0)
            Dim numCols As Integer = matrix1.GetLength(1)

            ' Initialize the result matrix
            Dim resultMatrix As Double(,) = New Double(numRows - 1, numCols - 1) {}

            ' Perform element-wise addition
            For i As Integer = 0 To numRows - 1
                For j As Integer = 0 To numCols - 1
                    resultMatrix(i, j) = matrix1(i, j) + matrix2(i, j)
                Next
            Next

            Return resultMatrix
        End Function

        'Page 768
        Private Function CalculateABMatFromStates(states As List(Of State), lastStates As List(Of State)) As Double(,)
            Dim resultMatrix As Double(,) = New Double(states.Count - 1, lastStates.Count - 1) {}
            For i = 0 To states.Count - 1
                For j = 0 To lastStates.Count - 1
                    Dim tempJ = j
                    Dim nextState = states.ElementAt(i).GetBranches.Where(Function(x) x.ToState.Equals(lastStates.ElementAt(tempJ))).FirstOrDefault
                    If nextState Is Nothing Then
                        resultMatrix(i, j) = 0
                    Else
                        resultMatrix(i, j) = nextState.P
                    End If
                Next
            Next
            Return resultMatrix
        End Function


        '' We use this algorithm from https://www.geeksforgeeks.org/gaussian-elimination/
        Private Function SolveWithGaussian(ByVal coefficients As Double(,), ByVal constants As Double()) As Double()
            Dim num_rows As Integer = coefficients.GetLength(0)
            Dim num_cols As Integer = coefficients.GetLength(1)

            ' Augment the matrix with constants
            Dim augmented_matrix(num_rows - 1, num_cols) As Double
            For r As Integer = 0 To num_rows - 1
                For c As Integer = 0 To num_cols - 1
                    augmented_matrix(r, c) = coefficients(r, c)
                Next
                augmented_matrix(r, num_cols) = constants(r)
            Next

            ' Perform Gaussian elimination
            For pivot_row As Integer = 0 To num_rows - 1
                Dim pivot_value As Double = augmented_matrix(pivot_row, pivot_row)
                For r As Integer = pivot_row + 1 To num_rows - 1
                    Dim factor As Double = augmented_matrix(r, pivot_row) / pivot_value
                    For c As Integer = pivot_row To num_cols
                        augmented_matrix(r, c) -= factor * augmented_matrix(pivot_row, c)
                    Next
                Next
            Next

            ' Backsolve to find the solution
            Dim solution(num_rows - 1) As Double
            For r As Integer = num_rows - 1 To 0 Step -1
                Dim sum As Double = 0
                For c As Integer = r + 1 To num_rows - 1
                    sum += augmented_matrix(r, c) * solution(c)
                Next
                solution(r) = (augmented_matrix(r, num_cols) - sum) / augmented_matrix(r, r)
            Next

            Return solution
        End Function
        Public Function LU_Decomposition(ByVal coefficients As Double(,)) As Tuple(Of Double(,), Double(,))
            Dim num_rows As Integer = coefficients.GetLength(0)
            Dim num_cols As Integer = coefficients.GetLength(1)

            If num_rows <> num_cols Then
                Throw New ArgumentException("Matrix must be square")
            End If

            Dim L(num_rows - 1, num_cols - 1) As Double
            Dim U(num_rows - 1, num_cols - 1) As Double

            For i As Integer = 0 To num_rows - 1
                For j As Integer = 0 To i
                    Dim sum As Double = 0
                    For k As Integer = 0 To j - 1
                        sum += L(i, k) * U(k, j)
                    Next
                    L(i, j) = coefficients(i, j) - sum
                Next
                For j As Integer = i To num_cols - 1
                    Dim sum As Double = 0
                    For k As Integer = 0 To i - 1
                        sum += L(i, k) * U(k, j)
                    Next
                    If L(i, i) = 0 Then
                        Throw New ArgumentException("Matrix is singular")
                    End If
                    U(i, j) = (coefficients(i, j) - sum) / L(i, i)
                Next
            Next

            Return New Tuple(Of Double(,), Double(,))(L, U)
        End Function

        Public Function ForwardSubstitution(ByVal L As Double(,), ByVal b As Double()) As Double()
            Dim n As Integer = L.GetLength(0)
            Dim y(n - 1) As Double

            For i As Integer = 0 To n - 1
                y(i) = b(i)
                For j As Integer = 0 To i - 1
                    y(i) -= L(i, j) * y(j)
                Next
                y(i) /= L(i, i)
            Next

            Return y
        End Function

        Public Function BackwardSubstitution(ByVal U As Double(,), ByVal y As Double()) As Double()
            Dim n As Integer = U.GetLength(0)
            Dim x(n - 1) As Double

            For i As Integer = n - 1 To 0 Step -1
                x(i) = y(i)
                For j As Integer = i + 1 To n - 1
                    x(i) -= U(i, j) * x(j)
                Next
                x(i) /= U(i, i)
            Next

            Return x
        End Function

        Public Function SolveWithLU(ByVal coefficients As Double(,), ByVal constants As Double()) As Double()
            Dim LU = LU_Decomposition(coefficients)
            Dim L = LU.Item1
            Dim U = LU.Item2
            Dim y As Double() = ForwardSubstitution(L, constants)
            Dim x As Double() = BackwardSubstitution(U, y)
            Return x
        End Function


    End Class
End Namespace
