using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DragItemObject : MonoBehaviour
{
   [SerializeField] private Transform originParent;
   public Transform OriginParent
   {
      set => originParent = value;
      get => originParent;
   }
   [SerializeField] private Collider2D collider;
   public Collider2D Collider2D => collider;
   [SerializeField] private SortingGroup sort;
   
#if UNITY_EDITOR
        
   [ContextMenu("Get Component")]
   public void GetComponent()
   {
      Debug.LogError(name);
      if (originParent == null) originParent = transform.parent;
      collider = GetComponent<Collider2D>();
      sort = GetComponent<SortingGroup>();
   }
#endif
   public void OnSelected()
   {
      originParent = transform.parent;
      collider.enabled = false;
      sort.sortingOrder = 100;
   }

   public void OnDrag(Vector3 position)
   {
      position.z = 0;
      transform.SetParent(transform.root);
      transform.position = position;
   }
   public void OnDrop()
   {
      transform.SetParent(originParent);
      transform.localPosition = Vector3.zero;
      collider.enabled = true;
      sort.sortingOrder = 10;
   }
}
