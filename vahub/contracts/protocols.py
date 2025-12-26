from typing import (
	TypeAlias,
	Callable,
	Protocol,
	TypeVar,
	Any,
)
from .models import SearchResult


T = TypeVar('T')


class Context(Protocol):
	def say(self, text: str) -> None: ...
	def get_options(self, name: str) -> dict: ...
	def normalize_numbers(self, text: str) -> int | None: ...
	def set_context_handler(self, context: "Handler") -> None: ...
	def pop_context(self) -> "Payload": ...


OptionsProvider: TypeAlias = Callable[[str], dict]
Normalizer: TypeAlias = Callable[[str], Any | None]
Searcher: TypeAlias = Callable[[str], SearchResult[T]]
Handler: TypeAlias = Callable[[Context, str], None]
Speaker: TypeAlias = Callable[[str], None]
Payload: TypeAlias = "Handler | None"
