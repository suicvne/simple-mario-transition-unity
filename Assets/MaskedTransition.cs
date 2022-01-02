/**

NOTE: This is the component as created in the video

Copyright (c) 2022 Michael Santiago (admin@ignoresolutions.xyz)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IgnoreSolutions
{
    [ExecuteInEditMode]
    public class MaskedTransition : MonoBehaviour
    {
        public enum Sides
        {
            Left,
            Right,
            Top,
            Bottom
        }
        Image[] SideRects = new Image[((int)Sides.Bottom) + 1];
        [SerializeField] Image MaskImage;

        void OnEnable()
        {
            for(int i = 0; i < SideRects.Length; i++)
            {
                if(SideRects[i] != null) continue;
                GameObject newSideRect = new GameObject("RectSide_" + (Sides)i, typeof(Image));
                newSideRect.transform.parent = MaskImage.transform.parent;

                Image sideRectImg = newSideRect.GetComponent<Image>();
                sideRectImg.color = Color.black;
                sideRectImg.rectTransform.anchorMin = Vector2.zero;
                sideRectImg.rectTransform.anchorMax = Vector2.zero;
                sideRectImg.rectTransform.pivot = Vector2.zero;

                SideRects[i] = sideRectImg;
                SetRectSizePosBySide(SideRects[i].rectTransform, (Sides)i);
            }
        }

        private void SetRectSizePosBySide(RectTransform sideRect, Sides side)
        {
            // 
            Vector2 MaskPos = MaskImage.rectTransform.anchoredPosition;
            /* RENDERED SIZE */Vector2 MaskSize = MaskImage.rectTransform.sizeDelta;
            // 

            switch(side)
            {
                case Sides.Left:
                    sideRect.anchoredPosition = Vector2.zero;
                    sideRect.sizeDelta = new Vector2(MaskPos.x, Screen.height);
                    break;
                case Sides.Right:
                    sideRect.anchoredPosition = new Vector2(MaskPos.x + MaskSize.x, 0);
                    sideRect.sizeDelta = new Vector2(Screen.width - (MaskPos.x + MaskSize.x), Screen.height);
                    break;
                case Sides.Top:
                    sideRect.anchoredPosition = new Vector2(MaskPos.x, MaskPos.y + MaskSize.y);
                    sideRect.sizeDelta = new Vector2(MaskSize.x, Screen.height - (MaskPos.y + MaskSize.y));
                    break;
                case Sides.Bottom:
                    sideRect.anchoredPosition = new Vector2(MaskPos.x, 0);
                    sideRect.sizeDelta = new Vector2(MaskSize.x, MaskPos.y);
                    break;
            }
        }

        void Update()
        {
            for(int i = 0; i < SideRects.Length; i++)
            {
                SetRectSizePosBySide(SideRects[i].rectTransform, (Sides)i);
            }
        }
    }
}