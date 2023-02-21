using UnityEngine;

public class AttackBox : MonoBehaviour
{
    private enum AttacksWhat { Player, Enemy };

    [SerializeField] private AttacksWhat attacksWhat;
    [SerializeField] private int attackPower = 1;
    [SerializeField] private int targetSide;
    [SerializeField] private bool isPassiveEnemy;

    private float playerRecoveryCounter = 2;
    private bool playerHit = false;

    private void Update()
    {
        if (playerHit)
        {
            playerRecoveryCounter += Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (attacksWhat == AttacksWhat.Player)
        {
            if (col.gameObject == NewPlayer.Instance.gameObject && playerRecoveryCounter >= 2)
            {
                playerHit = true;
                playerRecoveryCounter = 0;

                if (!isPassiveEnemy)
                {
                    gameObject.GetComponentInParent<Animator>().SetTrigger("attack");
                }

                if (FindObjectOfType<UI>() != null)
                {
                    NewPlayer.Instance.Hurt(attackPower, targetSide);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (transform.parent.transform.position.x < col.transform.position.x)
        {
            targetSide = -1;
        }
        else
        {
            targetSide = 1;
        }

        if (attacksWhat == AttacksWhat.Enemy)
        {
            if (col.gameObject.GetComponent<Enemy>())
            {
                AnimatorFunctions.Instance.onEnemy = true;
                col.gameObject.GetComponent<Enemy>().Hurt(attackPower);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (attacksWhat == AttacksWhat.Enemy)
        {
            if (col.gameObject.GetComponent<Enemy>())
            {
                AnimatorFunctions.Instance.onEnemy = false;
            }
        }

        if (attacksWhat == AttacksWhat.Player)
        {
            if (col.gameObject == NewPlayer.Instance.gameObject)
            {
                playerHit = false;
                playerRecoveryCounter = 2;
            }
        }
    }
}
