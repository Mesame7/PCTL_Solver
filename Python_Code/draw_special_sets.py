#pip install networkx
import networkx as nx
from networkx.drawing.nx_agraph import to_agraph
import numpy as np
import imageio
import os
from pctl_utils import *

# for sample_size in list(range(100, 3000, 300)):
#     x=1
#     with open('output.txt', 'w') as file:
#     # Iterate from 1 to 998
#         file.write("s0:1:start:s1-1/2,s0-1/2\n")
#         for i in range(1, sample_size-1):
#             # Prepare the line with current index i
#             line = f"s{i}:0:mid:s{i+1}-1/2,s{i-1}-1/2\n"
            

            
#             # Write the line to the file
#             file.write(line)
#         line = f"s{sample_size-1}:0:final:s{sample_size-1}-1/2,s{sample_size-1}-1/2\n"
#         file.write(line)
#     with open('formulas.txt', 'w') as file:
#         file.write("s0 : ( Pin[1,1]{ ( true ) U ( final ) } ) ")
#         formulaBounded="s0 : ( Pin[1,1]{ ( true ) U<=sample_size ( final ) } )"
#         formulaBounded=formulaBounded.replace("sample_size",str(sample_size-1))
#         file.write(formulaBounded)
#         formulaNext="s0 : ( Pin[1,1]{ ( true ) U ( final ) } ) "
#         file.write(formulaNext)
#     model_full_path = os.path.join("D:\Thesis\python export", "output.txt")
#     formula_full_path = os.path.join("D:\Thesis\python export", "formulas.txt")
    
APIExporter = load_pctl_export_dll()

mc_file = r"D:\Thesis\marcov files\craps\craps.txt"

APIExporter.ReadNetwork(mc_file)

#pctl_file = r"D:\Thesis\marcov files\craps\formulas - Copy - inf.txt"
pctl_file = r"D:\Thesis\marcov files\craps\formulas.txt"
eval_dict_vb = APIExporter.EvaluateFormula(pctl_file)

eval_dict = {key: list(eval_dict_vb[key]) for key in eval_dict_vb.Keys}
for k, v in eval_dict.items():
    print(f"Key: {k}, Value: {v}")
N_steps = len(eval_dict.items())
# Markov chain parameters
node_positions = [
    (10, 10),
    (5, 5),
    (7, 5),
    (9, 5),
    (11, 5),
    (13, 5),
    (15, 5),
    (8, 0),
    (12, 0)
]

states = list(APIExporter.GetStates())
p_mat_vb = APIExporter.GetPMatrix()
Q = [[element for element in row] for row in p_mat_vb]

# Sampling the markov chain over 100 steps

node_ind = 0



# Setting up network

# Setting up node color for each iteration     
for k, v in eval_dict.items():
    node_colors = ['yellow' if x == 1 else 'green' for x in v]
    G = nx.MultiDiGraph()
    for i in range(len(states)):
        
        G.add_node(states[i], pos=node_positions[i], color=node_colors[i], type='gates')
    labels = {}
    edge_labels = {}

    for i, origin_state in enumerate(states):
        for j, destination_state in enumerate(states):
            rate = Q[i][j]
            if rate > 0:
                G.add_edge(origin_state, destination_state, weight=rate, label="{:.02f}".format(rate), len=50)

    
    A = to_agraph(G)
    A.graph_attr.update(splines='curved')
    A.node_attr['style']='filled'
    # for node_index in v:
    #     if v[node_index]:
    #         my_node=A.get_node(states[node_index])
    #         my_node.attr['fillcolor']='cyan'
    A.layout()
    A.draw(f'net_{k}.png')

# Create gif with imageio
images = []
filenames = [f'net_{k}.png' for k in range(N_steps)]
for filename in filenames:
    images.append(imageio.imread(filename))
imageio.mimsave('markov_chain.gif', images, fps=3)