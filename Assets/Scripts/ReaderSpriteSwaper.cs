using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReaderSpriteSwaper : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController[] animatorControllers;
    [SerializeField] private Animator animator;
    private void Start()
    {
        int randReader = (int)Random.Range(0f, animatorControllers.Length);
        animator.runtimeAnimatorController = animatorControllers[randReader];
    }

}
