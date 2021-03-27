using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboUI : MonoBehaviour
{
    Subscription<ResetComboEvent> combo_event_subcription;
    Subscription<IncrementCombo> increment_event_subscription;
    public int currentCombo = 0;
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
            if (GetComponent<Text>() != null){
                GetComponent<Text>().text = "";
            }
        }
    }
    void _OnResetCombo(ResetComboEvent e)
    {
        if (timer!= null){
            StopCoroutine(Timer());
        }
        if (GetComponent<Text>() != null){
            GetComponent<Text>().text = "Combo x" + e.new_combo +"!";
        }
        currentCombo = e.new_combo;
    }

    void _OnComboUpdated(IncrementCombo e){
        if (timer!= null){
            StopCoroutine(timer);
        }
        currentCombo += e.inc_amt;
        if (GetComponent<Text>() != null){
            GetComponent<Text>().text = "Combo x" + currentCombo  +"!";
        }
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

public class ResetComboEvent {
    public int new_combo = 0;
    public ResetComboEvent(int _new_combo){
        new_combo = _new_combo;
    }

    public override string ToString(){
        return "Combo x" + new_combo + "!";
    }
}

public class IncrementCombo{
    public int inc_amt = 1;
    public IncrementCombo(){}
    public IncrementCombo(int _inc_amt){
        inc_amt = _inc_amt;
    }
}

