import glob

def format_nested(text, indent_level):
    indent = "    " * indent_level
    current_indent = indent_level
    formatted = ""
    in_string = False
    i = 0
    while i < len(text):
        char = text[i]
        if char == '"' and (i == 0 or text[i-1] != '\\'):
            in_string = not in_string
            formatted += char
        elif not in_string:
            if char in '{[':
                current_indent += 1
                formatted += char + "\n" + ("    " * current_indent)
                if i + 1 < len(text) and text[i+1] == ' ': i += 1
            elif char in '}]':
                current_indent -= 1
                formatted += "\n" + ("    " * current_indent) + char
            elif char == ',':
                formatted += ",\n" + ("    " * current_indent)
                if i + 1 < len(text) and text[i+1] == ' ': i += 1
            else:
                formatted += char
        else:
            formatted += char
        i += 1
    return formatted

for filepath in glob.glob('Controllers/*ApiController.cs'):
    with open(filepath, 'r', encoding='utf-8') as f:
        lines = f.readlines()
    new_lines = []
    in_static = False
    for line in lines:
        if 'private static readonly List<' in line:
            in_static = True
            new_lines.append(line)
            continue
        if in_static:
            if line.strip() == '];':
                in_static = False
                new_lines.append(line)
                continue
            stripped = line.strip()
            if stripped.startswith('new()'):
                ends_with_comma = stripped.endswith(',')
                if ends_with_comma: stripped = stripped[:-1]
                prefix = line[:len(line) - len(line.lstrip())]
                base_indent = len(prefix) // 4
                formatted_item = format_nested(stripped, base_indent)
                if ends_with_comma: formatted_item += ','
                new_lines.append(prefix + formatted_item + '\n')
            else:
                new_lines.append(line)
        else:
            new_lines.append(line)
    with open(filepath, 'w', encoding='utf-8') as f:
        f.writelines(new_lines)
