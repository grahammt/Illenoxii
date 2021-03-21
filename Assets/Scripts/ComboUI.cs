using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboUI : MonoBehaviour
{
    Subscription<ResetComboEvent> combo_event_subcription;
    Subscription<IncrementCombo> increment_event_subscription;
    int currentCombo = 0;
    public float duration = 3.0f;
    private Coroutine timer;
    // Start is called before the first frame update
    void Start()
    {
        combo_event_subcription = EventBus.Subscribe<ResetComboEvent>(_OnResetCombo);
        increment_event_subscription = EventBus.Subscribe<IncrementCombo>(_OnComboUpdated);
    }

    void Update(){
        if (currentCombo == 0){
            GetComponent<Text>().text = "";
        }
    }
    void _OnResetCombo(ResetComboEvent e)
    {
        if (timer!= null){
            StopCoroutine(Timer());
        }
        GetComponent<Text>().text = "Combo x" + e.new_combo +"!";
        currentCombo = e.new_combo;
    }

    void _OnComboUpdated(IncrementCombo e){
        if (timer!= null){
            StopCoroutine(timer);
        }
        currentCombo += e.inc_amt;
        GetComponent<Text>().text = "Combo x" + currentCombo  +"!";
        timer = StartCoroutine(Timer());
    }
    public IEnumerator Timer(){
        Debug.Log("Start Timer");
        yield return new WaitForSeconds(duration);
        EventBus.Publish<ResetComboEvent>(new ResetComboEvent(0));
    }

    private void OnDestroy(){
        EventBus.Unsubscribe(combo_event_subcription);
        EventBus.Unsubscribe(increment_event_subscription);
    }
}
