using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExposionObject : MonoBehaviour
{
    private Sprite[] _spritesArray;
    private int _spriteIndex;
    private float _currentAnimationTime;
    private float _maxAnimationTime = 0.05f;
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;
    private bool _playAnimation = false;

    public bool StartAnimatinFlag {  get { return _playAnimation; } set { _playAnimation = value; } }


   

    public void Initialize(Sprite[] SpriteArray, AudioClip clip)
    {
        _spritesArray = SpriteArray;
        _spriteIndex = 0;
        _currentAnimationTime = 0;

        transform.localScale = Vector3.one * 1.5f;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();

        _spriteRenderer.sprite = _spritesArray[0];
        _audioSource.clip = clip;

        _maxAnimationTime = 0.05f;

        _playAnimation = false;

        gameObject.SetActive(false); 
    }

    public bool UpdateScript()
    {
        bool result = false;

        if (_playAnimation) 
        {
           result = SpriteAnimationProcess();
        }

        return result;
    }

    public void BeginEffect(Vector3 position)
    {
        _spriteIndex = 0;
        _currentAnimationTime = 0;
        _spriteRenderer.sprite = _spritesArray[0];
        transform.position = position;
        gameObject.SetActive(true);
        _playAnimation = true;
        _maxAnimationTime = 0.05f;
        //_audioSource.pitch = Random.Range(-2.0f, 2.0f);
        _audioSource.Play();
    }

    public void PrimeEffect(Vector3 position)
    {
        transform.position = position;
        _spriteIndex = 0;
        _currentAnimationTime = 0;
        _maxAnimationTime = 0.02f;
        _spriteRenderer.sprite = _spritesArray[0];
        _playAnimation = false;
     
    }

    public void BeginBigVersion(Vector3 position)
    {
        transform.position = position;
        transform.localScale = Vector3.one * 5;
        _spriteIndex = 0;
        _currentAnimationTime = 0;
        _maxAnimationTime = 0.1f;

        gameObject.SetActive(true);


        _audioSource.pitch = 1;

        _spriteRenderer.sprite = _spritesArray[0];
        _playAnimation = true;

        _audioSource.Play();
    }

    public void PlaySoundEffect()
    {
        gameObject.SetActive(true);
        _audioSource.Play();
    }

    private bool SpriteAnimationProcess()
    {
        bool result = false;

        if (_currentAnimationTime == _maxAnimationTime)
        {
            _currentAnimationTime = 0;
            _spriteIndex++;

            if (_spriteIndex >= _spritesArray.Length)
            {
                _spriteIndex = 0; 

                result = true;
            }

            _spriteRenderer.sprite = _spritesArray[_spriteIndex];

        }
        else
        {
            _currentAnimationTime += Time.deltaTime;
            if (_currentAnimationTime > _maxAnimationTime)
            {
                _currentAnimationTime = _maxAnimationTime;
            }
        }

        return result;
    }

}
