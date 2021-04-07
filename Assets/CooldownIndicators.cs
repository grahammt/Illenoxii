using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownIndicators : MonoBehaviour
{
    Subscription<MoveUsed> move_event_subcription;
    public int moveTrigger;

    // Start is called before the first frame update
    void Start()
    {
        move_event_subcription = EventBus.Subscribe<MoveUsed>(_OnMoveUsed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void _OnMoveUsed(MoveUsed e){
        if (e.move == moveTrigger){
            StartCoroutine(Timer(e.cooldown));
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
