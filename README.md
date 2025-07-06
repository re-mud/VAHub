# VAHub
Голосовой ассистент

## Настройка
1. Скачайте модель с https://alphacephei.com/vosk/models  
2. Укажите путь к модели в appsettings.json:
```json
{
  "VoskSpeechRecognition": {
    "ModelPath": "путь_к_модели"
  }
}
```