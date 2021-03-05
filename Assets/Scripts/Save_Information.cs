using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save_Information {

    public int Info_HP_Current { get; set; }
    public Vector3 Info_Position { get; set; }
    public string Info_Name { get; set; }
    public string Info_Type { get; set; }


    public Save_Information(int hp, Vector3 pos,string name ,string type)
    {
        Info_HP_Current = hp;
        Info_Position = pos;
        Info_Name = name;
        Info_Type = type;
    }
}
