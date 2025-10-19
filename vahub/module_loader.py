from importlib.util import spec_from_file_location, module_from_spec
from types import ModuleType
from pathlib import Path
from os import PathLike
import sys


def load(path: str | PathLike) -> ModuleType:
	p = Path(path)
	name = p.stem
	spec = spec_from_file_location(name, path)
	module = module_from_spec(spec)

	if not spec or not spec.loader:
		raise ImportError()

	sys.modules[name] = module
	try:
		spec.loader.exec_module(module)
		return module
	except:
		del sys.modules[name]
		raise
	
def load_all(path: str | PathLike, prefix: str | None = None) -> list[ModuleType]:
	p = Path(path)

	if not p.is_dir():
		raise NotADirectoryError()
	
	modules = []
	
	for child in p.iterdir():
		if not child.is_file():
			continue
		if child.suffix != ".py":
			continue
		if prefix and not child.stem.startswith(prefix):
			continue
		
		try:
			module = load(child)
			modules.append(module)
		except:
			pass

	return modules