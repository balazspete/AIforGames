# Negamax (node N, int Height)
#  if Height = 0 or no moves possible from Node
# then Return Evaluation (Node)
# /* From the perspective of the player to move! */
# else {real Temp, Score=-∞; for each move M at Node,
# {generate NewNode from Node & M; Temp:= ￼ Negamax (Newnode, Height-1); destroy NewNode;
# Score:= max (Score, Temp)}
#         Return Score}

INFINITY = -1.0/0

def Negamax n, height

	if height == 0 or n.daughters.length == 0
		return n.e
	else
		temp, score = INFINITY, INFINITY


		n.daughters.each do |daughter|
			temp = -(Negamax daughter, height-1)
			score = [score, temp].max
		end
		return score
	end

end
