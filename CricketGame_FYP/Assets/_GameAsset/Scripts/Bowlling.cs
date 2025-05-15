using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Bowlling : MonoBehaviour
{
    public Transform ball;
    public Transform startPoint;
    public Transform bouncePoint;
    public Transform targetPoint;
    public Transform ballPitchingPoint;

    Vector3 InitalPostion;

     float toBounceTime = 0.3f;
     float toBatsmanTime = 0.3f;
    public float lateralOffsetAmount = 0.05f;

    public Slider accuracySlider;
    public float speedSlider = 1f;    // Speed of the tween


    bool inBowlingState = false;
    bool isAiBowl = false;


    private void OnEnable()
    {
        InitalPostion = transform.position;
    }

    public void InBowlingState()
    {
        speedSlider = Random.Range(1f, 5f);
        inBowlingState = true;
        accuracySlider.value = 0;
    }

    void Update()
    {
        if (inBowlingState)
        {
            float t = Mathf.PingPong(Time.time * speedSlider, 1f);
            accuracySlider.value = t;
        }
    }





    public void Bowl(float lateralOffset)
    {
        if(!isAiBowl)
            GameManager.instance.showAccuracy(accuracySlider.value);

        BowlProcess(lateralOffset);

    }
    public void BowlProcess(float lateralOffset)
    {
        var currentBall = Instantiate(ball, startPoint.position, Quaternion.identity);

        // Apply lateral offset to target
        Vector3 offsetTarget = targetPoint.position + (Vector3.right * lateralOffset);
        //Vector3 offsetBounce = bouncePoint.position + (Vector3.right * (lateralOffset * 0.5f)); // optional: less offset on bounce


        // Tween to bounce point (using straight path for now)
        currentBall.transform.DOMove(bouncePoint.position, toBounceTime)/*.SetEase(Ease.OutQuad)*/.OnComplete(() => {

            if (!isAiBowl)
            {
                GameManager.instance.MakePlayerDoAnimaiton(accuracySlider.value);
            }
            else
            {
                FindAnyObjectByType<Batting>().DoAnimation();
                GameManager.instance.MakeSureActionDone();
            }

            // Tween to target
            currentBall.transform.DOMove(offsetTarget, toBatsmanTime)/*.SetEase(Ease.InQuad)*/.OnComplete(() => {

                Debug.Log("Tween Complet");
                // Tween to target
                GameManager.instance.Bowled(accuracySlider.value, currentBall, isAiBowl);/*.SetEase(Ease.InQuad)*/

                Invoke("ResetPlayer", 1f);
            });
        });

        

    }

    void ResetPlayer()
    {
        transform.DOMove(InitalPostion, 1f);
    }



    // Call this from buttons with directional flag
    public void BowlWithDirection(bool isLeft)
    {
        inBowlingState = false;
        isAiBowl = false;
        ProcessBowlReques(isLeft, false);
    }

    private void ProcessBowlReques(bool isLeft, bool isAi)
    {
        if(!isAi)
            UiManager.instance.ShowBowlingUI(false);

        float offset = (!isLeft) ? -lateralOffsetAmount : lateralOffsetAmount;

        GetComponent<BowlerAnimation>().BowlingAnimation();

        transform.DOMove(ballPitchingPoint.position, .8f).OnComplete(() =>
        {
            Bowl(offset);
        });
    }

    public void AiBowl()
    {
        isAiBowl = true;
        bool direation = Random.Range(0, 2) == 0;
        ProcessBowlReques(direation, isAiBowl);
    }
}
