from vahub.contracts import (
	Context,
	Handler,
	Searcher,
	SearchResult,
	Preprocessor
)
import logging


logger = logging.getLogger(__name__)


class VAHub:
	def __init__(self, 
			context: Context, 
			searcher: Searcher,
			preprocessor: Preprocessor,
			min_similarity: float = 0.6):
		self._context = context
		self._searcher = searcher
		self._preprocessor = preprocessor
		self._min_similarity = min_similarity

	def handle(self, text: str) -> None:
		payload = self._context.pop_context()
		if payload is None:
			text = self._preprocessor(text)
			if text:
				result: SearchResult[Handler] = self._searcher(text)
				logger.debug(f"input: '{text}' similarity: '{result.similarity}' handler: '{getattr(result.value, "__name__", None)}'")
				if result.similarity > self._min_similarity:
					self._execute(result.value, result.remaining_text)
		elif isinstance(payload, Handler):
			self._execute(payload, text)
		else:
			logger.error("unknown context type")
		self._context.update_queue()
	
	def _execute(self, handler: Handler, text: str) -> None:
		try:
			handler(self._context, text)
		except:
			name = getattr(handler, "__name__", "unknown")
			module = getattr(handler, "__module__", "unknown")
			logger.exception(f"{name} from {module} failed")
