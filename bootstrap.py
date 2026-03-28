from vahub.options import OptionsRegistry, OptionsFileProvider
from vahub.plugins import VAManifestManager, PluginManager
from vahub.task import CancellationToken
from vahub.vacontext import VAContext
from vahub.search import Solver
from vahub.core import VAHub
from pathlib import Path
from typing import Any
from json import loads
import logging


config: dict[str, Any] = {
	"speaker": None,
	"numbers_normalizer": None,
	"model": None,
	"samplerate": 16000,
	"fuzzy_solver": None,
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

def create_cancellation_token() -> CancellationToken:
	return CancellationToken()

def create_vahub(cancellation_token: CancellationToken) -> VAHub:
	plugin_manager = PluginManager("plugins/")
	plugin_manager.load()
	manifests = plugin_manager.emit_all("get_manifest")
	manifest_manager = VAManifestManager(manifests)

	options_provider = OptionsFileProvider("options/")
	default_options = manifest_manager.get_default_options()
	options_registry = OptionsRegistry(options_provider.get, default_options)

	numbers_normalizers = manifest_manager.get_numbers_normalizers()
	fuzzy_solvers = manifest_manager.get_fuzzy_solvers()
	commands = manifest_manager.get_commands()
	speakers = manifest_manager.get_speakers()

	speaker = speakers.get(config["speaker"], None)
	if speaker is None and config["speaker"] != None:
		logger.warning(f"speaker '{config["speaker"]}' not found")
		speaker = lambda t: print(f"[speaker]: {t}")

	numbers_normalizer = numbers_normalizers.get(config["numbers_normalizer"], None)
	if numbers_normalizer is None and config["numbers_normalizer"] != None:
		logger.warning(f"normalizer numbers '{config["numbers_normalizer"]}' not found")
		numbers_normalizer = lambda t: t

	fuzzy_solver = fuzzy_solvers.get(config["fuzzy_solver"], None)
	if fuzzy_solver is None and config["fuzzy_solver"] != None:
		logger.warning(f"normalizer numbers '{config["fuzzy_solver"]}' not found")
	
	context = VAContext(speaker, numbers_normalizer, options_registry.get, cancellation_token)
	searcher = Solver(fuzzy_solver)
	searcher.add_all(commands)

	return VAHub(context, searcher.search)
