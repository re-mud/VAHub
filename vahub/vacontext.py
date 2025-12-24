from vahub.contracts import (
	OptionsProvider,
	Handler,
	Normalizer,
	Payload,
	Speaker,
)
from datetime import datetime, timedelta
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
		self._timers: list[tuple[Handler, datetime]] = []

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

	def set_timer(self, context: Handler, duration: timedelta) -> None:
		time = datetime.now() + duration
		self.set_timer_at(context, time)
	
	def set_timer_at(self, context: Handler, time: datetime) -> None:
		for i in range(len(self._timers)):
			if time < self._timers[i][1]:
				self._timers.insert(i, (context, time))
				return
		self._timers.append((context, time))
	
	def pop_timer(self) -> Handler | None:
		if len(self._timers) > 0 and self._timers[0][1] <= datetime.now():
			return self._timers.pop(0)[0]

	def set_context_handler(self, context: Handler) -> None:
		self._context = context
	
	def pop_context(self) -> Payload:
		context = self._context
		self._context = None
		return context
