require_relative 'Tree.rb'
require_relative 'Algorithms.rb'

n = Node.new 2, 16, 15
#n.print_r
#n.reorder
#n.print_r "", true
#p negamax_alpha_beta n, 8
#p pvs n, 8
#p iterative_negamax_alpha_beta n, 8
p iterative_pvs n, 8