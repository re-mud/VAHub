from vahub.contracts import (
	Context,
	Handler,
	Searcher,
	SearchResult,
)
import logging


logger = logging.getLogger(__name__)


class VAHub:
	def __init__(self, 
			context: Context, 
			searcher: Searcher):
		self._context = context
		self._searcher = searcher

	def handle(self, text: str) -> None:
		payload = self._context.pop_context()
		if payload is None:
			result: SearchResult[Handler] = self._searcher(text)
			handler: Handler | None = result.value
			if handler != None:
				self._execute(handler, result.remaining_text)
		elif isinstance(payload, Handler):
			self._execute(payload, text)
		else:
			logger.error("unknown context type")
	
	def _execute(self, handler: Handler, text: str) -> None:
		try:
			handler(self._context, text)
		except:
			name = getattr(handler, "__name__", "unknown")
			module = getattr(handler, "__module__", "unknown")
			logger.exception(f"{name} from {module} failed")
