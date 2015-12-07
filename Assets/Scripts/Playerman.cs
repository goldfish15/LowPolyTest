using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Playerman : MonoBehaviour {

    public int Health = 100;
    public int Wood = 0;
    public int Stone = 0;
    public int Iron = 0;
    int resourceTextTimer = 0;
    public float BuildDistance = 5f;
    public int ParticleDeathTimer = 2;
    Animator animator;
    List<GameObject> collidedList = new List<GameObject>();
    public GameObject ExplosionMan;
    public GameObject House;
    public GameObject HouseTemplate;
    public GameObject Tower;
    public Text WoodText;
    public Text StoneText;
    public Text IronText;
    public Text ResourceText;

    private RaycastHit hit;
    private GameObject placingBuilding;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        ResourceText.enabled = false;
        ResourceText.text = "";
	}

    void FixedUpdate()
    {
        if (Health <= 0)
            animator.SetBool("Dead", true);
        else
            animator.SetBool("Dead", false);
        WoodText.text = "Wood: " + Wood;
        StoneText.text = "Stone: " + Stone;
        IronText.text = "Iron: " + Iron;
    }

    void Update() {
        if (Input.GetMouseButtonDown(1))
        {
            if (Wood == 2)
                Build("House");
            else if (Wood >= 3)
                Build("Tower");
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            ShowMessage("Wood +1", 1);
            Wood++;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ShowMessage("Stone +1", 1);
            Stone++;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ShowMessage("Iron +1", 1);
            Iron++;
        }

        if (resourceTextTimer > 0)
            resourceTextTimer--;
        else
            ResourceText.enabled = false;

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 30f))
            {
                Debug.Log("Hit: " + ray);

                if (hit.collider.gameObject.tag == "Ground")
                {
                float groundHeight = hit.point.y;// terrain.SampleHeight(hit.point);
                Debug.Log("Groundheight: " + groundHeight);

                //if (hit.point.y - .2f > groundHeight)
                //{
                GameObject go = Instantiate<GameObject>(HouseTemplate);

                go.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                placingBuilding = go;
                }
            }
        }

        if (placingBuilding != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.tag == "Ground")
                    placingBuilding.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }
        }
    }

    void ShowMessage(string message, float delay)
    {
        resourceTextTimer = 100;
        ResourceText.text = message;
        ResourceText.enabled = true;
    }

    public void LoopAttack(float num)
    {
        Debug.Log("LoopAttack");
    }

    public void Hit(float num)
    {
        var toDelete = new List<GameObject>(collidedList);
        foreach (GameObject item in toDelete)
        {
            GameObject tempExplosion = (GameObject)Instantiate(ExplosionMan, item.transform.position, item.transform.rotation);
            if (item.tag == "Tree")
            {
                ShowMessage("Wood +1", 1);
                Wood++;

                Stats stats = item.GetComponent<Stats>();
                stats.Health--;
                item.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                if (stats.Health == 0)
                {
                    Destroy(item);
                    collidedList.Remove(item);
                }
            }
            else if (item.tag == "Stone")
            {
                ShowMessage("Stone +1", 1);
                if (Random.Range(0, 10) > 7)
                {
                    ShowMessage("WOWWEEEE IRON +1", 1);
                    Iron++;
                }
                else
                    Stone++;

                Stats stats = item.GetComponent<Stats>();
                stats.Health--;
                item.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                if (stats.Health == 0)
                {
                    Destroy(item);
                    collidedList.Remove(item);
                }
            }
            else
            {
                Destroy(item);
                collidedList.Remove(item);
            }
            Destroy(tempExplosion, ParticleDeathTimer);
        }
    }

    void Build(string Building)
    {
        switch (Building)
        {
            case "House":
                Instantiate(House, transform.position + (transform.forward * BuildDistance), Quaternion.identity);
                Wood -= 2;
                break;
            case "Tower":
                Instantiate(Tower, transform.position + (transform.forward * BuildDistance), Quaternion.identity);
                Wood -= 3;
                break;
            default:
                break;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        collidedList.Add(collider.gameObject);
    }

    void OnTriggerExit(Collider collider)
    {
        collidedList.Remove(collider.gameObject);
    }
}
