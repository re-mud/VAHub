import bootstrap
import logging


bootstrap.setup_logging()
bootstrap.load_config()
logger = logging.getLogger(__name__)
cancellation_token = bootstrap.create_cancellation_token()
vahub = bootstrap.create_vahub(cancellation_token)

try:
	while True:
		text = input(">>> ")
		vahub.handle(text)
		if cancellation_token.is_cancelled:
			break
except KeyboardInterrupt:
	pass
except Exception as e:
	logger.exception(e)
