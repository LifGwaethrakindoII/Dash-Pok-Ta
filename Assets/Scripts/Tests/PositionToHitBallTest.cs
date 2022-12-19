using UnityEngine;
using System.Collections;

public class PositionToHitBallTest : MonoBehaviour
{
	public Transform goal;
	public Transform testPlayer;

	Vector3[] identities;

    public Transform testPlane;
    float planeDistance;

	// Use this for initialization
	void Start ()
	{
        planeDistance = 5f;
        this.transform.position = testPlane.position;

		identities = new Vector3[]
		{
			/*new Vector3(-1, 0, -1),
			new Vector3(1, 0, -1),
			new Vector3(1, 0, 1),
			new Vector3(-1, 0, 1)*/
			new Vector3(1, 0, 1),
			new Vector3(-1, 0, 1),
			new Vector3(-1, 0, -1),
			new Vector3(1, 0, -1)
		};
	}
	
	// Update is called once per frame
	void Update ()
	{
		//testPlayer.position = getPositionToHitTowardsTarget(this.transform, goal);
        //determinePlanePlayerIs();
        aroundThePlane();
	}

    void aroundThePlane()
    {
        float dst = getDistance();

        if(dst > planeDistance)
        {
            Vector3 vect = testPlane.transform.position - transform.position;
            vect = vect.normalized;
            vect *= (dst - planeDistance);
            //transform.position += vect;

            vect += transform.position;

            transform.position = vect;
        }
    }

    public float getDistance()
    {
        float d =  new Vector2((this.transform.position.x - testPlane.transform.position.x), this.transform.position.z - testPlane.transform.position.z).magnitude;

        return d;
    }

    public void determinePlanePlayerIs()
    {
        int layerMask = 1 << 10;

        Transform plane = null;

        Vector3 down = this.transform.TransformDirection(Vector3.down);
        RaycastHit hit;

        Debug.DrawRay(transform.position, down * 10, Color.green);
        
        if(Physics.Raycast(transform.position, down, out hit, Mathf.Infinity, layerMask))
        {
                Transform temp = hit.collider.transform;
                plane = temp;
                Debug.Log("Name of the plane: " +plane.transform.name);    
        }

        //return plane;
    }

	public float angleBetweenTarget(Transform _from, Transform _target)
    {
    	float angle = 0.0f;

    	Vector3 thisPos = _from.transform.position;
        Vector3 targetPos = _target.transform.position;
        float x = (thisPos.x - targetPos.x);
        float z = (thisPos.z - targetPos.z);

        if(thisPos.z < targetPos.z || thisPos.x < targetPos.x && thisPos.z < targetPos.z)
        {
            angle = (Mathf.Atan2(z, x) + (Mathf.PI * 2)) * Mathf.Rad2Deg;
        }
        else
        {
            angle = Mathf.Atan2(z, x) * Mathf.Rad2Deg;
        }

        return angle;
    }

    public Vector3 getPositionToHitTowardsTarget(Transform _from, Transform _target)
    {
    	Vector3 ballPos = new Vector3(_from.transform.position.x, this.transform.position.y, _from.transform.position.z);
    	float angle = angleBetweenTarget(_from, _target);

    	float tempX;
    	float tempZ;
    	Vector3 positionToHit = new Vector3(ballPos.x, ballPos.y, ballPos.z);

    	for(int i = 0; i < identities.Length; i++)
    	{
    		float minimRange = i * 90f;
    		//float dirMod = 1f;

    		if(angle >= minimRange && angle <= (minimRange + 90f)) //Checks the quadrant the angle belongs to...
    		{
    			//float quadrantVector = (identities[i].x * identities[i].z);
    			//dirMod = -1f; 

    			tempX = Mathf.Abs(Mathf.Cos(angle)) * identities[i].x * -1f;//To the contrary quadrant
    			tempZ = Mathf.Abs(Mathf.Sin(angle)) * identities[i].z * -1f;//To the contrary quadrant

    			positionToHit = new Vector3((positionToHit.x - (tempX * 3f)), positionToHit.y, (positionToHit.z - (tempZ * 3f)));

    			/*Debug.Log("Angle: " + angle);
    			Debug.Log("X: " + tempX);
    			Debug.Log("Z: " +tempZ);*/

    			return positionToHit;
    		}
    	}

    	return positionToHit;
    }
}
