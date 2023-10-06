
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.VisualScripting;
using Google.Play.Review;


public class RateController : MonoBehaviour
{

    public static RateController instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }

    public static RateController Instance { get { return instance; } }

    private ReviewManager _reviewManager;
    private PlayReviewInfo _playReviewInfo;
    private Coroutine _coroutine;

    /*public void ShowRate(UnityAction<bool> callback)
    {
        StartCoroutine(HandleShowRate(callback));
    }
    IEnumerator HandleShowRate(UnityAction<bool> callback)
    {
        ReviewManager _reviewManager = new ReviewManager();
        Debug.Log("Google Interface called show up");
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            Debug.Log(requestFlowOperation.Error.ToString());
            callback?.Invoke(false);
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        var _playReviewInfo = requestFlowOperation.GetResult();
        
        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;
        _playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            Debug.Log(requestFlowOperation.Error.ToString());
            // Log error. For example, using requestFlowOperation.Error.ToString().
            callback?.Invoke(false);
            yield break;
        }
        callback?.Invoke(true);
        Debug.Log("Fnished flow");
        // The flow has finished. The API does not indicate whether the user
        // reviewed or not, or even whether the review dialog was shown. Thus, no
        // matter the result, we continue our app flow.
    }*/
    private void Start()
    {
        _coroutine = StartCoroutine(InitReview());

    }

    public void RateAndReview()
    {
        StartCoroutine(LaunchReview());
    }
    private IEnumerator InitReview(bool force = false)
    {
        if (_reviewManager == null) _reviewManager = new ReviewManager();

        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            Debug.Log(requestFlowOperation.Error.ToString());
            if (force) DirectlyOpen();
            yield break;
        }

        _playReviewInfo = requestFlowOperation.GetResult();
    }

    public IEnumerator LaunchReview()
    {
        if (_playReviewInfo == null)
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
            yield return StartCoroutine(InitReview(true));
        }

        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;
        _playReviewInfo = null;
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            Debug.Log(launchFlowOperation.Error.ToString());
            DirectlyOpen();
            yield break;
        }
    }
    private void DirectlyOpen() { Application.OpenURL($"https://play.google.com/store/apps/details?id={Application.identifier}"); }
}
