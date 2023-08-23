using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour, IPointerClickHandler
{
    public Camera miniMapCam;
    public Camera miniMapCam2;

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 curosr = new Vector2(0, 0);

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RawImage>().rectTransform,
            eventData.pressPosition, eventData.pressEventCamera, out curosr))
        {

            Texture texture = GetComponent<RawImage>().texture;
            Rect rect = GetComponent<RawImage>().rectTransform.rect;

            float coordX = Mathf.Clamp(0, (((curosr.x - rect.x) * texture.width) / rect.width), texture.width);
            float coordY = Mathf.Clamp(0, (((curosr.y - rect.y) * texture.height) / rect.height), texture.height);

            float calX = coordX / texture.width;
            float calY = coordY / texture.height;


            curosr = new Vector2(calX, calY);

            if(eventData.button == PointerEventData.InputButton.Right)
                CastRayToWorld(curosr);

            if (eventData.button == PointerEventData.InputButton.Left)
                CastRayToWorld2(curosr);
        }


    }

    private void CastRayToWorld(Vector2 vec)
    {
        if (miniMapCam.isActiveAndEnabled)
        {

            Ray MapRay = miniMapCam.ScreenPointToRay(new Vector2(vec.x * miniMapCam.pixelWidth,
                vec.y * miniMapCam.pixelHeight));

            RaycastHit miniMapHit;
            if (Physics.Raycast(MapRay, out miniMapHit, Mathf.Infinity))
            {
                Debug.Log("miniMapHit: " + miniMapHit.collider.gameObject);
                GameObject.Find("Game Manager").GetComponent<PlayerControl>().Move(miniMapHit.point);
            }
        }

        else {
            Ray MapRay = miniMapCam2.ScreenPointToRay(new Vector2(vec.x * miniMapCam2.pixelWidth,
                vec.y * miniMapCam2.pixelHeight));

            RaycastHit miniMapHit;
            if (Physics.Raycast(MapRay, out miniMapHit, Mathf.Infinity))
            {
                Debug.Log("miniMapHit: " + miniMapHit.collider.gameObject);
                GameObject.Find("Game Manager").GetComponent<PlayerControl>().Move(miniMapHit.point);
            }
        }

           
        

    }
    private void CastRayToWorld2(Vector2 vec)
    {
        if (miniMapCam.isActiveAndEnabled)
        {
            Ray MapRay = miniMapCam.ScreenPointToRay(new Vector2(vec.x * miniMapCam.pixelWidth,
            vec.y * miniMapCam.pixelHeight));

            RaycastHit miniMapHit;

            if (Physics.Raycast(MapRay, out miniMapHit, Mathf.Infinity))
            {
                Debug.Log("miniMapHit: " + miniMapHit.collider.gameObject);
                GameObject.Find("Camera").transform.position = new Vector3(miniMapHit.point.x - GameObject.Find("Main Camera").transform.localPosition.x
                    , GameObject.Find("Camera").transform.position.y, miniMapHit.point.z - GameObject.Find("Main Camera").transform.localPosition.z - 50);
            }
        }
        else
        {
            Ray MapRay = miniMapCam2.ScreenPointToRay(new Vector2(vec.x * miniMapCam2.pixelWidth,
            vec.y * miniMapCam2.pixelHeight));

            RaycastHit miniMapHit;

            if (Physics.Raycast(MapRay, out miniMapHit, Mathf.Infinity))
            {
                Debug.Log("miniMapHit: " + miniMapHit.collider.gameObject);
                GameObject.Find("Camera").transform.position = new Vector3(miniMapHit.point.x - GameObject.Find("Main Camera").transform.localPosition.x
                    , GameObject.Find("Camera").transform.position.y, miniMapHit.point.z - GameObject.Find("Main Camera").transform.localPosition.z - 50);
            }

        }
    }
}
