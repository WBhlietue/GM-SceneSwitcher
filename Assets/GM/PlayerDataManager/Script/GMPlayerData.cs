using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GM.PlayerData.Value;
using GM.PlayerData;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GM.PlayerData
{
    [CreateAssetMenu(menuName = "GM/PlayerData")]
    public class GMPlayerData : ScriptableObject
    {
        public GMIntData intData = new GMIntData();
        public GMFloatData floatData = new GMFloatData();
        public GMStringData stringData = new GMStringData();
        public void Clear()
        {
            PlayerPrefs.DeleteAll();
#if UNITY_EDITOR
            intData.Clear();
            floatData.Clear();
            stringData.Clear();
#endif
        }

#if UNITY_EDITOR
        public void Load()
        {
            intData.Clear();
            floatData.Clear();
            stringData.Clear();
            string[] name = PlayerPrefs.GetString("GM-intNames").Split(" ");
            foreach (var item in name)
            {
                intData.Add(item, PlayerPrefs.GetInt(item, 0));
            }
            name = PlayerPrefs.GetString("GM-floatNames").Split(" ");
            foreach (var item in name)
            {
                floatData.Add(item, PlayerPrefs.GetFloat(item, 0f));
            }
            name = PlayerPrefs.GetString("GM-stringNames").Split(" ");
            foreach (var item in name)
            {
                stringData.Add(item, PlayerPrefs.GetString(item, ""));
            }
        }

        public void SaveIntName()
        {
            PlayerPrefs.SetString("GM-intNames", (intData.GetNames()));
        }
        public void SaveFloatName()
        {
            PlayerPrefs.SetString("GM-floatNames", (floatData.GetNames()));
        }
        public void SaveStringName()
        {
            PlayerPrefs.SetString("GM-stringNames", (stringData.GetNames()));
        }
        public void Save()
        {
            SaveIntName();
            SaveFloatName();
            SaveStringName();

            intData.Save();
            floatData.Save();
            stringData.Save();
        }
#endif
    }

    namespace Value
    {
        [System.Serializable]
        public abstract class GMData<T>
        {
#if UNITY_EDITOR
            public List<GMTupleValue<T>> data = new List<GMTupleValue<T>>();
            public bool isHave(string name)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].name == name)
                    {
                        return true;
                    }
                }
                return false;
            }
            public string GetNames()
            {
                string str = "";
                for (int i = 0; i < data.Count; i++)
                {
                    str += data[i].name + " ";
                }
                if (str.Length > 1)
                {
                    str = str.Substring(0, str.Length - 1);
                }
                return str;
            }
            public void Clear()
            {
                data.Clear();
            }
            public void Add(string name, T value)
            {
                data.Add(new GMTupleValue<T>(name, value));
            }
            public void Set(string name, T value)
            {
                foreach (var item in data)
                {
                    if (item.name == name)
                    {
                        item.value = value;
                        return;
                    }
                }
                data.Add(new GMTupleValue<T>(name, value));
                Save();
            }
            public abstract void Save();
#endif
        }
        [System.Serializable]
        public class GMIntData : GMData<int>
        {
#if UNITY_EDITOR
            public override void Save()
            {
                for (int i = 0; i < data.Count; i++)
                {
                    PlayerPrefs.SetInt(data[i].name, data[i].value);
                }
            }
#endif

            public int this[string name]
            {
                get { return PlayerPrefs.GetInt(name); }
                set
                {
                    PlayerPrefs.SetInt(name, value);
#if UNITY_EDITOR
                    Set(name, value);
#endif
                }
            }
        }
        [System.Serializable]
        public class GMFloatData : GMData<float>
        {
#if UNITY_EDITOR
            public override void Save()
            {
                for (int i = 0; i < data.Count; i++)
                {
                    PlayerPrefs.SetFloat(data[i].name, data[i].value);
                }
            }
#endif
            public float this[string name]
            {
                get { return PlayerPrefs.GetFloat(name); }
                set
                {
                    PlayerPrefs.SetFloat(name, value);
#if UNITY_EDITOR
                    Set(name, value);
#endif
                }
            }
        }
        [System.Serializable]
        public class GMStringData : GMData<string>
        {
#if UNITY_EDITOR
            public override void Save()
            {
                for (int i = 0; i < data.Count; i++)
                {
                    PlayerPrefs.SetString(data[i].name, data[i].value);
                }
            }
#endif
            public string this[string name]
            {
                get { return PlayerPrefs.GetString(name); }
                set
                {
                    PlayerPrefs.SetString(name, value);
#if UNITY_EDITOR
                    Set(name, value);
#endif
                }
            }
        }
        [System.Serializable]
        public class GMTupleValue<T>
        {
            public string name;
            public T value;
            public GMTupleValue()
            {

            }
            public GMTupleValue(string n, T v)
            {
                name = n;
                value = v;
            }
        }

    }
}

