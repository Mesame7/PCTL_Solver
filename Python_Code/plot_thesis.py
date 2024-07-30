#pip install networkx
#pip install pygraphviz
#pip install numpy
#pip install imageio
#pip install matplotlib


import numpy as np
import matplotlib.pyplot as plt
from numpy.polynomial import Polynomial

# Add the modelsizes and times generated from dynamic_net.py
model_sizes = [
    100, 400, 700, 1000, 1300, 1600, 1900, 2200, 2500, 2800,
    3100, 3400, 3700, 4000, 4300, 4600, 4900, 5200, 5500, 5800
]
set1_times = [0.013115, 0.267055, 1.452369, 3.771014, 8.383276, 16.718130, 28.498569, 42.044822, 66.395127, 87.325519, 123.773637, 157.105593, 220.584330, 242.006255, 310.851273, 373.741378, 485.681482, 560.878811, 655.308628, 732.259680]
set2_times = [0.008809, 0.424408, 1.921505, 5.664472, 11.954765, 24.055948, 38.798166, 62.787263, 93.385719, 125.847799, 169.194850, 221.173322, 297.601152, 348.580019, 420.309127, 547.876274, 675.198405, 762.582375, 917.847000, 1027.687399]
set3_times = [0.000354, 0.001315, 0.003685, 0.006462, 0.009425, 0.016664, 0.029867, 0.028973, 0.047330, 0.064428, 0.072881, 0.088402, 0.115953, 0.121219, 0.149066, 0.197118, 0.215212, 0.235134, 0.254656, 0.299438]

# Fit the data
cubic_fit1 = Polynomial.fit(model_sizes, set1_times, 3)
quartic_fit2 = Polynomial.fit(model_sizes, set2_times, 3)
quadratic_fit3 = Polynomial.fit(model_sizes, set3_times, 2)

# Generate points for plotting the fitted functions
x_plot = np.linspace(min(model_sizes), max(model_sizes), 500)
y_fit1 = cubic_fit1(x_plot)
y_fit2 = quartic_fit2(x_plot)
y_fit3 = quadratic_fit3(x_plot)

# Plot

plt.figure(figsize=(12, 18))

# UnboundedUntilFormula
plt.subplot(3, 1, 1)
plt.scatter(model_sizes, set1_times, color='blue', label='Data')
plt.plot(x_plot, y_fit1, color='red', label='Cubic Fit')
plt.title('UnboundedUntilFormula')
plt.xlabel('Model Size')
plt.ylabel('Execution Time')
plt.legend()

# BoundedUntilFormula
plt.subplot(3, 1, 2)
plt.scatter(model_sizes, set2_times, color='green', label='Data')
plt.plot(x_plot, y_fit2, color='red', label='Cubic Fit')
plt.title('BoundedUntilFormula')
plt.xlabel('Model Size')
plt.ylabel('Execution Time')
plt.legend()

# NextFormula
plt.subplot(3, 1, 3)
plt.scatter(model_sizes, set3_times, color='purple', label='Data')
plt.plot(x_plot, y_fit3, color='red', label='Quadratic Fit')
plt.title('NextFormula')
plt.xlabel('Model Size')
plt.ylabel('Execution Time')
plt.legend()

plt.tight_layout()
plt.show()
