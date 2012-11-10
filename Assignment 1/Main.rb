# Main.rb
# ---
# Author:
# Balázs Pete
# 09771417

require_relative 'Tree.rb'
require_relative 'Algorithms.rb'

def experiment b, d, a
	negamax_without, negamax_with, pvs_without, pvs_with = 0, 0, 0, 0
	concat = lambda {|count, arr| count + arr[2] }

	25.times do
		node = Node.new b, d, a
		negamax_without = 
		concat.call negamax_without, iterative_negamax_alpha_beta(node, d)
		node.reset_daughters
		negamax_with = 
		concat.call negamax_with, iterative_negamax_alpha_beta(node, d, true)
		node.reset_daughters
		pvs_without = concat.call pvs_without, iterative_pvs(node, d)
		node.reset_daughters
		pvs_with = concat.call pvs_with, iterative_pvs(node, d, true)
	end

	return "#{negamax_without}\t#{negamax_with}\t#{pvs_without}\t#{pvs_with}"
end

def get_input
	puts "Enter the desired breadth:"
	b = gets.chomp.to_i
	puts "Enter the desired depth:"
	d = gets.chomp.to_i
	puts "Enter the desired approx:"
	a = gets.chomp.to_i
	[b, d, a]
end

# input = get_input
# p experiment input[0], input[1], input[2]

# node = Node.new 2, 5, 30
# node.reorder
# node.print_r "", true
# node.reset_daughters
# node.print_r "", true



(4..8).each do |b|
	(3..9).each do |d|
		(0..6).each do |a|
			puts "b:#{b} d:#{d} a:#{a*5}\t\t#{experiment b, d, a*5}"
		end
	end
end