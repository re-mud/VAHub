from vahub.options import OptionsRegistry, OptionsFileProvider
from vahub.plugins import VAManifestManager, PluginManager
from vahub.vacontext import VAContext
from vahub.search import Resolver
from vahub.core import VAHub
from pathlib import Path
from typing import Any
from json import loads
import logging


config: dict[str, Any] = {
	"speaker": None,
	"numbers_normalizer": None,
	"model": None,
	"samplerate": 16000
}

logger = logging.getLogger(__name__)


def load_config() -> None:
	path = Path("config.json")
	if not path.exists():
		return
	
	with open("config.json") as f:
		loaded_config = loads(f.read())
	config.update(loaded_config)

def setup_logging(level=logging.WARNING, logfile=None) -> None:
    log_format = "[%(asctime)s] [%(name)s/%(levelname)s]: %(message)s"

    handlers = [logging.StreamHandler()]
    if logfile:
        handlers.append(logging.FileHandler(logfile, encoding="utf-8"))

    logging.basicConfig(level=level, format=log_format, datefmt="%H:%M:%S", handlers=handlers)

def create_vahub() -> VAHub:
	plugin_manager = PluginManager("plugins/")
	plugin_manager.load()
	manifests = plugin_manager.emit_all("get_manifest")
	manifest_manager = VAManifestManager(manifests)

	options_provider = OptionsFileProvider("options/")
	default_options = manifest_manager.get_default_options()
	options_registry = OptionsRegistry(options_provider.get, default_options)

	numbers_normalizers = manifest_manager.get_numbers_normalizers()
	commands = manifest_manager.get_commands()
	speakers = manifest_manager.get_speakers()

	speaker = speakers.get(config["speaker"], None)
	if speaker is None:
		logger.warning(f"speaker '{config["speaker"]}' not found")
		speaker = lambda t: print(f"[speaker]: {t}")

	normalize_numbers = numbers_normalizers.get(config["numbers_normalizer"], None)
	if normalize_numbers is None:
		logger.warning(f"normalizer numbers '{config["numbers_normalizer"]}' not found")
		normalize_numbers = lambda t: t
	
	context = VAContext(speaker, normalize_numbers, options_registry.get)
	searcher = Resolver()
	searcher.add_all(commands)

	return VAHub(context, searcher.search)
