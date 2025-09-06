# VAHub
Голосовой ассистент


## Настройка

### Голосовое распознавание (обязательно для Vosk)
1. Скачайте модель с https://alphacephei.com/vosk/models  
2. Укажите путь к модели в `appsettings.json`:
```json
"VoskSpeechRecognition": {
  "ModelPath": "путь_к_модели"
}
```

### Плагины
1. Для python-плагинов добавьте в `appsettings.json`:
```json
"PythonCommandHandler": {
  "PythonDLLPath": "путь_к_библиотеке"
}
```
Обычные названия библиотеки: `python312.dll` (Windows), `libpython3.12.so` (Linux).

### Фраза активации
Если включена, команды без этой фразы или сказанные до нее игнорируются.  
Для настройки фразы добавьте в `appsettings.json`:
```json
"App": {
  "ActivationPhrase": "фраза",
  "ActivationTimeoutSeconds": 15,
  "IsExtendActivationAfterCommand": false
}
```
При произношении `ActivationPhrase` начинается прослушивание в течении `ActivationTimeoutSeconds` секунд. После его истечения фразу нужно повторить.  
`IsExtendActivationAfterCommand` задаёт, продлевать ли время прослушивания после успешно выполненной команды.

### Синтез речи
Для выбора синтезатора добавьте в `appsettings.json`:
```json
"VACore": {
  "SynthesizerKey ": "синтезатор"
}
```
Доступные синтезаторы:
- WindowsSpeechSynthesizer - только для Windows
- ConsoleSpeechSynthesizer - заглушка, выводит текст в консоль

### Распознавание речи
Для выбора распознавателя добавьте в `appsettings.json`:
```json
"VACore": {
  "RecognitionKey ": "распознаватель"
}
```
Доступные распознаватели:
- VoskSpeechRecognition


## Документация
- [Создание плагинов](docs/plugins/)