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
    public Animator animator;
    Buff buff1, buff2;

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
        animator = GetComponent<Animator>();
        StartCoroutine(CallFunctionWithDelay(0.1f));
        int randomValue = Random.Range(0, 4);
        float angle = randomValue * 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        currentHealth = health;
    }

    void Update()
    {

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
                if (GameManager.Instance.isGrappling)
                {
                    GameManager.Instance.scoreChange(score);
                    buff1 = BuffContainer.Instance.GetRandomBuff();
                    buff2 = BuffContainer.Instance.GetRandomBuff();
                    while (buff1.name == buff2.name)
                    {
                        buff2 = BuffContainer.Instance.GetRandomBuff();
                    }

                    AudioManager.Instance.Play("Upgrade");
                    GameManager.Instance.playerAnimator.SetTrigger("ToCloseMouth");
                    animator.SetTrigger("Destory");
                    GameManager.Instance.player.GetComponent<Ship>().grappleObject = null;
                    transform.GetComponent<Attack>().Damage = 0;
                    Debug.Log("Planet is call destory");

                }
                else
                {
                    Destroy(gameObject);
                }
            }

        }
    }

    public void DestoryPlanet()
    {
        Debug.Log("Planet is destory");
        GameManager.Instance.applyBuff(buff1, buff2);
        Destroy(gameObject);
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
