from pycaw.pycaw import AudioUtilities
from vahub.contracts import Context


def get_manifest() -> dict:
	return {
		"name": "Volume control",
		"version": "1.1",

		"default_options": {
			"volume_step": 10,
			"min_volume": 10,
			"max_volume": 100,
		},

		"commands": {
			"громкость|звук": set_volume,
			"выключи звук|убери звук" :  mute,
			"включи звук|верни звук" :  unmute,
			"уменьши громкость|понизь громкость|тише" : volume_down,
			"увеличь громкость|повысь громкость|громче" : volume_up,
			"полная громкость|звук на максимум|максимальная громкость" : volume_max,
			"звук на минимум|минимальная громкость|минимум звука" : volume_min,
		}
	}

def _get_ev():
	return AudioUtilities.GetSpeakers().EndpointVolume

def _set_volume(volume: int) -> None:
	return _get_ev().SetMasterVolumeLevelScalar(volume, None)

def _get_volume() -> None:
	return _get_ev().GetMasterVolumeLevelScalar()

def volume_min(context: Context, text: str) -> None:
	_set_volume(context.get_options(__name__)["min_volume"] / 100)

def volume_max(context: Context, text: str) -> None:
	_set_volume(context.get_options(__name__)["max_volume"] / 100)

def mute(context: Context, text: str) -> None:
	_get_ev().SetMute(1, None)

def unmute(context: Context, text: str) -> None:
	_get_ev().SetMute(0, None)

def set_volume(context: Context, text: str) -> None:
	volume = get_volume_from_text(context, text)
	if volume is None:
		context.say("неверный параметр громкости")
	_set_volume(volume)

def volume_up(context: Context, text: str) -> None:
	volume = get_volume_from_text(context, text)
	if volume is None:
		volume = context.get_options(__name__)["volume_step"] / 100
	
	volume = min(100, _get_volume() + volume)
	_set_volume(volume)

def volume_down(context: Context, text: str) -> None:
	volume = get_volume_from_text(context, text)
	if volume is None:
		volume = context.get_options(__name__)["volume_step"] / 100
	
	volume = max(0, _get_volume() - volume)
	_set_volume(volume)

def get_volume_from_text(context: Context, text: str) -> int | None:
	volume = context.normalize_numbers(text)
	if volume is None:
		return
	return max(0, min(volume, 100)) / 100
