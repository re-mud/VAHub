from typing import Any, Callable, Type, TypeVar
from vahub.contracts import Normalizer, Speaker
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
	
	def get_fields_dict(self, 
			field: Any, 
			type_key: Type[T] = object, 
			type_value: Type[T] = object) -> dict[str, Callable]:
		fields = self.get_fields(field, dict)
		filtered_fields = {}
		for i in fields.values():
			for k, v in i.items():
				if isinstance(k, type_key) and isinstance(v, type_value):
					filtered_fields[k] = v
		return filtered_fields
	
	def get_numbers_normalizers(self) -> dict[str, Normalizer]:
		return self.get_fields_dict("numbers_normalizers", str, Callable)
	
	def get_speakers(self) -> dict[str, Speaker]:
		return self.get_fields_dict("speakers", str, Callable)

	def get_default_options(self) -> dict[str, dict]:
		return self.get_fields("default_options", dict)
	
	def get_commands(self) -> dict[str, Callable]:
		return self.get_fields_dict("commands", str, Callable)
