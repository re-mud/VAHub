from vahub.contracts.protocols import FuzzySolver
from vahub.contracts import SearchResult
from .word_trie import WordTrie
from typing import TypeVar


T = TypeVar('T')


class Solver[T]:
	def __init__(self, fuzzy_solver: FuzzySolver | None = None):
		self._data: dict[str, T] = {}
		self._trie = WordTrie[T]()
		self._fuzzy = fuzzy_solver

	def add(self, text: str, value: T) -> None:
		self._data[text] = value
		self._trie.add(text, value)
	
	def add_all(self, kvps: dict[str, T]) -> None:
		for k, v in kvps.items():
			self.add(k, v)

	def search(self, text: str) -> SearchResult[T]:
		if text in self._data:
			return SearchResult(similarity=1.0, value=self._data[text])
		
		trie_result = self._trie.start_with(text)
		if trie_result:
			value, remaining_text = trie_result
			return SearchResult(remaining_text, 1.0, value)
		
		if self._fuzzy != None:
			return self._fuzzy(text, self._data)
		
		return SearchResult()
