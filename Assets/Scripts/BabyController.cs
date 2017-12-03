using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyController : MonoBehaviour {

	public enum State {
		Default,
		Playing,
		DoNotMove,
		Swimming,
		Walking,
		Killed
	}
    
	State state;

	public State BabyState 
	{
		get
		{
			return state;
		}
		set 
		{
			state = value;
		}
	}




    Transform attachmentTransform;
    SpringJoint2D attachmentJoint;

    [SerializeField]
    float minDistance = 0.05f;

    [SerializeField]
    float maxDistance = 0.3f;

	[SerializeField]
	List<AudioClip> jollers; 

	[SerializeField]
	List<AudioClip> laughs; 

	[SerializeField]
	List<AudioClip> screams; 

    Rigidbody2D rb;

	public bool DontMove {
		get {
			return state == State.Killed || state == State.Playing || state == State.DoNotMove || state == State.Swimming;
		}
	}

    public bool Floating
    {
        get
        {
			return state == State.Swimming;
        }
    }

	public bool PickedUp 
	{
		get 
		{
			return attachmentTransform == null;
		}
	}

	bool doNotRemove;

	public bool DoNotRemove 
	{
		get {
			return doNotRemove && !Killed;
		}
		set {
			doNotRemove = value;
		}
	}
	 
	public bool Killed 
	{
		get {
			return state == State.Killed;
		}
	}

    SpriteRenderer rend;

    private void Awake()
    {
        attachmentJoint = GetComponent<SpringJoint2D>();
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !Floating && !Killed)
        {
            collision.SendMessage("PickupBaby", this, SendMessageOptions.DontRequireReceiver);
			AudioSource audioSource = GetComponent<AudioSource> ();
			audioSource.clip = jollers [UnityEngine.Random.Range (0, jollers.Count - 1)];
			audioSource.pitch = UnityEngine.Random.value + 1f;
			audioSource.Play ();
        } else if (collision.tag == "Water")
        {
            rb.gravityScale = -.1f;           
			state = State.Swimming;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
			state = State.Default;
            rb.gravityScale = 1f;            
        }
    }

    public void SetAttachment(GameObject go, Rect attachmentArea)
    {
        attachmentTransform = go.transform;
        attachmentJoint.connectedBody = go.GetComponent<Rigidbody2D>();
        attachmentJoint.distance = Random.Range(minDistance, maxDistance);
        attachmentJoint.enabled = true;

        attachmentJoint.connectedAnchor =
            new Vector2(
                Mathf.Lerp(attachmentArea.min.x, attachmentArea.max.x, Random.value),
                Mathf.Lerp(attachmentArea.min.y, attachmentArea.max.y, Random.value)
            );

		state = State.Default;
        rb.gravityScale = 1f;
        rb.velocity = Vector3.zero;
    }

    public void FreeAttachment()
    {
        attachmentTransform = null;
        attachmentJoint.enabled = false;
        rb.gravityScale = 1f;
		state = State.DoNotMove;
    }

    Vector2 aim;
    [SerializeField]
    float minNextAction = 5f;

    [SerializeField]
    float maxNextaction = 20f;

    float nextAction;
    float xDirection = 0;

    [SerializeField]
    float crawlSpeed;

    [SerializeField]
    CapsuleCollider2D capsulCollider;

	private System.DateTime? visibleTime;

	public System.DateTime? VisibleTime {
		get {
			return visibleTime;
		}
	}

	void OnBecameVisible() {
		visibleTime = System.DateTime.Now;
	}

	void OnBecameInvisible() {
		visibleTime = null;
	}
		
    private void Update()
    {
        if (Killed)
        {
            return;
        }

        if (attachmentTransform == null)
        {
            if (Floating)
            {
                rb.velocity *= 0.9f;
            }

			if (!DontMove)
            {

                if (nextAction < Time.timeSinceLevelLoad)
                {
                    nextAction = Random.Range(minNextAction, maxNextaction) + Time.timeSinceLevelLoad;
                    xDirection = Random.Range(-1f, 1f);
                }
                else
                {
                    rb.AddForce(new Vector2(xDirection * crawlSpeed, 0));
                    if (Random.value < 0.1f)
                        capsulCollider.size = new Vector2(Random.Range(0.7f, 0.9f), Random.Range(0.8f, 1.0f));
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
		if (!Killed && !Floating && state != State.Playing && collision.gameObject.tag == "Ground")
        {
			state = State.Default;
        }
    }

    float health = 1f;
    public float Health
    {
        get
        {
            return health;
        }

        set
        {
            health = Mathf.Max(0, value);
            rend.color = Color.Lerp(Color.white, Color.black, 1f - value);
        }
    }
		
    public void SetBurning(bool value)
    {
        if (attachmentTransform == null) {
			state = State.Killed;
            health -= 0.1f;
        }

    }

    public void Kill()
    {
		AudioSource audioSource = GetComponent<AudioSource> ();
		audioSource.clip = screams [Random.Range (0, screams.Count - 1)];
		audioSource.pitch = Random.value + 1f;
		audioSource.Play ();

        Debug.Log(name + " killed");
        attachmentJoint.enabled = false;
		state = State.Killed;
    }
}
