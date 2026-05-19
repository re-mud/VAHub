from vahub.processors import ActivationPhrase, number_normalizer
from vahub.options import OptionsRegistry, OptionsFileProvider
from vahub.plugins import VAManifestManager, PluginManager
from vahub.task import CancellationToken
from vahub.vacontext import VAContext
from typing import Any, Callable
from vahub.search import Solver
from vahub.core import VAHub
from pathlib import Path
from json import loads
import logging


config: dict[str, Any] = {
	"speaker": None,
	"model": None,
	"samplerate": 16000,
	"wake_words": "",
	"context_duration": 15,
	"similarity_threshold": 0.6, 
	"wake_words_similarity_threshold": 0.4,
	"log_file": None,
	"log_level": "INFO",
	"normalize_numbers": True,
}

logger = logging.getLogger(__name__)


def load_config() -> None:
	path = Path("config.json")
	if not path.exists():
		return
	
	with open("config.json", encoding="utf-8") as f:
		loaded_config = loads(f.read())
	config.update(loaded_config)

def setup_logging() -> None:
	log_file = config["log_file"]
	log_level = config["log_level"]
	log_format = "[%(asctime)s] [%(name)s/%(levelname)s]: %(message)s"
	date_format = "%H:%M:%S"

	handlers = [logging.StreamHandler()]
	if log_file:
		handlers.append(logging.FileHandler(log_file, encoding="utf-8"))

	logging.basicConfig(
		level=log_level, 
		format=log_format, 
		datefmt=date_format, 
		handlers=handlers
	)

def create_cancellation_token() -> CancellationToken:
	return CancellationToken()

def create_manifest_manager() -> VAManifestManager:
	plugin_manager = PluginManager("plugins/")
	plugin_manager.load()
	manifests = plugin_manager.emit_all("get_manifest")
	return VAManifestManager(manifests)

def _get_func_from_config(name: str, array: dict[str, Any], default: Callable | None = None):
	func = array.get(config[name], None)
	if func is None and config[name] != None:
		logger.warning(f"{name} '{config[name]}' not found")
		func = default
	return func

def create_vahub(cancellation_token: CancellationToken) -> VAHub:
	manifest_manager = create_manifest_manager()

	options_provider = OptionsFileProvider("options/")
	default_options = manifest_manager.get_default_options()
	options_registry = OptionsRegistry(options_provider.get, default_options)

	commands = manifest_manager.get_commands()
	speakers = manifest_manager.get_speakers()

	speaker = _get_func_from_config("speaker", speakers, lambda t: print(f"[speaker]: {t}"))

	wake_words = config["wake_words"].split("|")
	context_duration = config["context_duration"]
	words_sim_threshold = config["wake_words_similarity_threshold"]
	preprocessor = ActivationPhrase(wake_words, context_duration, words_sim_threshold)

	normalizer = number_normalizer.text_to_number if config["normalize_numbers"] else None
	context = VAContext(speaker, options_registry.get, cancellation_token)
	searcher = Solver()
	searcher.add_all(commands)

	return VAHub(context, searcher.search, preprocessor.preprocessing, normalizer, config["similarity_threshold"])
