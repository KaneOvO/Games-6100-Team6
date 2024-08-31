using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Buff
{
    public string name;
    public float duration;
    public bool isPermanent;
    public string description;

    public Action ApplyEffect;
    public Action RemoveEffect;

    public Buff(string name, float duration, bool isPermanent = false, string description = "")
    {
        this.name = name;
        this.duration = duration;
        this.isPermanent = isPermanent;
        this.description = description;
    }
}

public class BuffManager : MonoBehaviour
{
    private static BuffManager _instance;
    public List<Buff> activeBuffs = new List<Buff>();

    public static BuffManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BuffManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("BuffManager");
                    _instance = obj.AddComponent<BuffManager>();
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddBuff(Buff buff)
    {
        activeBuffs.Add(buff);
        buff.ApplyEffect?.Invoke();
        if (!buff.isPermanent)
        {
            StartCoroutine(RemoveBuffAfterDuration(buff));
        }
    }

    private IEnumerator RemoveBuffAfterDuration(Buff buff)
    {
        yield return new WaitForSeconds(buff.duration);
        RemoveBuff(buff);
    }

    public void RemoveBuff(Buff buff)
    {
        if (activeBuffs.Contains(buff))
        {
            buff.RemoveEffect?.Invoke();
            activeBuffs.Remove(buff);
        }
    }

    public bool HasBuff(string buffName)
    {
        return activeBuffs.Exists(buff => buff.name == buffName);
    }

    public List<Buff> GetActiveBuffs()
    {
        return new List<Buff>(activeBuffs);
    }
}