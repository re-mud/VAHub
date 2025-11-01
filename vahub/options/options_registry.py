from vahub.contracts import OptionsProvider
from copy import deepcopy


class OptionsRegistry:
	def __init__(self, 
			options_provider: OptionsProvider,
			default_options: dict[str, dict] | None = None):
		self._options_provider = options_provider
		self._default_options = default_options or {}

	def get(self, name) -> dict | None:
		options = self._options_provider(name)
		if options and not isinstance(options, dict):
			raise TypeError()
		
		default = self._default_options.get(name)
		if default is None:
			return options
		base = deepcopy(default)
		if options:
			base |= options
		return base
	
	def add(self, name: str, value: dict) -> None:
		self._default_options[name] = value
