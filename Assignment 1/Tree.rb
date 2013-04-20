# Tree.rb
# ---
# Author:
# Balázs Pete
# 09771417

# Class representing a node of a tree
class Node
	attr_reader :e, :children
	# Daughters can be used to reorder child nodes
	attr_accessor :daughters

	# Initialise note, and and further initialise children depending on input attributes
	def initialize b, d, approx, _t=nil
		random = lambda {|min, max| rand((max - min).abs)+min }
		new_node = lambda {|__t| Node.new b,d-1,approx,__t}
		t = (_t or random.call(-250, 250))

		@e = t
		@children = []

		if d > 1
			@e = t + random.call(-approx, approx)
			@children = [new_node.call(-(t or t))]
			(b-1).times do
				if [true,false].sample
					@children.unshift new_node.call(random.call(-t, 1000))
				else
					@children.push new_node.call(random.call(-t, 1000))
				end
			end
		end

		@daughters = Array.new @children
	end
	# String representation of the node
	def to_s
		"<Node:#{@e}>"
	end
	# Print node and its children, recursively
	def print_r prefix="", daughters=false
		puts "#{prefix}#{@e}"
		if @children
			(daughters ? @daughters : @children).each do |child|
				child.print_r "#{prefix}\t", daughters
			end
		end
	end
	# Reodrder daughters list, depending on input comparator
	def reorder best=nil
		if !best
			best = lambda {|d1, d2| d1.e < d2.e}
		end
		if @daughters.length != 0
			index = 0
			(0...@daughters.length).each do |i|
				if best.call @daughters[i], @daughters[index]
					index = i
				end
			end
			@daughters.unshift(@daughters.delete_at index)[0].reorder !best
		end
	end
	# Reset the daughters list
	def reset_daughters
		@daughters = Array.new @children
		@children.each do |child|
			child.reset_daughters
		end
	end
end
