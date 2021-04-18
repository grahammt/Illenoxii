using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownIndicators : MonoBehaviour
{
    Subscription<MoveUsed> move_event_subcription;
    public int moveTrigger;
    private Coroutine timer;

    // Start is called before the first frame update
    void Start()
    {
        move_event_subcription = EventBus.Subscribe<MoveUsed>(_OnMoveUsed);
    }

    void _OnMoveUsed(MoveUsed e){
        if (e.move == moveTrigger){
            if (timer!=null){
                StopCoroutine(timer);
            }
            timer=StartCoroutine(Timer(e.cooldown));
        }
    }

    IEnumerator Timer(float cooldown){
        float initial_time = Time.time;
        float progress = (Time.time - initial_time)/cooldown;
        while (progress < 1.0f){
            progress = (Time.time - initial_time)/cooldown;
            GetComponent<Image>().fillAmount = progress;
            yield return null;
        }
        GetComponent<Image>().fillAmount = 1;
    }

    private void OnDestroy(){
        EventBus.Unsubscribe(move_event_subcription);
    }
}
