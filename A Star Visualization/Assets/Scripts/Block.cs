using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    public enum States { NOT_VISITED, OPEN_SET, PATH, WALL, START, END };
    public States currentState_;
    private bool stateChangedFlag_;
    //private GameObject cube;
    private Renderer rend_;
    private float dist_;

    // Use this for initialization
    void Start () {
        currentState_ = States.NOT_VISITED;
    }


    public void switchState(States newState)
    {
        currentState_ = newState;
        checkState();
    }

    public void checkState()
    {
        rend_ = GetComponent<Renderer>();
        Transform t = GetComponent<Transform>();
        //Debug.Log(currentState_);
        switch (currentState_)
        {
            case States.NOT_VISITED:
                rend_.material.color = Color.white;
                t.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                break;
            case States.OPEN_SET:
                rend_.material.color = Color.cyan;
                t.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                break;
            case States.PATH:
                rend_.material.color = Color.yellow;
                t.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                break;
            case States.WALL:
                rend_.material.color = Color.black;
                break;
            case States.START:
                rend_.material.color = Color.green;
                break;
            case States.END:
                rend_.material.color = Color.red;
                break;
        }
    }
}
