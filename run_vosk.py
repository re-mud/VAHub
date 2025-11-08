from vosk import Model, KaldiRecognizer
import sounddevice as sd
from queue import Queue
import bootstrap
import logging
import json


bootstrap.setup_logging()
bootstrap.load_config()
logger = logging.getLogger(__name__)
vahub = bootstrap.create_vahub()

model_path = bootstrap.config["model"]
samplerate = bootstrap.config["samplerate"]

queue = Queue()
try:
	model = Model(model_path)
except:
	logger.critical("failed to create a model: check path 'model' in config.json")
	exit(1)
recognizer = KaldiRecognizer(model, samplerate)


def callback(indata, frames, time, status):
	queue.put(bytes(indata))

try:
	with sd.RawInputStream(
			samplerate=samplerate, 
			blocksize=samplerate // 2, 
			dtype='int16',
			channels=1, 
			callback=callback):
		while True:
			data = queue.get()
			if recognizer.AcceptWaveform(data):
				text = json.loads(recognizer.Result())["text"]
				vahub.handle(text)
except KeyboardInterrupt:
	pass
except Exception as e:
	logger.exception(e)
