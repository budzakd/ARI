using UnityEngine;
using System.Collections;

public class LeverController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool oneLeverFunction;
    [SerializeField] private bool leverPosition;
    [SerializeField] private bool canUseLever;

    [Header("Lever")]
    [SerializeField] private Animator LeverAnim;
    [SerializeField] private string leverAnimPos1;
    [SerializeField] private string leverAnimPos2;
    private AudioSource LeverAudioSource;

    [Header("Object To Control")]
    [SerializeField] private Animator[] ObjectAnim;
    [SerializeField] private AudioSource ObjectAudioSource;
    [SerializeField] private string objectAnimPos1;
    [SerializeField] private string objectAnimPos2;

    private void Start() 
    {
        LeverAudioSource = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!oneLeverFunction)
        {
            if (canUseLever && FindObjectOfType<GameManager>().touchUseButton && !leverPosition)
            {
                StartCoroutine(ObjectAnimDelay(0.3f, objectAnimPos2));
                UseLever(leverAnimPos2);
                leverPosition = true;
            }
            else if (canUseLever && FindObjectOfType<GameManager>().touchUseButton && leverPosition)
            {
                StartCoroutine(ObjectAnimDelay(0.3f, objectAnimPos1));
                UseLever(leverAnimPos1);
                leverPosition = false;
            }
        }
        else
        {
            if (canUseLever && FindObjectOfType<GameManager>().touchUseButton)
            {
                StartCoroutine(ObjectAnimDelay(0.3f, objectAnimPos2));
                UseLever(leverAnimPos1);
            }
        }
    } 

    private IEnumerator ObjectAnimDelay(float time, string anim)
    {
        yield return new WaitForSeconds(time);
        foreach (Animator a in ObjectAnim) { a.Play(anim); }
        ObjectAudioSource.Play();
    }

    private void UseLever(string anim)
    {
        LeverAnim.Play(anim);
        LeverAudioSource.Play();
        FindObjectOfType<GameManager>().touchUseButton = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canUseLever = true;
            FindObjectOfType<GameManager>().UseButton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canUseLever = false;
            FindObjectOfType<GameManager>().UseButton.SetActive(false);
        }
    }
}