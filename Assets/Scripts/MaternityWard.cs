using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaternityWard : MonoBehaviour {

    public List<BabyCannon> windows = new List<BabyCannon>();

    public BoxCollider2D doorCollider;

    public int mothers;

    public Mother motherPrefab;

    public List<Collider2D> carryingMothers = new List<Collider2D>();
    List<Collider2D> gestatingMothers = new List<Collider2D>();

    public Transform door;

    private void Start()
    {
        
    }

    void Update () {
        for (int i=carryingMothers.Count - 1; i>=0; i--)        
        {
            Collider2D mother = carryingMothers[i];
            if (doorCollider.bounds.Intersects(mother.bounds))
            {
                TakeUpMother(mother);
            }
        }	    
	}

    void TakeUpMother(Collider2D mother)
    {
        carryingMothers.Remove(mother);
        mother.gameObject.SetActive(false);
        gestatingMothers.Add(mother);
        RandomCanon.BirthChild();
    }

    BabyCannon RandomCanon
    {
        get
        {
            return windows[Random.Range(0, windows.Count)];
        }
    }
}
