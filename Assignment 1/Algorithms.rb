INFINITY = 1.0/0

# αβ (node Node, int Ht, real Achievable, real Hope) 
#  if Ht=0 or no moves exist
#   then Return Evaluation (Node)
#  else {real Temp;
#        for each move M at node Node,
#         {generate NewNode from Node & M;
# Temp:= - αβ(Newnode, Ht-1, -Hope, -Achievable); Destroy NewNode;
# If Temp>=Hope then Return Temp; Achievable:=Max(Temp, Achievable) }}
# ￼Return Achievable }


def negamax_alpha_beta node, height, achievable=-INFINITY, hope=INFINITY, count=0
	_return = lambda {|r| [r, count +1]}

	if height == 0 or node.daughters.length == 0
		count += 1
		return _return.call node.e
	else
		node.daughters.each do |daughter|
			temp, count = negamax_alpha_beta daughter, height-1, -hope, -achievable, count
			if temp >= hope
				return _return.call temp
			end
			achievable = [temp, achievable].max
		end
		return _return.call achievable
	end
end
