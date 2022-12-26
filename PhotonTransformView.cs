// ----------------------------------------------------------------------------
// <copyright file="PhotonTransformView.cs" company="Exit Games GmbH">
//   PhotonNetwork Framework for Unity - Copyright (C) 2018 Exit Games GmbH
// </copyright>
// <summary>
//   Component to synchronize Transforms via PUN PhotonView.
// </summary>
// <author>developer@exitgames.com</author>
// ----------------------------------------------------------------------------


namespace Photon.Pun
{
    using Photon.Realtime;
    //using System.Numerics;
    using UnityEngine;

    [AddComponentMenu("Photon Networking/Photon Transform View")]
    [HelpURL("https://doc.photonengine.com/en-us/pun/v2/gameplay/synchronization-and-state")]
    public class PhotonTransformView : MonoBehaviourPun, IPunObservable
    {
        public float playerspeed;
        public float delta = 0F;
        float PLAYx = 0.0F;
        float CAMx = 0.0F;
        float PLAYy = 0.0F;
        float CAMy = 0.0F;
        float PLAYz = 0.0F;
        float CAMz = 0.0F;


        private float m_Distance;
        private float m_Angle;
        private Transform tempTransform;

        private Vector3 m_Direction;
        private Vector3 m_NetworkPosition;
        private Vector3 m_StoredPosition;

        private Quaternion m_NetworkRotation;
        private Quaternion lastDirection = Quaternion.identity;
        private Vector3 lastPosition = Vector3.zero;
        private float n_Angle = 0F;
        private float n_Distance = 0F;

        public bool m_SynchronizePosition = true;
        public bool m_SynchronizeRotation = true;
        public bool m_SynchronizeScale = false;

        [Tooltip("Indicates if localPosition and localRotation should be used. Scale ignores this setting, and always uses localScale to avoid issues with lossyScale.")]
        public bool m_UseLocal;

        bool m_firstTake = false;

        public void Awake()
        {
            m_StoredPosition = transform.localPosition;
            //m_NetworkPosition = Vector3.zero;
            m_NetworkPosition = new Vector3(0f,1.65F,0f);

            m_NetworkRotation = Quaternion.identity;
            tempTransform = this.transform;
            playerspeed = 3F;
            delta = 0F;

            if (!this.photonView.IsMine)
            {
            Debug.Log("Awake fucntion in PhotonTransformView is being called");
                GetComponent<Camera>().enabled = false;
                m_NetworkPosition = new Vector3(3f, 0f, 0f);
            }
        }

        private void Reset()
        {
            // Only default to true with new instances. useLocal will remain false for old projects that are updating PUN.
            m_UseLocal = true;
        }

        void OnEnable()
        {
            m_firstTake = true;
        }

