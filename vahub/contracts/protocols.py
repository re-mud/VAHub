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
	def set_context_handler(self, context: "Handler") -> None: ...
	def get_context(self) -> "Payload": ...
	def clear_context(self) -> "Payload": ...
	def normalize_numbers(self, text: str) -> str: ...
	def get_options(self, name: str) -> dict: ...


OptionsProvider: TypeAlias = Callable[[str], dict]
Normalizer: TypeAlias = Callable[[str], Any | None]
Searcher: TypeAlias = Callable[[str], SearchResult[T]]
Handler: TypeAlias = Callable[[Context, str], None]
Speaker: TypeAlias = Callable[[str], None]
Payload: TypeAlias = "Handler | None"
