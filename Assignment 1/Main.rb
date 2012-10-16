require_relative 'Tree.rb'
require_relative 'Algorithms.rb'





n = Node.new 2, 4, 15
n.print_r
n.reorder
n.print_r "", true
p negamax_alpha_beta n, 2
#p iterative_negamax_alpha_beta n, 2