        private Vector3 move(Vector3 before, float x, float y, float z)
        {
            return new Vector3(before.x+x,before.y+y,before.z+z); 
        }
        public void SetPlayerPosition(Vector3 pos)
        {
            this.GetComponent<CharacterController>().enabled = false;
            transform.position = pos;
            this.GetComponent<CharacterController>().enabled = true;
        }
        public void Update()
        {
            var tr = transform;
            
            if (!this.photonView.IsMine)
            {
                if (m_UseLocal)
                {
                    tr.localPosition = Vector3.MoveTowards(tr.localPosition, this.m_NetworkPosition, this.m_Distance  * Time.deltaTime * PhotonNetwork.SerializationRate);
                    tr.localRotation = Quaternion.RotateTowards(tr.localRotation, this.m_NetworkRotation, this.m_Angle * Time.deltaTime * PhotonNetwork.SerializationRate);
                }
                else
                {
                    tr.position = Vector3.MoveTowards(tr.position, this.m_NetworkPosition, this.m_Distance * Time.deltaTime * PhotonNetwork.SerializationRate);
                    tr.rotation = Quaternion.RotateTowards(tr.rotation, this.m_NetworkRotation, this.m_Angle * Time.deltaTime *  PhotonNetwork.SerializationRate);
                }
            }//should have 'else' here to deal with own movement
            else {
                bool tap = false;
                if (Input.touchSupported) { if (Input.touchCount > 0) { tap = true; } }
                DeviceOrientation orient = Input.deviceOrientation;
                int ocompare = orient.CompareTo(orient);
                Vector3 velocity = Input.acceleration;
                velocity.y = 0;
                Compass ompass = Input.compass;
                Vector3 direct = ompass.rawVector;

                //Quaternion qdirect = (Quaternion)direct;
                if (Input.GetButton("Fire1") || tap)
                {
                    n_Angle = Vector3.Angle(Camera.main.transform.position, this.GetComponent<Transform>().position);
                    n_Distance = Vector3.Distance(Camera.main.transform.position, this.GetComponent<Transform>().position);
                    
                    this.GetComponent<CharacterController>().enabled = false;
                    //float a = Camera.main.velocity.normalized.x;
                    //float b = Camera.main.velocity.normalized.z;
                    //Vector3 pos = new Vector3(a, 1.65f, b);
                    //Debug.Log("x: "+a.ToString()+", z: " +b.ToString()+ ", pos: "+pos.ToString());
                    //tr.position =  pos;
                    tr.position = tr.position + Camera.main.transform.forward * playerspeed * Time.deltaTime;
                    this.GetComponent<CharacterController>().enabled = true;

                    tr.rotation = Quaternion.RotateTowards(tr.rotation, this.GetComponent<Transform>().rotation, this.n_Angle * Time.deltaTime * playerspeed);
                }// this is to use cardboard and save tap input for other things
                else if (Input.acceleration.magnitude > 0.05f)
                {
                    n_Angle = Vector3.Angle(Camera.main.transform.position, this.GetComponent<Transform>().position);
                    n_Distance = Vector3.Distance(Camera.main.transform.position, this.GetComponent<Transform>().position);

                    if(Input.acceleration.magnitude > 1.0f)
                    {
                        playerspeed = Input.acceleration.sqrMagnitude;
                    }
                    else
                    {
                        playerspeed = Input.acceleration.magnitude;
                    }
                    direct = Input.compass.rawVector;
                    direct.y = 0;
                    this.GetComponent<CharacterController>().enabled = false;

                    //tr.position = this.GetComponent<Transform>().position + direct * playerspeed * Time.deltaTime*100;
                    tr.position = this.GetComponent<Transform>().position + Camera.main.transform.forward * playerspeed * Time.deltaTime;

                    this.GetComponent<CharacterController>().enabled = true;

                    tr.rotation = Quaternion.RotateTowards(tr.rotation, this.GetComponent<Transform>().rotation, this.n_Angle * Time.deltaTime * playerspeed);
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            var tr = transform;

            // Write
            if (stream.IsWriting)
            {
                if (this.m_SynchronizePosition)
                {
                    if (m_UseLocal)
                    {
                        this.m_Direction = tr.localPosition - this.m_StoredPosition;
                        this.m_StoredPosition = tr.localPosition;
                        stream.SendNext(tr.localPosition);
                        stream.SendNext(this.m_Direction);
                    }
                    else
                    {
                        this.m_Direction = tr.position - this.m_StoredPosition;
                        this.m_StoredPosition = tr.position;
                        stream.SendNext(tr.position);
                        stream.SendNext(this.m_Direction);
                    }
                }

                if (this.m_SynchronizeRotation)
                {
                    if (m_UseLocal)
                    {
                        stream.SendNext(tr.localRotation);
                    }
                    else
                    {
                        stream.SendNext(tr.rotation);
                    }
                }

                if (this.m_SynchronizeScale)
                {
                    stream.SendNext(tr.localScale);
                }
            }
            // Read
            else
            {
                if (this.m_SynchronizePosition)
                {
                    this.m_NetworkPosition = (Vector3)stream.ReceiveNext();
                    this.m_Direction = (Vector3)stream.ReceiveNext();

                    if (m_firstTake)
                    {
                        if (m_UseLocal)
                            tr.localPosition = this.m_NetworkPosition;
                        else
                            tr.position = this.m_NetworkPosition;

                        this.m_Distance = 0f;
                    }
                    else
                    {
                        float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                        this.m_NetworkPosition += this.m_Direction * lag;
                        if (m_UseLocal)
                        {
                            this.m_Distance = Vector3.Distance(tr.localPosition, this.m_NetworkPosition);
                        }
                        else
                        {
                            this.m_Distance = Vector3.Distance(tr.position, this.m_NetworkPosition);
                        }
                    }

                }

                if (this.m_SynchronizeRotation)
                {
                    this.m_NetworkRotation = (Quaternion)stream.ReceiveNext();

                    if (m_firstTake)
                    {
                        this.m_Angle = 0f;

                        if (m_UseLocal)
                        {
                            tr.localRotation = this.m_NetworkRotation;
                        }
                        else
                        {
                            tr.rotation = this.m_NetworkRotation;
                        }
                    }
                    else
                    {
                        if (m_UseLocal)
                        {
                            this.m_Angle = Quaternion.Angle(tr.localRotation, this.m_NetworkRotation);
                        }
                        else
                        {
                            this.m_Angle = Quaternion.Angle(tr.rotation, this.m_NetworkRotation);
                        }
                    }
                }

                if (this.m_SynchronizeScale)
                {
                    tr.localScale = (Vector3)stream.ReceiveNext();
                }

                if (m_firstTake)
                {
                    m_firstTake = false;
                }
            }
        }
    }
}