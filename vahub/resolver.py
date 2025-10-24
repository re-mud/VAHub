from typing import TypeVar
from dataclasses import dataclass
from word_trie import WordTrie


T = TypeVar('T')


@dataclass(slots=True, frozen=True)
class SearchResult[T]:
	remaining_text: str = ""
	similarity: float | None = None
	value: T | None = None


class Resolver[T]:
	def __init__(self):
		self._data: dict[str, T] = {}
		self._trie = WordTrie[T]()

	def add(self, text: str, value: T) -> None:
		self._data[text] = value
		self._trie.add(text, value)

	def search(self, text: str) -> SearchResult[T]:
		if text in self._data:
			return SearchResult(similarity=1.0, value=self._data[text])
		
		trie_result = self._trie.start_with(text)
		if trie_result:
			value, remaining_text = trie_result
			return SearchResult(remaining_text, 1.0, value)
		
		return SearchResult()
