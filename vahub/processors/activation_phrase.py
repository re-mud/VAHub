import time

class ActivationPhrase:
	def __init__(self, phrases: list, activity_sec: int = 15):
		self._phrases = sorted(phrases)
		self._activity_sec = activity_sec
		self._activity_expired = 0

	def preprocessing(self, text: str) -> str:
		for phrase in self._phrases:
			if phrase in text:
				startid = text.index(phrase) + len(phrase)
				self._activity_expired = time.time() + self._activity_sec
				return text[startid:].strip()
		if self._activity_expired > time.time():
			return text
		return ""
