from typing import TypeVar
from dataclasses import dataclass


T = TypeVar('T')


@dataclass(slots=True, frozen=True)
class SearchResult[T]:
	remaining_text: str = ""
	similarity: float = 0.0
	value: T | None = None
