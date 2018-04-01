#!usr/bin/env python3
#-*- coding:utf-8 -*-

import os

remove_list = ['NodeDebug.cs']

def main():
    files = os.listdir()

    for file in files:
        if file.endswith(".cs"):
            content = ""
            with open(file, "r", encoding="utf-8") as f:
                content = f.read()
                content = content.replace("NodeEditor", "MyNamespace").replace("NodeDebug", "Debug")
            with open(file, "w", encoding="utf-8") as f:
                f.write(content)

    for file in remove_list:
        os.remove(file)

    print("finish")

if __name__ == '__main__':
    main()
