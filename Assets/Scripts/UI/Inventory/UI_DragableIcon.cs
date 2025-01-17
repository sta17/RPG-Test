using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_DragableIcon : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Icon and Slot")]
    [SerializeField] private Image iconimage;
    [SerializeField] public UI_Slot_Item slot;

    [Header("Moveable Icon")]
    [SerializeField] private Vector2 HomePosition;
    [SerializeField] public Image moveable_icon;
    [SerializeField] public GameObject moveable_icon_gameObject;
    [SerializeField] public RectTransform moveable_icon_rectTransform;

    [Header("TooltipPopup")]
    [SerializeField] public UI_TooltipPopup tooltipPopup;
    [SerializeField] private Item item;

    private void Awake()
    {
        // get moving icon rectTransform instead.
        //moveable_icon_rectTransform = moveable_icon_gameObject.GetComponent<RectTransform>();

        item = null;

        iconimage = GetComponent<Image>();

        //remove or set position to 0,0,0
        HomePosition = GetComponent<RectTransform>().position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (iconimage.sprite != null)
        {
            moveable_icon.sprite = iconimage.sprite;
            moveable_icon_rectTransform.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            moveable_icon_rectTransform.position = GetComponent<RectTransform>().position;
            moveable_icon_gameObject.SetActive(true);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (iconimage.sprite != null)
        {
            moveable_icon_rectTransform.position += (Vector3)eventData.delta;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        moveable_icon_rectTransform.anchoredPosition = HomePosition;
        moveable_icon_gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            slot.OnDrop(eventData);
        }
    }

    public void SetDisplayItem(Item newItem)
    {
        item = newItem;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null && tooltipPopup != null)
        {
            tooltipPopup.DisplayInfo(item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipPopup != null)
        {
            tooltipPopup.HideInfo();
        }
    }
}