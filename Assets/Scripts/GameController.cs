using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject TrainPrefab;

    [Range(0.0005f, 1.0f)]
    public float TrainSpeed = 0.5f;

    private GameObject mTrain;
    private GameObject mWaypointPlayerStart;
    private GameObject mDestinationWaypoint;
    private GameObject mPreviousWaypoint;

    private bool mIsMoving = false;

    void Start()
    {
        mWaypointPlayerStart = GameObject.FindGameObjectWithTag( "PlayerStartWaypoint" );
        mTrain = Instantiate( TrainPrefab, mWaypointPlayerStart.transform.position, Quaternion.identity );
    }


    void Update()
    {
        if( !mIsMoving ) {
            MoveToNext();
        }

    }

    private void MoveToNext()
    {
        if( !mIsMoving ) {
            mDestinationWaypoint = FindNextWaypoint();
            if( mDestinationWaypoint ) {
                StartCoroutine( DoMove() );
            }
            else {
                Debug.Log( "Can't find another waypoint." );
            }
        }
    }

    private IEnumerator DoMove()
    {
        Debug.Log( "moving" );
        mIsMoving = true;

        Vector3 start_pos = mTrain.transform.position;
        //Vector3 dest_pos = ;
        float c = 0;
        while( c < 1 ) {
            mTrain.transform.position = Vector3.Lerp( start_pos, mDestinationWaypoint.transform.position, c );
            c += Time.deltaTime * TrainSpeed;
            Debug.Log( c );
            yield return null;
        }
        Debug.Log( "Not moving" );

        mIsMoving = false;
        yield return null;
    }

    private GameObject FindNextWaypoint()
    {
        Collider[] hitColliders = Physics.OverlapSphere(mTrain.transform.position,6.0f);

        foreach( var collider in hitColliders ) {
            if( collider.tag == "Waypoint" && ( collider.gameObject != mDestinationWaypoint ) && ( collider.gameObject != mPreviousWaypoint ) ) {
                mPreviousWaypoint = mDestinationWaypoint;
                return collider.gameObject;
            }
        }

        return null;
    }
}
