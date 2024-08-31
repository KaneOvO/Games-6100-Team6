using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuffContainer : MonoBehaviour
{
    private BuffManager buffManager;

    public Buff grappleSpeedBuff;
    public Buff grapDistanceBuff;
    public Buff extraLifeBuff;
    public Buff longerInvincibleBuff;
    public Buff rotationSpeedBuff;


    private Dictionary<Buff, float> buffWeights;
    private HashSet<string> onceOnlyBuffs;

    //Singleton
    private static BuffContainer _instance;
    public static BuffContainer Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BuffContainer>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("BuffContainer");
                    _instance = obj.AddComponent<BuffContainer>();
                }
            }
            return _instance;
        }
    }


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        buffManager = BuffManager.Instance;

        IncreaseGrapSpeed();
        IncreaseGrapDistance();
        AddExtraLife();
        IncreaseInvincibleTime();
        IncreaseRotationSpeed();

        InitializeBuffWeights();
    }

    private void Update()
    {
        
    }

    private void IncreaseGrapSpeed()
    {
        grappleSpeedBuff = new Buff("Grapple Speed Buff", 0, true, "Increase grapple speed by 50%")
        {
            ApplyEffect = () =>
        {

            GameManager.Instance.hook.GetComponent<Hook>().launchSpeed *= 1.5f;
        },
            RemoveEffect = () =>
            {
                GameManager.Instance.hook.GetComponent<Hook>().launchSpeed /= 1.5f;
            }
        };
    }

    private void IncreaseGrapDistance()
    {
        grapDistanceBuff = new Buff("Grapple Distance Buff", 0, true, "Increase grapple distance by 50%")
        {
            ApplyEffect = () =>
            {
                GameManager.Instance.hook.GetComponent<Hook>().maxDistance *= 1.5f;
            },
            RemoveEffect = () =>
            {
                GameManager.Instance.hook.GetComponent<Hook>().maxDistance /= 1.5f;
            }
        };
    }

    private void AddExtraLife()
    {
        extraLifeBuff = new Buff("Extra Life Buff", 0, true, "Add an extra life")
        {
            ApplyEffect = () =>
            {
                GameManager.Instance.lives++;
            },
            RemoveEffect = () =>
            {

            }
        };
    }

    private void IncreaseInvincibleTime()
    {
        longerInvincibleBuff = new Buff("Longer Invincible Buff", 0, true, "Increase invincible time by 50%")
        {
            ApplyEffect = () =>
            {
                GameManager.Instance.hook.GetComponent<Hook>().invinciblePeriod *= 1.5f;
            },
            RemoveEffect = () =>
            {
                GameManager.Instance.hook.GetComponent<Hook>().invinciblePeriod /= 1.5f;
            }
        };
    }

    private void IncreaseRotationSpeed()
    {
        rotationSpeedBuff= new Buff("Rotation Speed Buff", 0, true, "Increase rotation speed by 50%")
        {
            ApplyEffect = () =>
            {
                GameManager.Instance.player.GetComponent<Ship>().Rotation *= 1.5f;
            },
            RemoveEffect = () =>
            {
                GameManager.Instance.player.GetComponent<Ship>().Rotation /= 1.5f;
            }
        };
    }



    private void InitializeBuffWeights()
    {
        buffWeights = new Dictionary<Buff, float>
        {
            { grappleSpeedBuff, 1 },
            { grapDistanceBuff, 1 },
            { extraLifeBuff, 1 },
            { longerInvincibleBuff, 1 },
            { rotationSpeedBuff, 1 }
        };

        onceOnlyBuffs = new HashSet<string> { };
    }

    public void IncreaseBuffWeight(Buff buff, float increment)
    {
        if (buffWeights.ContainsKey(buff))
        {
            buffWeights[buff] += increment;
        }
        else
        {
            buffWeights[buff] = increment;
        }
    }

    public void DecreaseBuffWeight(Buff buff, float decrement)
    {
        if (buffWeights.ContainsKey(buff) && buffWeights[buff] > decrement)
        {
            buffWeights[buff] -= decrement;
        }
        else
        {
            buffWeights.Remove(buff);
        }
    }

    public bool isBuffExist(Buff buff)
    {
        return buffWeights.ContainsKey(buff);
    }

    public void SetBuffWeight(Buff buff, float newWeight)
    {
        if (newWeight > 0)
        {
            buffWeights[buff] = newWeight;
        }
        else
        {
            buffWeights.Remove(buff);
        }
    }

    public void AddOnceOnlyBuff(string buffName)
    {
        onceOnlyBuffs.Add(buffName);
    }

    public Buff GetRandomBuff()
    {
        if (onceOnlyBuffs.Count > 0)
        {
            buffWeights.Remove(buffWeights.Keys.FirstOrDefault(b => onceOnlyBuffs.Contains(b.name)));
            onceOnlyBuffs.Clear();
        }


        float totalWeight = buffWeights.Values.Sum();
        float randomPoint = Random.value * totalWeight;

        foreach (var buff in buffWeights)
        {
            if (randomPoint < buff.Value)
            {
                return buff.Key;
            }
            randomPoint -= buff.Value;
        }
        return null;
    }

    public void printBuffWeights()
    {
        foreach (var buff in buffWeights)
        {
            Debug.Log(buff.Key.name + ": " + buff.Value);
        }
    }
}
