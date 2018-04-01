#!usr/bin/env python3
#-*- coding:utf-8 -*-

import os

def main():
    files = os.listdir()

    for file in files:
        if file.endswith(".cs"):
            content = ""
            with open(file, "r", encoding="utf-8") as f:
                content = f.read()
                content = content.replace("NodeEditor", "MyNamespace")
            with open(file, "w", encoding="utf-8") as f:
                f.write(content)

    print("finish")

if __name__ == '__main__':
    main()
