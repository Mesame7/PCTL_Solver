#pip install networkx
import networkx as nx
from networkx.drawing.nx_agraph import to_agraph
import numpy as np
import imageio
import os
from pctl_utils import *


APIExporter = load_pctl_export_dll()
for sample_size in list(range(100, 5900, 300)):
    print("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$")
    print("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$")
    print(f"Sample Size: {sample_size}")
    with open('output.txt', 'w') as file:
    # Iterate from 1 to 998
        file.write("s0:1:start:s1-1/2,s0-1/2\n")
        for i in range(1, sample_size-1):
            # Prepare the line with current index i
            line = f"s{i}:0:mid:s{i+1}-1/2,s{i-1}-1/2\n"
            

            
            # Write the line to the file
            file.write(line)
        line = f"s{sample_size-1}:0:final:s{sample_size-1}-1/2,s{sample_size-2}-1/2\n"
        file.write(line)
    with open('formulas.txt', 'w') as file:
        file.write("s0 : ( Pin[1,1]{ ( true ) U ( final ) } ) \n")
        formulaBounded="s0 : ( Pin[1,1]{ ( true ) U<=sample_size ( final ) } )\n"
        formulaBounded=formulaBounded.replace("sample_size",str(sample_size-1))
        file.write(formulaBounded)
        formulaNext="s0 : ( Pin[1,1]{ X ( s1 ) } ) \n"
        file.write(formulaNext)
    
    model_full_path = os.path.join(r"C:\Users\ahmed.mansour\Desktop\Private\python export\python export", "output.txt")
    formula_full_path = os.path.join(r"C:\Users\ahmed.mansour\Desktop\Private\python export\python export", "formulas.txt")
    APIExporter.Reset()    
    APIExporter.ReadNetwork(model_full_path)

    APIExporter.EvaluateFormula(formula_full_path)
    APIExporter.Reset()
    print("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$")
    print("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$")
my_dict=APIExporter.GetTimes()
for key in my_dict:
    print(f"Key: {key.Key}, Value: {key.Value}")
    