from typing import Any, Callable, Type, TypeVar
from copy import deepcopy


T = TypeVar("T")


class VAManifestManager:
	def __init__(self, manifests: dict[str, dict]):
		self._manifests = manifests

	def get_fields(self, field: Any, type_field: Type[T] = object) -> dict[str, T]:
		fields: dict[str, T] = {}
		for key in self._manifests:
			value = self._manifests[key].get(field, None)
			if value is not None and isinstance(value, type_field):
				fields[key] = deepcopy(value)
		return fields

	def get_default_options(self) -> dict[str, dict]:
		return self.get_fields("default_options", dict)
	
	def get_commands(self) -> dict[str, Callable]:
		fields = self.get_fields("commands", dict)
		commands: dict[str, Callable] = {}
		for c in fields.values():
			commands |= c
		return commands
