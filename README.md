
# Model Checking with PCTL

In my thesis project, I'm building a tool aiming to evaluate PCTL formulas on Markov chains.

For background, please check [my thesis](Thesis/Print_29.pdf)

## Python Code
Please install all the needed dependencies with:  
* `pip install pythonnet`
* `pip install networkx`  
* `pip install pygraphviz`  
* `pip install numpy`  

also please make sure you build the **`PCTL_Export_VB`** library and find the file:  

**`PATH_TO_PROJECT\PCTL_Solver\PCTL_Export_VB\bin\Debug\net48\PCTL_Export_VB.dll`**

and use its path in the function **`load_pctl_export_dll`**.

### Dynamic Creation of Models
Please use **`dynamic_net.py`** to dynamically create networks , formulas, evaluate them, and print their execution times.

### Graphs
In **`draw_special_sets.py`**, user can read a model and formula file, then manually setting the location of the nodes according to the same order as in the model file.


