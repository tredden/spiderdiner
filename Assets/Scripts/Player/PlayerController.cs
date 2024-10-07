using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    Camera viewCamera;
    AudioSource audioSource;

    [SerializeField]
    SpiderD spiderD;

    [SerializeField]
    BounceWeb bounceWebPrefab;

    BounceWeb activePlacementWeb = null;
    BounceWeb highlighted = null;
    List<BounceWeb> placedWebs = new List<BounceWeb>();
    int pointMoveMode = -1;

    [SerializeField]
    float addedCursorWidth = 1f;

    [SerializeField]
    float distTreshold = 0.5f;

    [SerializeField]
    AudioClip webPlaceSound;

    [SerializeField]
    AudioClip webCutSound;

    // Start is called before the first frame update
    void Start()
    {
        viewCamera = GameObject.FindAnyObjectByType<Camera>();
        audioSource = this.GetComponent<AudioSource>();
        if (spiderD == null) {
            spiderD = FindObjectOfType<SpiderD>();
        }
        foreach (BounceWeb web in GameObject.FindObjectsOfType<BounceWeb>(true)) {
            placedWebs.Add(web);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        mousePos = Input.mousePosition;
        worldPos = viewCamera.ScreenToWorldPoint(mousePos);
        // UpdateSpiderD(dt);

        if (pointMoveMode == -1) { //no active point
            BounceWeb underMouse = GetWebUnderMouse();
            if (highlighted != underMouse) {
                if (highlighted != null) {
                    highlighted.SetHighlight(false);
                }
                if (underMouse != null) {
                    underMouse.SetHighlight(true);
                }
                highlighted = underMouse;
            }

            if (Input.GetMouseButtonDown(0)) {
                if (underMouse != null) {
                    activePlacementWeb = underMouse;
                    pointMoveMode = underMouse.GetCloserEndpoint(worldPos.x, worldPos.y);
                    audioSource.PlayOneShot(webPlaceSound);
                } else { // make new web
                    activePlacementWeb = GameObject.Instantiate<BounceWeb>(bounceWebPrefab);
                    spiderD.SetTargetPos(worldPos);
                    activePlacementWeb.SetPointA(worldPos.x, worldPos.y);
                    activePlacementWeb.SetPointB(worldPos.x, worldPos.y);
                    audioSource.PlayOneShot(webPlaceSound);
                    pointMoveMode = 1; // move pointB
                }
            }
            if (Input.GetMouseButton(1)) {
                spiderD.SetTargetPos(worldPos);
            }
            if (Input.GetMouseButtonDown(1)) {
                if (underMouse != null) {
                    GameObject.Destroy(underMouse.gameObject);
                    placedWebs.Remove(underMouse);
                    audioSource.PlayOneShot(webCutSound);
                }
            }
        } else { // has active point
            if (pointMoveMode == 0) {
                activePlacementWeb.SetPointA(worldPos.x, worldPos.y);
            }
            if (pointMoveMode == 1) {
                activePlacementWeb.SetPointB(worldPos.x, worldPos.y);
            }
            spiderD.SetTargetPos(worldPos);
            if (activePlacementWeb.GetDist() >= distTreshold && Input.GetMouseButtonUp(0) || Input.GetMouseButtonDown(0)) {
                pointMoveMode = -1;
                placedWebs.Add(activePlacementWeb);
                audioSource.PlayOneShot(webPlaceSound);
                activePlacementWeb = null;
            } else if (Input.GetMouseButtonDown(1)) {
                pointMoveMode = -1;
                activePlacementWeb.gameObject.SetActive(false);
                GameObject.Destroy(activePlacementWeb.gameObject);
                audioSource.PlayOneShot(webCutSound);
                activePlacementWeb = null;
            }
        }
    }

    Vector3 mousePos;
    Vector3 worldPos;

    BounceWeb GetWebUnderMouse()
    {
        foreach (BounceWeb web in placedWebs) {
            if (web.IsPointInRegion(worldPos.x, worldPos.y, addedCursorWidth)) {
                return web;
            }
        }
        return null;
    }


}
