using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;

    [SerializeField] private AudioClip fireballSound;

    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;
    public float attackOffset;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
#if !UNITY_STANDALONE
        InvokeRepeating(nameof(Attack), 0, attackOffset);
#endif
    }

    private void Update()
    {
#if UNITY_STANDALONE
        if (Input.GetMouseButton(0) && (cooldownTimer > attackCooldown) && playerMovement.canAttack())
        {
            Attack();
        }

        cooldownTimer += Time.deltaTime;
#endif
    }

    private void Attack()
    {
        SoundManager.instance.PlaySound(fireballSound);

        anim.SetTrigger("attack");
        cooldownTimer = 0;

        //pool fireballs
        fireballs[FindFireball()].transform.position = firePoint.position;
        fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindFireball()
    {
        for (int i =0; i< fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy) return i;
        }
        return 0;
    }
}
