using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//[System.Serializable]


//public class EventVector3 : UnityEvent<Vector3> { }

public class MouseManager : MonoBehaviour
{
    RaycastHit hit;
    public event Action<Vector3> onMouseClicked;
    public event Action<GameObject> onEnemyClicked;
    public static MouseManager instance;
    public Texture2D arrow, target, attack, doorway, point;



    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }


    void Update()
    {
        SetCursorTexture();
        MouseControl();
    }

    void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            // 切换鼠标贴图
            switch (hit.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Enemy":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
    }

    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0) && hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Ground"))
                onMouseClicked?.Invoke(hit.point);
            if (hit.collider.gameObject.CompareTag("Enemy"))
                onEnemyClicked?.Invoke(hit.collider.gameObject);

        }
    }

}


