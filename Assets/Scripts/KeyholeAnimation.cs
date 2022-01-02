/**

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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IgnoreSolutions
{
    // [ExecuteInEditMode]
    public class KeyholeAnimation : MonoBehaviour
    {
        [Serializable]
        public struct SizePos
        {
            public Vector2 Size;
            public Vector2 Pos;
        }

        public enum Sides
        {
            Left,
            Right,
            Top,
            Bottom
        }

        [SerializeField] private bool _Enable = true;
        [SerializeField] private float _Speed = 0.4f;
        [SerializeField] private Image _KeyholeImage;
        [SerializeField] private SizePos PointA = new SizePos(){Size = new Vector2(12508, 14433), Pos = new Vector2(-5293.748f, -6725.214f)}, PointB = new SizePos(){Size = new Vector2(2, 2), Pos = new Vector2(959.5582f, 489.7585f)};
        private Coroutine _CurrentCoroutine = null;
        private RectTransform[] _SideRects = new RectTransform[(int)Sides.Bottom + 1];

        private void SetRectSizePosBySide(RectTransform sideRect, Sides side)
        {
            // 
            Vector2 MaskPos = _KeyholeImage.rectTransform.anchoredPosition;

            /* RENDERED SIZE */
            Vector2 MaskSize = _KeyholeImage.rectTransform.sizeDelta;
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

        private void UpdateRects()
        {
            for (int i = 0; i < _SideRects.Length; i++)
            {
                if (_SideRects[i] != null)
                    SetRectSizePosBySide(_SideRects[i], (Sides)i);
            }
        }

        IEnumerator LerpSizePos()
        {
            // Fade in 
            for(float f = 0; f <= 1.0f; f += _Speed * Time.fixedDeltaTime)
            {
                _KeyholeImage.rectTransform.sizeDelta = Vector2.Lerp(PointA.Size, PointB.Size, f);
                _KeyholeImage.rectTransform.anchoredPosition = Vector2.Lerp(PointA.Pos, PointB.Pos, f);
                UpdateRects();
                yield return null;   
            }
            _KeyholeImage.rectTransform.sizeDelta = PointB.Size;
            _KeyholeImage.rectTransform.anchoredPosition = PointB.Pos;
            UpdateRects();

            // Wait
            yield return new WaitForSeconds(2f);

            // Fade Out
            for(float f = 0.0f; f <= 1.0f; f += _Speed * Time.fixedDeltaTime)
            {
                _KeyholeImage.rectTransform.sizeDelta = Vector2.Lerp(PointB.Size, PointA.Size, f);
                _KeyholeImage.rectTransform.anchoredPosition = Vector2.Lerp(PointB.Pos, PointA.Pos, f);
                UpdateRects();
                yield return null;   
            }
            _KeyholeImage.rectTransform.sizeDelta = PointA.Size;
            _KeyholeImage.rectTransform.anchoredPosition = PointA.Pos;
            UpdateRects();

            yield return new WaitForSeconds(2f);
            _CurrentCoroutine = null;
        }


        private void OnEnable()
        {
            // Make 4 empty Unity.UI Images. No Sprite attached, just black.
            // Only makes them if the reference we have at a given index is null.
            for(int i = 0; i < _SideRects.Length; i++)
            {
                if(_SideRects[i] != null) continue;

                GameObject newSideRect = new GameObject("RectSide_" + (Sides)i, typeof(Image));

                // You can uncomment this code to get an outline around the object.
                // Good for visualization purposes.           
                // Outline newOutline = newSideRect.AddComponent<Outline>();
                // newOutline.effectDistance = Vector2.one * 4;
                // newOutline.effectColor = Color.white;

                Image img = newSideRect.GetComponent<Image>();
                img.color = Color.black;
                _SideRects[i] = img.rectTransform;
                _SideRects[i].parent = transform.parent;
                _SideRects[i].anchorMin = Vector2.zero;
                _SideRects[i].anchorMax = Vector2.zero;
                _SideRects[i].pivot = Vector2.zero;

                SetRectSizePosBySide(_SideRects[i], (Sides)i);
            }
        }

        public void Update()
        {
            // Create a continually transitioning state.
            // If you just want this to update every frame so you can 
            // control it by another script or by Animation Controller,
            // you can remove the 2nd if statement and just call "UpdateRects();" every frame.
            if(_Enable)
                if(_CurrentCoroutine == null)
                    _CurrentCoroutine = StartCoroutine(LerpSizePos());
        }
    }
}