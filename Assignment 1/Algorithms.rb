# Algorithms.rb
# ---
# Author:
# Balázs Pete
# 09771417

INFINITY = 1.0/0

def negamax_alpha_beta node, height, achievable=-INFINITY, hope=INFINITY, count=0
	neg = lambda {|arr| [-arr[0], arr[1], arr[2]]}
	_return = lambda {|r, pv| [r, pv, count +1]}

	if height == 0 or node.daughters.length == 0
		return _return.call node.e, []
	else
		pv = []
		node.daughters.each do |daughter|
			temp, _pv, count =
			neg.call negamax_alpha_beta daughter, height-1, -hope, -achievable, count
			if temp >= hope
				return _return.call temp, _pv
			end
			achievable = [temp, achievable].max
			if achievable == temp
				pv = _pv.unshift daughter
			end
		end
		return _return.call achievable, pv
	end
end

def iterative_negamax_alpha_beta node,height,achievable=-INFINITY,hope=INFINITY,count=0
	pv = []
	(0..height).each do |h|
		achievable, pv, count = negamax_alpha_beta node, height, achievable, hope, count
	end
	[achievable, pv, count]
end

def pvs node, depth, alpha=-INFINITY, beta=INFINITY, count=0
	neg = lambda {|arr| [-arr[0], arr[1], arr[2]]}
	_return = lambda {|r, pv| [r, pv, count +1]}

	if depth == 0 or node.children.length == 0
		return _return.call node.e, []
	else
		score, pv, count = neg.call pvs node.daughters[0], depth-1, -beta, -alpha, count
		pv= pv.unshift node.daughters[0]
		if score < beta
			node.daughters[1..-1].each do |daughter|
				lb = [alpha, score].max
				ub, _pv = lb+1, []
				temp, _pv, count = neg.call pvs daughter, depth-1, -ub, -lb, count

				if temp >= ub and temp < beta
					temp, _pv, count=neg.call pvs daughter, depth-1, -beta, -temp, count
				end
				score = [score, temp].max
				if temp >= beta
					pv = _pv.unshift daughter
					break
				end
			end
		end
		return _return.call score, pv
	end
end

def iterative_pvs node, depth, alpha=-INFINITY, beta=INFINITY, count=0
	score, pv = 0, []
	(0..depth).each do |h|
		score, pv, count = pvs node, depth, alpha, beta, count
	end
	[score, pv, count]
end




