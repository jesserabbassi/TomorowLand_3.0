using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class playermvt : MonoBehaviour
{
    public float speed = 6f, gravity = 10f;
    public float smoothtime = 0.1f;
    CharacterController ctr;
    public Transform cam;
    Animator anim;
    private float turningvelocity;
    dollcontroller doll;
    Vector3 movdir;
    [SerializeField] GameObject body;
    public bool die = false;
    public bool readytobepushed = true;
    
    public bool issafe;
    [SerializeField]Image panelpush;
    Vector3 firstpos;

    public float pushForce = 3f;         // Strength of the push
    public float pushRange = 2f;         // How far the player can reach
    public LayerMask botLayer;
    public AnimationClip clip;

    private void Awake()
    {
        firstpos = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        ctr = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        doll = FindAnyObjectByType<dollcontroller>();
        
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!die && !doll.endgame)
        {
            float h, v;
            (h, v) = (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            Vector3 dir = new Vector3(h, 0, v).normalized;



            movdir.y -= gravity;
            if (h != 0 || v != 0)
            {
                float targetang = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float ang = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetang, ref turningvelocity, smoothtime);
                transform.rotation = Quaternion.Euler(0, ang, 0);
                anim.SetBool("run", true);
                movdir = Quaternion.Euler(0, targetang, 0) * Vector3.forward;
                StartCoroutine(loosing());



            }
            else
            {
                anim.SetBool("run", false);
                (movdir.x, movdir.z) = (0, 0);
            }
            ctr.Move(movdir * speed * Time.deltaTime);

            Vector3 origin = body.transform.position + Vector3.up * 1f;  // Chest height
            Vector3 direction = body.transform.forward;
            Ray ray = new Ray(origin, direction);
            if (Physics.Raycast(ray, out RaycastHit hit, pushRange, botLayer) && readytobepushed)
            {
                panelpush.gameObject.SetActive(true);

                if (Input.GetKey(KeyCode.E))
                {

                    StartCoroutine(push());

                    Ray ray2 = new Ray(doll.head.transform.position, body.transform.position - doll.head.transform.position);
                     
                    if (Physics.Raycast(ray2, out RaycastHit hit2, Mathf.Infinity))
                    {
                        if (hit2.collider.tag == "Player" && !issafe && doll.checkplayers)
                        {
                            StartCoroutine(loosing());
                        }
                    }
                    Vector3 pushDir = transform.forward;
                    hit.collider.transform.position += pushDir * pushForce * Time.smoothDeltaTime;

                    StartCoroutine(hit.collider.GetComponent<NPCSmvt>().fallingcrot());

                }


            }
            else
            {

                panelpush.gameObject.SetActive(false);
            }
            if (!readytobepushed)
            {
                StartCoroutine(loosing());
            }
        }
    }
    
    IEnumerator push()
    {
        
        anim.SetBool("push",true);
        
        yield return new WaitForSeconds(clip.length);

        anim.SetBool("push", false);

    }
    public IEnumerator loosing(bool endgame = false)
    {
        if (doll.checkplayers && !issafe && !die)
        {
            die = true;
            
            yield return new WaitForSeconds(0.7f);
            doll.source.PlayOneShot(doll.audios[4]);
            anim.SetTrigger("die");
            GetComponent<playertimemanagment>().msgpanel.SetActive(true);
            GetComponent<playertimemanagment>().msgtxt.text = "you die\nrespowning in 4s";
            yield return new WaitForSeconds(4);
            if(!endgame)
                GetComponent<playertimemanagment>().msgpanel.SetActive(false);
                die = false;
                resetplayer();
            
        }
    }


    void resetplayer()
    {
        StopAllCoroutines();
        anim.SetTrigger("reset");
        transform.position = firstpos;
        issafe = false;
        readytobepushed = true;
        transform.rotation = Quaternion.identity;
    }


    public IEnumerator fallingcrot()
    {
        if (readytobepushed && anim)
        {
            readytobepushed = false;
            anim.SetTrigger("fall");
            yield return new WaitForSeconds(1.1f);

            anim.SetTrigger("stand");
            yield return new WaitForSeconds(2.06f);
            readytobepushed = true;
            anim.SetTrigger("idle");

        }
    }




}   
