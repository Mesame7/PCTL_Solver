import numpy as np
import matplotlib.pyplot as plt
from numpy.polynomial import Polynomial

# Data
model_sizes = [
    100, 400, 700, 1000, 1300, 1600, 1900, 2200, 2500, 2800,
    3100, 3400, 3700, 4000, 4300, 4600, 4900, 5200, 5500, 5800
]
set1_times = [
    0.0069684, 0.126991, 0.6531329, 1.8584944, 4.2660379, 7.8726982, 
    14.3417902, 21.0985671, 32.3894472, 44.6486793, 63.2363929, 
    81.0750186, 110.5853175, 131.965325, 179.4709429, 202.3928807, 
    275.3497268, 334.5567163, 421.7196072, 488.1596902
]
set2_times = [
    0.0144859, 0.3728976, 1.8885961, 5.5546709, 11.7607566, 22.1645222, 
    36.8958575, 56.9228914, 83.8582699, 117.2308072, 159.1032752, 
    209.4306513, 269.2576984, 340.7614351, 422.516422, 551.4734413, 
    668.7138092, 859.0373981, 1056.2031249, 1194.9151125
]
set3_times = [
    0.0003033, 0.0011324, 0.0031639, 0.0065267, 0.0089677, 0.014475, 
    0.0271109, 0.0269842, 0.0487271, 0.0614552, 0.0743592, 0.0900834, 
    0.1086477, 0.1198922, 0.1486437, 0.1807405, 0.1906437, 0.2540228, 
    0.2603824, 0.3607775
]

# Fit the data
cubic_fit1 = Polynomial.fit(model_sizes, set1_times, 3)
quartic_fit2 = Polynomial.fit(model_sizes, set2_times, 4)
quadratic_fit3 = Polynomial.fit(model_sizes, set3_times, 2)

# Generate points for plotting the fitted functions
x_plot = np.linspace(min(model_sizes), max(model_sizes), 500)
y_fit1 = cubic_fit1(x_plot)
y_fit2 = quartic_fit2(x_plot)
y_fit3 = quadratic_fit3(x_plot)

# Plotting
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
plt.plot(x_plot, y_fit2, color='red', label='4th Degree Polynomial Fit')
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
