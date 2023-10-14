using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum State
    {
        All, Focus_On_Target, Roaming
    }
    public Camera _camera;
    private float focusSize;
    private float allSize;
    public State CurrentState;

    public Vector2 YBound;
    public Vector2 XBound;
    public Transform currentTarget;

    public Transform P1;
    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }
    void Start()
    {
        CurrentState = State.All;
        allSize = _camera.orthographicSize;
        float camWidth = _camera.aspect * _camera.orthographicSize;
        float camHeight = _camera.orthographicSize;
        XBound = new Vector2(-camWidth, camWidth);
        YBound = new Vector2(-camHeight, camHeight);
        ResetCamera();

    }
    public void ResetCamera()
    {
        CurrentState = State.All;
        _camera.DOOrthoSize(allSize, 1f);
        _camera.transform.DOMove(new Vector3(0, 0, -10), 1f);
    }
    public void SetTarget(Transform target)
    {
        currentTarget = target;
        if (CurrentState == State.Focus_On_Target)
        {
            _camera.DOOrthoSize(allSize, .5f);
            _camera.transform.DOMove(new Vector3(0, 0, -10), .5f)
                .OnComplete(
                () => SetTarget()
                );
        }
        else
        {
            SetTarget();
        }
       
    }

    public void SetTarget()
    {
        CurrentState = State.Focus_On_Target;
        float monsterWidth = 3.4f;
        float camOrthoSize = monsterWidth / _camera.aspect / 2;
        _camera.DOOrthoSize(camOrthoSize, 1f);

        float newCamX = currentTarget.position.x;
        float newCamY = currentTarget.position.y;

        newCamX = Mathf.Clamp(newCamX, XBound.x + monsterWidth / 2, XBound.y - monsterWidth / 2);
        newCamY = Mathf.Clamp(newCamY, YBound.x + camOrthoSize / 2, YBound.y - camOrthoSize / 2);
        Debug.Log(camOrthoSize);
        _camera.transform.DOMove(new Vector3(newCamX, newCamY + 1.5f, -10), 1f);

    }

    public void Roaming()
    {

    }
}
