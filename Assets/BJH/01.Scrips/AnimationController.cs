using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator animator;
    MovePlayer movePlayer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        movePlayer = GetComponent<MovePlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(movePlayer.isMoving)
        {
            animator.SetTrigger("Walk");
        }
    }
}
