using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    public abstract class LoopScrollRect : LoopScrollRectBase, LoopScrollPrefabSource
    {
        private LoopScrollDataSource dataSource = null;
        public GameObject prototypePrefab;
        private Stack<Transform> pool = new Stack<Transform>();
        public void Initialize(LoopScrollDataSource _dataSource)
        {
            // 오브젝트가 활성화 되기 전(Awake가 불려지기 전)에
            // RefreshCell이 작동되는 경우가 종종 있어서 이렇게 만듬
            dataSource = _dataSource;
            prefabSource = this;
        }
        protected override void ProvideData(Transform transform, int index)
        {
            dataSource.ProvideData(transform, index);
        }
        
        protected override RectTransform GetFromTempPool(int itemIdx)
        {
            RectTransform nextItem = null;
            if (deletedItemTypeStart > 0)
            {
                deletedItemTypeStart--;
                nextItem = m_Content.GetChild(0) as RectTransform;
                nextItem.SetSiblingIndex(itemIdx - itemTypeStart + deletedItemTypeStart);
            }
            else if (deletedItemTypeEnd > 0)
            {
                deletedItemTypeEnd--;
                nextItem = m_Content.GetChild(m_Content.childCount - 1) as RectTransform;
                nextItem.SetSiblingIndex(itemIdx - itemTypeStart + deletedItemTypeStart);
            }
            else
            {
                nextItem = prefabSource.GetObject(itemIdx).transform as RectTransform;
                nextItem.transform.SetParent(m_Content, false);
                nextItem.gameObject.SetActive(true);
            }
            ProvideData(nextItem, itemIdx);
            return nextItem;
        }

        protected override void ReturnToTempPool(bool fromStart, int count)
        {
            if (fromStart)
                deletedItemTypeStart += count;
            else
                deletedItemTypeEnd += count;
        }

        protected override void ClearTempPool()
        {
            Debug.Assert(m_Content.childCount >= deletedItemTypeStart + deletedItemTypeEnd);
            if (deletedItemTypeStart > 0)
            {
                for (int i = deletedItemTypeStart - 1; i >= 0; i--)
                {
                    prefabSource.ReturnObject(m_Content.GetChild(i));
                }
                deletedItemTypeStart = 0;
            }
            if (deletedItemTypeEnd > 0)
            {
                int t = m_Content.childCount - deletedItemTypeEnd;
                for (int i = m_Content.childCount - 1; i >= t; i--)
                {
                    prefabSource.ReturnObject(m_Content.GetChild(i));
                }
                deletedItemTypeEnd = 0;
            }
        }

        public GameObject GetObject(int index)
        {
            if (pool.Count == 0)
            {
                return Instantiate(prototypePrefab);
            }
            Transform candidate = pool.Pop();
            candidate.gameObject.SetActive(true);
            return candidate.gameObject;
        }

        public void ReturnObject(Transform trans)
        {
            if (trans == null)
                return;

            // Use `DestroyImmediate` here if you don't need Pool
            trans.gameObject.SetActive(false);
            trans.SetParent(m_Content.parent, false);
            pool.Push(trans);
        }
    }
}