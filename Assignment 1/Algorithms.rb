# Algorithms.rb
# ---
# Author:
# Balázs Pete
# 09771417

INFINITY = 1.0/0

def negamax_alpha_beta node, height, reorder=false, a=-INFINITY, h=INFINITY, count=0
	neg = lambda {|arr| [-arr[0], arr[1], arr[2]]}
	_return = lambda {|r, pv| [r, pv, count +1]}
	if reorder
		node.reorder
	end
	if height == 0 or node.daughters.length == 0
		return _return.call node.e, []
	else
		pv = []
		node.daughters.each do |daughter|
			temp, _pv, count =
			neg.call negamax_alpha_beta daughter, height-1, reorder, -h, -a, count
			if temp >= h
				return _return.call temp, _pv
			end
			a = [temp, a].max
			if a == temp
				pv = _pv.unshift daughter
			end
		end
		return _return.call a, pv
	end
end

def iterative_negamax_alpha_beta node,height,reorder=false,a=-INFINITY,h=INFINITY,count=0
	pv = []
	(0..height).each do |h|
		a, pv, count = negamax_alpha_beta node, height, reorder, a, h, count
	end
	[a, pv, count]
end

def pvs node, depth, reorder=false, alpha=-INFINITY, beta=INFINITY, count=0
	neg = lambda {|arr| [-arr[0], arr[1], arr[2]]}
	_return = lambda {|r, pv| [r, pv, count +1]}
	if reorder
		node.reorder
	end
	if depth == 0 or node.children.length == 0
		return _return.call node.e, []
	else
		score, pv, count = neg.call pvs node.daughters[0], depth-1, reorder, -beta, -alpha, count
		pv= pv.unshift node.daughters[0]
		if score < beta
			node.daughters[1..-1].each do |daughter|
				lb = [alpha, score].max
				ub, _pv = lb+1, []
				temp, _pv, count = neg.call pvs daughter, depth-1, reorder, -ub, -lb, count

				if temp >= ub and temp < beta
					temp, _pv, count=neg.call pvs daughter, depth-1, reorder, -beta, -temp, count
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

def iterative_pvs node, depth, reorder=false, alpha=-INFINITY, beta=INFINITY, count=0
	score, pv = 0, []
	(0..depth).each do |h|
		score, pv, count = pvs node, depth, reorder, alpha, beta, count
	end
	[score, pv, count]
end




