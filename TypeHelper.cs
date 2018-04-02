using System;
using System.Reflection;
using UnityEngine;

namespace MyNamespace.Utils
{
    public class TypeHelper : Singleton<TypeHelper>
    {
        private const string OutHandlerName = "MyNamespace.Core.OutHandle";

        public static bool IsOutHandler(string fieldName)
        {
            return fieldName == OutHandlerName;
        }

        public static bool IsTypeEqual(Type type1, Type type2)
        {
            if (type1 == type2)
                return true;

            if (type1 == null || type2 == null)
                return false;

            return type2.IsAssignableFrom(type1);
        }

        private const string UNITY_ASM_PREFIX = "UnityEngine";
        private const string UNITY_EDITOR_ASM_PREFIX = "UnityEditor";

        private readonly Assembly _unityAssembly = null;
        private readonly Assembly _unityEditorAssembly = null;
        private readonly Assembly _defaultAssembly = null;

        public TypeHelper()
        {
            _defaultAssembly = Assembly.GetExecutingAssembly();
            _unityAssembly = Assembly.Load(UNITY_ASM_PREFIX);
            _unityEditorAssembly = Assembly.Load(UNITY_EDITOR_ASM_PREFIX);
        }

        public Type GetTypeByName(string typeName)
        {
            typeName = typeName.Trim();

            if (string.IsNullOrEmpty(typeName))
            {
                Debug.LogError("Type name is null or empty");
                return null;
            }

            Type ret = Type.GetType(typeName);

            if (ret == null)
            {
                if (typeName.StartsWith(UNITY_ASM_PREFIX))
                {
                    return _unityAssembly == null ? null : _unityAssembly.GetType(typeName);
                }

                if (typeName.StartsWith(UNITY_EDITOR_ASM_PREFIX))
                {
                    return _unityEditorAssembly == null ? null : _unityEditorAssembly.GetType(typeName);
                }

                return _defaultAssembly == null ? null : _defaultAssembly.GetType(typeName);
            }

            return ret;
        }


        public static string GetNodeValueFullName(NodeValueEnum enm)
        {
            switch(enm)
            {
                case NodeValueEnum.Object:
                    return typeof(System.Object).FullName;
                case NodeValueEnum.Byte:
                    return typeof(Byte).FullName;
                case NodeValueEnum.Short:
                    return typeof(short).FullName;
                case NodeValueEnum.Int:
                    return typeof(int).FullName;
                case NodeValueEnum.Float:
                    return typeof(float).FullName;
                case NodeValueEnum.String:
                    return typeof(string).FullName;
                //case NodeValueEnum.IntArray:
                //    return typeof().FullName;
                //case NodeValueEnum.FloatArray:
                    //return typeof().FullName;
                case NodeValueEnum.Blackboard:
                    return typeof(short).FullName;
                case NodeValueEnum.Event:
                    return typeof(short).FullName;
            }

            return string.Empty;
        }
    }
}

