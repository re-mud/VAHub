import bootstrap
import logging


bootstrap.setup_logging()
logger = logging.getLogger(__name__)
logger.info("initializing...")
bootstrap.load_config()
cancellation_token = bootstrap.create_cancellation_token()
vahub = bootstrap.create_vahub(cancellation_token)

try:
	logger.info(f"application started")
	while True:
		text = input(">>> ")
		vahub.handle(text)
		if cancellation_token.is_cancelled:
			break
except KeyboardInterrupt:
	pass
except Exception as e:
	logger.exception(e)
