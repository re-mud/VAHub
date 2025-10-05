from typing import Generic, Optional, TypeVar
from dataclasses import dataclass


T = TypeVar('T')


class Resolver(Generic[T]):
	def __init__(self):
		self._data: dict[str, T] = {}

	def add(self, text: str, value: T) -> None:
		self._data[text] = value

	def search(self, text: str) -> "SearchResult"[T]:
		if text in self._data:
			return SearchResult(similarity=1.0, value=self._data[text])
		
		return SearchResult()
		

@dataclass(slots=True, frozen=True)
class SearchResult(Generic[T]):
	remaining_text: str = ""
	similarity: Optional[float] = None
	value: Optional[T] = None
