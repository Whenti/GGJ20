using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Illuminated : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject illumination;

    private void Start()
    {
        illumination.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        illumination.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        illumination.SetActive(false);
    }
}
