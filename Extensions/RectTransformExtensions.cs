using UnityEngine;

namespace SnUnityCommonUtils.Extensions
{
    public static class RectTransformExtensions
    {
        public static void SetLeft(this RectTransform rt, float left)
        {
            rt.offsetMin = new Vector2(left, rt.offsetMin.y);
        }

        public static void SetRight(this RectTransform rt, float right)
        {
            rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
        }

        public static void SetTop(this RectTransform rt, float top)
        {
            rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
        }

        public static void SetBottom(this RectTransform rt, float bottom)
        {
            rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
        }

        public static void SetHeight(this RectTransform rt, float height)
        {
            rt.sizeDelta = rt.sizeDelta.SetY(height);
        }
        
        public static void SetX(this RectTransform rt, float x)
        {
            rt.anchoredPosition = rt.anchoredPosition.SetX(x);
        }

        public static float GetX(this RectTransform rt)
        {
            return rt.anchoredPosition.x;
        }
        
        public static void SetY(this RectTransform rt, float y)
        {
            rt.anchoredPosition = rt.anchoredPosition.SetY(y);
        }

        public static float GetY(this RectTransform rt)
        {
            return rt.anchoredPosition.y;
        }
    }
}