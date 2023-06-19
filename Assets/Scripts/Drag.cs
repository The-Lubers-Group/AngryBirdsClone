using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private Collider2D drag;
    public LayerMask layer;
    [SerializeField] private bool clicked;
    private Touch touch;
    public float TempoParaMorrer;

    public LineRenderer lineFront;
    public LineRenderer lineBack;
    
    private Ray leftCatapultRay;
    private CircleCollider2D passaroCol;
    private Vector2 catapultToBird;
    private Vector3 pointL;

    private SpringJoint2D spring;
    private Vector2 prevVel;
    private Rigidbody2D passaroRB;

    public GameObject bomb;

    //Limite do elastico

    private Transform catapult;
    private Ray rayToMT;
    void Start()
    {
        drag = GetComponent<Collider2D>();
        SetupLine();
        leftCatapultRay = new Ray(lineFront.transform.position, Vector3.zero);
        passaroCol = GetComponent<CircleCollider2D>();
        spring = GetComponent<SpringJoint2D>();
        passaroRB = GetComponent<Rigidbody2D>();

        catapult = spring.connectedBody.transform;
        rayToMT = new Ray(catapult.position, Vector3.zero);

    }

    // Update is called once per frame
    void Update()
    {
        LineUpdate();
        SpringEffect();
        prevVel = passaroRB.velocity;

        #if UNITY_ANDROID

        if(Input.touchCount > 0 )
        {
            touch = Input.GetTouch(0);
            
            Vector2 wp = Camera.main.ScreenToWorldPoint(touch.position);
            RaycastHit2D hit = Physics2D.Raycast(wp,Vector2.zero,Mathf.Infinity, layer.value);
            if(hit.collider != null)
            {
                clicked = true;
            }
            if (clicked)
            {
                if(touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                {
                    Vector3 tPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                    catapultToBird = tPos - catapult.position;
                    if(catapultToBird.sqrMagnitude > 9.0f)
                    {
                        rayToMT.direction = catapultToBird;
                        tPos = rayToMT.GetPoint(3f);
                    }
                    transform.position = tPos;
                }

            }
            if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                passaroRB.isKinematic = false;
                clicked = false;
                MataPassaro();
            }
        }
        #endif
        #if UNITY_EDITOR
        if(clicked)
        {
            Dragging();
        }
        #endif

        if(!clicked && !passaroRB.isKinematic)
        {
            MataPassaro();
        }
    }
    void SetupLine()
    {
        //Setando o começo das linhas nos pivot da estilingue
        lineFront.SetPosition(0,lineFront.transform.position);
        lineBack.SetPosition(0,lineBack.transform.position);

    }
    void LineUpdate()
    {
        //Obtendo a Direção de Estilingue até o Passaro
        catapultToBird = transform.position - lineFront.transform.position;
        //Asignando a direção que esta apontando para o passaro
        leftCatapultRay.direction = catapultToBird;

        //Obtendo do Raio um ponto desse raio, somando a distancia que leva desde a estilingue até o passaro e adicionamos o raio do seu colisor para cobrir o passaro completamente.  
        pointL = leftCatapultRay.GetPoint(catapultToBird.magnitude + passaroCol.radius);

        //Atualiza posição da linha com respeito a posição do passaro.
        lineFront.SetPosition(1,pointL);
        lineBack.SetPosition(1,pointL);
    }

    void SpringEffect()
    {
        if(spring != null)
        {
            if(passaroRB.isKinematic == false)
            {
                //verificio velocidade
                if(prevVel.sqrMagnitude > passaroRB.velocity.sqrMagnitude)
                {
                    lineBack.enabled =false;
                    lineFront.enabled =false;
                    Destroy(spring);
                    //ajuste da velocidade para corregir o efeto do spring
                    passaroRB.velocity = prevVel;  
                }
            }
        }
    }
    void MataPassaro()
    {
        if(passaroRB.velocity.magnitude == 0 && passaroRB.IsSleeping())
        {
            StartCoroutine(TempoMorte());
        }
    }
    IEnumerator TempoMorte()
    {
        yield return new WaitForSeconds(TempoParaMorrer);
        Instantiate(bomb, new Vector3 (transform.position.x, transform.position.y,-0.5f), Quaternion.identity);
        Destroy(gameObject);
    }

    void Dragging()
    {
        Vector3 mouseWp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWp.z = 0f;

        catapultToBird = mouseWp - catapult.position;
        if(catapultToBird.sqrMagnitude > 9.0f)
        {
            rayToMT.direction = catapultToBird;
            mouseWp = rayToMT.GetPoint(3f);
        }

        transform.position = mouseWp;

    }
    void OnMouseDown()
    {
        clicked = true;
    }
    void OnMouseUp()
    { 
        passaroRB.isKinematic = false;
        clicked = false;
    }
}
