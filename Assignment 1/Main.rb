require_relative 'Algorithms.rb'
require_relative 'Tree.rb'




n = Node.new 2, 4, 15
p negamax_alpha_beta n, 2
p iterative_negamax_alpha_beta n, 2