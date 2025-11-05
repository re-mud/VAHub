from typing import (
	Protocol,
	TypeAlias,
	TypeVar,
	runtime_checkable,
)
from .models import SearchResult
from .enums import AppCommand


T = TypeVar('T')


class Searcher(Protocol[T]):
	def __call__(self, text: str) -> SearchResult[T]: ...

class OptionsProvider(Protocol):
	def __call__(self, name: str) -> dict: ...

@runtime_checkable
class Handler(Protocol):
	def __call__(self, ctx: "Context", remaining_text: str) -> AppCommand: ...

@runtime_checkable
class Speaker(Protocol):
	def __call__(self, text: str) -> None: ...

@runtime_checkable
class Normalizer(Protocol):
	def __call__(self, text: str) -> str: ...

class Context(Protocol):
	def say(self, text: str) -> None: ...
	def set_context_handler(self, context: Handler) -> None: ...
	def get_context(self) -> "Payload": ...
	def clear_context(self) -> "Payload": ...
	def normalize_numbers(self, text: str) -> str: ...
	def get_options(self, name: str) -> dict: ...


Payload: TypeAlias = Handler | None
