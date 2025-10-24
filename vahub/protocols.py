from vahub.models import SearchResult
from typing import Protocol, TypeVar


T = TypeVar('T')


class SearcherProtocol(Protocol[T]):
	def add(self, text: str, value: T) -> None: ...
	def search(self, text: str) -> SearchResult[T]: ...