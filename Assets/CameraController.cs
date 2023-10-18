using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;

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

    private Vector2 YBound;
    private Vector2 XBound;
    public Transform currentTarget;

    public Transform P1;
    public Transform P2;
    float elapsedTime = 0;
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
        Roaming();

    }
    public void ResetCamera()
    {
        if (CurrentState == State.All)
            return;
        CurrentState = State.All;
        _camera.DOOrthoSize(allSize, 1f);
        _camera.transform.DOMove(new Vector3(0, 0, -10), 1f);
    }
    public void SetTarget(Transform target)
    {
        elapsedTime = 0;
        if (CurrentState == State.Focus_On_Target && currentTarget == target)
            return;

        if (CurrentState == State.All)
        {
            CurrentState = State.Focus_On_Target;
            currentTarget = target;
            SetTarget();
        }
        else
        {
            CurrentState = State.Focus_On_Target;
            currentTarget = target;
            _camera.DOOrthoSize(allSize, .5f);
            _camera.transform.DOMove(new Vector3(0, 0, -10), .5f)
                .OnComplete(
                () => SetTarget()
                );
        }



    }

    public void SetTarget()
    {

        float monsterWidth = 4f;
        float camOrthoSize = monsterWidth / _camera.aspect / 2;
        _camera.DOOrthoSize(camOrthoSize, 1f);

        float newCamX = currentTarget.position.x;
        float newCamY = currentTarget.position.y;

        newCamX = Mathf.Clamp(newCamX, XBound.x + monsterWidth / 2, XBound.y - monsterWidth / 2);
        newCamY = Mathf.Clamp(newCamY, YBound.x + camOrthoSize / 2, YBound.y - camOrthoSize / 2);
        _camera.transform.DOMove(new Vector3(newCamX, newCamY + 1.5f, -10), 1f);

    }

    public void CheckResetCamera()
    {
        if (elapsedTime > 2)
        {
            ResetCamera();
        }
    }

    public void Update()
    {
        if (CurrentState == State.Focus_On_Target)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 2)
            {
                ResetCamera();
            }
        }
    }

    public void Roaming()
    {

    }

    public void Init(UnityEvent<TileRunner.Player> onNoteMissed, UnityEvent<TileRunner.Player> onNoteHit, UnityEvent startGameEvent, UnityEvent stopGameEvent, UnityEvent lastNotePassed)
    {
        lastNotePassed.AddListener(() => OnLastNotePassed());
        onNoteMissed.AddListener((player) => OnNoteMissed(player));
        onNoteHit.AddListener((player) => OnNoteHit(player));
        startGameEvent.AddListener(() => OnGameStart());
        stopGameEvent.AddListener(() => OnGameStop());
    }

    private void OnLastNotePassed()
    {
        ResetCamera();
    }

    private void OnGameStop()
    {
    }

    private void OnGameStart()
    {
        ResetCamera();
    }

    private void OnNoteHit(TileRunner.Player player)
    {
        if (player == TileRunner.Player.P1)
        {
            SetTarget(P1);
        }
        else
        {
            SetTarget(P2);
        }
    }

    private void OnNoteMissed(TileRunner.Player player)
    {
        ResetCamera();
    }
}
