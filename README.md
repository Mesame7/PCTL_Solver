**
# Model Checking with PCTL

In my thesis project, I'm building a tool aiming to solve model checking proplems with PCTL.

## Input Format
Input is expected to be a text file with the following format:

`s1:0.1:!A,!B,C:s1-0.7,s2-0.3`
`s2:0.9:A,!B,C:s2-0.7,s1-0.3`

where a column seperates state name, inital propability ,labels and branches from that state.
So in the first line: a state with the name s1 has an initial propability of 0.1, and it has the labels !A, !B, C, and finally 2 transitions, one to itself with a propability of 0.7 and another one to s2 with a propability 0.3 
**
