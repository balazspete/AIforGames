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
