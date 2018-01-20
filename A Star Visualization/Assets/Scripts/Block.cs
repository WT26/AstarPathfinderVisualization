using UnityEngine;

public class Block : MonoBehaviour {

    public enum States { NOT_VISITED, OPEN_SET, PATH, WALL, START, END };
    public States currentState;
    private Renderer rend;

    int i;
    int j;

    // Use this for initialization
    void Start () {
        currentState = States.NOT_VISITED;
    }

   
    public void switchState(States newState)
    {
        currentState = newState;
        checkState();
    }

    public void checkState()
    {
        rend = GetComponent<Renderer>();
        Transform t = GetComponent<Transform>();
        switch (currentState)
        {
            case States.NOT_VISITED:
                rend.material.color = Color.white;
                t.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                break;
            case States.OPEN_SET:
                rend.material.color = Color.cyan;
                t.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                break;
            case States.PATH:
                rend.material.color = Color.yellow;
                t.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                break;
            case States.WALL:
                rend.material.color = Color.black;
                t.localScale = new Vector3(1f,1f, 1f);

                break;
            case States.START:
                rend.material.color = Color.green;
                t.localScale = new Vector3(1f, 1f, 1f);

                break;
            case States.END:
                rend.material.color = Color.red;
                t.localScale = new Vector3(1f, 1f, 1f);

                break;
        }
    }

    public void setCoordinates(int i, int j)
    {
        this.i = i;
        this.j = j;
    }

    public int getCoordI()
    {
        return i;
    }

    public int getCoordJ()
    {
        return j;
    }
}
