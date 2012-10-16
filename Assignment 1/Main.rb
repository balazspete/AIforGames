require_relative 'Algorithms.rb'
require_relative 'Tree.rb'




n = Node.new 2, 7, 15
n.print_r
puts negamax_alpha_beta n, 2