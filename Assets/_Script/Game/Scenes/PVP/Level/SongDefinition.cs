﻿using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/FNF/SongDefinition", fileName = "Song Definition")]
public class SongDefinition : ScriptableObject
{
    public int Id;
    public string SongName;
    public int Stars;
    public bool IsNewAndHot;
    public bool IsTrending;
    public bool IsSkibidy;
    public GameObject EasyLevel;
    public GameObject NormalLevel;
    public GameObject HardLevel;
}
