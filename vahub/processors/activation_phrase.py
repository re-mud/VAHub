import time

class ActivationPhrase:
	def __init__(self, phrase: str, activity_sec: int = 15):
		self._phrase = phrase
		self._activity_sec = activity_sec
		self._activity_expired = 0

	def preprocessing(self, text: str) -> str:
		if self._phrase in text:
			startid = text.index(self._phrase) + len(self._phrase)
			self._activity_expired = time.time() + self._activity_sec
			return text[startid:].strip()
		elif self._activity_expired > time.time():
			return text
		else: 
			return ""
