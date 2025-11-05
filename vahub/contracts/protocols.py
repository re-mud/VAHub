from typing import (
	Protocol,
	TypeAlias,
	TypeVar,
	runtime_checkable,
)
from vahub.vacontext import VAContext
from .models import SearchResult
from .enums import AppCommand


T = TypeVar('T')


class Searcher(Protocol[T]):
	def __call__(self, text: str) -> SearchResult[T]: ...

class OptionsProvider(Protocol):
	def __call__(self, name: str) -> dict: ...

@runtime_checkable
class Handler(Protocol):
	def __call__(self, ctx: "VAContext", remaining_text: str) -> AppCommand: ...

@runtime_checkable
class Speaker(Protocol):
	def __call__(self, text: str) -> None: ...

@runtime_checkable
class Normalizer(Protocol):
	def __call__(self, text: str) -> str: ...


Payload: TypeAlias = Handler | None
