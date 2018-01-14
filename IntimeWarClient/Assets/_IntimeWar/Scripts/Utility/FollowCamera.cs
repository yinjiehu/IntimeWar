using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowCamera : MonoBehaviour {

    [SerializeField]
    SpriteRenderer _map;

    public List<Transform> Targets;

    float _speed = 3f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    Transform _target;

    Vector3 GetAveragePos()
    {
        var vectorArray = new Vector3[Targets.Count];
        Vector3 averagePos = Vector3.zero;
        for (int i = 0; i < Targets.Count; i++)
        {
            if (Targets[i] == null)
            {
                // handle in case a target was removed
                Debug.LogWarning("A Camera GameObject Target is null! Removing entry.");

                // find an alternative target
                int s = i > 0
                    ? Targets[i - 1] != null
                        ? i - 1
                        : i + 1
                    : 1;

                Debug.Log(s);

                vectorArray[s] = Targets[s].transform.position;

                averagePos += vectorArray[s];

                // remove the null target
                Targets.RemoveAt(i);
            }
            else
            {
                // business as usual
                vectorArray[i] = Targets[i].transform.position;

                averagePos += vectorArray[i];
            }
        }

        //CorrectAveragePositionByMapArea();

        return averagePos / vectorArray.Length;
    }

    void LateUpdate()
    {

        var averagePos = GetAveragePos();
        averagePos.z = -10;
        Vector3 targetPosition = Vector3.Lerp(transform.position, averagePos, Time.deltaTime * _speed);
        float width = (MapSize.x - CameraSize.x) / 2f;
        float height = (MapSize.y - CameraSize.y) / 2f;
        if (averagePos.x > -width && averagePos.x < width)
        {
            targetPosition.x = averagePos.x;
        }
        else
        {
            targetPosition.x = transform.position.x;
        }
        if (averagePos.y > -height && averagePos.y < height)
        {
            targetPosition.y = averagePos.y;
        }
        else
        {
            targetPosition.y = transform.position.y;
        }
        targetPosition.z = averagePos.z;
        transform.position = targetPosition;
    }

    Vector2 CameraSize
    {
        get
        {
            var camera = GetComponent<Camera>();
            var halfHeight = camera.orthographicSize;
            var halfWidth = halfHeight * camera.aspect;
            return new Vector2(halfWidth * 2, halfHeight * 2);
        }
    }



    public Vector2 MapSize
    {
        get
        {
            if (_map == null)
                return Vector2.zero;
            return _map.bounds.size;
        }
    }

	public bool IsContiansPoint(Vector2 point)
	{
		if (point.x > -MapSize.x && point.x < MapSize.x && point.y > -MapSize.y && point.y < MapSize.y)
			return true;
		return false;
	}

    void OnDrawGizmosSelected()
    {
        //DrawGizmos();
    }
    void DrawGizmos()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(CameraSize.x, CameraSize.y, 0f));


        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(MapSize.x, MapSize.y, 0f));

        Debug.Log("CameraSize:" + CameraSize + "    MapSize:" + MapSize);
    }
}