#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(GMPlayerData))]
public class GMPlayerDataEditor : Editor
{
    bool intEnable = true;
    int intSize = -1;
    bool floatEnable = true;
    int floatSize = -1;
    bool stringEnable = true;
    int stringSize = -1;
    public override void OnInspectorGUI()
    {
        GUILayout.MinWidth(1);
        GMPlayerData pl = (GMPlayerData)target;
        if (intSize < 0)
        {
            intSize = pl.intData.data.Count;
        }
        if (floatSize < 0)
        {
            floatSize = pl.floatData.data.Count;
        }
        if (stringSize < 0)
        {
            stringSize = pl.stringData.data.Count;
        }
        float originWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 1;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Int Datas", GUILayout.ExpandWidth(true));
        intEnable = EditorGUILayout.Toggle(intEnable);
        EditorGUILayout.LabelField("Size", GUILayout.ExpandWidth(false));
        intSize = EditorGUILayout.IntField(intSize);
        if (GUILayout.Button("Change"))
        {
            int delta = intSize - pl.intData.data.Count;
            while (delta != 0)
            {
                if (delta > 0)
                {
                    pl.intData.data.Add(new GMTupleValue<int>());
                    delta--;
                }
                else
                {
                    pl.intData.data.RemoveAt(pl.intData.data.Count - 1);
                    delta++;
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        if (intEnable)
        {
            for (int i = 0; i < pl.intData.data.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(i + ". name", GUILayout.ExpandWidth(false));
                pl.intData.data[i].name = EditorGUILayout.TextField(pl.intData.data[i].name);
                EditorGUILayout.LabelField("  value", GUILayout.ExpandWidth(false));
                pl.intData.data[i].value = EditorGUILayout.IntField(pl.intData.data[i].value);
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUIUtility.labelWidth = originWidth;

        ///////////

        originWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 1;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Float Datas", GUILayout.ExpandWidth(true));
        floatEnable = EditorGUILayout.Toggle(floatEnable);
        EditorGUILayout.LabelField("Size", GUILayout.ExpandWidth(false));
        floatSize = EditorGUILayout.IntField(floatSize);
        if (GUILayout.Button("Change"))
        {
            int delta = floatSize - pl.floatData.data.Count;
            while (delta != 0)
            {
                if (delta > 0)
                {
                    pl.floatData.data.Add(new GMTupleValue<float>());
                    delta--;
                }
                else
                {
                    pl.floatData.data.RemoveAt(pl.floatData.data.Count - 1);
                    delta++;
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        if (floatEnable)
        {
            for (int i = 0; i < pl.floatData.data.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(i + ". name", GUILayout.ExpandWidth(false));
                pl.floatData.data[i].name = EditorGUILayout.TextField(pl.floatData.data[i].name);
                EditorGUILayout.LabelField("  value", GUILayout.ExpandWidth(false));
                pl.floatData.data[i].value = EditorGUILayout.FloatField(pl.floatData.data[i].value);
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUIUtility.labelWidth = originWidth;

        ///////////////////

        originWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 1;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("String Datas", GUILayout.ExpandWidth(true));
        stringEnable = EditorGUILayout.Toggle(stringEnable);
        EditorGUILayout.LabelField("Size", GUILayout.ExpandWidth(false));
        stringSize = EditorGUILayout.IntField(stringSize);
        if (GUILayout.Button("Change"))
        {
            int delta = stringSize - pl.stringData.data.Count;
            while (delta != 0)
            {
                if (delta > 0)
                {
                    pl.stringData.data.Add(new GMTupleValue<string>());
                    delta--;
                }
                else
                {
                    pl.stringData.data.RemoveAt(pl.stringData.data.Count - 1);
                    delta++;
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        if (stringEnable)
        {
            for (int i = 0; i < pl.stringData.data.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(i + ". name", GUILayout.ExpandWidth(false));
                pl.stringData.data[i].name = EditorGUILayout.TextField(pl.stringData.data[i].name);
                EditorGUILayout.LabelField("  value", GUILayout.ExpandWidth(false));
                pl.stringData.data[i].value = EditorGUILayout.TextField(pl.stringData.data[i].value);
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUIUtility.labelWidth = originWidth;
        for (int i = 0; i < 10; i++)
        {
            EditorGUILayout.Space();
        }
        if (GUILayout.Button("Load"))
        {
            pl.Load();
            intSize = -1;
            floatSize = -1;
            stringSize = -1;
        }
        if (GUILayout.Button("Save"))
        {
            pl.Save();
        }
        if (GUILayout.Button("Clear"))
        {
            pl.Clear();
        }
    }
}

#endif