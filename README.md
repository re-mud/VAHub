# VAHub
Голосовой ассистент

## Настройка
### Голосовое распознавание
1. Скачайте модель с https://alphacephei.com/vosk/models  
2. Укажите путь к модели в `appsettings.json`:
```json
"VoskSpeechRecognition": {
  "ModelPath": "путь_к_модели"
}
```
### Плагины (необязательно)
1. Для python-плагинов добавьте в `appsettings.json`:
```json
"Python": {
  "PythonDLLPath": "путь_к_библиотеке"
}
```

Обычные названия библиотеки: `python312.dll` (Windows), `libpython3.12.so` (Linux).