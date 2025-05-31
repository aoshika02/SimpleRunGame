using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerAnimController : SingletonMonoBehaviour<PlayerAnimController>
{
    [SerializeField] private Animator _animator;
    public void AnimJump(bool isJump) 
    {
        AnimationCall(ParamConsts.JUMP, isJump);
    }

    public void AnimStan(bool isStan)
    {
        AnimationCall(ParamConsts.STAN, isStan);
    }

    private void AnimationCall(string animParam,bool animFlag) 
    {
        _animator.SetBool(animParam, animFlag);
    }
    public float GetAnimationClipLength(string name,float speed=1)
    {
        if (_animator == null || _animator.runtimeAnimatorController == null)
            return -1f;

        foreach (var clip in _animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == name)
            {
                return (float)(clip.length/speed);
            }
        }

        return -1f; // Not found
    }
}
