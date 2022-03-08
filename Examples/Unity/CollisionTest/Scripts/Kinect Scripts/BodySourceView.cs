using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

using Windows.Kinect;
using Joint = Windows.Kinect.Joint;

public class BodySourceView : MonoBehaviour
{
    public BodySourceManager mBodySourceManager;
    public GameObject smallJoint;
    public GameObject bigJoint;

    private Dictionary<ulong, GameObject> mBodies = new Dictionary<ulong, GameObject>();
    private List<JointType> _BigJoints = new List<JointType>
    {
        JointType.ShoulderLeft,
        JointType.ShoulderRight,
    
        JointType.SpineBase,
        
        JointType.SpineMid,
        JointType.Head
    };

    private List<JointType> _SmallJoints = new List<JointType>
    {
        JointType.HandLeft,
        JointType.ElbowLeft,
    
        JointType.HandRight,
        JointType.ElbowRight,

        JointType.FootLeft,
        JointType.KneeLeft,

        JointType.FootRight,
        JointType.KneeRight,
    };

    void Update()
    {
        #region Get Kinect data
        Body[] data = mBodySourceManager.GetData();
        if (data == null)
            return;

        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data)
        {
            if (body == null)
                continue;

            if (body.IsTracked)
                trackedIds.Add(body.TrackingId);
        }
        #endregion

        #region Delete Kinect bodies
        List<ulong> knownIds = new List<ulong>(mBodies.Keys);
        foreach (ulong trackingId in knownIds)
        {
            if (!trackedIds.Contains(trackingId))
            {
                // Destroy body object
                Destroy(mBodies[trackingId]);

                // Remove from list
                mBodies.Remove(trackingId);
            }
        }
        #endregion

        #region Create Kinect bodies
        foreach (var body in data)
        {
            // If no body, skip
            if (body == null)
                continue;

            if (body.IsTracked)
            {
                // If body isn't tracked, create body
                if (!mBodies.ContainsKey(body.TrackingId))
                    mBodies[body.TrackingId] = CreateBodyObject(body.TrackingId);

                // Update positions
                UpdateBodyObject(body, mBodies[body.TrackingId]);
            }
        }
        #endregion
    }

    private GameObject CreateBodyObject(ulong id)
    {
        // Create body parent
        GameObject body = new GameObject("Body:" + id);

        // Create joints
        foreach (JointType joint in _BigJoints)
        {
            // Create Object
            GameObject newJoint = Instantiate(bigJoint);
            //GameObject newJoint = GameObject.CreatePrimitive(PrimitiveType.Cube);
            newJoint.name = joint.ToString();
            // Parent to body
            newJoint.transform.parent = body.transform;
        }
        foreach (JointType joint in _SmallJoints)
        {
            // Create Object
            GameObject newJoint = Instantiate(smallJoint);
            //GameObject newJoint = GameObject.CreatePrimitive(PrimitiveType.Cube);
            newJoint.name = joint.ToString();
            // Parent to body
            newJoint.transform.parent = body.transform;
        }

        return body;
    }

    private void UpdateBodyObject(Body body, GameObject bodyObject)
    {
        // Update joints
        foreach (JointType _joint in _BigJoints)
        {
            // Get new target position
            Joint sourceJoint = body.Joints[_joint];
            Vector3 targetPosition = GetVector3FromJoint(sourceJoint);

            // Get joint, set new position
            Transform jointObject = bodyObject.transform.Find(_joint.ToString());
            jointObject.position = targetPosition;
        }
        foreach (JointType _joint in _SmallJoints)
        {
            // Get new target position
            Joint sourceJoint = body.Joints[_joint];
            Vector3 targetPosition = GetVector3FromJoint(sourceJoint);

            // Get joint, set new position
            Transform jointObject = bodyObject.transform.Find(_joint.ToString());
            jointObject.position = targetPosition;
        }
    }

    private Vector3 GetVector3FromJoint(Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }
}
