using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] GameObject mainBall = null;
    [SerializeField] float power = 0.1f;
    [SerializeField] Transform arrow = null;
    [SerializeField] List<ColorBall> ballList = new List<ColorBall>();
    Vector3 mousePosition = new Vector3();
    Rigidbody mainRigid = null;
    Vector3 mainBallDefaultPosition = new Vector3();
    #if Android
        Vector3 startTouch = new Vector3();
    #endif
    void Start()
    {
        mainRigid = mainBall.GetComponent<Rigidbody>();
        mainBallDefaultPosition = mainBall.transform.localPosition;
        arrow.gameObject.SetActive(false);
    }

    void Update()
    {
        if(mainBall.activeSelf == true){
            #if Android
                if(Input.touchCount > 0)
                {
                    Touch touch = input.GetTouch(0);
                    if(touch.phase == TouchPhase.Began)
                    {
                        Debug.Log("タッチ開始");
                        startTouch = touch.Position;
                        arrow.gameObject.SetActive(true);
                    }
                    else if(touch.phase == TouchPhase.Moved || touch.phase ==TouchPhase.Stationary)
                    {
                        Debug.Log("タッチ中");
                        Vector3 position = touch.position;

                        Vector3 def = startTouch - position;
                        float rad = Mathf.Atan2(def.x, def.y);
                        float angle = rad * Mathf.Rad2Deg;
                        Vector3 rot = new Vector3(0, angle, 0);
                        Quaternion qua = Quaternion.Euler(rot);

                        arrow.localRotation = qua;
                        arrow.transform.position = mainBall.transform.position;
                    }
                    else if(touch.phase == TouchPhase.Ended)
                    {
                        Debug.Log("タッチ終了");
                        Vector3 upPosition = touch.position;
                        Vector3 def = startTouch - upPosition;
                        Vector3 add = new Vector3(def.x, 0, def.y);

                        mainRigid.AddForce(add * power);

                        arrow.gameObject.SetActive(false);

                        Debug.Log("クリック終了");
                    }
                }
            #else
                if(Input.GetMouseButtonDown(0) == true)
                {
                    mousePosition = Input.mousePosition;
                    arrow.gameObject.SetActive(true);
                    Debug.Log("クリック開始");
                }

                if(Input.GetMouseButton(0) == true)
                {
                    Vector3 position = Input.mousePosition;
                    Vector3 def = mousePosition - position;
                    float rad = Mathf.Atan2( def.x, def.y);
                    float angle = rad * Mathf.Rad2Deg;
                    Vector3 rot = new Vector3(0, angle, 0);
                    Quaternion qua = Quaternion.Euler(rot);

                    arrow.localRotation = qua;
                    arrow.transform.position = mainBall.transform.position;
                }

                if(Input.GetMouseButtonUp(0) == true)
                {
                    Vector3 upPosition = Input.mousePosition;

                    Vector3 def = mousePosition - upPosition;
                    Vector3 add = new Vector3(def.x, 0, def.y);

                    mainRigid.AddForce(add * power);

                    arrow.gameObject.SetActive(false);

                    Debug.Log("クリック終了");
                }
            #endif
        }
        else
        {
            Debug.Log("メインボールがアクティブではないためリセットします．");
            OnResetButtonClicked();
        }
    }
    public void OnResetButtonClicked()
    {
        mainBall.SetActive(true);
        mainRigid.velocity = Vector3.zero;
        mainRigid.angularVelocity = Vector3.zero;
        mainBall.transform.localPosition = mainBallDefaultPosition;
        foreach(ColorBall ball in ballList)
        {
            ball.Reset();
        }
    }
}
