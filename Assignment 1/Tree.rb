require_relative 'Algorithms.rb'

class Node
	attr_reader :e, :children
	# Daughters can be used to reorder child nodes
	attr_accessor :daughters, :pv

	def initialize b, d, approx, _t=nil
		random = lambda {|min, max| rand((max - min).abs)+min }
		new_node = lambda {|__t| Node.new b,d-1,approx,__t}
		t = (_t or random.call(-250, 250))
		
		@pv = []
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
	def to_s
		"<Node:#{@e}>"
	end
	def print_r prefix=""
		puts "#{prefix}#{@e}"
		if @children 
			@children.each do |child|
				child.print_r "#{prefix}\t"
			end
		end
	end
end



n = Node.new 2, 7, 15
n.print_r
puts negamax_alpha_beta n, 2