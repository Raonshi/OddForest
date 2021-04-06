using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum Name
    {
        Wolf,
        Bat,
        Vampire,
        Orge,
    }

    public Name name;

    public int hp;
    public int atk;
    public float moveSpeed;

    public SpriteRenderer sprite;


    public void Init()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();

        Sprite[] sp;

        switch (name)
        {
            case Name.Wolf:
                hp = 20;
                atk = 10;
                moveSpeed = 2;

                sp = Resources.LoadAll<Sprite>("Images/Units/Wolf/Idle");
                sprite.sprite = sp[0];

                //sprite.sprite = Resources.Load<Sprite>("Images/Units/Wolf/Idle_0");

                break;
            case Name.Bat:
                hp = 10;
                atk = 10;
                moveSpeed = 3;

                sp = Resources.LoadAll<Sprite>("Images/Units/Bat/Fly");
                sprite.sprite = sp[0];

                //sprite.sprite = Resources.Load<Sprite>("Images/Units/Bat/Fly_0");

                break;
            case Name.Vampire:
                hp = 150;
                atk = 25;
                moveSpeed = 4;

                sp = Resources.LoadAll<Sprite>("Images/Units/Vampire/Idle");
                sprite.sprite = sp[0];

                //sprite.sprite = Resources.Load<Sprite>("Images/Units/Vampire/Idle_0");

                break;
            case Name.Orge:
                hp = 300;
                atk = 60;
                moveSpeed = 2;

                sp = Resources.LoadAll<Sprite>("Images/Units/Orge/Idle");
                sprite.sprite = sp[0];

                //sprite.sprite = Resources.Load<Sprite>("Images/Units/Orge/Idle_0");

                break;
        }
    }
}
