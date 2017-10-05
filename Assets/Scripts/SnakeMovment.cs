using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovment : MonoBehaviour {

    //Array
    public List<Rigidbody> BodyParts = new List<Rigidbody>();

    public float MinDistance = 1.0f;
    public int beginsize;
    public float speed = 5.0f;
    public float rotationSpeed = 50;
    public GameObject Bodyprefab;

    private float dis;
    private Rigidbody curBodyPart;
    private Rigidbody PrevBodyPart;


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

        //Get Cursor Back
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = (true);
        }

        //Remove Cursor
        if (Input.GetKey(KeyCode.P))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = (false);
        }

        //replace cursor
        if (Input.GetKey(KeyCode.Q))
        {
            AddBodyPart();
        }
	}

    public void Move()
    {
        //Always move forward
        float curspeed = speed;
        BodyParts[0].GetComponent<Rigidbody>().AddForce(BodyParts[0].transform.forward * curspeed * Time.smoothDeltaTime);

        //Double Speed
        if (Input.GetKey(KeyCode.W))
        {
            curspeed *= 2;
        }

        //Move Left and Right
        Vector3 dir=Vector3.Cross  (BodyParts[0].GetComponent<Rigidbody>().transform.forward, Vector3.up);

        if (Input.GetAxis("Horizontal") != 0)
        {
            BodyParts[0].GetComponent<Rigidbody>().AddForce(dir * rotationSpeed * Time.deltaTime * Input.GetAxis("Horizontal"));
        }

        //Move like a Snake and Keep min Distance
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
        Rigidbody newpart = (Instantiate(Bodyprefab.GetComponent<Rigidbody>(), BodyParts[BodyParts.Count - 1].position, BodyParts[BodyParts.Count - 1].rotation));

        BodyParts.Add(newpart);

    }
}
