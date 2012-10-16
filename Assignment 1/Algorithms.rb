INFINITY = 1.0/0

def negamax_alpha_beta node, height, achievable=-INFINITY, hope=INFINITY, count=0
	neg = lambda {|arr| [-arr[0], arr[1], arr[2]]}
	_return = lambda {|r, pv| [r, pv, count +1]}

	if height == 0 or node.daughters.length == 0
		count += 1
		return _return.call node.e, []
	else
		pv = []
		node.daughters.each do |daughter|
			temp, _pv, count = neg.call negamax_alpha_beta daughter, height-1, -hope, -achievable, count
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

def iterative_negamax_alpha_beta node, height, achievable=-INFINITY, hope=INFINITY, count=0
	pv = []
	(0..height).each do |h|
		achievable, pv, count = negamax_alpha_beta node, height, achievable=-INFINITY, hope=INFINITY, count
	end
	[achievable, pv, count]
end