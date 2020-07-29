using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Actor
{
  
    CharacterController charController;
    
    [Header ("Equipment")]
    public GameObject Weapon;
    GameObject EquipedObject;

    [Header ("Animation States")]
    public Animator animator;
    public RuntimeAnimatorController [] animControllers; //0 is Weapon, 1 is torch

    [Header ("Combat")]
    public int noOfClicks = 0;
    float lastClickedTime = 0f;
    public float maxComboDelay = 0.9f;
    public GameObject AbilityObject;
    public float abilityCooldown = 1;
    
    [Header ("Movement")]
    public float moveSpeed = 5f;
    public float dodgeSpeed = 10f;
    public float gravity = 20f;
    bool isDodging;
    bool isAttacking;
    bool canMove = true;
    Vector3 input;

   // [Header ("Torch")]
   bool hasTorch = false;
     void Start()
     {
        charController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        Equip(Weapon);  
     } 

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }
        
        Movement();
        GetInput();
    }
    void Movement()
    {
        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        input = Vector3.ClampMagnitude(input, 1);
        if(canMove)
        {
            if(isDodging)
            {
                charController.Move(input *Time.deltaTime * dodgeSpeed);
            }
            else
            {
                if(charController.isGrounded)
                {
                    if(input != Vector3.zero)
                    {
                        animator.SetBool("IsMoving", true);
                    }
                    if(input == Vector3.zero)
                    {
                        animator.SetBool("IsMoving", false);
                    }
                    transform.LookAt(input + transform.position);
                    charController.Move(input * Time.deltaTime * moveSpeed);
                }
            }
        }
        input.y -= gravity * Time.deltaTime;
        charController.Move(input * Time.deltaTime);
    }


    void GetInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            lastClickedTime = Time.time;
            noOfClicks++;
            Attack();
        }
        if(Input.GetKeyDown(KeyCode.LeftShift) && charController.isGrounded)
        {
            Dodge();
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Ability();
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(hasTorch)
            {
                DropEquipped();
                hasTorch = false;
                animator.runtimeAnimatorController = animControllers[0];
            }
        }
    }
    void Equip(GameObject item)
    {
        item.transform.parent = RightHandSocket.transform;
        item.transform.position = RightHandSocket.transform.position;
        item.transform.rotation  = RightHandSocket.transform.rotation;
        EquipedObject = item;
    }

    void UnEquipWeapon(GameObject item)
    {
        item.transform.parent = BackSocket.transform;
        item.transform.position = BackSocket.transform.position;
        item.transform.rotation  = BackSocket.transform.rotation;
    }

    void DropEquipped()
    {
        EquipedObject.transform.parent = null;
        EquipedObject.transform.position = transform.position + transform.forward * 1;
        EquipedObject.transform.eulerAngles = new Vector3 (-90, 0, 0);
        Equip(Weapon);
    }

    void Attack()
    {
        if(!hasTorch)
        {
            if (noOfClicks == 1)
            {
                canMove = false;
                animator.SetTrigger("Attack1");
                Debug.Log(1);
            }
        }
    }

    void Ability()
    {
        animator.SetTrigger("Ability");
        canMove = false;
    }

    void Dodge()
    {
        if(!isDodging)
        {
            isDodging = true;
            animator.SetTrigger("Dodge");
        }
    }

    /*Animation events
    */
    void EndAbility()
    {
        canMove = true;
    }

    void EndDodge()
    {
        Debug.Log("Stop Dodge");
        isDodging = false;
    }

    void EndCombo()
    {
        Debug.Log("Combo End");
        animator.SetTrigger("EndAttack");
        canMove = true;
    }

    void Attack1()
    {
        if (noOfClicks >= 2)
        {
            animator.SetTrigger("Attack2");
            Debug.Log(2);
        }
        else
        {
            EndCombo();
        }
    }

    void Attack2()
    {
        if (noOfClicks >= 3)
        {
            animator.SetTrigger("Attack3");
            Debug.Log(3);
        }
        else
        {
            EndCombo();
        }
    }

    void Attack3()
    {
        EndCombo();
    }

    private void OnTriggerStay(Collider other) 
    {
        if(other.tag == "Torch")
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                UnEquipWeapon(Weapon);
                Equip(other.gameObject);
                animator.runtimeAnimatorController = animControllers[1];
                hasTorch = true;
                
            }
        }
    }

}
