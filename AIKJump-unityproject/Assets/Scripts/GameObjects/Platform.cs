using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

	public static int PLATFORM_TYPE_STATIC = 0;
	public static int PLATFORM_TYPE_MOVING = 1;
	public static int PLATFORM_STATE_NORMAL = 0;
	public static int PLATFORM_STATE_PULVERIZING = 1;

	public const int PLATFORM_MATERIAL_WOOD = 0;
	public const int PLATFORM_MATERIAL_GLASS = 1;

	public float myMinHori;
	public float myMaxHori;

	public int material;
	public int maxJumps;
	int jumps;
	
	public int state;

	public Transform mySpring;

	Animator anim;

	// Use this for initialization
	public void Start () {
		state = PLATFORM_STATE_NORMAL;
		BoxCollider2D box = GetComponent<BoxCollider2D>();
		myMinHori = GameController.worldHorizMin + box.size.x*1.5f;
		myMaxHori = GameController.worldHorizMax - box.size.x*1.5f;
		anim = GetComponent<Animator>();
		anim.SetInteger("Material", material);
	}
	
	public void pulverize() {
		state = PLATFORM_STATE_PULVERIZING;
		if(rigidbody2D)
			rigidbody2D.velocity = Vector2.zero;

		anim.SetTrigger("Shatter");
		anim.SetInteger("Material", -1);
		if(material == PLATFORM_MATERIAL_GLASS) {
			GameController.current.audioManager.GlassBreak();
		}
		if(mySpring)
			mySpring.GetComponent<Spring>().Hide();
	}

	public void Restart(int material) {
		jumps = 0;
		state = PLATFORM_STATE_NORMAL;
		this.material = material;
		anim.SetInteger("Material", material);
	}

	void Update() {
		if(mySpring && state == PLATFORM_STATE_NORMAL)
			mySpring.position = transform.position;
	}

	public void Jump() {
		jumps++;
//		Debug.Log("JUMPONGLASS: " + jumps);
		if(material != PLATFORM_MATERIAL_WOOD && jumps >= maxJumps)
			pulverize();
	}
}
