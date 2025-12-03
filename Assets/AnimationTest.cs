using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    public float SpeedOffset;
    Animator _animator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("Speed", SpeedOffset);
    }
}
