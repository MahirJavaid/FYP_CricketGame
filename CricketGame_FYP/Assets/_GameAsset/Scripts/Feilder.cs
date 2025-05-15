using DG.Tweening;
using System;
using UnityEngine;

public class Feilder : MonoBehaviour
{
    Transform keeperSaved;
    Transform savedBall;

    internal void PickBallAndThrow(Transform keeper, float time1, Transform ball)
    {
        keeperSaved = keeper;
        this.savedBall = ball;
        GetComponent<Animator>().SetTrigger("PickBall");

        

        Invoke("HideBall", (time1));
    }

    void HideBall()
    {
        savedBall.gameObject.SetActive(false);
        Invoke("ThrowBall", 0.5f);
    }

    void ThrowBall() 
    {
        Vector3 start = transform.position;
        Vector3 end = keeperSaved.position;
        start.y += 1f;
        end.y += 1f;

        savedBall.transform.position = start;

        keeperSaved.GetComponent<Keeper>().TriggerCatch();

        Vector3 midPoint = (start + end) / 2 + (Vector3.up * .5f);
        Sequence seq = DOTween.Sequence();
        seq.Append(savedBall.transform.DOMove(midPoint, .3f).SetEase(Ease.InOutSine));
        seq.Append(savedBall.transform.DOMove(end, .3f).SetEase(Ease.InOutSine)).OnComplete(InformManager);

        savedBall.gameObject.SetActive(true);
        seq.Play();

        //ballSaved.transform.position = start;

        //// Midpoint elevated to create a curve effect
        //Vector3 midPoint = (start + end) / 2f + Vector3.up * 2f;

        //Sequence seq = DOTween.Sequence();
        //seq.Append(ballSaved.transform.DOMove(midPoint, .25f).SetEase(Ease.OutQuad));
        //seq.Append(ballSaved.transform.DOMove(end, .25f).SetEase(Ease.InQuad)).OnComplete(InformManager);

    }

    private void InformManager()
    {
        savedBall.gameObject.SetActive(false);
        GameManager.instance.CompleteLogic();
    }
}
