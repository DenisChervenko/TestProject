using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 _originalPosition;
    private CanvasGroup _canvasGroup;

    [Inject] private EventManager _eventManager;

    void Awake() => _canvasGroup = GetComponent<CanvasGroup>();
        

    public void OnBeginDrag(PointerEventData eventData)
    {
        _originalPosition = transform.position;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) => transform.position = Input.mousePosition;

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        GameObject dropZone = eventData.pointerEnter;

        if (dropZone != null && dropZone.CompareTag("InventorySlot"))
        {
            transform.position = dropZone.transform.position;
            dropZone.transform.position = _originalPosition;
            _eventManager.onArrayChanged.Invoke(gameObject.transform.GetSiblingIndex(), dropZone.transform.GetSiblingIndex());
            return;
        }

        transform.position = _originalPosition;
    }
}