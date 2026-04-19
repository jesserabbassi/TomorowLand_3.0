using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
[RequireComponent(typeof(AudioSource))]

public class playermvt : MonoBehaviour
{
    public float speed = 6f, gravity = 10f,jumpforce = 8f;
    public float smoothtime = 0.1f;
    CharacterController ctr;
    public Transform cam;
    Animator anim;
    private float turningvelocity;
    
    Vector3 movdir;
    AudioSource AudioSource;
    




    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        ctr = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        AudioSource = GetComponent<AudioSource>();


    }

    // Update is called once per frame
    void Update()
    {


        float h, v;
        (h, v) = (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Vector3 dir = new Vector3(h, 0, v).normalized;



        movdir.y -= gravity*Time.deltaTime;
        if (h != 0 || v != 0)
        {
            float targetang = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float ang = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetang, ref turningvelocity, smoothtime);
            transform.rotation = Quaternion.Euler(0, ang, 0);
            anim.SetBool("run", true);
            movdir = Quaternion.Euler(0, targetang, 0) * Vector3.forward+movdir.y*Vector3.up;




        }
        else
        {
            anim.SetBool("run", false);
            movdir.x = 0f;
            movdir.z = 0f;
        }




        ctr.Move(movdir * speed * Time.deltaTime);

    }

    
    
    
    public void PlayRepairSound(AudioClip audio)
    {
        if (AudioSource != null && audio != null)
        {
            AudioSource.PlayOneShot(audio);
        }
    }






}   
