using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour
{

    void Start()
    {
        string[] w;
        string str;
        str = "hi 123 abc def YOOOOO";
        // List<string> l;
        w = str.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        str = "";
        for (int i = w.Length - 1; i >= 0; i--)
        {
            str += w[i] + " ";
        }
        Debug.Log(str);
    }
}
