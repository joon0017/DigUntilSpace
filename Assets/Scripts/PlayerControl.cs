using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D Rig;
    [SerializeField]
    private float Speed;
    [SerializeField]
    public float jumpPower = 3;
    public int curJump = 0;
    private Vector2 RespawnPosition;
    public Animator ani;
    private bool faceDir = false;
    [SerializeField]
    private GameObject colSide;
    [SerializeField]
    private GameObject colGround;



    // Start is called before the first frame update
    void Start()
    {
        Rig = this.GetComponent<Rigidbody2D>();
        RespawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        actionSpace();
        
    }
    // ani.SetBool("isWalking",false);ani.SetBool("Idle",false);ani.SetBool("isDig",true);
    private void dig(GameObject block){
        ani.SetBool("isWalking",false);ani.SetBool("Idle",false);ani.SetBool("isDig",true);
            Destroy(block);
    }
    private void MovePlayer(){
        // move left and right
        float X = Input.GetAxisRaw("Horizontal");
        float Y = 0;
        Vector2 curPosition = transform.position;
        Vector2 newPosition = new Vector2(X,Y) * Speed * Time.deltaTime;
        transform.position = curPosition + newPosition;
        if ( X !=  0) {
            ani.SetBool("isWalking",true);ani.SetBool("Idle",false);ani.SetBool("isDig",false);
            if (X == -1 && faceDir) Flip();
            else if (X == 1 && !faceDir) Flip();
        }
        else {ani.SetBool("isWalking",false);ani.SetBool("Idle",true);ani.SetBool("isDig",false);}
        
    }

    // Flip sprite / animation over the x-axis
    protected void Flip()    
    {
        Vector2 curScale = gameObject.transform.localScale;
        curScale.x *= -1;
        gameObject.transform.localScale = curScale;
        faceDir = !faceDir;
    }

    

    private void actionSpace()
    {
        if (curJump == 0 && Input.GetKeyDown(KeyCode.Space) && Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0)
        {
            //jump only when not moving
            curJump++; 
            Rig.velocity = Vector2.zero;
            Rig.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
        else if(colGround.tag == "dirt" && Input.GetAxisRaw("Vertical") == -1 && Input.GetKeyDown(KeyCode.Space))
        {
            dig(colGround);
            //dig below
        }
        else if(colSide.tag == "dirt" && Input.GetAxisRaw("Horizontal") != 0 && Input.GetKeyDown(KeyCode.Space))
        {  
            dig(colSide);
        }

    }

    public void OnGround()
    {
        curJump = 0;
    }

    private void OnCollisionStay2D(Collision2D boom)
    {

        Collider2D collider = boom.collider;
        //reset jump number to 0 when on on floor
        Vector3 objecthitSize = collider.bounds.size/2f;
        Vector3 contactPoint = boom.contacts[0].point;
        
        Vector3 center = collider.bounds.center;
        if (collider.tag == "dirt")    
        {            
            //contactPoint.y > center.y
            
            if (contactPoint.y >= center.y + objecthitSize.y)
            {
                OnGround(); //only reset jump if contact point is above
                colGround = boom.gameObject;
            }
            else if (contactPoint.x <= center.x + objecthitSize.x || contactPoint.x >= center.x + objecthitSize.x ) 
            {
                colSide = boom.gameObject;
            }
        }
    }

    
    private void OnCollisionEnter2D(Collision2D boom)
    {
        Collider2D collider = boom.collider;
        Vector3 objecthitSize = collider.bounds.size/2f;
        Vector3 contactPoint = boom.contacts[0].point;
        
        Vector3 center = collider.bounds.center;
        if (collider.tag == "dirt")    
        {            
            //contactPoint.y > center.y
            
            if (contactPoint.y >= center.y + objecthitSize.y)
            {
                OnGround(); //only reset jump if contact point is above
            }
        }
    }
    private void OnCollisionExit2D(Collision2D boom){
        if(boom.gameObject ==  colSide){
            colSide = null;
        }
        else if(boom.gameObject == colGround){
            colGround = null;
        }
    }


    public void ResetGravity()
    {
        if (Rig.gravityScale != 1) Rig.gravityScale = 1;
    }
}
