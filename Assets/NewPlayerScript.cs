using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class NewPlayerScript : NetworkBehaviour {

    [Serializable]
    public class AdvancedSettings {
        public float groundCheckDistance = 0.01f; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )
        public float stickToGroundHelperDistance = 0.5f; // stops the character
        public float slowDownRate = 20f; // rate at which the controller comes to a stop when there is no input
        public bool airControl; // can the user control the direction that is being moved in the air
        [Tooltip("set it to 0.1 or more if you get stuck in wall")]
        public float shellOffset; //reduce the radius by that ratio to avoid getting stuck in wall (a value of 0.1f is nice)
    }

    [Serializable]
    public class InteractableDoor {

    }

    public Texture crosshair;

    public Texture[] perkTextures;

    public GameObject[] weapons = new GameObject[3];

    [SyncVar]
    public int points = 500;

    //Sync this eventually, not needed at the moment.
    public bool[] perks = new bool[] { false, false, false, false };

    public Camera cam;
    public AdvancedSettings advancedSettings = new AdvancedSettings();

    private Animator anim;
    private Rigidbody m_RigidBody;
    private CapsuleCollider m_Capsule;
    private float m_YRotation;
    private Vector3 m_GroundContactNormal;
    public bool m_Jump, m_PreviouslyGrounded, m_Jumping, m_IsGrounded;

    private void Start() {
        anim = GetComponent<Animator>();
        //GameObject.Find("Engine").SendMessage("setPoints", points);
        m_RigidBody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        gameObject.name = "Player_" + GetComponent<NetworkIdentity>().netId;
        if (!isLocalPlayer) {
            GameObject.Find(gameObject.name + "/HeadSlot").GetComponent<Camera>().enabled = false;
        }

        weapons[0] = Instantiate((GameObject) Resources.Load("Prefabs/Weapons/M1911", typeof(GameObject)));
        weapons[0].SetActive(false);
        selectWeapon(0);
    }

    private void OnGUI() {
        //GUI.Box(new Rect((Screen.width) - 105, (Screen.height) - 90, 80, 25), "Points: " + points);
        GUI.DrawTexture(new Rect((Screen.width / 2) - 12, (Screen.height / 2) - 12, 24, 24), crosshair);
        for (int i = 0; i < 4; i++) {
            if (perks[i]) {
                GUI.DrawTexture(new Rect(5 + (i * 44), Screen.height - 45, 40, 40), perkTextures[i]);
            }
        }
    }

    private void Update() {
        RotateView();

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            selectWeapon(0);
        }
        if (CrossPlatformInputManager.GetButtonDown("Jump") && !m_Jump) {
            m_Jump = true;
        }
    }

    private void selectWeapon(int index) {
        weapons[index].transform.SetParent(GameObject.Find(gameObject.name + "/HeadSlot/HeldWeapon/FPLocation").transform, false);
        weapons[index].SetActive(true);
        anim.Play("EquipWeap", -1, 0f);
    }
    private void FixedUpdate() {
        GroundCheck();
        if (isLocalPlayer) {
            Vector2 input = GetInput();

            if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && (advancedSettings.airControl || m_IsGrounded)) {
                // always move along the camera forward as it is the direction that it being aimed at
                Vector3 desiredMove = cam.transform.forward * input.y + cam.transform.right * input.x;
                desiredMove = Vector3.ProjectOnPlane(desiredMove, m_GroundContactNormal).normalized;

                desiredMove.x = desiredMove.x * 4;
                desiredMove.z = desiredMove.z * 4;
                desiredMove.y = desiredMove.y * 4;
                if (m_RigidBody.velocity.sqrMagnitude < (4 * 4)) {
                    if (m_IsGrounded) {
                        m_RigidBody.AddForce(desiredMove * SlopeMultiplier(), ForceMode.Impulse);
                    } else {
                        m_RigidBody.AddForce(desiredMove * 50, ForceMode.Acceleration);
                    }
                }
            }

            if (m_IsGrounded) {
                m_RigidBody.drag = 10f;

                if (m_Jump) {
                    m_RigidBody.drag = 0f;
                    m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0f, m_RigidBody.velocity.z);
                    m_RigidBody.AddForce(new Vector3(0f, 40f, 0f), ForceMode.Impulse);
                    m_Jumping = true;
                }

                if (!m_Jumping && Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && m_RigidBody.velocity.magnitude < 1f) {
                    m_RigidBody.Sleep();
                }
            } else {
                m_RigidBody.drag = 0f;
                if (m_PreviouslyGrounded && !m_Jumping) {
                    StickToGroundHelper();
                }
            }
            m_Jump = false;
        }
    }

    public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));

    private float SlopeMultiplier() {
        float angle = Vector3.Angle(m_GroundContactNormal, Vector3.up);
        if(!m_IsGrounded) {
            return 1;
        }
        return SlopeCurveModifier.Evaluate(angle);
    }


    private void StickToGroundHelper() {
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                               ((m_Capsule.height / 2f) - m_Capsule.radius) +
                               advancedSettings.stickToGroundHelperDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore)) {
            if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f) {
                m_RigidBody.velocity = Vector3.ProjectOnPlane(m_RigidBody.velocity, hitInfo.normal);
            }
        }
    }


    private Vector2 GetInput() {

        Vector2 input = new Vector2 {
            x = CrossPlatformInputManager.GetAxis("Horizontal"),
            y = CrossPlatformInputManager.GetAxis("Vertical")
        };
        return input;
    }


    private void RotateView() {
        //avoids the mouse looking if the game is effectively paused
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        // get the rotation before it's changed
        float oldYRotation = transform.eulerAngles.y;

        if (m_IsGrounded || advancedSettings.airControl) {
            // Rotate the rigidbody velocity to match the new direction that the character is looking
            Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
            m_RigidBody.velocity = velRotation * m_RigidBody.velocity;
        }
    }

    /// sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
    private void GroundCheck() {
        m_PreviouslyGrounded = m_IsGrounded;
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                               ((m_Capsule.height / 2f) - m_Capsule.radius) + advancedSettings.groundCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore)) {
            m_IsGrounded = true;
            m_GroundContactNormal = hitInfo.normal;
        } else {
            m_IsGrounded = false;
            m_GroundContactNormal = Vector3.up;
        }
        if (!m_PreviouslyGrounded && m_IsGrounded && m_Jumping) {
            m_Jumping = false;
        }
    }
}
