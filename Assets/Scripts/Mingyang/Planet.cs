using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : Item
{
    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;

    [SerializeField] int score;
    [SerializeField] bool hasTakenDamage = false;
    public bool isInScene = true;

    public float MinSpeed
    {
        get { return minSpeed; }
    }

    public float MaxSpeed
    {
        get { return maxSpeed; }
    }

    void Start()
    {
        StartCoroutine(CallFunctionWithDelay(0.1f));
        int randomValue = Random.Range(0, 4);
        float angle = randomValue * 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public override void TakeDamage(Attack attacker)
    {
        if (hasTakenDamage) return;
        if (attacker.CompareTag("Player"))
        {
            currentHealth -= attacker.Damage;
            hasTakenDamage = true;

            if (currentHealth < 1)
            {
                if (attacker.CompareTag("Player"))
                {
                    if (attacker.GetComponent<Ship>().isInvincible)
                    {
                        GameManager.Instance.scoreChange(score);
                        Buff buff1 = BuffContainer.Instance.GetRandomBuff();
                        Buff buff2 = BuffContainer.Instance.GetRandomBuff();
                        while (buff1.name == buff2.name)
                        {
                            buff2 = BuffContainer.Instance.GetRandomBuff();
                        }
                        GameManager.Instance.applyBuff(buff1, buff2);
                        AudioManager.Instance.Play("Upgrade");
                    }

                }
                GameManager.Instance.playerAnimator.SetTrigger("ToCloseMouth");
                
                //Destroy(gameObject);
            }

        }
    }



    IEnumerator CallFunctionWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetDamage();

    }

    void SetDamage()
    {
        GetComponent<Attack>().Damage = 1;
    }
}
