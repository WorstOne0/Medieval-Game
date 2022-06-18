using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionTimer {
    public static FunctionTimer Create(Action action, float time) {
        GameObject gameObject = new GameObject("FunctionTimer", typeof(MonoBehaviourHook));

        FunctionTimer functionTimer = new FunctionTimer(action, time, gameObject);

        gameObject.GetComponent<MonoBehaviourHook>().onUpdate = functionTimer.Update;

        return functionTimer;
    }
    public class MonoBehaviourHook : MonoBehaviour {
        public Action onUpdate;
        void Update() {
            if (onUpdate != null) {
                onUpdate();
            }
        }
    }

    Action action;
    float time;
    GameObject gameObject;
    bool isDestroyed = false;

    FunctionTimer(Action action, float time, GameObject gameObject) {
        this.action = action;
        this.time = time;
        this.gameObject = gameObject;
    }

    public void Update() {
        if (!isDestroyed) {
            time -= Time.deltaTime;

            if (time <= 0) {
                action();
                DestroySelf();
            }
        }
    }

    void DestroySelf() {
        isDestroyed = true;
        UnityEngine.Object.Destroy(gameObject);
    }
}
