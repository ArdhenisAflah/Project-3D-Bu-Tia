using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickToClose : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject panelToClose;

    public void OnPointerClick(PointerEventData eventData)
    {
        panelToClose.SetActive(false);
    }
}
