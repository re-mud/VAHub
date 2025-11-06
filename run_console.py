import bootstrap
import logging


bootstrap.setup_logging()
bootstrap.load_config()
logger = logging.getLogger(__name__)
vahub = bootstrap.create_vahub()

try:
	while True:
		text = input(">>> ")
		vahub.handle(text)
except KeyboardInterrupt:
	pass
except Exception as e:
	logger.exception(e)
