using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform m_transform;
    public int m_life = 5;

    public CharacterController m_ch;
    public float m_movSpeed = 3.0f;
    public float m_gravity = 2.0f;

    //camera
    public Transform m_camTransform;
    public Vector3 m_camRot;
    public float m_canHeight = 1.4f;

    Transform m_muzzlepoint;
    public LayerMask m_layer;
    public Transform m_fx;
    public AudioClip m_audio;
    float m_shootTimer = 0;

    public Enemy enemy;
    public bool is_attacked = false;

    // Start is called before the first frame update
    void Start()
    {
        m_transform = this.transform;
        m_ch = this.GetComponent<CharacterController>();
        m_camTransform = Camera.main.transform;
        m_camTransform.position = m_transform.TransformPoint(0, m_canHeight, 0);
        //hide mouse
        Cursor.lockState = CursorLockMode.Locked;

        m_muzzlepoint = m_camTransform.Find("M16/weapon/muzzlepoint").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("attack Single is" + is_attacked);

        if (m_life <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        Control();
        cameraCtrl();
        shoot();
    }

    private void Control()
    {
        Vector3 motion = Vector3.zero;
        motion.x = Input.GetAxis("Horizontal") * m_movSpeed * Time.deltaTime;
        motion.z = Input.GetAxis("Vertical") * m_movSpeed * Time.deltaTime;
        motion.y -= m_gravity * Time.deltaTime;

        m_ch.Move(m_transform.TransformDirection(motion));
    }

    private void cameraCtrl()
    {
        // mouse move
        float rh = Input.GetAxis("Mouse X");
        float rv = Input.GetAxis("Mouse Y");

        //mouse rotation
        m_camRot.x -= rv;
        m_camRot.y += rh;
        m_camTransform.eulerAngles = m_camRot;

        //sync player and camera
        Vector3 camrot = m_camTransform.eulerAngles;
        camrot.x = 0;
        camrot.z = 0;
        m_transform.eulerAngles = camrot;
        m_camTransform.position = m_transform.TransformPoint(0,m_canHeight,0);

    }
    
    private void shoot()
    {
        m_shootTimer -= Time.deltaTime;

        if (Input.GetMouseButton(0) && m_shootTimer <= 0)
        {
            m_shootTimer = 0.1f;
            this.GetComponent<AudioSource>().PlayOneShot(m_audio);

            GameManager.Instance.SetAmmo(1);

            RaycastHit info;


            bool hit = Physics.Raycast(m_muzzlepoint.position, m_camTransform.TransformDirection(Vector3.forward), out info, 100, m_layer);
            if (hit)
            {
                
                if (info.transform.tag == "enemy")
                {
                    Enemy enemy = info.transform.GetComponent<Enemy>();
                    enemy.OnDamage(1);
                }
                Instantiate(m_fx, info.point, info.transform.rotation);
            }

        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(this.transform.position, "Spawn.tif");
    }

    public void OnDamage(int damage)
    {
        m_life -= damage;
        GameManager.Instance.SetLife(m_life);
    }

}
