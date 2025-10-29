from .models import SearchResult
from typing import Protocol, TypeVar


T = TypeVar('T')


class Searcher(Protocol[T]):
	def __call__(self, text: str) -> SearchResult[T]: ...

class OptionsProvider(Protocol):
	def __call__(self, name: str) -> dict: ...
