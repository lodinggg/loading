using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Animator animator;


    public float Impules { get; private set; }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        movv();
        jump();
        fall();
        handsup();
    }
    private void movv()
    {
        if (Input.GetKey(KeyCode.A)) // �������� �ȴ� ���
        {
            animator.SetBool("IsSideMoving", true);
            transform.Translate(-speed * Time.deltaTime, 0, 0); // x y z ��
        }
        else if (Input.GetKey(KeyCode.D)) // ���������� �ȴ� ���
        {
            animator.SetBool("IsSideMoving", true);
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.W)) // ������ �ȴ� ���
        {
            animator.SetBool("IsMoving", true);
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S)) // �ڷΰȴ� ���
        {
            animator.SetBool("IsBack", true);
            transform.Translate(0, 0, -speed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("IsMoving", false);
            animator.SetBool("IsSideMoving", false);
            animator.SetBool("IsBack", false);
        }
    }

    private void jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("IsJump");
        }
    }
    private void fall()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            animator.SetTrigger("IsFall");
        }
    }

    private void handsup()
    {
        if(Input.GetMouseButton(0))
        {
            animator.SetBool("IsHand", true);
        }
        else
        {
            animator.SetBool("IsHand", false);
        }
    }
}
