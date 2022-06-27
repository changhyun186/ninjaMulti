using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AimWeightController : MonoBehaviour
{
    [SerializeField]
    Rig rig;
    MyPlayer myPlayer;
    Animator animator;
    [SerializeField]
    float weight;

    public void SetWeight(float amount)
    {
        print(amount);
        weight = amount;
    }
    // Start is called before the first frame update
    void Start()
    {
        weight = 1;
        myPlayer = GetComponentInParent<MyPlayer>();
        animator = myPlayer.animationController.animator;
    }

    // Update is called once per frame
    void Update()
    {
        rig.weight = weight;
    }
}
