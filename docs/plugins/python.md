# Python-плагин

## Манифест
В `manifest.json` укажите тип:
```json
"type": "python"
```

## Команды
В поле `commands` из `manifest.json` под значениями ключей укажите имя скрипта и его функцию в формате `имя:функция`, пример: `script.py:foo`.

Скрипт должен принимать один аргумент:
```python
def foo(text):
	pass
```
В качестве аргумента будет передан текст команды которым была вызвана функция.

Функция может вернуть ответ в виде json
```python
import json

def foo(text):
	return json.dumps({
		"speak": text
	})
```