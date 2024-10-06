using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum InputMode { UNSET = 0, MOVE = 1, START_DRAW_WEB = 2, FINISH_DRAW_WEB = 3, REMOVE_WEB = 4 };

    public InputMode inputMode = InputMode.START_DRAW_WEB;

    Camera viewCamera;

    [SerializeField]
    SpiderD spiderD;

    [SerializeField]
    BounceWeb bounceWebPrefab;

    BounceWeb activePlacementWeb = null;

    [SerializeField]
    float distTreshold = 0.5f;

    void SetInputMode(InputMode mode)
    {
        inputMode = mode;
    }

    // Start is called before the first frame update
    void Start()
    {
        viewCamera = GameObject.FindAnyObjectByType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        mousePos = Input.mousePosition;
        worldPos = viewCamera.ScreenToWorldPoint(mousePos);
        // UpdateSpiderD(dt);

        switch (this.inputMode) {
            case InputMode.UNSET:
                UpdateUnset(dt);
                break;
            case InputMode.MOVE:
                // TODO
                break;
            case InputMode.START_DRAW_WEB:
                UpdateStartDrawWeb(dt);
                break;
            case InputMode.FINISH_DRAW_WEB:
                UpdateEndDrawWeb(dt);
                break;
        }
    }

    Vector3 mousePos;
    Vector3 worldPos;
    
    void UpdateUnset(float dt)
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SetInputMode(InputMode.START_DRAW_WEB);
        }
    }

    void UpdateStartDrawWeb(float dt)
    {
        if (Input.GetMouseButtonDown(0)) {
            activePlacementWeb = GameObject.Instantiate<BounceWeb>(bounceWebPrefab);
            spiderD.SetTargetPos(worldPos);
            activePlacementWeb.SetPointA(worldPos.x, worldPos.y);
            activePlacementWeb.SetPointB(worldPos.x, worldPos.y);
            SetInputMode(InputMode.FINISH_DRAW_WEB);
        }
    }

    void UpdateEndDrawWeb(float dt)
    {
        activePlacementWeb.SetPointB(worldPos.x, worldPos.y);
        spiderD.SetTargetPos(worldPos);

        if (activePlacementWeb.GetDist() >= distTreshold && Input.GetMouseButtonUp(0)) {
            SetInputMode(InputMode.START_DRAW_WEB);
            activePlacementWeb = null;
        } else if (Input.GetMouseButtonDown(0)) {
            SetInputMode(InputMode.START_DRAW_WEB);
            activePlacementWeb = null;
        } else if (Input.GetMouseButtonDown(1)) {
            activePlacementWeb.gameObject.SetActive(false);
            GameObject.Destroy(activePlacementWeb);
            activePlacementWeb = null;
            SetInputMode(InputMode.START_DRAW_WEB);
        }
    }
}
