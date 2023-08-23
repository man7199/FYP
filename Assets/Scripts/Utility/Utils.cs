using UnityEngine;

namespace Utils
{
    public static class UtilsClass
    {
        public const int SortingOrderDefault = 5000;

        private static Texture2D _whiteTexture;

        public static Texture2D WhiteTexture
        {
            get
            {
                if (_whiteTexture == null)
                {
                    _whiteTexture = new Texture2D(1, 1);
                    _whiteTexture.SetPixel(0, 0, Color.white);
                    _whiteTexture.Apply();
                }

                return _whiteTexture;
            }
        }

        public static TextMesh CreateWorldText(string text, Vector3 localPosition = default, Transform parent = null,
            int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft,
            TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = SortingOrderDefault)
        {
            if (color == null) color = Color.white;
            var gameObject = new GameObject("World_Text", typeof(TextMesh));
            var transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            transform.Rotate(90, 0, 0);
            var textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = (Color)color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }

        public static void CreateWorldTextPopup(string text, Vector3 localPosition = default, Transform parent = null,
            int fontSize = 40, Color? color = null, float popupTime = 1f)
        {
            var textMesh = CreateWorldText(text, localPosition, parent, fontSize, color, TextAnchor.LowerLeft);
            var popUptext = textMesh.gameObject.AddComponent<PopUpText>();
            popUptext.SetUpTimer(popupTime);
        }


        public static Vector3 GetMouseWorldPosition()
        {
            return GetMouseWorldPosition(Input.mousePosition);
        }

        public static Vector3 GetMouseWorldPosition(Vector2 point)
        {
            LayerMask layerMask = LayerMask.GetMask("Tile");
            var maximumRange = 1000;
            var ray = Camera.main.ScreenPointToRay(point);
            if (Physics.Raycast(ray, out var raycastHit, maximumRange, layerMask))
                return raycastHit.point;
            return Vector3.zero;
        }

        public static void DrawScreenRect(Rect rect, Color color)
        {
            GUI.color = color;
            GUI.DrawTexture(rect, WhiteTexture);
            GUI.color = Color.white;
        }

        public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
        {
            // Top
            DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
            // Left
            DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
            // Right
            DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
            // Bottom
            DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        }

        public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
        {
            // Move origin from bottom left to top left
            screenPosition1.y = Screen.height - screenPosition1.y;
            screenPosition2.y = Screen.height - screenPosition2.y;
            // Calculate corners
            var topLeft = Vector3.Min(screenPosition1, screenPosition2);
            var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
            // Create Rect
            return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
        }
    }
}