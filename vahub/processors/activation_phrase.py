from vahub.search import levenshtein
import time


class ActivationPhrase:
	def __init__(self, words: list, duration: int = 15, similarity_threshold: float = 0.4):
		self._similarity_threshold = similarity_threshold
		self._words = sorted(words)
		self._duration = duration
		self._expired = 0

	def preprocessing(self, text: str) -> str:
		if text == "":
			return
		
		for phrase in self._words:
			if phrase in text:
				print(phrase)
				startid = text.index(phrase) + len(phrase)
				self._expired = time.time() + self._duration
				return text[startid:].strip()
		
		word = text.split()[0]
		for phrase in self._words:
			if levenshtein.similarity(phrase, word) > self._similarity_threshold:
				self._expired = time.time() + self._duration
				return text[len(word):].strip()

		if self._expired > time.time():
			return text
		
		return ""
