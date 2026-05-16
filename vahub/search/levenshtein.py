def distance(s1, s2):
	n, m = len(s1), len(s2)
	if n < m: return distance(s2, s1)
	if m == 0: return n
	previous_row = range(m + 1)
	for i, c1 in enumerate(s1):
		current_row = [i + 1]
		for j, c2 in enumerate(s2):
			insertions = previous_row[j + 1] + 1
			deletions = current_row[j] + 1
			substitutions = previous_row[j] + (c1 != c2)
			current_row.append(min(insertions, deletions, substitutions))
		previous_row = current_row
	return previous_row[-1]

def similarity(s1, s2):
	return 1 - distance(s1, s2) / max(len(s1), len(s2))
