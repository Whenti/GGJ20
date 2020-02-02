using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Illuminated : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject illumination;
    [SerializeField] GameObject sound;

    bool is_active;

    public void MySetActive(bool b)
    {
        illumination.gameObject.SetActive(false);
        gameObject.SetActive(b);
        is_active = b;
    }

    public bool MyIsActive()
    {
        return is_active;
    }

    private void Start()
    {
        MySetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        illumination.SetActive(true);
        sound.GetComponent<AudioSource>().Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        illumination.SetActive(false);
    }
}
