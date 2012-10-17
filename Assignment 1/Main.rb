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
		negamax_without = concat.call negamax_without, iterative_negamax_alpha_beta(node, d)
		node.reset_daughters
		node.reorder
		negamax_with = concat.call negamax_with, iterative_negamax_alpha_beta(node, d)
		node.reset_daughters
		pvs_without = concat.call pvs_without, iterative_pvs(node, d)
		node.reset_daughters
		node.reorder
		pvs_with = concat.call pvs_with, iterative_pvs(node, d)
	end

	puts "Negamax a-b (without):\t#{negamax_without}"
	puts "Negamax a-b (with):\t#{negamax_with}"
	puts "PVS (without):\t\t#{pvs_without}"
	puts "PVS (with):\t\t#{pvs_with}"
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

input = get_input
experiment input[0], input[1], input[2]