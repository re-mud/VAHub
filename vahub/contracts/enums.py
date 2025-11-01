from enum import StrEnum
from typing import Any


class AppCommand(StrEnum):
	NONE = "none"
	CLOSE = "close"

	@classmethod
	def from_value(cls, v: Any):
		if v is None:
			return cls.NONE
		if isinstance(v, str):
			return cls(v)
