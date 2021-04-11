﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasArmor : MonoBehaviour
{
    public int stunNeeded = 25;
    public double currentStun = 0;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine("stunreset");
    }

    void Update()
    {
        if(!PausedGameManager.is_paused) {
            if (currentStun > stunNeeded)
            {
                if (animator)
                    animator.SetBool("Dazed", true);
                gameObject.GetComponent<platformerPathfinding>().dazed = true;
            }
            else
            {
                gameObject.GetComponent<platformerPathfinding>().dazed = false;
            }
            Debug.Log("stun val: " + currentStun);
        }
    }
    public bool isStunned()
    {
        return currentStun >= stunNeeded;
    }
    IEnumerator stunreset()
    {
        while (true)
        {
            if (currentStun > 0)
            {
                if (GetComponent<platformerPathfinding>())
                {
                    if (GetComponent<platformerPathfinding>().onGround())
                    {
                        if (currentStun > 30)
                        {
                            currentStun -= currentStun * 0.02f;
                        }
                        else
                        {
                            if (currentStun > 20)
                            {
                                currentStun -= currentStun * 0.01f;
                            }
                            else
                            {
                                currentStun -= 0.2f;
                            }

                        }
                    }
                    else
                    {
                        if (currentStun > 30)
                        {
                            currentStun -= currentStun * 0.01f;
                        }
                        else
                        {
                            if (currentStun > 20)
                            {
                                currentStun -= currentStun * 0.005f;
                            }
                            else
                            {
                                currentStun -= 0.1f;
                            }

                        }
                    }
                }

                else
                {
                    if (currentStun > 30)
                    {
                        currentStun -= currentStun * 0.01f;
                    }
                    else
                    {
                        if (currentStun > 20)
                        {
                            currentStun -= currentStun * 0.005f;
                            if (stunNeeded < 5)
                            {
                                currentStun += currentStun*0.0025f;
                            }
                        }
                        else
                        {
                            currentStun -= 0.1f;
                            if (stunNeeded < 5)
                            {
                                currentStun += 0.05f;
                            }
                        }

                    }
                }

            }
                yield return null;
        }
    }

    public void AddDamage(float dmg)
    {
     currentStun += dmg;
    }
}
