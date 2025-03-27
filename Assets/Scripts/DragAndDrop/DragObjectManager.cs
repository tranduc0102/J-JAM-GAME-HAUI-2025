using System;
using Character;
using UnityEngine;

public class DragObjectManager : MonoBehaviour
{
    public static DragObjectManager Instance;
    private DragItemObject currentItem;
    private DragItemObject nearItem;

    public bool smartDrag = true;
    public bool isDraggable = true;
    public bool canDrag = true;
    private Vector2 initialPostionMouse;
    public Vector2 initialPositionObject;

    
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (GameController.Instance.IsFirstPlay && _Scripts.UI.UIController.Instance.Message.IndexMessage == 7)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnBeginDrag(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                OnDrag(Input.mousePosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnEndDrag(Input.mousePosition);
            }
        }
        if(GameController.Instance.State != StateGame.Playing) return;
        if (Input.GetMouseButtonDown(0))
        {
            OnBeginDrag(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            OnDrag(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnEndDrag(Input.mousePosition);
        }
    }
    
    public void OnBeginDrag(Vector3 position)
    {
        if(!canDrag) return;
        canDrag = false;
        if (isDraggable && Input.GetMouseButtonDown(0))
        {
            if (smartDrag)
            {
                initialPostionMouse = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 newPosition = position;
                newPosition.z = 0f;
                RaycastHit2D hit = Physics2D.Raycast(MouseWorldPosition(newPosition), Vector3.down);
                if (hit && hit.transform.TryGetComponent(out DragItemObject item))
                {
                    currentItem = item;
                    currentItem.OnSelected();
                    initialPositionObject = currentItem.transform.localPosition;
                    PlayerController.Instance.ToggleMovementState(false);
                }
            }

            isDraggable = true;
        }
    }

    public void OnDrag(Vector3 position)
    {
        if(!isDraggable) return;
        if (!currentItem) return;
        transform.SetParent(transform.root);
        Vector3 pos = currentItem.transform.position;
        if (isDraggable)
        {
            if (!smartDrag)
            {
                currentItem.transform.localPosition = (Vector2)MouseWorldPosition(Input.mousePosition);
            }
            else
            {
                currentItem.transform.localPosition =
                    initialPositionObject + (Vector2)MouseWorldPosition(Input.mousePosition) - initialPostionMouse;
            }
        }
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down);
        if (hit)
        {
            if (hit.transform.TryGetComponent(out DragItemObject item))
            {
                SwapItem(currentItem, item, true);
            }
        }
    }

    public void SwapItem(DragItemObject item1, DragItemObject item2, bool auto = false)
    {
        if (auto)
        {
            if (Math.Abs(Vector2.Distance(item1.transform.position, item2.transform.position)) <= 2f)
            {
                Transform tmpParent = item1.OriginParent;
                item1.OriginParent = item2.OriginParent;
                item2.OriginParent = tmpParent;
                item2.Collider2D.enabled = false;
                item2.transform.SetParent(tmpParent);
                item2.transform.localPosition = Vector2.zero;
                item2.Collider2D.enabled = true;
            }
            return;
        }
        Transform obj = item1.OriginParent;
        item1.OriginParent = item2.OriginParent;
        item2.OriginParent = obj;
        item2.transform.SetParent(obj);
        item2.transform.localPosition = Vector2.zero;
    }

    public void OnEndDrag(Vector3 position)
    {
        Vector3 currentPos;
        if (currentItem)
        {
            currentPos = currentItem.transform.position;
        }
        else
        {
            currentPos = position;
        }
        ActionEndDrag(currentPos);
    }

    public void OnPointerUp(Vector3 position)
    {
        ActionEndDrag(position);
    }

    private void ActionEndDrag(Vector3 position)
    {
        isDraggable = true;
        smartDrag = true;
        initialPositionObject = Vector2.zero;
        canDrag = true;
        PlayerController.Instance.ToggleMovementState(true);
        if (currentItem)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(position);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down);
            if (hit)
            {
                if (hit.transform.TryGetComponent(out DragItemObject item))
                {
                    SwapItem(currentItem, item);
                }
            }   
            currentItem.OnDrop();
            currentItem = null;
        }
    }
    private Vector3 MouseWorldPosition(Vector3 position)
    {
        return Camera.main.ScreenToWorldPoint(position);
    }
}