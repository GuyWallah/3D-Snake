using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovment : MonoBehaviour {

    //Array
    public List<Transform> BodyParts = new List<Transform>();

    public float MinDistance = 1.0f;
    public int beginsize;
    public float speed = 1.0f;
    public float rotationSpeed = 50;
    public GameObject Bodyprefab;

    private float dis;
    private Transform curBodyPart;
    private Transform PrevBodyPart;


	// Use this for initialization
	void Start ()
    {
        for(int i=0; i< beginsize-1; i++)
        {
            AddBodyPart();
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = (false);
    }
	
	// Update is called once per frame
	void Update () {

        Move();

        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = (true);
        }
        if (Input.GetKey(KeyCode.P))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = (false);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            AddBodyPart();
        }
	}

    public void Move()
    {
        float curspeed = speed;
        BodyParts[0].Translate(BodyParts[0].forward * curspeed * Time.smoothDeltaTime, Space.World);

        if (Input.GetKey(KeyCode.W))
        {
            curspeed *= 2;
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            BodyParts[0].Rotate(Vector3.up * rotationSpeed * Time.deltaTime * Input.GetAxis("Horizontal"));
        }

        for (int i = 1; i<BodyParts.Count; i++)
        {
            curBodyPart = BodyParts[i];
            PrevBodyPart = BodyParts[i - 1];

            dis = Vector3.Distance(PrevBodyPart.position, curBodyPart.position);

            Vector3 newpos = PrevBodyPart.position;

            newpos.y = BodyParts[0].position.y;

            float T = Time.deltaTime * dis / MinDistance * curspeed;

            if (T > 0.5f)
                T = 0.5f;
            curBodyPart.position = Vector3.Slerp(curBodyPart.position, newpos, T);
            curBodyPart.rotation = Quaternion.Slerp(curBodyPart.rotation, PrevBodyPart.rotation, T);
        }
    }

    public void AddBodyPart()
    {
        Transform newpart = (Instantiate(Bodyprefab, BodyParts[BodyParts.Count - 1].position, BodyParts[BodyParts.Count - 1].rotation) as GameObject).transform;

        newpart.SetParent(transform);

        BodyParts.Add(newpart);

    }
}
