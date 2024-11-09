using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BigGuy : Enemy
{
    [Header("Skill Settings")] public Transform pickPoint;
    public Vector2 throwForce;
    public bool hasBomb;
    private Rigidbody2D pickedBomb;

    public override void Skill(Behaviour item)
    {
        PickUpBomb(item);
    }

    private void PickUpBomb(Behaviour item)
    {
        pickedBomb = item.GetComponent<Rigidbody2D>();

        pickedBomb.bodyType = RigidbodyType2D.Kinematic;

        pickedBomb.transform.position = pickPoint.position;
        pickedBomb.transform.SetParent(pickPoint);

        hasBomb = true;
    }

    public void ThrowBomb()
    {
        if (pickedBomb != null)
        {
            pickedBomb.bodyType = RigidbodyType2D.Dynamic;
            pickedBomb.transform.SetParent(null);

            float randValue = Random.Range(-1f, 1f);
            pickedBomb.AddForce(new Vector2(throwForce.x * randValue, throwForce.y), ForceMode2D.Impulse);
            if (Mathf.Abs(randValue) < .3f)
            {
                Debug.Log("ÊÖ»¬ÁË£¡");
            }

            pickedBomb = null;
        }

        hasBomb = false;
    }

    public override void MoveToTarget()
    {
        if (!hasBomb)
        {
            base.MoveToTarget();
        }
    }
}