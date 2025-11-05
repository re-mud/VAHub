from vahub.contracts import (
	Context,
	Handler,
	Searcher,
	AppCommand,
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

	def handle(self, text: str) -> AppCommand:
		payload = self._context.clear_context()

		if payload is None:
			result: SearchResult[Handler] = self._searcher(text)
			handler: Handler | None = result.value
			if handler != None:
				return self._execute(handler, result.remaining_text)
			return AppCommand.NONE
		
		if isinstance(payload, Handler):
			return self._execute(payload, text)

		logger.error("unknown context type")
		return AppCommand.NONE
	
	def _execute(self, handler: Handler, text: str) -> AppCommand:
		try:
			command = handler(self._context, text)
			return AppCommand.from_value(command)
		except:
			name = getattr(handler, "__name__", "unknown")
			module = getattr(handler, "__module__", "unknown")
			logger.exception(f"{name} from {module} failed")
			return AppCommand.NONE
