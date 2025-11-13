from pycaw.pycaw import AudioUtilities
from vahub.contracts import Context


device = AudioUtilities.GetSpeakers()


def get_manifest() -> dict:
	return {
		"name": "Volume control",
		"version": "1.0",

		"default_options": {
			"volume_step": 10,
			"min_volume": 10,
			"max_volume": 100,
		},

		"commands": {
			"громкость|звук": set_volume,
			"выключи звук|выключить звук|убери звук" :  mute,
			"включи звук|включить звук|верни звук" :  unmute,
			"уменьши громкость|уменьшить громкость|понизить громкость|понизь громкость|тише" : volume_down,
			"увеличь громкость|увеличить громкость|повысить громкость|повысь громкость|громче" : volume_up,
			"полная громкость|звук на максимум|максимальная громкость" : volume_max,
			"звук на минимум|минимальная громкость|минимум звука" : volume_min,
		}
	}

def volume_min(context: Context, text: str) -> None:
	device.EndpointVolume.SetMasterVolumeLevelScalar(
		context.get_options(__name__)["min_volume"] / 100, 
		None)

def volume_max(context: Context, text: str) -> None:
	device.EndpointVolume.SetMasterVolumeLevelScalar(
		context.get_options(__name__)["max_volume"] / 100, 
		None)

def mute(context: Context, text: str) -> None:
	device.EndpointVolume.SetMute(1, None)

def unmute(context: Context, text: str) -> None:
	device.EndpointVolume.SetMute(0, None)

def set_volume(context: Context, text: str) -> None:
	volume = get_volume_from_text(context, text)
	if volume is None:
		context.say("неверный параметр громкости")

	device.EndpointVolume.SetMasterVolumeLevelScalar(volume, None)

def volume_up(context: Context, text: str) -> None:
	volume = get_volume_from_text(context, text)
	if volume is None:
		volume = context.get_options(__name__)["volume_step"] / 100
	
	volume = min(100, device.EndpointVolume.GetMasterVolumeLevelScalar() + volume)
	device.EndpointVolume.SetMasterVolumeLevelScalar(volume, None)

def volume_down(context: Context, text: str) -> None:
	volume = get_volume_from_text(context, text)
	if volume is None:
		volume = context.get_options(__name__)["volume_step"] / 100
	
	volume = max(0, device.EndpointVolume.GetMasterVolumeLevelScalar() - volume)
	device.EndpointVolume.SetMasterVolumeLevelScalar(volume, None)

def get_volume_from_text(context: Context, text: str) -> int | None:
	volume = context.normalize_numbers(text)
	if volume is None:
		return
	return max(0, min(volume, 100)) / 100
