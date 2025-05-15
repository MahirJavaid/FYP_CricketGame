using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Batting : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform hitStartingPoint;
    public Transform hitStraightPoint;
    public Transform hitLeftPoint;
    public Transform hitRightPoint;


    public Transform leftFeilder, rightFeilder, downFeilder, Keeper;

    public Slider accuracySlider;
    public float accuracyThreshold = 0.8f;

     float Time1 = 0.6f;
     float Time2 = 0.4f;

    public bool inbattingState = false;
    public float speedSlider = 1f;    // Speed of the tween

    [ContextMenu("InBattingState")]
    public void InBattingState()
    {
        speedSlider = Random.Range(1f, 5f);
        inbattingState = true;
        accuracySlider.value = 0;
        savedAccuracy = 0;
    }

    void Update()
    {
        if (inbattingState)
        {
            float t = Mathf.PingPong(Time.time * speedSlider, 1f);
            accuracySlider.value = t;
        }
    }

    int savedDirection = 0;
    float savedAccuracy = 0;

    public void HitBall(int direction)
    {
        inbattingState = false;

        savedAccuracy = accuracySlider.value;
       
        savedDirection = direction;

        GameManager.instance.Bat(savedAccuracy);
    }

    float score = 0f;
    bool completeLogic;

    private void ProcessBallHiting(float score, Transform hitTarget, Transform ball)
    {
        completeLogic = true;
        this.score = score;

        if (score == 6)
        {
            // Good hit – curved
            Vector3 midPoint = (ball.transform.position + hitTarget.position) / 2 + Vector3.up * 3f;
            Sequence seq = DOTween.Sequence();
            seq.Append(ball.transform.DOMove(midPoint, Time1).SetEase(Ease.OutQuad));
            seq.Append(ball.transform.DOMove(hitTarget.position, Time2).SetEase(Ease.InQuad));
        }
        else if (score >= 1)
        {
            if (savedDirection == 0)
                hitTarget = rightFeilder;

            else if (savedDirection == 1)
                hitTarget = downFeilder;
            else if (savedDirection == 2)
                        hitTarget = leftFeilder;


            // Okay hit – straight with slight randomness
            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            ball.transform.DOMove(hitTarget.position + randomOffset, Time1);

            hitTarget.GetComponent<Feilder>().PickBallAndThrow(Keeper, Time1, ball);

            completeLogic = false;

        }
        else
        {
            //// Bad hit – OUT
            Vector3 outPosition = ball.transform.position + new Vector3(Random.Range(-1f, 1f), 0f, -2f);
            ball.transform.DOMove(outPosition, 0.3f).SetEase(Ease.InBack).OnComplete(() => {
                ball.gameObject.SetActive(false);
            });
        }

        Invoke("DealyShowScore", 0.5f);

    }

    void DealyShowScore()
    {
        var text = score.ToString();

        if (score == 0) {
            text = "OUT\n New Player";

            UiManager.instance.Fade();

        }
        

        UiManager.instance.ShowScores(text.ToString());

        if(completeLogic)
            GameManager.instance.CompleteLogic();
    }

    internal void HitBall(Transform currentBall, bool isBowling, float accuracy)
    {
        if(isBowling)
        {
            Debug.Log("Batting Hit");

            var hitTarget = hitStartingPoint;
            var direction = Random.Range(0, 3);

            savedDirection = direction;

            if (direction == 0)
                hitTarget = hitLeftPoint;
            else if (direction == 1)
                hitTarget = hitStraightPoint;
            else if (direction == 2)
                hitTarget = hitRightPoint;

            var aiAccuracy = 1f;

            switch (accuracy)
            {
                case float a when a >= 0.85f:
                    aiAccuracy = 0;
                        break;

                case float a when a >= 0.7f:
                    aiAccuracy = Random.Range(0f, 0.4f);
                    break;

                case float a when a >= 0.35f:
                    aiAccuracy = 0.5f;
                    break;

                default:
                    aiAccuracy = Random.Range(0.8f, 0.999f);

                    break;
            }

            ProcessBallHiting(CalculateScore(aiAccuracy), hitTarget, currentBall);
        }
        else
        {
            Transform hitTarget = hitStartingPoint;

            if (savedDirection == 0)
                hitTarget = hitLeftPoint;
            else if (savedDirection == 1)
                hitTarget = hitStraightPoint;
            else if (savedDirection == 2)
                hitTarget = hitRightPoint;

            var score = 1;

            switch (savedAccuracy)
            {
                case float a when a >= 0.85f:
                    score = 6;
                    break;

                case float a when a >= 0.7f:
                    score = 4;
                    break;

                case float a when a >= 0.35f:
                    score = Random.Range(1, 3);
                    break;

                default:
                    score = 0;

                    break;
            }

            ProcessBallHiting(score, hitTarget, currentBall);
        }

    }

    private float CalculateScore(float accuracy)
    {
        var score = 0;

        switch (accuracy)
        {
            case float a when a >= 0.9f:
                score = 6;
                break;

            case float a when a >= 0.8f:
                score = 4;
                break;

            case float a when a >= 0.35f:
                score = Random.Range(1, 4);

                break;

            default:
                score = 0;
                break;
        }

        return score;
    }

    internal void DoAnimation()
    {
        GameManager.instance.MakePlayerDoAnimaiton(savedAccuracy);
    }
}

