from dataclasses import dataclass, field
from typing import TypeVar


T = TypeVar('T')


class WordTrie[T]:
	def __init__(self):
		self._root: WordTrieNode = WordTrieNode()

	def add(self, text: str, value: T) -> bool:
		words = text.split()
		node = self._root
		for word in words:
			if not word in node.children:
				node.children[word] = WordTrieNode()
			node = node.children[word]
		if node.end:
			return False
		
		node.value = value
		node.end = True
		return True

	def start_with(self, text: str) -> tuple[T, str] | None:
		words = text.split()
		node = self._root
		last_match_value = None
		last_match_count = 0
		count = 0
		for word in words:
			if not word in node.children:
				if node.end:
					return (node.value, " ".join(words[count:]))
				if not last_match_value is None:
					return (last_match_value, " ".join(words[last_match_count:]))
				return None
			
			if node.end:
				last_match_value = node.value
				last_match_count = count
			
			node = node.children[word]
			count += 1

		if node.end:
			return (node.value, " ".join(words[count:]))
		if not last_match_value is None:
			return (last_match_value, " ".join(words[last_match_count:]))
		return None


@dataclass(slots=True)
class WordTrieNode[T]():
	children: dict[str, "WordTrieNode"] = field(default_factory=dict)
	value: T = None
	end: bool = False