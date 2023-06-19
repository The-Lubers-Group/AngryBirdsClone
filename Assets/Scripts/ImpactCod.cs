using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactCod : MonoBehaviour
{
    private int limite;
    private SpriteRenderer spriteR;
    [SerializeField] private Sprite [] sprites;

    // Start is called before the first frame update
    void Start()
    {
        limite = 0;
        spriteR = GetComponent<SpriteRenderer>();
        spriteR.sprite = sprites[0];

    }

    // Update is called once per frame
    void Update()
    {
        print(limite);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(limite < sprites.Length - 1)
            {
                limite++;
                spriteR.sprite = sprites[limite];
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
