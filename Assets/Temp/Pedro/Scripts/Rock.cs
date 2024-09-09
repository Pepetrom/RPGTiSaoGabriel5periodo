using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    /*public GameObject rockSpawnPos;
    public BossStateMachine boss;
    public float speed;
    private Rigidbody rb;
    private bool isThrown;*/

    void Start()
    {
        //rockSpawnPos = GameObject.Find("RockHandler"); // Substitua "NomeDoObjetoRockSpawnPos" pelo nome real do objeto
    }
    void FixedUpdate()
    {
        /*if (!isThrown)
        {
            FollowSpawnPosition();

            if (boss.isThrowing)
            {
                Debug.Log("Tacou");
                ThrowRock();
            }
        }
        if (boss.isThrowing)
        {
            Debug.Log("Tacou");
            ThrowRock();
        }*/
    }

    public void FollowSpawnPosition(GameObject rockSpawnPos)
    {
        // Mantenha o proj�til na posi��o de rockSpawnPos
        transform.position = rockSpawnPos.transform.position;
        transform.rotation = rockSpawnPos.transform.rotation;
    }

    public void ThrowRock(float speed, GameObject rockSpawnPos,Rigidbody rb)
    {
        // Define que o proj�til foi lan�ado
        // Remove o controle de posi��o do proj�til e aplica velocidade
        rb.velocity = rockSpawnPos.transform.forward * speed;
    }
}
