from typing import Protocol, TypeAlias, runtime_checkable
from vahub.contracts import OptionsProvider, AppCommand
import logging


logger = logging.getLogger(__name__)


@runtime_checkable
class Handler(Protocol):
	def __call__(self, ctx: "VAContext") -> AppCommand: ...

class Speaker(Protocol):
	def __call__(self, text: str) -> None: ...

class Normalizer(Protocol):
	def __call__(self, text: str) -> str: ...

Payload: TypeAlias = Handler | None


class VAContext:
	def __init__(self, 
			speaker: Speaker,
			normalizer: Normalizer,
			options_provider: OptionsProvider):
		self._speaker = speaker
		self._normalizer = normalizer
		self._options_provider = options_provider
		self._context: Payload = None

	def say(self, text: str) -> None:
		try:
			self._speaker(text)
		except:
			logger.exception("speak failed")

	def set_context_handler(self, context: Handler) -> None:
		self._context = context

	def get_context(self) -> Payload:
		return self._context
	
	def clear_context(self) -> Payload:
		context = self._context
		self._context = None
		return context

	def normalize(self, text: str) -> str:
		try:
			return self._normalizer(text)
		except:
			logger.exception("normalize failed")
			return text

	def get_options(self, name: str) -> dict:
		return self._options_provider(name)
