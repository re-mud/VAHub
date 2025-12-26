from vahub.contracts import (
	OptionsProvider,
	Handler,
	Normalizer,
	Payload,
	Speaker,
)
import logging


logger = logging.getLogger(__name__)


class VAContext:
	def __init__(self, 
			speaker: Speaker,
			normalize_numbers: Normalizer,
			options_provider: OptionsProvider):
		self._speaker = speaker
		self._normalize_numbers = normalize_numbers
		self._options_provider = options_provider
		self._context: Payload = None

	def say(self, text: str) -> None:
		try:
			self._speaker(text)
		except:
			logger.exception("speak failed")

	def get_options(self, name: str) -> dict:
		return self._options_provider(name)

	def normalize_numbers(self, text: str) -> int | None:
		try:
			return self._normalize_numbers(text)
		except:
			logger.exception("normalize failed")

	def set_context_handler(self, context: Handler) -> None:
		self._context = context
	
	def pop_context(self) -> Payload:
		context = self._context
		self._context = None
		return context
