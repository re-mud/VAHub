from json import dumps, loads
from pathlib import Path
from os import PathLike
from typing import Any


class OptionsFileProvider:
	def __init__(self, path: str | PathLike):
		self._path = Path(path)
		self._options: dict[str, Any] = {}

		self._path.mkdir(parents=True, exist_ok=True)

	def get(self, name: str) -> Any | None:
		if name in self._options:
			return self._options[name]
		data = self.load(name)
		self._options[name] = data
		return data
		
	def load(self, name: str) -> Any | None:
		path = self._path_from_name(name)
		if not path.exists():
			return None

		with open(path, "r", encoding="utf-8") as file:
			return loads(file.read())

	def save(self, name: str, data: Any) -> None:
		path = self._path_from_name(name)
		with open(path, "w", encoding="utf-8") as file:
			file.write(dumps(data))

	def delete(self, name: str) -> bool:
		if name in self._options:
			del self._options[name]
			
		path = self._path_from_name(name)
		if path.exists():
			path.unlink()
			return True
		return False

	def reset(self) -> None:
		self._options.clear()

	def _path_from_name(self, name: str) -> Path:
		return self._path / (name + ".json")
