import threading


class CancellationToken:
	def __init__(self):
		self._is_cancelled = False
		self._lock = threading.Lock()

	@property
	def is_cancelled(self):
		with self._lock:
			return self._is_cancelled
		
	def cancel(self):
		with self._lock:
			self._is_cancelled = True
