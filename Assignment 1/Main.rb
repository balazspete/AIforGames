# Main.rb
# ---
# Author:
# Balázs Pete
# 09771417

require_relative 'Tree.rb'
require_relative 'Algorithms.rb'


n =
Node.new 6, 3, 25
#n.print_r
n.reorder
#n.print_r "", true
#p negamax_alpha_beta n, 8
#p pvs n, 8
p iterative_negamax_alpha_beta n, 3
p iterative_pvs n, 3