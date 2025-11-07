from pyttsx3 import speak


def get_manifest() -> dict:
	return {
		"name": "pyttsx3 TTS",
		"version": "1.0",

		"speakers": {
			"pyttsx3": speak,
		}
	}
