using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ComboUI : MonoBehaviour
{
    Subscription<ResetComboEvent> combo_event_subcription;
    Subscription<IncrementCombo> increment_event_subscription;
    public int currentCombo = 0;
    public float duration = 5.0f;
    public Image background;
    public Image SecondBackground;
    private Coroutine timer;
    private float progress;
    // Start is called before the first frame update
    void Start()
    {
        combo_event_subcription = EventBus.Subscribe<ResetComboEvent>(_OnResetCombo);
        increment_event_subscription = EventBus.Subscribe<IncrementCombo>(_OnComboUpdated);
        duration--;
    }

    void Update(){
        if(!PausedGameManager.is_paused) {
            if (currentCombo == 0){
                if (GetComponent<TextMeshProUGUI>() != null){
                    GetComponent<TextMeshProUGUI>().text = "";
                }
                if (background!= null){
                    SecondBackground.fillAmount = 0;
                    progress = 0;
                    background.fillAmount = 0;
                }
            }
        }
    }
    void _OnResetCombo(ResetComboEvent e)
    {
        if (timer!= null){
            StopCoroutine(timer);
        }
        if (GetComponent<TextMeshProUGUI>() != null){
            GetComponent<TextMeshProUGUI>().text = "x" + e.new_combo +"!";
        }
        if (background != null){
            SecondBackground.fillAmount = 0;
            progress = 0;
            background.fillAmount = 0;
        }
        currentCombo = e.new_combo;
    }

    void _OnComboUpdated(IncrementCombo e){
        if (timer!= null){
            StopCoroutine(timer);
        }
        currentCombo += e.inc_amt;
        if (GetComponent<TextMeshProUGUI>() != null){
            GetComponent<TextMeshProUGUI>().text = "x" + currentCombo  +"!";
        }
        if (background != null){
            SecondBackground.fillAmount = 1;
            progress = 1;
            background.fillAmount = 1;
        }
        StartCoroutine(SizeChange(1.25f, 0.25f));

    }
    IEnumerator Timer(){
        if (timer != null){
            StopCoroutine(timer);
        }
        if (background != null){
            SecondBackground.fillAmount = 1;
            float initial_time = Time.time;
            progress = 1 - (Time.time - initial_time)/duration;
            while (progress>0.0f){
                progress = 1 - (Time.time - initial_time)/duration;
                background.fillAmount = progress;
                yield return null;
            }
            background.fillAmount = 0;
            EventBus.Publish<ResetComboEvent>(new ResetComboEvent(0));
        }
    }

    IEnumerator SizeChange(float scale, float time){
        Vector3 start = new Vector3(1.0f, 1.0f, 1.0f);
        Vector3 result = new Vector3(scale, scale, 1.0f);
        if (transform.localScale != start){
            start = transform.localScale;
        }
        float init_time = Time.time;
        float prog = (Time.time - init_time) / time;
        while (prog < 1.0f){
            prog = (Time.time-init_time) / time;
            Vector3 new_scale = Vector3.Lerp(start,result,prog);
            if (GetComponent<Text>()!=null){
                transform.localScale = new_scale;
            }
            if (background != null){
                background.transform.localScale = new_scale;
                SecondBackground.transform.localScale = new_scale;
            }
            yield return null;
        }
        if (GetComponent<Text>() != null){
            transform.localScale = result;
        }
        if (background != null){
            background.transform.localScale = result;
            SecondBackground.transform.localScale = result;
        }
        start = new Vector3(1.0f, 1.0f, 1.0f);
        init_time = Time.time;
        prog = (Time.time - init_time) / time;
        while (prog < 1.0f){
            prog = (Time.time-init_time) / time;
            Vector3 new_scale = Vector3.Lerp(result,start,prog);
            if (GetComponent<Text>()!=null){
                transform.localScale = new_scale;
            }
            if (background != null){
                background.transform.localScale = new_scale;
                SecondBackground.transform.localScale = new_scale;
            }
            yield return null;
        }
        if (GetComponent<Text>() != null){
            transform.localScale = start;
        }
        if (background != null){
            background.transform.localScale = start;
            SecondBackground.transform.localScale = start;
        }
        timer = StartCoroutine(Timer());
        yield return timer;
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
        return "x" + new_combo + "!";
    }
}

public class IncrementCombo{
    public int inc_amt = 1;
    public IncrementCombo(){}
    public IncrementCombo(int _inc_amt){
        inc_amt = _inc_amt;
    }
}

