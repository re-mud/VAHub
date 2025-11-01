from types import ModuleType
from pathlib import Path
from os import PathLike
from typing import Any
from . import loader


class PluginManager:
	def __init__(self, path: str | PathLike, prefix = "plugin_"):
		self._path = Path(path)
		self._plugins: dict[str, ModuleType] = {}
		self._prefix = prefix

		self._path.mkdir(parents=True, exist_ok=True)

	def load(self) -> None:
		self._plugins = loader.load_all(self._path, self._prefix)

	def emit_all(self, method_name: str, *args: Any) -> dict[str, Any]:
		results = {}
		for key in self._plugins.keys():
			try:
				result = self.emit(key, method_name, *args)
				if result is not None:
					results[key] = result
			except:
				pass
		return results

	def emit(self, key: str, method_name: str, *args: Any) -> Any | None:
		plugin = self._plugins.get(key)
		if plugin:
			method = getattr(plugin, method_name, None)
			if callable(method):
				return method(*args)
	
	def keys(self) -> list[str]:
		return list(self._plugins.keys())
	
	def get_plugin(self, key: str) -> ModuleType | None:
		return self._plugins.get(key, None)
