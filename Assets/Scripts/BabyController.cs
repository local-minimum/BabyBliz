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
            if (state != value)
            {                                
                if (state == State.Killed)
                {
                    anim.SetTrigger("Revive");
                } else if (value == State.Playing)
                {
                    anim.SetTrigger("Sit");
                } else if (value == State.Walking)
                {
                    anim.SetTrigger("Crawl");
                } else if (value == State.Killed)
                {
                    anim.SetTrigger("Kill");
                    transform.rotation = Quaternion.Euler(0, 0, 0f);
                }
            }
			state = value;
		}
	}

    Animator anim;

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
    AudioSource audioSource;

    private void Awake()
    {
        attachmentJoint = GetComponent<SpringJoint2D>();
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !Floating && !Killed)
        {
            collision.SendMessage("PickupBaby", this, SendMessageOptions.DontRequireReceiver);
            PlaySound(jollers);
        } else if (collision.tag == "Water")
        {
            rb.gravityScale = -.1f;           
			BabyState = State.Swimming;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
			BabyState = State.Default;
            rb.gravityScale = 1f;            
        }
    }

    void PlaySound(List<AudioClip> sounds)
    {
        audioSource.clip = sounds[Random.Range(0, sounds.Count - 1)];
        audioSource.pitch = Random.value + 1f;
        audioSource.Play();
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

		BabyState = State.Default;
        rb.gravityScale = 1f;
        rb.velocity = Vector3.zero;
        anim.SetTrigger("Cling");
    }

    public void FreeAttachment()
    {
        attachmentTransform = null;
        attachmentJoint.enabled = false;
        rb.gravityScale = 1f;
		BabyState = State.DoNotMove;
        anim.SetTrigger("Sit");
    }

    Vector2 aim;
    [SerializeField]
    float minNextAction = 5f;

    [SerializeField]
    float maxNextaction = 20f;

    float nextAction;
    float xDirection = 0;

    [SerializeField]
    float crawlForce = 100f;

    [SerializeField]
    float maxCrawl = 4f;

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
                    if (Random.value < 0.25f)
                    {
                        BabyState = State.Default;
                        anim.SetTrigger("Sit");
                        xDirection = 0;
                        transform.rotation = Quaternion.identity;
                        PlaySound(laughs);
                    } else
                    {
                        BabyState = State.Walking;
                        xDirection = Random.Range(-1f, 1f);
                        xDirection = Mathf.Min(Mathf.Abs(xDirection), 0.5f) * Mathf.Sign(xDirection);
                    }
                }
                else
                {
                    rb.AddForce(new Vector2(xDirection * crawlForce, 0));
                    float x = Mathf.Min(Mathf.Abs(rb.velocity.x), maxCrawl) * Mathf.Sign(rb.velocity.x);
                    rb.velocity = new Vector2(x, rb.velocity.y);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
		if (!Killed && !Floating && state != State.Playing && collision.gameObject.tag == "Ground")
        {
            if (rb.velocity.y < -2f)
            {
                PlaySound(laughs);
            }
			BabyState = State.Default;
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
            if (state != State.Killed)
            {
                PlaySound(screams);
            }
			BabyState = State.Killed;
            health -= 0.1f;
        }

    }

    public void Kill()
    {
		AudioSource audioSource = GetComponent<AudioSource> ();
        
        Debug.Log(name + " killed");
        attachmentJoint.enabled = false;
		BabyState = State.Killed;
    }
}
