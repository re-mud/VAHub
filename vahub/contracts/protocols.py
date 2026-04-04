from typing import (
	runtime_checkable,
	TypeAlias,
	Callable,
	Protocol,
	TypeVar,
	Any,
)
from .models import SearchResult


T = TypeVar('T')


@runtime_checkable
class Context(Protocol):
	def say(self, text: str) -> None: ...
	def update_queue(self) -> None: ...
	def get_options(self, name: str) -> dict: ...
	def normalize_numbers(self, text: str) -> int | None: ...
	def set_context_handler(self, context: "Handler") -> None: ...
	def pop_context(self) -> "Payload": ...
	@property
	def is_cancelled(self) -> bool: ...
	def cancel(self) -> None: ...

@runtime_checkable
class OptionsProvider(Protocol):
	def __call__(self, name: str) -> dict: ...

@runtime_checkable
class FuzzySolver(Protocol):
	def __call__(self, text: str, data: dict[str, T]) -> SearchResult: ...

@runtime_checkable
class Normalizer(Protocol):
	def __call__(self, text: str) -> Any | None: ...

@runtime_checkable
class Searcher(Protocol):
	def __call__(self, text: str) -> SearchResult[T]: ...

@runtime_checkable
class Handler(Protocol):
	def __call__(self, context: Context, text: str) -> None: ...

@runtime_checkable
class Speaker(Protocol):
	def __call__(self, text: str) -> None: ...

Payload: TypeAlias = Handler | None
