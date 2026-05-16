from vahub.contracts import SearchResult
from vahub.search import levenshtein
from .word_trie import WordTrie
from typing import TypeVar


T = TypeVar('T')


class Solver[T]:
	def __init__(self):
		self._data: dict[str, T] = {}
		self._trie = WordTrie[T]()

	def add(self, text: str, value: T) -> None:
		self._data[text] = value
		self._trie.add(text, value)
	
	def add_all(self, kvps: dict[str, T]) -> None:
		for k, v in kvps.items():
			self.add(k, v)

	def search(self, text: str) -> SearchResult[T]:
		if text in self._data:
			return SearchResult("", 1.0, self._data[text])
		
		trie_result = self._trie.start_with(text)
		if trie_result:
			value, remaining_text = trie_result
			return SearchResult(remaining_text, 1.0, value)
		
		best = ("", 0) # key, similarity
		for k in self._data.keys():
			s = levenshtein.similarity(k, text)
			if best[1] < s:
				best = (k, s)
		
		return SearchResult("", best[1], self._data.get(best[0]))
