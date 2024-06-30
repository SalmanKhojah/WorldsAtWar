using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmYoungOmarAnimationEvent : MonoBehaviour
{
  private Animator _youngOmarGFXAnimator;
 public void Initialize()
 {
    _youngOmarGFXAnimator = GetComponent<Animator>();
 }

public void ResetScytheAnimationLayerWeight()
{
    _youngOmarGFXAnimator.SetLayerWeight(2, 0);
}
}
